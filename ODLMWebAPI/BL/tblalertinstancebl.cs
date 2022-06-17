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
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{  
    public class TblAlertInstanceBL : ITblAlertInstanceBL
    { 
        private readonly ITblAlertInstanceDAO _iTblAlertInstanceDAO;
        private readonly ITblAlertDefinitionBL _iTblAlertDefinitionBL;
        private readonly ITblAlertActionDtlDAO _iTblAlertActionDtlDAO;
        private readonly ITblAlertSubscriptSettingsBL _iTblAlertSubscriptSettingsBL;
        private readonly ITblAlertUsersBL _iTblAlertUsersBL;
        private readonly ITblUserBL _iTblUserBL;
        private readonly IVitplNotify _iVitplNotify;
        private readonly ISendMailBL _iSendMailBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly IVitplSMS _iVitplSMS;
        private readonly ITblSmsBL _iTblSmsBL;
        private readonly IConnectionString _iConnectionString;

        public TblAlertInstanceBL(IConnectionString iConnectionString, ITblSmsBL iTblSmsBL, IVitplSMS iVitplSMS, ITblConfigParamsBL iTblConfigParamsBL, ISendMailBL iSendMailBL, IVitplNotify iVitplNotify, ITblUserBL iTblUserBL, ITblAlertUsersBL iTblAlertUsersBL, ITblAlertSubscriptSettingsBL iTblAlertSubscriptSettingsBL, ITblAlertDefinitionBL iTblAlertDefinitionBL, ITblAlertInstanceDAO iTblAlertInstanceDAO, ITblAlertActionDtlDAO iTblAlertActionDtlDAO)
        {
            _iTblAlertActionDtlDAO = iTblAlertActionDtlDAO;
            _iTblAlertInstanceDAO = iTblAlertInstanceDAO;
            _iTblAlertDefinitionBL = iTblAlertDefinitionBL;
            _iTblAlertSubscriptSettingsBL = iTblAlertSubscriptSettingsBL;
            _iTblAlertUsersBL = iTblAlertUsersBL;
            _iTblUserBL = iTblUserBL;
            _iVitplNotify = iVitplNotify;
            _iSendMailBL = iSendMailBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iVitplSMS = iVitplSMS;
            _iTblSmsBL = iTblSmsBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection


        public void postSnoozeForAndroid()
        {
            List<TblAlertActionDtlTO> actionList = _iTblAlertActionDtlDAO.SelectAllTblAlertActionDtlOnTime();
            if (actionList == null || actionList.Count == 0)
                return;
           var actionGroupList = actionList.GroupBy(e => e.AlertInstanceId).ToList();
            List<TblAlertActionDtlTO> finalActionList = new List<TblAlertActionDtlTO>();
            actionGroupList.ForEach(ele =>
            {
                List<TblAlertActionDtlTO> actionGroup = ele.ToList();
                String[] deviceList = new String[actionGroup.Count];
                TblAlertActionDtlTO finalTO = new TblAlertActionDtlTO();
                for (int i = 0; i < actionGroup.Count; i++)
                {
                    if (i == 0)
                    {
                        finalTO = actionGroup[i].Clone();
                        finalTO.DeviceId = "";
                    }
                    deviceList[i] = actionGroup[i].DeviceId;

                }
                finalTO.DeviceList = deviceList;
                finalActionList.Add(finalTO);

            });

            for (int i = 0; i < finalActionList.Count; i++)
            {
            

                String notifyBody = actionList[i].AlertComment;
                String notifyTitle = actionList[i].DefDesc;
               // String[] devices = new String[1];
               // devices[0] = actionList[i].DeviceId;
                _iVitplNotify.NotifyToRegisteredDevices(finalActionList[i].DeviceList, notifyBody, notifyTitle, actionList[i].AlertInstanceId);


            }

        }

        public List<TblAlertInstanceTO> SelectAllTblAlertInstanceList()
        {
            return _iTblAlertInstanceDAO.SelectAllTblAlertInstance();
        }

        public TblAlertInstanceTO SelectTblAlertInstanceTO(Int32 idAlertInstance)
        {
            return _iTblAlertInstanceDAO.SelectTblAlertInstance(idAlertInstance);
        }

        public List<TblAlertInstanceTO> SelectAllTblAlertInstanceList(Int32 userId, Int32 roleId)
        {
            return _iTblAlertInstanceDAO.SelectAllTblAlertInstance();
        }

        #endregion



        #region Insertion
        //Save New Alert
        public ResultMessage SaveAlertInstance(TblAlertInstanceTO alertInstanceTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save New Alert Instance

                resultMessage = SaveNewAlertInstance(alertInstanceTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Error While Inserting New Alert");
                    return resultMessage;
                }

                #endregion




                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception)
            {
                tran.Rollback();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DefaultBehaviour("Error While Inserting New Alert");
                return resultMessage;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }


        public ResultMessage SaveNewAlertInstance(TblAlertInstanceTO alertInstanceTO, SqlConnection conn, SqlTransaction tran)
        {

            String commSprtedConcernedUsers = string.Empty;
            string hodUserIdList = string.Empty;
            Dictionary<int, Dictionary<int, string>> levelUserIdNameDCT = new Dictionary<int, Dictionary<int, string>>();
           

            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                #region Sanjay [21 Sept 2018] Reset Given Alert List

                ResetPrevAlertsIfAny(alertInstanceTO, conn, tran);

                #endregion

                // 1. Get Alert Definition
                TblAlertDefinitionTO mstAlertDefinitionTO = _iTblAlertDefinitionBL.SelectTblAlertDefinitionTO(alertInstanceTO.AlertDefinitionId, conn, tran);
                if (mstAlertDefinitionTO == null)
                {
                    resultMessage.Text = "TblAlertDefinitionTO Found NULL. Alert Definition is not given for this alert";
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    return resultMessage;
                }

                List<TblAlertSubscriptSettingsTO> channelTOList = new List<TblAlertSubscriptSettingsTO>();
                List<TblAlertUsersTO> alertUsersTOList = new List<TblAlertUsersTO>();

                //Boolean checkCustomAlert = false;

                if (alertInstanceTO.EscalationOn == DateTime.MinValue)
                {
                    // 2. Check If subscription and subscribed users & Channels
                    if (mstAlertDefinitionTO.AlertSubscribersTOList == null || mstAlertDefinitionTO.AlertSubscribersTOList.Count == 0)
                    {
                        //checkCustomAlert = true;

                        //resultMessage.Text = "Subscribers Not Found";
                        //resultMessage.MessageType = ResultMessageE.Information;
                        //resultMessage.Result = 1;
                        //return resultMessage;
                    }

                    #region Check Custom Alert & User

                    List<TblAlertSubscriptSettingsTO> alertSubscriptSettingsTOListListForCustom = _iTblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsListByAlertDefId(alertInstanceTO.AlertDefinitionId, conn, tran);

                    if (alertSubscriptSettingsTOListListForCustom != null && alertSubscriptSettingsTOListListForCustom.Count > 0)
                    {
                        channelTOList.AddRange(alertSubscriptSettingsTOListListForCustom);

                        if (alertInstanceTO != null && alertInstanceTO.AlertUsersTOList != null && alertInstanceTO.AlertUsersTOList.Count > 0)
                        {
                            for (int k = 0; k < alertInstanceTO.AlertUsersTOList.Count; k++)
                            {
                                alertInstanceTO.AlertUsersTOList[k].AlertSubscriptSettingsTOList = new List<TblAlertSubscriptSettingsTO>();
                                alertInstanceTO.AlertUsersTOList[k].AlertSubscriptSettingsTOList.AddRange(alertSubscriptSettingsTOListListForCustom);
                                alertUsersTOList.Add(alertInstanceTO.AlertUsersTOList[k]);
                            }
                        }

                        alertInstanceTO.AlertUsersTOList = new List<TblAlertUsersTO>();

                    }


                    #endregion

                    for (int i = 0; i < mstAlertDefinitionTO.AlertSubscribersTOList.Count; i++)
                    {
                        TblAlertSubscribersTO mstAlertDefinitionSubscribersTO = mstAlertDefinitionTO.AlertSubscribersTOList[i];
                        TblAlertUsersTO alertUsersTO = new TblAlertUsersTO();
                        alertUsersTO.AlertSubscriptSettingsTOList = mstAlertDefinitionSubscribersTO.AlertSubscriptSettingsTOList;
                        alertUsersTO.UserId = mstAlertDefinitionSubscribersTO.UserId;
                        alertUsersTO.RoleId = mstAlertDefinitionSubscribersTO.RoleId;

                        channelTOList.AddRange(alertUsersTO.AlertSubscriptSettingsTOList);
                        alertUsersTOList.Add(alertUsersTO);
                    }
                }
                else
                {
                    #region Escalation --- Can be done afterwards
                    // 2. Check If Escalation and Escalation users & Channels
                    //if (mstAlertDefinitionTO.MstAlertDefinitionEscalationSettingsTOList == null || mstAlertDefinitionTO.MstAlertDefinitionEscalationSettingsTOList.Count == 0)
                    //{
                    //    VegaERPFrameWork.VErrorList.Add("Escalation Not Found for Alert Def :" + mstAlertDefinitionTO.AlertDefinitionDesc, EMessageType.Error, null, null);

                    //    return 1;
                    //    //return 0;
                    //}

                    //if (alertInstanceTO.MstAlertDefinitionEscalationSettingsTOList != null)
                    //{
                    //    mstAlertDefinitionTO.MstAlertDefinitionEscalationSettingsTOList = alertInstanceTO.MstAlertDefinitionEscalationSettingsTOList;
                    //}

                    //for (int i = 0; i < mstAlertDefinitionTO.MstAlertDefinitionEscalationSettingsTOList.Count; i++)
                    //{
                    //    TO.MstAlertDefinitionEscalationSettingsTO mstAlertDefinitionEscalationSettingsTO = mstAlertDefinitionTO.MstAlertDefinitionEscalationSettingsTOList[i];

                    //    TO.AlertUsersTO alertUsersTO = new AlertUsersTO();
                    //    alertUsersTO.AlertSubscriptionCommSettingsTOList = mstAlertDefinitionEscalationSettingsTO.AlertSubscriptionCommSettingsTOList;
                    //    alertUsersTO.UserId = mstAlertDefinitionEscalationSettingsTO.UserId;
                    //    alertUsersTO.RoleId = mstAlertDefinitionEscalationSettingsTO.RoleId;
                    //    alertUsersTO.DeptId = mstAlertDefinitionEscalationSettingsTO.DeptId;
                    //    alertUsersTO.IsHierarchicalAlert = mstAlertDefinitionEscalationSettingsTO.IsHierarchicalAlert;

                    //    channelTOList.AddRange(alertUsersTO.AlertSubscriptionCommSettingsTOList);
                    //    alertUsersTOList.Add(alertUsersTO);
                    //}
                    #endregion
                }


                // 3. Insert Alert Instance
                int result = InsertTblAlertInstance(alertInstanceTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblAlertInstance";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                var channelList = channelTOList.GroupBy(c => c.NotificationTypeId).ToList();

                Dictionary<Int32, string> regDeviceDCT = new Dictionary<int, string>();

                // 4. Insert alert Instance Users according to communication channels
                for (int c = 0; c < channelList.Count; c++)
                {

                    #region Dashboard alert
                    if (channelList[c].Key == (int)NotificationConstants.NotificationTypeE.ALERT)
                    {

                        var userList = (from x in alertUsersTOList
                                        where x.AlertSubscriptSettingsTOList.Any(b => b.NotificationTypeId == (int)NotificationConstants.NotificationTypeE.ALERT)
                                        select x).ToList();

                        for (int auCnt = 0; auCnt < userList.Count; auCnt++)
                        {
                            userList[auCnt].AlertInstanceId = alertInstanceTO.IdAlertInstance;

                            result = _iTblAlertUsersBL.InsertTblAlertUsers(userList[auCnt], conn, tran);
                            if (result != 1)
                            {
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While InsertTblAlertUsers";
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }
                        List<string> broadCastinguserList = new List<string>();
                        var userIds = string.Join(",", userList.Where(p => p.UserId > 0)
                                 .Select(p => p.UserId.ToString()));

                        if (!string.IsNullOrEmpty(userIds))
                        {
                            Dictionary<Int32, string> userDeviceDCT = new Dictionary<int, string>();
                            userDeviceDCT = _iTblUserBL.SelectUserDeviceRegNoDCTByUserIdOrRole(userIds, true, conn, tran);
                            broadCastinguserList.AddRange(userList.Where(p => p.UserId > 0)
                                .Select(p => p.UserId.ToString()));
                            if (userDeviceDCT != null && userDeviceDCT.Count > 0)
                                regDeviceDCT = userDeviceDCT;
                        }

                        // As per discussion with Nitin Kabra Sir 31-03-2017 ,Do Not Consider C&F Agent as for C&F Agent SMS will be sent on registered mobile number of the firm.

                        //var roleIds = string.Join(",", userList.Where(p => p.RoleId > 0 && p.RoleId != (int)Constants.SystemRolesE.C_AND_F_AGENT)
                        //      .Select(p => p.RoleId.ToString()));

                        var roleIds = string.Join(",", userList.Where(p => p.RoleId > 0) //As Saket suggested to change this. 
                             .Select(p => p.RoleId.ToString()));


                        if (!string.IsNullOrEmpty(roleIds))
                        {
                            Dictionary<Int32, string> roleDeviceDCT = new Dictionary<int, string>();
                            Dictionary<Int32, string> usersOnRoleDic = new Dictionary<int, string>();
                            roleDeviceDCT = _iTblUserBL.SelectUserDeviceRegNoDCTByUserIdOrRole(roleIds, false, conn, tran);
                            usersOnRoleDic = _iTblUserBL.SelectUserUsingRole(roleIds, false, conn, tran);
                            if (roleDeviceDCT != null && roleDeviceDCT.Count > 0)
                            {
                                foreach (var item in roleDeviceDCT.Keys)
                                {
                                    if (!regDeviceDCT.ContainsKey(item))
                                    {
                                        regDeviceDCT.Add(item, roleDeviceDCT[item]);
                                    }
                                }

                            }
                            if (usersOnRoleDic != null && usersOnRoleDic.Count > 0)
                            {
                                foreach (var item in usersOnRoleDic.Keys)
                                {
                                    broadCastinguserList.Add(Convert.ToString(item));
                                }
                            }
                        }
                        alertInstanceTO.BroadCastinguserList = broadCastinguserList;
                        if (alertInstanceTO.BroadCastinguserList != null && alertInstanceTO.BroadCastinguserList.Count > 0)
                        {
                            //added code by @kiran for send broadcasting msg using thread @21/11/2018
                            Thread thread = new Thread(delegate ()
                            {
                                _iVitplNotify.SystembrodCasting(alertInstanceTO);
                            });
                            thread.Start();
                        }
                    }
                    #endregion

                    #region Send Alert Email

                    else if (channelList[c].Key == (int)NotificationConstants.NotificationTypeE.EMAIL)
                    {
                        // added by aniket
                        var userList = (from x in alertUsersTOList
                                        where x.AlertSubscriptSettingsTOList.Any(b => b.NotificationTypeId == (int)NotificationConstants.NotificationTypeE.EMAIL)
                                        select x).ToList();
                        if(userList!=null)
                        {
                            List<SendMail> emailTOList = new List<SendMail>();
                            var userIds = string.Join(",", userList.Where(p => p.UserId > 0)
                                  .Select(p => p.UserId.ToString()));
                            if (!string.IsNullOrEmpty(userIds))
                            {
                                Dictionary<Int32, string> userDCT = new Dictionary<int, string>();
                                userDCT = _iTblUserBL.SelectUserEmailDCTByUserIdOrRole(userIds, true, conn, tran);

                                if (userDCT != null)
                                {
                                    foreach (var item in userDCT.Values)
                                    {
                                        SendMail emailTO = new SendMail();
                                        emailTO.Subject = alertInstanceTO.AlertAction;
                                        emailTO.BodyContent = alertInstanceTO.AlertComment;
                                        emailTO.To =item;
                                        emailTOList.Add(emailTO);
                                    }
                                }
                            }
                            var roleIds = string.Join(",", userList.Where(p => p.RoleId > 0)
                                 .Select(p => p.RoleId.ToString()));
                            if (!string.IsNullOrEmpty(roleIds))
                            {
                                Dictionary<Int32, string> roleDCT = new Dictionary<int, string>();
                                roleDCT = _iTblUserBL.SelectUserEmailDCTByUserIdOrRole(roleIds, false, conn, tran);

                                if (roleDCT != null)
                                {
                                    foreach (var item in roleDCT.Values)
                                    {
                                        SendMail emailTO = new SendMail();
                                        emailTO.Subject = alertInstanceTO.AlertAction;
                                        emailTO.BodyContent = alertInstanceTO.AlertComment;
                                        emailTO.To = item;
                                        emailTOList.Add(emailTO);
                                    }
                                }
                            }
                            foreach (SendMail item in emailTOList)
                            {
                                _iSendMailBL.SendEmailNotification(item);
                            }
                        }
                  
                       

                    }
                    #endregion

                    #region SMS

                    else if (channelList[c].Key == (int)NotificationConstants.NotificationTypeE.SMS)
                    {
                        var userList = (from x in alertUsersTOList
                                        where x.AlertSubscriptSettingsTOList.Any(b => b.NotificationTypeId == (int)NotificationConstants.NotificationTypeE.SMS)
                                        select x).ToList();

                        //Get Mobile No Dtls

                        if (userList != null)
                        {
                            List<TblSmsTO> smsTOList = new List<TblSmsTO>();

                            var userIds = string.Join(",", userList.Where(p => p.UserId > 0)
                                  .Select(p => p.UserId.ToString()));

                            if (!string.IsNullOrEmpty(userIds))
                            {
                                Dictionary<Int32, string> userDCT = new Dictionary<int, string>();
                                userDCT = _iTblUserBL.SelectUserMobileNoDCTByUserIdOrRole(userIds, true, conn, tran);

                                if (userDCT != null)
                                {
                                    //Reshma Added
                                    if (alertInstanceTO.SmsTOList != null && alertInstanceTO.SmsTOList.Count > 0)
                                    {
                                        alertInstanceTO.SmsComment = alertInstanceTO.SmsTOList[0].SmsTxt ;
                                    }
                                    foreach (var item in userDCT.Keys)
                                    {
                                        TblSmsTO smsTO = new TblSmsTO();
                                        smsTO.MobileNo = userDCT[item];
                                        smsTO.SourceTxnDesc = alertInstanceTO.SourceDisplayId;
                                        smsTO.SmsTxt = alertInstanceTO.AlertComment;
                                        //Vijaymala added[03-05-2018]:added to change notification with party name but sms is not modified 
                                        if (!String.IsNullOrEmpty(alertInstanceTO.SmsComment))
                                            smsTO.SmsTxt = alertInstanceTO.SmsComment;
                                        smsTOList.Add(smsTO);
                                    }
                                }
                            }

                            // As per discussion with Nitin Kabra Sir 31-03-2017 ,Do Not Consider C&F Agent as for C&F Agent SMS will be sent on registered mobile number of the firm.

                            //var roleIds = string.Join(",", userList.Where(p => p.RoleId > 0 && p.RoleId != (int)Constants.SystemRolesE.C_AND_F_AGENT)
                            //      .Select(p => p.RoleId.ToString()));

                            //Sanjay [29 Nov 2018] Commented the cnf agent role from list as not applicable in generic DELIVER condition.
                            var roleIds = string.Join(",", userList.Where(p => p.RoleId > 0)
                                  .Select(p => p.RoleId.ToString()));


                            if (!string.IsNullOrEmpty(roleIds))
                            {
                                Dictionary<Int32, string> roleDCT = new Dictionary<int, string>();
                                roleDCT = _iTblUserBL.SelectUserMobileNoDCTByUserIdOrRole(roleIds, false, conn, tran);

                                if (roleDCT != null)
                                {
                                    foreach (var item in roleDCT.Keys)
                                    {
                                        TblSmsTO smsTO = new TblSmsTO();
                                        smsTO.MobileNo = roleDCT[item];
                                        smsTO.SourceTxnDesc = alertInstanceTO.SourceDisplayId;
                                        smsTO.SmsTxt = alertInstanceTO.AlertComment;
                                        if (!String.IsNullOrEmpty(alertInstanceTO.SmsComment))
                                            smsTO.SmsTxt = alertInstanceTO.SmsComment;
                                        smsTOList.Add(smsTO);
                                    }
                                }
                            }

                            if (smsTOList != null && smsTOList.Count > 0)
                            {
                                if (alertInstanceTO.SmsTOList == null)
                                    alertInstanceTO.SmsTOList = smsTOList;
                                else
                                {
                                    alertInstanceTO.SmsTOList.AddRange(smsTOList);
                                }
                            }
                        }

                    }

                    #endregion

                }

                #region Dashboard Alert For Organizations

                if (alertInstanceTO.AlertUsersTOList != null)
                {
                    for (int auCnt = 0; auCnt < alertInstanceTO.AlertUsersTOList.Count; auCnt++)
                    {
                        alertInstanceTO.AlertUsersTOList[auCnt].AlertInstanceId = alertInstanceTO.IdAlertInstance;

                        result = _iTblAlertUsersBL.InsertTblAlertUsers(alertInstanceTO.AlertUsersTOList[auCnt], conn, tran);
                        if (result != 1)
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblAlertUsers";
                            resultMessage.Result = 0;
                            return resultMessage;
                        }

                        if (!regDeviceDCT.ContainsKey(alertInstanceTO.AlertUsersTOList[auCnt].UserId))
                        {
                            if (!string.IsNullOrEmpty(alertInstanceTO.AlertUsersTOList[auCnt].DeviceId))
                                regDeviceDCT.Add(alertInstanceTO.AlertUsersTOList[auCnt].UserId, alertInstanceTO.AlertUsersTOList[auCnt].DeviceId);
                        }
                    }
                }

                // Call to FCM Notification Webrequest. This is currently synchronous webrequest call as its async call is not working
                // If we observed slower performance we may need o change the call

                if (regDeviceDCT != null && regDeviceDCT.Count > 0)
                {
                    string[] devices = new string[regDeviceDCT.Count];
                    String notifyBody = alertInstanceTO.AlertComment;
                    String notifyTitle = mstAlertDefinitionTO.AlertDefDesc;
                    int array = 0;
                    foreach (var item in regDeviceDCT.Keys)
                    {
                        devices[array] = regDeviceDCT[item];
                        array++;
                    }

                    //string[] devices = new string[1];
                    //String notifyBody = alertInstanceTO.AlertComment;
                    //String notifyTitle = mstAlertDefinitionTO.AlertDefDesc;
                    //int array = 0;
                  
                    //    devices[0] = "zxczxcj7394jzxalkcj";
                    //    array++;
                  

                    _iVitplNotify.NotifyToRegisteredDevices(devices, notifyBody, notifyTitle, alertInstanceTO.IdAlertInstance);
                }

                #endregion

                #region Send SMS

                TblConfigParamsTO smsActivationConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.SMS_SUBSCRIPTION_ACTIVATION, conn, tran);
                Int32 smsActive = 0;
                if (smsActivationConfTO != null)
                    smsActive = Convert.ToInt32(smsActivationConfTO.ConfigParamVal);

                if (smsActive == 1)
                {
                    if (alertInstanceTO.SmsTOList != null && alertInstanceTO.SmsTOList.Count > 0)
                    {
                        for (int sms = 0; sms < alertInstanceTO.SmsTOList.Count; sms++)
                        {
                            if (alertInstanceTO.SmsTOList[sms].MobileNo!=null && alertInstanceTO.SmsTOList[sms].MobileNo.Length >=10)
                            {
                                String smsResponse = _iVitplSMS.SendSMSAsync(alertInstanceTO.SmsTOList[sms]);
                                alertInstanceTO.SmsTOList[sms].ReplyTxt = smsResponse;
                                alertInstanceTO.SmsTOList[sms].AlertInstanceId = alertInstanceTO.IdAlertInstance;
                                alertInstanceTO.SmsTOList[sms].SentOn = alertInstanceTO.RaisedOn;

                                result = _iTblSmsBL.InsertTblSms(alertInstanceTO.SmsTOList[sms], conn, tran);
                            }
                        }
                    }
                }

                #endregion



                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Alert Sent Successfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception In Method SaveNewAlertInstance(AlertInstanceTO alertInstanceTO, SqlConnection conn, SqlTransaction tran)";
                return resultMessage;
            }
        }

        private void ResetPrevAlertsIfAny(TblAlertInstanceTO alertInstanceTO, SqlConnection conn, SqlTransaction tran)
        {
            if (alertInstanceTO.AlertsToReset != null)
            {
                if (alertInstanceTO.AlertsToReset.AlertDefIdList != null && alertInstanceTO.AlertsToReset.AlertDefIdList.Count > 0)
                {
                    String alertDefIds = String.Join(",", alertInstanceTO.AlertsToReset.AlertDefIdList);
                    ResetAlertInstanceByDef(alertDefIds, conn, tran);
                }

                if (alertInstanceTO.AlertsToReset.ResetAlertInstanceTOList != null && alertInstanceTO.AlertsToReset.ResetAlertInstanceTOList.Count > 0)
                {
                    foreach (var item in alertInstanceTO.AlertsToReset.ResetAlertInstanceTOList)
                    {
                        int alertDefId = item.AlertDefinitionId;
                        int sourceEntityId = item.SourceEntityTxnId;
                        int userId = item.UserId;
                        ResetAlertInstance(alertDefId, sourceEntityId, userId,  conn, tran);
                    }
                }
            }
        }

        //Deepali.... 24/10/2018  for Reset old alerts
        public void ResetOldAlerts(TblAlertInstanceTO alertInstanceTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran =conn.BeginTransaction();
                
                ResetPrevAlertsIfAny(alertInstanceTO, conn, tran);
            }
            catch (Exception e)
            {

            }
            finally
            {
               
                tran.Commit();
                 conn.Close();
            }

        }

        public ResultMessage AutoResetAndDeleteAlerts()
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //To Delete The alerts , we need to delete alertUser, alertUserActions and dependentSMSs
                // We will incorporate this logic later
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELETE_ALERT_BEFORE_DAYS, conn, tran);
                Int32 delBforeDays = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                DateTime cancellationDateTime = DateTime.MinValue;


                //Reset All alert Instances which are having isAutoReset = 1
                int result = _iTblAlertInstanceDAO.ResetAutoResetAlertInstances(conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In ResetAutoResetAlertInstances";
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Alerts Resetted Sucessfully";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error in Method AutoResetAndDeleteAlerts at BL Level";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO)
        {
            return _iTblAlertInstanceDAO.InsertTblAlertInstance(tblAlertInstanceTO);
        }

        public int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertInstanceDAO.InsertTblAlertInstance(tblAlertInstanceTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO)
        {
            return _iTblAlertInstanceDAO.UpdateTblAlertInstance(tblAlertInstanceTO);
        }

        public int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertInstanceDAO.UpdateTblAlertInstance(tblAlertInstanceTO, conn, tran);
        }

        public int ResetAlertInstance(int alertDefId, int sourceEntityId, int userId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertInstanceDAO.ResetAlertInstance(alertDefId, sourceEntityId, userId, conn, tran);
        }

        public int ResetAlertInstanceByDef(string alertDefIds, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertInstanceDAO.ResetAlertInstanceByDef(alertDefIds, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblAlertInstance(Int32 idAlertInstance)
        {
            return _iTblAlertInstanceDAO.DeleteTblAlertInstance(idAlertInstance);
        }

        public int DeleteTblAlertInstance(Int32 idAlertInstance, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertInstanceDAO.DeleteTblAlertInstance(idAlertInstance, conn, tran);
        }

        #endregion

    }
}
