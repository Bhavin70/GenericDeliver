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
using Newtonsoft.Json;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.DashboardModels;

namespace ODLMWebAPI.BL
{   
    public class TblStockSummaryBL : ITblStockSummaryBL
    {
        private readonly ITblStockSummaryDAO _iTblStockSummaryDAO;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly IDimensionBL _iDimensionBL;
        private readonly ITblStockDetailsBL _iTblStockDetailsBL;
        private readonly ITblProductInfoBL _iTblProductInfoBL;
        private readonly ITblStockConsumptionBL _iTblStockConsumptionBL;
        private readonly ITblBookingsBL _iTblBookingsBL;
        private readonly ITblLoadingSlipExtBL _iTblLoadingSlipExtBL;
        private readonly ITblLoadingQuotaDeclarationBL _iTblLoadingQuotaDeclarationBL;
        private readonly ITblLoadingQuotaDeclarationDAO _iTblLoadingQuotaDeclarationDAO;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblStockSummaryBL(ICommon iCommon, IConnectionString iConnectionString, ITblAlertInstanceBL iTblAlertInstanceBL, ITblLoadingQuotaDeclarationDAO iTblLoadingQuotaDeclarationDAO, ITblLoadingQuotaDeclarationBL iTblLoadingQuotaDeclarationBL, ITblLoadingSlipExtBL iTblLoadingSlipExtBL, ITblBookingsBL iTblBookingsBL, ITblStockConsumptionBL iTblStockConsumptionBL, ITblProductInfoBL iTblProductInfoBL, ITblStockSummaryDAO iTblStockSummaryDAO, ITblConfigParamsBL iTblConfigParamsBL, IDimensionBL iDimensionBL, ITblStockDetailsBL iTblStockDetailsBL)
        {
            _iTblStockSummaryDAO = iTblStockSummaryDAO;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iDimensionBL = iDimensionBL;
            _iTblStockDetailsBL = iTblStockDetailsBL;
            _iTblProductInfoBL = iTblProductInfoBL;
            _iTblStockConsumptionBL = iTblStockConsumptionBL;
            _iTblBookingsBL = iTblBookingsBL;
            _iTblLoadingSlipExtBL = iTblLoadingSlipExtBL;
            _iTblLoadingQuotaDeclarationBL = iTblLoadingQuotaDeclarationBL;
            _iTblLoadingQuotaDeclarationDAO = iTblLoadingQuotaDeclarationDAO;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblStockSummaryTO> SelectAllTblStockSummaryList()
        {
            return _iTblStockSummaryDAO.SelectAllTblStockSummary();
        }

        public String SelectLastStockUpdatedDateTime(Int32 compartmentId, Int32 prodCatId)
        {
            return _iTblStockSummaryDAO.SelectLastStockUpdatedDateTime(compartmentId, prodCatId);
        }

        public TblStockSummaryTO SelectTblStockSummaryTO(Int32 idStockSummary)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblStockSummaryDAO.SelectTblStockSummary(idStockSummary,conn,tran);

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

        public TblStockSummaryTO SelectTblStockSummaryTO(Int32 idStockSummary,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblStockSummaryDAO.SelectTblStockSummary(idStockSummary, conn, tran);

        }

        public TblStockSummaryTO SelectTblStockSummaryTO(DateTime stocDate)
        {
            try
            {
                return _iTblStockSummaryDAO.SelectTblStockSummary(stocDate);
            }
            catch (Exception ex)
            {
                return null;
            }

            //SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            //SqlTransaction tran = null;
            //try
            //{
            //    conn.Open();
            //    tran = conn.BeginTransaction();
            //    return _iTblStockSummaryDAO.SelectTblStockSummary(stocDate, conn, tran);
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
            //finally
            //{
            //    conn.Close();
            //}
        }

        public StockUpdateInfo SelectDashboardStockUpdateInfo(DateTime sysDate)
        {
           
            DashboardModels.StockUpdateInfo stockUpdateInfo = _iTblStockSummaryDAO.SelectDashboardStockUpdateInfo(sysDate);
            if (stockUpdateInfo == null)
            {
                stockUpdateInfo = new DashboardModels.StockUpdateInfo();
            }
            List<DropDownTO> ConsumerTypeList = new List<DropDownTO>();
            TblConfigParamsTO ConsumerTypeConfTo = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELIVER_SKIP_CONSUMER_TYPE_ID_IN_SOLD_STOCK_DISPLAY_ON_DASHBOARD);
            if(ConsumerTypeConfTo != null)
            {
                if (!String.IsNullOrEmpty(ConsumerTypeConfTo.ConfigParamVal))
                {
                    ConsumerTypeList = _iCommon.GetConsumerCategoryList(ConsumerTypeConfTo.ConfigParamVal);
                }
            }
            
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TODAYS_BOOKING_OPENING_BALANCE);

            //SoldStock will be set from ConfigParamVal as per booking type.
            if (tblConfigParamsTO != null && tblConfigParamsTO.ConfigParamVal != null)
            {
                List<PendingQtyOrderTypeTo> PendingQtyWithOrderTypeList = JsonConvert.DeserializeObject<List<PendingQtyOrderTypeTo>>(tblConfigParamsTO.ConfigParamVal);
                stockUpdateInfo.SoldStock = 0;
                if(PendingQtyWithOrderTypeList != null)
                {
                    foreach (PendingQtyOrderTypeTo PendingQtyWithOrderType in PendingQtyWithOrderTypeList)
                    {
                        if(ConsumerTypeList != null && ConsumerTypeList.Count > 0)
                        {
                            var matchTO = ConsumerTypeList.Where(w => w.Text == PendingQtyWithOrderType.ConsumerTypeName).ToList();
                            if(matchTO == null || matchTO.Count == 0)
                            {
                                stockUpdateInfo.SoldStock += PendingQtyWithOrderType.BookingQty;
                            }
                        }
                        else
                        {
                            stockUpdateInfo.SoldStock += PendingQtyWithOrderType.BookingQty;
                        }
                    }
                }
               
                //stockUpdateInfo.SoldStock = Convert.ToDouble(tblConfigParamsTO.ConfigParamVal);

                stockUpdateInfo.PendingQtyWithOrderType = tblConfigParamsTO.ConfigParamVal;

            }

            DateTime yestDate = _iCommon.ServerDateTime.AddDays(-1);
            yestDate = Constants.GetEndDateTime(yestDate);


            //Double bookedQty = stockUpdateInfo.TodaysStock;
            ////Double bookedQty = _iTblBookingsBL.SelectTotalPendingBookingQty(yestDate);

            //stockUpdateInfo.SoldStock = bookedQty;
            
            stockUpdateInfo.UnsoldStock = stockUpdateInfo.TotalSysStock - stockUpdateInfo.SoldStock; 
            //stockUpdateInfo.UnsoldStock = stockUpdateInfo.TodaysStock - stockUpdateInfo.SoldStock;

            return stockUpdateInfo;
        }

        //Aniket
        public StockSummaryTO GetLastStockSummaryDetails()
        {
            return _iTblStockSummaryDAO.GetLastStockSummaryDetails();
        }

        #endregion

        #region Insertion
        public int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO)
        {
            return _iTblStockSummaryDAO.InsertTblStockSummary(tblStockSummaryTO);
        }

        public int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockSummaryDAO.InsertTblStockSummary(tblStockSummaryTO, conn, tran);
        }


        public ResultMessage ResetStockDetails()
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Int32 updateOrCreatedUser = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region consuption details

                String sqlCmd = "DELETE FROM tblStockConsumption";
                result = _iDimensionBL.ExecuteGivenCommand(sqlCmd, conn, tran);
                if (result == -2)
                {
                    throw new Exception("Error while deleteing the stock consumption records");
                }

                sqlCmd = "DBCC CHECKIDENT ('[tblStockConsumption]', RESEED, 0);";
                result = _iDimensionBL.ExecuteGivenCommand(sqlCmd, conn, tran);
                if (result == -2)
                {
                    throw new Exception("Error while deleteing the stock consumption records");
                }

                #endregion

                #region  Stock Details
                sqlCmd = "DELETE FROM tblStockDetails";
                result = _iDimensionBL.ExecuteGivenCommand(sqlCmd, conn, tran);
                if (result == -2)
                {
                    throw new Exception("Error while deleteing the stock detials records");
                }
                sqlCmd = "DBCC CHECKIDENT ('[tblStockDetails]', RESEED, 0);";
                result = _iDimensionBL.ExecuteGivenCommand(sqlCmd, conn, tran);
                if (result == -2)
                {
                    throw new Exception("Error while deleteing the stock detials records");
                }

                #endregion

                #region Stock Summary

                sqlCmd = "DELETE FROM tblStockSummary";
                result = _iDimensionBL.ExecuteGivenCommand(sqlCmd, conn, tran);
                if (result == -2)
                {
                    throw new Exception("Error while deleteing the stock summary records");
                }

                sqlCmd = "DBCC CHECKIDENT ('[tblStockSummary]', RESEED, 0);";
                result = _iDimensionBL.ExecuteGivenCommand(sqlCmd, conn, tran);
                if (result == -2)
                {
                    throw new Exception("Error while deleteing the stock summary records");
                }

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TODAYS_BOOKING_OPENING_BALANCE);
                tblConfigParamsTO.ConfigParamVal = "0";
                result = _iTblConfigParamsBL.UpdateTblConfigParams(tblConfigParamsTO);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error while UpdateTblConfigParams : Method UpdateTblConfigParams";
                    resultMessage.DisplayMessage = "Error.. Records could not be saved";
                    return resultMessage;
                }

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Stock Reset successfully!";
                resultMessage.DisplayMessage = "Stock Reset successfully!";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Delete : ResetStockDetails";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DisplayMessage = "Error.. Records could not be deleted";
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// Sanjay [2017-03-18] To Update Daily Stock Entries. If it is for the first time for that day
        /// then records will be inserted otherwise records will be updated.
        /// </summary>
        /// <param name="tblStockSummaryTO"></param>
        /// <returns></returns>
        public ResultMessage UpdateDailyStock(TblStockSummaryTO tblStockSummaryTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Int32 updateOrCreatedUser = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region Check For Existed Or Not

                //Check if Todays Stock Summary Record is inserted or not
                // If Inserted then its update request else insert request
                DateTime stockDate = _iCommon.ServerDateTime.Date;
                TblStockSummaryTO todaysStockSummaryTO = _iTblStockSummaryDAO.SelectTblStockSummary(new DateTime(), conn, tran);
                if (todaysStockSummaryTO == null)
                {
                    tblStockSummaryTO.StockDate = stockDate;
                    result = InsertTblStockSummary(tblStockSummaryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error While InsertTblStockSummary : MEthod UpdateDailyStock";
                        resultMessage.DisplayMessage = "Error.. Records could not be saved";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }
                else
                {
                    //if (todaysStockSummaryTO.ConfirmedOn != DateTime.MinValue)
                    //{
                    //    tran.Rollback();
                    //    resultMessage.Result = 0;
                    //    resultMessage.Text = "Stock For the Date :" + todaysStockSummaryTO.StockDate.Date.ToString(Constants.DefaultDateFormat) + " is already confirmed. You can not update the stock now";
                    //    resultMessage.DisplayMessage = resultMessage.Text;
                    //    resultMessage.MessageType = ResultMessageE.Error;
                    //    return resultMessage;
                    //}

                    todaysStockSummaryTO.StockDetailsTOList = tblStockSummaryTO.StockDetailsTOList;
                    tblStockSummaryTO = todaysStockSummaryTO;
                }

                #endregion

                #region Based On Step 1 Calculate and Update Otherwise Insert New Records
                // To Compare Against Existing and Update
                List<TblStockDetailsTO> existingStockList = _iTblStockDetailsBL.SelectAllTblStockDetailsList(tblStockSummaryTO.IdStockSummary, conn, tran);

                // For weight and Stock in MT calculations
                List<TblProductInfoTO> productList = _iTblProductInfoBL.SelectAllTblProductInfoList(conn, tran);

                // Insert All New Records
                if (tblStockSummaryTO.StockDetailsTOList != null && tblStockSummaryTO.StockDetailsTOList.Count > 0)
                {
                    updateOrCreatedUser = tblStockSummaryTO.StockDetailsTOList[0].CreatedBy;

                    for (int i = 0; i < tblStockSummaryTO.StockDetailsTOList.Count; i++)
                    {
                        tblStockSummaryTO.StockDetailsTOList[i].StockSummaryId = tblStockSummaryTO.IdStockSummary;

                        Boolean isExist = false;

                        Double existingQtyInMt = 0;
                        Double newQtyInMt = 0;
                        Int32 isInMTExisting = 0;

                        if (existingStockList != null && existingStockList.Count > 0)
                        {

                            TblStockDetailsTO existingStocDtlTO = existingStockList.Where(w => w.IdStockDtl == tblStockSummaryTO.StockDetailsTOList[i].IdStockDtl).FirstOrDefault();

                            //if (tblStockSummaryTO.StockDetailsTOList[i].IsConsolidatedStock == 1)
                            //{
                            //    existingStocDtlTO = existingStockList.Where(w => w.IsConsolidatedStock == 1).FirstOrDefault();
                            //}
                            //else
                            //{
                            //    existingStocDtlTO = existingStockList.Where(e => e.LocationId == tblStockSummaryTO.StockDetailsTOList[i].LocationId
                            //                                                    && e.MaterialId == tblStockSummaryTO.StockDetailsTOList[i].MaterialId
                            //                                                    && e.ProdCatId == tblStockSummaryTO.StockDetailsTOList[i].ProdCatId
                            //                                                    && e.ProdSpecId == tblStockSummaryTO.StockDetailsTOList[i].ProdSpecId
                            //                                                    && e.BrandId == tblStockSummaryTO.StockDetailsTOList[i].BrandId).FirstOrDefault();
                            //}
                            if (existingStocDtlTO != null)
                            {
                                isExist = true;
                                tblStockSummaryTO.StockDetailsTOList[i].IdStockDtl = existingStocDtlTO.IdStockDtl;
                                isInMTExisting = existingStocDtlTO.IsInMT;

                                existingQtyInMt = existingStocDtlTO.BalanceStock;

                            }
                        }

                        if (tblStockSummaryTO.StockDetailsTOList[i].IsInMT == 1)
                        {

                            if (productList != null && productList.Count > 0 && tblStockSummaryTO.StockDetailsTOList[i].MaterialId > 0
                                && tblStockSummaryTO.StockDetailsTOList[i].ProdCatId > 0 && tblStockSummaryTO.StockDetailsTOList[i].ProdSpecId > 0)
                            {


                                var productInfo = productList.Where(p => p.MaterialId == tblStockSummaryTO.StockDetailsTOList[i].MaterialId
                                                                    && p.ProdCatId == tblStockSummaryTO.StockDetailsTOList[i].ProdCatId
                                                                    && p.ProdSpecId == tblStockSummaryTO.StockDetailsTOList[i].ProdSpecId
                                                                    && p.BrandId == tblStockSummaryTO.StockDetailsTOList[i].BrandId).OrderByDescending(d => d.CreatedOn).FirstOrDefault();
                                if (productInfo != null)
                                {

                                    if (tblStockSummaryTO.StockDetailsTOList[i].TotalStock > 0)
                                    {
                                        Double totalStkInMT = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                        totalStkInMT = totalStkInMT * 1000;

                                        Double noOfBundles = 0;
                                        if (productInfo.AvgBundleWt != 0)
                                            noOfBundles = (totalStkInMT) / productInfo.AvgBundleWt;
                                        //Double noOfBundles = Math.Round(totalStkInMT / productInfo.NoOfPcs / productInfo.AvgSecWt / productInfo.StdLength, 2);

                                        tblStockSummaryTO.StockDetailsTOList[i].BalanceStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                        tblStockSummaryTO.StockDetailsTOList[i].TodaysStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                        tblStockSummaryTO.StockDetailsTOList[i].ProductId = productInfo.IdProduct;
                                        tblStockSummaryTO.StockDetailsTOList[i].NoOfBundles = noOfBundles;
                                    }
                                    else
                                    {
                                        tblStockSummaryTO.StockDetailsTOList[i].BalanceStock = 0;
                                        tblStockSummaryTO.StockDetailsTOList[i].TodaysStock = 0;
                                        tblStockSummaryTO.StockDetailsTOList[i].ProductId = productInfo.IdProduct;
                                        tblStockSummaryTO.StockDetailsTOList[i].NoOfBundles = 0;
                                    }

                                }
                                else
                                {
                                    continue;

                                    tran.Rollback();
                                    resultMessage.Result = 0;
                                    resultMessage.Text = "Error Product Configuration Not Found : Method UpdateDailyStock";
                                    resultMessage.DisplayMessage = "Error. Record Could not be saved. Product Configuration Not Found For " + tblStockSummaryTO.StockDetailsTOList[i].MaterialDesc + " " + tblStockSummaryTO.StockDetailsTOList[i].ProdCatDesc + "-" + tblStockSummaryTO.StockDetailsTOList[i].ProdSpecDesc;
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    return resultMessage;
                                }

                            }
                            else
                            {
                                isInMTExisting = 1;   //Saket [2017-11-27] Added for consolidate stock it is alway 1
                                    tblStockSummaryTO.StockDetailsTOList[i].NoOfBundles = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                    tblStockSummaryTO.StockDetailsTOList[i].BalanceStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;                               
                            }

                            //tblStockSummaryTO.StockDetailsTOList[i].NoOfBundles = 0;
                        }
                        else
                        {
                            if (productList != null)
                            {
                                var productInfo = productList.Where(p => p.MaterialId == tblStockSummaryTO.StockDetailsTOList[i].MaterialId
                                                                    && p.ProdCatId == tblStockSummaryTO.StockDetailsTOList[i].ProdCatId
                                                                    && p.ProdSpecId == tblStockSummaryTO.StockDetailsTOList[i].ProdSpecId
                                                                    && p.BrandId == tblStockSummaryTO.StockDetailsTOList[i].BrandId).OrderByDescending(d => d.CreatedOn).FirstOrDefault();

                                if (productInfo != null)
                                {
                                    Double totalStkInMT = tblStockSummaryTO.StockDetailsTOList[i].NoOfBundles * productInfo.NoOfPcs * productInfo.AvgSecWt * productInfo.StdLength;
                                    tblStockSummaryTO.StockDetailsTOList[i].TotalStock = totalStkInMT / 1000;
                                    tblStockSummaryTO.StockDetailsTOList[i].BalanceStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                    tblStockSummaryTO.StockDetailsTOList[i].TodaysStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                    tblStockSummaryTO.StockDetailsTOList[i].ProductId = productInfo.IdProduct;
                                }
                                else
                                {

                                    continue;

                                    tran.Rollback();
                                    resultMessage.Result = 0;
                                    resultMessage.Text = "Error. Record Could not be saved. Product Configuration Not Found For " + tblStockSummaryTO.StockDetailsTOList[i].MaterialDesc + " " + tblStockSummaryTO.StockDetailsTOList[i].ProdCatDesc + "-" + tblStockSummaryTO.StockDetailsTOList[i].ProdSpecDesc;
                                    resultMessage.DisplayMessage = "Error. Record Could not be saved. Product Configuration Not Found For " + tblStockSummaryTO.StockDetailsTOList[i].MaterialDesc + " " + tblStockSummaryTO.StockDetailsTOList[i].ProdCatDesc + "-" + tblStockSummaryTO.StockDetailsTOList[i].ProdSpecDesc;
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    return resultMessage;
                                }
                            }
                        }


                        tblStockSummaryTO.StockDetailsTOList[i].IsInMT = isInMTExisting;


                        newQtyInMt = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;

                        if (isExist)
                        {
                            //Update Existing Records 
                            tblStockSummaryTO.StockDetailsTOList[i].UpdatedOn = _iCommon.ServerDateTime;
                            result = _iTblStockDetailsBL.UpdateTblStockDetails(tblStockSummaryTO.StockDetailsTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.Result = 0;
                                resultMessage.Text = "Error While UpdateTblStockDetails : Method UpdateDailyStock";
                                resultMessage.DisplayMessage = "Error.. Records could not be saved";
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                        }
                        else
                        {
                            // Insert New Stock Entry
                            tblStockSummaryTO.StockDetailsTOList[i].UpdatedOn = DateTime.MinValue;
                            tblStockSummaryTO.StockDetailsTOList[i].UpdatedBy = 0;
                            result = _iTblStockDetailsBL.InsertTblStockDetails(tblStockSummaryTO.StockDetailsTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Error While InsertTblStockDetails : Method UpdateDailyStock";
                                resultMessage.DisplayMessage = "Error.. Records could not be saved";
                                resultMessage.Result = 0;
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                        }

                        #region Insert In Consumption

                        TblStockConsumptionTO tblStockConsumptionTO = new TblStockConsumptionTO();

                        tblStockConsumptionTO.BeforeStockQty = existingQtyInMt;
                        tblStockConsumptionTO.AfterStockQty = newQtyInMt;
                        tblStockConsumptionTO.LoadingSlipExtId = 0;
                        tblStockConsumptionTO.CreatedBy = tblStockSummaryTO.CreatedBy;
                        tblStockConsumptionTO.CreatedOn = _iCommon.ServerDateTime;
                        tblStockConsumptionTO.StockDtlId = tblStockSummaryTO.StockDetailsTOList[i].IdStockDtl;

                        tblStockConsumptionTO.TxnQty = Math.Round(newQtyInMt - existingQtyInMt, 2);

                        if (tblStockConsumptionTO.TxnQty > 0)
                        {
                            tblStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
                            tblStockConsumptionTO.Remark = tblStockConsumptionTO.TxnQty + " Qty is added against stock updation";

                        }
                        else
                        {
                            tblStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;
                            tblStockConsumptionTO.Remark = tblStockConsumptionTO.TxnQty + " Qty is consumed against stock updation";

                        }


                        if (tblStockConsumptionTO.TxnQty != 0)
                        {



                            result = _iTblStockConsumptionBL.InsertTblStockConsumption(tblStockConsumptionTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Error While InsertTblStockConsumption : Method UpdateDailyStock";
                                resultMessage.DisplayMessage = "Error.. Records could not be saved";
                                resultMessage.Result = 0;
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                        }
                        #endregion

                    }
                }

                else
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,StockDetailsTOList Found NULL : Method UpdateDailyStock";
                    resultMessage.DisplayMessage = "Error.. Records could not be saved";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                #region Update Total No Of Bundels and Stock Qty in MT

                // Consolidate All No Of Bundles and Total Stock Qty
                List<TblStockDetailsTO> totalStockList = _iTblStockDetailsBL.SelectAllTblStockDetailsList(tblStockSummaryTO.IdStockSummary, conn, tran);
                Double totalNoOfBundles = 0;
                Double totalStockMT = 0;
                int skipOtherQty = 0;
                // Aniket [18-02-2019] added to skip TotalQty of other marerial  should not add in tblStockSummary table
                TblConfigParamsTO tblConfigParams = _iTblConfigParamsBL.SelectTblConfigParamsValByName(StaticStuff.Constants.IS_OTHER_MATERIAL_QTY_HIDE_ON_DASHBOARD);
                if(tblConfigParams!=null)
                {
                    if (tblConfigParams.ConfigParamVal == "1")
                    {
                        skipOtherQty = 1;
                    }
                }
               
                if (totalStockList != null && totalStockList.Count > 0)
                {
                    
                    if(skipOtherQty==1)
                    {
                        totalNoOfBundles = totalStockList.Where(x => x.ProdItemId == 0).Sum(x => x.NoOfBundles);
                        totalStockMT = totalStockList.Where(x => x.ProdItemId == 0).Sum(x => x.TotalStock);
                    }
                    else
                    {
                        totalNoOfBundles = totalStockList.Sum(t => t.NoOfBundles);
                        totalStockMT = totalStockList.Sum(t => t.TotalStock);
                    }
                    

                    tblStockSummaryTO.NoOfBundles = totalNoOfBundles;
                    tblStockSummaryTO.TotalStock = totalStockMT;
                    tblStockSummaryTO.UpdatedBy = updateOrCreatedUser;
                    tblStockSummaryTO.UpdatedOn = _iCommon.ServerDateTime;
                               
                        result = UpdateTblStockSummary(tblStockSummaryTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error,While UpdateTblStockSummary : Method UpdateDailyStock";
                            resultMessage.DisplayMessage = "Error.. Records could not be saved";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                  #region Priyanka [02-08-2018] : Set the total stock to the configuration param.
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TODAYS_BOOKING_OPENING_BALANCE);
                    if (tblConfigParamsTO != null)
                    {
                        //DashboardModels.StockUpdateInfo stockUpdateInfo = _iTblStockSummaryDAO.SelectDashboardStockUpdateInfo(stockDate);

                       List<PendingQtyOrderTypeTo> TotalPendingQtyOrderTypeList = _iTblBookingsBL.SelectTotalPendingBookingQty(stockDate);

                        string TotalPendingQtyOrderType = JsonConvert.SerializeObject(TotalPendingQtyOrderTypeList);
                        tblConfigParamsTO.ConfigParamVal = TotalPendingQtyOrderType;

                        List<PendingQtyOrderTypeTo> a = JsonConvert.DeserializeObject<List<PendingQtyOrderTypeTo>>(TotalPendingQtyOrderType);
                        
                        //tblConfigParamsTO.ConfigParamVal = bookedQty.ToString();
                        
                        //  stockUpdateInfo.SoldStock = Convert.ToDouble(tblConfigParamsTO.ConfigParamVal);

                        result = _iTblConfigParamsBL.UpdateTblConfigParams(tblConfigParamsTO);
                        if (result == -1)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateTblConfigParams");
                            return resultMessage;
                        }
                    }
                                                        
                    #endregion

                }
                else
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,StockDetailsTOList Found NULL For Final Summation : Method UpdateDailyStock";
                    resultMessage.DisplayMessage = "Error.. Records could not be saved";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : UpdateDailyStock";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DisplayMessage = "Error.. Records could not be saved";
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Sanjay [2017-06-13]
        /// </summary>
        /// <param name="sizeSpecWiseStockTOList"></param>
        /// <returns></returns>
        public ResultMessage ConfirmStockSummary(List<SizeSpecWiseStockTO> sizeSpecWiseStockTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Int32 updateOrCreatedUser = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (sizeSpecWiseStockTOList == null || sizeSpecWiseStockTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,sizeSpecWiseStockTOList Found NULL : Method ConfirmStockSummary";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #region 0.1 Check For Yesterddays Pending Loading Slip and Reduce Stock Qty
                List<TblStockDetailsTO> stockList = _iTblStockDetailsBL.SelectAllTblStockDetailsList(sizeSpecWiseStockTOList[0].StockSummaryId, conn, tran);
                Boolean canStockDeclare = true;
                String notDeclaReasons = string.Empty;
                updateOrCreatedUser = sizeSpecWiseStockTOList[0].ConfirmedBy;

                List<TblLoadingSlipExtTO> postponeList = _iTblLoadingSlipExtBL.SelectCnfWiseLoadingMaterialToPostPoneList(conn, tran);
                if (postponeList != null)
                {
                    var groupItemsList = postponeList.GroupBy(x => new { x.ProdCatId, x.ProdCatDesc, x.ProdSpecId, x.ProdSpecDesc, x.MaterialId, x.MaterialDesc }).ToList();

                    for (int g = 0; g < groupItemsList.Count; g++)
                    {
                        //var tempList = postponeList.Where(p => p.MaterialId == groupItemsList[g].Key.MaterialId && p.ProdCatId == groupItemsList[g].Key.ProdCatId && p.ProdCatId == groupItemsList[g].Key.ProdSpecId).ToList();
                        var tempList = postponeList.Where(p => p.MaterialId == groupItemsList[g].Key.MaterialId && p.ProdCatId == groupItemsList[g].Key.ProdCatId && p.ProdSpecId == groupItemsList[g].Key.ProdSpecId).ToList();

                        Double loadingQty = tempList.Sum(x => x.LoadingQty);

                        var curStockList = stockList.Where(p => p.MaterialId == groupItemsList[g].Key.MaterialId && p.ProdCatId == groupItemsList[g].Key.ProdCatId && p.ProdSpecId == groupItemsList[g].Key.ProdSpecId).ToList();

                        Double itemStockQty = 0;
                        if (curStockList != null)
                            itemStockQty = curStockList.Sum(a => a.TotalStock);

                        if (itemStockQty < loadingQty)
                        {
                            canStockDeclare = false;
                            notDeclaReasons += groupItemsList[g].Key.MaterialDesc + " " + groupItemsList[g].Key.ProdCatDesc + "-" + groupItemsList[g].Key.ProdSpecDesc + " Qty :" + loadingQty + " ,";
                        }
                        else
                        {
                            Double qtyToRemove = loadingQty;
                            for (int si = 0; si < curStockList.Count; si++)
                            {
                                if (qtyToRemove > 0)
                                {
                                    TblStockDetailsTO stockDtlTO = curStockList[si];
                                    if (stockDtlTO.TotalStock >= qtyToRemove)
                                    {
                                        stockDtlTO.RemovedStock = qtyToRemove;
                                        qtyToRemove = 0;
                                    }
                                    else
                                    {
                                        stockDtlTO.RemovedStock = stockDtlTO.TotalStock;
                                        qtyToRemove = qtyToRemove - stockDtlTO.TotalStock;
                                    }

                                    stockDtlTO.UpdatedBy = updateOrCreatedUser;
                                    stockDtlTO.UpdatedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;
                                    result = _iTblStockDetailsBL.UpdateTblStockDetails(stockDtlTO, conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        resultMessage.Text = "Error,While UpdateTblStockDetails : Method ConfirmStockSummary";
                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Result = 0;
                                        return resultMessage;
                                    }
                                }
                                else break;
                            }
                        }
                    }
                }

                if (!canStockDeclare)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,While UpdateTblStockDetails : Method ConfirmStockSummary";
                    notDeclaReasons = "Final stock is less than 0 for below items. Please cofirm stock from yesterdays Vehicle In ,"+ Environment.NewLine + notDeclaReasons;
                    resultMessage.DisplayMessage = notDeclaReasons;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                #region 1. Mark Balance Stock = Total Stock i.e Make this stock available to C&F View

                Double totalNoOfBundles = 0;
                Double totalStock = 0;
                for (int i = 0; i < stockList.Count; i++)
                {
                    //Sanjay [2017-06-13] As yesterdays loading slips stock reduces from current stock
                    //stockList[i].BalanceStock = stockList[i].TotalStock;
                    stockList[i].BalanceStock = stockList[i].TotalStock - stockList[i].RemovedStock;
                    stockList[i].UpdatedBy = updateOrCreatedUser;
                    stockList[i].UpdatedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;
                    result = _iTblStockDetailsBL.UpdateTblStockDetails(stockList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error,While UpdateTblStockDetails : Method ConfirmStockSummary";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    totalNoOfBundles += stockList[i].NoOfBundles;
                    totalStock += stockList[i].TotalStock;

                }
                #endregion

                #region 2. Mark Stock Summary As Confirmed. After Confirmation Do Not Allow Stock Edit

                TblStockSummaryTO stockSummaryTO = SelectTblStockSummaryTO(sizeSpecWiseStockTOList[0].StockSummaryId, conn, tran);
                if (stockSummaryTO != null)
                {
                    stockSummaryTO.TotalStock = totalStock;
                    stockSummaryTO.NoOfBundles = totalNoOfBundles;
                    stockSummaryTO.ConfirmedBy = updateOrCreatedUser;
                    stockSummaryTO.ConfirmedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;
                    stockSummaryTO.UpdatedBy = updateOrCreatedUser;
                    stockSummaryTO.UpdatedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;

                    result = UpdateTblStockSummary(stockSummaryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error,While UpdateTblStockSummary : Method ConfirmStockSummary";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }
                else
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,stockSummaryTO Found NULL : Method ConfirmStockSummary";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                #region 2.1 If AutoDeclare Loading Quota Setting = 1 Then Declare Loading Quota to All C&F

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION, conn, tran);

                if (tblConfigParamsTO != null && Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 1)
                {
                    //Declare Loading Quota 
                    DateTime stockDate = sizeSpecWiseStockTOList[0].ConfirmedOn;

                    List<TblLoadingQuotaDeclarationTO> list = _iTblLoadingQuotaDeclarationBL.SelectLatestCalculatedLoadingQuotaDeclarationList(stockDate,0, conn, tran);
                    if (list == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error,C&F Quota list found empty ";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    #region 1. Mark All Previous Loading Quota As Inactive

                    result = _iTblLoadingQuotaDeclarationDAO.DeactivateAllPrevLoadingQuota(sizeSpecWiseStockTOList[0].ConfirmedBy, conn, tran);
                    if (result < 0)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While DeactivateAllPrevLoadingQuota : SaveLoadingQuotaDeclaration";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    #endregion

                    #region 2. Assign New Quota 

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].CreatedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;
                        list[i].CreatedBy = sizeSpecWiseStockTOList[0].ConfirmedBy;

                        result = _iTblLoadingQuotaDeclarationBL.InsertTblLoadingQuotaDeclaration(list[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error While InsertTblLoadingQuotaDeclaration : ConfirmStockSummary";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }

                    #endregion
                }

                #endregion

                #region 3.1 Notification to Directors and account person on stock confirmation

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.TODAYS_STOCK_CONFIRMED;
                tblAlertInstanceTO.AlertAction = "TODAYS_STOCK_CONFIRMED";
                tblAlertInstanceTO.AlertComment = "Today's Stock is Confirmed";// . Total Stock(In MT) is :" + stockSummaryTO.TotalStock;
                tblAlertInstanceTO.EffectiveFromDate = stockSummaryTO.ConfirmedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "TODAYS_STOCK_CONFIRMED";
                tblAlertInstanceTO.SourceEntityId = stockSummaryTO.IdStockSummary;
                tblAlertInstanceTO.RaisedBy = stockSummaryTO.ConfirmedBy;
                tblAlertInstanceTO.RaisedOn = stockSummaryTO.ConfirmedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }

                #endregion

                #region 3.2 Notifications of Loading Quota Declaration To All C&F

                tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_QUOTA_DECLARED;
                tblAlertInstanceTO.AlertAction = "LOADING_QUOTA_DECLARED";
                tblAlertInstanceTO.AlertComment = "Today's Loading Quota is Declared";

                tblAlertInstanceTO.EffectiveFromDate = stockSummaryTO.ConfirmedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "LOADING_QUOTA_DECLARED";
                tblAlertInstanceTO.SourceEntityId = stockSummaryTO.IdStockSummary;
                tblAlertInstanceTO.RaisedBy = stockSummaryTO.ConfirmedBy;
                tblAlertInstanceTO.RaisedOn = stockSummaryTO.ConfirmedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                String alertDefIds = (int)NotificationConstants.NotificationsE.LOADING_QUOTA_DECLARED + "," + (int)NotificationConstants.NotificationsE.LOADING_STOPPED;
                result = _iTblAlertInstanceBL.ResetAlertInstanceByDef(alertDefIds, conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While ResetAlertInstanceByDef";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }

                rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }
                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.DisplayMessage = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : ConfirmStockSummary";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Updation
        public int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO)
        {
            return _iTblStockSummaryDAO.UpdateTblStockSummary(tblStockSummaryTO);
        }

        public int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockSummaryDAO.UpdateTblStockSummary(tblStockSummaryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblStockSummary(Int32 idStockSummary)
        {
            return _iTblStockSummaryDAO.DeleteTblStockSummary(idStockSummary);
        }

        public int DeleteTblStockSummary(Int32 idStockSummary, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockSummaryDAO.DeleteTblStockSummary(idStockSummary, conn, tran);
        }

        #endregion
        
    }
}
