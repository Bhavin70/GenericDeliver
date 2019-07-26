using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using ODLMWebAPI.BL;
using System.Net.Http;
using System.Net;

using System.Threading;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.Controllers
{

    [Route("api/[controller]")]
    public class NotifyController : Controller
    {
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblAlertUsersBL _iTblAlertUsersBL;
        private readonly ITblAlertDefinitionBL _iTblAlertDefinitionBL;
        private readonly ITblAlertSubscribersBL _iTblAlertSubscribersBL;
        private readonly ITblAlertSubscriptSettingsBL _iTblAlertSubscriptSettingsBL;
        private readonly IVitplNotify _iVitplNotify;
        private readonly ITblUserBL _iTblUserBL;
        private readonly ITblAlertActionDtlBL _iTblAlertActionDtlBL;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly ICommon _iCommon;
        public NotifyController(ICommon iCommon, ITblAlertInstanceBL iTblAlertInstanceBL, ITblAlertActionDtlBL iTblAlertActionDtlBL, IVitplNotify iVitplNotify, ITblUserBL iTblUserBL, ITblAlertSubscriptSettingsBL iTblAlertSubscriptSettingsBL, ITblAlertSubscribersBL iTblAlertSubscribersBL, ITblAlertDefinitionBL iTblAlertDefinitionBL, ITblAlertUsersBL iTblAlertUsersBL, ITblUserDAO iTblUserDAO)
        {

            _iTblUserDAO = iTblUserDAO;
            _iTblAlertUsersBL = iTblAlertUsersBL;
            _iTblAlertDefinitionBL = iTblAlertDefinitionBL;
            _iTblAlertSubscribersBL = iTblAlertSubscribersBL;
            _iTblAlertSubscriptSettingsBL = iTblAlertSubscriptSettingsBL;
            _iTblUserBL = iTblUserBL;
            _iVitplNotify = iVitplNotify;
            _iTblAlertActionDtlBL = iTblAlertActionDtlBL;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iCommon = iCommon;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("GetAllActiveAlertList")]
        [HttpGet]
        public List<TblAlertUsersTO> GetAllActiveAlertList(Int32 userId, String userRoleTOList, Int32 loginId = 0, Int32 ModuleId = 0)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return _iTblAlertUsersBL.SelectAllActiveAlertList(userId, tblUserRoleTOList, loginId, ModuleId);
        }


        /// <summary>
        /// Priyanka [20-09-2018] : Added to get the alert defination list
        /// </summary>
        /// <returns></returns>
        [Route("GetAlertDefinationList")]
        [HttpGet]
        public List<TblAlertDefinitionTO> GetAlertDefinationList()
        {
            return _iTblAlertDefinitionBL.SelectAllTblAlertDefinitionList();
        }

        /// <summary>
        /// Priyanka [20-09-2018] : Added to get the alert Subscribers list
        /// </summary>
        /// <returns></returns>
        [Route("GetAlertSubscribersList")]
        [HttpGet]
        public List<TblAlertSubscribersTO> GetAlertSubscribersList(Int32 alertDefId)
        {
            return _iTblAlertSubscribersBL.SelectTblAlertSubscribersByAlertDefId(alertDefId);
        }

        /// <summary>
        /// Priyanka [21-09-2018] : Added to post the new alert Subscription setting
        /// </summary>
        /// <returns></returns>
        [Route("PostAlertSubcriptionSettings")]
        [HttpPost]
        public ResultMessage PostAlertSubcriptionSettings([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO = JsonConvert.DeserializeObject<TblAlertSubscriptSettingsTO>(data["alertSubscriptSettingTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblAlertSubscriptSettingsTO == null)
                {
                    resultMessage.DefaultBehaviour("tblAlertSubscriptSettingsTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                //tblAlertSubscriptSettingsTO.UpdatedOn = _iCommon.ServerDateTime;
                //tblAlertSubscriptSettingsTO.UpdatedBy = Convert.ToInt32(loginUserId);


                TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTONew = _iTblAlertSubscriptSettingsBL.SelectTblAlertSubscriptSettingsFromNotifyId(tblAlertSubscriptSettingsTO.NotificationTypeId, tblAlertSubscriptSettingsTO.SubscriptionId, tblAlertSubscriptSettingsTO.AlertDefId);
                if (tblAlertSubscriptSettingsTONew != null)
                {
                    tblAlertSubscriptSettingsTONew.IsActive = 0;
                    tblAlertSubscriptSettingsTONew.IdSubscriSettings = tblAlertSubscriptSettingsTONew.IdSubscriSettings;
                    tblAlertSubscriptSettingsTONew.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblAlertSubscriptSettingsTONew.UpdatedOn = _iCommon.ServerDateTime;
                    int result1 = _iTblAlertSubscriptSettingsBL.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTONew);
                    if (result1 != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be saved");
                        return resultMessage;
                    }
                }
                tblAlertSubscriptSettingsTO.CreatedOn = _iCommon.ServerDateTime;
                int result = _iTblAlertSubscriptSettingsBL.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEmailConfigurationDetails");
                return resultMessage;
            }
        }


        /// <summary>
        /// Kiran Mehetre [24-10-2018] : Send Notification Perticular UserId
        /// </summary>
        /// <returns></returns>
        [Route("SendNotificationByUserId")]
        [HttpGet]
        public ResultMessage GetAlertDefinationList(Int32 UserId, String Msg)
        {
            TblUserTO userTo = new TblUserTO();
            ResultMessage resultMessage = new ResultMessage();
            userTo = _iTblUserBL.SelectTblUserTO(UserId);
            string[] devices = new string[1];
            if (userTo.RegisteredDeviceId != null)
            {
                devices[0] = userTo.RegisteredDeviceId;
                String notifyBody = userTo.UserDisplayName + Environment.NewLine + Msg;
                String notifyTitle = "SimpliChat";
                _iVitplNotify.NotifyToRegisteredDevices(devices, notifyBody, notifyTitle, 0);
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Acknowledged Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "User Registered Device Id not found";
                resultMessage.Result = -1;
                return resultMessage;
            }
        }


        /// <summary>
        /// Priyanka [24-09-2018] : Added to post new role or user in subscibers list.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewRoleOrUserForSubscribers")]
        [HttpPost]
        public ResultMessage PostNewRoleOrUserForSubscribers([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblAlertSubscribersTO tblAlertSubscribersTO = JsonConvert.DeserializeObject<TblAlertSubscribersTO>(data["alertSubscribersToNew"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblAlertSubscribersTO == null)
                {
                    resultMessage.DefaultBehaviour("tblAlertSubscribersTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                tblAlertSubscribersTO.SubscribedOn = _iCommon.ServerDateTime;

                int result = _iTblAlertSubscribersBL.InsertTblAlertSubscribers(tblAlertSubscribersTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEmailConfigurationDetails");
                return resultMessage;
            }
        }

        /// <summary>
        /// Priyanka [25-09-2018] : Added to update the alert subscribers.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateAlertSubcribers")]
        [HttpPost]
        public ResultMessage PostUpdateAlertSubcribers([FromBody] JObject data)
        {

            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblAlertSubscribersTO tblAlertSubscribersTO = JsonConvert.DeserializeObject<TblAlertSubscribersTO>(data["alertSubscribersTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblAlertSubscribersTO == null)
                {
                    resultMessage.DefaultBehaviour("tblAlertSubscribersTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }


                tblAlertSubscribersTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //tblAlertSubscribersTO.SubscribedOn = _iCommon.ServerDateTime;
                tblAlertSubscribersTO.UpdatedOn = _iCommon.ServerDateTime;

                return _iTblAlertSubscribersBL.UpdateAlertSubscribers(tblAlertSubscribersTO);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEmailConfigurationDetails");
                return resultMessage;
            }
        }

        [Route("PostAlertAcknowledgement")]
        [HttpPost]
        public ResultMessage PostAlertAcknowledgement([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblAlertUsersTO alertUsersTO = JsonConvert.DeserializeObject<TblAlertUsersTO>(data["alertUsersTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (alertUsersTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                int result = 0;
                TblAlertActionDtlTO tblAlertActionDtlTO = new TblAlertActionDtlTO();

                if (alertUsersTO.IsReseted == 1)
                {
                    //Check For Existence
                    TblAlertActionDtlTO existingAlertActionDtlTO = _iTblAlertActionDtlBL.SelectTblAlertActionDtlTO(alertUsersTO.AlertInstanceId, Convert.ToInt32(loginUserId));
                    if (existingAlertActionDtlTO != null)
                    {
                        existingAlertActionDtlTO.ResetDate = _iCommon.ServerDateTime;
                        result = _iTblAlertActionDtlBL.UpdateTblAlertActionDtl(existingAlertActionDtlTO);
                        if (result == 1)
                        {
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.Text = "Alert Resetted Sucessfully";
                            resultMessage.Result = 1;
                            return resultMessage;
                        }
                        else
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Alert Acknowledgement/Reset";
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
                tblAlertActionDtlTO.UserId = Convert.ToInt32(loginUserId);
                tblAlertActionDtlTO.AcknowledgedOn = _iCommon.ServerDateTime;
                tblAlertActionDtlTO.AlertInstanceId = alertUsersTO.AlertInstanceId;
                result = _iTblAlertActionDtlBL.InsertTblAlertActionDtl(tblAlertActionDtlTO);
                if (result == 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Alert Acknowledged Sucessfully";
                    resultMessage.Result = 1;
                    return resultMessage;
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While Alert Acknowledgement";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostAlertAcknowledgement";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        [Route("PostResetAllAlerts")]
        [HttpPost]
        public ResultMessage PostResetAllAlerts([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                //Int32 roleId = Convert.ToInt32(data["roleId"].ToString());
                List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(data["userRoleList"].ToString());

                List<TblAlertUsersTO> list = _iTblAlertUsersBL.SelectAllActiveAlertList(loginUserId, tblUserRoleTOList, 0, 0);

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                if (list == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "list Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                int result = 0;
                TblAlertActionDtlTO tblAlertActionDtlTO = new TblAlertActionDtlTO();
                return _iTblAlertActionDtlBL.ResetAllAlerts(loginUserId, list, result);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostResetAllAlerts";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        [Route("PostAutoResetAndDeleteAlerts")]
        [HttpGet]
        public ResultMessage PostAutoResetAndDeleteAlerts()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                return _iTblAlertInstanceBL.AutoResetAndDeleteAlerts();

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostAutoResetAndDeleteAlerts";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        /// <summary>
        /// Sudhir --Added for Save New Alert.
        /// </summary>
        /// <param name="tblAlertInstanceTO"></param>
        /// <returns></returns>
        [Route("PostNewAlert")]
        [HttpPost]
        public ResultMessage PostNewAlert([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblAlertInstanceTO tblAlertInstanceTO = JsonConvert.DeserializeObject<TblAlertInstanceTO>(data["tblAlertInstanceTO"].ToString());

                if (tblAlertInstanceTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblAlertInstanceTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                tblAlertInstanceTO.RaisedOn = _iCommon.ServerDateTime;
                //tblAlertInstanceTO.RaisedBy= Convert.ToInt32(loginUserId);

                return _iTblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostResetAllAlerts";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }



        /// <summary>
        /// Deepali --Added for Reset Old Alerts.
        /// </summary>
        /// <param name="tblAlertInstanceTO"></param>
        /// <returns></returns>
        [Route("PostResetOldAlerts")]
        [HttpPost]
        public ResultMessage PostResetOldAlerts([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblAlertInstanceTO tblAlertInstanceTO = JsonConvert.DeserializeObject<TblAlertInstanceTO>(data["tblAlertInstanceTO"].ToString());

                if (tblAlertInstanceTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblAlertInstanceTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                tblAlertInstanceTO.RaisedOn = _iCommon.ServerDateTime;
                _iTblAlertInstanceBL.ResetOldAlerts(tblAlertInstanceTO);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostResetAllAlerts";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        //TO send android Snooze notifications
        [Route("postSnoozeAndroidNotification")]
        [HttpGet]
        public void PostSnoozeAndroidNotification()
        {
            _iTblAlertInstanceBL.postSnoozeForAndroid();
        }


        [Route("snoozeResetNotification")]
        [HttpGet]
        public ResultMessage PostSnooze(int alertInstanceId, int snoozeTime, int userId = 0, string deviceId = null)
        {


            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                if (userId == 0 && deviceId == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "specify deviceId or userId";
                    resultMessage.Result = -1;
                    return resultMessage;
                }
                int result = 0;
                List<TblAlertActionDtlTO> list = _iTblAlertActionDtlBL.SelectAllTblAlertActionDtlList();
                if (deviceId != null)
                {
                    DropDownTO userTO = _iTblUserDAO.SelectTblUserOnDeviceId(deviceId);
                    if (userTO == null)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "couldnt find user for DeviceId";
                        resultMessage.Result = -1;
                        return resultMessage;
                    }

                    userId = userTO.Value;
                }


                var chkAlert = list.Where(e => e.UserId == userId && e.AlertInstanceId == alertInstanceId).LastOrDefault();
                if (chkAlert == null)
                {
                    TblAlertActionDtlTO tblAlertActionDtlTO = new TblAlertActionDtlTO();
                    tblAlertActionDtlTO.AcknowledgedOn = _iCommon.ServerDateTime;
                    tblAlertActionDtlTO.AlertInstanceId = alertInstanceId;
                    tblAlertActionDtlTO.UserId = userId;
                    tblAlertActionDtlTO.SnoozeOn = _iCommon.ServerDateTime.AddMinutes(snoozeTime);
                    tblAlertActionDtlTO.SnoozeCount++;
                    result = _iTblAlertActionDtlBL.InsertTblAlertActionDtl(tblAlertActionDtlTO);
                }
                else
                {
                    chkAlert.SnoozeCount++;
                    chkAlert.SnoozeOn = _iCommon.ServerDateTime.AddMinutes(snoozeTime);
                    result = _iTblAlertActionDtlBL.UpdateTblAlertActionDtl(chkAlert);
                }
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;

                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostResetAllAlerts";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }



        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
