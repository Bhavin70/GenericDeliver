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
    public class TblAlertActionDtlBL : ITblAlertActionDtlBL
    {
        private readonly ITblAlertActionDtlDAO _iTblAlertActionDtlDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblAlertActionDtlBL(ICommon iCommon, ITblAlertActionDtlDAO iTblAlertActionDtlDAO, IConnectionString iConnectionString)
        {
            _iTblAlertActionDtlDAO = iTblAlertActionDtlDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblAlertActionDtlTO> SelectAllTblAlertActionDtlList()
        {
            return _iTblAlertActionDtlDAO.SelectAllTblAlertActionDtl();
        }

        public TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 idAlertActionDtl)
        {
            return _iTblAlertActionDtlDAO.SelectTblAlertActionDtl(idAlertActionDtl);
        }

        public TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 alertInstanceId,Int32 userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblAlertActionDtlDAO.SelectTblAlertActionDtl(alertInstanceId, userId,conn,tran);
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

        public TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 alertInstanceId, Int32 userId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblAlertActionDtlDAO.SelectTblAlertActionDtl(alertInstanceId, userId,conn,tran);
        }

        public List<TblAlertActionDtlTO> SelectAllTblAlertActionDtlList(Int32 userId)
        {
            return _iTblAlertActionDtlDAO.SelectAllTblAlertActionDtl(userId);
        }
        #endregion

        #region Insertion
        public int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO)
        {
            return _iTblAlertActionDtlDAO.InsertTblAlertActionDtl(tblAlertActionDtlTO);
        }

        public int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertActionDtlDAO.InsertTblAlertActionDtl(tblAlertActionDtlTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO)
        {
            return _iTblAlertActionDtlDAO.UpdateTblAlertActionDtl(tblAlertActionDtlTO);
        }

        public int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertActionDtlDAO.UpdateTblAlertActionDtl(tblAlertActionDtlTO, conn, tran);
        }


        public ResultMessage ResetAllAlerts(int loginUserId, List<TblAlertUsersTO> list, int result)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            ResultMessage resultMessage = new ResultMessage();
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                for (int i = 0; i < list.Count; i++)
                {

                    TblAlertUsersTO alertUsersTO = list[i];
                    TblAlertActionDtlTO tblAlertActionDtlTO = new TblAlertActionDtlTO();

                    alertUsersTO.IsReseted = 1;
                    if (alertUsersTO.IsReseted == 1)
                    {
                        //Check For Existence
                        TblAlertActionDtlTO existingAlertActionDtlTO = SelectTblAlertActionDtlTO(alertUsersTO.AlertInstanceId, Convert.ToInt32(loginUserId), conn, tran);
                        if (existingAlertActionDtlTO != null)
                        {
                            existingAlertActionDtlTO.ResetDate = _iCommon.ServerDateTime;
                            result = UpdateTblAlertActionDtl(existingAlertActionDtlTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.Text = "Error While UpdateTblAlertActionDtl";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }
                        else
                        {
                            tblAlertActionDtlTO.ResetDate = _iCommon.ServerDateTime;
                            goto xxx;
                        }
                    }

                    xxx:
                    tblAlertActionDtlTO.UserId = loginUserId;
                    tblAlertActionDtlTO.AcknowledgedOn = _iCommon.ServerDateTime;
                    tblAlertActionDtlTO.AlertInstanceId = alertUsersTO.AlertInstanceId;
                    result = InsertTblAlertActionDtl(tblAlertActionDtlTO);
                    if (result != 1)
                    {
                        resultMessage.Text = "Error While InsertTblAlertActionDtl";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;

                    }

                }

                tran.Commit();
                resultMessage.Text = "All Alert Reseted";
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Error While InsertTblAlertActionDtl";
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

        #region Deletion
        public int DeleteTblAlertActionDtl(Int32 idAlertActionDtl)
        {
            return _iTblAlertActionDtlDAO.DeleteTblAlertActionDtl(idAlertActionDtl);
        }

        public int DeleteTblAlertActionDtl(Int32 idAlertActionDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertActionDtlDAO.DeleteTblAlertActionDtl(idAlertActionDtl, conn, tran);
        }

        #endregion
        
    }
}
