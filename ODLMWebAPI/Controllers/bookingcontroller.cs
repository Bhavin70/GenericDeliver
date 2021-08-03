using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.DashboardModels;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class BookingController : Controller
    {

        #region Declaration

        private readonly ILogger loggerObj;
        private readonly ITblBookingActionsBL _iTblBookingActionsBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblBookingsBL _iTblBookingsBL;
        private readonly ITblBookingBeyondQuotaBL _iTblBookingBeyondQuotaBL;
        private readonly ITblBookingDelAddrBL _iTblBookingDelAddrBL;
        private readonly ITblBookingExtBL _iTblBookingExtBL;
        private readonly ITblBookingOpngBalBL _iTblBookingOpngBalBL;
        private readonly ITblBookingScheduleBL _iTblBookingScheduleBL;
        private readonly ICommon _iCommon;
        private readonly ICircularDependencyBL _iCircularDependencyBL;
        private readonly ITblPaymentTermsForBookingBL _iTblPaymentTermsForBookingBL;
        private readonly ITblBookingQtyConsumptionBL _iTblBookingQtyConsumptionBL;
        private readonly ITblReportsBL _iTblReportsBL;
        #endregion

        #region Constructor

        public BookingController(ITblPaymentTermsForBookingBL iTblPaymentTermsForBookingBL, ITblBookingScheduleBL iTblBookingScheduleBL, ITblBookingOpngBalBL iTblBookingOpngBalBL, ITblBookingExtBL iTblBookingExtBL, ITblBookingDelAddrBL iTblBookingDelAddrBL, ITblBookingBeyondQuotaBL iTblBookingBeyondQuotaBL, ICircularDependencyBL iCircularDependencyBL, ITblBookingsBL iTblBookingsBL, ICommon iCommon, ILogger<BookingController> logger, ITblBookingActionsBL iTblBookingActionsBL, ITblConfigParamsBL iTblConfigParamsBL, ITblBookingQtyConsumptionBL iTblBookingQtyConsumptionBL, ITblReportsBL iTblReportsBL)
        {
            loggerObj = logger;
            _iTblBookingActionsBL = iTblBookingActionsBL; 
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblBookingsBL = iTblBookingsBL;
            _iTblBookingBeyondQuotaBL = iTblBookingBeyondQuotaBL;
            _iTblBookingDelAddrBL = iTblBookingDelAddrBL;
            _iTblBookingExtBL = iTblBookingExtBL;
            _iTblBookingOpngBalBL = iTblBookingOpngBalBL;
            _iTblBookingScheduleBL = iTblBookingScheduleBL;
            _iCommon = iCommon;
            _iCircularDependencyBL = iCircularDependencyBL;
            _iTblPaymentTermsForBookingBL = iTblPaymentTermsForBookingBL;
            _iTblBookingQtyConsumptionBL = iTblBookingQtyConsumptionBL;
            Constants.LoggerObj = logger;
            _iTblReportsBL = iTblReportsBL;
        }

        #endregion

        #region Get


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetBookinOpenCloseInfo")]
        [HttpGet]
        public TblBookingActionsTO GetBookinOpenCloseInfo()
        {
            return _iTblBookingActionsBL.SelectLatestBookingActionTO();
        }
        [Route("GetBookingOpenCloseForTheDay")]
        [HttpGet]
        public bool GetBookingOpenCloseForTheDay()
        {
            bool allowBooking = true;
            TblConfigParamsTO obj = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_BOOKING_END_TIME);


            //Double a = 0;

            if (obj == null)
            {
                return allowBooking;
            }

            //Double.TryParse(obj.ConfigParamVal, out a);

            if (String.IsNullOrEmpty(obj.ConfigParamVal))
            {
                return allowBooking;
            }


            if (obj.ConfigParamVal == "0" || obj.ConfigParamVal == "00.00")
            {
                return allowBooking;
            }
            else
            {

                DateTime serverDateTime = _iCommon.ServerDateTime;
                //var servertime = Convert.ToDouble(_iCommon.ServerDateTime.ToString("HH:mm"));

                double serveHour = serverDateTime.Hour;
                double ServerMinute = serverDateTime.Minute;


                List<String> list = obj.ConfigParamVal.Split(':').ToList();

                if (list != null && list.Count == 2)
                {
                    Double closureHour = Convert.ToDouble(list[0]);
                    Double closureMin = Convert.ToDouble(list[1]);


                    if (serveHour >= closureHour)
                    {

                        if (serveHour > closureHour)
                            return false;
                        if (ServerMinute > closureMin)
                            return false;
                    }
                }
                //if (Convert.ToDouble(obj.ConfigParamVal) < servertime)
                //    allowBooking = false;
            }
            
            return allowBooking;
        }

        [Route("GetBookingDetails")]
        [HttpGet]
        public TblBookingsTO GetBookingDetails(int bookingId)
        {
            return _iCircularDependencyBL.SelectBookingsTOWithDetails(bookingId);
        }

        [Route("GetBookingAvgQtyDetailsStatus")]
        [HttpGet]
        public ResultMessage GetBookingAvgQtyDetailsStatus(int dealerOrgId, Int32 bookingId)
        {
            return _iTblBookingsBL.GetBookingAvgQtyDetailsStatus(dealerOrgId, bookingId);
        }

        [Route("GetBookingStatusHistory")]
        [HttpGet]
        public List<TblBookingBeyondQuotaTO> GetBookingStatusHistory(int bookingId)
        {
            List<TblBookingBeyondQuotaTO> list = _iTblBookingBeyondQuotaBL.SelectAllStatusHistoryOfBooking(bookingId);
            if (list != null)
            {
                List<TblBookingBeyondQuotaTO> finalList = new List<TblBookingBeyondQuotaTO>();
                var statusIds = list.GroupBy(g => g.StatusId).ToList();
                for (int i = 0; i < statusIds.Count; i++)
                {
                    //This call has beeb used for Approval Qty and Rate is taken from History table
                   // var latestObj = list.Where(l => l.StatusId == statusIds[i].Key).OrderBy(s => s.StatusDate).FirstOrDefault();  //Saket [2020-11-05] Reverse code of pandurang for SRJ as we need to show first record of the status.
                    var latestObj = list.Where(l => l.StatusId == statusIds[i].Key).OrderBy(s => s.StatusDate).LastOrDefault(); //Pandurang[2018-07-17] For Finance Approval get latest entry.
                    finalList.Add(latestObj);
                }

                finalList = finalList.OrderByDescending(s => s.StatusDate).ToList();
                return finalList;
            }

            return null;
        }

        [Route("GetBookingAddressDetails")]
        [HttpGet]
        public List<TblBookingDelAddrTO> GetBookingAddressDetails(Int32 bookingId)
        {
            List<TblBookingDelAddrTO> list = null;
            list = _iTblBookingDelAddrBL.SelectAllTblBookingDelAddrList(bookingId);
            //Sanjay [2017-07-06] Commented as functionality changed. Now address will be from dealer,Cnf or New 
            //Chages shifted to LoadSlip controller
            //if (list == null || list.Count==0)
            //{
            //    list = BL._iTblBookingDelAddrBL.SelectDeliveryAddrListFromDealer(bookingId);
            //}
            return list;
        }


        [Route("GetDealerBookingHistory")]
        [HttpGet]
        public List<TblBookingsTO> GetDealerBookingHistory(Int32 dealerId , Int32 lastNRecords=4, Int32 bookingId = 0)
       {
            return _iTblBookingsBL.SelectAllLatestBookingOfDealer(dealerId, lastNRecords, bookingId);
        }


        [Route("GetDealerBookingDtlList")]
        [HttpGet]
        public List<TblBookingExtTO> GetDealerBookingDtlList(Int32 dealerId)
        {
            return _iTblBookingExtBL.SelectAllBookingDetailsWrtDealer(dealerId);
        }

        [Route("GetAllBookingList")]
        [HttpGet]
        public List<TblBookingsTO> GetAllBookingList(Int32 cnfId, Int32 dealerId,Int32 statusId,string fromDate,string toDate, String userRoleTOList, Int32 isConfirm = -1, Int32 isPendingQty = 0,Int32 bookingId = 0, Int32 isViewAllPendingEnq=0, Int32 RMId = 0,int orderTypeId=0)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

      
            List<TblBookingsTO> tblBookingsTOList = _iTblBookingsBL.SelectBookingList(cnfId, dealerId, statusId, frmDt, toDt, tblUserRoleTOList, isConfirm, isPendingQty, bookingId, isViewAllPendingEnq, RMId, orderTypeId);

            _iTblBookingsBL.AssignOverDueAmount(tblBookingsTOList);

            return tblBookingsTOList;

        }
        //Aniket [16-Jan-2019] added to view cnFList against confirm and not confirmbooking
        [Route("CnFConfirmNotConfirmBookingReport")]
        [HttpGet]
        public List<CnFWiseReportTO> CnFConfirmNotConfirmBookingReport(string fromDate,string toDate)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;

            return _iTblBookingsBL.SelectCnfCNCBookingReport(frmDt, toDt);
        }
        // Aniket [19-03-2019] added to get pending booking qty
        [HttpGet]
        [Route("BookingPendingQtyRpt")]
        public List<TblBookingPendingRptTO> BookingPendingQtyRpt(string fromDate, string toDate,int reportType)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;
            return _iTblBookingsBL.SelectBookingPendingQryRpt(frmDt, toDt,reportType);
        }

        //Aniket [11-6-2019]
        [Route("SelectAllTblBookingExtListByBookingId")]
        [HttpGet]
       public List<TblBookingExtTO> SelectAllTblBookingExtListByBookingId(Int32 bookingId,Int32 brandId,Int32 prodCatId,Int32 stateId)
        {
           return  _iTblBookingExtBL.SelectAllTblBookingExtList(bookingId,brandId,prodCatId,stateId);
        }
        /// <summary>
        /// Priyanka [14-03-2018] : Added to get the view bookings in Booking Summary Report
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="masterId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("GetAllBookingSummaryList")]
        [HttpGet]
        public List<TblBookingSummaryTO> GetAllBookingSummaryList(Int32 typeId, Int32 masterId, string fromDate, string toDate, String userRoleTOList, Int32 cnfId)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

            return _iTblBookingsBL.SelectBookingSummaryList(typeId, masterId, frmDt, toDt, tblUserRoleTOList, cnfId);
        }

        /// <summary>
        /// Priyanka [02-03-2018] : Added to get the userwise and status wise booking in view booking.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="statusId"></param>
        /// <param name="activeUserId"></param>
        /// <returns></returns>

        [Route("GetAllUserwiseBooking")]
        [HttpGet]
        public List<TblBookingsTO> GetAllUserwiseBooking( string fromDate, string toDate, Int32 statusId, Int32 activeUserId)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;


            return _iTblBookingsBL.SelectUserwiseBookingList( frmDt, toDt, statusId, activeUserId);
        }
        [Route("GetPendingBookingList")]
        [HttpGet]
        public List<TblBookingsTO> GetPendingBookingList(Int32 cnfId, Int32 dealerId, String userRoleTOList)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return _iTblBookingsBL.SelectAllBookingList(cnfId, dealerId, tblUserRoleTOList);
        }

        /// <summary>
        /// Priyanka [11-06-2018] : Added to get order wise dealer list for SHIVANGI.
        /// </summary>
        /// <returns></returns>
        [Route("GetOrderwiseDealerList")]
        [HttpGet]
        public List<TblBookingsTO> GetOrderwiseDealerList()
        {
            //TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
            return _iTblBookingsBL.GetOrderwiseDealerList();
        }

        /// <summary>
        /// Priyanka [11-06-2018] : Added for post the overdue status from booking for SHIVANGI.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostUpdateOverdueExistOrNotFromBooking")]
        [HttpPost]
        public ResultMessage PostUpdateOverdueExistOrNotFromBooking([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblBookingsTO bookingTo = JsonConvert.DeserializeObject<TblBookingsTO>(data["bookingTo"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                }

                if (bookingTo != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    bookingTo.UpdatedBy = Convert.ToInt32(loginUserId);
                    bookingTo.UpdatedOn = serverDate;
                    resultMessage = _iTblBookingsBL.PostUpdateOverdueExistOrNotFromBooking(bookingTo, (Convert.ToInt32(loginUserId)));
                }
                else
                {
                    resultMessage.DefaultBehaviour("bookingsTo found null");
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error IN API Call PostUpdateOverdueExistOrNotFromBooking");
                return resultMessage;
            }

        }

        [Route("GetPendingBookingsForApproval")]
        [HttpGet]
        public List<TblBookingsTO> GetPendingBookingsForApproval(Int32 isConfirmed = 0, Int32 idBrand = 1)
        {
            List<TblBookingsTO> tblBookingsTOList = _iTblBookingsBL.SelectAllBookingsListForApproval(isConfirmed, idBrand);

            _iTblBookingsBL.AssignOverDueAmount(tblBookingsTOList);

            return tblBookingsTOList;
        }


        //Priyanka [22-06-2018]
        [Route("MigrateBookingSizesQty")]
        [HttpPost]
        public ResultMessage MigrateBookingSizesQty()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                Int32 res= _iTblBookingsBL.MigrateBookingSizesQty();
                if (res != 1)
                {
                    resultMessage.DefaultBehaviour("Error in MigrateBookingSizesQty");
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "MigrateBookingSizesQty()");
                return resultMessage;
            }
        }



        [Route("GetPendingBookingsForAcceptance")]
        [HttpGet]
        /// <summary>
        /// Sanjay [2017-02-27] Will return list of all bookings of given cnf which are beyond quota and Rate Band
        /// and Approved By Directors. This will be for Acceptance By Cnf
        /// </summary>
        /// <param name="cnfId"></param>
        public List<TblBookingsTO> GetPendingBookingsForAcceptance(Int32 cnfId, Int32 isConfirmed = 0)
        {
            return _iTblBookingsBL.SelectAllBookingsListForAcceptance(cnfId,null, isConfirmed);
        }

        [Route("GetPendingBookingsForAcceptanceByRole")]
        [HttpGet]
        /// <summary>
        /// Sanjay [2017-02-27] Will return list of all bookings of given cnf which are beyond quota and Rate Band
        /// and Approved By Directors. This will be for Acceptance By Cnf
        /// </summary>
        /// <param name="cnfId"></param>
        public List<TblBookingsTO> GetPendingBookingsForAcceptance(Int32 cnfId, string userRoleTOList, Int32 isConfirmed =0)
      {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            List<TblBookingsTO> tblBookingsTOList = _iTblBookingsBL.SelectAllBookingsListForAcceptance(cnfId, tblUserRoleTOList, isConfirmed);
            _iTblBookingsBL.AssignOverDueAmount(tblBookingsTOList);
            return tblBookingsTOList;


        }

        [Route("GetEmptySizeAndProductListForBooking")]
        [HttpGet]
        public List<TblBookingExtTO> GetEmptySizeAndProductListForBooking(Int32 prodCatId,Int32 prodSpecId)
        {
            List<TblBookingExtTO> list = _iTblBookingExtBL.SelectEmptyBookingExtList(prodCatId, prodSpecId);
            return list;
        }


        [Route("GetBookingDashboardInfo")]
        [HttpGet]
        public List<BookingInfo> GetBookingDashboardInfo(String userRoleList, Int32 orgId,Int32 dealerId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleList);
            return _iTblBookingsBL.SelectBookingDashboardInfo(tblUserRoleTOList, orgId,dealerId, sysDate);
        }

        [Route("GetMinAndMaxValueConfigForBookingRate")]
        [HttpGet]
        public String GetMinAndMaxValueConfigForBookingRate()
        {
            string configValue = string.Empty;
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_BOOKING_RATE_MIN_AND_MAX_BAND);
            if (tblConfigParamsTO != null)
                configValue = Convert.ToString(tblConfigParamsTO.ConfigParamVal);
            return configValue;
        }

        [Route("GetAllPendingBookingsForReport")]
        [HttpGet]
        public List<PendingBookingRptTO> GetAllPendingBookingsForReport(Int32 cnfOrgId,Int32 dealerOrgId, string userRoleTOList,int isTransporterScopeYn, int isConfirmed, string fromDate, string toDate,Boolean isDateFilter, Int32 brandId)
      //  public List<PendingBookingRptTO> GetAllPendingBookingsForReport(Int32 cnfOrgId,Int32 dealerOrgId, string userRoleTO,int isTransporterScopeYn, int isConfirmed, string fromDate, string toDate,Boolean isDateFilter,Int32 brandId)
        {
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return _iTblBookingsBL.SelectAllPendingBookingsForReport(cnfOrgId,dealerOrgId, tblUserRoleTOList,  isTransporterScopeYn, isConfirmed, from_Date, to_Date,isDateFilter, brandId);
            //TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
            //return BL._iTblBookingsBL.SelectAllPendingBookingsForReport(cnfOrgId,dealerOrgId, tblUserRoleTO,  isTransporterScopeYn, isConfirmed, from_Date, to_Date,isDateFilter,brandId);
        }

        [Route("CalculateBookingsOpeningBalance")]
        [HttpGet]
        public ResultMessage CalculateBookingsOpeningBalance()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return _iTblBookingOpngBalBL.CalculateBookingOpeningBalance();
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method CalculateBookingsOpeningBalance";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        [Route("SendBookingDueNotification")]
        [HttpGet]
        public ResultMessage SendBookingDueNotification()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return _iTblBookingsBL.SendBookingDueNotification();
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method SendBookingDueNotification";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }



        /// <summary>
        /// Vijaymala[2017-09-11]Added to get booking list to plot graph
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <param name="userRoleTO"></param>
        /// <returns></returns>
        [Route("GetBookingsListForGraph")]
        [HttpGet]
        public List<BookingGraphRptTO> GetBookingsListForGraph(Int32 OrganizationId,string userRoleTOList,Int32 dealerId)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return _iTblBookingsBL.SelectBookingListForGraph(OrganizationId, tblUserRoleTOList,dealerId);
        }

        [Route("GetBookingScheduleListByBookingId")]
        [HttpGet]
        public List<TblBookingScheduleTO> GetBookingScheduleListByBookingId(Int32 bookingId)
        {
            List<TblBookingScheduleTO> list = _iCircularDependencyBL.SelectBookingScheduleByBookingId(bookingId);
            return list;
        }

        /// <summary>
        /// Harshkunj [2018-06-27] Use this call to get booking end time        
        /// </summary>
        [Route("GetAllowedBookingEndTimeDetails")]
        [HttpGet]
        public TblBookingAllowedTimeTO GetAllowedBookingEndTimeDetails(DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

                TblBookingAllowedTimeTO tblBookingAllowedTimeTO = new TblBookingAllowedTimeTO();
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_BOOKING_END_TIME);
                if(tblConfigParamsTO != null)
            {
                tblBookingAllowedTimeTO.ExtendedHrs = tblConfigParamsTO.ConfigParamVal;

            }
            //Double maxAllowedDelPeriod = Convert.ToDouble(Convert.ToString( tblConfigParamsTO.ConfigParamVal).Replace(" ", string.Empty));
            //DateTime serverDate = _iCommon.ServerDateTime;
            //DateTime curSysDate = Convert.ToDateTime(serverDate.ToShortDateString())  ;
            //DateTime dateToCheck = curSysDate.AddHours(maxAllowedDelPeriod);


            return tblBookingAllowedTimeTO;
        }

        /// <summary>
        /// Priyanka [14-12-2018]
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [Route("GetExistingBookingAddrListByDealerId")]
        [HttpGet]
        public List<TblBookingDelAddrTO> GetExistingBookingAddrListByDealerId(Int32 dealerOrgId, String addrSrcTypeString)
        {
            List<TblBookingDelAddrTO> list = _iTblBookingDelAddrBL.SelectExistingBookingAddrListByDealerId(dealerOrgId, addrSrcTypeString);
            return list; 
        }

        /// <summary>
        /// Priyanka [18-01-2019]
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [Route("GetPaymentTermOptionDetails")]
        [HttpGet]
        public List<TblPaymentTermsForBookingTO> GetPaymentTermOptionDetails(Int32 bookingId = 0, Int32 invoiceId = 0)
        {
            return _iTblPaymentTermsForBookingBL.SelectAllTblPaymentTermsForBookingFromBookingId(bookingId, invoiceId);
        }

        [Route("GetConsumptionQuantityDetailsByBookingId")]
        [HttpGet]
        public List<TblBookingQtyConsumptionTO> GetConsumptionQuantityDetailsByBookingId(int bookingId)
        {
            return _iTblBookingQtyConsumptionBL.SelectTblBookingQtyConsumptionTOByBookingId(bookingId); 
        }
        [Route("PrintLodingSlipDetailDetails")]
        [HttpPost]
        public ResultMessage PrintLodingSlipDetailDetails([FromBody] JObject data)
        {
            try
            {
                var bookingId = Convert.ToInt32(data["bookingId"].ToString());
                ResultMessage resultMessage = new StaticStuff.ResultMessage();
                if (bookingId >0)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    return _iTblReportsBL.PrintLoadingTODetailsReport(bookingId);
                }

                else
                {
                    resultMessage.DefaultBehaviour("tempInvoiceDocumentDetailsTO Found NULL");
                    return resultMessage;
                }
            }

            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }


        //Deepali added for task 1272 [03-08-2021]
        [Route("GetBookingAnalysisReport")]
        [HttpGet]
        public List<TblBookingAnalysisReportTO> GetBookingAnalysisReport(DateTime startDate, DateTime endDate,Int32 distributorId,Int32 cOrNcId,Int32 brandId, int skipDate)
        {
            return _iTblBookingsBL.GetBookingAnalysisReport(startDate, endDate, distributorId, cOrNcId, brandId, skipDate);
        }
        #endregion

        #region Post

        /// <summary>
        /// Sanjay [2017-03-06] Use this call to Update Bookings for Various Statuses
        /// i.e. Booking Confirmation beyond quota and Rate By Directors,Booking Acceptance By C&F
        /// Booking Delete,Booking Reject et.c
        /// When Booking is Deleted then additional Attribute IsDeleted should be marked 1 to check for further calculations
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // POST api/values
        [Route("PostBookingAcceptance")]
        [HttpPost]
        public Int32 PostBookingAcceptance([FromBody] JObject data)
        {

            try
            {
                TblBookingsTO tblBookingsTO = JsonConvert.DeserializeObject<TblBookingsTO>(data["bookingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblBookingsTO == null)
                {
                    return 0;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    return 0;
                }

                tblBookingsTO.StatusDate = _iCommon.ServerDateTime;
                tblBookingsTO.UpdatedOn = _iCommon.ServerDateTime;
                tblBookingsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage resMsg = _iTblBookingsBL.UpdateBookingConfirmations(tblBookingsTO);
                if (resMsg.MessageType != ResultMessageE.Information)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // POST api/values 
        [Route("PostNewBooking")]
        [HttpPost]
        public ResultMessage PostNewBooking([FromBody] JObject data) 
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage(); 
            try
            {

                //loggerObj.LogInformation(1, "In PostNewBooking", data);
                TblBookingsTO tblBookingsTO = JsonConvert.DeserializeObject<TblBookingsTO>(data["bookingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblBookingsTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "bookingTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                tblBookingsTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblBookingsTO.CreatedOn = _iCommon.ServerDateTime;
                if(tblBookingsTO.EnquiryId == 0)
                    tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;

                tblBookingsTO.StatusDate = tblBookingsTO.CreatedOn;
                tblBookingsTO.BookingDatetime = tblBookingsTO.CreatedOn;
                return _iTblBookingsBL.SaveNewBooking(tblBookingsTO);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Exception Error in API Call";
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
        }

        // POST api/values
        [Route("PostBookingUpdate")]
        [HttpPost]
        public ResultMessage PostBookingUpdate([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblBookingsTO tblBookingsTO = JsonConvert.DeserializeObject<TblBookingsTO>(data["bookingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblBookingsTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblBookingsTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }


                //TblBookingsTO existingBookingTO = BL._iTblBookingsBL.SelectTblBookingsTO(tblBookingsTO.IdBooking);
                //tblBookingsTO.TranStatusE = existingBookingTO.TranStatusE;
                //tblBookingsTO.StatusRemark = existingBookingTO.StatusRemark;

                tblBookingsTO.UpdatedOn = _iCommon.ServerDateTime;
                tblBookingsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                return _iTblBookingsBL.UpdateBooking(tblBookingsTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method PostBookingUpdate";
                return resultMessage;
            }
        }


        // POST api/values
        [Route("PostBookingClosure")]
        [HttpPost]
        public ResultMessage PostBookingClosure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblBookingActionsTO bookingActionsTO = JsonConvert.DeserializeObject<TblBookingActionsTO>(data["bookingActionsTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (bookingActionsTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "bookingActionsTO found null";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId found null";
                    return resultMessage;
                }

                bookingActionsTO.StatusDate = _iCommon.ServerDateTime;
                bookingActionsTO.StatusBy = Convert.ToInt32(loginUserId);
                return _iTblBookingActionsBL.SaveBookingActions(bookingActionsTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method PostBookingClosure";
                return resultMessage;
            }
        }


        [Route("PostDeleteBookingById")]
        [HttpPost]
        public ResultMessage PostDeleteBookingById([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                var bookingId = data["bookingId"].ToString();
                var statusRemark = data["statusRemark"].ToString();
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(bookingId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "bookingId not found";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId not found";
                    return resultMessage;
                }

                TblBookingsTO tblBookingsTO = _iTblBookingsBL.SelectTblBookingsTO(Convert.ToInt32(bookingId));
                tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_DELETE;
                tblBookingsTO.IsDeleted = 1;
                tblBookingsTO.StatusRemark = statusRemark;
                tblBookingsTO.StatusDate = _iCommon.ServerDateTime;
                tblBookingsTO.UpdatedOn = _iCommon.ServerDateTime;
                tblBookingsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                return _iTblBookingsBL.UpdateBookingConfirmations(tblBookingsTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception In API Call : PostDeleteBooking";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Harshkunj [2018-06-27] Use this call to set booking end time        
        /// </summary>
        [Route("PostBookingEndTime")]
        [HttpPost]
        public ResultMessage PostBookingEndTime([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                TblBookingAllowedTimeTO loadingAllowedTimeTO = JsonConvert.DeserializeObject<TblBookingAllowedTimeTO>(data["bookingAllowedTimeTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingAllowedTimeTO == null)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "bookingAllowedTimeTO Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                loadingAllowedTimeTO.CreatedBy = Convert.ToInt32(loginUserId);
                loadingAllowedTimeTO.CreatedOn = createdDate;

                //loadingAllowedTimeTO.AllowedBookingTime = Convert.ToDateTime(loadingAllowedTimeTO.ExtendedHrs) ;
                loadingAllowedTimeTO.AllowedBookingTime = Convert.ToDateTime(loadingAllowedTimeTO.ExtendedHrs);

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_BOOKING_END_TIME);
                if (tblConfigParamsTO != null)
                    //tblConfigParamsTO.ConfigParamVal =  Convert.ToString(loadingAllowedTimeTO.AllowedBookingTimeStr).Replace(" ", string.Empty);
                    tblConfigParamsTO.ConfigParamVal = loadingAllowedTimeTO.ExtendedHrs;

                int result= _iTblConfigParamsBL.UpdateTblConfigParams(tblConfigParamsTO);
                rMessage.MessageType = ResultMessageE.Information;
                if (result > 0)
                {
                    rMessage.Text = "Record Updated Sucessfully";
                    rMessage.Result = 1;
                }
                else
                {
                    rMessage.Text = "Record Could Updated Sucessfully";
                    rMessage.Result = -1;
                }
                return rMessage;

            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostAllowedLoadingTime";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }

        [Route("PostCloseQuantity")]
        [HttpPost]
        public ResultMessage PostCloseQuantity([FromBody] JObject data)
        {

            ResultMessage resultMessage = new ResultMessage();

            try
            {
                //TblBookingsTO tblBookingsTO = JsonConvert.DeserializeObject<TblBookingsTO>(data.ToString());
                var idBooking = data["bookingId"].ToString();
                var remark = data["statusRemark"].ToString();
                var createdByuserId = data["createdById"].ToString();
                TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO = new TblBookingQtyConsumptionTO();
                tblBookingQtyConsumptionTO.BookingId = Convert.ToInt32(idBooking);
                tblBookingQtyConsumptionTO.Remark = remark;
                tblBookingQtyConsumptionTO.CreatedBy = Convert.ToInt32(createdByuserId);
                return _iTblBookingsBL.UpdatePendingQuantity(tblBookingQtyConsumptionTO);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                return resultMessage;
            }

        }

        #endregion

        #region Put

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion

        #region Delete

        
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion
    }
}
