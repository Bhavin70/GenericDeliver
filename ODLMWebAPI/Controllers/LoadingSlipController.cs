using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class LoadingSlipController : Controller
    {
        private readonly ITblLoadingBL _iTblLoadingBL;
      private readonly ITblLoadingDAO _iTblLoadingDAO;
        private readonly ICommon _iCommon;
        public LoadingSlipController(ICommon iCommon, ITblLoadingBL iTblLoadingBL,ITblLoadingDAO iTblLoadingDAO)
        {
            _iTblLoadingDAO =iTblLoadingDAO;
            _iTblLoadingBL = iTblLoadingBL;
            _iCommon = iCommon;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        [Route("PostNewLoadingSlip")]
        [HttpPost]
        public int PostNewLoadingSlip([FromBody] JObject data)
        {
            try
            {

                TblLoadingTO tblLoadingSlipTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingSlipTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingSlipTO == null)
                {
                    return 0;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    return 0;
                }

                if (tblLoadingSlipTO.LoadingSlipList == null || tblLoadingSlipTO.LoadingSlipList.Count == 0)
                {
                    return 0;
                }              

                tblLoadingSlipTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblLoadingSlipTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;
                tblLoadingSlipTO.StatusDate = _iCommon.ServerDateTime;
                tblLoadingSlipTO.CreatedOn = _iCommon.ServerDateTime;
                tblLoadingSlipTO.StatusReason = "New - Considered For Loading";

                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblLoadingBL.SaveNewLoadingSlip(tblLoadingSlipTO);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    return 0;
                }
                else
                {
                    // loggerObj.LogInformation("Sucess");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                //loggerObj.LogError(1, ex, "Exception Error in POstNewBooking", data);
                return -1;
            }
        }





        //IOT
        //hrushikesh added to read modbusList         
        [Route("getModbusRefList")]
        [HttpGet]
        public List<int> getModbusRefList()
        {
            try
            {
                return _iTblLoadingDAO.GeModRefMaxData();
             
            }
            catch (Exception ex)
            {
                //loggerObj.LogError(1, ex, "Exception Error in POstNewBooking", data);
                return null;
            }
        }
        
        [Route("CheckIotConnectivity")]
        [HttpGet]
        public ResultMessage CheckIotConnectivity()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return _iTblLoadingBL.CheckIotConnectivity();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                return resultMessage;
            }
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
