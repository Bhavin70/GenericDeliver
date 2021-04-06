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
    public class TblAlertUsersBL : ITblAlertUsersBL
    {
        private readonly ITblAlertUsersDAO _iTblAlertUsersDAO;
        private readonly ITblAlertActionDtlDAO _iTblAlertActionDtlDAO;
        private readonly ITblModuleBL _iTblModuleBL;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblAlertUsersBL(ITblModuleBL iTblModuleBL, ICommon iCommon, IConnectionString iConnectionString, ITblAlertUsersDAO iTblAlertUsersDAO, ITblAlertActionDtlDAO iTblAlertActionDtlDAO)
        {
            _iConnectionString = iConnectionString;
            _iTblAlertUsersDAO = iTblAlertUsersDAO;
            _iTblAlertActionDtlDAO = iTblAlertActionDtlDAO;
            _iTblModuleBL = iTblModuleBL;
            _iCommon = iCommon;
        }
        #region Selection
        public List<TblAlertUsersTO> SelectAllTblAlertUsersList()
        {
            return _iTblAlertUsersDAO.SelectAllTblAlertUsers();
        }

        public TblAlertUsersTO SelectTblAlertUsersTO(Int32 idAlertUser)
        {
            return _iTblAlertUsersDAO.SelectTblAlertUsers(idAlertUser);
        }

        public List<TblAlertUsersTO> SelectAllActiveNotAKAlertList(Int32 userId, Int32 roleId)
        {
            return _iTblAlertUsersDAO.SelectAllActiveNotAKAlertList(userId, roleId);
        }
        // check Logout Entry user Tracking
        public int CheckUserLogin(int loginId, int ModuleId, int userId)
        {

            int result = _iCommon.CheckLogOutEntry(loginId);
            if (result == 0 && ModuleId != 0 && userId != 0)
            {
                TblModuleTO tblModule = _iTblModuleBL.GetAllActiveAllowedCnt(ModuleId, userId, loginId);
                if ((tblModule.NoOfActiveLicenseCnt > tblModule.NoOfAllowedLicenseCnt) && tblModule.IsImpPerson != 1)
                {
                    return 1;
                }
                if ((tblModule.ImpPersonCount > tblModule.NoOfAllowedLicenseCnt) && tblModule.IsImpPerson == 1)
                {
                    return 1;
                }
                int userActive = _iCommon.IsUserDeactivate(userId);
                if (userActive == 0)
                {
                    // return 1;
                    return 2; //user Deactivate
                }
            }
            return result;
        }
        //End
        public List<TblAlertUsersTO> SelectAllActiveAlertList(Int32 userId, List<TblUserRoleTO> tblUserRoleToList, int loginId, int ModuleId)
        {

            //UserTracking
            int result = CheckUserLogin(loginId, ModuleId, userId);
            //End
            String roleIds = String.Empty;
            if (tblUserRoleToList != null && tblUserRoleToList.Count > 0)
            {
                var stringsArray = tblUserRoleToList.Select(i => i.RoleId.ToString()).ToArray();
                roleIds = string.Join(",", stringsArray);
            }

            List<TblAlertUsersTO> list = _iTblAlertUsersDAO.SelectAllActiveAlertList(userId, roleIds);
            List<TblAlertActionDtlTO> alertActionDtlTOList = _iTblAlertActionDtlDAO.SelectAllTblAlertActionDtl(userId);
            if (alertActionDtlTOList != null)
            {
                for (int i = 0; i < list.Count; i++)
                {

                    list[i].NotificationTimeHint = _iCommon.getNotificationTimeHint(list[i].RaisedOn);
                    var isAck = alertActionDtlTOList.Where(a => a.AlertInstanceId == list[i].AlertInstanceId).LastOrDefault();
                    if (isAck != null)
                    {
                        if (isAck.ResetDate != DateTime.MinValue)
                        {


                            list.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            //Hrushikesh added to not to filter snoozed notification

                            list[i].IsAcknowledged = 1;
                            // while time is greater than snooze time dont show notification
                            if (isAck.SnoozeOn > _iCommon.ServerDateTime)
                            {

                                //Removed from list
                                list.RemoveAt(i);
                                i--;
                            }
                            else if (isAck.SnoozeOn < _iCommon.ServerDateTime && isAck.SnoozeOn > DateTime.MinValue)
                            {
                                list[i].IsAcknowledged = 0;
                            }

                        }



                    }
                }


                //list = list.OrderByDescending(a => a.IsAcknowledged==0).ThenBy(a=>a.AlertInstanceId).ToList();
                list = list.OrderByDescending(a => a.RaisedOn).ThenBy(a => a.AlertInstanceId).ToList();
                if (list != null && list.Count > 0)
                {
                    List<TblAlertUsersTO> accList = list.Where(w => w.IsAcknowledged == 1).ToList();

                    List<TblAlertUsersTO> notAccList = list.Where(w => w.IsAcknowledged == 0).ToList();

                    list = new List<TblAlertUsersTO>();

                    list.AddRange(notAccList);
                    list.AddRange(accList);
                }

            }
            if (list != null && list.Count > 0)
            {

                list = list.GroupBy(ele => ele.AlertInstanceId).Select(s => s.FirstOrDefault()).ToList();
            }

            //UserTracking/
            // if(result==1)
            if (result == 1 || result == 2)
            {
                //logout user by loginID
                var InvalidSessionHis = new TblModuleCommHisTO();
                InvalidSessionHis.LoginId = loginId;
                _iTblModuleBL.UpdateTblModuleCommHisBeforeLogin(InvalidSessionHis, null, null);
            }
            if (list != null && list.Count > 0)
            {
                list[0].IsLogOut = result;
                if (result == (int)Constants.LogoutValueE.LogoutWithTimer || result == (int)Constants.LogoutValueE.DirectLogout)
                {
                    list[0].IsLogOut = (int)Constants.LogoutValueE.DirectLogout;
                }
            }
            else
            {
                TblAlertUsersTO a = new TblAlertUsersTO();
                if (result == (int)Constants.LogoutValueE.LogoutWithTimer)
                {
                    result = (int)Constants.LogoutValueE.DirectLogout; ;
                }
                //Hrushikesh added to not to show empty notification
                a.IsAcknowledged = 1;
                //Prajakta[2021-04-6] dont show empty notification when we dont want to logout
                if (result != (int)Constants.LogoutValueE.DirectLogout)
                {
                    a.IsLogOut = result;
                    list.Add(a);
                }
            }
            //end
            return list;
        }

        #endregion

        #region Insertion
        public int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            return _iTblAlertUsersDAO.InsertTblAlertUsers(tblAlertUsersTO);
        }

        public int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertUsersDAO.InsertTblAlertUsers(tblAlertUsersTO, conn, tran);
        }

        #endregion

        #region Updation


        public ResultMessage snoozeTblAlert(int alertUserId, int snoozeTime, int tranType)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMSg = new ResultMessage();
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();

                TblAlertUsersTO alertTO = SelectTblAlertUsersTO(alertUserId);
                if (alertTO == null)
                {
                    resultMSg.DefaultExceptionBehaviour(null, "Exception in snooze");
                    tran.Rollback();
                    return resultMSg;
                }

                //snooze
                //if (tranType == 1)
                //{
                alertTO.SnoozeCount++;
                alertTO.SnoozeDate = _iCommon.ServerDateTime.AddMinutes(snoozeTime);
                //}
                //reset
                //else
                //{

                //    //alertTO.IsReseted = 1;
                //    //alertTO.SnoozeDate = _iCommon.ServerDateTime;
                //}


                result = UpdateTblAlertUsers(alertTO, conn, tran);
                if (result != 1)
                {
                    resultMSg.DefaultExceptionBehaviour(null, "Exception in snooze");
                    tran.Rollback();
                    return resultMSg;
                }
                resultMSg.DefaultSuccessBehaviour();
                tran.Commit();
                return resultMSg;

            }
            catch (Exception ex)
            {
                resultMSg.DefaultExceptionBehaviour(ex, "");
                return resultMSg;
            }
            finally
            {
                conn.Close();
            }

        }


        public int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            return _iTblAlertUsersDAO.UpdateTblAlertUsers(tblAlertUsersTO);
        }

        public int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertUsersDAO.UpdateTblAlertUsers(tblAlertUsersTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblAlertUsers(Int32 idAlertUser)
        {
            return _iTblAlertUsersDAO.DeleteTblAlertUsers(idAlertUser);
        }

        public int DeleteTblAlertUsers(Int32 idAlertUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertUsersDAO.DeleteTblAlertUsers(idAlertUser, conn, tran);
        }

        #endregion

    }
}
