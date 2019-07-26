using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class CompetitorController : Controller
    {
        private readonly ITblOtherSourceBL _iTblOtherSourceBL;
        private readonly ITblCompetitorUpdatesBL _iTblCompetitorUpdatesBL;
        private readonly ITblCompetitorExtBL _iTblCompetitorExtBL;
        private readonly ITblFreightUpdateBL _iTblFreightUpdateBL;
        private readonly ICommon _iCommon;
        public CompetitorController(ICommon iCommon, ITblFreightUpdateBL iTblFreightUpdateBL, ITblCompetitorExtBL iTblCompetitorExtBL, ITblOtherSourceBL iTblOtherSourceBL, ITblCompetitorUpdatesBL iTblCompetitorUpdatesBL)
        {
            _iTblOtherSourceBL = iTblOtherSourceBL;
            _iTblCompetitorUpdatesBL = iTblCompetitorUpdatesBL;
            _iTblCompetitorExtBL = iTblCompetitorExtBL;
            _iTblFreightUpdateBL = iTblFreightUpdateBL;
            _iCommon = iCommon;
        }
        #region Get

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

        [Route("GetOtherSourceOfMarketTrendForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetOtherSourceOfMarketTrendForDropDown()
        {
            List<DropDownTO> list = _iTblOtherSourceBL.SelectOtherSourceOfMarketTrendForDropDown();
            return list;
        }

        [Route("GetCompeUpdateUserDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCompeUpdateUserDropDown()
        {
            List<DropDownTO> list = _iTblCompetitorUpdatesBL.SelectCompeUpdateUserDropDown();
            return list;
        }

        [Route("GetCompetitorBrandNamesDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetCompetitorBrandNamesDropDownList(Int32 competitorOrgId)
        {
            List<DropDownTO> list = _iTblCompetitorExtBL.SelectCompetitorBrandNamesDropDownList(competitorOrgId);
            return list;
        }

        [Route("GetOtherSourceOfMarketTrendWrtDesc")]
        [HttpGet]
        public List<TblOtherSourceTO> GetOtherSourceOfMarketTrendWrtDesc(String otherDesc)
        {
            return _iTblOtherSourceBL.SelectTblOtherSourceListFromDesc(otherDesc);
        }

        [Route("GetCompetitorUpdates")]
        [HttpGet]
        public List<TblCompetitorUpdatesTO> GetCompetitorUpdates(DateTime fromDate, DateTime toDate)
        {
            if (fromDate == DateTime.MinValue)
                fromDate = _iCommon.ServerDateTime.AddDays(-7);
            if (toDate == DateTime.MinValue)
                toDate = _iCommon.ServerDateTime;

            return _iTblCompetitorUpdatesBL.SelectAllTblCompetitorUpdatesList(0, 0, fromDate, toDate);
        }

        [Route("GetLastPriceForCompetitorAndBrand")]
        [HttpGet]
        public TblCompetitorUpdatesTO GetLastPriceForCompetitorAndBrand(Int32 brandId)
        {
            return _iTblCompetitorUpdatesBL.SelectLastPriceForCompetitorAndBrand(brandId);
        }


        [Route("GetMarketUpdate")]
        [HttpGet]
        public List<TblCompetitorUpdatesTO> GetMarketUpdate(string fromDate, string toDate, Int32 competitorId = 0, Int32 enteredBy = 0)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                String[] splitChars = fromDate.Split('-');
                String newFrmDate = "";
                newFrmDate = splitChars[2] + "/" + splitChars[1] + "/" + splitChars[0];
                frmDt = Convert.ToDateTime(fromDate);
               // frmDt = Convert.ToDateTime(frmDt.ToString(Constants.AzureDateFormat));

                //DateTime dt = DateTime.ParseExact(fromDate, "dd/MM/yyyy",
                //                                   CultureInfo.InvariantCulture);

                //string azureDate = dt.ToString("yyyy/MM/dd");

                //DateTime dt1 = DateTime.ParseExact(azureDate, "yyyy/MM/dd",
                //                                   CultureInfo.InvariantCulture);

            }
            if (Constants.IsDateTime(toDate))
            {
                String[] splitChars = toDate.Split('-');
                String newToDate = "";
                newToDate = splitChars[2] + "/" + splitChars[1] + "/" + splitChars[0];
                toDt = Convert.ToDateTime(toDate);
                //toDt = Convert.ToDateTime(toDt.ToString(Constants.AzureDateFormat));
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.AddDays(-7);
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime;

            return _iTblCompetitorUpdatesBL.SelectAllTblCompetitorUpdatesList(competitorId, enteredBy, frmDt, toDt);
        }

        [Route("GetFreightUpdate")]
        [HttpGet]
        public List<TblFreightUpdateTO> GetFreightUpdate(string fromDate, string toDate, Int32 districtId = 0, Int32 talukaId = 0,Int32 userId=0)
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
                frmDt = _iCommon.ServerDateTime.AddDays(-7);
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime;

            List<TblFreightUpdateTO> list= _iTblFreightUpdateBL.SelectAllTblFreightUpdateList(frmDt,toDt, districtId, talukaId);
            if(list!=null && userId >0)
            {
                var finList = list.Where(c => c.CreatedBy == userId).ToList();
                return finList;
            }

            return list;
        }


        //Sudhir[09-APR-2018] Added for getAll Competitor List
        [Route("GetAllCompetitorWthBrands")]
        [HttpGet]
        public List<DropDownTO> GetAllCompetitorWthBrands()
        {
            List<DropDownTO> list = _iTblCompetitorExtBL.SelectAllCompetitorDropDownList();
            return list;
        }




        #endregion

        #region Post

        [Route("PostMarketUpdate")]
        [HttpPost]
        public ResultMessage PostMarketUpdate([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<TblCompetitorUpdatesTO> competitorUpdatesTOList = JsonConvert.DeserializeObject<List<TblCompetitorUpdatesTO>>(data["competitorUpdatesTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (competitorUpdatesTOList == null || competitorUpdatesTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : sizeSpecWiseStockTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = _iCommon.ServerDateTime;
                DateTime stockDate = confirmedDate.Date;
                for (int i = 0; i < competitorUpdatesTOList.Count; i++)
                {
                    competitorUpdatesTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    competitorUpdatesTOList[i].CreatedOn = confirmedDate;
                    competitorUpdatesTOList[i].UpdateDatetime = confirmedDate;
                    if (competitorUpdatesTOList[i].DealerId > 0)
                        competitorUpdatesTOList[i].InformerName = competitorUpdatesTOList[i].DealerName;
                    else if (competitorUpdatesTOList[i].OtherSourceId > 0)
                        competitorUpdatesTOList[i].InformerName = competitorUpdatesTOList[i].OtherSourceDesc;
                    else
                        competitorUpdatesTOList[i].InformerName = competitorUpdatesTOList[i].OtherSourceOtherDesc;

                }

                ResultMessage resMsg = _iTblCompetitorUpdatesBL.SaveMarketUpdate(competitorUpdatesTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostMarketUpdate";
                return returnMsg;
            }
        }

        [Route("PostFreightUpdate")]
        [HttpPost]
        public ResultMessage PostFreightUpdate([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblFreightUpdateTO freightUpdateTO = JsonConvert.DeserializeObject<TblFreightUpdateTO>(data["freightUpdateTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (freightUpdateTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : freightUpdateTO Found Null";
                    return returnMsg;
                }

                freightUpdateTO.CreatedBy = Convert.ToInt32(loginUserId);
                freightUpdateTO.CreatedOn = _iCommon.ServerDateTime;

                int result = _iTblFreightUpdateBL.InsertTblFreightUpdate(freightUpdateTO);
                if (result == 1)
                {
                    returnMsg.MessageType = ResultMessageE.Information;
                    returnMsg.Result = 1;
                    returnMsg.Text = "Freight Update Saved Sucessfully";
                    return returnMsg;
                }

                returnMsg.DefaultBehaviour();
                returnMsg.Text = "Error While InsertTblFreightUpdate";
                return returnMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostFreightUpdate";
                return returnMsg;
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        #endregion

        #region Put

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion

        #region Delete
        //Aniket [4-01-2019]
        [Route("PostDeleteCompetitorById")]
        [HttpPost]
        public int PostDeleteCompetitorById([FromBody] JObject data)
        {
            Int32 IdCompeUpdate = JsonConvert.DeserializeObject<Int32>(data["IdCompeUpdate"].ToString());
            return _iTblCompetitorUpdatesBL.DeleteTblCompetitorUpdates(IdCompeUpdate);
        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion

    }
}
