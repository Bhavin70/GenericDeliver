using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using Microsoft.Extensions.Logging;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblBookingActionsBL : ITblBookingActionsBL
    {
        #region Selection

        public List<TblBookingActionsTO> SelectAllTblBookingActionsList()
        {
            return TblBookingActionsDAO.SelectAllTblBookingActions();
        }

        public TblBookingActionsTO SelectTblBookingActionsTO(Int32 idBookingAction)
        {
            return TblBookingActionsDAO.SelectTblBookingActions(idBookingAction);
        }

        public TblBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn,SqlTransaction tran)
        {
            return TblBookingActionsDAO.SelectLatestBookingActionTO(conn,tran);
        }

        public TblBookingActionsTO SelectLatestBookingActionTO()
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblBookingActionsTO tblBookingActionsTO = TblBookingActionsDAO.SelectLatestBookingActionTO(conn, tran);
                tran.Commit();
                return tblBookingActionsTO;
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

        #endregion

        #region Insertion
        public int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO)
        {
            return TblBookingActionsDAO.InsertTblBookingActions(tblBookingActionsTO);
        }

        public int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingActionsDAO.InsertTblBookingActions(tblBookingActionsTO, conn, tran);
        }

        public ResultMessage SaveBookingActions(TblBookingActionsTO tblBookingActionsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Clear All Previous Quota i.e. Deactivate All Prev Quotas If Booking Status is CLOSE

                if (tblBookingActionsTO.BookingStatus == "CLOSE")
                {
                    result = TblQuotaDeclarationDAO.DeactivateAllDeclaredQuota(tblBookingActionsTO.StatusBy, conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While DeactivateAllDeclaredQuota";
                        resultMessage.Tag = tblBookingActionsTO;
                        return resultMessage;
                    }
                }
                #endregion

                #region 2. Mark Booking Action Status

                result = InsertTblBookingActions(tblBookingActionsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblBookingActions";
                    resultMessage.Tag = tblBookingActionsTO;
                    return resultMessage;
                }

                #endregion

                #region 3. Notify All C&F For Booking Status. ---Pending

                //Saket [2018-01-31] Commented.
                //List<TblSmsTO> smsTOList = new List<TblSmsTO>();
                //Dictionary<int, string> cnfDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCTByOrgType(((int)Constants.OrgTypeE.C_AND_F_AGENT).ToString(), conn, tran);
                //foreach (var item in cnfDCT.Keys)
                //{
                //    TblSmsTO smsTO = new TblSmsTO();
                //    smsTO.MobileNo = cnfDCT[item];
                //    smsTO.SourceTxnDesc = "Booking Closed";
                //    smsTO.SmsTxt = "Bookings closed.";
                //    smsTOList.Add(smsTO);
                //}

                //Saket [2018-01-31]Added to send sms for cnf subsidary number
                List<TblSmsTO> smsTOList = new List<TblSmsTO>();

                Dictionary<int, string> cnfDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCTByOrgType(((int)Constants.OrgTypeE.C_AND_F_AGENT).ToString(), conn, tran);
                foreach (var item in cnfDCT.Keys)
                {
                    List<String> mobileNoList = new List<String>();

                    List<TblPersonTO> tblPersonTOList = BL.TblPersonBL.SelectAllPersonListByOrganization(item);

                    if (tblPersonTOList == null || tblPersonTOList.Count == 0)
                    {
                        tblPersonTOList = new List<TblPersonTO>();
                    }
                    TblPersonTO tblPersonTORegMobNo = new TblPersonTO();
                    tblPersonTORegMobNo.MobileNo = cnfDCT[item];
                    tblPersonTOList.Add(tblPersonTORegMobNo);

                    if (tblPersonTOList != null && tblPersonTOList.Count > 0)
                    {
                        for (int k = 0; k < tblPersonTOList.Count; k++)
                        {
                            TblSmsTO smsTO = new TblSmsTO();
                            smsTO.MobileNo = tblPersonTOList[k].MobileNo;

                            if (!mobileNoList.Contains(smsTO.MobileNo))
                            {
                                smsTO.SourceTxnDesc = "Booking Closed";
                                smsTO.SmsTxt = "Bookings closed.";
                                mobileNoList.Add(smsTO.MobileNo);
                                smsTOList.Add(smsTO);
                            }
                        }
                    }

                }


                //[17/01/2018]Added to send sms for role manager,director,loading person mobile number
                Dictionary<Int32, List<string>> roleDCT = new Dictionary<int, List<string>>();
                //Dictionary<Int32, string> roleDCT = new Dictionary<int, string>();
                //String roleIds = ((int)Constants.SystemRolesE.DIRECTOR + "," + (int)Constants.SystemRolesE.LOADING_PERSON + "," + (int)Constants.SystemRolesE.REGIONAL_MANAGER);
                //Sanjay [29 Nov 2018 Removed hardcoding and taken from config. Was already done in BRM ]
                String roleIds = string.Empty;
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ROLES_TO_SEND_SMS_ABOUT_RATE_AND_QUOTA,conn,tran);
                if (tblConfigParamsTO != null)
                {
                    roleIds = tblConfigParamsTO.ConfigParamVal;
                }

                if (!string.IsNullOrEmpty(roleIds))
                    roleDCT = BL.TblUserBL.SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(roleIds, false, conn, tran);

                if (roleDCT != null)
                {
                    foreach (var item in roleDCT.Keys)
                    {
                        List<string> list = roleDCT[item];
                        if (list != null && list.Count > 0)
                        {
                            for (int mn = 0; mn < list.Count; mn++)
                            {
                                TblSmsTO smsTOExist = smsTOList.Where(w => w.MobileNo == list[mn]).FirstOrDefault();
                                if (smsTOExist == null)
                                {
                                    TblSmsTO smsTO = new TblSmsTO();
                                    smsTO.MobileNo = list[mn];
                                    smsTO.SourceTxnDesc = "Booking Closed";
                                    smsTO.SmsTxt = "Bookings closed.";
                                    smsTOList.Add(smsTO);
                                }
                            }
                        }

                    }
                }


                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKINGS_CLOSED;
                tblAlertInstanceTO.AlertAction = "BOOKINGS_CLOSED";
                tblAlertInstanceTO.AlertComment = "Bookings closed.";
                tblAlertInstanceTO.EffectiveFromDate = tblBookingActionsTO.StatusDate;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "BOOKINGS_CLOSED";
                tblAlertInstanceTO.SourceEntityId = tblBookingActionsTO.IdBookingAction;
                tblAlertInstanceTO.RaisedBy = tblBookingActionsTO.StatusBy;
                tblAlertInstanceTO.RaisedOn = tblBookingActionsTO.StatusDate;
                tblAlertInstanceTO.IsAutoReset = 1;
                if (smsTOList != null)
                {
                    tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                    tblAlertInstanceTO.SmsTOList = smsTOList;
                }

                ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
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
                resultMessage.Text = "Booking Confirmed Sucessfully";
                resultMessage.Tag = tblBookingActionsTO;
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method SaveBookingActions";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Updation
        public int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO)
        {
            return TblBookingActionsDAO.UpdateTblBookingActions(tblBookingActionsTO);
        }

        public int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingActionsDAO.UpdateTblBookingActions(tblBookingActionsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblBookingActions(Int32 idBookingAction)
        {
            return TblBookingActionsDAO.DeleteTblBookingActions(idBookingAction);
        }

        public int DeleteTblBookingActions(Int32 idBookingAction, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingActionsDAO.DeleteTblBookingActions(idBookingAction, conn, tran);
        }

        #endregion

    }
}
