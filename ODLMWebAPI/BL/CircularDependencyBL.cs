using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL
{
    public class CircularDependencyBL : ICircularDependencyBL
    {
        private readonly ITblBookingScheduleDAO _iTblBookingScheduleDAO;
        private readonly ITblBookingDelAddrDAO _iTblBookingDelAddrDAO;
        private readonly ITblBookingExtDAO _iTblBookingExtDAO;
        private readonly ITblLoadingSlipDAO _iTblLoadingSlipDAO;
        private readonly ITblLoadingSlipDtlDAO _iTblLoadingSlipDtlDAO;
        private readonly ITblLoadingSlipExtDAO _iTblLoadingSlipExtDAO;
        private readonly ITblLoadingSlipAddressDAO _iTblLoadingSlipAddressDAO;
        private readonly ITblWeighingMeasuresDAO _iTblWeighingMeasuresDAO;
        private readonly ITblLoadingDAO _iTblLoadingDAO;
        private readonly ITblInvoiceDAO _iTblInvoiceDAO;
        private readonly ITblBookingsDAO _iTblBookingsDAO;
        private readonly ITblStockSummaryDAO _iTblStockSummaryDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblParityDetailsDAO _iTblParityDetailsDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        public CircularDependencyBL(ITblConfigParamsDAO iTblConfigParamsDAO,ITblParityDetailsDAO iTblParityDetailsDAO,ITblLoadingSlipDAO iTblLoadingSlipDAO, IConnectionString iConnectionString, ITblStockSummaryDAO iTblStockSummaryDAO, ITblBookingsDAO iTblBookingsDAO, ITblInvoiceDAO iTblInvoiceDAO, ITblLoadingDAO iTblLoadingDAO, ITblWeighingMeasuresDAO iTblWeighingMeasuresDAO, ITblLoadingSlipAddressDAO iTblLoadingSlipAddressDAO, ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO, ITblBookingScheduleDAO iTblBookingScheduleDAO, ITblBookingDelAddrDAO iTblBookingDelAddrDAO, ITblBookingExtDAO iTblBookingExtDAO)
        {
            _iTblBookingScheduleDAO = iTblBookingScheduleDAO;
            _iTblBookingDelAddrDAO = iTblBookingDelAddrDAO;
            _iTblBookingExtDAO = iTblBookingExtDAO;
            _iTblLoadingSlipDAO = iTblLoadingSlipDAO;
            _iTblLoadingSlipDtlDAO = iTblLoadingSlipDtlDAO;
            _iTblLoadingSlipExtDAO = iTblLoadingSlipExtDAO;
            _iTblLoadingSlipAddressDAO = iTblLoadingSlipAddressDAO;
            _iTblWeighingMeasuresDAO = iTblWeighingMeasuresDAO;
            _iTblLoadingDAO = iTblLoadingDAO;
            _iTblInvoiceDAO = iTblInvoiceDAO;
            _iTblBookingsDAO = iTblBookingsDAO;
            _iTblStockSummaryDAO = iTblStockSummaryDAO;
            _iConnectionString = iConnectionString;
            _iTblParityDetailsDAO = iTblParityDetailsDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }
        public List<TblBookingScheduleTO> SelectBookingScheduleByBookingId(Int32 bookingId)
        { 
            List<TblBookingScheduleTO> list = _iTblBookingScheduleDAO.SelectAllTblBookingScheduleList(bookingId);
            if (list != null && list.Count > 0)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    TblBookingScheduleTO tblBookingScheduleTO = list[k];

                    tblBookingScheduleTO.DeliveryAddressLst = _iTblBookingDelAddrDAO.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);

                    tblBookingScheduleTO.OrderDetailsLst = _iTblBookingExtDAO.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);
                }
            }
            return list;
        }

        public List<TblLoadingSlipTO> SelectAllLoadingSlipListWithDetails(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                List<TblLoadingSlipTO> list = _iTblLoadingSlipDAO.SelectAllTblLoadingSlip(loadingId, conn, tran);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].TblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(list[i].IdLoadingSlip, conn, tran);
                    list[i].LoadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(list[i].IdLoadingSlip, conn, tran);
                    list[i].DeliveryAddressTOList = _iTblLoadingSlipAddressDAO.SelectAllTblLoadingSlipAddress(list[i].IdLoadingSlip, conn, tran);
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId)
        {
            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(loadingId);
            if (tblWeighingMeasuresTOList.Count > 0)
            {
                tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
            }
            return tblWeighingMeasuresTOList;
        }
        public ResultMessage CheckInvoiceNoGeneratedByVehicleNo(string vehicleNo, SqlConnection conn, SqlTransaction tran,int loadingId, Boolean isForOutOnly = false)
        {
            ResultMessage resMessage = new StaticStuff.ResultMessage();
            try
            {
                int weightSourceConfigId= _iTblConfigParamsDAO.IoTSetting();
                List<TblLoadingTO> loadingList = new List<TblLoadingTO>();
                if (isForOutOnly)
                {
                    if (weightSourceConfigId != (Int32)Constants.WeighingDataSourceE.IoT)
                    {
                        loadingList = _iTblLoadingDAO.SelectAllLoadingListByVehicleNoForDelOut(vehicleNo, conn, tran);
                    }
                    else
                    {
                        loadingList = _iTblLoadingDAO.SelectAllLoadingListByVehicleNoForDelOut(loadingId, conn, tran);

                    }
                }
                else
                {
                    if (weightSourceConfigId != (Int32)Constants.WeighingDataSourceE.IoT)
                    {
                        loadingList = _iTblLoadingDAO.SelectAllLoadingListByVehicleNo(vehicleNo, false, 0, conn, tran);
                    }
                    else
                    {
                        loadingList = _iTblLoadingDAO.SelectAllLoadingListByVehicleNo(vehicleNo, false, loadingId, conn, tran);
                    }

                }

                if (loadingList == null || loadingList.Count == 0)
                {
                    resMessage.DefaultBehaviour("Loading To Found Null againest Vehicle No");
                    return resMessage;
                }
                List<TblLoadingSlipTO> loadingSlipTOList = new List<TblLoadingSlipTO>();
                for (int i = 0; i < loadingList.Count; i++)
                {
                    TblLoadingTO loadingEle = loadingList[i];
                    List<TblLoadingSlipTO> loadingSlipTOListById = new List<TblLoadingSlipTO>();
                    loadingSlipTOListById = SelectAllLoadingSlipListWithDetails(loadingEle.IdLoading, conn, tran);
                    if (loadingSlipTOListById == null || loadingSlipTOListById.Count == 0)
                    {
                        resMessage.DefaultBehaviour("Loading Slip List Found Null againest Loading Id");
                        return resMessage;
                    }
                    loadingSlipTOList.AddRange(loadingSlipTOListById);
                }

                if (loadingSlipTOList == null || loadingSlipTOList.Count == 0)
                {
                    resMessage.DefaultBehaviour("Loading Slip List Found Null againest Vehicle No");
                    return resMessage;
                }
                string resultStr = "Invoices are not authorized for selected Vehicle " + vehicleNo + " Pending Loading slips are :  ";
                string LoadingSlipNos = string.Empty;
                for (int i = 0; i < loadingSlipTOList.Count; i++)
                {
                    TblInvoiceTO invoiceTo = new TblInvoiceTO();
                    invoiceTo = _iTblInvoiceDAO.SelectInvoiceTOFromLoadingSlipId(loadingSlipTOList[i].IdLoadingSlip, conn, tran);
                    if (invoiceTo == null || invoiceTo.StatusId != (int)Constants.InvoiceStatusE.AUTHORIZED)
                    {
                        LoadingSlipNos = string.IsNullOrEmpty(LoadingSlipNos) ? loadingSlipTOList[i].LoadingSlipNo : LoadingSlipNos + "," + loadingSlipTOList[i].LoadingSlipNo;
                    }
                }
                if (!string.IsNullOrEmpty(LoadingSlipNos))
                {
                    resMessage.MessageType = ResultMessageE.Error;
                    resMessage.DisplayMessage = resultStr + LoadingSlipNos;
                    resMessage.Text = resMessage.DisplayMessage;
                    resMessage.Result = 0;
                    return resMessage;
                }

                resMessage.DefaultSuccessBehaviour();
                return resMessage;
            }
            catch (Exception ex)
            {
                resMessage.DefaultExceptionBehaviour(ex, "CheckInvoiceNoGeneratedByVehicleNo");
                return resMessage;
            }
        }

        public List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId, SqlConnection conn, SqlTransaction tran)
        {
            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(loadingId, conn, tran);
            if (tblWeighingMeasuresTOList.Count > 0)
            {
                tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
            }
            return tblWeighingMeasuresTOList;
        }

        /// <summary>
        /// Sanjay [2017-03-03] To Get the Details of Given Booking with child details
        /// </summary>
        /// <param name="idBooking"></param>
        /// <returns></returns>
        public TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking)
        {
            try
            {
                TblBookingsTO tblBookingsTO = _iTblBookingsDAO.SelectTblBookings(idBooking);
                // tblBookingsTO.DeliveryAddressLst =_iTblBookingDelAddrBL.SelectAllTblBookingDelAddrList(idBooking);
                //tblBookingsTO.OrderDetailsLst = _iTblBookingExtBL.SelectAllTblBookingExtList(idBooking);

                //[15-12-2017]Vijaymala :Added to  get booking schedule list against booking
                List<TblBookingScheduleTO> tblBookingScheduleTOList = _iTblBookingScheduleDAO.SelectAllTblBookingScheduleList(idBooking);
               

                if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0)
                {
                    for (int i = 0; i < tblBookingScheduleTOList.Count; i++)
                    {

                        TblBookingScheduleTO tblBookingScheduleTO = tblBookingScheduleTOList[i];
                        List<TblBookingExtTO> tblBookingExtTOLst = _iTblBookingExtDAO.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);
                       
                        tblBookingScheduleTO.OrderDetailsLst = tblBookingExtTOLst;
                        List<TblBookingDelAddrTO> tblBookingDelAddrTOLst = _iTblBookingDelAddrDAO.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);
                        tblBookingScheduleTO.DeliveryAddressLst = tblBookingDelAddrTOLst;
                        //Aniket [23-7-2019] added for shree balaji client, add rate + parity while edit booking
                        if(tblBookingsTO.IsItemized==1)
                        {
                            foreach (var item in tblBookingScheduleTO.OrderDetailsLst)
                            {
                                var parityList = _iTblParityDetailsDAO.SelectParityDetailToListOnBooking(item.MaterialId, item.ProdCatId, item.ProdSpecId, item.ProdItemId, item.BrandId, tblBookingsTO.StateId, tblBookingsTO.CreatedOn);
                                if (parityList != null)
                                {
                                    if (tblBookingsTO.IsConfirmed == 1)
                                        item.Rate = item.Rate + parityList[0].ParityAmt;
                                    else
                                        item.Rate = item.Rate + parityList[0].ParityAmt + parityList[0].NonConfParityAmt;
                                }
                            }

                        }
                   

                    }
                }
                tblBookingsTO.BookingScheduleTOLst = tblBookingScheduleTOList;
                return tblBookingsTO;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
