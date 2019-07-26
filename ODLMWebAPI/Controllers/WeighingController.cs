using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class WeighingController : Controller
    {
        private readonly ITblWeighingMachineBL _iTblWeighingMachineBL;
        private readonly ITblWeighingMeasuresBL _iTblWeighingMeasuresBL;
        private readonly ITblWeighingBL _iTblWeighingBL;
        private readonly ITblMachineCalibrationBL _iTblMachineCalibrationBL;
        private readonly ICommon _iCommon;
        private readonly ICircularDependencyBL _iCircularDependencyBL;
        public WeighingController(ITblWeighingMeasuresBL iTblWeighingMeasuresBL, ICircularDependencyBL iCircularDependencyBL, ICommon iCommon, ITblMachineCalibrationBL iTblMachineCalibrationBL,ITblWeighingBL iTblWeighingBL,ITblWeighingMachineBL iTblWeighingMachineBL)
        {
            _iTblWeighingMachineBL = iTblWeighingMachineBL;
            _iTblWeighingMeasuresBL = iTblWeighingMeasuresBL;
            _iTblWeighingBL = iTblWeighingBL;
            _iTblMachineCalibrationBL = iTblMachineCalibrationBL;
            _iCommon = iCommon;
            _iCircularDependencyBL = iCircularDependencyBL;
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

        [Route("GetAllWeighingMachines")]
        [HttpGet]
        public List<TblWeighingMachineTO> GetAllWeighingMachines()
        {
            List<TblWeighingMachineTO> list = _iTblWeighingMachineBL.SelectAllTblWeighingMachineList();
            return list;
        }

        [Route("GetWeighingMachinesDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetWeighingMachinesDropDownList()
        {
            List<DropDownTO> list = _iTblWeighingMachineBL.SelectTblWeighingMachineDropDownList();
            return list;
        }

        [Route("GetAllWeighingMeasuresByLoadingId")]
        [HttpGet]
        public List<TblWeighingMeasuresTO> GetAllWeighingMeasuresByLoadingId(int loadingId)
        {
            List<TblWeighingMeasuresTO> list = _iCircularDependencyBL.SelectAllTblWeighingMeasuresListByLoadingId(loadingId);
            return list;
        }

        [Route("GetAllWeighingMeasuresByLoadingIds")]
        [HttpGet]
        public List<TblWeighingMeasuresTO> GetAllWeighingMeasuresByLoadingId(string loadingIds, Boolean isUnloading )
        {
            List<TblWeighingMeasuresTO> list = _iTblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(loadingIds, isUnloading);
            return list;
        }

        [Route("GetAllWeighingMeasuresByUnLoadingId")]
        [HttpGet]
        public List<TblWeighingMeasuresTO> GetAllWeighingMeasuresByUnLoadingId(string unLoadingId)
        {
            List<TblWeighingMeasuresTO> list = _iTblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(unLoadingId, true);
            return list;
        }

        [Route("GetAllWeighingMeasuresByVehicleNo")]
        [HttpGet]
        public List<TblWeighingMeasuresTO> GetAllWeighingMeasuresByVehicleNo(string vehicleNo)
        {
            List<TblWeighingMeasuresTO> list = _iTblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByVehicleNo(vehicleNo);
            return list;
        }

        [Route("GetLatestWeightByMachineIp")]
        [HttpGet]
        public TblWeighingTO GetLatestWeightByMachineIp(string ipAddr)
        {
            return _iTblWeighingBL.SelectTblWeighingByMachineIp(ipAddr);
        }

        
        [Route("GetLatestCalibrationValByMachineId")]
        [HttpGet]
        public TblMachineCalibrationTO GetLatestCalibrationValByMachineId(int weighingMachineId)
        {
            TblMachineCalibrationTO tblMachineCalibrationTO = _iTblMachineCalibrationBL.SelectTblMachineCalibrationTOByWeighingMachineId(weighingMachineId);
            return tblMachineCalibrationTO;
        }

        #endregion

        #region POST


        [Route("PostNewWeighingMachine")]
        [HttpPost]
        public ResultMessage PostNewWeighingMachine([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                TblWeighingMachineTO weighingMachineTO = JsonConvert.DeserializeObject<TblWeighingMachineTO>(data["weighingMachineTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (weighingMachineTO == null)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "weighingMachineTO found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "loginUserId found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }

                DateTime serverDate = _iCommon.ServerDateTime;
                Int32 userId = Convert.ToInt32(loginUserId);
                weighingMachineTO.CreatedBy = userId;
                weighingMachineTO.CreatedOn = serverDate;
                int result = _iTblWeighingMachineBL.InsertTblWeighingMachine(weighingMachineTO);
                if(result==1)
                {
                    resMsg.DefaultSuccessBehaviour();
                    //resMsg.MessageType = ResultMessageE.Information;
                    //resMsg.DisplayMessage = Constants.DefaultSuccessMsg;
                    //resMsg.Text = Constants.DefaultSuccessMsg;
                    //resMsg.Result = 1;
                }
                else
                {
                    resMsg.DefaultBehaviour();
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    resMsg.Text = "Error While InsertTblWeighingMachine";
                    resMsg.Result = 0;
                }
                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostNewWeighingMachine :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [Route("PostUpdateWeighingMachine")]
        [HttpPost]
        public ResultMessage PostUpdateWeighingMachine([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                TblWeighingMachineTO weighingMachineTO = JsonConvert.DeserializeObject<TblWeighingMachineTO>(data["weighingMachineTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (weighingMachineTO == null)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "weighingMachineTO found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "loginUserId found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }

                DateTime serverDate = _iCommon.ServerDateTime;
                Int32 userId = Convert.ToInt32(loginUserId);
                weighingMachineTO.UpdatedBy = userId;
                weighingMachineTO.UpdatedOn = serverDate;
                int result = _iTblWeighingMachineBL.UpdateTblWeighingMachine(weighingMachineTO);
                if (result == 1)
                {
                    resMsg.MessageType = ResultMessageE.Information;
                    resMsg.DisplayMessage = Constants.DefaultSuccessMsg;
                    resMsg.Text = Constants.DefaultSuccessMsg;
                    resMsg.Result = 1;
                }
                else
                {
                    resMsg.DefaultBehaviour();
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    resMsg.Text = "Error While UpdateTblWeighingMachine";
                    resMsg.Result = 0;
                }
                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostUpdateWeighingMachine :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [Route("PostNewWeighingMeasurement")]
        [HttpPost]
        public ResultMessage PostNewWeighingMeasurement([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                TblWeighingMeasuresTO tblWeighingMeasuresTO = new TblWeighingMeasuresTO();
                tblLoadingSlipExtTOList = JsonConvert.DeserializeObject<List<TblLoadingSlipExtTO>>(data["tblLoadingSlipExtTOList"].ToString());
                tblWeighingMeasuresTO = JsonConvert.DeserializeObject<TblWeighingMeasuresTO>(data["tblWeighingMeasuresTO"].ToString());

                // Vaibhav [30-Mar-2018] Added to unloading weighing measure.
                List<TblUnLoadingItemDetTO> tblUnLoadingItemDetTOList = data["tblUnLoadingItemDetTOList"]==null? null:JsonConvert.DeserializeObject<List<TblUnLoadingItemDetTO>>(data["tblUnLoadingItemDetTOList"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                tblWeighingMeasuresTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblWeighingMeasuresTO.CreatedOn = _iCommon.ServerDateTime;
               
                resMsg = _iTblWeighingMeasuresBL.SaveNewWeighinMachineMeasurement(tblWeighingMeasuresTO, tblLoadingSlipExtTOList, tblUnLoadingItemDetTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.DefaultExceptionBehaviour(ex, "Exception Error IN API Call PostNewWeighingMeasurement :");
                return resMsg;
            }

        }

        /// <summary>
        /// Saket [2018-02-16] Added To Create intermediate invoice against loadigslip.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("CreateInvoiceAgainstLoadingSlip")]
        [HttpPost]
        public ResultMessage CreateInvoiceAgainstLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                TblWeighingMeasuresTO tblWeighingMeasuresTO = new TblWeighingMeasuresTO();
                tblLoadingSlipExtTOList = JsonConvert.DeserializeObject<List<TblLoadingSlipExtTO>>(data["tblLoadingSlipExtTOList"].ToString());
                tblWeighingMeasuresTO = JsonConvert.DeserializeObject<TblWeighingMeasuresTO>(data["tblWeighingMeasuresTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                tblWeighingMeasuresTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblWeighingMeasuresTO.CreatedOn = _iCommon.ServerDateTime;

                resMsg = _iTblWeighingMeasuresBL.SaveNewWeighinMachineMeasurement(tblWeighingMeasuresTO, tblLoadingSlipExtTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.DefaultExceptionBehaviour(ex, "Exception Error IN API Call PostNewWeighingMeasurement :");
                return resMsg;
            }

        }


        [Route("PostIsAllowWeighingMachineToWt")]
        [HttpPost]
        public ResultMessage PostIsAllowWeighingMachineToWt([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                tblLoadingSlipExtTO = JsonConvert.DeserializeObject<TblLoadingSlipExtTO >(data["tblLoadingSlipExtTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                tblLoadingSlipExtTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingSlipExtTO.UpdatedOn = _iCommon.ServerDateTime;

                resMsg = _iTblWeighingMeasuresBL.UpdateLoadingSlipExtTo(tblLoadingSlipExtTO);

                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostNewWeighingMeasurement :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [Route("PostMachineCalibration")]
        [HttpPost]
        public ResultMessage PostMachineCalibration([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            int result = 0;
            try
            {
                List<TblMachineCalibrationTO> tblLoadingSlipExtTOList = new List<TblMachineCalibrationTO>();
                TblMachineCalibrationTO tblMachineCalibrationTO = new TblMachineCalibrationTO();
                tblMachineCalibrationTO = JsonConvert.DeserializeObject<TblMachineCalibrationTO>(data["tblMachineCalibrationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                //tblLoadingSlipExtTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //tblLoadingSlipExtTO.UpdatedOn = _iCommon.ServerDateTime;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resMsg.DefaultBehaviour("loginUserId found null");
                    return resMsg;
                }
                if(tblMachineCalibrationTO == null)
                {
                    resMsg.DefaultBehaviour("tblMachineCalibrationTO found null");
                    return resMsg;
                }

                tblMachineCalibrationTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblMachineCalibrationTO.CreatedOn = _iCommon.ServerDateTime;
                return _iTblMachineCalibrationBL.InsertTblMachineCalibration(tblMachineCalibrationTO);     
            }
            catch (Exception ex)
            {
                resMsg.DefaultExceptionBehaviour(ex, "PostMachineCalibration");
                return resMsg;
            }
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        #endregion

        #region PUT

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion

        #region DELETE

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion

    }

}
