using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class MarketingVisitController : Controller
    {
        private readonly ITblVisitPurposeBL _iTblVisitPurposeBL;
        private readonly ITblVisitPersonDetailsBL _iTblVisitPersonDetailsBL;
        private readonly ITblSiteStatusBL _iTblSiteStatusBL;
        private readonly ITblPaymentTermBL _iTblPaymentTermBL;
        private readonly ITblSiteTypeBL _iTblSiteTypeBL;
        private readonly ITblVisitFollowUpRolesBL _iTblVisitFollowUpRolesBL;
        private readonly ITblVisitIssueReasonsBL _iTblVisitIssueReasonsBL;
        private readonly ITblVisitProjectDetailsBL _iTblVisitProjectDetailsBL;
        private readonly ITblVisitDetailsBL _iTblVisitDetailsBL;
        private readonly IMarketingDetailsBL _iMarketingDetailsBL;
        private readonly ITblPersonAddrDtlBL _iTblPersonAddrDtlBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ICommon _iCommon;
        public MarketingVisitController(ICommon iCommon, ITblPersonBL iTblPersonBL, ITblPersonAddrDtlBL iTblPersonAddrDtlBL, ITblPaymentTermBL iTblPaymentTermBL, IMarketingDetailsBL iMarketingDetailsBL, ITblVisitDetailsBL iTblVisitDetailsBL, ITblVisitProjectDetailsBL iTblVisitProjectDetailsBL, ITblVisitIssueReasonsBL iTblVisitIssueReasonsBL, ITblVisitFollowUpRolesBL iTblVisitFollowUpRolesBL, ITblSiteStatusBL iTblSiteStatusBL, ITblVisitPurposeBL iTblVisitPurposeBL, ITblVisitPersonDetailsBL iTblVisitPersonDetailsBL)
        {
            _iTblVisitPurposeBL = iTblVisitPurposeBL;
            _iTblVisitPersonDetailsBL = iTblVisitPersonDetailsBL;
            _iTblSiteStatusBL = iTblSiteStatusBL;
            _iTblVisitFollowUpRolesBL = iTblVisitFollowUpRolesBL;
            _iTblVisitIssueReasonsBL = iTblVisitIssueReasonsBL;
            _iTblVisitProjectDetailsBL = iTblVisitProjectDetailsBL;
            _iTblVisitDetailsBL = iTblVisitDetailsBL;
            _iMarketingDetailsBL = iMarketingDetailsBL;
            _iTblSiteStatusBL = iTblSiteStatusBL;
            _iTblPaymentTermBL = iTblPaymentTermBL;
            _iTblPersonAddrDtlBL = iTblPersonAddrDtlBL;
            _iTblPersonBL = iTblPersonBL;
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
        /// Vaibhav [3-Oct-2017] added to get all visit purpose
        /// </summary>
        /// <param name="value"></param>
        [Route("GetVisitPurposeListForDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetVisitPurposeListForDropDownList(int visitTypeId)
        {
            return _iTblVisitPurposeBL.SelectVisitPurposeListForDropDown(visitTypeId);
        }

        /// <summary>
        /// Vaibhav [3-Oct-2017] added to get all visit related person list
        /// </summary>
        /// <param name="value"></param>
        [Route("GetVisitPersonList")]
        [HttpGet]
        public List<TblVisitPersonDetailsTO> GetVisitPersonList(int visitTypeId)
        {
            return _iTblVisitPersonDetailsBL.SelectAllTblVisitPersonDetailsList(visitTypeId);
        }

        //Hrushikesh added to get person data for saving for offline
        [Route("GetVisitPersonDtlsForOffline")]
        [HttpGet]
        public List<TblVisitPersonDetailsTO> GetVisitPersonDtlsForOffline(String personTypeIds)
        {
            return _iTblVisitPersonDetailsBL.SelectPersonDetailsForOffline(personTypeIds);
        }
        /// <summary>
        /// Vaibhav [3-Oct-2017] added to get all site status
        /// </summary>
        /// <param name="value"></param>
        [Route("GetSiteStatusListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetSiteStatusListForDropDown()
        {
            return _iTblSiteStatusBL.SelectAllSiteStatusForDropDown();
        }

        /// <summary>
        /// Vaibhav [3_oct-2017] added to get all payment terms
        /// </summary>
        /// <param name="value"></param>
        [Route("GetPaymentTermListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetPaymentTermListForDropDown()
        {
            return _iTblPaymentTermBL.SelectPaymentTermListForDopDown();
        }

        /// <summary>
        /// Vaibhav [5-oct-2017] added to get all site type list
        /// </summary>
        /// <param name="value"></param>
        [Route("GetAllSiteTypeList")]
        [HttpGet]
        public List<TblSiteTypeTO> GetAllSiteTypeList()
        {
            return _iTblSiteTypeBL.SelectAllTblSiteTypeList();
        }

        /// <summary>
        /// Vaibhav [9-Oct-2017] added to get visit follow up list
        /// </summary>
        /// <param name="value"></param>
        [Route("GetAllVisitFollowUpRoleList")]
        [HttpGet]
        public List<TblVisitFollowUpRolesTO> GetAllVisitFollowUpRoleList()
        {
            return _iTblVisitFollowUpRolesBL.SelectAllTblVisitFollowUpRolesList();
        }

        /// <summary>
        /// Vaibhav [9-Oct-2017] added to get visit issue reason list
        /// </summary>
        /// <param name="value"></param>
        [Route("GetVisitIssueReasonListForDropDown")]
        [HttpGet]
        public List<TblVisitIssueReasonsTO> GetVisitIssueReasonListForDropDown()
        {
            return _iTblVisitIssueReasonsBL.SelectAllVisitIssueReasonsListForDropDown();
        }

        /// <summary>
        /// Vaibhav [24-Oct-2017] added to get influencer visit project details 
        /// </summary>
        /// <param name="value"></param>
        [Route("GetVisitProjectDetailsList")]
        [HttpGet]
        public List<TblVisitProjectDetailsTO> GetVisitProjectDetailsList()
        {
            return _iTblVisitProjectDetailsBL.SelectAllTblVisitProjectDetailsList();
        }

        /// <summary>
        /// Vaibhav [27-Oct-2017] added to get all user follow up role list
        /// </summary>
        /// <param name="value"></param>
        [Route("GetFollowUpUserRoleListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetFollowUpUserRoleListForDropDown()
        {
            return _iTblVisitFollowUpRolesBL.SelectFollowUpUserRoleListForDropDown();
        }

        /// <summary>
        /// Vaibhav [27-Oct-2017] added to get non user follow up roles
        /// </summary>
        /// <param name="value"></param>
        [Route("GetFollowUpRoleListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetFollowUpRoleListForDropDown()
        {
            return _iTblVisitFollowUpRolesBL.SelectFollowUpRoleListForDropDown();
        }

        /// <summary>
        /// Vaibhav [28-Oct-2017] added to get all visit details
        /// </summary>
        /// <param name="value"></param>
        [Route("GetAllVisitDetailsList")]
        [HttpGet]
        public List<TblVisitDetailsTO> GetAllVisitDetailsList()
        {
            return _iTblVisitDetailsBL.SelectAllVisitDetailsList();
        }

        /// <summary>
        /// Vaibhav [31-Oct-2017] added to get visit details
        /// </summary>
        /// <param name="value"></param>
        [Route("GetVisitDetailsList")]
        [HttpGet]
        public MarketingDetailsTO GetVisitDetailsList(int visitId)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                return _iMarketingDetailsBL.SelectVisitDetailsList(visitId);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetVisitDetailsList");
                return null;
            }
        }

        /// <summary>
        /// Vaibhav [08-Nov-2017] added to get visit roles 
        /// </summary>
        /// <param name="value"></param>
        [Route("GetVisitRoleForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetVisitRoleForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                return _iTblVisitFollowUpRolesBL.SelectVisitRoleListForDropDown();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetVisitRoleForDropDown");
                return null;
            }
        }

        /// <summary>
        /// Sudhir - Added for Get All Visit Person TypeList.
        /// </summary>
        /// <returns></returns>
        [Route("GetVisitPersonTypeDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetVisitPersonTypeDropDownList()
        {
            return _iTblVisitPersonDetailsBL.SelectAllVisitPersonTypeList();
        }

        /// <summary>
        /// Sudhir - Added for Get Visit Persons Based on Person Type.
        /// </summary>
        /// <param name="personType"></param>
        /// <returns></returns>
        [Route("GetVisitPersonDropDownListOnPersonType")]
        [HttpGet]
        public List<DropDownTO> GetVisitPersonDropDownListOnPersonType(Int32 personType)
        {
            return _iTblVisitPersonDetailsBL.SelectVisitPersonDropDownListOnPersonType(personType);
        }

        /// <summary>
        /// Sudhir[13-March-2018] - Added for Get Address on PersonId And AddressTypeId.
        /// </summary>
        /// <returns></returns>
        [Route("GetPersonAddressDetails")]
        [HttpGet]
        public TblAddressTO GetPersonAddressDetails(Int32 personId,Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS)
        {
            Int32 addressTypeId = (int)addressTypeE;
            return _iTblPersonAddrDtlBL.SelectAddressTOonPersonAddrDtlId(personId, addressTypeId);
        }
        /// <summary>
        /// Sudhir - Added for Get Visit Persons Based on Person Type.
        /// </summary>
        /// <param name="personType"></param>
        /// <returns></returns>
        [Route("GetVisitPersonDropDownListOnPersonTypeAndOrgId")]
        [HttpGet]
        public List<DropDownTO> GetVisitPersonDropDownListOnPersonType(Int32 personType, int? organizationId = 0)
        {
            return _iTblVisitPersonDetailsBL.SelectVisitPersonDropDownListOnPersonType(personType, organizationId);
        }

        #endregion

        #region POST

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Vaibhav [30-Oct-2017] added to Insert Marketing details 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostMarketingDetails")]
        [HttpPost]
        public ResultMessage PostMarketingDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                MarketingDetailsTO marketingDetailsTO = JsonConvert.DeserializeObject<MarketingDetailsTO>(data["marketingDetailsTO"].ToString());

                if (marketingDetailsTO == null)
                {
                    resultMessage.DefaultBehaviour("marketingDetialsTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                marketingDetailsTO.CreatedBy = Convert.ToInt32(loginUserId);
                marketingDetailsTO.CreatedOn = _iCommon.ServerDateTime;

                return _iMarketingDetailsBL.SaveMarketingDetails(marketingDetailsTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostMarketingDetails");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [02-Nov-2017] Added to update marketing details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostUpdateMarketingDetails")]
        [HttpPost]
        public ResultMessage PostUpdateMarketingDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                MarketingDetailsTO marketingDetailsTO = JsonConvert.DeserializeObject<MarketingDetailsTO>(data["marketingDetailsTO"].ToString());

                if (marketingDetailsTO == null)
                {
                    resultMessage.DefaultBehaviour("marketingDetialsTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                marketingDetailsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                marketingDetailsTO.UpdatedOn = _iCommon.ServerDateTime;

                return _iMarketingDetailsBL.UpdateMarketingDetails(marketingDetailsTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateMarketingDetails");   
                return resultMessage;
            }
        }

        /// <summary>
        /// This Method is For Add New Person.
        /// </summary>
        /// <remarks>
        /// Sample Data {'personTO':{'SalutationId':1, 'MobileNo':"123456789", 'AlternateMobNo':"", 'PhoneNo':"", 'CreatedBy':1, 'FirstName':"xyz", 'MidName':"", 'LastName':"xyz", 'PrimaryEmail':"", 'AlternateEmail':"", 'Comments':"" }, 'loginUserId':1 }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostSaveNewPerson")]
        [HttpPost]
        public ResultMessage PostSaveNewPerson([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblPersonTO tblPersonTO = JsonConvert.DeserializeObject<TblPersonTO>(data["personTO"].ToString());

                if (tblPersonTO == null)
                {
                    resultMessage.DefaultBehaviour("marketingDetialsTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                tblPersonTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblPersonTO.CreatedOn = _iCommon.ServerDateTime;

                return _iTblPersonBL.AddNewPerson(tblPersonTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateMarketingDetails");
                return resultMessage;
            }
        }


        /// <summary>
        /// This Method is For Adding New Person , Address and its Mapping.
        /// </summary>
        /// <param name="tblPerson"></param>
        /// <param name="tblAddress"></param>
        /// <returns></returns>
        [Route("PostNewPersonWithAddress")]
        [HttpPost]
        public ResultMessage PostNewPersonWithAddress(TblPersonTO tblPerson,TblAddressTO tblAddress)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblPersonTO tblPersonTO = tblPerson;//JsonConvert.DeserializeObject<TblPersonTO>(data["personTO"].ToString());

                if (tblPersonTO == null)
                {
                    resultMessage.DefaultBehaviour("tblPersonTO found null");
                    return resultMessage;
                }
                var loginUserId = tblPerson.CreatedBy;//data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                //tblPersonTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblPersonTO.CreatedOn = _iCommon.ServerDateTime;

                return _iTblPersonBL.AddNewPersonWithAddressDetails(tblPersonTO,tblAddress);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewPersonWithAddress");
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

        #region DELETE 

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        #endregion
    }
}
