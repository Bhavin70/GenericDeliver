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
    public class TblLoadingAllowedTimeBL : ITblLoadingAllowedTimeBL
    {
        private readonly ITblLoadingAllowedTimeDAO _iTblLoadingAllowedTimeDAO;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly IConnectionString _iConnectionString;
        public TblLoadingAllowedTimeBL(IConnectionString iConnectionString, ITblLoadingAllowedTimeDAO iTblLoadingAllowedTimeDAO, ITblAlertInstanceBL iTblAlertInstanceBL)
        {
            _iTblLoadingAllowedTimeDAO = iTblLoadingAllowedTimeDAO;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection
    
        public List<TblLoadingAllowedTimeTO> SelectAllTblLoadingAllowedTimeList()
        {
            return  _iTblLoadingAllowedTimeDAO.SelectAllTblLoadingAllowedTime();
        }

        public TblLoadingAllowedTimeTO SelectTblLoadingAllowedTimeTO(Int32 idLoadingTime)
        {
            return  _iTblLoadingAllowedTimeDAO.SelectTblLoadingAllowedTime(idLoadingTime);
        }

        public TblLoadingAllowedTimeTO SelectAllowedLoadingTimeTO(DateTime date)
        {
            return _iTblLoadingAllowedTimeDAO.SelectTblLoadingAllowedTime(date);
        }

        #endregion

        #region Insertion

        public ResultMessage SaveAllowedLoadingTime(TblLoadingAllowedTimeTO loadingAllowedTimeTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (loadingAllowedTimeTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loadingQuotaTransferTOList is found empty";
                    return resultMessage;
                }

                DateTime txnDate = loadingAllowedTimeTO.CreatedOn;
                String alertMsg = "New Loading Time Update. Loading Will be stoped On " + loadingAllowedTimeTO.AllowedLoadingTimeStr;

                result = InsertTblLoadingAllowedTime(loadingAllowedTimeTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblLoadingAllowedTime";
                    resultMessage.DisplayMessage = "Error...Record could not be update";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #region 3.2 Notifications of Loading Quota Declaration To All C&F

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_STOPPED;
                tblAlertInstanceTO.AlertAction = "LOADING_STOPPED";
                tblAlertInstanceTO.AlertComment = alertMsg;

                tblAlertInstanceTO.EffectiveFromDate = loadingAllowedTimeTO.CreatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "LOADING_STOPPED";
                tblAlertInstanceTO.SourceEntityId = loadingAllowedTimeTO.IdLoadingTime;
                tblAlertInstanceTO.RaisedBy = loadingAllowedTimeTO.CreatedBy;
                tblAlertInstanceTO.RaisedOn = loadingAllowedTimeTO.CreatedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                resultMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }
                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Allowed Loading Time Is Updated Successfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback();

                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method SaveLoadingQuotaTransferNotes";
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public int InsertTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO)
        {
            return _iTblLoadingAllowedTimeDAO.InsertTblLoadingAllowedTime(tblLoadingAllowedTimeTO);
        }

        public int InsertTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingAllowedTimeDAO.InsertTblLoadingAllowedTime(tblLoadingAllowedTimeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO)
        {
            return _iTblLoadingAllowedTimeDAO.UpdateTblLoadingAllowedTime(tblLoadingAllowedTimeTO);
        }

        public int UpdateTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingAllowedTimeDAO.UpdateTblLoadingAllowedTime(tblLoadingAllowedTimeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingAllowedTime(Int32 idLoadingTime)
        {
            return _iTblLoadingAllowedTimeDAO.DeleteTblLoadingAllowedTime(idLoadingTime);
        }

        public int DeleteTblLoadingAllowedTime(Int32 idLoadingTime, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingAllowedTimeDAO.DeleteTblLoadingAllowedTime(idLoadingTime, conn, tran);
        }

        #endregion
        
    }
}
