using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DashboardModels;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class QuotaAndRateController : Controller
    {
        private readonly ITblRateDeclareReasonsBL _iTblRateDeclareReasonsBL;
        private readonly ITblGlobalRateBL _iTblGlobalRateBL;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        private readonly ITblGroupBL _iTblGroupBL;
        private readonly ITblQuotaDeclarationBL _iTblQuotaDeclarationBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblBookingsBL _iTblBookingsBL;
        private readonly ICommon _iCommon;
        public QuotaAndRateController(ITblBookingsBL iTblBookingsBL, ICommon iCommon, ITblConfigParamsBL iTblConfigParamsBL, ITblQuotaDeclarationBL iTblQuotaDeclarationBL, ITblGroupBL iTblGroupBL, ITblOrganizationBL iTblOrganizationBL, ITblRateDeclareReasonsBL iTblRateDeclareReasonsBL, ITblGlobalRateBL iTblGlobalRateBL)
        {
            _iTblRateDeclareReasonsBL = iTblRateDeclareReasonsBL;
            _iTblGlobalRateBL = iTblGlobalRateBL;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iTblGroupBL = iTblGroupBL;
            _iTblQuotaDeclarationBL = iTblQuotaDeclarationBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblBookingsBL = iTblBookingsBL;
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

        [Route("GetRateReasonsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetRateReasonsForDropDown()
        {
            List<TblRateDeclareReasonsTO> tblRateDeclareReasonsTOList = _iTblRateDeclareReasonsBL.SelectAllTblRateDeclareReasonsList();
            if (tblRateDeclareReasonsTOList != null && tblRateDeclareReasonsTOList.Count > 0)
            {
                List<DropDownTO> reasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblRateDeclareReasonsTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblRateDeclareReasonsTOList[i].ReasonDesc;
                    dropDownTO.Value = tblRateDeclareReasonsTOList[i].IdRateReason;
                    reasonList.Add(dropDownTO);
                }
                return reasonList;
            }
            else return null;
        }

        [Route("GetRateDeclarationHistory")]
        [HttpGet]
        public List<TblGlobalRateTO> GetRateDeclarationHistory(String fromDate, String toDate)
        {
            DateTime frmDate = Convert.ToDateTime(fromDate);
            DateTime tDate = Convert.ToDateTime(toDate);
            if (frmDate == DateTime.MinValue)
                frmDate = _iCommon.ServerDateTime.AddDays(-7);
            if (tDate == DateTime.MinValue)
                tDate = _iCommon.ServerDateTime;

            return _iTblGlobalRateBL.SelectTblGlobalRateTOList(frmDate, tDate);
        }

        /// <summary>
        /// Sanjay [2017-11-21] Showing Sales Agent List for quota declaration with Rate and Band
        /// </summary>
        /// <returns></returns>
        [Route("GetSalesAgentListWithBrandAndRate")]
        [HttpGet]
        public List<TblOrganizationTO> GetSalesAgentListWithBrandAndRate()
        {
            List<TblOrganizationTO> list = _iTblBookingsBL.SelectSalesAgentListWithBrandAndRate();
            return list;
        }

        /// <summary>
        /// Saket [2018-01-19] Added.
        /// </summary>
        /// <returns></returns>
        [Route("GetGroupWithRate")]
        [HttpGet]
        public List<TblGroupTO> GetGroupWithRate()
        {
            List<TblGroupTO> tblGroupTOList = _iTblGroupBL.SelectTblGroupTOWithRate();
            return tblGroupTOList;
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Get Information About Specific Quota
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        [Route("GetQuotaDeclarationInfo")]
        [HttpGet]

        public TblQuotaDeclarationTO GetQuotaDeclarationInfo(Int32 quotaDeclarationId)
        {
            return _iTblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(quotaDeclarationId);
        }

        /// <summary>
        /// Sanjay [2017-05-08] To Get Information About Specific Quota
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        [Route("CheckForValidityAndReset")]
        [HttpGet]
        public Boolean CheckForValidityAndReset(int quotaDeclarationId)
        {
            TblQuotaDeclarationTO tblQuotaDeclarationTO = _iTblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(quotaDeclarationId);
            return _iTblQuotaDeclarationBL.CheckForValidityAndReset(tblQuotaDeclarationTO);
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Show Latest Quota remaining and Rate Band for C&F agent on booking screen
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        [Route("GetLatestQuotaAndRateInfo")]
        [HttpGet]
        public List<TblQuotaDeclarationTO> GetLatestQuotaAndRateInfo(Int32 cnfId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

            List<TblQuotaDeclarationTO> list = _iTblQuotaDeclarationBL.SelectLatestQuotaDeclarationTOList(cnfId, sysDate);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ValidUpto > 0)
                    {
                        if (!_iTblQuotaDeclarationBL.CheckForValidityAndReset(list[i]))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }

                return list;
            }
            return null;
        }

        [Route("GetQuotaAndRateDashboardInfo")]
        [HttpGet]
        public QuotaAndRateInfo GetQuotaAndRateDashboardInfo(int roletypeId, Int32 orgId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

            return _iTblQuotaDeclarationBL.SelectQuotaAndRateDashboardInfo(roletypeId, orgId, sysDate);
        }

        [Route("GetQuotaAndRateDashboardInfoList")]
        [HttpGet]
        public List<QuotaAndRateInfo> GetQuotaAndRateDashboardInfoList(int roleTypeId, Int32 orgId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

            return _iTblQuotaDeclarationBL.SelectQuotaAndRateDashboardInfoList(roleTypeId, orgId, sysDate);
        }

        //Prajakta[2020-02-05] Added to get always latest rate
        [Route("GetLatestRateInfo")]
        [HttpGet]
        public List<TblQuotaDeclarationTO> GetLatestRateInfo(Int32 cnfId, DateTime sysDate, Boolean isQuotaDeclaration)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

            List<TblQuotaDeclarationTO> list = _iTblQuotaDeclarationBL.GetLatestRateInfo(cnfId, sysDate, isQuotaDeclaration);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ValidUpto > 0)
                    {
                        if (!_iTblQuotaDeclarationBL.CheckForValidityAndReset(list[i]))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }

                return list;
            }
            return null;
        }


        [Route("GetMinAndMaxValueConfigForRate")]
        [HttpGet]
        public String GetMinAndMaxValueConfigForRate()
        {
            string configValue = string.Empty;
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_MIN_AND_MAX_RATE_DEFAULT_VALUES);
            if (tblConfigParamsTO != null)
                configValue = Convert.ToString(tblConfigParamsTO.ConfigParamVal);
            return configValue;
        }
        //Aniket [29-4-2019] added to update booking quota against perticular CNF
        [HttpGet]
        [Route("GetBookingQuotaAgainstCNF")]
        public TblQuotaDeclarationTO GetBookingQuotaAgainstCNF(Int32 cnfOrgId, Int32 brandId)
        {
            

            return _iTblQuotaDeclarationBL.GetBookingQuotaAgainstCNF(cnfOrgId, brandId);

        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // POST api/values
        [Route("AnnounceRateAndQuota")]
        [HttpPost]
        public ResultMessage AnnounceRateAndQuota([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblOrganizationTO> tblOrganizationTOList = JsonConvert.DeserializeObject<List<TblOrganizationTO>>(data["cnfList"].ToString());

                List<TblGroupTO> tblGroupTOList = JsonConvert.DeserializeObject<List<TblGroupTO>>(data["groupRateList"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                var comments = data["comments"].ToString();
                var rateReasonId = data["rateReasonId"].ToString();
                var rateReasonDesc = data["rateReasonDesc"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }
                //[12/12/2017] Vijaymala :Commented the code because rate reason is not mandatory
                //if (Convert.ToInt32(rateReasonId) <= 0)
                //{
                //    resultMessage.DefaultBehaviour("rateReasonId Found NULL");
                //    return resultMessage;
                //}

                if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
                {
                    // 1. Prepare TblGlobalRateTO

                    var brandList = tblOrganizationTOList[0].BrandRateDtlTOList.GroupBy(b => b.BrandId).ToList();
                    List<TblGlobalRateTO> tblGlobalRateTOList = new List<TblGlobalRateTO>();
                    DateTime serverDate = _iCommon.ServerDateTime;
                    List<TblQuotaDeclarationTO> tblQuotaDeclarationTOList = new List<TblQuotaDeclarationTO>();

                    for (int i = 0; i < brandList.Count; i++)
                    {
                        int brandId = brandList[i].Key;
                        TblGlobalRateTO tblGlobalRateTO = new TblGlobalRateTO();
                        tblGlobalRateTO.CreatedOn = serverDate;
                        tblGlobalRateTO.CreatedBy = Convert.ToInt32(loginUserId);
                        tblGlobalRateTO.BrandId = brandId;
                        tblGlobalRateTO.Rate = tblOrganizationTOList[0].BrandRateDtlTOList.Where(a => a.BrandId == brandId).FirstOrDefault().Rate;
                        //if (tblGlobalRateTO.Rate <= 0)
                        //{
                        //    resultMessage.Result = 0;
                        //    resultMessage.DefaultBehaviour("Rate can not less than or equal to zero");
                        //    return resultMessage;

                        //}
                        tblGlobalRateTO.BrandName = tblOrganizationTOList[0].BrandRateDtlTOList.Where(a => a.BrandId == brandId).FirstOrDefault().BrandName;
                        tblGlobalRateTO.Comments = Convert.ToString(comments);
                        tblGlobalRateTO.RateReasonId = Convert.ToInt32(rateReasonId);
                        tblGlobalRateTO.RateReasonDesc = Convert.ToString(rateReasonDesc);
                        tblGlobalRateTO.QuotaDeclarationTOList = new List<TblQuotaDeclarationTO>();

                        for (int q = 0; q < tblOrganizationTOList.Count; q++)
                        {
                            TblOrganizationTO orgTO = tblOrganizationTOList[q];

                            BrandRateDtlTO brandRateDtlTO = orgTO.BrandRateDtlTOList.Where(a => a.BrandId == brandId).FirstOrDefault();

                            TblQuotaDeclarationTO tblQuotaDeclarationTO = new TblQuotaDeclarationTO();
                            tblQuotaDeclarationTO.OrgId = orgTO.IdOrganization;
                            tblQuotaDeclarationTO.QuotaAllocDate = serverDate;
                            tblQuotaDeclarationTO.RateBand = orgTO.BrandRateDtlTOList.Where(a => a.BrandId == brandId).FirstOrDefault().RateBand;
                            tblQuotaDeclarationTO.ValidUpto = orgTO.BrandRateDtlTOList.Where(a => a.BrandId == brandId).FirstOrDefault().ValidUpto; //Sudhir[25-06-2018] Added For Madhav
                            tblQuotaDeclarationTO.AllocQty = orgTO.BrandRateDtlTOList.Where(a => a.BrandId == brandId).FirstOrDefault().LastAllocQty; //Sudhir[25-06-2018] Added For Madhav

                            //tblQuotaDeclarationTO.AllocQty = orgTO.LastAllocQty;
                            tblQuotaDeclarationTO.CreatedOn = serverDate;
                            tblQuotaDeclarationTO.CreatedBy = Convert.ToInt32(loginUserId);
                            tblQuotaDeclarationTO.IsActive = 1;

                            tblQuotaDeclarationTO.Tag = orgTO;

                            //Aniket [4-7-2019] added to keep validupto field active with previous rate
                            // as discussed with Saket
                            if (tblQuotaDeclarationTO.ValidUpto > 0)
                            {
                                TblQuotaDeclarationTO temp = new TblQuotaDeclarationTO();
                               temp.OrgId = tblQuotaDeclarationTO.OrgId;
                               temp.IdQuotaDeclaration = brandRateDtlTO.QuotaDeclarationId;
                               temp.ValidUpto = tblQuotaDeclarationTO.ValidUpto;
                               temp.UpdatedOn = serverDate;
                               temp.UpdatedBy = Convert.ToInt32(loginUserId);
                               temp.IsActive = 1;
                               tblGlobalRateTO.PreviousQuotaDeclarationTOList.Add(temp);
                            }
                                

                            tblQuotaDeclarationTO.ValidUpto = 0;
                            tblGlobalRateTO.QuotaDeclarationTOList.Add(tblQuotaDeclarationTO);
                            
                        }

                        tblGlobalRateTOList.Add(tblGlobalRateTO);

                    }


                    for (int j = 0; j < tblGroupTOList.Count; j++)
                    {
                        TblGlobalRateTO tblGlobalRateTO = new TblGlobalRateTO();
                        tblGlobalRateTO.CreatedOn = serverDate;
                        tblGlobalRateTO.CreatedBy = Convert.ToInt32(loginUserId);
                        tblGlobalRateTO.GroupId = tblGroupTOList[j].IdGroup;
                        tblGlobalRateTO.Rate = tblGroupTOList[j].Rate;
                        tblGlobalRateTO.BrandName = tblGroupTOList[j].GroupName;
                        tblGlobalRateTO.Comments = Convert.ToString(comments);
                        tblGlobalRateTO.RateReasonId = Convert.ToInt32(rateReasonId);
                        tblGlobalRateTO.RateReasonDesc = Convert.ToString(rateReasonDesc);

                        tblGlobalRateTOList.Add(tblGlobalRateTO);

                    }

                    return _iTblQuotaDeclarationBL.SaveDeclaredRateAndAllocatedBand(tblGlobalRateTOList);


                }
                else
                {
                    resultMessage.DefaultBehaviour("tblOrganizationTOList Found NULL");
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AnnounceRateAndQuota");
                return resultMessage;
            }
        }
        //Aniket [30-4-2019]
        [HttpPost]
        [Route("UpdateTblQuotaDeclaration")]
        public int UpdateTblQuotaDeclaration([FromBody] JObject data)
        {
            try
            {
                TblQuotaDeclarationTO tblQuotaDeclarationTO = JsonConvert.DeserializeObject<TblQuotaDeclarationTO>(data["tblQuota"].ToString());
                return _iTblQuotaDeclarationBL.UpdateTblQuotaDeclaration(tblQuotaDeclarationTO);
            }
            catch(Exception ex)
            {
                return -1;
            }
          
        }

        //// POST api/values
        //[Route("AnnounceRateAndQuota")]
        //[HttpPost]
        //public ResultMessage AnnounceRateAndQuota([FromBody] JObject data)
        //{
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    try
        //    {

        //        List<TblOrganizationTO> tblOrganizationTOList = JsonConvert.DeserializeObject<List<TblOrganizationTO>>(data["cnfList"].ToString());
        //        var declaredRate = data["declaredRate"].ToString();
        //        var loginUserId = data["loginUserId"].ToString();
        //        var comments = data["comments"].ToString();
        //        var rateReasonId = data["rateReasonId"].ToString();
        //        var rateReasonDesc = data["rateReasonDesc"].ToString();

        //        if (Convert.ToDouble(declaredRate) == 0)
        //        {
        //            resultMessage.MessageType = ResultMessageE.Error;
        //            resultMessage.Result = 0;
        //            resultMessage.Text = "declaredRate Found 0";
        //            return resultMessage;
        //        }

        //        if (Convert.ToInt32(loginUserId) <= 0)
        //        {
        //            resultMessage.MessageType = ResultMessageE.Error;
        //            resultMessage.Result = 0;
        //            resultMessage.Text = "loginUserId Found 0";
        //            return resultMessage;
        //        }

        //        if (Convert.ToInt32(rateReasonId) <= 0)
        //        {
        //            resultMessage.MessageType = ResultMessageE.Error;
        //            resultMessage.Result = 0;
        //            resultMessage.Text = "rateReasonId Found 0";
        //            return resultMessage;
        //        }

        //        if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
        //        {
        //            // 1. Prepare TblGlobalRateTO

        //            DateTime serverDate = _iCommon.ServerDateTime;
        //            TblGlobalRateTO tblGlobalRateTO = new TblGlobalRateTO();
        //            tblGlobalRateTO.CreatedOn = serverDate;
        //            tblGlobalRateTO.CreatedBy = Convert.ToInt32(loginUserId);
        //            tblGlobalRateTO.Rate = Convert.ToDouble(declaredRate);
        //            tblGlobalRateTO.Comments = Convert.ToString(comments);
        //            tblGlobalRateTO.RateReasonId = Convert.ToInt32(rateReasonId);
        //            tblGlobalRateTO.RateReasonDesc = Convert.ToString(rateReasonDesc);

        //            //2. Prepare Quota Declaration List
        //            List<TblQuotaDeclarationTO> tblQuotaDeclarationTOList = new List<TblQuotaDeclarationTO>();
        //            List<TblQuotaDeclarationTO> tblQuotaExtensionTOList = new List<TblQuotaDeclarationTO>();

        //            var quotaExtList = tblOrganizationTOList.Where(q => q.ValidUpto > 0).ToList();

        //            if (quotaExtList != null && quotaExtList.Count > 0)
        //            {
        //                for (int i = 0; i < quotaExtList.Count; i++)
        //                {
        //                    TblOrganizationTO orgTO = quotaExtList[i];

        //                    TblQuotaDeclarationTO tblQuotaDeclarationTO = new TblQuotaDeclarationTO();
        //                    tblQuotaDeclarationTO.OrgId = orgTO.IdOrganization;
        //                    tblQuotaDeclarationTO.IdQuotaDeclaration = orgTO.QuotaDeclarationId;
        //                    tblQuotaDeclarationTO.ValidUpto = orgTO.ValidUpto;
        //                    tblQuotaDeclarationTO.UpdatedOn = serverDate;
        //                    tblQuotaDeclarationTO.UpdatedBy = Convert.ToInt32(loginUserId);
        //                    tblQuotaDeclarationTO.IsActive = 1;
        //                    tblQuotaExtensionTOList.Add(tblQuotaDeclarationTO);
        //                }
        //            }

        //            for (int i = 0; i < tblOrganizationTOList.Count; i++)
        //            {
        //                TblOrganizationTO orgTO = tblOrganizationTOList[i];

        //                TblQuotaDeclarationTO tblQuotaDeclarationTO = new TblQuotaDeclarationTO();
        //                tblQuotaDeclarationTO.OrgId = orgTO.IdOrganization;
        //                tblQuotaDeclarationTO.QuotaAllocDate = serverDate;
        //                tblQuotaDeclarationTO.RateBand = orgTO.LastRateBand;
        //                tblQuotaDeclarationTO.AllocQty = orgTO.LastAllocQty;
        //                tblQuotaDeclarationTO.CreatedOn = serverDate;
        //                tblQuotaDeclarationTO.CreatedBy = Convert.ToInt32(loginUserId);
        //                tblQuotaDeclarationTO.IsActive = 1;

        //                tblQuotaDeclarationTO.Tag = orgTO;
        //                tblQuotaDeclarationTOList.Add(tblQuotaDeclarationTO);

        //            }

        //            int result= _iTblQuotaDeclarationBL.SaveDeclaredRateAndAllocatedQuota(tblQuotaExtensionTOList,tblQuotaDeclarationTOList, tblGlobalRateTO);
        //            if(result!=1)
        //            {
        //                resultMessage.MessageType = ResultMessageE.Error;
        //                resultMessage.Result = 0;
        //                resultMessage.Text = "Error In SaveDeclaredRateAndAllocatedQuota Method";
        //                return resultMessage;
        //            }

        //            resultMessage.MessageType = ResultMessageE.Information;
        //            resultMessage.Result = 1;
        //            resultMessage.Text = "Booking Quota Announced Sucessfully";
        //            return resultMessage;

        //        }
        //        else
        //        {
        //            resultMessage.MessageType = ResultMessageE.Error;
        //            resultMessage.Result = 0;
        //            resultMessage.Text = "tblOrganizationTOList Found NULL";
        //            return resultMessage;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.MessageType = ResultMessageE.Error;
        //        resultMessage.Result = -1;
        //        resultMessage.Exception = ex;
        //        resultMessage.Text = "Exception Error IN API Call AnnounceRateAndQuota";
        //        return resultMessage;
        //    }
        //}



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
