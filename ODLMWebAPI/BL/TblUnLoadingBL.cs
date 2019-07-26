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
    public class TblUnLoadingBL : ITblUnLoadingBL
    {
        private readonly ITblUnLoadingDAO _iTblUnLoadingDAO;
        private readonly ITblUnLoadingItemDetBL _iTblUnLoadingItemDetBL;
        private readonly IConnectionString _iConnectionString;
        public TblUnLoadingBL(IConnectionString iConnectionString, ITblUnLoadingDAO iTblUnLoadingDAO, ITblUnLoadingItemDetBL iTblUnLoadingItemDetBL)
        {
            _iTblUnLoadingDAO = iTblUnLoadingDAO;
            _iTblUnLoadingItemDetBL = iTblUnLoadingItemDetBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public List<TblUnLoadingTO> SelectAllTblUnLoadingList(DateTime startDate, DateTime endDate)
        {
            //startDate = Constants.GetStartDateTime(startDate);
            //endDate = Constants.GetEndDateTime(endDate);
            startDate = Convert.ToDateTime(startDate);
            endDate = Convert.ToDateTime(endDate);
            return _iTblUnLoadingDAO.SelectAllTblUnLoading(startDate,endDate);
        }

        public TblUnLoadingTO SelectTblUnLoadingTO(Int32 idUnLoading)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                // Get all perticular unloading slip details with respecive item details
                TblUnLoadingTO tblUnLoadingTO = _iTblUnLoadingDAO.SelectTblUnLoading(idUnLoading);
                if (tblUnLoadingTO != null)
                    tblUnLoadingTO.UnLoadingItemDetTOList = _iTblUnLoadingItemDetBL.SelectAllUnLoadingItemDetailsList(idUnLoading);

                return tblUnLoadingTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblUnLoadingTO");
                return null;
            }
        }

        #endregion

        #region Insertion
        public int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO)
        {
            return _iTblUnLoadingDAO.InsertTblUnLoading(tblUnLoadingTO);
        }

        public int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUnLoadingDAO.InsertTblUnLoading(tblUnLoadingTO, conn, tran);
        }

        // Vaibhav [13-Sep-2017] save unloading slip
        public ResultMessage SaveNewUnLoadingSlipDetails(TblUnLoadingTO tblUnLoadingTO)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                result = InsertTblUnLoading(tblUnLoadingTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While InsertTblUnLoading");
                    return resultMessage;
                }

                if (tblUnLoadingTO.UnLoadingItemDetTOList.Count != 0)
                {
                    for (int i = 0; i < tblUnLoadingTO.UnLoadingItemDetTOList.Count; i++)
                    {
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].CreatedBy = tblUnLoadingTO.CreatedBy;
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].CreatedOn = tblUnLoadingTO.CreatedOn;
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].UnLoadingId = tblUnLoadingTO.IdUnLoading;

                        result = _iTblUnLoadingItemDetBL.InsertTblUnLoadingItemDet(tblUnLoadingTO.UnLoadingItemDetTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblUnLoadingItemDet");
                            return resultMessage;
                        }
                    }

                    // Vaibhav [14-Sep-2017] Calculate total unload qty from itemdetails
                    tblUnLoadingTO.TotalUnLoadingQty = tblUnLoadingTO.UnLoadingItemDetTOList.Sum(i => i.UnLoadingQty);

                    result = UpdateUnloadingQuantity(tblUnLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While UpdateUnloadingQuantity");
                        return resultMessage;
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewUnLoadingSlipDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public int UpdateTblUnLoading(TblUnLoadingTO tblUnLoadingTO)
        {
            return _iTblUnLoadingDAO.UpdateTblUnLoading(tblUnLoadingTO);
        }


        public int UpdateTblUnLoading(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUnLoadingDAO.UpdateTblUnLoading(tblUnLoadingTO, conn, tran);
        }

        // Vaibhav [14-Sep-2017] Update unloading slip details
        public ResultMessage UpdateUnLoadingSlipDetails(TblUnLoadingTO tblUnLoadingTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                result = UpdateTblUnLoading(tblUnLoadingTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While UpdateTblUnLoading");
                    return resultMessage;
                }

                if (tblUnLoadingTO.UnLoadingItemDetTOList.Count != 0)
                {
                    for (int i = 0; i < tblUnLoadingTO.UnLoadingItemDetTOList.Count; i++)
                    {
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].UpdatedBy = tblUnLoadingTO.UpdatedBy;
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].UpdatedOn = tblUnLoadingTO.UpdatedOn;
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].UnLoadingId = tblUnLoadingTO.IdUnLoading;

                        result = _iTblUnLoadingItemDetBL.UpdateTblUnLoadingItemDet(tblUnLoadingTO.UnLoadingItemDetTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While UpdateTblUnLoadingItemDet");
                            return resultMessage;
                        }
                    }

                    // Vaibhav [14-Sep-2017] Calculate total unload qty from itemdetails                   
                    tblUnLoadingTO.TotalUnLoadingQty = tblUnLoadingTO.UnLoadingItemDetTOList.Sum(i => i.UnLoadingQty);

                    result = UpdateUnloadingQuantity(tblUnLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While UpdateUnloadingQuantity");
                        return resultMessage;
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateUnLoadingSlipDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        // Vaibhav [14-Sep-2017] Update total unloading qty for perticular unloading transaction
        public int UpdateUnloadingQuantity(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                return _iTblUnLoadingDAO.UpdateUnLoadingQty(tblUnLoadingTO, conn, tran);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateUnloadingQuantity");
                return 0;
            }
        }


        // Vaibhav [12-oct-2017] added to deactivate unloading slip
        public ResultMessage DeactivateUnLoadingSlip(TblUnLoadingTO tblUnLoadingTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                result =  _iTblUnLoadingDAO.DeactivateUnLoadingSlip(tblUnLoadingTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While DeactivateUnLoadingSlip");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactivateUnLoadingSlip");
                return resultMessage;
            }
        }

        #endregion

        #region Deletion
        public int DeleteTblUnLoading(Int32 idUnLoading)
        {
            return _iTblUnLoadingDAO.DeleteTblUnLoading(idUnLoading);
        }

        public int DeleteTblUnLoading(Int32 idUnLoading, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUnLoadingDAO.DeleteTblUnLoading(idUnLoading, conn, tran);
        }

        #endregion

    }
}
