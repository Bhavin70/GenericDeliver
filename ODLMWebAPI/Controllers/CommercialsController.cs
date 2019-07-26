using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.BL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class CommercialsController : Controller
    {
        private readonly IDimGstCodeTypeBL _iDimGstCodeTypeBL;
        private readonly IDimTaxTypeBL _iDimTaxTypeBL;
        private readonly ITblGstCodeDtlsBL _iTblGstCodeDtlsBL;
        private readonly ITblTaxRatesBL _iTblTaxRatesBL;
        private readonly ITblProdGstCodeDtlsBL _iTblProdGstCodeDtlsBL;
        private readonly ITblOtherTaxesBL _iTblOtherTaxesBL;
        private readonly ITblGroupItemBL _iTblGroupItemBL;
        private readonly ICommon _iCommon;
        public CommercialsController(ICommon iCommon, ITblGroupItemBL iTblGroupItemBL, ITblOtherTaxesBL iTblOtherTaxesBL, ITblProdGstCodeDtlsBL iTblProdGstCodeDtlsBL, ITblTaxRatesBL iTblTaxRatesBL, ITblGstCodeDtlsBL iTblGstCodeDtlsBL, IDimGstCodeTypeBL iDimGstCodeTypeBL, IDimTaxTypeBL iDimTaxTypeBL)
        {
            _iDimGstCodeTypeBL = iDimGstCodeTypeBL;
            _iDimTaxTypeBL = iDimTaxTypeBL;
            _iTblGstCodeDtlsBL = iTblGstCodeDtlsBL;
            _iTblTaxRatesBL = iTblTaxRatesBL;
            _iTblProdGstCodeDtlsBL = iTblProdGstCodeDtlsBL;
            _iTblOtherTaxesBL = iTblOtherTaxesBL;
            _iTblGroupItemBL = iTblGroupItemBL;
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

        [Route("GetGSTCodeTypeFromDropDown")]
        [HttpGet]
        public List<DropDownTO> GetGSTCodeTypeFromDropDown()
        {
            List<DimGstCodeTypeTO> CodeTypesList = _iDimGstCodeTypeBL.SelectAllDimGstCodeTypeList();
            List<DropDownTO> list = new List<DropDownTO>();
            if (CodeTypesList != null && CodeTypesList.Count > 0)
            {
                for (int i = 0; i < CodeTypesList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = CodeTypesList[i].CodeDesc;
                    dropDownTO.Value = CodeTypesList[i].IdCodeType;
                    list.Add(dropDownTO);
                }
            }

            return list;
        }

        [Route("GetTaxTypesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetTaxTypesForDropDown()
        {
            List<DimTaxTypeTO> taxTypeTOList = _iDimTaxTypeBL.SelectAllDimTaxTypeList();
            List<DropDownTO> list = new List<DropDownTO>();
            if (taxTypeTOList != null && taxTypeTOList.Count > 0)
            {
                for (int i = 0; i < taxTypeTOList.Count; i++)
                {
                    if (taxTypeTOList[i].IsActive != 1)
                        continue;

                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = taxTypeTOList[i].TaxTypeDesc;
                    dropDownTO.Value = taxTypeTOList[i].IdTaxType;
                    list.Add(dropDownTO);
                }
            }

            return list;
        }

        [Route("GetAllGSTCodeMasterList")]
        [HttpGet]
        public List<TblGstCodeDtlsTO> GetAllGSTCodeMasterList()
        {
            return _iTblGstCodeDtlsBL.SelectAllTblGstCodeDtlsList();
        }

        /// <summary>
        /// Ramdas.W @14092017: Api This Method is Used to GetTax Rate  
        /// </summary>
        /// <param name="gstCodeId"></param>
        /// <returns></returns>
        [Route("GetGSTCodeDetails")]
        [HttpGet]
        public TblGstCodeDtlsTO GetGSTCodeDetails(Int32 idGstCode)
        {
            TblGstCodeDtlsTO gstCodeDtlsTO = _iTblGstCodeDtlsBL.SelectTblGstCodeDtlsTO(idGstCode);
            if (gstCodeDtlsTO != null)
            {
                gstCodeDtlsTO.TaxRatesTOList = _iTblTaxRatesBL.SelectAllTblTaxRatesList(idGstCode);
            }
            return gstCodeDtlsTO;
        }
        
       
        [Route("GetProductAndGSTINCodeLinkageList")]
        [HttpGet]
        public List<TblProdGstCodeDtlsTO> GetProductAndGSTINCodeLinkageList(Int32 gstCodeId=0)
        {
            List<TblProdGstCodeDtlsTO> prodGstCodeDtlsTOList = _iTblProdGstCodeDtlsBL.SelectAllTblProdGstCodeDtlsList(gstCodeId);
            return prodGstCodeDtlsTOList;
        }
        /// <summary>
        /// RW@14092017: Api This Method is Used to GSTCode Id 
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <param name="prodSpecId"></param>
        /// <param name="materialId"></param>
        /// <param name="prodItemId"></param>
        /// <returns></returns>
        [Route("GetProductAndGSTINCodeLinkage")]
        [HttpGet]
        public TblProdGstCodeDtlsTO GetProductAndGSTINCodeLinkage(Int32 prodCatId, Int32 prodSpecId,Int32 materialId, Int32 prodItemId, Int32 prodClassId = 0)
        {
            TblProdGstCodeDtlsTO prodGstCodeDtlsTO = _iTblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTO(prodCatId, prodSpecId, materialId, prodItemId, prodClassId);
            return prodGstCodeDtlsTO;
        }

        [Route("GetProductAndGSTINCodeLinkageById")]
        [HttpGet]
        public TblProdGstCodeDtlsTO GetProductAndGSTINCodeLinkage(int idProdGstCode)
        {
            TblProdGstCodeDtlsTO prodGstCodeDtlsTO = _iTblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTO(idProdGstCode);
            return prodGstCodeDtlsTO;
        }
        
        /// <summary>
        /// GJ@20170919 : Get the Other Tax details list
        /// </summary>
        /// <returns></returns>
        [Route("GetAllOtherTaxesList")]
        [HttpGet]
        public List<TblOtherTaxesTO> GetAllOtherTaxesList()
        {
            return _iTblOtherTaxesBL.SelectAllTblOtherTaxesList();
        }

        /// <summary>
        /// Vijaymala[2018-19-01] :Added to get product item by group 
        /// </summary>
        /// <param name="gstCodeId"></param>
        /// <returns></returns>

        [Route("GetProductAndGroupLinkageList")]
        [HttpGet]
        public List<TblGroupItemTO> GetProductAndGroupLinkageList(Int32 groupId = 0)
        {
            List<TblGroupItemTO> TblGroupItemTOList = _iTblGroupItemBL.SelectAllTblGroupItemDtlsList(groupId);
            return TblGroupItemTOList;
        }

        #endregion

        #region POST

        [Route("PostNewGSTCodeDetails")]
        [HttpPost]
        public ResultMessage PostNewGSTCodeDetails([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                TblGstCodeDtlsTO gstCodeDtlsTO = JsonConvert.DeserializeObject<TblGstCodeDtlsTO>(data["gstCodeDtlsTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (gstCodeDtlsTO == null)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "gstCodeDtlsTO found null";
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

                gstCodeDtlsTO.CreatedBy = Convert.ToInt32(loginUserId);
                gstCodeDtlsTO.CreatedOn = _iCommon.ServerDateTime;
                gstCodeDtlsTO.IsActive = 1;
                return _iTblGstCodeDtlsBL.SaveNewGSTCodeDetails(gstCodeDtlsTO);
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostNewGSTCodeDetails :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }


        [Route("PostUpdateGSTCodeDetails")]
        [HttpPost]
        public ResultMessage PostUpdateGSTCodeDetails([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                TblGstCodeDtlsTO gstCodeDtlsTO = JsonConvert.DeserializeObject<TblGstCodeDtlsTO>(data["gstCodeDtlsTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (gstCodeDtlsTO == null)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "gstCodeDtlsTO found null";
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

                gstCodeDtlsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                gstCodeDtlsTO.UpdatedOn = _iCommon.ServerDateTime;
                return _iTblGstCodeDtlsBL.UpdateNewGSTCodeDetails(gstCodeDtlsTO);
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostNewGSTCodeDetails :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }





        [Route("PostProductAndGSTINCodeLinkage")]
        [HttpPost]
        public ResultMessage PostProductAndGSTINCodeLinkage([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                List<TblProdGstCodeDtlsTO> prodGstCodeDtlsTOList = JsonConvert.DeserializeObject<List<TblProdGstCodeDtlsTO>>(data["prodGstCodeDtlsTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (prodGstCodeDtlsTOList == null)
                {
                    //
                    resMsg.DefaultBehaviour("prodGstCodeDtlsTOList found null.");
                    return resMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resMsg.DefaultBehaviour("loginUserId found null");
                    return resMsg;
                }
               
                return _iTblProdGstCodeDtlsBL.UpdateProductGstCode(prodGstCodeDtlsTOList, Convert.ToInt32(loginUserId));
            }
            catch (Exception ex)
            {
                resMsg.DefaultExceptionBehaviour(ex, "PostProductAndGSTINCodeLinkage");
                return resMsg;
            }
        }

        /// <summary>
        /// Vijaymala [19/01/2018] :Added to link product item to group 
        /// </summary>
        /// <param name="value"></param>
        /// 
        [Route("PostProductItemAndGroupLinkage")]
        [HttpPost]
        public ResultMessage PostProductItemAndGroupLinkage([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                List<TblGroupItemTO> groupItemTOList = JsonConvert.DeserializeObject<List<TblGroupItemTO>>(data["groupItemTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (groupItemTOList == null)
                {
                    //
                    resMsg.DefaultBehaviour("groupItemTOList found null.");
                    return resMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resMsg.DefaultBehaviour("loginUserId found null");
                    return resMsg;
                }

                return _iTblGroupItemBL.UpdateProductGroupITem(groupItemTOList, Convert.ToInt32(loginUserId));
            }
            catch (Exception ex)
            {
                resMsg.DefaultExceptionBehaviour(ex, "PostProductAndGSTINCodeLinkage");
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
