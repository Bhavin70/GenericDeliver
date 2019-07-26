using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{   
    public class TblLoadingSlipBL : ITblLoadingSlipBL
    {
        private readonly ITblLoadingSlipDAO _iTblLoadingSlipDAO;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblStockConfigDAO _iTblStockConfigDAO;
        private readonly ITblLoadingSlipDtlDAO _iTblLoadingSlipDtlDAO;
        private readonly ITblLoadingSlipExtDAO _iTblLoadingSlipExtDAO;
        private readonly ITblLoadingSlipAddressBL _iTblLoadingSlipAddressBL;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITempLoadingSlipInvoiceDAO _iTempLoadingSlipInvoiceDAO;
        private readonly ITblLoadingStatusHistoryBL _iTblLoadingStatusHistoryBL;
        private readonly ITblBookingsDAO _iTblBookingsDAO;
        private readonly ITblLoadingDAO _iTblLoadingDAO;
        private readonly ITblLoadingSlipExtHistoryDAO _iTblLoadingSlipExtHistoryDAO;
        private readonly ITblAddressDAO _iTblAddressDAO;
        private readonly ITblParityDetailsBL _iTblParityDetailsBL;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblPaymentTermOptionRelationDAO _iTblPaymentTermOptionRelationDAO;
        private readonly ICommon _iCommon;
        public TblLoadingSlipBL(ITblPaymentTermOptionRelationDAO iTblPaymentTermOptionRelationDAO, ITblLoadingDAO iTblLoadingDAO, IDimensionDAO iDimensionDAO, ITblParityDetailsBL iTblParityDetailsBL, ITblAddressDAO iTblAddressDAO, ITblLoadingSlipExtHistoryDAO iTblLoadingSlipExtHistoryDAO, ITblBookingsDAO iTblBookingsDAO, ITblLoadingStatusHistoryBL iTblLoadingStatusHistoryBL, ITempLoadingSlipInvoiceDAO iTempLoadingSlipInvoiceDAO, ITblUserRoleBL iTblUserRoleBL, ITblLoadingSlipAddressBL iTblLoadingSlipAddressBL, ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO, ITblStockConfigDAO iTblStockConfigDAO, ITblConfigParamsBL iTblConfigParamsBL, ITblLoadingSlipDAO iTblLoadingSlipDAO, ICommon iCommon, IConnectionString iConnectionString)
        {
            _iTblLoadingSlipDAO = iTblLoadingSlipDAO;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblStockConfigDAO = iTblStockConfigDAO;
            _iTblLoadingSlipDtlDAO = iTblLoadingSlipDtlDAO;
            _iTblLoadingSlipExtDAO = iTblLoadingSlipExtDAO;
            _iTblLoadingSlipAddressBL = iTblLoadingSlipAddressBL;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTempLoadingSlipInvoiceDAO = iTempLoadingSlipInvoiceDAO;
            _iTblLoadingStatusHistoryBL = iTblLoadingStatusHistoryBL;
            _iTblBookingsDAO = iTblBookingsDAO;
            _iTblLoadingDAO = iTblLoadingDAO;
            _iTblLoadingSlipExtHistoryDAO = iTblLoadingSlipExtHistoryDAO;
            _iTblAddressDAO = iTblAddressDAO;
            _iTblParityDetailsBL = iTblParityDetailsBL;
            _iDimensionDAO = iDimensionDAO;
            _iConnectionString = iConnectionString;
            _iTblPaymentTermOptionRelationDAO = iTblPaymentTermOptionRelationDAO;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblLoadingSlipTO> SelectAllTblLoadingSlipList()
        {
           return  _iTblLoadingSlipDAO.SelectAllTblLoadingSlip();
           
        }
        
        public List<TblLoadingSlipTO> SelectAllLoadingSlipListWithDetails(Int32 loadingId)
        {
            try
            {
                Int32 isConsolidateStk = _iTblConfigParamsBL.GetStockConfigIsConsolidate();
                List<TblStockConfigTO> tblStockConfigTOList = _iTblStockConfigDAO.SelectAllTblStockConfigTOList();

                List<TblLoadingSlipTO> list = _iTblLoadingSlipDAO.SelectAllTblLoadingSlip(loadingId);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].TblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(list[i].IdLoadingSlip);
                    list[i].LoadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(list[i].IdLoadingSlip);
                    list[i].DeliveryAddressTOList = _iTblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(list[i].IdLoadingSlip);
                    list[i].PaymentTermOptionRelationTOLst = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelationByLoadingId(list[i].IdLoadingSlip);

                    //Saket [2018-02-07] Added
                    if (isConsolidateStk == 0)
                    {
                        if (tblStockConfigTOList != null && tblStockConfigTOList.Count > 0)
                        {
                            if (list[i].LoadingSlipExtTOList != null && list[i].LoadingSlipExtTOList.Count > 0)
                            {
                                for (int j = 0; j < list[i].LoadingSlipExtTOList.Count; j++)
                                {
                                    TblLoadingSlipExtTO tblLoadingSlipExtTO = list[i].LoadingSlipExtTOList[j];
                                    TblStockConfigTO tblStockConfigTO = tblStockConfigTOList.Where(w => w.BrandId == tblLoadingSlipExtTO.BrandId
                                                                        && w.MaterialId == tblLoadingSlipExtTO.MaterialId && w.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                                        && w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId && w.IsItemizedStock == 1).FirstOrDefault();
                                    if (tblStockConfigTO != null)  //Get Brought Out Items
                                    {
                                        tblLoadingSlipExtTO.IsWeighingAllow = 1;
                                    }
                                }
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TblLoadingSlipTO> SelectAllLoadingSlipListWithDetails(Int32 loadingId,SqlConnection conn,SqlTransaction tran)
        {
            try
            {
                List<TblLoadingSlipTO> list = _iTblLoadingSlipDAO.SelectAllTblLoadingSlip(loadingId,conn,tran);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].TblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(list[i].IdLoadingSlip,conn,tran);
                    list[i].LoadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(list[i].IdLoadingSlip,conn,tran);
                    list[i].DeliveryAddressTOList = _iTblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(list[i].IdLoadingSlip,conn,tran);

                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //GJ@20171002 : Get the Loading Slip details By Loading Slip id
        public TblLoadingSlipTO SelectAllLoadingSlipWithDetails(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipDAO.SelectTblLoadingSlip(loadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    return null;
                }
                tblLoadingSlipTO.PaymentTermOptionRelationTOLst = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelationByLoadingId(loadingSlipId, conn, tran);
                tblLoadingSlipTO.TblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(loadingSlipId, conn, tran);
                tblLoadingSlipTO.LoadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(loadingSlipId, conn, tran);
                tblLoadingSlipTO.DeliveryAddressTOList = _iTblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(loadingSlipId, conn, tran);
                return tblLoadingSlipTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TblLoadingSlipTO SelectTblLoadingSlipTO(Int32 idLoadingSlip)
        {
            return  _iTblLoadingSlipDAO.SelectTblLoadingSlip(idLoadingSlip);
        }

        public Dictionary<int, string> SelectRegMobileNoDCTForLoadingDealers(String loadingIds, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDAO.SelectRegMobileNoDCTForLoadingDealers(loadingIds, conn, tran);
        }

        public TblLoadingSlipTO SelectAllLoadingSlipWithDetails(Int32 loadingSlipId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
                tblLoadingSlipTO = SelectAllLoadingSlipWithDetails(loadingSlipId, conn, tran);
                if(tblLoadingSlipTO == null)
                {
                    tran.Rollback();
                    return null;
                }
                return tblLoadingSlipTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        //Priyanka [01-06-2018] : Added to get loading slip list.
        public List<TblLoadingSlipTO> SelectAllLoadingSlipList(List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, Int32 isConfirm, Int32 brandId,Int32 superwisorId)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            List<TblLoadingSlipTO> tblLoadingTOSlipList = _iTblLoadingSlipDAO.SelectAllTblLoadingSlipList(tblUserRoleTO, cnfId, loadingStatusId, fromDate, toDate, loadingTypeId, dealerId, isConfirm, brandId, superwisorId);
            return tblLoadingTOSlipList;
        }

        public TblLoadingSlipTO SelectAllLoadingSlipWithDetailsByInvoice(Int32 invoiceId)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblLoadingSlipTO tblLoadingSlipTO = null;

                List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceDAO.SelectTempLoadingSlipInvoiceTOByInvoiceId(invoiceId, conn, tran);

                for (int i = 0; i < tempLoadingSlipInvoiceTOList.Count; i++)
                {
                    TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = tempLoadingSlipInvoiceTOList[i];

                    TblLoadingSlipTO tblLoadingSlipTOTemp = SelectAllLoadingSlipWithDetails(tempLoadingSlipInvoiceTO.LoadingSlipId, conn, tran);

                    if (tblLoadingSlipTO == null)
                    {
                        tblLoadingSlipTO = tblLoadingSlipTOTemp;
                    }
                    else
                    {
                        if (tblLoadingSlipTO.LoadingSlipExtTOList == null)
                        {
                            tblLoadingSlipTO.LoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                        }

                        if (tblLoadingSlipTOTemp.LoadingSlipExtTOList != null && tblLoadingSlipTOTemp.LoadingSlipExtTOList.Count > 0)
                        {
                            tblLoadingSlipTO.LoadingSlipExtTOList.AddRange(tblLoadingSlipTOTemp.LoadingSlipExtTOList);
                        }
                    }

                }


                return tblLoadingSlipTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        //Sudhir
        public List<TblLoadingSlipTO> SelectAllTblLoadingSlipByDate(DateTime fromDate,DateTime toDate, TblUserRoleTO tblUserRoleTO, Int32 cnfId)
        {
            try
            {
                List<TblLoadingSlipTO> tblLoadingSlipTOList = _iTblLoadingSlipDAO.SelectAllTblLoadingSlipByDate(fromDate,toDate, tblUserRoleTO, cnfId);
                
                return tblLoadingSlipTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Sudhir[27-02-2018] Added for Selecting Loading Cycle List
        public List<TblLoadingSlipTO> SelectAllLoadingCycleList(DateTime startDate, DateTime endDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId,Int32 vehicleStatus)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
                {
                    tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
                }

                List<TblLoadingSlipTO> loadingSlipTOList = SelectAllTblLoadingSlipByDate(startDate, endDate, tblUserRoleTO, cnfId);
                foreach (TblLoadingSlipTO tblLoadingSlipTO in loadingSlipTOList)
                {
                    LoadingStatusDateTO loadingStatusDateTO = new LoadingStatusDateTO();
                    tblLoadingSlipTO.LoadingStatusHistoryTOList = _iTblLoadingStatusHistoryBL.SelectAllTblLoadingStatusHistoryList(tblLoadingSlipTO.LoadingId);
                    //tblLoadingSlipTO.LoadingSlipExtTOList = BL._iTblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(tblLoadingSlipTO.IdLoadingSlip);
                    if (tblLoadingSlipTO.LoadingStatusHistoryTOList != null && tblLoadingSlipTO.LoadingStatusHistoryTOList.Count > 0)
                    {
                        for (int i = 0; i < tblLoadingSlipTO.LoadingStatusHistoryTOList.Count; i++)
                        {
                            if(tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING))
                            {
                                loadingStatusDateTO.VehicleReported = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.VehicleReportedStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.VehicleReported - tblLoadingSlipTO.CreatedOn;
                                loadingStatusDateTO.VehicleReportedMin = diff.ToString();
                            }
                            if(tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN))
                            {
                                loadingStatusDateTO.InstructedForIn = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.InstructedForInStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.InstructedForIn - loadingStatusDateTO.VehicleReported;
                                loadingStatusDateTO.InstructedForInMin = diff.ToString();
                            }
                            if (tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_GATE_IN))
                            {
                                loadingStatusDateTO.GateIn = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.GateInStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.GateIn - loadingStatusDateTO.InstructedForIn;
                                loadingStatusDateTO.GateInMin = diff.ToString();
                            }
                            if (tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED))
                            {
                                loadingStatusDateTO.GateOut = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.GateOutStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.GateOut - loadingStatusDateTO.LoadingCompleted;
                                loadingStatusDateTO.GateOutMin = diff.ToString();
                            }
                            if (tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_COMPLETED))
                            {
                                loadingStatusDateTO.LoadingCompleted = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.LoadingCompletedStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.LoadingCompleted - loadingStatusDateTO.GateIn;
                                loadingStatusDateTO.LoadingCompletedMin = diff.ToString();
                            }
                        }
                    }
                    tblLoadingSlipTO.LoadingStatusDateTO = loadingStatusDateTO;
                }
                List<TblLoadingSlipTO> list = new List<TblLoadingSlipTO>();
                List<TblLoadingSlipTO> finalList = new List<TblLoadingSlipTO>();
                if (vehicleStatus == 0) //For All
                {
                    list = loadingSlipTOList.Where(x => x.StatusId != Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED)).ToList();
                    finalList.AddRange(list);
                    finalList.AddRange(loadingSlipTOList.Where(x => x.StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED)).ToList());
                    loadingSlipTOList = finalList;
                }
                else if (vehicleStatus == 1) //For Pending
                {
                    loadingSlipTOList = loadingSlipTOList.Where(x => x.StatusId != Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED)).ToList();

                }
                else if (vehicleStatus == 2)//For Completed
                {
                    loadingSlipTOList = loadingSlipTOList.Where(x => x.StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED)).ToList();
                }

                return loadingSlipTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTempLoading");
                return null;
            }
        }

        //Sudhir-Added For Support
        public List<TblLoadingSlipTO> SelectAllLoadingListByVehicleNo(string vehicleNo)
        {
            return _iTblLoadingSlipDAO.SelectAllLoadingSlipListByVehicleNo(vehicleNo);
        }
        //Sudhir-Added For Support
        public List<TblLoadingSlipTO> SelectLoadingTOWithDetailsByLoadingSlipIdForSupport(String loadingSlipNo)
        {
            return _iTblLoadingSlipDAO.SelectTblLoadingTOByLoadingSlipIdForSupport(loadingSlipNo);
        }

        //Vijaymala-Added [12-04-2018]
        public List<TblLoadingSlipTO> SelectAllLoadingListByVehicleNo(string vehicleNo, DateTime startDate, DateTime endDate)
        {
            return _iTblLoadingSlipDAO.SelectAllLoadingSlipListByVehicleNo(vehicleNo, startDate,endDate);
        }

        //Vijaymala-Added [03-05-2018]added to get loading slip  list by loading id 
        public List<TblLoadingSlipTO> SelectAllTblLoadingSlip(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDAO.SelectAllTblLoadingSlip(loadingId, conn, tran);
        }

        /// <summary>
        /// Vijaymala [08-05-2018] added to get notified loading list withiin period 
        /// </summary>
        /// <returns></returns>
        public List<TblLoadingSlipTO> SelectAllNotifiedTblLoadingList(DateTime fromDate, DateTime toDate,Int32 callFlag)
        {
            return _iTblLoadingSlipDAO.SelectAllNotifiedTblLoadingList(fromDate, toDate, callFlag);
        }

        //Priyanka [11-05-2018] : Added for showing ORC report in loading slip.
        public List<TblORCReportTO> SelectORCReportDetailsList(DateTime fromDate, DateTime toDate, Int32 flag)
        {
            fromDate = Convert.ToDateTime(fromDate);
            toDate = Convert.ToDateTime(toDate);
            return _iTblLoadingSlipDAO.SelectORCReportDetailsList(fromDate, toDate, flag);
        }
        #endregion

        #region Insertion
        public int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            return _iTblLoadingSlipDAO.InsertTblLoadingSlip(tblLoadingSlipTO);
        }

        public int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDAO.InsertTblLoadingSlip(tblLoadingSlipTO, conn, tran);
        }

        

        #endregion
        
        #region Updation
        public int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            return _iTblLoadingSlipDAO.UpdateTblLoadingSlip(tblLoadingSlipTO);
        }

        public int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDAO.UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
        }

        public int UpdateTblLoadingSlip(TblLoadingTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDAO.UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
        }

        public ResultMessage ChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO,Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMessage =ChangeLoadingSlipConfirmationStatus(tblLoadingSlipTO, loginUserId, conn, tran);
                if(resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return null;
                }
                //Priyanka [15-05-2018] added to commit the transaction.
                tran.Commit();         
                return resultMessage;
            }
            catch (Exception ex)
            {
                 resultMessage.DefaultExceptionBehaviour(ex, "ChangeLoadingSlipConfirmationStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
         


        }

        public ResultMessage ChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO, Int32 loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            DateTime serverDate = _iCommon.ServerDateTime;
            try
            {
                
                
                Int32 lastConfirmationStatus = tblLoadingSlipTO.IsConfirmed;
                if (lastConfirmationStatus == 1)
                    tblLoadingSlipTO.IsConfirmed = 0;
                else
                    tblLoadingSlipTO.IsConfirmed = 1;

                List<TblLoadingSlipExtTO> loadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                var stringsArray = loadingSlipExtTOList.Select(i => i.ParityDtlId.ToString()).ToArray();
                var parityDtlIds = string.Join(",", stringsArray);
                List<TblParityDetailsTO> parityDetailsTOList = new List<TblParityDetailsTO>(); //Sudhir[30-APR-2018] Added for The New Parity Logic.

              
                List<TblBookingsTO> bookingList = _iTblBookingsDAO.SelectAllBookingsListFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                TblLoadingTO loadingTO = _iTblLoadingDAO.SelectTblLoading(tblLoadingSlipTO.LoadingId, conn, tran);
                Double freightPerMT = 0;
                //Vijaymala added[26-04-2018]:commented that code to get freight from loading slip 
                if (tblLoadingSlipTO.IsFreightIncluded == 1)//if (loadingTO.IsFreightIncluded == 1)
                {
                    freightPerMT = tblLoadingSlipTO.FreightAmt;  //loadingTO.FreightAmt;
                }

                Double forAmtPerRs = 0;
                //Vijaymala added[21-06-2018]for new For amount calculation
                if (tblLoadingSlipTO.IsForAmountIncluded == 1)
                {

                    if (tblLoadingSlipTO.ForAmount > 0)
                    {
                        forAmtPerRs = tblLoadingSlipTO.ForAmount;
                        freightPerMT = forAmtPerRs - freightPerMT;
                    }
                }

                for (int e = 0; e < tblLoadingSlipTO.LoadingSlipExtTOList.Count; e++)
                {

                    TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[e];
                    TblLoadingSlipExtHistoryTO loadingSlipExtHistoryTO = new TblLoadingSlipExtHistoryTO();

                    //Assign Last Calculated values to History Object
                    loadingSlipExtHistoryTO.CreatedBy = loginUserId;
                    loadingSlipExtHistoryTO.CreatedOn = serverDate;
                    loadingSlipExtHistoryTO.LastConfirmationStatus = lastConfirmationStatus;
                    loadingSlipExtHistoryTO.LastRateCalcDesc = tblLoadingSlipExtTO.RateCalcDesc;
                    loadingSlipExtHistoryTO.LastRatePerMT = tblLoadingSlipExtTO.RatePerMT;
                    loadingSlipExtHistoryTO.ParityDtlId = tblLoadingSlipExtTO.ParityDtlId;
                    loadingSlipExtHistoryTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
                    loadingSlipExtHistoryTO.LastCdAplAmt = tblLoadingSlipExtTO.CdApplicableAmt;

                    if (tblLoadingSlipExtTO.BookingId > 0)
                    {

                    }
                   
                    result = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error : While UpdateTblLoadingSlipExt Against LoadingSlip");
                        return resultMessage;
                    }


                    //Assign New Values and Save the history Record
                    loadingSlipExtHistoryTO.CurrentRatePerMT = tblLoadingSlipExtTO.RatePerMT;
                    loadingSlipExtHistoryTO.CurrentConfirmationStatus = tblLoadingSlipTO.IsConfirmed;
                    loadingSlipExtHistoryTO.CurrentRateCalcDesc = tblLoadingSlipExtTO.RateCalcDesc;
                    loadingSlipExtHistoryTO.CurrentCdAplAmt = tblLoadingSlipExtTO.CdApplicableAmt;

                    result = _iTblLoadingSlipExtHistoryDAO.InsertTblLoadingSlipExtHistory(loadingSlipExtHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error : While InsertTblLoadingSlipExtHistory Against LoadingSlip");
                        return resultMessage;
                    }
                }

                result = UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error : While UpdateTblLoadingSlip");
                    return resultMessage;
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ChangeLoadingSlipConfirmationStatus");
                return resultMessage;
            }
            finally
            {
            }
        }





        public ResultMessage OldChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMessage = ChangeLoadingSlipConfirmationStatus(tblLoadingSlipTO, loginUserId, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return null;
                }
                //Priyanka [15-05-2018] added to commit the transaction.
                tran.Commit();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ChangeLoadingSlipConfirmationStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }



        }

        public ResultMessage OldChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO, Int32 loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            DateTime serverDate = _iCommon.ServerDateTime;
            try
            {


                //Check First Invoice is created against the loading Slip. If created then do not allow to change the status
                //TblInvoiceTO invoiceTO = BL.TblInvoiceBL.SelectInvoiceTOFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                //if (invoiceTO != null)
                //{
                //    resultMessage.DefaultBehaviour("Invoice is already prepared against loadingSlip. Ref Inv Id:" + invoiceTO.IdInvoice);
                //    resultMessage.DisplayMessage = "Hey..Not allowed. Invoice is already prepared against selected loadingSlip";
                //    return resultMessage;
                //}

                Int32 lastConfirmationStatus = tblLoadingSlipTO.IsConfirmed;
                if (lastConfirmationStatus == 1)
                    tblLoadingSlipTO.IsConfirmed = 0;
                else
                    tblLoadingSlipTO.IsConfirmed = 1;

                List<TblLoadingSlipExtTO> loadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                var stringsArray = loadingSlipExtTOList.Select(i => i.ParityDtlId.ToString()).ToArray();
                var parityDtlIds = string.Join(",", stringsArray);
                List<TblParityDetailsTO> parityDetailsTOList = new List<TblParityDetailsTO>(); //Sudhir[30-APR-2018] Added for The New Parity Logic.

                // parityDetailsTOList = BL._iTblParityDetailsBL.SelectAllParityDetailsListByIds(parityDtlIds, conn, tran); Sudhir[30-APR-2018] Commented.

                List<TblBookingsTO> bookingList = _iTblBookingsDAO.SelectAllBookingsListFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                TblLoadingTO loadingTO = _iTblLoadingDAO.SelectTblLoading(tblLoadingSlipTO.LoadingId, conn, tran);
                Double freightPerMT = 0;
                //Vijaymala added[26-04-2018]:commented that code to get freight from loading slip 
                if (tblLoadingSlipTO.IsFreightIncluded == 1)//if (loadingTO.IsFreightIncluded == 1)
                {
                    freightPerMT = tblLoadingSlipTO.FreightAmt;  //loadingTO.FreightAmt;
                }

                Double forAmtPerRs = 0;
                //Vijaymala added[21-06-2018]for new For amount calculation
                if (tblLoadingSlipTO.IsForAmountIncluded == 1)
                {

                    if (tblLoadingSlipTO.ForAmount > 0)
                    {
                        forAmtPerRs = tblLoadingSlipTO.ForAmount;
                        freightPerMT = forAmtPerRs - freightPerMT;
                    }
                }

                for (int e = 0; e < tblLoadingSlipTO.LoadingSlipExtTOList.Count; e++)
                {

                    TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[e];
                    TblLoadingSlipExtHistoryTO loadingSlipExtHistoryTO = new TblLoadingSlipExtHistoryTO();

                    //Assign Last Calculated values to History Object
                    loadingSlipExtHistoryTO.CreatedBy = loginUserId;
                    loadingSlipExtHistoryTO.CreatedOn = serverDate;
                    loadingSlipExtHistoryTO.LastConfirmationStatus = lastConfirmationStatus;
                    loadingSlipExtHistoryTO.LastRateCalcDesc = tblLoadingSlipExtTO.RateCalcDesc;
                    loadingSlipExtHistoryTO.LastRatePerMT = tblLoadingSlipExtTO.RatePerMT;
                    loadingSlipExtHistoryTO.ParityDtlId = tblLoadingSlipExtTO.ParityDtlId;
                    loadingSlipExtHistoryTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
                    loadingSlipExtHistoryTO.LastCdAplAmt = tblLoadingSlipExtTO.CdApplicableAmt;

                    if (tblLoadingSlipExtTO.BookingId > 0)
                    {

                        #region ReCalculate Actual Price From Booking and Parity Settings

                        var tblBookingsTO = bookingList.Where(b => b.IdBooking == tblLoadingSlipExtTO.BookingId).FirstOrDefault();
                        Double orcAmtPerTon = 0;

                        //if (tblBookingsTO.OrcMeasure == "Rs/MT")
                        //{
                        //    orcAmtPerTon = tblBookingsTO.OrcAmt;
                        //}
                        //else
                        //{
                        //    if (tblBookingsTO.OrcAmt > 0)
                        //        orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;
                        //}

                        if (tblLoadingSlipTO.OrcMeasure == "Rs/MT")
                        {
                            orcAmtPerTon = tblLoadingSlipTO.OrcAmt;
                        }
                        else
                        {

                            if (tblLoadingSlipTO.OrcAmt > 0)
                                orcAmtPerTon = tblLoadingSlipTO.OrcAmt / tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty;
                        }

                        String rateCalcDesc = string.Empty;
                        rateCalcDesc = "B.R : " + tblBookingsTO.BookingRate + "|";
                        Double bookingPrice = tblBookingsTO.BookingRate;
                        Double parityAmt = 0;
                        Double priceSetOff = 0;
                        Double paritySettingAmt = 0;
                        Double bvcAmt = 0;
                        //TblParitySummaryTO parityTO = null; Sudhir[30-APR-2018] Commented for New Parity Logic.
                        TblParityDetailsTO parityDtlTO = null;

                        if (true)
                        {
                            //Sudhir[30-APR-2018] Commented for New Parity Logic.
                            /*var parityDtlTO = parityDetailsTOList.Where(m => m.MaterialId == tblLoadingSlipExtTO.MaterialId
                                                    && m.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                    && m.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId).FirstOrDefault();*/

                            //Get Latest To Based On -materialId, Date And Time Check Condition Actual TIme < = First Object.
                            TblAddressTO addrTO = _iTblAddressDAO.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);

                            //SUdhir[30-APR-2018] Added for the Get Parity Details List based on Material Id,ProdCat Id,ProdSpec Id ,State Id ,Brand Id and Booking Date.
                            parityDtlTO = _iTblParityDetailsBL.SelectParityDetailToListOnBooking(tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, addrTO.StateId, tblBookingsTO.BookingDatetime);

                            if (parityDtlTO != null)
                            {
                                parityAmt = parityDtlTO.ParityAmt;
                                if (tblLoadingSlipTO.IsConfirmed != 1)
                                    priceSetOff = parityDtlTO.NonConfParityAmt;
                                else
                                    priceSetOff = 0;

                                tblLoadingSlipExtTO.ParityDtlId = parityDtlTO.IdParityDtl;
                            }
                            else
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : ParityTO Not Found";
                                string mateDesc = tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
                                resultMessage.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
                                return resultMessage;
                            }
                            #region Sudhir[30-APR-2018] Commented for New Parity Logic
                            //parityTO = BL.TblParitySummaryBL.SelectTblParitySummaryTO(parityDtlTO.ParityId, conn, tran);
                            //if (parityTO == null)
                            //{
                            //    resultMessage.DefaultBehaviour();
                            //    resultMessage.Text = "Error : ParityTO Not Found";
                            //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            //    return resultMessage;
                            //}

                            //paritySettingAmt = parityTO.BaseValCorAmt + parityTO.ExpenseAmt + parityTO.OtherAmt;
                            //bvcAmt = parityTO.BaseValCorAmt;
                            //rateCalcDesc += "BVC Amt :" + parityTO.BaseValCorAmt + "|" + "Exp Amt :" + parityTO.ExpenseAmt + "|" + " Other :" + parityTO.OtherAmt + "|";
                            #endregion
                            paritySettingAmt = parityDtlTO.BaseValCorAmt + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                            bvcAmt = parityDtlTO.BaseValCorAmt;
                            rateCalcDesc += "BVC Amt :" + parityDtlTO.BaseValCorAmt + "|" + "Exp Amt :" + parityDtlTO.ExpenseAmt + "|" + " Other :" + parityDtlTO.OtherAmt + "|";
                        }
                        else
                        {
                            resultMessage.DefaultBehaviour();
                            resultMessage.Text = "Error : ParityTO Not Found";
                            resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                            return resultMessage;
                        }

                        Double cdApplicableAmt = (bookingPrice + orcAmtPerTon + parityAmt + priceSetOff + bvcAmt);
                        Double cdAmt = 0;

                        DropDownTO dropDownTO = _iDimensionDAO.SelectCDDropDown(tblLoadingSlipTO.CdStructureId);
                        if (tblLoadingSlipTO.CdStructure > 0)
                        {
                            Int32 isRsValue = Convert.ToInt32(dropDownTO.Text);
                            if (isRsValue == (int)Constants.CdType.IsRs)
                            {

                                cdAmt = tblLoadingSlipTO.CdStructure;
                            }
                            else
                            {

                                cdAmt = (cdApplicableAmt * tblLoadingSlipTO.CdStructure) / 100;
                            }
                        }

                        //if (tblLoadingSlipTO.CdStructure > 0)
                        //    cdAmt = (cdApplicableAmt * tblLoadingSlipTO.CdStructure) / 100;

                        rateCalcDesc += "CD :" + Math.Round(cdAmt, 2) + "|";
                        Double rateAfterCD = cdApplicableAmt - cdAmt;

                        Double gstApplicableAmt = 0;
                        //if (tblLoadingSlipTO.IsConfirmed == 1)
                        //    gstApplicableAmt = rateAfterCD + freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;
                        if (tblLoadingSlipTO.IsConfirmed == 1)
                            gstApplicableAmt = rateAfterCD + freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                        else
                            gstApplicableAmt = rateAfterCD;

                        Double gstAmt = (gstApplicableAmt * 18) / 100;
                        gstAmt = Math.Round(gstAmt, 2);

                        Double finalRate = 0;
                        if (tblLoadingSlipTO.IsConfirmed == 1)
                            finalRate = gstApplicableAmt + gstAmt;
                        //else
                        //    finalRate = gstApplicableAmt + gstAmt + freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;
                        else
                            finalRate = gstApplicableAmt + gstAmt + freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;

                        tblLoadingSlipExtTO.TaxableRateMT = gstApplicableAmt;
                        tblLoadingSlipExtTO.RatePerMT = finalRate;
                        tblLoadingSlipExtTO.CdApplicableAmt = cdApplicableAmt;
                        //tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;
                        tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;

                        rateCalcDesc += " ORC :" + orcAmtPerTon + "|" + " Parity :" + parityAmt + "|" + " NC Amt :" + priceSetOff + "|" + " Freight :" + freightPerMT + "|" + " GST :" + gstAmt + "|";
                        tblLoadingSlipExtTO.RateCalcDesc = rateCalcDesc;
                        #endregion
                    }

                    result = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error : While UpdateTblLoadingSlipExt Against LoadingSlip");
                        return resultMessage;
                    }


                    //Assign New Values and Save the history Record
                    loadingSlipExtHistoryTO.CurrentRatePerMT = tblLoadingSlipExtTO.RatePerMT;
                    loadingSlipExtHistoryTO.CurrentConfirmationStatus = tblLoadingSlipTO.IsConfirmed;
                    loadingSlipExtHistoryTO.CurrentRateCalcDesc = tblLoadingSlipExtTO.RateCalcDesc;
                    loadingSlipExtHistoryTO.CurrentCdAplAmt = tblLoadingSlipExtTO.CdApplicableAmt;

                    result = _iTblLoadingSlipExtHistoryDAO.InsertTblLoadingSlipExtHistory(loadingSlipExtHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error : While InsertTblLoadingSlipExtHistory Against LoadingSlip");
                        return resultMessage;
                    }
                }

                result = UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error : While UpdateTblLoadingSlip");
                    return resultMessage;
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ChangeLoadingSlipConfirmationStatus");
                return resultMessage;
            }
            finally
            {
            }
        }




        #endregion

        #region Deletion
        public int DeleteTblLoadingSlip(Int32 idLoadingSlip)
        {
            return _iTblLoadingSlipDAO.DeleteTblLoadingSlip(idLoadingSlip);
        }

        public int DeleteTblLoadingSlip(Int32 idLoadingSlip, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDAO.DeleteTblLoadingSlip(idLoadingSlip, conn, tran);
        }

        #endregion
        
    }
}
