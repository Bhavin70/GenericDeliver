using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace ODLMWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/DeliverMiddleware")]
    public class DeliverMiddlewareController : Controller
    {
        private readonly ICommon _iCommon;
        public DeliverMiddlewareController(ICommon iCommon)
        {
            _iCommon = iCommon;
        }

        [Route("CalculateBookingsOpeningBalance")]
        [HttpGet]
        public ResultMessage CalculateBookingsOpeningBalance()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@".\connection.json"));
                Thread thread = new Thread(delegate ()
                {
                    foreach (var property in o1)
                    {
                        string key = property.Key;
                        _iCommon.CalculateBookingsOpeningBalance((string)o1[key][Constants.REQUEST_ORIGIN_STRING]);
                    }
                });
                thread.Start();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Calculated Bookings Opening Balance Successfully";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method CalculateBookingsOpeningBalance";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        [Route("serialPortListner")]
        [HttpGet]
        public ResultMessage portListner(String clientName)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@".\connection.json"));
                Thread thread = new Thread(delegate ()
                {
                    foreach (var property in o1)
                    {
                        string key = property.Key;
                        if(clientName.Equals(key))
                        _iCommon.serialPortListner((string)o1[key][Constants.REQUEST_ORIGIN_STRING]);
                    }
                });
                thread.Start();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Calculated Bookings Opening Balance Successfully";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method CalculateBookingsOpeningBalance";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        [Route("PostCancelNotConfirmLoadings")]
        [HttpGet]
        public ResultMessage PostCancelNotConfirmLoadings()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@".\connection.json"));
                Thread thread = new Thread(delegate ()
                {
                    foreach (var property in o1)
                    {
                        string key = property.Key;
                        _iCommon.PostCancelNotConfirmLoadings((string)o1[key][Constants.REQUEST_ORIGIN_STRING]);
                    }
                });
                thread.Start();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Post Cancel Not Confirm Loadings Successfully";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        [Route("PostSnoozeNotificationsToAndroid")]
        [HttpGet]
        public ResultMessage PostSnoozeForAndroid()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@".\connection.json"));
                Thread thread = new Thread(delegate ()
                {
                    foreach (var property in o1)
                    {
                        string key = property.Key;
                        _iCommon.PostSnoozeAndroid((string)o1[key][Constants.REQUEST_ORIGIN_STRING]);
                    }
                });
                thread.Start();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Post Cancel Not Confirm Loadings Successfully";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
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
                JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@".\connection.json"));
                Thread thread = new Thread(delegate ()
                {
                    foreach (var property in o1)
                    {
                        string key = property.Key;
                        _iCommon.PostAutoResetAndDeleteAlerts((string)o1[key][Constants.REQUEST_ORIGIN_STRING]);
                    }
                });
                thread.Start();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Post Auto Reset And Delete Alerts Successfully";
                return resultMessage;
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
    }
}