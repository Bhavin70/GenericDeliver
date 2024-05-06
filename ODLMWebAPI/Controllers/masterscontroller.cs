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
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class MastersController : Controller
    {
        private readonly ITblCRMLabelBL _iTblCRMLabelBL;
        private readonly ITblItemBroadCategoriesBL _iTblItemBroadCategoriesBL;
        private readonly IDimensionBL _iDimensionBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly IDimStatusBL _iDimStatusBL;
        private readonly ITblModuleCommunicationBL _iTblModuleCommunicationBL;
        private readonly ITblItemTallyRefDtlsBL _iTblItemTallyRefDtlsBL;
        private readonly ITblSessionBL _iTblSessionBL;
        private readonly ITblPagesBL _iTblPagesBL;
        private readonly ITblPageElementsBL _iTblPageElementsBL;
        private readonly IDimVehicleTypeBL _iDimVehicleTypeBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblStockYardBL _iTblStockYardBL;
        private readonly ITblSupervisorBL _iTblSupervisorBL;
        private readonly IDimMstDesignationBL _iDimMstDesignationBL;
        private readonly ITblEmailConfigrationBL _iTblEmailConfigrationBL;
        private readonly IDimUnitMeasuresBL _iDimUnitMeasuresBL;
        private readonly ITblUnloadingStandDescBL _iTblUnloadingStandDescBL;
        private readonly IDimMstDeptBL _iDimMstDeptBL;
        private readonly ITblStockConfigBL _iTblStockConfigBL;
        private readonly IDimBrandBL _iDimBrandBL;
        private readonly IDimStateBL _iDimStateBL;
        private readonly ITblGroupBL _iTblGroupBL;
        private readonly ITblOtherDesignationsBL _iTblOtherDesignationsBL;
        private readonly ITblDocumentDetailsBL _iTblDocumentDetailsBL;
        private readonly IDimProdSpecDescBL _iDimProdSpecDescBL;
        private readonly ITblRoleOrgSettingBL _iTblRoleOrgSettingBL;
        private readonly ITblTaskModuleExtBL _iTblTaskModuleExtBL;
        private readonly ITbltaskWithoutSubscBL _iTbltaskWithoutSubscBL;
        private readonly ITblRoleBL _iTblRoleBL;
        private readonly ITblCRMShareDocsDetailsBL _iTblCRMShareDocsDetailsBL;
        private readonly ITblSessionHistoryBL _iTblSessionHistoryBL;
        private readonly ITblUserBL _iTblUserBL;
        private readonly IVitplNotify _iVitplNotify;
        private readonly IDimDistrictBL _iDimDistrictBL;
        private readonly IDimTalukaBL _iDimTalukaBL;
        private readonly ITblTranActionsBL _iTblTranActionsBL;
        private readonly ITblAddonsFunDtlsBL _iTblAddonsFunDtlsBL;
        private readonly ICommon _iCommon;
        private readonly IDimConfigurePageBL _iDimConfigurePageBL;
        private readonly IDimOrgTypeBL _iDimOrgTypeBL;
        public MastersController(IDimConfigurePageBL iDimConfigurePageBL,IDimOrgTypeBL iDimOrgTypeBL,ITblAddonsFunDtlsBL iTblAddonsFunDtlsBL, ITblTranActionsBL iTblTranActionsBL, IDimTalukaBL iDimTalukaBL, IDimDistrictBL iDimDistrictBL, IVitplNotify iVitplNotify, ITblUserBL iTblUserBL, ITblSessionHistoryBL iTblSessionHistoryBL, ITblCRMShareDocsDetailsBL iTblCRMShareDocsDetailsBL, ITblRoleBL iTblRoleBL, ITbltaskWithoutSubscBL iTbltaskWithoutSubscBL, ITblTaskModuleExtBL iTblTaskModuleExtBL, ITblRoleOrgSettingBL iTblRoleOrgSettingBL, IDimProdSpecDescBL iDimProdSpecDescBL, ITblDocumentDetailsBL iTblDocumentDetailsBL, ITblOtherDesignationsBL iTblOtherDesignationsBL, ITblGroupBL iTblGroupBL, IDimStateBL iDimStateBL, IDimBrandBL iDimBrandBL, ITblStockConfigBL iTblStockConfigBL, IDimMstDeptBL iDimMstDeptBL, ITblUnloadingStandDescBL iTblUnloadingStandDescBL, IDimUnitMeasuresBL iDimUnitMeasuresBL, ITblEmailConfigrationBL iTblEmailConfigrationBL, IDimMstDesignationBL iDimMstDesignationBL, ITblSupervisorBL iTblSupervisorBL, ITblStockYardBL iTblStockYardBL, ITblConfigParamsBL iTblConfigParamsBL, IDimVehicleTypeBL iDimVehicleTypeBL, ITblPageElementsBL iTblPageElementsBL, ITblPagesBL iTblPagesBL, ITblSessionBL iTblSessionBL, ITblItemTallyRefDtlsBL iTblItemTallyRefDtlsBL, ITblModuleCommunicationBL iTblModuleCommunicationBL, IDimStatusBL iDimStatusBL, ITblPersonBL iTblPersonBL, IDimensionBL iDimensionBL, ICommon iCommon, ITblItemBroadCategoriesBL iTblItemBroadCategoriesBL, ITblCRMLabelBL iTblCRMLabelBL)
        {
            _iDimOrgTypeBL =iDimOrgTypeBL;
            _iTblCRMLabelBL = iTblCRMLabelBL;
            _iTblItemBroadCategoriesBL = iTblItemBroadCategoriesBL;
            _iDimensionBL = iDimensionBL;
            _iTblPersonBL = iTblPersonBL;
            _iDimStatusBL = iDimStatusBL;
            _iTblModuleCommunicationBL = iTblModuleCommunicationBL;
            _iTblItemTallyRefDtlsBL = iTblItemTallyRefDtlsBL;
            _iTblSessionBL = iTblSessionBL;
            _iTblPagesBL = iTblPagesBL;
            _iTblPageElementsBL = iTblPageElementsBL;
            _iDimVehicleTypeBL = iDimVehicleTypeBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblStockYardBL = iTblStockYardBL;
            _iTblSupervisorBL = iTblSupervisorBL;
            _iDimMstDesignationBL = iDimMstDesignationBL;
            _iTblEmailConfigrationBL = iTblEmailConfigrationBL;
            _iDimUnitMeasuresBL = iDimUnitMeasuresBL;
            _iTblUnloadingStandDescBL = iTblUnloadingStandDescBL;
            _iDimMstDeptBL = iDimMstDeptBL;
            _iTblStockConfigBL = iTblStockConfigBL;
            _iDimBrandBL = iDimBrandBL;
            _iDimStateBL = iDimStateBL;
            _iTblGroupBL = iTblGroupBL;
            _iTblOtherDesignationsBL = iTblOtherDesignationsBL;
            _iTblDocumentDetailsBL = iTblDocumentDetailsBL;
            _iDimProdSpecDescBL = iDimProdSpecDescBL;
            _iTblRoleOrgSettingBL = iTblRoleOrgSettingBL;
            _iTblTaskModuleExtBL = iTblTaskModuleExtBL;
            _iTbltaskWithoutSubscBL = iTbltaskWithoutSubscBL;
            _iTblRoleBL = iTblRoleBL;
            _iTblCRMShareDocsDetailsBL = iTblCRMShareDocsDetailsBL;
            _iTblSessionHistoryBL = iTblSessionHistoryBL;
            _iTblUserBL = iTblUserBL;
            _iVitplNotify = iVitplNotify;
            _iDimDistrictBL = iDimDistrictBL;
            _iDimTalukaBL = iDimTalukaBL;
            _iTblTranActionsBL = iTblTranActionsBL;
            _iCommon = iCommon;
            _iTblAddonsFunDtlsBL = iTblAddonsFunDtlsBL;
            _iDimConfigurePageBL = iDimConfigurePageBL;
        }
        #region GET

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        /// <summary>
        /// Sudhir[23-APR-2018] Added for Select All Products .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllItemBroadCategories")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblItemBroadCategoriesTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllItemBroadCategories()
        {
            try
            {
                List<TblItemBroadCategoriesTO> list = _iTblItemBroadCategoriesBL.SelectAllTblItemBroadCategories();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        //hrushikesh added for getting addon Details

        [Route("GetAddOnsDetails")]
        [HttpGet]
        public List<TblAddonsFunDtlsTO> GetAddOnsDetails(int transId, int ModuleId, String TransactionType, String PageElementId = null, String transIds= null)
        {
            if (transId > 0)
            {
                return _iTblAddonsFunDtlsBL.SelectAddonDetails(transId, ModuleId, TransactionType, PageElementId, transIds);
            }
            return null;
        }

        //hrushikesh added for getting DynamicLabels
        [Route("GetLabelsOnPageId")]
        [HttpGet]
        public List<TblCRMLabelTO> GetLabelsOnPageId(int pageId=0,int langId=0)
        {
                return _iTblCRMLabelBL.SelectAllTblCRMLabelList(pageId,langId);
        }


        //Kiran[15-Mar-2018] Added for Get Coloumn Names of Selected Table
        [Route("GetColumnName")]
        [HttpGet]
        public List<Dictionary<string, string>> GetColumnName(string tableName, Int32 tableValue)
        {

            if (tableName != null)
            {
                return _iDimensionBL.GetColumnName(tableName, tableValue);
            }
            return null;
        }

        /// <summary>
        /// Saket [2018-10-30] Added as these call was in ODLM code.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [Route("GetPersonOnPersonId")]
        [HttpGet]
        public TblPersonTO GetPersonOnPersonId(Int32 personId)
        {
            return _iTblPersonBL.SelectTblPersonTO(personId);
        }

        /// <summary>
        /// Sudhir[24-APR-2018] Added for Get All Master Org Types.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// 
        [Route("GetAllOrganizationTypes")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllOrganizationTypes()
        {
            try
            {
                List<DropDownTO> list = _iDimensionBL.GetAllOrganizationType();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("GetStatusListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetStatusListForDropDown(Int32 txnTypeId)
        {


            List<DimStatusTO> statusList = _iDimStatusBL.SelectAllDimStatusList(txnTypeId);
            List<DropDownTO> list = new List<DropDownTO>();
            if (statusList != null && statusList.Count > 0)
            {
                for (int i = 0; i < statusList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = statusList[i].StatusName;
                    dropDownTO.Value = statusList[i].IdStatus;
                    list.Add(dropDownTO);
                }
            }

            return list;
        }

        [Route("GetStatusListByTxnTypeId")]
        [HttpGet]
        public List<DimStatusTO> GetStatusListByTxnTypeId(Int32 txnTypeId)
        {
            List<DimStatusTO> statusList = _iDimStatusBL.SelectAllDimStatusList(txnTypeId);
            return statusList;
        }

        [Route("GetExportTypeList")]
        [HttpGet]
        public List<DimExportTypeTO> GetExportTypeList()
        {
            return _iDimensionBL.GetExportTypeList();
        }

        [Route("GetIndustrySegmentList")]
        [HttpGet]
        public List<DimIndustrySegmentTO> GetIndustrySegmentList()
        {
            return _iDimensionBL.GetIndustryTypeList();
        }
        //for industry segment type list
        [Route("GetIndustrySegmentTypeList")]
        [HttpGet]
        public List<DimIndustrySegmentTypeTO> GetIndustrySegmentTypeList(Int32 industrySegmentId)
        {
            return _iDimensionBL.GetIndustrySegmentTypeList(industrySegmentId);
        }

        [Route("GetStatusByOrgId")]
        [HttpGet]
        public DimStatusTO GetStatusOnOrgId(Int32 orgId)
        {
            return _iDimStatusBL.SelectDimStatusOnOrgId(orgId);

        }

        /// <summary>
        /// Kiran[16-08-2018] To get module Communication List using moduleId and entityId
        /// </summary>
        /// <param name="srcModuleId"></param>
        /// <param name="srcTxnId"></param>
        /// <param name="destTxnTypeId"></param>
        /// <returns></returns>
        [Route("GetModuleCommunicationDetailsById")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblModuleCommunicationTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetModuleCommunicationDetailsById(Int32 srcModuleId, Int32 srcTxnId, Int32 destTxnTypeId)
        {
            try
            {
                List<TblModuleCommunicationTO> list = _iTblModuleCommunicationBL.SelectAllTblModuleCommunicationListById(srcModuleId, srcTxnId);
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("GetAllTallyRefDtlTOList")]
        [HttpGet]
        public List<TblItemTallyRefDtlsTO> GetAllTallyRefDtlTOList(Int32 brandId)
        {
            return _iTblItemTallyRefDtlsBL.SelectAllTallyRefDtlTOList(brandId);
        }

        ///// <summary>
        ///// Priyanka [27-03-2018] : Added to get list of items for Item Test Certificate
        ///// </summary>
        ///// <param name="brandId"></param>
        ///// <param name="prodCatId"></param>
        ///// <param name="ProdSpecId"></param>
        ///// <returns></returns>

        //[Route("GetTestCertItemValuesList")]
        //[HttpGet]
        //public List<TblTestCertItemValueTO> GetTestCertItemValuesList(Int32 brandId, Int32 prodCatId, Int32 prodSpecId)
        //{
        //    return BL.TblTestCertItemValueBL.SelectAllTblTestCertItemValueList(brandId, prodCatId, prodSpecId);

        //}


        //Kiran[15-Mar-2018] Added for get All  Dimesion Tables From tblMasterDimension
        [Route("GetMasterDimensionList")]
        [HttpGet]
        public List<DimensionTO> GetMasterDimensionList()
        {
            List<DimensionTO> masterList = _iDimensionBL.SelectAllMasterDimensionList();
            return masterList;
        }
       
        //Kiran[15-Mar-2018] Added for Add New Dimesion in Selected Table
        [Route("PostNewMasterData")]
        [HttpPost]
        public ResultMessage PostNewMasterData([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                Int32 result = 0;
                var tableData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data["data"].ToString());
                var tableName = data["tableName"].ToString();
                var isAdded = data["isAdded"].ToString();
                if (isAdded == "True")
                {
                    result = _iDimensionBL.saveNewDimensional(tableData, tableName);
                }
                else
                {
                    result = _iDimensionBL.UpdateDimensionalData(tableData, tableName);
                }
                if (result != 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostNewMasterData";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        //Kiran[31-10-2018] 
        [Route("removeAllMsgHistory")]
        [HttpGet]
        public ResultMessage removeAllMsgHistory()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                Int32 result = 0;
                result = _iTblSessionBL.deleteAllMsgData();
                if (result != 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method removeAllMsgHistory";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }

        }

        [Route("GetPagesListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetPagesListForDropDown(Int32 moduleId = 0)
        {
            List<TblPagesTO> pagesList = _iTblPagesBL.SelectAllTblPagesList(moduleId);
            List<DropDownTO> list = new List<DropDownTO>();
            if (pagesList != null && pagesList.Count > 0)
            {
                for (int i = 0; i < pagesList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = pagesList[i].PageName;
                    dropDownTO.Value = pagesList[i].IdPage;
                    list.Add(dropDownTO);
                }
            }
            return list;
        }

        [Route("GetPageElementListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetPageElementListForDropDown(Int32 pageId = 0)
        {
            List<TblPageElementsTO> pageEleList = _iTblPageElementsBL.SelectAllTblPageElementsList(pageId);
            List<DropDownTO> list = new List<DropDownTO>();
            if (pageEleList != null && pageEleList.Count > 0)
            {
                for (int i = 0; i < pageEleList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = pageEleList[i].ElementDisplayName;
                    dropDownTO.Value = pageEleList[i].IdPageElement;
                    list.Add(dropDownTO);
                }
            }
            return list;
        }

        [Route("GetVehicleTypesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetVehicleTypesForDropDown()
        {
            List<DimVehicleTypeTO> vehTypeList = _iDimVehicleTypeBL.SelectAllDimVehicleTypeList();
            List<DropDownTO> list = new List<DropDownTO>();
            if (vehTypeList != null && vehTypeList.Count > 0)
            {
                for (int i = 0; i < vehTypeList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = vehTypeList[i].VehicleTypeDesc;
                    dropDownTO.Value = vehTypeList[i].IdVehicleType;
                    list.Add(dropDownTO);
                }
            }

            return list;
        }

        [Route("GetCDStructureForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCDStructureForDropDown(Int32 moduleId=0)
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectCDStructureForDropDown(moduleId);
            return statusList;
        }

        [Route("GetDeliveryPeriodForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetDeliveryPeriodForDropDown()
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectDeliPeriodForDropDown();
            return statusList;
        }
        /// <summary>
        /// Priyanka [23-04-2019] : Added to get the SAP Master Value according to their type.
        /// </summary>
        /// <returns></returns>
        [Route("GetSAPMasterDropDown")]
        [HttpGet]
        public List<DropDownTO> GetSAPMasterDropDown(Int32 dimensionId)
        {
            List<DropDownTO> SAPMasterList = _iDimensionBL.GetSAPMasterDropDown(dimensionId);
            return SAPMasterList;
        }


        [Route("GetMaxAllowedDeliveryPeriod")]
        [HttpGet]
        public Int32 GetMaxAllowedDeliveryPeriod()
        {
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_MAX_ALLOWED_DEL_PERIOD);
            Int32 maxAllowedDelPeriod = 7;

            if (tblConfigParamsTO != null)
                maxAllowedDelPeriod = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

            return maxAllowedDelPeriod;
        }

        [Route("GetStockConfig")]
        [HttpGet]
        public Int32 GetStockConfigIsConsolidate()
        {
            return _iTblConfigParamsBL.GetStockConfigIsConsolidate();
        }


        [Route("GetDistrictForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetDistrictForDropDown(Int32 stateId)
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectDistrictForDropDown(stateId);
            return statusList;
        }

        [Route("getServerTime")]
        [HttpGet]
        public IActionResult getServerTime()
        {
            DateTime currentDate = _iCommon.ServerDateTime;
            return Ok(currentDate);
        }

        [Route("GetCountriesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCountriesForDropDown()
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectCountriesForDropDown();
            return statusList;
        }

        [Route("GetStatesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetStatesForDropDown(Int32 countryId)
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectStatesForDropDown(countryId);
            return statusList;
        }

        [Route("GetTalukasForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetTalukasForDropDown(Int32 districtId)
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectTalukaForDropDown(districtId);
            return statusList;
        }
        /// <summary>
        /// Hrishikesh[27 - 03 - 2018] Added to get taluka by district
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        [Route("GetTalukasForStateMaster")]
        [HttpGet]
        public List<StateMasterTO> GetTalukasForStateMaster(Int32 districtId)
        {
            List<StateMasterTO> statusList = _iDimensionBL.GetTalukasForStateMaster(districtId);
            return statusList;
        }

        /// <summary>
        ///Hrishikesh[27 - 03 - 2018] Added to get district
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        [Route("GetDistrictsForStateMaster")]
        [HttpGet]
        public List<StateMasterTO> GetDistrictsForStateMaster(Int32 districtId)
        {
            List<StateMasterTO> statusList = _iDimensionBL.GetDistrictsForStateMaster(districtId);
            return statusList;
        }

        [Route("GetSalutaionForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetSalutaionForDropDown()
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectSalutationsForDropDown();
            return statusList;
        }

        [Route("GetCommerLicensesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCommerLicensesForDropDown()
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectOrgLicensesForDropDown();
            return statusList;
        }

        [Route("GetRoleListWrtAreaAllocation")]
        [HttpGet]
        public List<DropDownTO> GetRoleListWrtAreaAllocation()
        {
            List<DropDownTO> roleList = _iDimensionBL.SelectRoleListWrtAreaAllocationForDropDown();
            return roleList;
        }

        [Route("GetRoleListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetRoleListForDropDown()
        {
            List<DropDownTO> roleList = _iDimensionBL.SelectAllSystemRoleListForDropDown();
            return roleList;
        }
        //Aniket [1-7-2019] added to get all role list by UserId
        [Route("GetRoleListForDropdownByUserId")]
        [HttpGet]
        public List<DropDownTO> GetRoleListForDropdownByUserId(Int32 userId)
        {
            List<DropDownTO> roleList = _iDimensionBL.SelectAllSystemRoleListForDropDownByUserId(userId);
            return roleList;
        }
        [Route("GetCnfDistrictForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCnfDistrictForDropDown(Int32 cnfOrgId)
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectCnfDistrictForDropDown(cnfOrgId);
            return statusList;
        }

        [Route("GetStockYardList")]
        [HttpGet]
        public List<TblStockYardTO> GetStockYardList()
        {
            return _iTblStockYardBL.SelectAllTblStockYardList();
        }

        [Route("GetSuperwisorDetailList")]
        [HttpGet]
        public List<TblSupervisorTO> GetSuperwisorDetailList()
        {
            return _iTblSupervisorBL.SelectAllTblSupervisorList();
        }


        [Route("GetTransportModeForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetTransportModeForDropDown()
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectAllTransportModeForDropDown();
            return statusList;
        }

        [Route("GetInvoiceTypeForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetInvoiceTypeForDropDown()
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectInvoiceTypeForDropDown();
            return statusList;
        }

        [Route("GetCurrencyForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCurrencyForDropDown()
        {
            List<DropDownTO> statusList = _iDimensionBL.SelectCurrencyForDropDown();
            return statusList;
        }

        [Route("GetInvoiceStatusForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetInvoiceStatusForDropDown()
        {
            List<DropDownTO> statusList = _iDimensionBL.GetInvoiceStatusForDropDown();
            return statusList;
        }

        //Vijaymala[08-09-2017] Added To Get Designation List
        [Route("GetDesignationList")]
        [HttpGet]
        public List<DimMstDesignationTO> GetDesignationList()
        {
            return _iDimMstDesignationBL.SelectAllDimMstDesignationList();
        }

        //Kiran [09-01-2018] Added To Get Emailconfigration List
        [Route("GetEmailConfigurationList")]
        [HttpGet]
        public List<TblEmailConfigrationTO> GetEmailConfigurationList()
        {
            return _iTblEmailConfigrationBL.SelectAllDimEmailConfigrationList();
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] added to fill UnitMeasures Drop Down
        /// </summary>
        /// <returns></returns>
        [Route("GetUnitMeasuresForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetUnitMeasuresForDropDown()
        {
            List<DropDownTO> unitMeasuresList = _iDimUnitMeasuresBL.SelectAllUnitMeasuresListForDropDown();
            return unitMeasuresList;
        }

        /// <summary>
        /// Kiran [08-Sep-2018] added to fill UnitMeasures Drop Down Using Cat Id
        /// </summary>
        /// <returns></returns>
        [Route("SelectAllUnitMeasuresForDropDownByCatId")]
        [HttpGet]
        public List<DropDownTO> SelectAllUnitMeasuresForDropDownByCatId(Int32 unitCatId)
        {
            List<DropDownTO> unitMeasuresList = _iDimUnitMeasuresBL.SelectAllUnitMeasuresForDropDownByCatId(unitCatId);
            return unitMeasuresList;
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] added to fill UnloadingStandDesc Drop Down
        /// </summary>
        /// <returns></returns>
        [Route("GetUnloadingStandDescForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetUnloadingStandDescForDropDown()
        {
            List<DropDownTO> unloadingStandDescList = _iTblUnloadingStandDescBL.SelectAllUnloadingStandDescForDropDown();
            return unloadingStandDescList;
        }

        /// <summary>
        /// Vaibhav [15-Sep-2017] Get all department
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllDepartmentList")]
        [HttpGet]
        public List<DimMstDeptTO> GetAllDepartmentList()
        {
            return _iDimMstDeptBL.SelectAllDimMstDeptList();
        }

        /// <summary>
        /// Deepali [19-Oct-2018] Get all department
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetUserListDepartmentWise")]
        [HttpGet]
        public List<DropDownTO> GetUserListDepartmentWise(string deptId)
        {
            return _iDimensionBL.GetUserListDepartmentWise(deptId);
        }

        /// <summary>
        /// Vaibhav [15-Sep-2017] added to fill division drop down.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetDivisionDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetDivisionDropDownList(Int32 DeptTypeId = 0)
        {
            List<DropDownTO> departmentMasterList = _iDimMstDeptBL.SelectDivisionDropDownList(DeptTypeId);
            return departmentMasterList;
        }


        /// <summary>
        /// Vaibhav [18-Sep-2017] added to fill department drop down by specific division 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [Route("GetDepartmentDropDownListByDivision")]
        [HttpGet]
        public List<DropDownTO> GetDepartmentDropDownListByDivision(Int32 DivisionId)
        {
            List<DropDownTO> divisionList = _iDimMstDeptBL.SelectDepartmentDropDownListByDivision(DivisionId);
            return divisionList;
        }

        /// <summary>
        /// Vaibhav [19-Sep-2017] Added to select BOM department
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetBOMDepartmentTO")]
        [HttpGet]
        public DropDownTO GetBOMDepartmentTO()
        {
            DropDownTO BOMDepartmentTO = _iDimMstDeptBL.SelectBOMDepartmentTO();
            return BOMDepartmentTO;
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] Get all UnloadingStandDesc List
        /// </summary>
        /// <returns></returns>
        [Route("GetUnloadingStandDescList")]
        [HttpGet]
        public List<TblUnloadingStandDescTO> GetUnloadingStandDescList()
        {
            List<TblUnloadingStandDescTO> unloadingStandDescList = _iTblUnloadingStandDescBL.SelectAllTblUnloadingStandDescList();
            return unloadingStandDescList;
        }

        /// <summary>
        /// Vaibhav [25-Sep-2017] get all sub departments for drop down by department
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetSubDepartmentDropDownListByDepartment")]
        [HttpGet]
        public List<DropDownTO> GetSubDepartmentDropDownListByDepartment(int DepartmentId)
        {
            List<DropDownTO> subDepartmentTOList = _iDimMstDeptBL.SelectSubDepartmentDropDownListByDepartment(DepartmentId);
            return subDepartmentTOList;
        }


  //Aniket [26-08-2019] added to fetch dimOrgType list
        [HttpGet]
        [Route("SelectAllDimOrgTypeList")]
        public List<DimOrgTypeTO> SelectAllDimOrgTypeList()
        {
            List<DimOrgTypeTO> dimOrgTypeTOList = _iDimOrgTypeBL.SelectAllDimOrgTypeList();
            return dimOrgTypeTOList;
        }

        /// <summary>
        /// Vaibhav [25-Sep-2017] get all designation for drop down
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllDesignationForDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetAllDesignationForDropDown()
        {
            List<DropDownTO> designationList = _iDimMstDesignationBL.SelectAllDesignationForDropDownList();
            return designationList;
        }

        /// <summary>
        /// Vaibhav [27-Sep-2017] added to select all designation list
        /// </summary>
        /// <param name="value"></param>
        [Route("GetReportingTypeList")]
        [HttpGet]
        public List<DropDownTO> GetReportingTypeList()
        {
            return _iDimensionBL.GetReportingType();
        }

        /// <summary>
        /// Vaibhav [3-Oct-2017] added to select visit issue reason list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetVisitIssueReasonsList")]
        [HttpGet]
        public List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList()
        {
            return _iDimensionBL.GetVisitIssueReasonsList();
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetBrandList")]
        [HttpGet]
        public List<DropDownTO> GetBrandList(int categoryType)
        {
            List<DropDownTO> brandList = _iDimensionBL.SelectBrandList(categoryType);
            return brandList;
        }

        [Route("GetBrandListV2")]
        [HttpGet]
        public List<DimBrandTO> GetBrandListV2()
        {
            List<DimBrandTO> brandList = _iDimensionBL.SelectBrandListV2();
            return brandList;
        }


        //Vijaymala[08-09-2017] Added To Get Designation List
        [Route("GetStockConfigurationList")]
        [HttpGet]
        public List<TblStockConfigTO> GetStockConfigurationList()
        {
            return _iTblStockConfigBL.SelectAllTblStockConfigTOList();
        }


        [Route("GetAllBrandList")]
        [HttpGet]
        public List<DimBrandTO> GetAllBrandList()
        {
            return _iDimBrandBL.SelectAllDimBrandList();
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetLoadingLayerList")]
        [HttpGet]
        public List<DropDownTO> GetLoadingLayerList()
        {
            List<DropDownTO> loadingLayerList = _iDimensionBL.SelectLoadingLayerList();
            return loadingLayerList;
        }

        [Route("GetBookingTaxCategoryList")]
        [HttpGet]
        public List<DropDownTO> GetBookingTaxCategoryList()
        {
            List<DropDownTO> BookingTaxCategoryList = _iDimensionBL.GetBookingTaxCategoryList();
            return BookingTaxCategoryList;
        }

        [Route("GetBookingCommentCategoryList")]
        [HttpGet]
        public List<DropDownTO> GetBookingCommentCategoryList()
        {
            List<DropDownTO> BookingCommentCategoryList = _iDimensionBL.GetBookingCommentCategoryList();
            return BookingCommentCategoryList;
        }
        /// <summary>
        ///Sudhir[09-12-2017] Added For GetAllStatesList 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllStatesList")]
        [HttpGet]
        public List<DimStateTO> GetAllStatesList()
        {
            return _iDimStateBL.SelectAllDimState();
        }
        /// <summary>
        /// Vijaymala[10/01/2018]:Added to get state code
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [Route("GetStateCode")]
        [HttpGet]
        public DropDownTO GetStateCode(Int32 stateId)
        {
            DropDownTO stateCodeTO = _iDimensionBL.SelectStateCode(stateId);
            return stateCodeTO;
        }
        /// <summary>
        /// Vijaymala:Added to get group list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>

        [Route("GetAllGroupList")]
        [HttpGet]
        public List<TblGroupTO> GetAllGroupList()
        {
            return _iTblGroupBL.SelectAllTblGroupList();
        }

        [Route("GetArrangeForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetArrangeForDropDown()
        {
            return _iDimensionBL.GetArrangeForDropDown();
        }

        [Route("GetArrangeVisitToDropDown")]
        [HttpGet]
        public List<DropDownTO> GetArrangeVisitToDropDown()
        {
            return _iDimensionBL.GetArrangeVisitToDropDown();
        }

        [Route("GetCallBySelfForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCallBySelfForDropDown()
        {
            return _iDimensionBL.GetCallBySelfForDropDown();
        }


        /// <summary>
        /// Sudhir[15-MAR-2018] Added for Select All Enquiry Channels  
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllEnquiryChannelsList")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllEnquiryChannelsList()
        {
            try
            {
                List<DropDownTO> list = _iDimensionBL.SelectAllEnquiryChannels();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Sudhir[15-MAR-2018] Added for Select All Industry Sector.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllIndustrySectorsList")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllIndustrySectorsList()
        {
            try
            {
                List<DropDownTO> list = _iDimensionBL.SelectAllIndustrySector();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Sudhir[08-MARCH-2018]-This Methods Shows AllFirmTypes
        /// </summary>
        /// <returns></returns>
        [Route("GetAllFirmTypesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetAllFirmTypesForDropDown()
        {
            List<DropDownTO> firmTypesList = _iDimensionBL.GetAllFirmTypesForDropDown();
            return firmTypesList;
        }
        /// <summary>
        /// Sudhir[23-APR-2018] Added for Select All OtherDesignations .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllOtherDesignations")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblOtherDesignationsTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllOtherDesignations()
        {
            try
            {
                List<TblOtherDesignationsTO> list = _iTblOtherDesignationsBL.SelectAllTblOtherDesignations();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        //Sudhir [10-05-2018] Added To Get Emailconfigration List
        [Route("GetUploadedFileBasedOnFileType")]
        [HttpGet]
        public List<TblDocumentDetailsTO> GetUploadedFileBasedOnFileType(Int32 FilteTypeId, Int32 createdBy)
        {
            return _iTblDocumentDetailsBL.GetUploadedFileBasedOnFileType(FilteTypeId, createdBy);
        }

        //Deepali [22-10-2018] Added To Get Emailconfigration List
        [Route("GetUploadedFileBasedOnDocumentId")]
        [HttpGet]
        public List<TblDocumentDetailsTO> GetUploadedFileBasedOnDocumentId(string DocumentIds)
        {
            return _iTblDocumentDetailsBL.GetUploadedFileBasedOnDocumentId(DocumentIds);
        }
        /// <summary>
        /// Sudhir[22-MAY-2018] Get All UnitMeasurement Types
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [Route("GetAllUnitMeasurement")]
        [HttpGet]
        public List<DimUnitMeasuresTO> GetAllUnitMeasurement()
        {
            return _iDimUnitMeasuresBL.SelectAllDimUnitMeasuresList();
        }
        /// <summary>
        /// Sudhir[08-MARCH-2018]-This Methods Shows AllInfluencerTypes
        /// </summary>
        /// <returns></returns>
        [Route("GetAllInfluencerTypesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetAllInfluencerTypesForDropDown()
        {
            List<DropDownTO> firmTypesList = _iDimensionBL.GetAllInfluencerTypesForDropDown();
            return firmTypesList;
        }

        /// <summary>
        /// vinod on Date:12/12/2017
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("getProdSpecDescList")]
        [HttpGet]
        public List<DimProdSpecDescTO> getProdSpecDescList()
        {
            return _iDimProdSpecDescBL.SelectAllDimProdSpecDescList();
        }

        //Vijaymala[28-03-2018] Added to get Address Type
        [Route("GetAddressTypeList")]
        [HttpGet]
        public List<DropDownTO> GetAddressTypeList()
        {
            List<DropDownTO> addressTypeList = _iDimensionBL.SelectAddressTypeListForDropDown();
            return addressTypeList;
        }



        /// <summary>
        /// Vaibhav [06-APril-2018] Added to get compartment with location name.
        /// </summary>
        /// <returns></returns>
        [Route("GetLocationWiseCompartmentListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetLocationWiseCompartmentListForDropDown()
        {
            return _iDimensionBL.GetLocationWiseCompartmentList();
        }


        [Route("GetCDStructureForDropDownById")]
        [HttpGet]
        public DropDownTO GetCDStructureForDropDownById(Int32 cdstructureId)
        {
            return _iDimensionBL.SelectCDDropDown(cdstructureId);

        }

        /// <summary>
        /// Sudhir[21-JUNE-2018]
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [Route("GetDropDownListOnPersonId")]
        [HttpGet]
        public List<DropDownTO> GetDropDownListOnPersonId(Int32 personId)
        {
            return _iTblPersonBL.SelectDropDownListOnPersonId(personId);
        }

        //Added By kiran for CRM entity range 
        [Route("SelectEntityRangeTOFromVisitType")]
        [HttpGet]
        public IActionResult SelectEntityRangeTOFromVisitType(string entityName, DateTime createdOn)
        {
            try
            {
                TblEntityRangeTO EntityRangeTO = _iDimensionBL.SelectEntityRangeTOFromVisitType(entityName, createdOn);
                if (EntityRangeTO != null)
                {
                    return Ok(EntityRangeTO);
                }
                else
                {
                    return NotFound(EntityRangeTO);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// Dipali[26-7-2018] Added for get List of role and organization type Configuration based on VisitTypeId and personTypeId.
        /// </summary>
        /// <param name="visitTypeId"></param>
        /// <param name="personTypeId"></param>
        /// <returns></returns>
        [Route("GetRoleOrgListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetRoleOrgListForDropDown(int visitTypeId, int personTypeId)
        {
            List<DropDownTO> roleOrgList = _iTblRoleOrgSettingBL.SelectAllSystemRoleOrgListForDropDown(visitTypeId, personTypeId);
            return roleOrgList;
        }

        //Dipali[26-07-2018] For RoleOrgTo List Mapping With Role
        [Route("GetAllRoleListForDropDown")]
        [HttpGet]
        public List<RoleOrgTO> GetAllRoleListForDropDown(int visitTypeId, int personTypeId)
        {
            return _iDimensionBL.SelectAllSystemRoleListForTbl(visitTypeId, personTypeId);

        }

        //Dipali[26-07-2018] For RoleOrgTo List Mapping With Role
        [Route("GetAllOrgListForDropDown")]
        [HttpGet]
        public List<RoleOrgTO> GetAllOrgListForDropDown(int visitTypeId, int personTypeId)
        {
            return _iDimensionBL.SelectAllSystemOrgListForTbl(visitTypeId, personTypeId);

        }
        [Route("GetAllVisitListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetAllVisitListForDropDown()
        {
            List<DropDownTO> visitList = _iDimensionBL.SelectAllVisitTypeListForDropDown();
            return visitList;
        }


        [Route("GetTaskModuleDetailsByEntityId")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblTaskModuleExtTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetTaskModuleDetailsByEntityId(Int32 EntityId)
        {
            try
            {
                List<TblTaskModuleExtTO> list = _iTblTaskModuleExtBL.SelectTaskModuleDetailsByEntityId(EntityId);
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [Route("GetDefaultRoleListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetDefaultRoleListForDropDown()
        {
            List<DropDownTO> roleList = _iDimensionBL.SelectDefaultRoleListForDropDown();
            return roleList;
        }


        /// <summary>
        /// Sudhir[30-08-2018] To get GetTasksWithousSubscList using moduleId and entityId
        /// </summary>
        /// <param name="srcModuleId"></param>
        /// <param name="srcTxnId"></param>
        /// <returns></returns>
        [Route("GetTasksWithousSubscList")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TbltaskWithoutSubscTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetTasksWithousSubscList(Int32 srcModuleId, Int32 entityId)
        {
            try
            {
                List<TbltaskWithoutSubscTO> list = _iTbltaskWithoutSubscBL.SelectTbltaskWithoutSubscList(srcModuleId, entityId);
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Vijaymala[08-09-2018]added to get state
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("GetStatesForDropDownAccToBooking")]
        [HttpGet]
        public List<DropDownTO> GetStatesForDropDownAccToBooking(Int32 countryId, string fromDate, string toDate)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;

            List<DropDownTO> statusList = _iDimensionBL.SelectStatesForDropDownAccToBooking(countryId, frmDt, toDt);
            return statusList;
        }

        /// <summary>
        /// Vijaymala[08-09-2018]added :To get distict from booking 
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>

        [Route("GetDistrictForDropDownAccToBooking")]
        [HttpGet]
        public List<DropDownTO> GetDistrictForDropDownAccToBooking(Int32 stateId, string fromDate, string toDate)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;

            List<DropDownTO> statusList = _iDimensionBL.SelectDistrictForDropDownAccToBooking(stateId, frmDt, toDt);
            return statusList;
        }

        //
        /// <summary>
        /// Sudhir[06-09-2018] To get Fixed DropDown List 
        /// </summary>
        /// <param name="srcModuleId"></param>
        /// <param name="srcTxnId"></param>
        /// <returns></returns>
        [Route("GetFixedDropDownValues")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetFixedDropDownValues()
        {
            try
            {
                List<DropDownTO> list = _iDimensionBL.GetFixedDropDownValues();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        //
        /// <summary>
        /// Sudhir[06-09-2018] To get Fixed DropDown List 
        /// </summary>
        /// <returns></returns>
        [Route("GetMasterSiteTypes")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetMasterSiteTypes(int parentSiteTypeId = 0)
        {
            try
            {
                List<DropDownTO> list = _iDimensionBL.SelectMasterSiteTypes(parentSiteTypeId);
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Yogesh [24-10-2018] Added to get Department id From UserId Id
        /// </summary>
        /// <param name="id"></param>
        [Route("getDepartmentIdFromUserId")]
        [HttpGet]
        public TblRoleTO getDepartmentIdFromUserId(Int32 userId)
        {
            TblRoleTO statusList = _iTblRoleBL.getDepartmentIdFromUserId(userId);
            return statusList;
        }

        //Aniket [8-7-2019]added to get customized page settings of  page based on PageId
        [Route("getPageConfigurationByPageId")]
        [HttpGet]
        public List<DimConfigurePageTO> getPageConfigurationByPageId(Int32 pageId)
        {
            List<DimConfigurePageTO> dimConfigurePageTOList = _iDimConfigurePageBL.GetConfigurationByPageId(pageId);
            return dimConfigurePageTOList;
        }

        #region Sizes Avilable Pipes and Strips 
     
        [Route("getTblPipesList")]
        [HttpGet]
        public List<TblPipesTO> getTblPipesList()
        {
            DateTime createdOn = _iCommon.ServerDateTime;
            return _iDimProdSpecDescBL.SelectAllTblPipesList(createdOn);
        }

        [Route("getTblStripsList")]
        [HttpGet]
        public List<TblStripsTO> getTblStripsList()
        {
            DateTime createdOn = _iCommon.ServerDateTime;
            return _iDimProdSpecDescBL.SelectAllTblStripsList(createdOn);
        }

        //[Route("GetAllPipesInchForDropDown")]
        //[HttpGet]
        //public List<TblPipesDropDownTo> GetAllPipesInchForDropDown()
        //{
        //    List<TblPipesDropDownTo> PipesInchList = _iDimProdSpecDescBL.GetAllPipesInchForDropDown();
        //    return PipesInchList;
        //}
        [Route("GetAlltblPipesStripCommonSizeForDropDown")]
        [HttpGet]
        public List<TblPipesStripCommonSizeTO> GetAlltblPipesStripCommonSizeForDropDown()
        {
            List<TblPipesStripCommonSizeTO> PipesStripCommonSize = _iDimProdSpecDescBL.GetAlltblPipesStripCommonSizeForDropDown();
            return PipesStripCommonSize;
        }
        [Route("GetAlltblPipesStripCommonThicknessForDropDown")]
        [HttpGet]
        public List<TblPipesStripCommonThicknessTO> GetAlltblPipesStripCommonThicknessForDropDown()
        {
            List<TblPipesStripCommonThicknessTO> PipesStripCommonThickness = _iDimProdSpecDescBL.GetAlltblPipesStripCommonThicknessForDropDown();
            return PipesStripCommonThickness;
        }
        [Route("GetAlltblStripsGradeForDropDown")]
        [HttpGet]
        public List<TblStripsGradeDropDownTo> GetAlltblStripsGradeForDropDown()
        {
            List<TblStripsGradeDropDownTo> StripGradeList = _iDimProdSpecDescBL.GetAlltblStripsGradeForDropDown();
            return StripGradeList;
        }
        [Route("GetAlltblPipesStripCommonQuantityForDropDown")]
        [HttpGet]
        public List<TblPipesStripCommonQuantityTO> GetAlltblPipesStripCommonQuantityForDropDown()
        {
            List<TblPipesStripCommonQuantityTO> PipesStripCommonQuantity = _iDimProdSpecDescBL.GetAlltblPipesStripCommonQuantityForDropDown();
            return PipesStripCommonQuantity;
        }

        [Route("GetTblInchForDropDown")]
        [HttpGet]
        public List<TblInchDropDownTO> GetTblInchForDropDown()
        {
            List<TblInchDropDownTO> InchList = _iDimProdSpecDescBL.GetTblInchForDropDown();
            return InchList;
        }


        [Route("GetTblSizeForDropDown")]
        [HttpGet]
        public List<TblSizeTO> GetTblSizeForDropDown(Int32 idInch = 0)
        {
            List<TblSizeTO> SizeList = _iDimProdSpecDescBL.GetTblSizeForDropDown(idInch);
            return SizeList;
        }

        [Route("GetTblThicknessForDropDown")]
        [HttpGet]
        public List<TblThicknessDropDownTO> GetTblThicknessForDropDown()
        {
            List<TblThicknessDropDownTO> ThicknessList = _iDimProdSpecDescBL.GetTblThicknessForDropDown();
            return ThicknessList;
        }

        [Route("GetTblWidthForDropDown")]
        [HttpGet]
        public List<TblWidthDropDownTO> GetTblWidthForDropDown()
        {
            List<TblWidthDropDownTO> WidthList = _iDimProdSpecDescBL.GetTblWidthForDropDown();
            return WidthList;
        }
        #endregion


        #endregion

        #region POST

        /// <summary>
        ///Sudhir[24-APR-2018] Added for Uploading Image  
        /// </summary>
        /// <param name="tblDocumentDetailsTOTblDocumentDetailsTO"></param>
        /// <returns></returns>
        [Route("UploadDocument")]
        [HttpPost]
        public ResultMessage UploadDocument([FromBody] List<TblDocumentDetailsTO> data)
        {
            List<TblDocumentDetailsTO> tblDocumentDetailsTOList = data;
            return _iTblDocumentDetailsBL.UploadDocument(tblDocumentDetailsTOList);
        }
        
        [Route("UploadDocumentset")]
        [HttpPost]
        public ResultMessage UploadDocumentset([FromBody] JObject data)
        {
            List<TblDocumentDetailsTO> tblDocumentDetailsTOList = JsonConvert.DeserializeObject<List<TblDocumentDetailsTO>>(data["tblDocumentDetailsTOList"].ToString());
            return _iTblDocumentDetailsBL.UploadDocument(tblDocumentDetailsTOList);
        }

        /// <summary>
        /// Sudhir [07-05-2018] Added to update email configration
        /// </summary>
        /// <param name="id"></param>
        [Route("SendShareDocumentToMail")]
        [HttpPost]
        public ResultMessage SendShareDocumentToMail([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            //ShareDetials
            try
            {
                //"TblCRMShareDocsDetailsTO": 

                int result = 0;
                TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO = JsonConvert.DeserializeObject<TblCRMShareDocsDetailsTO>(data["tblCRMShareDocsDetailsTO"].ToString());
                if (tblCRMShareDocsDetailsTO != null)
                {
                    resultMessage = _iTblCRMShareDocsDetailsBL.ShareDetials(tblCRMShareDocsDetailsTO);
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendShareDocumentToMail");
                // resultMessage.DefaultBehaviour("Failed to Send Email");
                resultMessage.DisplayMessage = "Email Wasn't Sent.";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
        }

        //SaveModuleCommunicationDetails
        /// <summary>
        /// Kiran [16-08-2018] Added to Save Module Communication Details
        /// </summary>
        /// <param name="id"></param>
        [Route("SaveModuleCommunicationDetails")]
        [HttpPost]
        public ResultMessage SaveModuleCommunicationDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblModuleCommunicationTO> tblModuleCommunicationList = JsonConvert.DeserializeObject<List<TblModuleCommunicationTO>>(data["ModuleCommunicationList"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblModuleCommunicationList == null)
                {
                    resultMessage.DefaultBehaviour("tblModuleCommunicationList found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                int result = _iTblModuleCommunicationBL.InsertTblModuleCommunication(tblModuleCommunicationList, loginUserId);
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
                resultMessage.DefaultExceptionBehaviour(ex, "SaveTaskModuleDetails");
                return resultMessage;
            }
        }

        //Kiran[25-oct-2018] start session of new conversion
        [Route("PostNewSessionData")]
        [HttpPost]
        public ResultMessage PostNewSessionData([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            int result = 0;
            try
            {
                TblSessionTO sessionTo = JsonConvert.DeserializeObject<TblSessionTO>(data.ToString());
                sessionTo.IsActive = 1;
                sessionTo.IsEndSession = 0;
                sessionTo.StartTime = _iCommon.ServerDateTime;
                sessionTo.EndTime = _iCommon.ServerDateTime;
                result = _iTblSessionBL.InsertTblSession(sessionTo);
                if (result != 0)
                {
                    resultMessage.Tag = sessionTo.Idsession;
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Tag = result;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostNewSessionData";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        //Kiran[15-01-2019] Store all list of AddOn Transaction
        [Route("PostAddOnsDetails")]
        [HttpPost]
        public ResultMessage PostAddOnsDetails([FromBody] JObject data)
        {

            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblAddonsFunDtlsTO> tblAddonsFunDtlsTOList = JsonConvert.DeserializeObject<List<TblAddonsFunDtlsTO>>(data["tblAddonsFunDtlsTO"].ToString());
                if (tblAddonsFunDtlsTOList == null || tblAddonsFunDtlsTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("tblAddonsFunDtlsTOList found null");
                    return resultMessage;
                }
                #region store all list against transaction id
                foreach (var item in tblAddonsFunDtlsTOList)
                {
                    if (item.IdAddonsfunDtls == 0)
                        resultMessage = _iTblAddonsFunDtlsBL.InsertTblAddonsFunDtls(item);
                    else
                        resultMessage = _iTblAddonsFunDtlsBL.UpdateTblAddonsFunDtls(item);

                    if (resultMessage.Result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in PostAddOnsDetails... Record could not be saved");
                        return resultMessage;
                    }
                }

                #endregion
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostAddOnsDetails");
                return resultMessage;
            }
        }

        //Kiran[25-oct-2018] Save All Conversion History
        [Route("SaveAllConversionHistory")]
        [HttpPost]
        public ResultMessage SaveAllConversionHistory([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            int result = 0;
            try
            {
                List<TblSessionHistoryTO> SessionHistoryTOList = JsonConvert.DeserializeObject<List<TblSessionHistoryTO>>(data["SessionHistoryList"].ToString());
                if (SessionHistoryTOList != null && SessionHistoryTOList.Count > 0)
                {
                    foreach (var SessionHistoryTO in SessionHistoryTOList)
                    {
                        if (SessionHistoryTO.IsEdit == 1)
                        {
                            result = _iTblSessionHistoryBL.InsertTblSessionHistory(SessionHistoryTO);
                            if (SessionHistoryTO.IsSendNotification == 1)
                            {
                                TblUserTO userTo = _iTblUserBL.SelectTblUserTO(SessionHistoryTO.ConversionUserId);
                                string[] devices = new string[1];
                                if (userTo.RegisteredDeviceId != null)
                                {
                                    devices[0] = userTo.RegisteredDeviceId;
                                    String notifyBody = userTo.UserDisplayName + Environment.NewLine + SessionHistoryTO.ConversionBody;
                                    String notifyTitle = "SimpliChat";
                                    _iVitplNotify.NotifyToRegisteredDevices(devices, notifyBody, notifyTitle,0);
                                    resultMessage.MessageType = ResultMessageE.Information;
                                    resultMessage.Text = "Acknowledged Sucessfully";
                                    resultMessage.Result = 1;
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {
                            result = _iTblSessionHistoryBL.UpdateTblSessionHistory(SessionHistoryTO);
                        }
                    }
                    if (result != 0)
                    {
                        //SessionHistoryTO.SessionId = result;
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour();
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method SaveAllConversionHistory";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }
        [Route("endChatSession")]
        [HttpGet]
        public ResultMessage endChatSession(Int32 idSession)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            int result = 0;
            try
            {
                result = _iTblSessionBL.UpdateTblSession(idSession);
                if (result != 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method endChatSession";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        [Route("UploadMultipleTypesFile")]
        [HttpPost]
        public async Task<IActionResult> UploadMultipleTypesFile(List<IFormFile> file, Int32 createdBy, Int32 FileTypeId, Int32 moduleId = 1)
        {
            //_iTblDocumentDetailsBL tblDocumentDetailsBL = new _iTblDocumentDetailsBL();
            Task task = _iTblDocumentDetailsBL.UploadMultipleTypesFile(file, createdBy, FileTypeId, moduleId);
            return Ok(task);
        }
        // POST api/values
        [Route("AddNewStockYard")]
        [HttpPost]
        public Int32 AddNewStockYard([FromBody] JObject data)
        {
            try
            {
                TblStockYardTO tblStockYardTO = JsonConvert.DeserializeObject<TblStockYardTO>(data["stockYardInfo"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblStockYardTO == null)
                {
                    return 0;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    return 0;
                }

                tblStockYardTO.CreatedOn = _iCommon.ServerDateTime;
                tblStockYardTO.CreatedBy = Convert.ToInt32(loginUserId);
                return _iTblStockYardBL.InsertTblStockYard(tblStockYardTO);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        //Hruhsikesh[29 - 03 - 2018]
        //Added to add District

        [Route("PostAddDistrict")]
        [HttpPost]
        public ResultMessage PostAddDistrict([FromBody] JObject data)
        {
            try
            {
                StateMasterTO addDistrictTO = JsonConvert.DeserializeObject<StateMasterTO>(data["distTO"].ToString());

                int result = _iDimDistrictBL.InsertDimDistrict(addDistrictTO);
                {
                    ResultMessage resultMessage = new StaticStuff.ResultMessage();
                    if (result != 1)
                    {

                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error In Method SaveNewSuperwisor";
                        resultMessage.DisplayMessage = "Error... Record could not be saved";
                        return resultMessage;
                    }

                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Success... Record saved";
                        resultMessage.DisplayMessage = "Success... Record saved";
                        return resultMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Hrishikesh[27 - 03 - 2018] Added to edit district 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 


        [Route("PostEditDistrict")]
        [HttpPost]
        public ResultMessage PostEditDistrict([FromBody] JObject data)
        {
            try
            {
                StateMasterTO editDistrictTO = JsonConvert.DeserializeObject<StateMasterTO>(data["distTO"].ToString());

                int result = _iDimDistrictBL.UpdateDimDistrict(editDistrictTO);
                {
                    ResultMessage resultMessage = new StaticStuff.ResultMessage();
                    if (result != 1)
                    {

                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error In Method SaveNewSuperwisor";
                        resultMessage.DisplayMessage = "Error... Record could not be saved";
                        return resultMessage;
                    }

                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Success... Record saved";
                        resultMessage.DisplayMessage = "Success... Record saved";
                        return resultMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //Hrushikesh Added Add Taluka 
        [Route("PostAddTaluka")]
        [HttpPost]
        public ResultMessage PostAddTaluka([FromBody] JObject data)
        {
            try
            {
                StateMasterTO addDistrictTO = JsonConvert.DeserializeObject<StateMasterTO>(data["distTO"].ToString());

                int result = _iDimTalukaBL.InsertDimTaluka(addDistrictTO);
                {
                    ResultMessage resultMessage = new StaticStuff.ResultMessage();
                    if (result != 1)
                    {

                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error In Method SaveNewSuperwisor";
                        resultMessage.DisplayMessage = "Error... Record could not be saved";
                        return resultMessage;
                    }

                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Success... Record saved";
                        resultMessage.DisplayMessage = "Success... Record saved";
                        return resultMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Hrishikesh[27 - 03 - 2018] Added to edit taluka
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostEditTaluka")]

        [HttpPost]
        public ResultMessage PostEditTaluka([FromBody] JObject data)
        {
            try
            {
                StateMasterTO edittalTO = JsonConvert.DeserializeObject<StateMasterTO>(data["distTO"].ToString());

                int result = _iDimTalukaBL.UpdateDimTaluka(edittalTO);
                {
                    ResultMessage resultMessage = new StaticStuff.ResultMessage();
                    if (result != 1)
                    {

                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error In Method SaveNewSuperwisor";
                        resultMessage.DisplayMessage = "Error... Record could not be saved";
                        return resultMessage;
                    }

                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Success... Record saved";
                        resultMessage.DisplayMessage = "Success... Record saved";
                        return resultMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        [Route("PostNewSuperwisorMaster")]
        [HttpPost]
        public ResultMessage PostNewSuperwisorMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblSupervisorTO supervisorTO = JsonConvert.DeserializeObject<TblSupervisorTO>(data["supervisorTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (supervisorTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "supervisorTO Found NULL";
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

                supervisorTO.CreatedBy = Convert.ToInt32(loginUserId);
                supervisorTO.CreatedOn = _iCommon.ServerDateTime;
                supervisorTO.IsActive = 1;
                return _iTblSupervisorBL.SaveNewSuperwisor(supervisorTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostNewSuperwisorMaster";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        [Route("PostUpdateSuperwisorMaster")]
        [HttpPost]
        public ResultMessage PostUpdateSuperwisorMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblSupervisorTO supervisorTO = JsonConvert.DeserializeObject<TblSupervisorTO>(data["supervisorTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (supervisorTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "supervisorTO Found NULL";
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

                return _iTblSupervisorBL.UpdateSuperwisor(supervisorTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostUpdateSuperwisorMaster";
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


        //Vijaymala[08-09-2017] Added To Insert Designation Master
        [Route("PostNewDesignationMaster")]
        [HttpPost]
        public ResultMessage PostNewDesignationMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                DimMstDesignationTO mstDesignationTO = JsonConvert.DeserializeObject<DimMstDesignationTO>(data["mstDesignationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (mstDesignationTO == null)
                {
                    resultMessage.DefaultBehaviour("mstDesignationTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                mstDesignationTO.CreatedBy = Convert.ToInt32(loginUserId);
                mstDesignationTO.CreatedOn = _iCommon.ServerDateTime;
                mstDesignationTO.IsVisible = 1;
                int result = _iDimMstDesignationBL.InsertDimMstDesignation(mstDesignationTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewDesignationMaster");
                return resultMessage;
            }
        }



        //Vijaymala[08-09-2017] Added To Update Designation Master
        [Route("PostUpdateDesignationMaster")]
        [HttpPost]
        public ResultMessage PostUpdateDesignationMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                DimMstDesignationTO mstDesignationTO = JsonConvert.DeserializeObject<DimMstDesignationTO>(data["mstDesignationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (mstDesignationTO == null)
                {
                    resultMessage.DefaultBehaviour("mstDesignationTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }
                mstDesignationTO.UpdatedOn = _iCommon.ServerDateTime;
                mstDesignationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                int result = _iDimMstDesignationBL.UpdateDimMstDesignation(mstDesignationTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateDesignationMaster");
                return resultMessage;
            }
        }

        //Vijaymala[08-09-2017] Added To Deactivate Designation 
        [Route("PostDeactivateDesignation")]
        [HttpPost]
        public ResultMessage PostDeactivateDesignation([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                DimMstDesignationTO mstDesignationTO = JsonConvert.DeserializeObject<DimMstDesignationTO>(data["mstDesignationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (mstDesignationTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    mstDesignationTO.DeactivatedBy = Convert.ToInt32(loginUserId);
                    mstDesignationTO.DeactivatedOn = _iCommon.ServerDateTime;
                    //  organizationTO.DeactivatedOn = serverDate;
                    mstDesignationTO.IsVisible = 0;

                    int result = _iDimMstDesignationBL.UpdateDimMstDesignation(mstDesignationTO);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour("mstDesignationTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDesignation");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [16-Sep-2017] Added to insert new department. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostNewDepartmentMaster")]
        [HttpPost]
        public ResultMessage PostNewDepartmentMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                DimMstDeptTO dimMstDeptTO = JsonConvert.DeserializeObject<DimMstDeptTO>(data["mstDepartmentTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimMstDeptTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                // check parent department id not null
                if (dimMstDeptTO.ParentDeptId <= 0)
                {
                    resultMessage.DefaultBehaviour("ParentDeptId found null");
                    return resultMessage;
                }

                return _iDimMstDeptBL.SaveDepartment(dimMstDeptTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewDepartment");
                return resultMessage;
            }
        }


        /// <summary>
        /// Vaibhav [16-Sep-2017] Added to update department master
        /// </summary>
        /// <param name="id"></param>
        [Route("PostUpdateDepartmentMaster")]
        [HttpPost]
        public ResultMessage PostUpdateDepartmentMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DimMstDeptTO dimMstDeptTO = JsonConvert.DeserializeObject<DimMstDeptTO>(data["mstDepartmentTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimMstDeptTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                // check parent department id not null
                if (dimMstDeptTO.ParentDeptId <= 0)
                {
                    resultMessage.DefaultBehaviour("ParentDeptId found null");
                    return resultMessage;
                }

                return _iDimMstDeptBL.UpdateDepartment(dimMstDeptTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDepartment");
                return resultMessage;
            }
        }

        /// <summary>
        /// [05-12-2017] Vijaymala:Added to save stock configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// 
        [Route("PostStockConfiguration")]
        [HttpPost]
        public ResultMessage PostStockConfiguration([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblStockConfigTO tblStockConfigTO = JsonConvert.DeserializeObject<TblStockConfigTO>(data["stockConfigurationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblStockConfigTO == null)
                {
                    resultMessage.DefaultBehaviour("tblStockConfigTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                tblStockConfigTO.IsItemizedStock = 1;

                int result = _iTblStockConfigBL.InsertTblStockConfig(tblStockConfigTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostStockConfiguration");
                return resultMessage;
            }
        }

        /// <summary>
        /// [09-01-2018] Kiran:Added to save email configration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// 
        [Route("PostEmailConfigurationDetails")]
        [HttpPost]
        public ResultMessage PostEmailConfigurationDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblEmailConfigrationTO dimEmailConfigrationTO = JsonConvert.DeserializeObject<TblEmailConfigrationTO>(data["EmailConfigrationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimEmailConfigrationTO == null)
                {
                    resultMessage.DefaultBehaviour("dimEmailConfigrationTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                dimEmailConfigrationTO.IsActive = 0;
                int result = _iTblEmailConfigrationBL.InsertDimEmailConfigration(dimEmailConfigrationTO);
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
        /// Kiran [09-01-2018] Added to update email configration
        /// </summary>
        /// <param name="id"></param>
        [Route("PostUpdateEmailConfigurationDetails")]
        [HttpPost]
        public ResultMessage PostUpdateEmailConfigurationDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblEmailConfigrationTO dimEmailConfigrationTO = JsonConvert.DeserializeObject<TblEmailConfigrationTO>(data["EmailConfigrationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimEmailConfigrationTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                int result = _iTblEmailConfigrationBL.UpdateDimEmailConfigration(dimEmailConfigrationTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDepartment");
                return resultMessage;
            }
        }
        //Vijaymala[08-09-2017] Added To Update Designation Master
        [Route("PostUpdateStockConfiguration")]
        [HttpPost]
        public ResultMessage PostUpdateStockConfiguration([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblStockConfigTO tblStockConfigTO = JsonConvert.DeserializeObject<TblStockConfigTO>(data["stockConfigurationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblStockConfigTO == null)
                {
                    resultMessage.DefaultBehaviour("tblStockConfigTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                int result = _iTblStockConfigBL.UpdateTblStockConfig(tblStockConfigTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateStockConfiguration");
                return resultMessage;
            }
        }

        [Route("PostDeactivateStockConfiguration")]
        [HttpPost]
        public ResultMessage PostDeactivateStockConfiguration([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblStockConfigTO tblStockConfigTO = JsonConvert.DeserializeObject<TblStockConfigTO>(data["stockConfigurationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblStockConfigTO == null)
                {
                    resultMessage.DefaultBehaviour("tblStockConfigTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                resultMessage = _iTblStockConfigBL.DeactivateTblStockConfig(tblStockConfigTO);
                //if (result != 1)
                //{
                //    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                //    return resultMessage;
                //}
                // resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateStockConfiguration");
                return resultMessage;
            }
        }

        /// <summary>
        /// Sudhir[09-12-2017]Added for Adding New State.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostNewState")]
        [HttpPost]
        public ResultMessage PostNewStatetMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DimStateTO dimStateTO = JsonConvert.DeserializeObject<DimStateTO>(data["stateTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimStateTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }


                return _iDimStateBL.SaveNewState(dimStateTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDepartment");
                return resultMessage;
            }
        }

        /// <summary>
        /// Sudhir[09-12-2017]Added for Updating State.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostUpdateState")]
        [HttpPost]
        public ResultMessage PostUpdateState([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DimStateTO dimStateTO = JsonConvert.DeserializeObject<DimStateTO>(data["stateTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimStateTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                return _iDimStateBL.UpdateState(dimStateTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDepartment");
                return resultMessage;
            }
        }


        //Sudhir[11-12-2017] Added To Delete State.
        [Route("PostDeleteState")]
        [HttpPost]
        public ResultMessage PostDeleteState([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                DimStateTO dimStateTO = JsonConvert.DeserializeObject<DimStateTO>(data["stateTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimStateTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                result = _iDimStateBL.DeleteDimState(dimStateTO.IdState);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeleteState");
                return resultMessage;
            }
        }


        [Route("PostDimBrand")]
        [HttpPost]
        public ResultMessage PostDimBrand([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                DimBrandTO dimBrandTO = JsonConvert.DeserializeObject<DimBrandTO>(data["brandTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (dimBrandTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "brandTO Found NULL";
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


                List<DimBrandTO> dimBrandTOList = _iDimBrandBL.SelectAllDimBrandList(dimBrandTO);
                if (dimBrandTOList != null && dimBrandTOList.Count > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Brand Name Already Exist";
                    resultMessage.Result = 0;
                    return resultMessage;
                }


                //dimBrandTO.CreatedBy = Convert.ToInt32(loginUserId);
                dimBrandTO.CreatedOn = _iCommon.ServerDateTime;

                Int32 result = 0;

                if (dimBrandTO.IdBrand > 0)
                {
                    result = _iDimBrandBL.UpdateDimBrand(dimBrandTO);
                    if (result == 1)
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Success... Brand Updated";
                        resultMessage.DisplayMessage = "Success... Brand Updated";
                        return resultMessage;
                    }
                }
                else
                {
                    dimBrandTO.IsActive = 1;
                    result = _iDimBrandBL.InsertDimBrand(dimBrandTO);
                    if (result == 1)
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Success... Brand Saved";
                        resultMessage.DisplayMessage = "Success... Brand Saved";
                        return resultMessage;
                    }
                }

                return resultMessage;
            }

            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostDimBrand";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        /// <summary>
        /// [18-01-2018] Vijaymala:Added to save,update,deactivate group master
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostGroupMaster")]
        [HttpPost]
        public ResultMessage PostGroupMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblGroupTO tblGroupTO = JsonConvert.DeserializeObject<TblGroupTO>(data["groupTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblGroupTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "groupTO Found NULL";
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


                List<TblGroupTO> tblGroupTOList = _iTblGroupBL.SelectAllGroupList(tblGroupTO);
                if (tblGroupTOList != null && tblGroupTOList.Count > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Group Name Already Exist";
                    resultMessage.DisplayMessage = "Group Name Already Exist";
                    resultMessage.Result = 0;
                    return resultMessage;
                }



                Int32 result = 0;

                if (tblGroupTO.IdGroup > 0)
                {
                    tblGroupTO.UpdatedOn = _iCommon.ServerDateTime;
                    tblGroupTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    return _iTblGroupBL.UpdateTblGroup(tblGroupTO);
                }
                else
                {
                    tblGroupTO.CreatedOn = _iCommon.ServerDateTime;
                    tblGroupTO.CreatedBy = Convert.ToInt32(loginUserId);
                    result = _iTblGroupBL.InsertTblGroup(tblGroupTO);
                    if (result != 1)
                    {

                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While InsertTblGroup";
                        resultMessage.DisplayMessage = "Record Cound Not Saved";
                        return resultMessage;

                    }
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Success... Group Saved";
                    resultMessage.DisplayMessage = "Success... Group Saved";


                }

                return resultMessage;
            }

            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostGroupMaster";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        //Vinod [08-09-2017] Added To Deactivate Product Specification Description Master 
        [Route("AddProdSpecDesc")]
        [HttpPost]
        public ResultMessage AddProdSpecDesc([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                DimProdSpecDescTO tblProdSpecDescTO = JsonConvert.DeserializeObject<DimProdSpecDescTO>(data["ProductSpecTO"].ToString());
                // DimProdSpecDescTO mstProdSpecDescTO = JsonConvert.DeserializeObject<DimProdSpecDescTO>(data["ProductSpecTO"].ToString()); //ProductSpecTO
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblProdSpecDescTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    tblProdSpecDescTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblProdSpecDescTO.UpdatedOn = _iCommon.ServerDateTime;
                    //  organizationTO.DeactivatedOn = serverDate;
                    tblProdSpecDescTO.IsVisible = 0;

                    int ProdSpecDescId = _iDimProdSpecDescBL.SelectAllDimProdSpecDescriptionList();
                    tblProdSpecDescTO.IdProductSpecDesc = ProdSpecDescId;
                    int result = _iDimProdSpecDescBL.InsertDimProdSpecDesc(tblProdSpecDescTO);

                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be Inserted");
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.DefaultBehaviour("tblProdSpecDescTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AddProdSpecDesc");
                return resultMessage;
            }
        }

        //Vinod [08-09-2017] Update To Product Specification Description Master 
        [Route("UpdateProdSpecDesc")]
        [HttpPost]
        public ResultMessage UpdateProdSpecDesc([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                DimProdSpecDescTO tblProdSpecDescTO = JsonConvert.DeserializeObject<DimProdSpecDescTO>(data["ProductSpecTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblProdSpecDescTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    tblProdSpecDescTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblProdSpecDescTO.UpdatedOn = _iCommon.ServerDateTime;
                    tblProdSpecDescTO.IsVisible = 0;
                    int result = _iDimProdSpecDescBL.UpdateDimProSpecDesc(tblProdSpecDescTO);

                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be Updated");
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.DefaultBehaviour("tblProdSpecDescTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProdSpecDesc");
                return resultMessage;
            }
        }


        //Vinod [08-09-2017] Added To Deactivate Product Specification Description Master 
        [Route("PostDeactivateProdSpecDesc")]
        [HttpPost]
        public ResultMessage PostDeactivateProdSpecDesc([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                DimProdSpecDescTO mstProdSpecDescTO = JsonConvert.DeserializeObject<DimProdSpecDescTO>(data["ProductSpecTO"].ToString()); //ProductSpecTO
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (mstProdSpecDescTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    mstProdSpecDescTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    mstProdSpecDescTO.UpdatedOn = _iCommon.ServerDateTime;
                    //  organizationTO.DeactivatedOn = serverDate;
                    mstProdSpecDescTO.IsVisible = 0;

                    int result = _iDimProdSpecDescBL.DeleteDimProSpecDesc(mstProdSpecDescTO);

                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.DefaultBehaviour("mstProdSpecDescTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateProdSpecDesc");
                return resultMessage;
            }
        }

        /// <summary>
        /// [18-01-2018] Vijaymala:Added to PostItemTallyRefMaster
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostItemTallyRefMaster")]
        [HttpPost]
        public ResultMessage PostItemTallyRefMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO = JsonConvert.DeserializeObject<TblItemTallyRefDtlsTO>(data["itemTallyRefDtlsTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblItemTallyRefDtlsTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblItemTallyRefDtlsTO Found NULL";
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


                if (tblItemTallyRefDtlsTO != null)
                {
                    //if (!String.IsNullOrEmpty(tblItemTallyRefDtlsTO.OverdueTallyRefId))
                    //{
                    //    List<TblItemTallyRefDtlsTO> tblItemTallyRefDtlsTOList = _iTblItemTallyRefDtlsBL.SelectExistingAllTblOrganizationByRefIds(tblItemTallyRefDtlsTO.OverdueTallyRefId, null);
                    //    if (tblItemTallyRefDtlsTOList != null)
                    //    {
                    //        Int32 overdueisExist = tblItemTallyRefDtlsTOList.Count;
                    //        if (overdueisExist > 0)
                    //        {
                    //            // itemName = String.Join(",", tblItemTallyRefDtlsTOList.Select(s => s.itemName.ToString()).ToList());

                    //            resultMessage.MessageType = ResultMessageE.Information;
                    //            resultMessage.Result = 2;
                    //            resultMessage.Text = "Overdue Reference Id is already assign  ";// + orgName;
                    //            return resultMessage;
                    //        }
                    //    }
                    //}
                    //if (!String.IsNullOrEmpty(tblItemTallyRefDtlsTO.EnquiryTallyRefId))
                    //{
                    //    List<TblItemTallyRefDtlsTO> tblItemTallyRefDtlsTOList = _iTblItemTallyRefDtlsBL.SelectExistingAllTblOrganizationByRefIds( null, tblItemTallyRefDtlsTO.EnquiryTallyRefId);
                    //    if (tblItemTallyRefDtlsTOList != null)
                    //    {
                    //        Int32 enqyisExist = tblItemTallyRefDtlsTOList.Count;
                    //        if (enqyisExist > 0)
                    //        {
                    //          //  String itemName = String.Join(",", tblItemTallyRefDtlsTOList.Select(s => s.itemName.ToString()).ToList());

                    //            resultMessage.MessageType = ResultMessageE.Information;
                    //            resultMessage.Result = 2;
                    //            resultMessage.Text = "Enquiry Reference Id is already assign  ";// + orgName;
                    //            return resultMessage;
                    //        }
                    //    }
                    //}

                    if (tblItemTallyRefDtlsTO.IdItemTallyRef > 0)
                    {
                        tblItemTallyRefDtlsTO.UpdatedOn = _iCommon.ServerDateTime;
                        tblItemTallyRefDtlsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                        return _iTblItemTallyRefDtlsBL.UpdateItemTallyRef(tblItemTallyRefDtlsTO);
                    }

                    else
                    {
                        //Check Already exist
                        TblItemTallyRefDtlsTO tblNewItemTallyRefDtlsTO = _iTblItemTallyRefDtlsBL.SelectExistingAllTblItemRef(tblItemTallyRefDtlsTO);
                        if (tblNewItemTallyRefDtlsTO != null)
                        {
                            tblItemTallyRefDtlsTO.UpdatedOn = _iCommon.ServerDateTime;
                            tblItemTallyRefDtlsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                            return _iTblItemTallyRefDtlsBL.UpdateItemTallyRef(tblItemTallyRefDtlsTO);
                        }
                        else
                        {
                            tblItemTallyRefDtlsTO.CreatedOn = _iCommon.ServerDateTime;
                            tblItemTallyRefDtlsTO.CreatedBy = Convert.ToInt32(loginUserId);
                            tblItemTallyRefDtlsTO.IsActive = 1;
                            return _iTblItemTallyRefDtlsBL.SaveNewItemTallyRef(tblItemTallyRefDtlsTO);

                        }
                    }
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "tblItemTallyRefDtlsTO Found NULL";
                    return resultMessage;
                }
            }

            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostItemTallyRefMaster";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }
        ///// <summary>
        ///// Priyanka [03-04-2018]: Added to post the ITC details.
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>

        //[Route("PostTestCertItemValuesList")]
        //[HttpPost]
        //public ResultMessage PostTestCertItemValuesList([FromBody] JObject data)
        //{
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    try
        //    {

        //        List<TblTestCertItemValueTO> itcToList = JsonConvert.DeserializeObject<List<TblTestCertItemValueTO>>(data["itcToList"].ToString());

        //        var loginUserId = data["loginUserId"].ToString();

        //        if (Convert.ToInt32(loginUserId) <= 0)
        //        {
        //            resultMessage.DefaultBehaviour("loginUserId Found NULL");
        //            return resultMessage;
        //        }

        //        if (itcToList != null && itcToList.Count > 0)
        //        {              
        //            return BL.TblTestCertItemValueBL.SaveITCDetails(itcToList,Convert.ToInt32(loginUserId));
        //        }
        //        else
        //        {
        //            resultMessage.DefaultBehaviour("itcToList Found NULL");
        //            return resultMessage;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "PostTestCertItemValuesList");
        //        return resultMessage;
        //    }
        //}


        // POST api/values
        [Route("PostRolesAndOrgDetails")]
        [HttpPost]
        public ResultMessage PostRolesAndOrgDetails([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<RoleOrgTO> roleOrgTOList = JsonConvert.DeserializeObject<List<RoleOrgTO>>(data["roleOrgTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (roleOrgTOList == null || roleOrgTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : roleOrgTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = _iCommon.ServerDateTime;
                for (int i = 0; i < roleOrgTOList.Count; i++)
                {
                    roleOrgTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    roleOrgTOList[i].CreatedOn = confirmedDate;
                }

                ResultMessage resMsg = _iTblRoleOrgSettingBL.SaveRolesAndOrg(roleOrgTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostRolesAndOrgDetails";
                return returnMsg;
            }
        }

        //SaveTaskModuleDetails
        /// <summary>
        /// Sudhir [07-05-2018] Added to update email configration
        /// </summary>
        /// <param name="id"></param>
        [Route("SaveTaskModuleDetails")]
        [HttpPost]
        public ResultMessage SaveTaskModuleDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblTaskModuleExtTO tblTaskModuleExtTO = JsonConvert.DeserializeObject<TblTaskModuleExtTO>(data["tblTaskModuleExtTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblTaskModuleExtTO == null)
                {
                    resultMessage.DefaultBehaviour("tblTaskModuleExtTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                tblTaskModuleExtTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblTaskModuleExtTO.CreatedOn = _iCommon.ServerDateTime;
                int result = _iTblTaskModuleExtBL.InsertTblTaskModuleExt(tblTaskModuleExtTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "SaveTaskModuleDetails");
                return resultMessage;
            }
        }



        /// <summary>
        /// Priyanka [10-08-2018] : Added to Post The transaction details.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostTransActionType")]
        [HttpPost]
        public ResultMessage PostTransActionType([FromBody] JObject data)
        {
            try
            {
                ResultMessage resultMessage = new StaticStuff.ResultMessage();

                TblTranActionsTO tranActionsTO = JsonConvert.DeserializeObject<TblTranActionsTO>(data["tranActionsTO"].ToString());
                DateTime dateTime = _iCommon.ServerDateTime;
                var loginUserId = data["loginUserId"].ToString();
                tranActionsTO.CreatedOn = dateTime;
                tranActionsTO.CreatedBy = Convert.ToInt32(loginUserId);

                List<TblTranActionsTO> exitingtranActionsTOList = _iTblTranActionsBL.SelectAllTblTranActionsList(tranActionsTO);
                if (exitingtranActionsTOList != null && exitingtranActionsTOList.Count > 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }



                int result = _iTblTranActionsBL.InsertTblTranActions(tranActionsTO);
                {
                    if (result != 1)
                    {

                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error In Method InsertTblTranActions";
                        resultMessage.DisplayMessage = "Error... Record could not be saved";
                        return resultMessage;
                    }

                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Success... Record saved";
                        resultMessage.DisplayMessage = "Success... Record saved";
                        return resultMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Sudhir [30-08-2018] Added to Save Tasks Without Subscription.
        /// </summary>
        /// <param name="id"></param>
        [Route("SaveTasksWithoutSubscription")]
        [HttpPost]
        public ResultMessage SaveTasksWithoutSubscription([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TbltaskWithoutSubscTO tbltaskWithoutSubscTO = JsonConvert.DeserializeObject<TbltaskWithoutSubscTO>(data["tbltaskWithoutSubscTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tbltaskWithoutSubscTO == null)
                {
                    resultMessage.DefaultBehaviour("tbltaskWithoutSubscTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                tbltaskWithoutSubscTO.CreatedOn = _iCommon.ServerDateTime;
                tbltaskWithoutSubscTO.CreatedBy = Convert.ToInt32(loginUserId);
                int result = _iTbltaskWithoutSubscBL.InsertTbltaskWithoutSubsc(tbltaskWithoutSubscTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "SaveTaskModuleDetails");
                return resultMessage;
            }
        }

        #endregion

        #region PUT

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion



    }
}
