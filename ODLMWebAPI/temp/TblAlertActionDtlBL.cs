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
namespace ODLMWebAPI.BL
{
    public class TblAlertActionDtlBL : ITblAlertActionDtlBL
    {
        #region Selection
        
        public List<TblAlertActionDtlTO> SelectAllTblAlertActionDtlList()
        {
            return  TblAlertActionDtlDAO.SelectAllTblAlertActionDtl();
        }

        public TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 idAlertActionDtl)
        {
            return  TblAlertActionDtlDAO.SelectTblAlertActionDtl(idAlertActionDtl);
        }

        public TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 alertInstanceId,Int32 userId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblAlertActionDtlDAO.SelectTblAlertActionDtl(alertInstanceId, userId,conn,tran);
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
            return TblAlertActionDtlDAO.SelectTblAlertActionDtl(alertInstanceId, userId,conn,tran);
        }

        public List<TblAlertActionDtlTO> SelectAllTblAlertActionDtlList(Int32 userId)
        {
            return TblAlertActionDtlDAO.SelectAllTblAlertActionDtl(userId);
        }
        #endregion

        #region Insertion
        public int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO)
        {
            return TblAlertActionDtlDAO.InsertTblAlertActionDtl(tblAlertActionDtlTO);
        }

        public int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertActionDtlDAO.InsertTblAlertActionDtl(tblAlertActionDtlTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO)
        {
            return TblAlertActionDtlDAO.UpdateTblAlertActionDtl(tblAlertActionDtlTO);
        }

        public int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertActionDtlDAO.UpdateTblAlertActionDtl(tblAlertActionDtlTO, conn, tran);
        }


        public ResultMessage ResetAllAlerts(int loginUserId, List<TblAlertUsersTO> list, int result)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
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
                        TblAlertActionDtlTO existingAlertActionDtlTO = BL.TblAlertActionDtlBL.SelectTblAlertActionDtlTO(alertUsersTO.AlertInstanceId, Convert.ToInt32(loginUserId), conn, tran);
                        if (existingAlertActionDtlTO != null)
                        {
                            existingAlertActionDtlTO.ResetDate = Constants.ServerDateTime;
                            result = BL.TblAlertActionDtlBL.UpdateTblAlertActionDtl(existingAlertActionDtlTO, conn, tran);
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
                            tblAlertActionDtlTO.ResetDate = Constants.ServerDateTime;
                            goto xxx;
                        }
                    }

                    xxx:
                    tblAlertActionDtlTO.UserId = loginUserId;
                    tblAlertActionDtlTO.AcknowledgedOn = Constants.ServerDateTime;
                    tblAlertActionDtlTO.AlertInstanceId = alertUsersTO.AlertInstanceId;
                    result = BL.TblAlertActionDtlBL.InsertTblAlertActionDtl(tblAlertActionDtlTO);
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
            return TblAlertActionDtlDAO.DeleteTblAlertActionDtl(idAlertActionDtl);
        }

        public int DeleteTblAlertActionDtl(Int32 idAlertActionDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertActionDtlDAO.DeleteTblAlertActionDtl(idAlertActionDtl, conn, tran);
        }

        #endregion
        
    }
}
