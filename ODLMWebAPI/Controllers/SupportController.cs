using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.BL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.Models;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class SupportController : Controller
    {
        private readonly IDimensionBL _iDimensionBL;
        private readonly ITblSupportDetailsBL _iTblSupportDetailsBL;
        private readonly ITblWeighingBL _iTblWeighingBL;
        private readonly ICommon _iCommon;
        public SupportController(ICommon iCommon, IDimensionBL iDimensionBL, ITblSupportDetailsBL iTblSupportDetailsBL, ITblWeighingBL iTblWeighingBL)
        {
            _iDimensionBL = iDimensionBL;
            _iTblSupportDetailsBL = iTblSupportDetailsBL;
            _iTblWeighingBL = iTblWeighingBL;
            _iCommon = iCommon;
        }
        #region GET
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
        /// <summary>
        /// Get All Invoioce MModes DropDown List.
        /// </summary>
        /// <returns></returns>
        [Route("GetAllInvoiceModes")]
        [HttpGet]
        public List<DropDownTO> GetAllInvoiceModes()
        {
            List<DropDownTO> invoiceModeslist = _iDimensionBL.SelectInvoiceModeForDropDown();
            if (invoiceModeslist != null && invoiceModeslist.Count > 0)
            {
                return invoiceModeslist;
            }
            else
                return null;
        }
        /// <summary>
        /// Get All Invoioce Status List
        /// </summary>
        /// <returns></returns>
        [Route("GetAllInvoiceStatus")]
        [HttpGet]
        public List<DropDownTO> GetAllInvoiceStatus()
        {
            List<DropDownTO> invoiceStatuslist = _iDimensionBL.GetInvoiceStatusDropDown();
            if (invoiceStatuslist != null && invoiceStatuslist.Count > 0)
            {
                return invoiceStatuslist;
            }
            else
                return null;
        }

        #endregion

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        /// <summary>
        /// This Method is for updating Invoice Modes.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateInvoiceForSupport")]
        [HttpPost]
        public ResultMessage PostUpdateInvoiceForSupport([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                Int32 FromUser = 0;
                var loginUserId = data["loginUserId"].ToString();
                var fromUser = data["fromUser"].ToString();
                var Comments = data["comments"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblInvoiceTO.UpdatedOn = serverDate;
                    // Aniket [06-02-2019]
                    tblInvoiceTO.GrossWtTakenDate = _iCommon.ServerDateTime;
                    tblInvoiceTO.PreparationDate = _iCommon.ServerDateTime;
                    FromUser = Convert.ToInt32(fromUser);
                    return _iTblSupportDetailsBL.UpdateInvoiceForSupport(tblInvoiceTO, FromUser, Comments.ToString());
                    //return BL.TblInvoiceBL.UpdateInvoiceConfrimNonConfirmDetails(tblInvoiceTO, tblInvoiceTO.UpdatedBy);
                }
                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoiceForStatusConversion");
                return resultMessage;
            }
        }


        [Route("StopWebApplication")]
        [HttpPost]
        public ResultMessage StopWebApplication( [FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();
            try
            {
                TblUserTO tblUserTO = JsonConvert.DeserializeObject<TblUserTO>(data["userTo"].ToString());
                if (tblUserTO != null)
                {
                    rMessage = _iTblSupportDetailsBL.StopService(tblUserTO);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return rMessage;
        }

        /// <summary>
        /// This Method is for  Delete Weighing Measures.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeleteWeighingMeasureForSupport")]
        [HttpPost]
        public ResultMessage PostDeleteWeighingMeasureForSupport([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblWeighingMeasuresTO weighingMeasuresTO = JsonConvert.DeserializeObject<TblWeighingMeasuresTO>(data["weighingMeasureTo"].ToString());
                Int32 FromUser = 0;
                var loginUserId = data["loginUserId"].ToString();
                var fromUser = data["fromUser"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (weighingMeasuresTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    weighingMeasuresTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    weighingMeasuresTO.UpdatedOn = serverDate;
                    FromUser = Convert.ToInt32(fromUser);
                    return _iTblSupportDetailsBL.PostDeleteWeighingMeasureForSupport(weighingMeasuresTO, FromUser);
                    //return new ResultMessage();
                }
                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoiceForStatusConversion");
                return resultMessage;
            }
        }

        /// <summary>
        /// For posting weight using api call
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostTblWeighingTO")]
        [HttpPost]
        public ResultMessage PostTblWeighingTO(String data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                if (!String.IsNullOrEmpty(data))
                {

                    List<String> strlist = data.Split("*#*#*").ToList();

                    if (strlist != null && strlist.Count == 2)
                    {
                        TblWeighingTO tblWeighingTO = new TblWeighingTO();
                        tblWeighingTO.Measurement = strlist[0];
                        tblWeighingTO.MachineIp = strlist[1];
                        tblWeighingTO.TimeStamp = _iCommon.ServerDateTime;

                        Int32 result = _iTblWeighingBL.DeleteTblWeighingByByMachineIp(tblWeighingTO.MachineIp);


                        result = _iTblWeighingBL.InsertTblWeighing(tblWeighingTO);
                        if (result == 1)
                        {
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.Text = "Success";
                            resultMessage.DisplayMessage = "Success";
                            resultMessage.Result = 1;
                        }
                        else
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error";
                            resultMessage.DisplayMessage = "Error";
                            resultMessage.Result = -1;
                        }

                    }

                }

                //TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                //Int32 FromUser = 0;
                //var loginUserId = data["loginUserId"].ToString();
                //var fromUser = data["fromUser"].ToString();
                //var Comments = data["comments"].ToString();
                //if (Convert.ToInt32(loginUserId) <= 0)
                //{
                //    resultMessage.DefaultBehaviour("loginUserId Not Found");
                //    return resultMessage;
                //}

                //if (tblInvoiceTO != null)
                //{
                //    DateTime serverDate = _iCommon.ServerDateTime;
                //    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //    tblInvoiceTO.UpdatedOn = serverDate;
                //    FromUser = Convert.ToInt32(fromUser);
                //    return _iTblSupportDetailsBL.UpdateInvoiceForSupport(tblInvoiceTO, FromUser, Comments.ToString());
                //    //return BL.TblInvoiceBL.UpdateInvoiceConfrimNonConfirmDetails(tblInvoiceTO, tblInvoiceTO.UpdatedBy);
                //}
                //else
                //{
                //    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                //    return resultMessage;
                //}
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoiceForStatusConversion");
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