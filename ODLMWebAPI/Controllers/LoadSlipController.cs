using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DashboardModels;
using System.Globalization;

using System.Threading;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.IoT.Interfaces;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class LoadSlipController : Controller
    {
        private readonly ITblStatusReasonBL _iTblStatusReasonBL;
        private readonly ITblUserBL _iTblUserBL; 
        private readonly ITblLoadingBL _iTblLoadingBL; 
        private readonly ITblTransportSlipBL _iTblTransportSlipBL;
        private readonly ITblLoadingVehDocExtBL _iTblLoadingVehDocExtBL;
        private readonly ITblInvoiceBL _iTblInvoiceBL;
        private readonly ITblLoadingSlipBL _iTblLoadingSlipBL;
        private readonly ITblLoadingQuotaConfigBL _iTblLoadingQuotaConfigBL;
        private readonly ITblLoadingQuotaDeclarationBL _iTblLoadingQuotaDeclarationBL;
        private readonly ITblLoadingSlipExtBL _iTblLoadingSlipExtBL;
        private readonly ITblLoadingAllowedTimeBL _iTblLoadingAllowedTimeBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblBookingDelAddrBL _iTblBookingDelAddrBL;
        private readonly ITblUnLoadingBL _iTblUnLoadingBL;
        private readonly ITblUnloadingStandDescBL _iTblUnloadingStandDescBL;
        private readonly ITblGlobalRateBL _iTblGlobalRateBL;
        private readonly ITblLoadingQuotaTransferBL _iTblLoadingQuotaTransferBL;
        private readonly IDimStatusBL _iDimStatusBL;
        private readonly ICommon _iCommon;
        private readonly IIotCommunication _iIotCommunication;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        public LoadSlipController(ITblConfigParamsDAO iTblConfigParamsDAO,IIotCommunication iIotCommunication,ITblInvoiceBL iTblInvoiceBL, IDimStatusBL iDimStatusBL, ITblLoadingQuotaTransferBL iTblLoadingQuotaTransferBL, ITblGlobalRateBL iTblGlobalRateBL, ITblUnloadingStandDescBL iTblUnloadingStandDescBL, ITblUnLoadingBL iTblUnLoadingBL, ITblBookingDelAddrBL iTblBookingDelAddrBL, ITblConfigParamsBL iTblConfigParamsBL, ITblLoadingAllowedTimeBL iTblLoadingAllowedTimeBL, ITblLoadingSlipExtBL iTblLoadingSlipExtBL, ITblLoadingQuotaDeclarationBL iTblLoadingQuotaDeclarationBL, ITblLoadingQuotaConfigBL iTblLoadingQuotaConfigBL, ITblLoadingSlipBL iTblLoadingSlipBL, ITblLoadingVehDocExtBL iTblLoadingVehDocExtBL, ICommon iCommon, ITblStatusReasonBL iTblStatusReasonBL, ITblUserBL iTblUserBL, ITblLoadingBL iTblLoadingBL, ITblTransportSlipBL iTblTransportSlipBL)
        {
            _iTblStatusReasonBL = iTblStatusReasonBL;
            _iTblUserBL = iTblUserBL;
            _iTblLoadingBL = iTblLoadingBL;
            _iTblTransportSlipBL = iTblTransportSlipBL;
            _iTblLoadingVehDocExtBL = iTblLoadingVehDocExtBL;
            _iTblInvoiceBL = iTblInvoiceBL;
            _iTblLoadingSlipBL = iTblLoadingSlipBL;
            _iTblLoadingQuotaConfigBL = iTblLoadingQuotaConfigBL;
            _iTblLoadingQuotaDeclarationBL = iTblLoadingQuotaDeclarationBL;
            _iTblLoadingSlipExtBL = iTblLoadingSlipExtBL;
            _iTblLoadingAllowedTimeBL = iTblLoadingAllowedTimeBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblBookingDelAddrBL = iTblBookingDelAddrBL;
            _iTblUnLoadingBL = iTblUnLoadingBL;
            _iTblUnloadingStandDescBL = iTblUnloadingStandDescBL;
            _iTblGlobalRateBL = iTblGlobalRateBL;
            _iTblLoadingQuotaTransferBL = iTblLoadingQuotaTransferBL;
            _iDimStatusBL = iDimStatusBL;
            _iCommon = iCommon;
            _iIotCommunication = iIotCommunication;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }
        #region Get
        
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetLoadStsReasonsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetLoadStsReasonsForDropDown(Int32 statusId)
        {
            List<TblStatusReasonTO> tblStatusReasonTOList = _iTblStatusReasonBL.SelectAllTblStatusReasonList(statusId);
            if (tblStatusReasonTOList != null && tblStatusReasonTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblStatusReasonTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblStatusReasonTOList[i].ReasonDesc;
                    dropDownTO.Value = tblStatusReasonTOList[i].IdStatusReason;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }


        [Route("GetSuperwisorListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetSuperwisorListForDropDown()
        {

            List<TblUserTO> tblUserTOList = _iTblUserBL.SelectAllTblUserByRoleType(Convert.ToInt32(Constants.SystemRoleTypeE.SUPERWISOR));
            if(tblUserTOList != null && tblUserTOList.Count >0)
            {
                Dictionary<Int32, Int32> DCT = _iTblLoadingBL.SelectCountOfLoadingsOfSuperwisorDCT(_iCommon.ServerDateTime);
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblUserTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Value = tblUserTOList[i].IdUser;

                    String countToAppend = string.Empty;
                    if (DCT != null && DCT.ContainsKey(tblUserTOList[i].IdUser))
                        countToAppend = " (" + DCT[tblUserTOList[i].IdUser] + ")";

                    dropDownTO.Text = (tblUserTOList[i].UserDisplayName) + countToAppend;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            //List<TblSupervisorTO> tblSupervisorTOList = BL.TblSupervisorBL.SelectAllTblSupervisorList();
            //if (tblSupervisorTOList != null && tblSupervisorTOList.Count > 0)
            //{
            //    tblSupervisorTOList = tblSupervisorTOList.OrderBy(a => a.SupervisorName).ToList();
            //    Dictionary<Int32, Int32> DCT = _iTblLoadingBL.SelectCountOfLoadingsOfSuperwisorDCT(_iCommon.ServerDateTime);

            //    List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
            //    for (int i = 0; i < tblSupervisorTOList.Count; i++)
            //    {
            //        DropDownTO dropDownTO = new DropDownTO();
            //        dropDownTO.Value = tblSupervisorTOList[i].IdSupervisor;

            //        String countToAppend = string.Empty;
            //        if (DCT != null && DCT.ContainsKey(tblSupervisorTOList[i].IdSupervisor))
            //            countToAppend = " (" + DCT[tblSupervisorTOList[i].IdSupervisor] + ")";
            //        dropDownTO.Text = tblSupervisorTOList[i].SupervisorName + countToAppend;
            //        statusReasonList.Add(dropDownTO);
            //    }
            //    return statusReasonList;
            //}
            else return null;
        }

        [Route("GetVehicleNumberList")]
        [HttpGet]
        public List<VehicleNumber> GetVehicleNumberList()
        {
            return _iTblLoadingBL.SelectAllVehicles();
        }
        /// <summary>
        /// Kiran 09-12-2017 @Add for get all transport slip List
        /// </summary>
        /// <returns></returns>
        [Route("GetAllTransportSlipList")]
        [HttpGet]
        public List<TblTransportSlipTO> GetAllTransportSlipList(string toDate, int isLink)
        {
            DateTime tDate = DateTime.MinValue;
            if (Constants.IsDateTime(toDate))
                tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
            return _iTblTransportSlipBL.SelectAllTblTransportSlip(tDate, isLink);
        }

        [Route("GetLoadingslipDetails")]
        [HttpGet]
        public TblLoadingTO GetLoadingslipDetails(Int32 loadingId)
        {
            return _iTblLoadingBL.SelectLoadingTOWithDetails(loadingId);
        }
        /// <summary>
        /// GJ@20171012 : Get the LoadingTo details by LoadingSlipId
        /// </summary>
        /// <param name="loadingId"></param>
        /// <returns></returns>
        [Route("GetLoadingTODetailsByLoadingSlipId")]
        [HttpGet]
        public TblLoadingTO GetLoadingTODetailsByLoadingSlipId(Int32 loadingSlipId)
        {
            return _iTblLoadingBL.SelectLoadingTOWithDetailsByLoadingSlipId(loadingSlipId);
        }


        // <summary>
        /// Saket [2018-04-25] Added
        /// </summary>
        /// <param name="loadingId"></param>
        /// <returns></returns>
        [Route("GetLoadingTODetailsByInvoiceId")]
        [HttpGet]
        public TblLoadingTO GetLoadingTODetailsByInvoiceId(Int32 invoiceId)
        {
            return _iTblLoadingBL.SelectLoadingTOWithDetailsByInvoiceId(invoiceId);
        }


        [Route("GetLoadingslipListWithDetails")]
        [HttpGet]
        public List<TblLoadingTO> GetLoadingslipDetailsList(string loadingIds)
        {
            return _iTblLoadingBL.SelectLoadingTOListWithDetails(loadingIds);
        }

        /// <summary>
        /// Kiran 19-04-2018 : Get the Get LoadingDetails For Report
        /// </summary>
        /// <param name="loadingId"></param>
        /// <returns></returns>
        [Route("GetLoadingDetailsForReport")]
        [HttpGet]
        public List<TblLoadingTO> GetLoadingDetailsForReport(string fromDate, string toDate)
        {
            DateTime frmDate = DateTime.MinValue;
            DateTime tDate = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            DateTime serverDate = _iCommon.ServerDateTime;
            if (frmDate == DateTime.MinValue)
                frmDate = serverDate.Date;
            if (tDate == DateTime.MinValue)
                tDate = serverDate.Date;
            return _iTblLoadingBL.GetLoadingDetailsForReport(frmDate, tDate);
        }


        /// <summary>
        /// Saket [2018-02-21] Added.
        /// </summary>
        /// <param name="loadingId"></param>
        /// <returns></returns>
        [Route("GetLoadingVehDocExtTOList")]
        [HttpGet]
        public List<TblLoadingVehDocExtTO> GetLoadingVehDocExtTOList(Int32 loadingId, Int32 getEmptyList = 0)
        {
            return _iTblLoadingVehDocExtBL.SelectAllTblLoadingVehDocExtListEmptyAgainstLLoading(loadingId, getEmptyList);
        }

        /// <summary>
        /// Saket [2018-02-16] Added
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("CreateIntermediateInvoiceAgainstLoading")]
        [HttpPost]
        public ResultMessage CreateIntermediateInvoiceAgainstLoading([FromBody] JObject data)
        {

            var loadingIds = data["loadingIds"].ToString();
            var loginUserId = data["loginUserId"].ToString();

            Int32 userId = 0;
            Int32.TryParse(loginUserId, out userId);
            return _iTblLoadingBL.CreateIntermediateInvoiceAgainstLoading(loadingIds, userId);
        }


        [Route("GetAllPendingLoadingList")]
        [HttpGet]
        public List<TblLoadingTO> GetAllPendingLoadingList(string userRoleTOList, Int32 cnfId, Int32 loadingStatusId, string fromDate, String toDate,Int32 loadingTypeId,Int32 dealerId, Int32 isConfirm = -1, Int32 brandId = 0, Int32 loadingNavigateId = 0,Int32 superwisorId=0)
        {
            try
            {
                DateTime frmDate = DateTime.MinValue;
                DateTime tDate = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                    frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

                DateTime serverDate = _iCommon.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate.Date;
                if (tDate == DateTime.MinValue)
                    tDate = serverDate.Date;

                List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

                return _iTblLoadingBL.SelectAllTblLoadingList(tblUserRoleTOList, cnfId, loadingStatusId, frmDate, tDate, loadingTypeId, dealerId, isConfirm, brandId, loadingNavigateId,superwisorId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Priyanka [01-06-2018] : Added to get loading slip list in view loading slips.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cnfId"></param>
        /// <param name="loadingStatusId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="loadingTypeId"></param>
        /// <param name="dealerId"></param>
        /// <param name="isConfirm"></param>
        /// <returns></returns>

        [Route("GetAllLoadingSlipList")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetAllLoadingSlipList(string userRoleTOList, Int32 cnfId, Int32 loadingStatusId, string fromDate, String toDate, Int32 loadingTypeId, Int32 dealerId, Int32 isConfirm = -1, Int32 brandId = 0,Int32 superwisorId=0)
        {
            try
            {
                DateTime frmDate = DateTime.MinValue;
                DateTime tDate = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                    frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

                DateTime serverDate = _iCommon.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate.Date;
                if (tDate == DateTime.MinValue)
                    tDate = serverDate.Date;

                List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

                return _iTblLoadingSlipBL.SelectAllLoadingSlipList(tblUserRoleTOList, cnfId, loadingStatusId, frmDate, tDate, loadingTypeId, dealerId, isConfirm, brandId, superwisorId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// @kiran 11-12-2017 Added for get all loading slip agians dealer
        /// </summary>
        /// <param name="vehicleNo"></param>
        /// <param name="loadingDate"></param>
        /// <returns></returns>
        [Route("GetAllLoadingLinkList")]
        [HttpGet]
        public List<TblLoadingTO> GetAllLoadingLinkList(string userRoleTOList, Int32 dearlerOrgId, Int32 loadingStatusId, string fromDate, String toDate)
        {
            try
            {
                DateTime frmDate = DateTime.MinValue;
                DateTime tDate = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                    frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

                DateTime serverDate = _iCommon.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate.Date;
                if (tDate == DateTime.MinValue)
                    tDate = serverDate.Date;

                List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

                return _iTblLoadingBL.SelectAllTblLoadingLinkList(tblUserRoleTOList, dearlerOrgId, loadingStatusId, frmDate, tDate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("GetAllLoadingListByVehicleNo")]
        [HttpGet]
        public List<TblLoadingTO> GetAllLoadingListByVehicleNo(string vehicleNo, String loadingDate)
        {
            try
            {

                DateTime frmDate = Convert.ToDateTime(Convert.ToDateTime(loadingDate).ToString(Constants.AzureDateFormat));
                DateTime serverDate = _iCommon.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate;

                return _iTblLoadingBL.SelectAllLoadingListByVehicleNo(vehicleNo, frmDate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// GJ@20170829 : Get the Loading slip list those are in but not completed losding slip
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <param name="prodSpecId"></param>
        /// <returns></returns>
        [Route("GetAllInLoadingListByVehicleNo")]
        [HttpGet]
        public List<TblLoadingTO> GetAllInLoadingListByVehicleNo(string vehicleNo,int loadingId=0)//Aniket [13-6-2019] added loadingId paramater
        {
            try
            {

                return _iTblLoadingBL.SelectAllLoadingListByVehicleNo(vehicleNo, false,loadingId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("GetCnfLoadingCofiguration")]
        [HttpGet]
        public List<TblLoadingQuotaConfigTO> GetCnfLoadingCofiguration(Int32 prodCatId, Int32 prodSpecId)
        {
            List<TblLoadingQuotaConfigTO> list = _iTblLoadingQuotaConfigBL.SelectLatestLoadingQuotaConfigList(prodCatId, prodSpecId);
            return list;
        }

        [Route("GetCnfLoadingQuotaDeclaration")]
        [HttpGet]
        public List<TblLoadingQuotaDeclarationTO> GetCnfLoadingQuotaDeclaration(DateTime stockDate, Int32 prodCatId, Int32 prodSpecId)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime;

            List<TblLoadingQuotaDeclarationTO> list = _iTblLoadingQuotaDeclarationBL.SelectLatestCalculatedLoadingQuotaDeclarationList(stockDate, prodCatId, prodSpecId);
            return list;
        }

        [Route("IsLoadingQuotaDeclaredToday")]
        [HttpGet]
        public Boolean IsLoadingQuotaDeclaredToday(DateTime stockDate, Int32 prodCatId, Int32 prodSpecId)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime;

            return _iTblLoadingQuotaDeclarationBL.IsLoadingQuotaDeclaredForTheDate(stockDate, prodCatId, prodSpecId);
        }

        [Route("IsLoadingQuotaDeclaredForTheDate")]
        [HttpGet]
        public Boolean IsLoadingQuotaDeclaredForTheDate(DateTime stockDate)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime;

            return _iTblLoadingQuotaDeclarationBL.IsLoadingQuotaDeclaredForTheDate(stockDate);
        }

        [Route("GetAvailableLoadingQuotaForCnf")]
        [HttpGet]
        public List<TblLoadingQuotaDeclarationTO> GetAvailableLoadingQuotaForCnf(Int32 cnfId, DateTime stockDate)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime;

            List<TblLoadingQuotaDeclarationTO> list = _iTblLoadingQuotaDeclarationBL.SelectAvailableLoadingQuotaForCnf(cnfId, stockDate);
            return list;
        }

        [Route("GetEmptySizeAndProductListForLoading")]
        [HttpGet]
        public List<TblLoadingSlipExtTO> GetEmptySizeAndProductListForLoading(Int32 prodCatId, Int32 boookingId)
        {
            List<TblLoadingSlipExtTO> list = _iTblLoadingSlipExtBL.SelectEmptyLoadingSlipExtList(prodCatId, boookingId);
            return list;
        }

        [Route("GetEmptySizeAndProdLstForNewLoading")]
        [HttpGet]
        public List<TblLoadingSlipExtTO> GetEmptySizeAndProductListForLoading(Int32 prodCatId, Int32 prodSpecId, Int32 boookingId, Int32 brandId)
        {
            List<TblLoadingSlipExtTO> list = _iTblLoadingSlipExtBL.SelectEmptyLoadingSlipExtList(prodCatId, prodSpecId, boookingId, brandId);
            return list;
        }

        [Route("GetEmptySizeAndProductListForLoadingAgainstSch")]
        [HttpGet]
        public List<TblBookingScheduleTO> GetEmptySizeAndProductListForLoadingAgainstSch(Int32 prodCatId, Int32 prodSpecId, Int32 boookingId, Int32 brandId)
        {
            List<TblBookingScheduleTO> list = _iTblLoadingSlipExtBL.SelectEmptyLoadingSlipExtListAgainstSch(prodCatId, prodSpecId, boookingId, brandId);
            return list;
        }

        /// <summary>
        /// Sanjay [2017-05-25] It will return material details alongwith its layer details
        /// Can be used to show popup for showing why Loading Approval is needed based on quotaafterloading property
        /// </summary>
        /// <param name="loadingId"></param>
        /// <returns></returns>
        [Route("GetLoadingMaterialDetails")]
        [HttpGet]
        public List<TblLoadingSlipExtTO> GetLoadingMaterialDetails(Int32 loadingId)
        {
            List<TblLoadingSlipExtTO> list = _iTblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(loadingId);
            return list;
        }


        [Route("GetLoadingSlipsByStatus")]
        [HttpGet]
        public List<TblLoadingTO> GetLoadingSlipsByStatus(String statusId)
        {
            //Aniket [30-7-2019] added for IOT
            int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();

           
            var tempStatusIds = statusId;
            if (weightSourceConfigId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                tempStatusIds = Convert.ToString((int)Constants.TranStatusE.LOADING_CONFIRM);
            }
            List<TblLoadingTO> list = _iTblLoadingBL.SelectAllLoadingListByStatus(tempStatusIds);
            if (list != null)
            {
                string finalStatusId = _iIotCommunication.GetIotEncodedStatusIdsForGivenStatus(statusId);
                list = _iTblLoadingBL.SetLoadingStatusData(finalStatusId, true, weightSourceConfigId, list);
                List<TblLoadingTO> finalList = new List<TblLoadingTO>();

                string[] statusIds = statusId.Split(',');

                for (int i = 0; i < statusIds.Length; i++)
                {
                    if (Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_CONFIRM
                        || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
                        || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN)
                    {
                        var sendInList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (sendInList != null)
                            finalList.AddRange(sendInList);

                        var reportedList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (reportedList != null)
                            finalList.AddRange(reportedList);

                        var confirmList = list.Where(r => r.StatusId != (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
                                                     && r.StatusId != (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (confirmList != null)
                            finalList.AddRange(confirmList);

                        return finalList;
                    }
                    else if (Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_GATE_IN
                        || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_COMPLETED)
                    {

                        var reportedList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_COMPLETED).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (reportedList != null)
                            finalList.AddRange(reportedList);

                        var confirmList = list.Where(r => r.StatusId != (int)Constants.TranStatusE.LOADING_COMPLETED).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (confirmList != null)
                            finalList.AddRange(confirmList);

                        return finalList;
                    }
                }
            }

            return list;
        }

        [Route("GetLoadingDashboardInfo")]
        [HttpGet]
        public LoadingInfo GetLoadingDashboardInfo(String userRoleList, Int32 orgId, DateTime sysDate, Int32 loadingType = 1)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleList);
            return _iTblLoadingBL.SelectDashboardLoadingInfo(tblUserRoleTOList, orgId, sysDate, loadingType);
        }

        [Route("IsLoadingAllowed")]
        [HttpGet]
        public ResultMessage IsLoadingAllowed(DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

            ResultMessage resultMessage = new ResultMessage();
            TblLoadingAllowedTimeTO latestAllowedLoadingTimeTO = _iTblLoadingAllowedTimeBL.SelectAllowedLoadingTimeTO(sysDate);
            if (latestAllowedLoadingTimeTO == null)
            {
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_DEFAULT_ALLOWED_UPTO_TIME);
                Double maxAllowedDelPeriod = Convert.ToDouble(tblConfigParamsTO.ConfigParamVal);

                DateTime serverDate = _iCommon.ServerDateTime;
                DateTime curSysDate = serverDate.Date;
                DateTime dateToCheck = curSysDate.AddHours(maxAllowedDelPeriod);
                if (serverDate < dateToCheck)
                {
                    resultMessage.Result = 1;
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Allowed Upto " + dateToCheck.ToString(Constants.DefaultDateFormat);
                }
                else
                {
                    resultMessage.Result = 0;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Loading is allowed upto " + dateToCheck.ToString(Constants.DefaultDateFormat) + " only";
                }
            }
            else
            {
                DateTime serverDate = _iCommon.ServerDateTime;
                if (serverDate < latestAllowedLoadingTimeTO.AllowedLoadingTime)
                {
                    resultMessage.Result = 1;
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Allowed Upto " + latestAllowedLoadingTimeTO.AllowedLoadingTime.ToString(Constants.DefaultDateFormat);
                }
                else
                {
                    resultMessage.Result = 0;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Loading is allowed upto " + latestAllowedLoadingTimeTO.AllowedLoadingTime.ToString(Constants.DefaultDateFormat) + " only";
                }
            }

            return resultMessage;
        }

        /// <summary>
        /// Priyanka [11-05-2018] : Added to convert Non-Confirmed to Confirmed.
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        [Route("PostUpdateNCToCLoadingSlip")]
        [HttpGet]
        public ResultMessage PostUpdateNCToCLoadingSlip( Int32 loginUserId)
        {
           return _iTblLoadingBL.UpdateNCToCLoadingSlip(loginUserId);
        }

        [Route("IsLoadingSlipCanBePrepared")]
        [HttpGet]
        public ResultMessage IsLoadingSlipCanBePrepared(int parentLoadingId)
        {

            ResultMessage resultMessage = new ResultMessage();
            List<TblLoadingTO> list = _iTblLoadingBL.SelectAllLoadingsFromParentLoadingId(parentLoadingId);
            if (list == null || list.Count == 0)
            {
                resultMessage.Result = 1;
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Allowed, No Loading Slips Prepared.";
            }
            else
            {
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Not Allowed. Loading is already Prepared against this . Ref No : " + list[0].IdLoading + " " + list[0].LoadingSlipNo + " Vehicle No :" + list[0].VehicleNo;
            }

            return resultMessage;
        }

        [Route("GetAllowedLoadingTimeDetails")]
        [HttpGet]
        public TblLoadingAllowedTimeTO GetAllowedLoadingTimeDetails(DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

            ResultMessage resultMessage = new ResultMessage();
            TblLoadingAllowedTimeTO latestAllowedLoadingTimeTO = _iTblLoadingAllowedTimeBL.SelectAllowedLoadingTimeTO(sysDate);
            if (latestAllowedLoadingTimeTO == null)
            {
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_DEFAULT_ALLOWED_UPTO_TIME);
                Double maxAllowedDelPeriod = Convert.ToDouble(tblConfigParamsTO.ConfigParamVal);

                DateTime serverDate = _iCommon.ServerDateTime;
                DateTime curSysDate = serverDate.Date;
                DateTime dateToCheck = curSysDate.AddHours(maxAllowedDelPeriod);
                latestAllowedLoadingTimeTO = new TblLoadingAllowedTimeTO();
                latestAllowedLoadingTimeTO.AllowedLoadingTime = dateToCheck;
                latestAllowedLoadingTimeTO.CreatedOn = serverDate;
            }

            return latestAllowedLoadingTimeTO;
        }

        [Route("IsThisVehicleDelivered")]
        [HttpGet]
        public ResultMessage IsThisVehicleDelivered(String vehicleNo)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<TblLoadingTO> list = _iTblLoadingBL.SelectAllLoadingListByVehicleNo(vehicleNo, true,0);
            if (list == null || list.Count == 0)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Allowed , All Loadings Are Delivered";
                resultMessage.Result = 1;
            }
            else
            {
                var lastObj = list.OrderByDescending(s => s.StatusDate).FirstOrDefault();
                if (lastObj != null && lastObj.IsAllowNxtLoading == 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Allowed ,  next Loadings is Allowed";
                    resultMessage.Result = 1;
                    return resultMessage;

                }
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Not Allowed , Selected Vehicle :" + vehicleNo + " is not delivered . Last status is " + lastObj.StatusDesc;
                resultMessage.Result = 0;
            }

            return resultMessage;
        }


        [Route("GetAddressesForNewLoadingSlip")]
        [HttpGet]
        public List<TblBookingDelAddrTO> GetAddressesForNewLoadingSlip(Int32 addrSourceTypeId, Int32 entityId)
        {
            return _iTblBookingDelAddrBL.SelectDeliveryAddrListFromDealer(addrSourceTypeId, entityId);
        }

        /// <summary>
        /// GJ@20170810 : get All vehicle number List , those are in 'In' premises
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [Route("GetVehicleNumberListByStauts")]
        [HttpGet]
        public List<DropDownTO> GetVehicleNumberListByStauts(int statusId)
        {
            return _iTblLoadingBL.SelectAllVehiclesByStatus(statusId);
        }

        /// <summary>
        /// Vaibhav [14-Sep-2017] Added to select all unloading slip details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllUnLoadingDetails")]
        [HttpGet]
        public List<TblUnLoadingTO> GetAllUnLoadingDetails(DateTime startDate, DateTime endDate)
        {
            return _iTblUnLoadingBL.SelectAllTblUnLoadingList(startDate, endDate);
        }

        /// <summary>
        /// Vaibhav [14-Sep-2017] Added to select particular unloading slip details 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetUnLoadingDetails")]
        [HttpGet]
        public TblUnLoadingTO GetUnLoadingDetails(Int32 unLoadingId)
        {
            return _iTblUnLoadingBL.SelectTblUnLoadingTO(unLoadingId);
        }

        /// <summary>
        /// Vaibhav [20-Sep-2017] Added to get unloading standard desc list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetUnloadingStandDescList")]
        [HttpGet]
        public List<TblUnloadingStandDescTO> GetUnloadingStandDescList()
        {
            return _iTblUnloadingStandDescBL.SelectAllTblUnloadingStandDescList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [Route("IsInvoiceGeneratedByLoadingSlipId")]
        [HttpGet]
        public TblInvoiceTO IsInvoiceGeneratedByLoadingSlipId(Int32 idLoadingSlip)
        {
            TblInvoiceTO invoiceTO = _iTblInvoiceBL.SelectInvoiceTOFromLoadingSlipId(idLoadingSlip);
            if (invoiceTO == null)
            {
                return null;
            }
            return invoiceTO;
        }

        /// <summary>
        /// GJ@20171012 : get the Loading Slip details By LoadingSlipId
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetLoadingSlipDetailsByLoadingSlipId")]
        [HttpGet]
        public TblLoadingSlipTO GetLoadingSlipDetailsByLoadingSlipId(Int32 loadingSlipId)
        {
            return _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails(loadingSlipId);
        }

        /// <summary>
        /// Saket [2018-04-20] Added
        /// </summary>
        /// <param name="loadingSlipId"></param>
        /// <returns></returns>
        [Route("GetLoadingSlipDetailsByInvoiceId")]
        [HttpGet]
        public TblLoadingSlipTO GetLoadingSlipDetailsByInvoiceId(Int32 invoiceId)
        {
            return _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetailsByInvoice(invoiceId);
        }

        /// <summary>
        /// [13-12-2017] Vijaymala : Added To Get Loading slip extension list according to filter 
        /// </summary>
        /// <returns></returns>

        [Route("GetAllTblLoadingSlipExtByDate")]
        [HttpGet]
        public List<TblLoadingSlipExtTO> GetAllTblLoadingSlipExtByDate(string fromDate, string toDate)
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


            String statusIdsStr = String.Empty;

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SIZEWISE_LOADING_REPORT_STATUS_IDS);
            if (tblConfigParamsTO != null)
            {
                statusIdsStr = tblConfigParamsTO.ConfigParamVal;
            }

            return _iTblLoadingSlipExtBL.SelectAllTblLoadingSlipExtByDate(frmDt, toDt, statusIdsStr);
        }

        [Route("GetProductGroupItemGlobalRate")]
        [HttpGet]
        /// <summary>
        /// [19/01/2018] Vijaymala : Added To Get Rate Of Group Against Product Item
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public TblGlobalRateTO GetProductGroupItemGlobalRate(Int32 prodItemId)
        {

            return _iTblGlobalRateBL.SelectProductGroupItemGlobalRate(prodItemId);
        }


        /// <summary>
        /// [27-02-2018] Sudhir : Added To Get Loading Cycle List By Date. 
        /// </summary>
        /// <returns></returns>

        [Route("GetAllLoadingCycleListByDate")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetAllLoadingCycleListByDate(string fromDate, string toDate, String userRoleTOList, Int32 cnfId,Int32 vehicleStatus)
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

            //vijaymala[04-04-2018]modify the code to display records acc to role
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return _iTblLoadingSlipBL.SelectAllLoadingCycleList(frmDt, toDt, tblUserRoleTOList, cnfId, vehicleStatus);
           // return _iTblLoadingSlipExtBL.SelectAllTblLoadingSlipExtByDate(frmDt, toDt);
        }

        /// <summary>
        /// Priyanka [11-05-2018] : Added for Displaying ORC Report for booking and loading.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        
        [Route("GetORCReportDetailsList")]
        [HttpGet]
        public List<TblORCReportTO> GetORCReportDetailsList(DateTime fromDate, DateTime toDate, Int32 flag)
        {
            return _iTblLoadingSlipBL.SelectORCReportDetailsList(fromDate, toDate, flag);
        }



        //Sudhir
        [Route("GetAllLoadingListByVehicleNoForSupport")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetAllLoadingListByVehicleNoForSupport(string vehicleNo)
        {
            try
            {

                //DateTime frmDate = Convert.ToDateTime(Convert.ToDateTime(loadingDate).ToString(Constants.AzureDateFormat));
                //DateTime serverDate = _iCommon.ServerDateTime;
                //if (frmDate == DateTime.MinValue)
                //    frmDate = serverDate;

                return _iTblLoadingSlipBL.SelectAllLoadingListByVehicleNo(vehicleNo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Sudhir
        [Route("GetLoadingTODetailsByLoadingSlipNoForSupport")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetLoadingTODetailsByLoadingSlipNoForSupport(String loadingSlipNo)
        {
            return _iTblLoadingSlipBL.SelectLoadingTOWithDetailsByLoadingSlipIdForSupport(loadingSlipNo);
        }


        [Route("GetLoadSlipExtValues")]
        [HttpPost]
        public ResultMessage GetLoadSlipExtValues([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingSlipTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "LoadingSlipList Found NULL";
                    return resultMessage;
                }

                return _iTblLoadingBL.CalculateLoadingValuesRate(tblLoadingTO);

            }
            catch (Exception ex)
            {
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception in API Call";
                return resultMessage;
            }
        }


        /// <summary>
        /// Vijaymala[24-04-2018] added : to get loading details by using booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [Route("GetLoadingTODetailsByBookingId")]
        [HttpGet]
        public List<TblLoadingTO> GetLoadingTODetailsByBookingId(Int32 bookingId)
        {
            return _iTblLoadingBL.SelectAllTblLoadingByBookingId(bookingId);
        }

        /// <summary>
        /// Vijaymala [08-05-2018] added to get notified loading list withiin period 
        /// </summary>
        /// <returns></returns>
        [Route("GetAllNotifiedLoadingList")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetAllNotifiedLoadingList(string fromDate, String toDate,Int32 callFlag)
        {
            try
            {
                DateTime frmDate = DateTime.MinValue;
                DateTime tDate = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                    frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

                DateTime serverDate = _iCommon.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate.Date;
                if (tDate == DateTime.MinValue)
                    tDate = serverDate.Date;


                return _iTblLoadingSlipBL.SelectAllNotifiedTblLoadingList(frmDate, tDate, callFlag);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Vijaymala[19-09-2018] added to get loading slip list by using booking
        /// </summary>
        /// <returns></returns>
        [Route("GetLoadingslipDetailsByBooking")]
        [HttpGet]
        public TblLoadingTO GetLoadingslipDetailsByBooking(String bookingIdsList,String scheduleIdsList)
        {
            //List<Int32> bookingIdsList = JsonConvert.DeserializeObject<List<Int32>>(data["bookingIdsList"].ToString());
            //List<Int32> scheduleIdsList = JsonConvert.DeserializeObject<List<Int32>>(data["scheduleIdsList"].ToString());
            
            return _iTblLoadingBL.SelectLoadingTOWithDetailsByBooking(bookingIdsList,scheduleIdsList);
        }

        //Aniket[21 - 8 - 2019]
        [Route("RemoveVehOutDatFromIotDevice")]
        [HttpGet]
        public ResultMessage RemoveVehOutDatFromIotDevice()
        {
            return _iTblLoadingBL.RemoveDatFromIotDevice();
        }
        #endregion

        #region Post




        [Route("PostNewLoadingSlip")]
        [HttpPost]
        public ResultMessage PostNewLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingSlipTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "LoadingSlipList Found NULL";
                    return resultMessage;
                }


                tblLoadingTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;
                tblLoadingTO.StatusDate = _iCommon.ServerDateTime;
                tblLoadingTO.CreatedOn = _iCommon.ServerDateTime;
                tblLoadingTO.StatusReason = "New - Considered For Loading";
                tblLoadingTO.ParentLoadingId = tblLoadingTO.IdLoading;

                return _iTblLoadingBL.SaveNewLoadingSlip(tblLoadingTO);

            }
            catch (Exception ex)
            {
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception in API Call";
                return resultMessage;
            }
        }


        [Route("PostDeliverySlipConfirmations")]
        [HttpPost]
        public ResultMessage PostDeliverySlipConfirmations([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }


                tblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingTO.StatusDate = _iCommon.ServerDateTime;
                tblLoadingTO.UpdatedOn = tblLoadingTO.StatusDate;

                if (tblLoadingTO.IsRestorePrevStatus == 0)
                    return _iTblLoadingBL.UpdateDeliverySlipConfirmations(tblLoadingTO);
                else
                    return _iTblLoadingBL.RestorePreviousStatusForLoading(tblLoadingTO);


            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        [Route("PostLoadingVehDocExtDetails")]
        [HttpPost]
        public ResultMessage PostLoadingVehDocExtDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }


                tblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingTO.StatusDate = _iCommon.ServerDateTime;
                tblLoadingTO.UpdatedOn = tblLoadingTO.StatusDate;

                return _iTblLoadingBL.InsertLoadingVehDocExtDetailsAgainstLoading(tblLoadingTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        /// <summary>
        /// GJ@20170829 : Delivery slip Loading completion confirmation for multiple Loading Slip
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeliverySlipListConfirmations")]
        [HttpPost]
        public ResultMessage PostDeliverySlipListConfirmations([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblLoadingTO> tblLoadingTOList = JsonConvert.DeserializeObject<List<TblLoadingTO>>(data["loadingTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTOList == null || tblLoadingTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("tblLoadingTOList Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                foreach (var tblLoadingTO in tblLoadingTOList)
                {
                    tblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblLoadingTO.StatusDate = _iCommon.ServerDateTime;
                    tblLoadingTO.UpdatedOn = tblLoadingTO.StatusDate;

                    if (tblLoadingTO.IsRestorePrevStatus == 0)
                        _iTblLoadingBL.UpdateDeliverySlipConfirmations(tblLoadingTO);
                    else
                        _iTblLoadingBL.RestorePreviousStatusForLoading(tblLoadingTO);
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeliverySlipListConfirmations");
                return resultMessage;
            }
        }


        [Route("PostLoadingSuperwisorDtl")]
        [HttpPost]
        public ResultMessage PostLoadingSuperwisorDtl([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error; 
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                return _iTblLoadingBL.AllocateSuperwisor(tblLoadingTO, loginUserId);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

     
        [Route("PostLoadingConfiguration")]
        [HttpPost]
        public ResultMessage PostLoadingConfiguration([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                List<TblLoadingQuotaConfigTO> loadingQuotaConfigTOList = JsonConvert.DeserializeObject<List<TblLoadingQuotaConfigTO>>(data["loadingQuotaConfigList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingQuotaConfigTOList == null || loadingQuotaConfigTOList.Count == 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "loadingQuotaConfigTOList Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;

                for (int i = 0; i < loadingQuotaConfigTOList.Count; i++)
                {
                    loadingQuotaConfigTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    loadingQuotaConfigTOList[i].CreatedOn = createdDate;
                }

                return _iTblLoadingQuotaConfigBL.SaveNewLoadingQuotaConfiguration(loadingQuotaConfigTOList);
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostLoadingConfiguration";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }

        [Route("PostLoadingQuotaDeclaration")]
        [HttpPost]
        public ResultMessage PostLoadingQuotaDeclaration([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                List<TblLoadingQuotaDeclarationTO> loadingQuotaDeclarationTOList = JsonConvert.DeserializeObject<List<TblLoadingQuotaDeclarationTO>>(data["loadingQuotaDeclarationTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingQuotaDeclarationTOList == null || loadingQuotaDeclarationTOList.Count == 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "loadingQuotaDeclarationTOList Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                Boolean isQuotaDeclared = _iTblLoadingQuotaDeclarationBL.IsLoadingQuotaDeclaredForTheDate(createdDate.Date, loadingQuotaDeclarationTOList[0].ProdCatId, loadingQuotaDeclarationTOList[0].ProdSpecId);
                if (isQuotaDeclared)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Loading Quota Is Already Declared For The Day :" + createdDate.Date.ToString(Constants.DefaultDateFormat);
                    rMessage.Result = 0;
                    return rMessage;
                }


                for (int i = 0; i < loadingQuotaDeclarationTOList.Count; i++)
                {
                    loadingQuotaDeclarationTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    loadingQuotaDeclarationTOList[i].CreatedOn = createdDate;
                    loadingQuotaDeclarationTOList[i].IsActive = 1;
                }

                rMessage = _iTblLoadingQuotaDeclarationBL.SaveLoadingQuotaDeclaration(loadingQuotaDeclarationTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostLoadingQuotaDeclaration";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }


        [Route("PostLoadingQuotaTransferNotes")]
        [HttpPost]
        public ResultMessage PostLoadingQuotaTransferNotes([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                List<TblLoadingQuotaTransferTO> loadingQuotaTransferTOList = JsonConvert.DeserializeObject<List<TblLoadingQuotaTransferTO>>(data["loadingQuotaTransferTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingQuotaTransferTOList == null || loadingQuotaTransferTOList.Count == 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "loadingQuotaTransferTOList Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;

                for (int i = 0; i < loadingQuotaTransferTOList.Count; i++)
                {
                    loadingQuotaTransferTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    loadingQuotaTransferTOList[i].CreatedOn = createdDate;
                }

                rMessage = _iTblLoadingQuotaTransferBL.SaveLoadingQuotaTransferNotes(loadingQuotaTransferTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostLoadingQuotaTransferNotes";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }

        [Route("PostMaterailTransferNotes")]
        [HttpPost]
        public ResultMessage PostMaterailTransferNotes([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                List<TblLoadingQuotaTransferTO> loadingQuotaTransferTOList = JsonConvert.DeserializeObject<List<TblLoadingQuotaTransferTO>>(data["loadingQuotaTransferTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingQuotaTransferTOList == null || loadingQuotaTransferTOList.Count == 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "loadingQuotaTransferTOList Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;

                for (int i = 0; i < loadingQuotaTransferTOList.Count; i++)
                {
                    loadingQuotaTransferTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    loadingQuotaTransferTOList[i].CreatedOn = createdDate;
                }

                rMessage = _iTblLoadingQuotaTransferBL.SaveMaterialTransferNotes(loadingQuotaTransferTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostLoadingQuotaTransferNotes";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }


        [Route("PostCancelNotConfirmLoadings")]
        [HttpGet]
        public ResultMessage PostCancelNotConfirmLoadings()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return _iTblLoadingBL.CancelAllNotConfirmedLoadingSlips();
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        [Route("PostAllowedLoadingTime")]
        [HttpPost]
        public ResultMessage PostAllowedLoadingTime([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                TblLoadingAllowedTimeTO loadingAllowedTimeTO = JsonConvert.DeserializeObject<TblLoadingAllowedTimeTO>(data["loadingAllowedTimeTO"].ToString());
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
                    rMessage.Text = "loadingAllowedTimeTO Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                loadingAllowedTimeTO.CreatedBy = Convert.ToInt32(loginUserId);
                loadingAllowedTimeTO.CreatedOn = createdDate;

                loadingAllowedTimeTO.AllowedLoadingTime = Convert.ToDateTime(loadingAllowedTimeTO.ExtendedHrs);

                //int result= _iTblLoadingAllowedTimeBL.InsertTblLoadingAllowedTime(loadingAllowedTimeTO);
                //if(result==1)
                //{
                //    rMessage.MessageType = ResultMessageE.Information;
                //    rMessage.Text = "Loading Time Saved Successfully";
                //    rMessage.Result = 1;
                //    return rMessage;
                //}

                //rMessage.MessageType = ResultMessageE.Error;
                //rMessage.Text = "Error while  InsertTblLoadingAllowedTime: PostAllowedLoadingTime";
                //rMessage.Result = 0;
                //return rMessage;
                return _iTblLoadingAllowedTimeBL.SaveAllowedLoadingTime(loadingAllowedTimeTO);
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

        /// <summary>
        /// GJ@20170824 : API to populate the Loading Slip status changes cycle automatically
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeliverySlipConfirmationsCycleAuto")]
        [HttpPost]
        public ResultMessage PostDeliverySlipConfirmationsCycleAuto([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS);
                string autoCancelCycleStatusIds = tblConfigParamsTO.ConfigParamVal;
                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM)
                {
                    TblLoadingTO existingTblLoadingTO = _iTblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading);
                    if (existingTblLoadingTO == null)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "existingTblLoadingTO Found NULL";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                    existingTblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    existingTblLoadingTO.StatusDate = _iCommon.ServerDateTime;
                    existingTblLoadingTO.UpdatedOn = _iCommon.ServerDateTime;
                    if (autoCancelCycleStatusIds != null && autoCancelCycleStatusIds != "")
                    {
                        string[] statusIds = autoCancelCycleStatusIds.Split(',');
                        foreach (var statusId in statusIds)
                        {
                            existingTblLoadingTO.StatusId = Convert.ToInt32(statusId);
                            existingTblLoadingTO.SuperwisorId = tblLoadingTO.SuperwisorId;
                            DimStatusTO dimStatusTO = _iDimStatusBL.SelectDimStatusTO(existingTblLoadingTO.StatusId);
                            if(dimStatusTO != null)
                            {
                                existingTblLoadingTO.StatusReason = dimStatusTO.StatusDesc;
                            }
                            // tblLoadingTO.TranStatusE = Convert.ToInt32(statusId);
                            resultMessage = _iTblLoadingBL.UpdateDeliverySlipConfirmations(existingTblLoadingTO);
                        }

                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Text = "Record Updated Sucessfully";
                        resultMessage.Result = 1;
                        return resultMessage;
                    }
                }
                //return _iTblLoadingBL.UpdateDeliverySlipConfirmations(tblLoadingTO);              


            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
            return resultMessage;
        }

        [Route("PostLoadingSlipUpdate")]
        [HttpPost]
        public ResultMessage PostLoadingSlipUpdate([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblLoadingTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }
                TblLoadingTO existingTblLoadingTO = _iTblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading);
                if (existingTblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "existingTblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                //Added By Kiran For avoid to change old value 14/03/19
                int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();
                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                {
                    tblLoadingTO.VehicleNo = existingTblLoadingTO.VehicleNo;
                    tblLoadingTO.StatusId = existingTblLoadingTO.StatusId;
                    tblLoadingTO.TransporterOrgId = existingTblLoadingTO.TransporterOrgId;
                }

                //TblLoadingTO existingTblLoadingTO = _iTblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading);
                //if (existingTblLoadingTO == null)
                //{
                //    resultMessage.MessageType = ResultMessageE.Error;
                //    resultMessage.Text = "existingTblLoadingTO Found NULL";
                //    resultMessage.Result = 0;
                //    return resultMessage;
                //}
                //existingTblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //existingTblLoadingTO.UpdatedOn = _iCommon.ServerDateTime;
                //existingTblLoadingTO.CallFlag = tblLoadingTO.CallFlag;
                //existingTblLoadingTO.FlagUpdatedOn = _iCommon.ServerDateTime;
                tblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingTO.UpdatedOn = _iCommon.ServerDateTime;
                int result = _iTblLoadingBL.UpdateTblLoading(tblLoadingTO);
                if (result == 1)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblLoading");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostLoadingSlipUpdate");
                return resultMessage;
            }
        }

        /// <summary>
        /// Priyanka [15-05-2018]: Added to change loading slip confirmation status.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostChangeLoadSlipConfirmationStatus")]
        [HttpPost]
        public ResultMessage PostChangeLoadSlipConfirmationStatus([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                //var loadingSlipId = data["loadingSlipId"].ToString();
                var loginUserId = data["loginUserId"].ToString();
                TblLoadingSlipTO loadingSlipTO = JsonConvert.DeserializeObject<TblLoadingSlipTO>(data["loadingSlipTo"].ToString());

                //TblLoadingSlipTO loadingSlipTO = BL._iTblLoadingSlipBL.SelectTblLoadingSlipTO(Convert.ToInt32(loadingSlipId));
                if (loadingSlipTO == null)
                {
                    resultMessage.DefaultBehaviour("loadingSlipTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                return _iTblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(loadingSlipTO, Convert.ToInt32(loginUserId));
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostChangeLoadSlipConfirmationStatus");
                return resultMessage;
            }
        }


        /// <summary>
        ///  Vaibhav [13-Sep-2017] API to insert Unloading Slip Details
        /// </summary>
        /// <param name="value"></param>
        [Route("PostNewUnLoadingSlip")]
        [HttpPost]
        public ResultMessage PostNewUnLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblUnLoadingTO tblUnLoadingTO = JsonConvert.DeserializeObject<TblUnLoadingTO>(data["UnLoadingTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblUnLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblUnLoadingTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                tblUnLoadingTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblUnLoadingTO.CreatedOn = _iCommon.ServerDateTime;
                tblUnLoadingTO.StatusId = Convert.ToInt32(Constants.TranStatusE.UNLOADING_NEW);
                tblUnLoadingTO.StatusDate = _iCommon.ServerDateTime;
                tblUnLoadingTO.Status = Constants.TranStatusE.UNLOADING_NEW.ToString();

                return _iTblUnLoadingBL.SaveNewUnLoadingSlipDetails(tblUnLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewUnLoadingSlip");
                return resultMessage;
            }
        }

        /// <summary>
        /// GJ@20170917 : API to Update the UnLoading Slip Confirmations
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateUnloadingById")]
        [HttpPost]
        public ResultMessage PostUpdateUnloadingById([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            int result = 0;
            try
            {
                TblUnLoadingTO tblUnLoadingTO = JsonConvert.DeserializeObject<TblUnLoadingTO>(data["UnLoadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblUnLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblUnLoadingTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                tblUnLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblUnLoadingTO.StatusDate = _iCommon.ServerDateTime;
                tblUnLoadingTO.UpdatedOn = tblUnLoadingTO.StatusDate;

                result = _iTblUnLoadingBL.UpdateTblUnLoading(tblUnLoadingTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Server error : Record not updated");
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateUnloadingById");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [20-Sep-2017] Added to get unloading standard desc list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewUnloadingDescription")]
        [HttpPost]
        public ResultMessage PostNewUnloadingDescription([FromBody] JObject data)
        {

            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblUnloadingStandDescTO> UnloadingStandDescTOList = JsonConvert.DeserializeObject<List<TblUnloadingStandDescTO>>(data["UnloadingStandDescTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                int result = 0;
                if (UnloadingStandDescTOList == null)
                {
                    resultMessage.DefaultBehaviour("UnloadingStandDescTOList Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                foreach (var UnloadingStandDescTO in UnloadingStandDescTOList)
                {
                    UnloadingStandDescTO.IsActive = 1;
                    result = _iTblUnloadingStandDescBL.InsertTblUnloadingStandDesc(UnloadingStandDescTO);
                }
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewUnloadingDescription");
                return resultMessage;
            }

        }

        /// <summary>
        /// Vaibhav [20-Sep-2017] Added to Update unloading standard desc
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateUnloadingDescription")]
        [HttpPost]
        public ResultMessage PostUpdateUnloadingDescription([FromBody] JObject data)
        {

            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblUnloadingStandDescTO UnloadingStandDescTO = JsonConvert.DeserializeObject<TblUnloadingStandDescTO>(data["UnloadingStandDescTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (UnloadingStandDescTO == null)
                {
                    resultMessage.DefaultBehaviour("UnloadingStandDescTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }


                UnloadingStandDescTO.IsActive = 1;
                int result = _iTblUnloadingStandDescBL.UpdateTblUnloadingStandDesc(UnloadingStandDescTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateUnloadingDescription");
                return resultMessage;
            }

        }

        /// <summary>
        /// Vaibhav [20-Sep-2017] Deactivate Unloading Description
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeactivateUnloadingDescription")]
        [HttpPost]
        public ResultMessage PostDeactivateUnloadingDescription([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblUnloadingStandDescTO UnloadingStandDescTO = JsonConvert.DeserializeObject<TblUnloadingStandDescTO>(data["UnloadingStandDescTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (UnloadingStandDescTO == null)
                {
                    resultMessage.DefaultBehaviour("UnloadingStandDescTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }


                UnloadingStandDescTO.IsActive = 0;

                int result = _iTblUnloadingStandDescBL.UpdateTblUnloadingStandDesc(UnloadingStandDescTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be updated");
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateUnloadingDescription");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [12-Oct-2017] added to deactivate unloading slip
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostDeactivateUnLoadingSlip")]
        [HttpPost]
        public ResultMessage PostDeactivateUnLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblUnLoadingTO tblUnLoadingTO = JsonConvert.DeserializeObject<TblUnLoadingTO>(data["UnLoadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (tblUnLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblUnLoadingTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                return _iTblUnLoadingBL.DeactivateUnLoadingSlip(tblUnLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateUnLoadingSlip");
                return resultMessage;
            }
        }

        /// <summary>
        /// GJ@20171107 : Remove the IsAllow condition on Weighment screen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// 
        [Route("PostRemoveIsAllowOneMoreLoading")]
        [HttpPost]
        public ResultMessage PostRemoveIsAllowOneMoreLoading([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblLoadingTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }
                return _iTblLoadingBL.removeIsAllowOneMoreLoading(tblLoadingTO, Convert.ToInt32(loginUserId));               
              
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostLoadingSlipUpdate");
                return resultMessage;
            }
        }
        /// <summary>
        /// KiranM @ Added new transport Slip 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewTransoprtSlip")]
        [HttpPost]
        public ResultMessage PostNewTransoprtSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblTransportSlipTO transportSlipTO = JsonConvert.DeserializeObject<TblTransportSlipTO>(data["TransportSlipTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (transportSlipTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "TransportSlipTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                transportSlipTO.CreatedBy = Convert.ToInt32(loginUserId);
                transportSlipTO.CreatedOn = _iCommon.ServerDateTime;
                //transportSlipTO.UpdatedOn= _iCommon.ServerDateTime;

                return _iTblTransportSlipBL.SaveNewtransportSlip(transportSlipTO);

            }
            catch (Exception ex)
            {
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception in API Call";
                return resultMessage;
            }
        }


        /// <summary>
        /// KiranM @ Added new transport Slip 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateTransoprtSlip")]
        [HttpPost]
        public ResultMessage PostUpdateTransoprtSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblTransportSlipTO transportSlipTO = JsonConvert.DeserializeObject<TblTransportSlipTO>(data["TransportSlipTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (transportSlipTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "TransportSlipTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                transportSlipTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //transportSlipTO.CreatedOn = _iCommon.ServerDateTime;
                transportSlipTO.UpdatedOn= _iCommon.ServerDateTime;

                return _iTblTransportSlipBL.UpdateNewtransportSlip(transportSlipTO);

            }
            catch (Exception ex)
            {
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception in API Call";
                return resultMessage;
            }
        }


        /// <summary>
        /// Kiran [12-12-2017] to update loading transportatin details from trasport slip
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateLoadingTransportDetails")]
        [HttpPost]
        public ResultMessage PostUpdateLoadingTransportDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblTransportSlipTO transportSlipTO = JsonConvert.DeserializeObject<TblTransportSlipTO>(data["TransportSlipTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (transportSlipTO == null)
                {
                    resultMessage.DefaultBehaviour("transportSlipTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                transportSlipTO.UpdatedBy = Convert.ToInt32(loginUserId);
                transportSlipTO.UpdatedOn = _iCommon.ServerDateTime;

                return _iTblLoadingBL.UpdateLoadingTransportDetails(transportSlipTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateLoadingTransportDetails");
                return resultMessage;
            }
        }

        /// <summary>
        /// Priyanka [18-04-2018] : Added to update the Vehicle Number in delivery slip.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateVehicleNo")]
        [HttpPost]
        public ResultMessage PostUpdateVehicleNo([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblLoadingTO LoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["LoadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (LoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("transportSlipTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                LoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                LoadingTO.UpdatedOn = _iCommon.ServerDateTime;

                return _iTblLoadingBL.UpdateVehicleDetails(LoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateVehicleNo");
                return resultMessage;
            }
        }
        /// <summary>
        /// Priyanka [19-04-2018] : Added to remove the item whose weight is not taken.(Edit Loading Slip)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostRemoveLoadingItem")]
        [HttpPost]
        public ResultMessage PostRemoveLoadingItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblLoadingSlipExtTO tblLoadingSlipExtTO = JsonConvert.DeserializeObject<TblLoadingSlipExtTO>(data["TblLoadingSlipExtTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingSlipExtTO == null)
                {
                    resultMessage.DefaultBehaviour("tblLoadingSlipExtTO Found NULL");
                    return resultMessage;
                }

                //Check if loading is completed or not against the vehicle number in loading slip.
                TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
                tblLoadingSlipTO = _iTblLoadingSlipBL.SelectTblLoadingSlipTO(tblLoadingSlipExtTO.LoadingSlipId);
                if(tblLoadingSlipTO == null)
                {
                    resultMessage.DefaultBehaviour("tblLoadingSlipTO Found Null");
                    return resultMessage;
                }
                
                if(tblLoadingSlipTO.StatusId == (int)Constants.TranStatusE.LOADING_COMPLETED)
                {
                    resultMessage.DefaultBehaviour("Loading Against Vehicle Is Already Completed");
                    return resultMessage;
                }
                
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                tblLoadingSlipExtTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingSlipExtTO.UpdatedOn = _iCommon.ServerDateTime;

                return _iTblLoadingBL.RemoveItemFromLoadingSlip(tblLoadingSlipExtTO, Convert.ToInt32(loginUserId));
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostRemoveLoadingItem");
                return resultMessage;
            }
        }


        /// <summary>
        /// Saket [2018-04-26] Added to Add/Edit item in loading slip.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostAddLoadingItem")]
        [HttpPost]
        public ResultMessage PostAddLoadingItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblLoadingSlipExtTO tblLoadingSlipExtTO = JsonConvert.DeserializeObject<TblLoadingSlipExtTO>(data["TblLoadingSlipExtTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingSlipExtTO == null)
                {
                    resultMessage.DefaultBehaviour("tblLoadingSlipExtTO Found NULL");
                    return resultMessage;
                }

                //Check if loading is completed or not against the vehicle number in loading slip.
                TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
                tblLoadingSlipTO = _iTblLoadingSlipBL.SelectTblLoadingSlipTO(tblLoadingSlipExtTO.LoadingSlipId);
                if (tblLoadingSlipTO == null)
                {
                    resultMessage.DefaultBehaviour("tblLoadingSlipTO Found Null");
                    return resultMessage;
                }

                if (tblLoadingSlipTO.StatusId == (int)Constants.TranStatusE.LOADING_COMPLETED)
                {
                    resultMessage.DefaultBehaviour("Loading Against Vehicle Is Already Completed");
                    return resultMessage;
                }


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                tblLoadingSlipExtTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingSlipExtTO.UpdatedOn = _iCommon.ServerDateTime;

                return _iTblLoadingBL.AddItemInLoadingSlip(tblLoadingSlipExtTO, Convert.ToInt32(loginUserId));
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostAddLoadingItem");
                return resultMessage;
            }
        }


        /// <summary>
        /// Saket [2018-04-26] Added to reverse weighing process.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostReverseWeighing")]
        [HttpPost]
        public ResultMessage PostReverseWeighing([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                Int32 loadingId = JsonConvert.DeserializeObject<Int32>(data["loadingId"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (loadingId == 0)
                {
                    resultMessage.DefaultBehaviour("loadingId == 0");
                    return resultMessage;
                }

                return _iTblLoadingBL.ReverseWeighingProcessAgainstLoading(loadingId, Convert.ToInt32(loginUserId));

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostAddLoadingItem");
                return resultMessage;
            }
        }

        #endregion

        #region Put    


        /// <summary>
        /// Vaibhav [14-Sep-2017] API to update unloading details
        /// </summary>
        /// <param name="id"></param>
        [Route("PutUnLoadingSlip")]
        [HttpPut]
        public ResultMessage PutUnLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblUnLoadingTO tblUnLoadingTO = JsonConvert.DeserializeObject<TblUnLoadingTO>(data[""].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblUnLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblUnLoadingTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                tblUnLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblUnLoadingTO.UpdatedOn = _iCommon.ServerDateTime;
                tblUnLoadingTO.StatusId = Convert.ToInt32(Constants.TranStatusE.LOADING_NEW);
                tblUnLoadingTO.StatusDate = _iCommon.ServerDateTime;

                return _iTblUnLoadingBL.UpdateUnLoadingSlipDetails(tblUnLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PutUnLoadingSlip");
                return resultMessage;
            }
        }

        #endregion

    }
}
