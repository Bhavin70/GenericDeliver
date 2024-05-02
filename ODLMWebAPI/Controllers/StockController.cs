using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.StaticStuff;
using System.Net.Http;
using System.Dynamic;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class StockController : Controller
    {
        private readonly ITblLocationBL _iTblLocationBL;
        private readonly IDimProdCatBL _iDimProdCatBL;
        private readonly IDimProdSpecBL _iDimProdSpecBL;
        private readonly ITblStockDetailsBL _iTblStockDetailsBL;
        private readonly ITblRunningSizesBL _iTblRunningSizesBL;
        private readonly ITblStockSummaryBL _iTblStockSummaryBL;
        private readonly ITblStockAsPerBooksBL _iTblStockAsPerBooksBL;
        private readonly ICommon _iCommon;
        public StockController(ICommon iCommon, ITblStockAsPerBooksBL iTblStockAsPerBooksBL, ITblStockSummaryBL iTblStockSummaryBL, ITblRunningSizesBL iTblRunningSizesBL, ITblStockDetailsBL iTblStockDetailsBL, IDimProdSpecBL iDimProdSpecBL, IDimProdCatBL iDimProdCatBL, ITblLocationBL iTblLocationBL)
        {
            _iTblLocationBL = iTblLocationBL;
            _iDimProdCatBL = iDimProdCatBL;
            _iDimProdSpecBL = iDimProdSpecBL;
            _iTblStockDetailsBL = iTblStockDetailsBL;
            _iTblRunningSizesBL = iTblRunningSizesBL;
            _iTblStockSummaryBL = iTblStockSummaryBL;
            _iTblStockAsPerBooksBL = iTblStockAsPerBooksBL;
            _iCommon = iCommon;
        }
        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetStockLocationsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetStockLocationsForDropDown()
        {
            List<TblLocationTO> tblLocationTOList = _iTblLocationBL.SelectAllParentLocation();
            if (tblLocationTOList != null && tblLocationTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblLocationTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblLocationTOList[i].LocationDesc;
                    dropDownTO.Value = tblLocationTOList[i].IdLocation;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetCompartmentsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCompartmentsForDropDown(Int32 locationId)
        {
            List<TblLocationTO> tblLocationTOList = _iTblLocationBL.SelectAllCompartmentLocationList(locationId);
            if (tblLocationTOList != null && tblLocationTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblLocationTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblLocationTOList[i].LocationDesc;
                    dropDownTO.Value = tblLocationTOList[i].IdLocation;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetProdCategoryForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetProdCategoryForDropDown()
        {
            List<DimProdCatTO> dimProdCatTOList = _iDimProdCatBL.SelectAllDimProdCatList();
        
           
            if (dimProdCatTOList != null && dimProdCatTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < dimProdCatTOList.Count; i++)
                {
                    string materialIdStr = String.Empty;
                    string specificationIdStr = String.Empty;

                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = dimProdCatTOList[i].ProdCateDesc;
                    dropDownTO.Value = dimProdCatTOList[i].IdProdCat;

                    //Aniket
                    if (!String.IsNullOrEmpty(dimProdCatTOList[i].MaterialIdStr))
                        materialIdStr = dimProdCatTOList[i].MaterialIdStr;
                    if (!String.IsNullOrEmpty(dimProdCatTOList[i].SpecificationIdStr))
                        specificationIdStr = dimProdCatTOList[i].SpecificationIdStr;
                   
                     dropDownTO.Tag = materialIdStr + "|" + specificationIdStr;
                   


                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetProdSepcificationsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetProdSepcificationsForDropDown()
        {
            List<DimProdSpecTO> dimProdSpecTOList = _iDimProdSpecBL.SelectAllDimProdSpecList();
            if (dimProdSpecTOList != null && dimProdSpecTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < dimProdSpecTOList.Count; i++)
                {

                    if (dimProdSpecTOList[i].IsActive == 1)  //Saket [2018-01-30] Added
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        dropDownTO.Text = dimProdSpecTOList[i].ProdSpecDesc;
                        dropDownTO.Value = dimProdSpecTOList[i].IdProdSpec;
                        statusReasonList.Add(dropDownTO);
                    }
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetMateAndSpecsList")]
        [HttpGet]
        public List<TblStockDetailsTO> GetMateAndSpecsList(Int32 locationId,Int32 prodCatId,DateTime stockDate,Int32 brandId,Int32 inchId,Int32 stripId)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime.Date;           
            return _iTblStockDetailsBL.SelectAllTblStockDetailsList(locationId, prodCatId, stockDate, brandId,inchId,stripId);
            
        }        
        

        [Route("GetStockDtlsByCategAndSpecs")]
        [HttpGet]
        public List<TblStockDetailsTO> GetStockDtlsByCategAndSpecs(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime.Date;

            return _iTblStockDetailsBL.SelectStockDetailsListByProdCatgAndSpec(prodCatId, prodSpecId, stockDate);

        }

        [Route("GetStockSummaryDetails")]
        [HttpGet]
        public List<SizeSpecWiseStockTO> GetStockSummaryDetails(DateTime stockDate, DateTime startDt, DateTime endDt, int compartmentId=0)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime.Date;

            // Vaibhav [21-April-2018] Added compartment filter.
            List<SizeSpecWiseStockTO>  list = _iTblStockDetailsBL.SelectSizeAndSpecWiseStockSummary(new DateTime(), startDt, endDt, compartmentId);
            //List<SizeSpecWiseStockTO>  xx=list.OrderBy(sp => sp.ProdSpecId).ToList();
            return list;

        }

        [Route("GetDatewiseRunningSizeDtls")]
        [HttpGet]
        public List<TblRunningSizesTO> GetDatewiseRunningSizeDtls(DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime.Date;

            return _iTblRunningSizesBL.SelectAllTblRunningSizesList(stockDate);

        }

        [Route("IsStockUpdateConfirmed")]
        [HttpGet]
        public Boolean IsStockUpdateConfirmed(DateTime stockDate)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime;

            TblStockSummaryTO tblStockSummaryTO = _iTblStockSummaryBL.SelectTblStockSummaryTO(new DateTime());
            if (tblStockSummaryTO != null && tblStockSummaryTO.ConfirmedOn != DateTime.MinValue)
                return true;
            else return false;
        }

        [Route("GetLastUpdatedStockDate")]
        [HttpGet]
        public String GetLastUpdatedStockDate(Int32 compartmentId,Int32 prodCatId)
        {
            return  _iTblStockSummaryBL.SelectLastStockUpdatedDateTime(compartmentId,prodCatId);
        }


        [Route("GetStockAsPerBooks")]
        [HttpGet]
        public TblStockAsPerBooksTO GetStockAsPerBooks(DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime.Date;

            return  _iTblStockAsPerBooksBL.SelectTblStockAsPerBooksTO(stockDate);
        }

        /// <summary>
        /// Sanjay [2017-05-03] To Get All the compartment whose stock for the given date is not taken
        /// </summary>
        /// <param name="stockDate"></param>
        /// <returns></returns>
        [Route("GetStockNotTakenCompartmentList")]
        [HttpGet]
        public List<TblLocationTO> GetStockNotTakenCompartmentList(DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime.Date;

            return _iTblLocationBL.SelectStkNotTakenCompartmentList(stockDate);
        }

        [Route("GetDashboardStockUpdateInfo")]
        [HttpGet]
        public ODLMWebAPI.DashboardModels.StockUpdateInfo GetDashboardStockUpdateInfo(DateTime sysDate,Int32 categoryType)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime.Date;

            return _iTblStockSummaryBL.SelectDashboardStockUpdateInfo(sysDate, categoryType);
        }

        //Aniket [20-02-2019] added to display last updated stock datetime with user name
        [Route("GetStockLastSummaryDetails")]
        [HttpGet]
        public StockSummaryTO GetStockLastSummaryDetails()
        {
            return _iTblStockSummaryBL.GetLastStockSummaryDetails();
        }

        [Route("GetTodaysStockSummaryDetails")]
        [HttpGet]
        public StockSummaryTO GetTodaysStockSummaryDetails()
        {
            return _iTblStockSummaryBL.GetTodaysStockSummaryDetails();
        }
        #endregion

        #region Post

        // POST api/values
        [Route("ResetStockDetails")]
        [HttpPost]
        public ResultMessage ResetStockDetails([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
               
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                //stockSummaryTO.CreatedOn = _iCommon.ServerDateTime;
                //stockSummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                ResultMessage resMsg = _iTblStockSummaryBL.ResetStockDetails();
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While DailyStockUpdate";
                return returnMsg;
            }
        }


        // POST api/values
        [Route("PostDailyStockUpdate")]
        [HttpPost]
        public ResultMessage PostDailyStockUpdate([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblStockSummaryTO stockSummaryTO = JsonConvert.DeserializeObject<TblStockSummaryTO>(data["stockSummaryTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
               // var isTodaysProduction = data["isTodaysProduction"].ToString();

                if (stockSummaryTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Stock Object Found Null";
                    return returnMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (stockSummaryTO.StockDetailsTOList == null || stockSummaryTO.StockDetailsTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : StockDetailsTOList Found Null";
                    return returnMsg;
                }

                stockSummaryTO.CreatedOn = _iCommon.ServerDateTime;
                stockSummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
               // stockSummaryTO.IsTodaysProduction = Convert.ToBoolean(isTodaysProduction);

                for (int i = 0; i < stockSummaryTO.StockDetailsTOList.Count; i++)
                {
                    stockSummaryTO.StockDetailsTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    stockSummaryTO.StockDetailsTOList[i].UpdatedBy = Convert.ToInt32(loginUserId);
                    stockSummaryTO.StockDetailsTOList[i].CreatedOn = stockSummaryTO.CreatedOn;
                    stockSummaryTO.StockDetailsTOList[i].UpdatedOn = stockSummaryTO.CreatedOn; 
                }

                ResultMessage resMsg = _iTblStockSummaryBL.UpdateDailyStock(stockSummaryTO);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While DailyStockUpdate";
                return returnMsg;
            }
        }

        // Add By Samadhan 10 March 2023
        [Route("ClosingStockUpdate")]
        [HttpPost]
        public ResultMessage ClosingStockUpdate()
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();

            ResultMessage resMsg  = new StaticStuff.ResultMessage();
            try
            {
                TblStockSummaryTO stockSummaryTO = new TblStockSummaryTO();
                int locationId = 1;
                List<TblLocationTO> tblLocationTOList = _iTblStockDetailsBL.SelectAllTblLocation();

                if (tblLocationTOList == null || tblLocationTOList.Count > 0)
                {
                    for (int k = 0; k < tblLocationTOList.Count; k++)
                    {
                        List<TblStockDetailsTO> tblStockDetailsTO = _iTblStockDetailsBL.SelectAllTblStockDetailsListForAutoInsert(tblLocationTOList[k].IdLocation);

                        if (tblStockDetailsTO == null || tblStockDetailsTO.Count > 0)
                        {
                            stockSummaryTO.StockDetailsTOList = tblStockDetailsTO;
                        }

                        var loginUserId = "1";

                        stockSummaryTO.CreatedOn = _iCommon.ServerDateTime;
                        stockSummaryTO.CreatedBy = Convert.ToInt32(loginUserId);

                        if (stockSummaryTO == null)
                        {
                            returnMsg.MessageType = ResultMessageE.Error;
                            returnMsg.Result = 0;
                            returnMsg.Text = "API : Stock Object Found Null";
                            return returnMsg;
                        }
                        if (Convert.ToInt32(loginUserId) <= 0)
                        {
                            returnMsg.MessageType = ResultMessageE.Error;
                            returnMsg.Result = 0;
                            returnMsg.Text = "API : UserID Found Null";
                            return returnMsg;
                        }

                        if (stockSummaryTO.StockDetailsTOList == null || stockSummaryTO.StockDetailsTOList.Count == 0)
                        {
                            returnMsg.MessageType = ResultMessageE.Error;
                            returnMsg.Result = 0;
                            returnMsg.Text = "API : StockDetailsTOList Found Null";
                            return returnMsg;
                        }

                        for (int i = 0; i < stockSummaryTO.StockDetailsTOList.Count; i++)
                        {
                            stockSummaryTO.StockDetailsTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                            stockSummaryTO.StockDetailsTOList[i].UpdatedBy = Convert.ToInt32(loginUserId);
                            stockSummaryTO.StockDetailsTOList[i].CreatedOn = stockSummaryTO.CreatedOn;
                            stockSummaryTO.StockDetailsTOList[i].UpdatedOn = stockSummaryTO.CreatedOn;                           
                        }

                        int IsRec  = _iTblStockDetailsBL.IsExistStockLocwise(Convert.ToInt32(tblLocationTOList[k].IdLocation));

                        if(IsRec > 0)
                        {
                            resMsg = _iTblStockSummaryBL.UpdateDailyStock(stockSummaryTO);
                        }


                       
                    }
                }               
               

               
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While DailyStockUpdate";
                return returnMsg;
            }
        }


        // POST api/values
        [Route("PostStockSummaryConfirmation")]
        [HttpPost]
        public ResultMessage PostStockSummaryConfirmation([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List< SizeSpecWiseStockTO> sizeSpecWiseStockTOList = JsonConvert.DeserializeObject<List<SizeSpecWiseStockTO>>(data["sizeSpecWiseStockTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

               
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (sizeSpecWiseStockTOList == null || sizeSpecWiseStockTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : sizeSpecWiseStockTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate= _iCommon.ServerDateTime;
                for (int i = 0; i < sizeSpecWiseStockTOList.Count; i++)
                {
                    sizeSpecWiseStockTOList[i].ConfirmedBy = Convert.ToInt32(loginUserId);
                    sizeSpecWiseStockTOList[i].ConfirmedOn = confirmedDate;
                }

                ResultMessage resMsg = _iTblStockSummaryBL.ConfirmStockSummary(sizeSpecWiseStockTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostStockSummaryConfirmation";
                return returnMsg;
            }
        }

        [Route("PostDailyRunningSize")]
        [HttpPost]
        public ResultMessage PostDailyRunningSize([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<TblRunningSizesTO> runningSizesTOList = JsonConvert.DeserializeObject<List<TblRunningSizesTO>>(data["runningSizesTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (runningSizesTOList == null || runningSizesTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : sizeSpecWiseStockTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = _iCommon.ServerDateTime;
                DateTime stockDate = confirmedDate.Date;

                TblStockSummaryTO tblStockSummaryTO = _iTblStockSummaryBL.SelectTblStockSummaryTO(stockDate);
                if (tblStockSummaryTO != null && tblStockSummaryTO.ConfirmedOn != DateTime.MinValue)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Stock Is Already confirmed for the Date " + stockDate.ToString(Constants.DefaultDateFormat);
                    return returnMsg;
                }

                for (int i = 0; i < runningSizesTOList.Count; i++)
                {
                    runningSizesTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    runningSizesTOList[i].CreatedOn = confirmedDate;
                }

                ResultMessage resMsg = _iTblRunningSizesBL.SaveDailyRunningSizeInfo(runningSizesTOList, stockDate);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostDailyRunningSize";
                return returnMsg;
            }
        }

        [Route("PostStockAsPerBooks")]
        [HttpPost]
        public ResultMessage PostStockAsPerBooks([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblStockAsPerBooksTO stockAsPerBooksTO = JsonConvert.DeserializeObject<TblStockAsPerBooksTO>(data["stockAsPerBooksTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (stockAsPerBooksTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Stock Object Found Null";
                    return returnMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                DateTime stockDate = _iCommon.ServerDateTime;

                TblStockSummaryTO tblStockSummaryTO = _iTblStockSummaryBL.SelectTblStockSummaryTO(stockDate);
                if (tblStockSummaryTO == null || tblStockSummaryTO.ConfirmedOn == DateTime.MinValue)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Todays Stock Is Not Confirmed Yet.";
                    return returnMsg;
                }

                stockAsPerBooksTO.CreatedOn = stockDate;
                stockAsPerBooksTO.CreatedBy = Convert.ToInt32(loginUserId);
                stockAsPerBooksTO.IsConfirmed = 1;
                stockAsPerBooksTO.StockFactor = stockAsPerBooksTO.StockInMT / tblStockSummaryTO.TotalStock;
                return _iTblStockAsPerBooksBL.SaveStockAsPerBooks(stockAsPerBooksTO);

            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostStockAsPerBooks";
                return returnMsg;
            }
        }

        [Route("RemoveRunningSizeDetails")]
        [HttpPost]
        public ResultMessage RemoveRunningSizeDetails([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblRunningSizesTO runningSizesTO = JsonConvert.DeserializeObject<TblRunningSizesTO>(data["runningSizesTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (runningSizesTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : runningSizesTO Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = _iCommon.ServerDateTime;
                DateTime stockDate = confirmedDate.Date;

                TblStockSummaryTO tblStockSummaryTO = _iTblStockSummaryBL.SelectTblStockSummaryTO(stockDate);
                if (tblStockSummaryTO != null && tblStockSummaryTO.ConfirmedOn != DateTime.MinValue)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Stock Is Already confirmed for the Date " + stockDate.ToString(Constants.DefaultDateFormat);
                    return returnMsg;
                }


                ResultMessage resMsg = _iTblRunningSizesBL.RemoveRunningSizeDtls(runningSizesTO, tblStockSummaryTO, Convert.ToInt32(loginUserId));
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostDailyRunningSize";
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion

    }
}
