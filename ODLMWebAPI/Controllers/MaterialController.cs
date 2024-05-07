using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json;
using System.Net;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
        private readonly ITblMaterialBL _iTblMaterialBL;
        private readonly ITblProductInfoBL _iTblProductInfoBL;
        private readonly ITblParitySummaryBL _iTblParitySummaryBL; 
        private readonly ITblParityDetailsBL _iTblParityDetailsBL; 
        private readonly ITblProdClassificationBL _iTblProdClassificationBL; 
        private readonly ITblProductItemBL _iTblProductItemBL; 
        private readonly IDimensionBL _iDimensionBL;// working
        private readonly ICommon _iCommon;
        public MaterialController(IDimensionBL iDimensionBL, ITblProductItemBL iTblProductItemBL, ITblProdClassificationBL iTblProdClassificationBL, ITblParityDetailsBL iTblParityDetailsBL, ICommon iCommon, ITblMaterialBL iTblMaterialBL, ITblProductInfoBL iTblProductInfoBL, ITblParitySummaryBL iTblParitySummaryBL)
        {
            _iTblMaterialBL = iTblMaterialBL;
            _iTblProductInfoBL = iTblProductInfoBL;
            _iTblParitySummaryBL = iTblParitySummaryBL;
            _iTblParityDetailsBL = iTblParityDetailsBL;
            _iTblProdClassificationBL = iTblProdClassificationBL;
            _iTblProductItemBL = iTblProductItemBL;
            _iDimensionBL = iDimensionBL;
            _iCommon = iCommon;
        }

        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetMaterialDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetMaterialDropDownList()
        {
            return _iTblMaterialBL.SelectAllMaterialListForDropDown();
        }

        [Route("GetProductAndSpecsList")]
        [HttpGet]
        public List<TblProductInfoTO> GetProductAndSpecsList(Int32 prodCatId)
        {
            return _iTblProductInfoBL.SelectAllEmptyProductInfoList(prodCatId);

        }

        [Route("GetMaterialList")]
        [HttpGet]
        public List<TblMaterialTO> GetMaterialList()
        {
            return _iTblMaterialBL.SelectAllTblMaterialList();
        }

        /// <summary>
        /// Sanjay [2017-04-25] If parityId=0 then will return latest parity details if exist 
        /// and if parityId <>0 then parity details of given parityId
        /// </summary>
        /// <param name="parityId"></param>
        /// <param name="prodSpecId"> Added On 24/07/2017. After discussion with Nitin K [Meeting Ref. 21/07/2017 Pune] parity will be against prod Spec also.</param>
        /// <returns></returns>
        [Route("GetParityDetails")]
        [HttpGet]
        public TblParitySummaryTO GetParityDetails(Int32 stateId, Int32 prodSpecId = 0,Int32 brandId=0)
        {
            TblParitySummaryTO latestParityTO = _iTblParitySummaryBL.SelectStatesActiveParitySummaryTO(stateId,brandId);
            int parityId = 0;
            if (latestParityTO == null)
            {
                latestParityTO = new TblParitySummaryTO();

            }
            else
            {
                parityId = latestParityTO.IdParity;
            }

            //Sanjay [2017-06-25] Changes as Statewsie latest all spec wise needs to show
            //List<TblParityDetailsTO> list = _iTblParityDetailsBL.SelectAllTblParityDetailsList(parityId, prodSpecId, stateId);
            List<TblParityDetailsTO> list = null;
            if (list == null || list.Count == 0)
            {
                list= _iTblParityDetailsBL.SelectAllEmptyParityDetailsList(prodSpecId, stateId,brandId);
                list = list.OrderBy(a => a.ProdCatId).ThenBy(a=>a.MaterialId).ToList();
                latestParityTO.ParityDetailList = list;
            }
            else
                latestParityTO.ParityDetailList = list;
            return latestParityTO;
        }

        [Route("GetProductClassificationList")]
        [HttpGet]
        public List<TblProdClassificationTO> GetProductClassificationList(string prodClassType = "")
        {
            return _iTblProdClassificationBL.SelectAllTblProdClassificationList(prodClassType);
        }

        [Route("GetProdClassesForDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetProdClassesForDropDownList(Int32 parentClassId = 0)
        {
            return _iTblProdClassificationBL.SelectAllProdClassificationForDropDown(parentClassId);
        }

        //@ Hudekar Priyanka [04-march-2019]
        [Route("GetProductItemPurchaseData")]
        [HttpGet]
        public TblPurchaseItemMasterTO GetProductItemPurchaseData(Int32 prodItemId)
        {
            return _iTblProductItemBL.SelectTblPurchaseItemMasterTO(prodItemId);
        }

        [Route("GetProductClassificationDetails")]
        [HttpGet]
        public TblProdClassificationTO GetProductClassificationDetails(Int32 idProdClass)
        {
            return _iTblProdClassificationBL.SelectTblProdClassificationTO(idProdClass);
        }

        [Route("GetProductItemList")]
        [HttpGet]
        public List<TblProductItemTO> GetProductItemList(Int32 specificationId = 0)
        {
            return _iTblProductItemBL.SelectAllTblProductItemList(specificationId);
        }
         

        [Route("GetProductItemDetails")]
        [HttpGet]
        public TblProductItemTO GetProductItemDetails(Int32 idProdItem)
        {
            return _iTblProductItemBL.SelectTblProductItemTO(idProdItem);
        }

        /// <summary>
        /// GJ@20170818 : Get the Prouct Master Info List by LoadingSlipExt Ids for Bundles calculation
        /// </summary>
        /// <param name="strLoadingSlipExtIds"></param>
        /// <param name="strLoadingSlipExtIds">Added to know the Loading Slip Ext Ids</param>
        /// <returns></returns>
        [Route("GetProductSpecificationListByLoadingSlipExtIds")]
        [HttpGet]
        public List<TblProductInfoTO> GetProductSpecificationListByLoadingSlipExtIds(string strLoadingSlipExtIds)
        {
            return _iTblProductInfoBL.SelectProductInfoListByLoadingSlipExtIds(strLoadingSlipExtIds);
        }

        /// <summary>
        /// Saket [2017-01-17] Added.
        /// </summary>
        /// <returns></returns>
        [Route("GetAllProductSpecificationList")]
        [HttpGet]
        public List<TblProductInfoTO> GetAllProductSpecificationList()
        {
            return _iTblProductInfoBL.SelectAllTblProductInfoListLatest();
        }
        /// <summary>
        /// Vijaymala[12-09-2017] Added To Get Material Type List
        /// </summary>
        /// <returns></returns>
        [Route("GetMaterialTypeDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetMaterialTypeDropDownList()
        {
            return _iTblMaterialBL.SelectMaterialTypeDropDownList();
        }


        /// <summary>
        /// Sanjay [2018-02-19] To Show all Item Prod Catg List
        /// </summary>
        /// <returns></returns>
        /// <remarks>Retrives All Item Product Categories for e.g. FG,Scrap,Service Items etc</remarks>
        [Route("GetItemProductCategoryList")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetItemProductCategoryList()
        {
            try
            {
                List<DropDownTO> list = _iDimensionBL.GetItemProductCategoryListForDropDown();
                if (list != null)
                {
                    if (list.Count == 0)
                        return NoContent();
                    return Ok(list);
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (System.Exception exc)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Sanjay [2018-02-19] To Retrive All the Product Classfication List By Item Product Category Enum
        /// </summary>
        /// <param name="itemProdCategoryE"></param>
        /// <returns></returns>
        [Route("GetProductClassListByItemCatg")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblProdClassificationTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetProductClassListByItemCatg(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            try
            {
                List<TblProdClassificationTO> list = _iTblProdClassificationBL.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);

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
            catch (System.Exception exc)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        //Get GetDefaultProductClassification List
        [Route("GetDefaultProductClassification")]
        [HttpGet]
        public List<Int32> GetDefaultProductClassification()
        {
            return _iTblProdClassificationBL.GetDefaultProductClassification();
        }

        /// <summary>
        ///Sudhir[15-Mar-2018] Added for Get ProductItem List Based Product Classification Id.  
        /// </summary>
        /// <param name="idProdClass"></param>
        /// <returns></returns>
        [Route("GetProductItemByProdClass")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblProductItemTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetProductItemByProdClass(Int32 idProdClass)
        {
            try
            {
                List<TblProductItemTO> productItemList = _iTblProductItemBL.SelectProductItemList(idProdClass);
                if (productItemList != null)
                {
                    if (productItemList.Count > 0)
                        return Ok(productItemList);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(productItemList);
                }

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        ///Sudhir[20-Mar-2018] Added for Get ParityDetails List
        /// </summary>
        /// <param name="idProdClass"></param>
        /// <returns></returns>
        //[Route("GetParityDetailsList")]
        //[HttpGet]
        //public List<TblParityDetailsTO> GetParityDetailsList(Int32 productItemId, Int32 brandId, Int32 prodCatId, Int32 prodSpecId, Int32 materialId)
        //{
        //    try
        //    {
        //        List<TblParityDetailsTO> list = _iTblParityDetailsBL.SelectAllParityDetailsOnProductItemId(productItemId, brandId, prodCatId, prodSpecId, materialId);
        //        if (list != null)
        //            return list;
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        /// <summary>
        /// Priyanka [29-08-18] : Added for Get ParityDetails List.  
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="productItemId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="stateId"></param>
        /// <param name="currencyId"></param>
        /// <param name="productSpecInfoListTo"></param>
        /// <param name="districtId"></param>
        /// <param name="talukaId"></param>
        /// <returns></returns>
        [Route("GetParityDetailsList")]
        [HttpGet]
        public List<TblParityDetailsTO> GetParityDetailsList(Int32 brandId, Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo = 0, Int32 productSpecForRegular = 0, Int32 districtId = 0, Int32 talukaId = 0)
        {
            try
            {
                List<TblParityDetailsTO> list = _iTblParityDetailsBL.SelectAllParityDetailsOnProductItemId(brandId, productItemId, prodCatId, stateId, currencyId, productSpecInfoListTo, productSpecForRegular, districtId, talukaId);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("GetParityDetailToOnBooking")]
        [HttpGet]
        public List<TblParityDetailsTO> GetParityDetailToOnBooking(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 brandId, Int32 stateId, DateTime boookingDate, Int32 districtId, Int32 talukaId, Int32 parityLevel)
        {
            boookingDate = _iCommon.ServerDateTime;
            
            List<TblParityDetailsTO> TblParityDetailsTOList = _iTblParityDetailsBL.GetCurrentParityDetailToListOnBooking(materialId, prodCatId, prodSpecId, productItemId, brandId, stateId, boookingDate, districtId, talukaId, parityLevel);
            return TblParityDetailsTOList;
        }
        // Aniket [21-Jan-2019] added to fetch ParityDetailsList against brand
        //[Route("GetParityDetailsListFromBrand")]
        //[HttpGet] 
        //public List<TblParityDetailsTO> GetParityDetailsListFromBrand(Int32 fromBrand,Int32 toBrand,Int32 currencyId,Int32 categoryId, Int32 stateId)
        //{
        //    try
        //    {
        //        List<TblParityDetailsTO> list = BL.TblParityDetailsBL.SelectBrandForCopy(fromBrand, toBrand, currencyId, categoryId, stateId);
        //         if (list != null)
        //            return list;
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        ///Vijaymala[29-08-2018]added to get productr classification list by using item id
        /// </summary>

        /// <param name="productItemId">Added to know the Loading Slip Ext Ids</param>
        /// <returns></returns>
        [Route("GetProductClassificationListByProductItem")]
        [HttpGet]
        public List<TblProdClassificationTO> GetProductClassificationListByProductItem(Int32 productItemId)
        {
            return _iTblProdClassificationBL.SelectProductClassificationListByProductItemId(productItemId);
        }

        [Route("GetNoOfPcesAndQtyAginsCatagory")]
        [HttpGet]
        public TblProductInfoTO GetNoOfPcesAndQtyAginsCatagory(int CategoryType = 1)
        {
            return _iTblProductInfoBL.GetNoOfPcesAndQtyAginsCatagory(CategoryType);
        }
        #endregion

        #region Post

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("PostProductInformation")]
        [HttpPost]
        public ResultMessage PostProductInformation([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblProductInfoTO> productInfoTOList = JsonConvert.DeserializeObject<List<TblProductInfoTO>>(data["productInfoTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (productInfoTOList == null || productInfoTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : productInfoTOList Found NULL";
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                for (int i = 0; i < productInfoTOList.Count; i++)
                {
                    productInfoTOList[i].CreatedBy = Convert.ToInt32( loginUserId);
                    productInfoTOList[i].CreatedOn = createdDate;
                }
                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblProductInfoBL.SaveProductInformation(productInfoTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostProductInformation";
                return resultMessage;
            }
        }


        /// <summary>
        /// Sanjay [2017-04-21] To Save Material Sizewise Parity Details
        /// Will Deactivate all Prev Parity Details and Inserts New Parity Details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostParityDetails")]
        [HttpPost]
        public ResultMessage PostParityDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblParitySummaryTO paritySummaryTO = JsonConvert.DeserializeObject<TblParitySummaryTO>(data["parityTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }
               
                    if (paritySummaryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : paritySummaryTO Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO.ParityDetailList == null || paritySummaryTO.ParityDetailList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : ParityDetailList Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO.StateId <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Selected State Not Found";
                    resultMessage.DisplayMessage = "Records could not be updated ";
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                paritySummaryTO.CreatedOn = createdDate;
                paritySummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                paritySummaryTO.IsActive = 1;

                return _iTblParitySummaryBL.SaveParitySettings(paritySummaryTO);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostParityDetails";
                return resultMessage;
            }
        }

        [Route("PostNewProductClassification")]
        [HttpPost]
        public ResultMessage PostNewProductClassification([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProdClassificationTO prodClassificationTO = JsonConvert.DeserializeObject<TblProdClassificationTO>(data["prodClassificationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (prodClassificationTO == null)
                {
                    resultMessage.DefaultBehaviour("API : prodClassificationTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                prodClassificationTO.CreatedOn = createdDate;
                prodClassificationTO.CreatedBy = Convert.ToInt32(loginUserId);
                prodClassificationTO.IsActive = 1;
                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProdClassificationBL.InsertProdClassification(prodClassificationTO);
                if(result==1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While InsertTblProdClassification");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewProductClassification");
                return resultMessage;
            }
        }

        [Route("PostUpdateProductClassification")]
        [HttpPost]
        public ResultMessage PostUpdateProductClassification([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProdClassificationTO prodClassificationTO = JsonConvert.DeserializeObject<TblProdClassificationTO>(data["prodClassificationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (prodClassificationTO == null)
                {
                    resultMessage.DefaultBehaviour("API : prodClassificationTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                prodClassificationTO.UpdatedOn = createdDate;
                prodClassificationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProdClassificationBL.UpdateProdClassification(prodClassificationTO);
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While UpdateTblProdClassification");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateProductClassification");
                return resultMessage;
            }
        }

        [Route("PostNewProductItem")]
        [HttpPost]
        public ResultMessage PostNewProductItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProductItemTO productItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["productItemTO"].ToString());
                TblPurchaseItemMasterTO purchaseItemMasterTO = JsonConvert.DeserializeObject<TblPurchaseItemMasterTO>(data["tblPurchaseItemMasterTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (productItemTO == null)
                {
                    resultMessage.DefaultBehaviour("API : productItemTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                productItemTO.CreatedOn = createdDate;
                productItemTO.CreatedBy = Convert.ToInt32(loginUserId);

               // purchaseItemMasterTO.UpdatedBy = Convert.ToInt32(loginUserId);
              //  purchaseItemMasterTO.UpdatedOn = _iCommon.ServerDateTime;
                productItemTO.IsActive = 1;

                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProductItemBL.InsertTblProductItem(productItemTO, purchaseItemMasterTO);
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While InsertTblProductItem");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewProductItem");
                return resultMessage;
            }
        }

        [Route("PostUpdateProductItem")]
        [HttpPost]
        public ResultMessage PostUpdateProductItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProductItemTO productItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["productItemTO"].ToString());
                TblPurchaseItemMasterTO purchaseItemMasterTO = JsonConvert.DeserializeObject<TblPurchaseItemMasterTO>(data["tblPurchaseItemMasterTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                //if (productItemTO == null)
                //{
                //    resultMessage.DefaultBehaviour("API : productItemTO Found NULL");
                //    return resultMessage;
                //}

                DateTime createdDate = _iCommon.ServerDateTime;
                productItemTO.UpdatedOn = createdDate;
                productItemTO.UpdatedBy = Convert.ToInt32(loginUserId);
              //  productItemTO.CreatedBy = Convert.ToInt32(loginUserId);
                purchaseItemMasterTO.UpdatedOn = createdDate;
                purchaseItemMasterTO.UpdatedBy = Convert.ToInt32(loginUserId);
                purchaseItemMasterTO.CreatedBy = Convert.ToInt32(loginUserId);
                purchaseItemMasterTO.CreatedOn = createdDate;


                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProductItemBL.UpdateTblProductItem(productItemTO, purchaseItemMasterTO);
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While UpdateTblProductItem");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateProductItem");
                return resultMessage;
            }
        }
        /// <summary>
        /// Vijaymala[12-09-2017] Added To save Material Size
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewMaterial")]
        [HttpPost]
        public ResultMessage PostNewMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                 TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null )
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }
                    //tblMaterialTO.MateCompOrgId = 19;
                   // tblMaterialTO.MateSubCompOrgId = 20;
                    tblMaterialTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblMaterialTO.CreatedOn = _iCommon.ServerDateTime;
                    tblMaterialTO.IsActive = 1;

                    int result = _iTblMaterialBL.InsertTblMaterial(tblMaterialTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewMaterial");
                return resultMessage;
            }
           
        }

        /// <summary>
        /// Vijaymala[12-09-2017] Added To Update Material Size
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateMaterial")]
        [HttpPost]
        public ResultMessage PostUpdateMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null)
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO != null )
                {
                    tblMaterialTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblMaterialTO.UpdatedOn = _iCommon.ServerDateTime;

                    result = _iTblMaterialBL.UpdateTblMaterial(tblMaterialTO);
                     if (result != 1)
                      {
                           resultMessage.DefaultBehaviour("Error... Record could not be updated");
                           return resultMessage;
                      }
                   
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateMaterial");
                return resultMessage;
            }

        }


        /// <summary>
        /// Vijaymala[12-09-2017] Added To Update Material Size
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeactivateMaterial")]
        [HttpPost]
        public ResultMessage PostDeactivateMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null)
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO != null)
                {
                    tblMaterialTO.DeactivatedBy = Convert.ToInt32(loginUserId);
                    tblMaterialTO.DeactivatedOn= _iCommon.ServerDateTime;
                    tblMaterialTO.IsActive = 0;
                    result = _iTblMaterialBL.UpdateTblMaterial(tblMaterialTO);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                        return resultMessage;
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateMaterial");
                return resultMessage;
            }

        }


        /// <summary>
        ///  Priyanka [21-02-2018] : Added to Deactivate the Category from product classification list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostDeactivateCategory")]
        [HttpPost]
        public ResultMessage PostDeactivateCategory([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblProdClassificationTO tblprodClassificationTO = JsonConvert.DeserializeObject<TblProdClassificationTO>(data["prodClassificationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblprodClassificationTO == null)
                {
                    resultMessage.DefaultBehaviour("tblprodClassificationTO Found NULL");
                    return resultMessage;
                }

                if (tblprodClassificationTO != null)
                {
                    tblprodClassificationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblprodClassificationTO.UpdatedOn = _iCommon.ServerDateTime;
                  
                    tblprodClassificationTO.IsActive = 0;
                   
                    result = _iTblProdClassificationBL.UpdateProdClassification(tblprodClassificationTO);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                        return resultMessage;
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateCategory");
                return resultMessage;
            }

        }

        /// <summary>
        ///  Priyanka [22-02-2018] : Added to Deactivate Item/Product in Product Classification.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostDeactivateItemOrProduct")]
        [HttpPost]
        public ResultMessage PostDeactivateItemOrProduct([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblProductItemTO tblprodItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["prodItemTO"].ToString());
             //   TblPurchaseItemMasterTO purchaseItemMasterTO = JsonConvert.DeserializeObject<TblPurchaseItemMasterTO>(data["tblPurchaseItemMasterTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblprodItemTO == null)
                {
                    resultMessage.DefaultBehaviour("tblprodItemTO Found NULL");
                    return resultMessage;
                }
                //if (purchaseItemMasterTO == null)
                //{
                //    resultMessage.DefaultBehaviour("purchaseItemMasterTO Found NULL");
                //    return resultMessage;
                //}

                if (tblprodItemTO != null)
                {
                    tblprodItemTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblprodItemTO.UpdatedOn = _iCommon.ServerDateTime;

                    tblprodItemTO.IsActive = 0;
                  //  purchaseItemMasterTO.IsActive = 0;

                    TblPurchaseItemMasterTO purchaseItemMaster = new TblPurchaseItemMasterTO();

                    result = _iTblProductItemBL.UpdateTblProductItem(tblprodItemTO, purchaseItemMaster);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                        return resultMessage;
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateItemOrProduct");
                return resultMessage;
            }

        }

        //Aniket [29-01-2019] added to copy parity values from one brand to multiple brands
        [Route("PostCopyParityValuesForMultiBrands")]
        [HttpPost]
        public ResultMessage PostCopyParityValuesForMultiBrands([FromBody] JObject data)
        {
            int brandId = JsonConvert.DeserializeObject<Int32>(data["brandId"].ToString());
            List<DropDownToForParity> selectedBrands = JsonConvert.DeserializeObject<List<DropDownToForParity>>(data["selectedBrands"].ToString());
            List<DropDownToForParity> selectedStates = JsonConvert.DeserializeObject<List<DropDownToForParity>>(data["selectedStates"].ToString());
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage = _iTblParityDetailsBL.GetParityDetialsForCopyBrand(brandId, selectedBrands, selectedStates);
            return resultMessage;

        }

        /// <summary>
        /// Sudhir[21-MARCH-2018] Added  for Insert Parity Details New Req. Data insert only in tblParityDetails.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("SaveParityDetailsOtherItem")]
        [HttpPost]
        public ResultMessage SaveParityDetailsOtherItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblParitySummaryTO paritySummaryTO = JsonConvert.DeserializeObject<TblParitySummaryTO>(data["parityTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : paritySummaryTO Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO.ParityDetailList == null || paritySummaryTO.ParityDetailList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : ParityDetailList Found NULL";
                    return resultMessage;
                }

                Int32 isForUpdate= (Convert.ToInt32(data["isForUpdate"].ToString()));

                //if (paritySummaryTO.StateId <= 0)
                //{
                //    resultMessage.DefaultBehaviour();
                //    resultMessage.Text = "API : Selected State Not Found";
                //    resultMessage.DisplayMessage = "Records could not be updated ";
                //    return resultMessage;
                //}

                DateTime createdDate = _iCommon.ServerDateTime;
                paritySummaryTO.CreatedOn = createdDate;
                paritySummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                paritySummaryTO.IsActive = 1;

                return _iTblParityDetailsBL.SaveParityDetailsOtherItem(paritySummaryTO, isForUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostParityDetails";
                return resultMessage;
            }
        }

        //@ Hudekar Priyanka [04-march-19]
        [Route("PostPurchaseItemMaster")]
        [HttpPost]
        public ResultMessage PostPurchaseItemMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblPurchaseItemMasterTO purchaseItemMasterTO = JsonConvert.DeserializeObject<TblPurchaseItemMasterTO>(data["generalItemMaster"].ToString());
                var loginUserId = data["loginId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (purchaseItemMasterTO == null)
                {
                    resultMessage.DefaultBehaviour("API :purchaseItemMasterTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                purchaseItemMasterTO.CreatedOn = createdDate;
                purchaseItemMasterTO.CreatedBy = Convert.ToInt32(loginUserId);

                purchaseItemMasterTO.UpdatedBy = Convert.ToInt32(loginUserId);
                purchaseItemMasterTO.UpdatedOn = _iCommon.ServerDateTime;

                purchaseItemMasterTO.IsActive = 1;
                ResultMessage rMessage = new ResultMessage();
                 //int result = BL.TblProductItemBL.InsertPurchaseItemMaster(purchaseItemMasterTO);
                  int result = _iTblProductItemBL.InsertTblPurchaseItemMaster(purchaseItemMasterTO);
                 //  int result = iTblProductItemBL.InsertTblPurchaseItemMaster(purchaseItemMasterTO); //using interface
                   if (result == 1)
                   {
                       rMessage.DefaultSuccessBehaviour();
                   }
                   else
                   {
                       rMessage.DefaultBehaviour("Error While InsertPurchaseItemMasterTO");
                   }
                    
                return rMessage;
               
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostPurchaseItemMaster");
                return resultMessage;
            }
        }
        //@  Hudekar Priyanka [04-march-2019]
        [Route("PostUpdatePurchaseItemMaster")]
        [HttpPost]
        public ResultMessage PostUpdatePurchaseItemMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblPurchaseItemMasterTO purchaseItemMasterTO = JsonConvert.DeserializeObject<TblPurchaseItemMasterTO>(data["generalItemMaster"].ToString());
                var loginUserId = data["loginId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (purchaseItemMasterTO == null)
                {
                    resultMessage.DefaultBehaviour("API : generalItemMaster Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                purchaseItemMasterTO.UpdatedOn = createdDate;
                purchaseItemMasterTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProductItemBL.UpdateTblPurchaseItemMasterTO(purchaseItemMasterTO);
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While UpdateTblProductItem");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdatePurchaseItemMaster");
                return resultMessage;
            }
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