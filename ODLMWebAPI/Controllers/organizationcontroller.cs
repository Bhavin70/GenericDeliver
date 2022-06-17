using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class OrganizationController : Controller
    {

        private readonly ILogger loggerObj;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ITblAddressBL _iTblAddressBL;
        private readonly ITblOrgLicenseDtlBL _iTblOrgLicenseDtlBL;
        private readonly ITblCompetitorExtBL _iTblCompetitorExtBL;
        private readonly ITblPurchaseCompetitorExtBL _iTblPurchaseCompetitorExtBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblOtherSourceBL _iTblOtherSourceBL;
        private readonly ITblCnfDealersBL _iTblCnfDealersBL;
        private readonly ITblInvoiceOtherDetailsBL _iTblInvoiceOtherDetailsBL;
        private readonly ITblInvoiceBankDetailsBL _iTblInvoiceBankDetailsBL;
        private readonly ITblEnquiryDtlBL _iTblEnquiryDtlBL;
        private readonly ITblOverdueDtlBL _iTblOverdueDtlBL;
        private readonly ITblKYCDetailsBL _iTblKYCDetailsBL;
        private readonly ICommon _iCommon;
        public OrganizationController(ICommon iCommon, ITblKYCDetailsBL iTblKYCDetailsBL, ITblOverdueDtlBL iTblOverdueDtlBL, ITblEnquiryDtlBL iTblEnquiryDtlBL, ITblInvoiceBankDetailsBL iTblInvoiceBankDetailsBL, ITblInvoiceOtherDetailsBL iTblInvoiceOtherDetailsBL, ITblCnfDealersBL iTblCnfDealersBL, ITblOtherSourceBL iTblOtherSourceBL, ITblConfigParamsBL iTblConfigParamsBL, ITblPurchaseCompetitorExtBL iTblPurchaseCompetitorExtBL, ITblCompetitorExtBL iTblCompetitorExtBL, ITblOrgLicenseDtlBL iTblOrgLicenseDtlBL, ITblAddressBL iTblAddressBL, ITblPersonBL iTblPersonBL, ITblOrganizationBL iTblOrganizationBL, ILogger<OrganizationController> logger)
        {
            loggerObj = logger;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iTblKYCDetailsBL = iTblKYCDetailsBL;
            _iTblPersonBL = iTblPersonBL;
            _iTblAddressBL = iTblAddressBL;
            _iTblOrgLicenseDtlBL = iTblOrgLicenseDtlBL;
            _iTblCompetitorExtBL = iTblCompetitorExtBL;
            _iTblPurchaseCompetitorExtBL = iTblPurchaseCompetitorExtBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblOtherSourceBL = iTblOtherSourceBL;
            _iTblCnfDealersBL = iTblCnfDealersBL;
            _iTblInvoiceOtherDetailsBL = iTblInvoiceOtherDetailsBL;
            _iTblInvoiceBankDetailsBL = iTblInvoiceBankDetailsBL;
            _iTblEnquiryDtlBL = iTblEnquiryDtlBL;
            _iTblOverdueDtlBL = iTblOverdueDtlBL;
            _iCommon = iCommon;
            Constants.LoggerObj = logger;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetOrganizationList")]
        [HttpGet]
        public List<TblOrganizationTO> GetOrganizationList(Int32 orgTypeId)
        {
            Constants.OrgTypeE orgTypeE = (Constants.OrgTypeE)Enum.Parse(typeof(Constants.OrgTypeE), orgTypeId.ToString());
            List<TblOrganizationTO> list = _iTblOrganizationBL.SelectAllTblOrganizationList(orgTypeE).OrderBy(o => o.FirmName).ToList(); 
            return list;
        }

        [Route("GetOrganizationInfo")]
        [HttpGet]
        public TblOrganizationTO GetOrganizationInfo(Int32 orgId)
        {
            return _iTblOrganizationBL.SelectTblOrganizationTO(orgId);
        }

        [Route("GetDealerOrganizationList")]
        [HttpGet]
        public List<TblOrganizationTO> GetDealerOrganizationList(Int32 cnfId)
        {
            int orgTypeId = (int)Constants.OrgTypeE.DEALER;
            List<TblOrganizationTO> list = _iTblOrganizationBL.SelectAllChildOrganizationList(orgTypeId, cnfId);
            return list;
        }

        [Route("GetOrgOwnerDetails")]
        [HttpGet]
        public List<TblPersonTO> GetOrgOwnerDetails(Int32 organizationId)
        {
            List<TblPersonTO> list = _iTblPersonBL.SelectAllPersonListByOrganization(organizationId);
            return list;
        }
        //Aniket [13-03-2019] added for get Organization details to send Email
        [Route("GetOrgOwnerDetailsForEmail")]
        [HttpGet]
        public List<TblPersonTOEmail> GetOrgOwnerDetailsForEmail(Int32 organizationId)
        {
           return _iTblPersonBL.SelectAllPersonListByOrganizationForEmail(organizationId);
        }
        [Route("GetOrgAddressDetails")]
        [HttpGet]
        public List<TblAddressTO> GetOrgAddressDetails(Int32 organizationId)
        {
            return _iTblAddressBL.SelectOrgAddressList(organizationId);
        }

        [Route("GetOrgCommercialLicenseDetails")]
        [HttpGet]
        public List<TblOrgLicenseDtlTO> GetOrgCommercialLicenseDetails(Int32 organizationId)
        {
            return _iTblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(organizationId);
        }

        [Route("GetCompetitorBrandList")]
        [HttpGet]
        public List<TblCompetitorExtTO> GetCompetitorBrandList(Int32 organizationId)
        {
            return _iTblCompetitorExtBL.SelectAllTblCompetitorExtList(organizationId);
        }

        /// <summary>
        /// Priyanka [16-02-18] : Added to get Purchase Competitor Material & Grade Details
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns>List of Material and Grade for given Purchase Competitor</returns>
        [Route("GetPurchaseCompetitorMaterialList")]
        [HttpGet]
        public List<TblPurchaseCompetitorExtTO> GetPurchaseCompetitorMaterialList(Int32 organizationId)
        {
            return _iTblPurchaseCompetitorExtBL.SelectAllTblPurchaseCompetitorExtList(organizationId);
        }

        [Route("GetCompetitorListWithHistory")]
        [HttpGet]
        public List<TblOrganizationTO> GetCompetitorListWithHistory()
        {
            Constants.OrgTypeE orgTypeE = Constants.OrgTypeE.COMPETITOR;
            List<TblOrganizationTO> list = _iTblOrganizationBL.SelectAllTblOrganizationList(orgTypeE);

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_COMPETITOR_TO_SHOW_IN_HISTORY);
            if (tblConfigParamsTO == null)
                return list;
            else
            {
                String csParamVal = tblConfigParamsTO.ConfigParamVal;
                if (csParamVal == "0")
                    return list;
                else
                {
                    List<TblOrganizationTO> finalList = new List<TblOrganizationTO>();
                    string[] idsToShow = csParamVal.Split(',');

                    for (int i = 0; i < idsToShow.Length; i++)
                    {
                        if (list != null)
                        {
                            var orgTO = list.Where(a => a.IdOrganization == Convert.ToInt32(idsToShow[i])).LastOrDefault();
                            if (orgTO != null)
                                finalList.Add(orgTO);
                        }
                    }

                    finalList = finalList.OrderByDescending(o => o.CompetitorUpdatesTO.UpdateDatetime).ToList();
                    return finalList;
                }
            }
        }

        [Route("GetOrganizationDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetOrganizationDropDownList(Int32 orgTypeId,String userRoleTOList)
        {
            Constants.OrgTypeE orgTypeE = (Constants.OrgTypeE)Enum.Parse(typeof(Constants.OrgTypeE), orgTypeId.ToString());
            if (orgTypeE == Constants.OrgTypeE.OTHER)
            {
                List<DropDownTO> list = _iTblOtherSourceBL.SelectOtherSourceOfMarketTrendForDropDown();
                return list;
            }
            else
            {
                List<TblUserRoleTO> tblUserRoleTOList = new List<TblUserRoleTO>();
                if (!string.IsNullOrEmpty(userRoleTOList))
                    tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

                //TblUserRoleTO tblUserRoleTO = null;
                //if(!string.IsNullOrEmpty(userRoleTO))
                //    tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);

                List<DropDownTO> list = _iTblOrganizationBL.SelectAllOrganizationListForDropDown(orgTypeE, tblUserRoleTOList).OrderBy(o => o.Text).ToList();
                return list;
            }
        }

        //Priyanka [10-09-2018] : Added to get the organization list for RM.
        [Route("GetOrganizationDropDownListForRM")]
        [HttpGet]
        public List<DropDownTO> GetOrganizationDropDownListForRM(Int32 orgTypeId, Int32 RMId, String userRoleTOList)
        {
            Constants.OrgTypeE orgTypeE = (Constants.OrgTypeE)Enum.Parse(typeof(Constants.OrgTypeE), orgTypeId.ToString());
                List<TblUserRoleTO> tblUserRoleTOList = null;
                if (!string.IsNullOrEmpty(userRoleTOList))
                tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

                List<DropDownTO> list = _iTblOrganizationBL.SelectAllOrganizationListForDropDownForRM(orgTypeE, RMId, tblUserRoleTOList).OrderBy(o => o.Text).ToList();
                return list;
           
        }

        [Route("GetDealerDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetDealerDropDownList(Int32 cnfId, String userRoleTOList, Int32 consumerType = 0)
       {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            //List<TblUserRoleTO> tblUserRoleTOList = new List<TblUserRoleTO>();
            if (!string.IsNullOrEmpty(userRoleTOList))
                tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            List<DropDownTO> list = _iTblOrganizationBL.SelectDealerListForDropDown(cnfId, tblUserRoleTOList, consumerType).OrderBy(o => o.Text).ToList(); 
            return list;
        }

        [Route("GetSpecialCnfDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetSpecialCnfDropDownList(String userRoleTOList)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            List<DropDownTO> list = _iTblOrganizationBL.SelectAllSpecialCnfListForDropDown(tblUserRoleTOList).OrderBy(o => o.Text).ToList();
            return list;
        }

        [Route("GetDealersSpecialCnfList")]
        [HttpGet]
        public List<TblCnfDealersTO> GetDealersSpecialCnfList(Int32 dealerId)
        {
            return  _iTblCnfDealersBL.SelectAllActiveCnfDealersList(dealerId,true);
        }
        [Route("GetDealersCnfList")]
        [HttpGet]
        public List<TblCnfDealersTO> GetDealersCnfList(Int32 dealerId)
        {
            return _iTblCnfDealersBL.SelectAllActiveCnfDealersList(dealerId, false);
        }
        [Route("GetActiveDealersCnfList")]
        [HttpGet]
        public List<DropDownTO> SelectActiveCnfDealersList(Int32 dealerId)
        {
            return _iTblCnfDealersBL.SelectActiveCnfDealersList(dealerId, false);
        }
        [Route("GetDealerForLoadingDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId)
        {
            List<DropDownTO> list = _iTblOrganizationBL.GetDealerForLoadingDropDownList(cnfId).OrderBy(o => o.Text).ToList();
            return list;
        }

        [Route("IsThisValidCommercialLicenses")]
        [HttpGet]
        public ResultMessage IsThisValidCommercialLicenses(Int32 orgId, Int32 licenseId, String licenseVal)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<TblOrgLicenseDtlTO> list = _iTblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(orgId, licenseId, licenseVal);
            if (list != null && list.Count > 0)
            {
                TblOrganizationTO orgTO = _iTblOrganizationBL.SelectTblOrganizationTO(list[0].OrganizationId);
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Not Allowed, This License is already attached to " + orgTO.OrgTypeE.ToString() + "-" + orgTO.FirmName;
                resultMessage.Result = 0;
            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Valid";
                resultMessage.Result = 1;
            }

            return resultMessage;
        }

        [Route("GetAllOrgListToExport")]
        [HttpGet]
        public List<OrgExportRptTO> GetAllOrgListToExport(Int32 orgTypeId, Int32 parentId)
        {
            List<OrgExportRptTO> list = _iTblOrganizationBL.SelectAllOrgListToExport(orgTypeId, parentId);
            return list;
        }
        /// <summary>
        /// Vijaymala[31-10-2017] Added to get invoice other details like desription wich display
        /// on footer to display terms an dconditions
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>

        [Route("GetInvoiceOtherDetails")]
        [HttpGet]
        public List<TblInvoiceOtherDetailsTO> GetInvoiceOtherDetails(Int32 organizationId)
        {
            List<TblInvoiceOtherDetailsTO> list = _iTblInvoiceOtherDetailsBL.SelectInvoiceOtherDetails(organizationId);
            return list;
        }

        [Route("GetInvoiceBankDetails")]
        [HttpGet]
        public List<TblInvoiceBankDetailsTO> GetInvoiceBankDetails(Int32 organizationId)
        {
            List<TblInvoiceBankDetailsTO> list = _iTblInvoiceBankDetailsBL.SelectInvoiceBankDetails(organizationId);
            return list;
        }

        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization list of particular region;
        /// </summary>
        /// <param name="orgTypeId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>       
        [Route("GetOrganizationListByRegion")]
        [HttpGet]
        public List<TblOrganizationTO> GetOrganizationListByRegion(Int32 orgTypeId, Int32 districtId)
        {
            List<TblOrganizationTO> list = _iTblOrganizationBL.SelectOrganizationListByRegion(orgTypeId, districtId);
            return list;
        }


        /// <summary>
        /// [2017-11-29]Vijaymala:Added to get enquiry detail of particular organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetOrgEnquiryList")]
        [HttpGet]
        public List<TblEnquiryDtlTO> GetOrgEnquiryList(Int32 organizationId)
        {
            return _iTblEnquiryDtlBL.SelectEnquiryDtlList(organizationId);
        }

        /// <summary>
        /// [2017-11-29]Vijaymala:Added to get overdue detail of particular organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetOrgOverdueList")]
        [HttpGet]
        public List<TblOverdueDtlTO> GetOrgOverdueList(Int32 organizationId)
        {
            return _iTblOverdueDtlBL.SelectTblOverdueDtlList(organizationId);
        }

        /// <summary>
        /// [2017-12-01]Vijaymala:Added to get enquiry detail List of organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllOrgEnquiryList")]
        [HttpGet]
        public List<TblEnquiryDtlTO> GetAllOrgEnquiryList(string organizationIds)
        {
            return _iTblEnquiryDtlBL.SelectAllTblEnquiryDtl(organizationIds);
        }

        /// <summary>
        /// [2017-12-01]Vijaymala:Added to get overdue detail List of particular organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllOrgOverdueList")]
        [HttpGet]
        public List<TblOverdueDtlTO> GetAllOrgOverdueList(string organizationIds)
        {
            return _iTblOverdueDtlBL.SelectAllTblOverdueDtlList(organizationIds);
        }


        /// <summary>
        /// [2017-11-29]Vijaymala:Added to get enquiry detail of particular organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetEnquiryListFromExcel")]
        [HttpGet]
        public List<TblEnquiryDtlTO> GetEnquiryListFromExcel(Int32 organizationId)
        {
            return _iTblEnquiryDtlBL.SelectEnquiryDtlList(organizationId);
        }




        /// <summary>
        /// Sudhir[20-March-2018] Added for Get Person List On OrganizationId also in tblOrgPersonDtls.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [Route("GetOrganizationPersonList")]
        [HttpGet]
        public List<TblPersonTO> GetOrganizationPersonList(Int32 organizationId, Int32 personTypeId = 0)
        {
            List<TblPersonTO> list = _iTblPersonBL.SelectAllPersonListByOrganizationV2(organizationId, personTypeId);
            return list;
        }
        [Route("GetAllPersonsOffline")]
        [HttpGet]
          public List<TblPersonTO> GetPersonsForOffline()
        {
            try
            {
                return _iTblPersonBL.selectPersonsForOffline();               
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("GetAllPersonsOfflineDropDown")]
        [HttpGet]
        public List<DropDownTO> GetPersonsForOfflineDropDown()
        {
            try
            {
                return _iTblPersonBL.selectPersonsDropdownForOffline();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Sudhir[23-APR-2018] Added for Select All OtherDesignations .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetPersonsOnOrgType")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetPersonsOnOrgType(Int32 OrgType)
        {
            try
            {
                List<DropDownTO> list = _iTblPersonBL.SelectPersonBasedOnOrgType(OrgType);//BL.TblOtherDesignationsBL.SelectAllTblOtherDesignations();
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
        /// Sudhir[23-APR-2018] Added for Checking Organization Name OR Phone No. is Already Present Or Not.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [Route("CheckOrgNameOrPhoneNoAlready")]
        [HttpGet]
        public ResultMessage CheckOrgNameOrPhoneNoAlready(String OrgName, String PhoneNo)
        {
            return _iTblOrganizationBL.CheckOrgNameOrPhoneNoIsExist(OrgName, PhoneNo);
        }


        /// <summary>
        /// Sudhir[26-July-2018] --Add this Method for District & field officer link to be establish. 
        ///                        Regional manger can see his field office visit list. 
        ///                        Also field office can see their own visits
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        [Route("GetDealerDropDownListForCRM")]
        [HttpGet]
        public List<DropDownTO> GetDealerDropDownListForCRM(Int32 cnfId, String userRoleTO)
        {
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
            List<DropDownTO> list = _iTblOrganizationBL.SelectDealerListForDropDownForCRM(cnfId, tblUserRoleTO).OrderBy(o => o.Text).ToList();
            return list;
        }

        //Priyanka [26-10-2018] : Added to get the KYC details of the particular organization.
        [Route("GetKYCDetails")]
        [HttpGet]
        public List<TblKYCDetailsTO> GetKYCDetails(Int32 organizationId)
        {
            return _iTblKYCDetailsBL.SelectTblKYCDetailsTOByOrgId(organizationId);
        }

        [Route("GetSalesEngineerDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetSalesEngineerDropDownList(Int32 orgId)
        {
            List<DropDownTO> list = _iTblOrganizationBL.SelectSalesEngineerListForDropDown(orgId);
            return list;
        }

        [Route("PostNewOrganization")]
        [HttpPost]
        public ResultMessage PostNewOrganization([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblOrganizationTO organizationTO = JsonConvert.DeserializeObject<TblOrganizationTO>(data["organizationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (organizationTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    organizationTO.CreatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.CreatedOn = serverDate;
                    organizationTO.IsActive = 1;
                    return _iTblOrganizationBL.SaveNewOrganization(organizationTO);

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "organizationTO Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostNewOrganization";
                return resultMessage;
            }
        }


        [Route("PostOrganizationRefIds")]
        [HttpPost]
        public ResultMessage PostOrganizationRefIds([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblOrganizationTO organizationTO = JsonConvert.DeserializeObject<TblOrganizationTO>(data["organizationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }
                
                if (organizationTO != null)
                {
                    return _iTblOrganizationBL.SaveOrganizationRefIds(organizationTO, loginUserId);
                 
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "organizationTO Found NULL";
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostNewOrganization";
                return resultMessage;
            }
        }

        [Route("PostUpdateOrganization")]
        [HttpPost]
        public ResultMessage PostUpdateOrganization([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblOrganizationTO organizationTO = JsonConvert.DeserializeObject<TblOrganizationTO>(data["organizationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (organizationTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    organizationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.UpdatedOn = serverDate;

                    return _iTblOrganizationBL.UpdateOrganization(organizationTO);

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "organizationTO Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostNewOrganization";
                return resultMessage;
            }
        }

        [Route("PostRemoveCnfDealerRelationShip")]
        [HttpPost]
        public ResultMessage PostRemoveCnfDealerRelationShip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblCnfDealersTO cnfDealersTO = JsonConvert.DeserializeObject<TblCnfDealersTO>(data["cnfDealersTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (cnfDealersTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    cnfDealersTO.IsActive = 0;

                    int result= _iTblCnfDealersBL.UpdateTblCnfDealers(cnfDealersTO);
                    if(result!=1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error. Record Could Not Be Updated ";
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Record Updated Successfully";
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "cnfDealersTO Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostRemoveCnfDealerRelationShip";
                return resultMessage;
            }
        }

        [Route("PostDeactivateOrganization")]
        [HttpPost]
        public ResultMessage PostDeactivateOrganization([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblOrganizationTO organizationTO = JsonConvert.DeserializeObject<TblOrganizationTO>(data["organizationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (organizationTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    organizationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.UpdatedOn = serverDate;
                    organizationTO.DeactivatedOn = serverDate;
                    organizationTO.IsActive = 0;

                    int result = _iTblOrganizationBL.UpdateTblOrganization(organizationTO);
                    if (result != 1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error. Record Could Not Be Updated ";
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Record Updated Successfully";
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "organizationTO Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostDeactivateOrganization";
                return resultMessage;
            }
        }

        /// <summary>
        /// [08/12/2017] Vijaymala :Added to save enquiry detail of organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostOrganizationEnquiryDtl")]
        [HttpPost]
        public ResultMessage PostOrganizationEnquiryDtl([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblEnquiryDtlTO> tblEnquiryDtlTOList = JsonConvert.DeserializeObject<List<TblEnquiryDtlTO>>(data["enquiryDtlTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (tblEnquiryDtlTOList != null && tblEnquiryDtlTOList.Count > 0)
                {
                    return _iTblEnquiryDtlBL.SaveOrgEnquiryDtl(tblEnquiryDtlTOList, Convert.ToInt32(loginUserId));
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblEnquiryDtlTOList Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }



            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostOrganizationEnquiryDtl";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        /// <summary>
        /// [11/12/2017] Vijaymala :Added to add enquiry detail of organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostOrganizationOverDueDtl")]
        [HttpPost]
        public ResultMessage PostOrganizationOverDueDtl([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblOverdueDtlTO> tblOverdueDtlTOList = JsonConvert.DeserializeObject<List<TblOverdueDtlTO>>(data["overDueDtlTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (tblOverdueDtlTOList != null && tblOverdueDtlTOList.Count > 0)
                {
                    return _iTblOverdueDtlBL.SaveOrgOverDueDtl(tblOverdueDtlTOList, Convert.ToInt32(loginUserId));

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblOverdueDtlTOList Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }



            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostOrganizationOverDueDtl";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        /// <summary>
        /// This Method is For Add New Person Along with Organization.
        /// </summary>
        /// <remarks>
        /// Sample Data {'personTO':{'SalutationId':1, 'MobileNo':"123456789", 'AlternateMobNo':"", 'PhoneNo':"", 'CreatedBy':1, 'FirstName':"xyz", 'MidName':"", 'LastName':"xyz", 'PrimaryEmail':"", 'AlternateEmail':"", 'Comments':"" }, 'loginUserId':1 }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostSaveNewPersonForOrganization")]
        [HttpPost]
        public ResultMessage PostSaveNewPersonForOrganization([FromBody]TblPersonTO personTO, Int32 organizationId, Int32 personTypeId)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblPersonTO tblPersonTO = personTO; //JsonConvert.DeserializeObject<TblPersonTO>(data["personTO"].ToString());

                if (tblPersonTO == null)
                {
                    resultMessage.DefaultBehaviour("marketingDetialsTO found null");
                    return resultMessage;
                }

                var loginUserId = tblPersonTO.CreatedBy;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                //tblPersonTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblPersonTO.CreatedOn = _iCommon.ServerDateTime;

                return _iTblPersonBL.SaveNewPersonAgainstOrganization(tblPersonTO, organizationId, personTypeId);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostSaveNewPersonForhOrganization");
                return resultMessage;
            }
        }
        
        /// <summary>
        /// Priyanka [07-06-2018] : Added for SHIVANGI.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostUpdateOverdueExistOrNot")]
        [HttpPost]
        public ResultMessage PostUpdateOverdueExistOrNot([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblOrganizationTO organizationTo = JsonConvert.DeserializeObject<TblOrganizationTO>(data["organizationTo"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                }

                if (organizationTo != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    organizationTo.UpdatedBy = Convert.ToInt32(loginUserId);
                    organizationTo.UpdatedOn = serverDate;
                    resultMessage= _iTblOrganizationBL.PostUpdateOverdueExistOrNot(organizationTo, (Convert.ToInt32(loginUserId)));
                }
                else
                {
                    resultMessage.DefaultBehaviour("organizationTO found null");
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error IN API Call PostUpdateOverdueExistOrNot");
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
