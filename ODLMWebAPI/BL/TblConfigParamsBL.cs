using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{             
    public class TblConfigParamsBL : ITblConfigParamsBL
    {
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblConfigParamHistoryDAO _iTblConfigParamHistoryDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblConfigParamsBL(ICommon iCommon, IConnectionString iConnectionString, ITblConfigParamsDAO iTblConfigParamsDAO, ITblConfigParamHistoryDAO iTblConfigParamHistoryDAO)
        {
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblConfigParamHistoryDAO = iTblConfigParamHistoryDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblConfigParamsTO> SelectAllTblConfigParamsList()
        {
            return _iTblConfigParamsDAO.SelectAllTblConfigParams();
        }
        /// <summary>
        /// GJ@20170810 : Get the Configuration value by Name 
        /// </summary>
        /// <param name="configParamName"></param>
        /// <returns></returns>
        public TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName)
        {
            return _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
        }

        public TblConfigParamsTO SelectTblConfigParamsTO(Int32 idConfigParam)
        {
            return _iTblConfigParamsDAO.SelectTblConfigParams(idConfigParam);
        }

        public TblConfigParamsTO SelectTblConfigParamsTO(String configParamName)
        {
            try
            {
                return _iTblConfigParamsDAO.SelectTblConfigParams(configParamName);
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
            //    return _iTblConfigParamsDAO.SelectTblConfigParams(configParamName, conn, tran);
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

        public TblConfigParamsTO SelectTblConfigParamsTO(string configParamName,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblConfigParamsDAO.SelectTblConfigParams(configParamName,conn,tran);
        }


        public Int32 GetStockConfigIsConsolidate()
        {
            TblConfigParamsTO tblConfigParamsTO = SelectTblConfigParamsTO(Constants.CONSOLIDATE_STOCK);
            Int32 isConsolidateStk = 0;

            if (tblConfigParamsTO != null)
                isConsolidateStk = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

            return isConsolidateStk;
        }

        #endregion

        #region Insertion
        public int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            return _iTblConfigParamsDAO.InsertTblConfigParams(tblConfigParamsTO);
        }

        public int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblConfigParamsDAO.InsertTblConfigParams(tblConfigParamsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            return _iTblConfigParamsDAO.UpdateTblConfigParams(tblConfigParamsTO);
        }

        public ResultMessage UpdateConfigParamsWithHistory(TblConfigParamsTO configParamsTO,Int32 updatedByUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblConfigParamsTO existingTblConfigParamsTO = SelectTblConfigParamsTO(configParamsTO.ConfigParamName, conn, tran);
                if(existingTblConfigParamsTO==null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While SelectTblConfigParamsTO. existingTblConfigParamsTO found NULL ";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                TblConfigParamHistoryTO historyTO = new TblConfigParamHistoryTO();
                historyTO.ConfigParamId = configParamsTO.IdConfigParam;
                historyTO.ConfigParamName = configParamsTO.ConfigParamName;
                historyTO.ConfigParamOldVal = existingTblConfigParamsTO.ConfigParamVal;
                historyTO.ConfigParamNewVal = configParamsTO.ConfigParamVal;
                historyTO.CreatedBy = updatedByUserId;
                historyTO.CreatedOn = serverDate;

                int result = _iTblConfigParamHistoryDAO.InsertTblConfigParamHistory(historyTO, conn, tran);
                if (result!=1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While InsertTblConfigParamHistory";
                    return resultMessage;
                }

                result = UpdateTblConfigParams(configParamsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While UpdateTblConfigParams";
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateConfigParamsWithHistory");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblConfigParamsDAO.UpdateTblConfigParams(tblConfigParamsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblConfigParams(Int32 idConfigParam)
        {
            return _iTblConfigParamsDAO.DeleteTblConfigParams(idConfigParam);
        }

        public int DeleteTblConfigParams(Int32 idConfigParam, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblConfigParamsDAO.DeleteTblConfigParams(idConfigParam, conn, tran);
        }

        #endregion
        
    }
}
