using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using System.Text;
using System.Net.Http;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{


    public class VitplNotify : IVitplNotify
    {
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ICommon _iCommon;
        private readonly IConnectionString _iConnectionString;
        private readonly InotificationDAO _inotificationDAO;
        public VitplNotify(ITblConfigParamsDAO iTblConfigParamsDAO, ICommon iCommon, IConnectionString iConnectionString, InotificationDAO inotificationDAO)
        {
            _inotificationDAO = inotificationDAO;
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iCommon = iCommon;
        }
        public string NotifyToRegisteredDevices()
        {
            string applicationID = "";
            string senderId = "";
            try
            {
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_FIRBASE_ANDROID_NOTIFICATION_SETTINGS);
                if (tblConfigParamsTO != null)
                {
                    applicationID = tblConfigParamsTO.ConfigParamVal.Split(",")[0];
                    senderId = tblConfigParamsTO.ConfigParamVal.Split(",")[1];
                }
                //string applicationID = "AIzaSyBY3gLvgh8KrY0wUiUOBAaj-a1U1c8uafM";
                //var senderId = "697536919216";

                string deviceId1 = "dLNdCoSAYio:APA91bHUMngAw9PCRrLTeHApXGAVoG-sPtj12uOq2XKfNDhXpys_M5x8nik9hwOfjRvxgXimJ40lftnUdesS1H7VAEjYw0nieN9C5TEu8zDvenXZn7IcxYcnbn4MADDip_xZN8VrIBCf";

                var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send") as HttpWebRequest;
                tRequest.Method = "post";

                //tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.ContentType = "application/json";

                var data = new
                {
                    to = deviceId1,
                    notification = new
                    {
                        body = "New Order Booked",
                        title = "Notification",
                        sound = "default"
                    }
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                //tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.Headers["Authorization"] = "key=" + applicationID;
                tRequest.Headers["Sender"] = "id=" + senderId;
                tRequest.UseDefaultCredentials = true;
                //tRequest.PreAuthenticate = true;
                tRequest.Credentials = CredentialCache.DefaultCredentials;

                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse =  tRequest.GetResponseAsync().Result)
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                return  sResponseFromServer;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                string str = ex.Message;
                return str;
            }
        }

        public string NotifyToRegisteredDevices(String[] devices, String body, String title, int instanceID)
        {
            string applicationID="";
            string senderId = "";
            try
            {

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_FIRBASE_ANDROID_NOTIFICATION_SETTINGS);
                NotificationTO notification = _inotificationDAO.SelectNotificationTO(instanceID);


                //no android snooze for non transaction notification 
                if (notification == null)
                {
                    notification = new NotificationTO();
                    notification.NavigationUrl = "";
                    notification.AndroidUrl = "";
                    //to insure no snooze for non transactional alerts
                    instanceID = 0;
                }
                if (tblConfigParamsTO != null)
                {
                    applicationID = tblConfigParamsTO.ConfigParamVal.Split(",")[0];
                    senderId = tblConfigParamsTO.ConfigParamVal.Split(",")[1];
                }
                //string deviceId1 = "dLNdCoSAYio:APA91bHUMngAw9PCRrLTeHApXGAVoG-sPtj12uOq2XKfNDhXpys_M5x8nik9hwOfjRvxgXimJ40lftnUdesS1H7VAEjYw0nieN9C5TEu8zDvenXZn7IcxYcnbn4MADDip_xZN8VrIBCf";
                var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send") as HttpWebRequest;
                tRequest.Method = "post";

                //tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.ContentType = "application/json";
                
                var data = new
                {
                    registration_ids = devices,
                    notification = new
                    {
                        body = body,
                        title = title,
                        sound = "default"
                    },
                    data = new
                    {

                        instanceId = instanceID,
                        apiUrl = _iConnectionString.GetConnectionString(Constants.REQUEST_ORIGIN_STRING),
                        commonUrl = Startup.DeliverUrl,
                        navigationUrl = notification.NavigationUrl,
                        androidUrl = notification.AndroidUrl,
                         notificationTitle = title,
                        notificationBody = body


                    }
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                //tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.Headers["Authorization"] = "key=" + applicationID;
                tRequest.Headers["Sender"] = "id=" + senderId;
                tRequest.UseDefaultCredentials = true;
                //tRequest.PreAuthenticate = true;
                tRequest.Credentials = CredentialCache.DefaultCredentials;

                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponseAsync().Result)
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                return sResponseFromServer;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                string str = ex.Message;
                return str;
            }
        }

        public async void NotifyToRegisteredDevicesAsync()
        {
            string applicationID = "";
            string senderId = "";
            try
            {
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_FIRBASE_ANDROID_NOTIFICATION_SETTINGS);
                if (tblConfigParamsTO != null)
                {
                    applicationID = tblConfigParamsTO.ConfigParamVal.Split(",")[0];
                    senderId = tblConfigParamsTO.ConfigParamVal.Split(",")[1];
                }
                // var applicationID = "AIzaSyB7tSzfqkgtoFDR-Pb5Kjo8fxl_uiTDLlw";
                //Kalika firbase details
                //string applicationID = "AIzaSyDuvkMEhiN0vA8mBaLri4lZFXEjA0H2x54";
                //var senderId = "743754001250";
                //  string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";

                // string deviceId1 = "fyrwLbLd8Yc:APA91bHMuZeNVf4hbN2XebAcfaRk0lANvpfVG7s3hq3LU4trka-wI37VQ4qORQpiPuumy4kGMajC8mi80zk2V1YTbwufh5g5CzLmjKKJQfVWTc17VhR4B3Mqc6PL6RZbJHJi9OFdEeSf";
                string deviceId1 = "e-NzSrsJ1m4:APA91bE4k9o44RNpLDPgeNFOwVgp60YC7j4rRvmJAqeVMN5xqKMi3hKrp5Yp2C84fTQ-2dbyx3que9odLaE5-8jKW2t5IdYlKryMfo8lyVYCRd28MIZy_0IyauXhPg7BmyUnVbONy2eN";
                //"dWQbNtmzSqA:APA91bE9NNCRiE-P0T1WdbeoAfzcyTqpYVxsHi9GBOMlLSPNte8GI_wExMup73snOHWEBRDRSxkn6qiNzDmZEP2CDkTC3Ph4UG1rzTk_WdNR6gsFRKODCzxFL3qW4Z8Jezd-UFo4kFjt";

                var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send") as HttpWebRequest;
                // HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";

                //tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.ContentType = "application/json";

                var data = new
                {
                    to = deviceId1,
                    notification = new
                    {
                        body = "New Order Booked",
                        title = "Notification"
                    }
                };

                //var serializer = new JavaScriptSerializer();

                //var json = serializer.Serialize(data);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                //tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                //tRequest.Headers["Authorization: key={0}"] = applicationID;
                //tRequest.Headers[(string.Format("Authorization: key={0}"))]= applicationID;
                //tRequest.Headers["Sender: id"] = senderId.ToString();
                //WebHeaderCollection myWebHeaderCollection = tRequest.Headers;
                tRequest.Headers["Authorization"] = "key=" + applicationID;
                tRequest.Headers["Sender"] = "id=" + senderId;
                //tRequest.ContentLength = byteArray.Length;
                tRequest.UseDefaultCredentials = true;
                //tRequest.PreAuthenticate = true;
                tRequest.Credentials = CredentialCache.DefaultCredentials;

                //Stream dataStream = await tRequest.GetRequestStreamAsync();

                using (Stream dataStream = await tRequest.GetRequestStreamAsync())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = await tRequest.GetResponseAsync())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
        //SystembrodCasting() method for send broadcasting msg to user @kiran 21/11/2018
        public void SystembrodCasting(TblAlertInstanceTO alertInstanceTO)
        {
            //try
            //{
            //    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName("CHAT_APP_CONFIGURATION_SETTINGS");
            //    if (tblConfigParamsTO != null)
            //    {
            //        realTimeDatabaseTO realTimeDatabase = JsonConvert.DeserializeObject<realTimeDatabaseTO>(tblConfigParamsTO.ConfigParamVal.ToString());
            //        if (alertInstanceTO.BroadCastinguserList != null && alertInstanceTO.BroadCastinguserList.Count > 0)
            //        {
            //            foreach (var item in alertInstanceTO.BroadCastinguserList)
            //            {
            //                String stringUrl = realTimeDatabase.DatabaseURL + "/" + item + "/.json";
            //                var tRequest = WebRequest.Create(stringUrl) as HttpWebRequest;
            //                //var tRequest = WebRequest.Create("https://chapapp-3c4db.firebaseio.com/1/.json") as HttpWebRequest;
            //                tRequest.Method = "post";
            //                tRequest.ContentType = "application/json";
            //                DateTime currentDate = StaticStuff._iCommon.ServerDateTime;
            //                long timeStamp = (int)(currentDate - new DateTime(1970, 1, 1)).TotalSeconds;

            //                var data = new
            //                {
            //                    msg = alertInstanceTO.AlertComment,
            //                    currentTimeStamp = currentDate,
            //                    isRead = 1
            //                };

            //                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            //                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            //                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
            //                {
            //                    dataStream.Write(byteArray, 0, byteArray.Length);
            //                }
            //                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;

            //                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //            }
            //        }

            //    }
            //    // return null;
            //}
            //catch (Exception ex)
            //{

            //    string str = ex.Message;
            //    //return str;
            //}
        }
    }
}
