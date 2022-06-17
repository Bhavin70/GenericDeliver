using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using Newtonsoft.Json;
using System.Net;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.Controllers
{
    
    [Produces("application/json")]
    [Route("api/GeoLocation")]
    public class GeoLocationController : Controller
    {
        private readonly IGeoLocationAddressBL _iGeoLocationAddressBL;
        private readonly ITblUserLocationBL _iTblUserLocationBL;
        private readonly ICommon _iCommon;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        public GeoLocationController(ICommon iCommon, IGeoLocationAddressBL iGeoLocationAddressBL, ITblUserLocationBL iTblUserLocationBL, ITblConfigParamsBL iTblConfigParamsBL)
        {
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iGeoLocationAddressBL = iGeoLocationAddressBL;
            _iTblUserLocationBL = iTblUserLocationBL;
            _iCommon = iCommon;
        }
        #region GET
        
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        ///Kiran[13-08-2018] Get Location using Lat/Lng.  
        /// </summary>
        /// <returns></returns>
        [Route("myLocationAddress")]
        [HttpGet]
        public IActionResult myLocationAddress(string lat, string logn)
        {
            string addressResponse = null;
            ResultMessage resultMessage = new ResultMessage();
           TblConfigParamsTO mapConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_MAP_MY_INDIA);
            addressResponse = _iGeoLocationAddressBL.myLocationAddress(lat, logn);
            //if(resultMessage.Tag == null)
            //{
            //    return Ok(resultMessage);
            //}
            //addressResponse = resultMessage.Tag.ToString();
           //For map my India
           GeoLocationAddressTo addressTo;
            if (mapConfigTO != null && Convert.ToInt32(mapConfigTO.ConfigParamVal) == 1)
            {
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(addressResponse);
                String addrStr = JsonConvert.SerializeObject(obj["results"][0]);
                MapMyIndiaResponse addressObj = JsonConvert.DeserializeObject<MapMyIndiaResponse>(addrStr);
                addressTo = new GeoLocationAddressTo();
                addressTo.formatted_address = addressObj.Formatted_address;
                addressTo.country = addressObj.Area;
                addressTo.administrative_area_level_1 = addressObj.State;
                addressTo.administrative_area_level_2 = addressObj.District.Split(" ")[0];
                addressTo.locality = addressObj.SubDistrict;
                addressTo.sublocality_level_1 = addressObj.Locality;
                addressTo.sublocality_level_2 = addressObj.SubLocality;
                addressTo.postal_code = addressObj.Pincode;
            }
            else
            {
                //for Google API
                GoogleGeoCodeResponse addressObj = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(addressResponse);   
                 addressTo = _iGeoLocationAddressBL.convertToProperAddress(addressObj);
            }


            if (addressTo != null)
            {
                var response = new ResponseWrapper<GeoLocationAddressTo>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Address Fetch Successfully",
                    Body = addressTo
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseWrapper<int>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "",
                    Body = 0
                };
                return NotFound(response);
            }

        }

        [Route("getAllCustomer")]
        [HttpGet]
        public IActionResult getAllCustomer(string lat, string logn)
        {
            string addressResponse = null;
            ResultMessage resultMessage = new ResultMessage();
            addressResponse = _iGeoLocationAddressBL.myLocationAddress(lat, logn);
            //if (resultMessage.Tag == null)
            //{
            //    return Ok(resultMessage);
            //}
            //addressResponse = resultMessage.Tag.ToString();
            GoogleGeoCodeResponse addressObj = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(addressResponse);
            GeoLocationAddressTo addressTo = _iGeoLocationAddressBL.convertToProperAddress(addressObj);
            if (addressTo != null)
            {
                var response = new ResponseWrapper<GeoLocationAddressTo>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Address Fetch Successfully",
                    Body = addressTo
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseWrapper<int>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "",
                    Body = 0
                };
                return NotFound(response);
            }

        }

        /// <summary>
        /// //Kiran[13-08-2018] Get Last Location 
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns>TblUserLocationTO</returns>
        [Route("getUserLastLocationListOnUserId")]
        [HttpGet]
        public List<TblUserLocationTO> getUserLastLocationListOnUserId(string userIds)
        {
            return _iTblUserLocationBL.getUserLastLocationListOnUserId(userIds);
        }

        /// <summary>
        /// Sudhir[14-AUG-2018] Added For GetPlanRoutes 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="routeTypeE"></param>
        /// <returns></returns>
        [Route("GetPlanRoutes")]
        [HttpGet]
        public IActionResult GetPlanRoutes(int userId, Constants.RouteTypeE routeTypeE)
        {
            try
            {
                List<TblUserLocationTO> list = _iTblUserLocationBL.SelectPlanRoute(userId, routeTypeE);
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


        [Route("GetRoutesOnLatLong")]
        [HttpGet]
        public IActionResult GetRoutesOnLatLong( String userLocStr)
        {
            try
            {


                TblConfigParamsTO mapConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_MAP_MY_INDIA);
                List<TblUserLocationTO> userLocList = JsonConvert.DeserializeObject< List<TblUserLocationTO>>(userLocStr);
                if (mapConfigTO == null || Convert.ToInt32(mapConfigTO.ConfigParamVal) == 0)
                {
                    return NotFound("");
                }
                String list = _iTblUserLocationBL.getMatrixAPIUsingMapMyIndia(userLocList);
                
                if (list != null)
                {
                   
                        return Ok(list);
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

        [Route("autoSearch")]
        [HttpGet]
        public IActionResult autoSerachMapMyindia(String query)
        {
            try
            {
                
                dynamic list = _iTblUserLocationBL.autoSerachMapMyindia(query);

                if (list != null)
                {
                    autoSeachTermsTO termTO = JsonConvert.DeserializeObject<autoSeachTermsTO>(list);
                    return Ok(list);
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




        //Sudhir[14-AUG-2018] Added For GetNearestDealerList 
        [Route("GetNearestDealerList")]
        [HttpGet]
        public IActionResult GetNearestDealerList(int distance, int cnfId, String userRoleTO)
        {
            try
            {
                TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
                List<nearBymeTo> list = _iTblUserLocationBL.SelectNearByDealer(distance, cnfId, tblUserRoleTO);
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


        #endregion

        #region POST
        /// <summary>
        /// //Kiran[13-08-2018] Get Last Near By customer 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="siteType"></param>
        /// <returns>TblUserLocationTO</returns>
        [Route("getNearBycustomer")]
        [HttpGet]
        public List<nearBymeTo> getNearBycustomer(int distance, int siteType, string currentLat, string currentLng, string visitDate,Int32 userId)
        {
            if (!String.IsNullOrEmpty(visitDate) && visitDate != "undefined")
            {
                DateTime tDate = Convert.ToDateTime(Convert.ToDateTime(visitDate).ToString(Constants.AzureDateFormat));
                return _iTblUserLocationBL.getNearBycustomer(distance, siteType, currentLat, currentLng, tDate, userId);
            }
            else
            {
                return _iTblUserLocationBL.getNearBycustomer(distance, siteType, currentLat, currentLng, null, userId);
            }
        }

        [Route("PostLocationDetails")]
        [HttpPost]
        public ResultMessage PostLocationDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblUserLocationTO UserLocationTO = JsonConvert.DeserializeObject<TblUserLocationTO>(data["locationDetailsTo"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                }
                if (UserLocationTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    UserLocationTO.CurTime = serverDate;
                    UserLocationTO.UserId = Convert.ToInt32(loginUserId);
                    int result = _iTblUserLocationBL.InsertTblUserLocation(UserLocationTO);
                    if (result != 1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error. Record Could Not Be Updated ";
                    }
                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Record Updated Successfully";
                    }
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostLocationDetails";
                return resultMessage;
            }
        }
       

        [Route("MigrationOfmyLocationAddress")]
        [HttpGet]
        public int MigrationOfmyLocationAddress()
        {
            string addressResponse = null;
            ResultMessage resultMessage = new ResultMessage();
            List<newdata> migrationDataList = new List<newdata>();
            migrationDataList = _iGeoLocationAddressBL.SelectAlllatlngData();
            if (migrationDataList != null)
            {
                foreach (var item in migrationDataList)
                {

                    addressResponse = _iGeoLocationAddressBL.myLocationAddress(item.Lat, item.Lng);
                    //addressResponse = resultMessage.Tag.ToString();
                    GoogleGeoCodeResponse addressObj = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(addressResponse);
                    if (addressObj.status == "OVER_QUERY_LIMIT")
                    {
                        return 0;
                    }
                    GeoLocationAddressTo addressTo = _iGeoLocationAddressBL.convertToProperAddress(addressObj);
                    if (addressTo != null)
                    {
                        _iGeoLocationAddressBL.insertAddress(addressTo, item.IdtblVisitDetails);
                    }
                }
            }
            return 1;

        }

        [Route("migrationLatlngdealerData")]
        [HttpGet]
        public int migrationLatlngdealerData()
        {
            var result = 0;
            List<TblAddressTO> migrationDataList = new List<TblAddressTO>();
            migrationDataList = _iGeoLocationAddressBL.SelectAllAddress();
            if (migrationDataList != null)
            {

                foreach (var item in migrationDataList)
                {
                    var address = "";
                    if (item.PlotNo != null)
                    {
                        address += item.PlotNo + ", ";
                    }
                    if (item.StreetName != null)
                    {
                        address += item.StreetName + ", ";
                    }
                    if (item.AreaName != null)
                    {
                        address += item.AreaName + ", ";
                    }
                    if (item.VillageName != null)
                    {
                        address += item.VillageName + ", ";
                    }
                    if (item.TalukaName != null)
                    {
                        address += item.TalukaName + ", ";
                    }
                    if (item.StateName != null)
                    {
                        address += item.StateName;
                    }
                    string addressResponse = _iGeoLocationAddressBL.myLatLngByAddress(address);
                    GoogleGeoCodeResponse addressObj = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(addressResponse);
                    if (addressObj.status == "OVER_QUERY_LIMIT" || addressObj.status == "ZERO_RESULTS")
                    {
                        //return 0;
                    }
                    else
                    {
                        GeoLocationAddressTo addressTo = _iGeoLocationAddressBL.convertToProperAddress(addressObj);
                        if (addressTo != null)
                        {
                            result = _iGeoLocationAddressBL.insertlatlong(addressTo, item.IdAddr);
                            if (result != 1)
                            {
                                return 0;
                            }
                        }
                    }

                }
                return 1;
            }
            return 0;
        }
        #endregion

    }
}