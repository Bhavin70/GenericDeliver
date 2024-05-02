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
    public class TblRunningSizesBL : ITblRunningSizesBL
    { 
        private readonly ITblRunningSizesDAO _iTblRunningSizesDAO;
        private readonly ITblStockSummaryDAO _iTblStockSummaryDAO;
        private readonly ITblStockSummaryBL _iTblStockSummaryBL;
        private readonly ITblStockDetailsBL _iTblStockDetailsBL;
        private readonly ITblProductInfoBL _iTblProductInfoBL;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblRunningSizesBL(ICommon iCommon , IConnectionString iConnectionString, ITblProductInfoBL iTblProductInfoBL, ITblStockDetailsBL iTblStockDetailsBL, ITblStockSummaryBL iTblStockSummaryBL, ITblRunningSizesDAO iTblRunningSizesDAO, ITblStockSummaryDAO iTblStockSummaryDAO)
        {
            _iTblRunningSizesDAO = iTblRunningSizesDAO;
            _iTblStockSummaryDAO = iTblStockSummaryDAO;
            _iTblStockSummaryBL = iTblStockSummaryBL;
            _iTblStockDetailsBL = iTblStockDetailsBL;
            _iTblProductInfoBL = iTblProductInfoBL;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection
        public List<TblRunningSizesTO> SelectAllTblRunningSizesList()
        {
            return _iTblRunningSizesDAO.SelectAllTblRunningSizes();
        }

        public TblRunningSizesTO SelectTblRunningSizesTO(Int32 idRunningSize)
        {
            return  _iTblRunningSizesDAO.SelectTblRunningSizes(idRunningSize);
        }


        public List<TblRunningSizesTO> SelectAllTblRunningSizesList(DateTime stockDate)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblRunningSizesList(stockDate, conn, tran);
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

        private List<TblRunningSizesTO> SelectAllTblRunningSizesList(DateTime stockDate, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRunningSizesDAO.SelectAllTblRunningSizes(stockDate,conn,tran);
        }

        #endregion

        #region Insertion
        public int InsertTblRunningSizes(TblRunningSizesTO tblRunningSizesTO)
        {
            return _iTblRunningSizesDAO.InsertTblRunningSizes(tblRunningSizesTO);
        }

        public int InsertTblRunningSizes(TblRunningSizesTO tblRunningSizesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRunningSizesDAO.InsertTblRunningSizes(tblRunningSizesTO, conn, tran);
        }

        public ResultMessage SaveDailyRunningSizeInfo(List<TblRunningSizesTO> runningSizesTOList,DateTime stockDate)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                Int32 updateOrCreatedUser = 0;
                DateTime createOrUpdateDate = _iCommon.ServerDateTime;

                List<TblRunningSizesTO> existingList = SelectAllTblRunningSizesList(stockDate, conn, tran);
                updateOrCreatedUser = runningSizesTOList[0].CreatedBy;

                TblStockSummaryTO tblStockSummaryTO = _iTblStockSummaryDAO.SelectTblStockSummary(stockDate, conn, tran);

                if (tblStockSummaryTO == null)
                {
                    tblStockSummaryTO = new TblStockSummaryTO();
                    tblStockSummaryTO.StockDate = stockDate;
                    tblStockSummaryTO.CreatedOn = createOrUpdateDate;
                    tblStockSummaryTO.CreatedBy = updateOrCreatedUser;
                    result = _iTblStockSummaryBL.InsertTblStockSummary(tblStockSummaryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error While InsertTblStockSummary : MEthod UpdateDailyStock";
                        resultMessage.DisplayMessage = "Error.. Record could not be saved";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }
                else
                {
                    if (tblStockSummaryTO.ConfirmedOn != DateTime.MinValue)
                    {
                        tran.Rollback();
                        resultMessage.Result = 0;
                        resultMessage.Text = "Stock For the Date :" + tblStockSummaryTO.StockDate.Date.ToString(Constants.DefaultDateFormat) + " is already confirmed. You can not update the stock now";
                        resultMessage.DisplayMessage = resultMessage.Text;
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                }

                List<TblStockDetailsTO> existingStockList = _iTblStockDetailsBL.SelectAllTblStockDetailsList(tblStockSummaryTO.IdStockSummary, conn, tran);

                // For weight and Stock in MT calculations
                List<TblProductInfoTO> productList = _iTblProductInfoBL.SelectAllTblProductInfoList(conn, tran);

                for (int i = 0; i < runningSizesTOList.Count; i++)
                {
                    TblRunningSizesTO tblRunningSizesTONew = runningSizesTOList[i];

                    double addedRunSizeQty = tblRunningSizesTONew.TotalStock;
                    tblRunningSizesTONew.ProdCatId = (int)Constants.ProductCategoryE.TMT;
                    tblRunningSizesTONew.ProdSpecId = (int)Constants.ProductSpecE.BEND;
                    tblRunningSizesTONew.InchId = (int)Constants.pgDashBoardType.PIPES;
                    tblRunningSizesTONew.StripId = (int)Constants.pgDashBoardType.STRIP;

                    TblProductInfoTO productInfoTO = null;
                    if (productList != null)
                    {
                        var productInfo = productList.Where(p => p.MaterialId == tblRunningSizesTONew.MaterialId
                                                            && p.ProdCatId == (int)Constants.ProductCategoryE.TMT
                                                            && p.ProdSpecId == (int)Constants.ProductSpecE.BEND).OrderByDescending(d => d.CreatedOn).FirstOrDefault();

                        if (productInfo != null)
                        {
                            productInfoTO = productInfo;
                        }
                        else
                        {
                            tran.Rollback();
                            resultMessage.Result = 0;
                            resultMessage.Text = "Product Configuration found null";
                            resultMessage.DisplayMessage = "Product Configuration Not Defined For " + tblRunningSizesTONew.MaterialDesc + " TMT-Bend";
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }

                    TblRunningSizesTO existingTblRunningSizesTO = null;
                    if (existingList != null)
                    {
                        existingTblRunningSizesTO = existingList.Where(r => r.LocationId == tblRunningSizesTONew.LocationId
                                                           && r.MaterialId == tblRunningSizesTONew.MaterialId).FirstOrDefault();


                    }

                    if (existingTblRunningSizesTO != null)
                    {
                        // Update Existing Record For That Stock Date
                        tblRunningSizesTONew.IdRunningSize = existingTblRunningSizesTO.IdRunningSize;
                        tblRunningSizesTONew.UpdatedBy = updateOrCreatedUser;
                        tblRunningSizesTONew.UpdatedOn = createOrUpdateDate;
                        tblRunningSizesTONew.StockDate = stockDate;
                        tblRunningSizesTONew.TotalStock += existingTblRunningSizesTO.TotalStock;

                        if (productInfoTO != null)
                        {
                            Double noOfBundles = (tblRunningSizesTONew.TotalStock * 1000) / productInfoTO.AvgBundleWt;
                            tblRunningSizesTONew.NoOfBundles = noOfBundles;
                        }

                        result = UpdateTblRunningSizes(tblRunningSizesTONew, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.Text = "Error While UpdateTblRunningSizes : SaveDailyRunningSizeInfo";
                            resultMessage.DisplayMessage = "Error.. Record could not be saved";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }
                    else
                    {
                        // Insert New Record
                        // Update Existing Record For That Stock Date
                        tblRunningSizesTONew.CreatedBy = updateOrCreatedUser;
                        tblRunningSizesTONew.CreatedOn = createOrUpdateDate;
                        tblRunningSizesTONew.StockDate = stockDate;
                        if (productInfoTO != null)
                        {
                            Double noOfBundles = (tblRunningSizesTONew.TotalStock * 1000) / productInfoTO.AvgBundleWt;
                            tblRunningSizesTONew.NoOfBundles = noOfBundles;
                        }

                        result = InsertTblRunningSizes(tblRunningSizesTONew, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.Text = "Error While UpdateTblRunningSizes : SaveDailyRunningSizeInfo";
                            resultMessage.DisplayMessage = "Error.. Record could not be saved";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }




                    Boolean isExist = false;
                    TblStockDetailsTO existingStocDtlTO = null;
                    if (existingStockList != null && existingStockList.Count > 0)
                    {
                        existingStocDtlTO = existingStockList.Where(e => e.LocationId == tblRunningSizesTONew.LocationId
                                                                        && e.MaterialId == tblRunningSizesTONew.MaterialId
                                                                        && e.ProdCatId == (int)Constants.ProductCategoryE.TMT
                                                                        && e.ProdSpecId == (int)Constants.ProductSpecE.BEND).FirstOrDefault();
                        if (existingStocDtlTO != null)
                        {
                            isExist = true;
                        }
                    }

                    if (isExist)
                    {
                        //Update Existing Records 
                        existingStocDtlTO.UpdatedOn = _iCommon.ServerDateTime;
                        Double existingStock = existingStocDtlTO.TotalStock;
                        double totalStock = addedRunSizeQty + existingStock;

                        if (productInfoTO != null)
                        {
                            Double noOfBundles = (totalStock * 1000) / productInfoTO.AvgBundleWt;
                            existingStocDtlTO.NoOfBundles = noOfBundles;
                            existingStocDtlTO.ProductId = productInfoTO.IdProduct;
                        }

                        existingStocDtlTO.TotalStock = totalStock;
                        existingStocDtlTO.BalanceStock = totalStock;
                        existingStocDtlTO.TodaysStock = totalStock;
                        existingStocDtlTO.UpdatedBy = updateOrCreatedUser;
                        existingStocDtlTO.UpdatedOn = createOrUpdateDate;
                        result = _iTblStockDetailsBL.UpdateTblStockDetails(existingStocDtlTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Result = 0;
                            resultMessage.Text = "Error While UpdateTblStockDetails : Method UpdateDailyStock";
                            resultMessage.DisplayMessage = "Error.. Record could not be saved";
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }
                    else
                    {
                        // Insert New Stock Entry
                        existingStocDtlTO = new TblStockDetailsTO();
                        existingStocDtlTO.UpdatedOn = DateTime.MinValue;
                        existingStocDtlTO.UpdatedBy = 0;
                        existingStocDtlTO.CreatedBy = updateOrCreatedUser;
                        existingStocDtlTO.CreatedOn = createOrUpdateDate;
                        existingStocDtlTO.ProdCatId = (int)Constants.ProductCategoryE.TMT;
                        existingStocDtlTO.ProdSpecId = (int)Constants.ProductSpecE.BEND;
                        existingStocDtlTO.MaterialId = tblRunningSizesTONew.MaterialId;
                        existingStocDtlTO.LocationId = tblRunningSizesTONew.LocationId;
                        existingStocDtlTO.StockSummaryId = tblStockSummaryTO.IdStockSummary;

                        double totalStock = addedRunSizeQty;

                        if (productInfoTO != null)
                        {
                            Double noOfBundles = (totalStock * 1000) / productInfoTO.AvgBundleWt;
                            existingStocDtlTO.NoOfBundles = noOfBundles;
                            existingStocDtlTO.ProductId = productInfoTO.IdProduct;
                        }

                        existingStocDtlTO.TotalStock = totalStock;
                        existingStocDtlTO.TodaysStock = totalStock;
                        existingStocDtlTO.BalanceStock = totalStock;

                        result = _iTblStockDetailsBL.InsertTblStockDetails(existingStocDtlTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error While InsertTblStockDetails : Method SaveDailyRunningSizeInfo";
                            resultMessage.DisplayMessage = "Error.. Record could not be saved";
                            resultMessage.Result = 0;
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }
                }

                #region Update Total No Of Bundels and Stock Qty in MT

                // Consolidate All No Of Bundles and Total Stock Qty
                List<TblStockDetailsTO> totalStockList = _iTblStockDetailsBL.SelectAllTblStockDetailsList(tblStockSummaryTO.IdStockSummary, conn, tran);
                Double totalNoOfBundles = 0;
                Double totalStockMT = 0;

                if (totalStockList != null && totalStockList.Count > 0)
                {
                    totalNoOfBundles = totalStockList.Sum(t => t.NoOfBundles);
                    totalStockMT = totalStockList.Sum(t => t.TotalStock);

                    tblStockSummaryTO.NoOfBundles = totalNoOfBundles;
                    tblStockSummaryTO.TotalStock = totalStockMT;
                    tblStockSummaryTO.UpdatedBy = updateOrCreatedUser;
                    tblStockSummaryTO.UpdatedOn = _iCommon.ServerDateTime;

                    result = _iTblStockSummaryBL.UpdateTblStockSummary(tblStockSummaryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DisplayMessage = "Error.. Record could not be saved";
                        resultMessage.Text = "Error,While UpdateTblStockSummary : Method SaveDailyRunningSizeInfo";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }
                else
                {
                    tran.Rollback();
                    resultMessage.DisplayMessage = "Error.. Record could not be saved";
                    resultMessage.Text = "Error,StockDetailsTOList Found NULL For Final Summation : Method SaveDailyRunningSizeInfo";
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
                resultMessage.Text = "Exception Error While Record Save : SaveDailyRunningSizeInfo";
                resultMessage.DisplayMessage = "Error.. Record could not be saved";
                resultMessage.MessageType = ResultMessageE.Error;
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
        public int UpdateTblRunningSizes(TblRunningSizesTO tblRunningSizesTO)
        {
            return _iTblRunningSizesDAO.UpdateTblRunningSizes(tblRunningSizesTO);
        }

        public int UpdateTblRunningSizes(TblRunningSizesTO tblRunningSizesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRunningSizesDAO.UpdateTblRunningSizes(tblRunningSizesTO, conn, tran);
        }


        #endregion

        #region Deletion
        public int DeleteTblRunningSizes(Int32 idRunningSize)
        {
            return _iTblRunningSizesDAO.DeleteTblRunningSizes(idRunningSize);
        }

        public int DeleteTblRunningSizes(Int32 idRunningSize, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRunningSizesDAO.DeleteTblRunningSizes(idRunningSize, conn, tran);
        }

        public ResultMessage RemoveRunningSizeDtls(TblRunningSizesTO runningSizesTO, TblStockSummaryTO tblStockSummaryTO,Int32 userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            DateTime txnDate = _iCommon.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblStockDetailsTO stockDtlTO = _iTblStockDetailsBL.SelectTblStockDetailsTO(runningSizesTO, conn, tran);
                if (stockDtlTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Stock DetailTO found null";
                    return resultMessage;
                }

                stockDtlTO.TotalStock = stockDtlTO.TotalStock - runningSizesTO.TotalStock;
                stockDtlTO.NoOfBundles = stockDtlTO.NoOfBundles - runningSizesTO.NoOfBundles;
                stockDtlTO.BalanceStock = stockDtlTO.BalanceStock - runningSizesTO.TotalStock;
                stockDtlTO.UpdatedBy = userId;
                stockDtlTO.UpdatedOn = txnDate;

                int result = _iTblStockDetailsBL.UpdateTblStockDetails(stockDtlTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While UpdateTblStockDetails In Method RemoveRunningSizeDtls";
                    return resultMessage;
                }


                result = DeleteTblRunningSizes(runningSizesTO.IdRunningSize);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While DeleteTblRunningSizes In Method RemoveRunningSizeDtls";
                    return resultMessage;
                }

                #region Update Total No Of Bundels and Stock Qty in MT

                // Consolidate All No Of Bundles and Total Stock Qty
                List<TblStockDetailsTO> totalStockList = _iTblStockDetailsBL.SelectAllTblStockDetailsList(tblStockSummaryTO.IdStockSummary, conn, tran);
                Double totalNoOfBundles = 0;
                Double totalStockMT = 0;

                if (totalStockList != null && totalStockList.Count > 0)
                {
                    totalNoOfBundles = totalStockList.Sum(t => t.NoOfBundles);
                    totalStockMT = totalStockList.Sum(t => t.TotalStock);

                    tblStockSummaryTO.NoOfBundles = totalNoOfBundles;
                    tblStockSummaryTO.TotalStock = totalStockMT;
                    tblStockSummaryTO.UpdatedBy = userId;
                    tblStockSummaryTO.UpdatedOn = txnDate;

                    result = _iTblStockSummaryBL.UpdateTblStockSummary(tblStockSummaryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error,While UpdateTblStockSummary : Method SaveDailyRunningSizeInfo";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }
                else
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,StockDetailsTOList Found NULL For Final Summation : Method SaveDailyRunningSizeInfo";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion
                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Running Size Removed Successfully";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Error Error at BL Level in Method RemoveRunningSizeDtls";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

    }
}
