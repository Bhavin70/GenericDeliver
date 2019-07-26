
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.BL
{ 
    public class TblUserLocationBL : ITblUserLocationBL
    {
        #region Selection
        private readonly ITblUserLocationDAO _iTblUserLocationDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblOrgStructureBL _iTblOrgStructureBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        public TblUserLocationBL(ITblUserLocationDAO iTblUserLocationDAO, ITblConfigParamsDAO iTblConfigParamsDAO, ITblOrgStructureBL iTblOrgStructureBL, ITblConfigParamsBL iTblConfigParamsBL)
        {
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblUserLocationDAO = iTblUserLocationDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblOrgStructureBL = iTblOrgStructureBL;

        }
        public TblUserLocationTO SelectAllTblUserLocation()
        {
            return _iTblUserLocationDAO.SelectTblUserLocation(); 
        }

        public List<TblUserLocationTO> SelectAllTblUserLocationList()
        {
            return _iTblUserLocationDAO.SelectAllTblUserLocation();
        }

        public TblUserLocationTO SelectTblUserLocationTO()
        {
            return _iTblUserLocationDAO.SelectTblUserLocation();
        }

        public List<TblUserLocationTO> getUserLastLocationListOnUserId(string userIds)
        {
            return _iTblUserLocationDAO.UserLastLocationListOnUserId(userIds);
        }

        /// <summary>
        /// Sudhir[14-AUG-2018] This Method is For Get Result For Actual Route Or Suggested Route.
        /// </summary>
        /// <param name="tblUserLocationTO"></param>
        /// <returns></returns>
        /// 
        public List<TblUserLocationTO> SelectPlanRoute(Int32 UserId, StaticStuff.Constants.RouteTypeE RouteTypeE)
        {
            try
            {
                TblConfigParamsTO mapConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_MAP_MY_INDIA);

                if (RouteTypeE == StaticStuff.Constants.RouteTypeE.ACTUAL)
                {

                    List<TblUserLocationTO> list = _iTblUserLocationDAO.SelectActualPlanRoute(UserId);
                    long estimatedTimeInSec = 0;
                    long distance = 0;

                    if (mapConfigTO == null || Convert.ToInt32(mapConfigTO.ConfigParamVal) == 0)
                    {
                      //  using Gmap
                       dynamic obj = getMatrixAPI(list);
                        JObject matrix = JsonConvert.DeserializeObject<JObject>(obj.ToString());
                        String rowsString = JsonConvert.SerializeObject(matrix["rows"]);
                        List<gmapRowTO> matrixData = JsonConvert.DeserializeObject<List<gmapRowTO>>(rowsString);
                        List<gmapElementsTO> travelRoute = new List<gmapElementsTO>();
                        for (int i = 0; i < matrixData.Count; i++)

                        {
                            for (int j = 0; j < matrixData[i].Elements.Count; j++)

                            {
                                if (i == j)
                                {
                                    travelRoute.Add(matrixData[i].Elements[j]);
                                }

                            }
                        }
                        travelRoute.ForEach(e =>
                        {
                            estimatedTimeInSec = estimatedTimeInSec + e.Duration.Value;

                            distance = distance + e.Distance.Value;

                        });
                     //   End
                    }

                    //using Map My India
                    else
                    { 
                    dynamic obj = getMatrixAPIUsingMapMyIndia(list);
                    JObject matrix = JsonConvert.DeserializeObject<JObject>(obj.ToString());
                    estimatedTimeInSec = (long)matrix["routes"][0]["distance"];
                    //End



                

                }


                    TimeSpan t1 = list[list.Count - 1].CurTime - list[0].CurTime;
                    string ActualTime = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                    t1.Hours,
                    t1.Minutes,
                    t1.Seconds,
                    t1.Milliseconds);

                    TimeSpan t = TimeSpan.FromSeconds(estimatedTimeInSec);
                    string EstimatedTime = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                t.Hours,
                t.Minutes,
                t.Seconds,
                t.Milliseconds);
                    list[0].EstimatedTime = EstimatedTime;
                    list[0].ActualTime = ActualTime;
                    //end
                    return list;
                }
                else if (RouteTypeE == StaticStuff.Constants.RouteTypeE.SUGGESTED)
                {
                    List<TblUserLocationTO> list= _iTblUserLocationDAO.SelectSuggestedPlanRoute(UserId);
                    long estimatedTimeInSec = 0;
                    long distance = 0;
                    if (mapConfigTO == null || Convert.ToInt32(mapConfigTO.ConfigParamVal) == 0)
                    {

                        //using Gmap
                        dynamic obj1 = getMatrixAPI(list);
                        JObject matrix1 = JsonConvert.DeserializeObject<JObject>(obj1.ToString());
                        String rowsString = JsonConvert.SerializeObject(matrix1["rows"]);
                        List<gmapRowTO> matrixData = JsonConvert.DeserializeObject<List<gmapRowTO>>(rowsString);
                        List<gmapElementsTO> travelRoute = new List<gmapElementsTO>();
                        for (int i = 0; i < matrixData.Count; i++)

                        {
                            for (int j = 0; j < matrixData[i].Elements.Count; j++)

                            {
                                if (i == j)
                                {
                                    travelRoute.Add(matrixData[i].Elements[j]);
                                }

                            }
                        }

                        travelRoute.ForEach(e =>
                        {
                            estimatedTimeInSec = estimatedTimeInSec + e.Duration.Value;

                            distance = distance + e.Distance.Value;

                        });
                        //end
                    }
                    else
                    { 
                        //using Map My India
                        dynamic obj = getMatrixAPIUsingMapMyIndia(list);
                    JObject matrix = JsonConvert.DeserializeObject<JObject>(obj.ToString());
                    estimatedTimeInSec = (long)matrix["routes"][0]["duration"];
                    //End
                    }


                    TimeSpan t = TimeSpan.FromSeconds(estimatedTimeInSec);
                    string EstimatedTime = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                    t.Hours,
                    t.Minutes,
                    t.Seconds,
                    t.Milliseconds);

                    TimeSpan t1 = list[list.Count - 1].CurTime - list[0].CurTime;
                    string ActualTime = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                t1.Hours,
                t1.Minutes,
                t1.Seconds,
                t1.Milliseconds);
                    list[0].EstimatedTime = EstimatedTime;
                    list[0].ActualTime = ActualTime;
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }   

        }

        public dynamic getTokenMapMyIndia()
        {

            String authApiKey = "https://outpost.mapmyindia.com/api/security/oauth/token?grant_type=client_credentials&client_id=FQdaSX8vsUfS6F6eaqpJRXSQWzWk2h_mfaotetDcC4FNZXomzeh5wZjEC0m-iv3Ij1KSd0I0U1zbOkqtyJWDOg==&client_secret=BiaZHfef9y0zN9_X0JT_HyTuy2WAABG7rntYFzlPL7kkB0YED5qAtWIEasrz2wsavoC91Nde2bTxsxin4YzgDw==";
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(authApiKey);
            String result;
            objRequest.Method = "POST";
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                Stream aa = objRequest.GetRequestStreamAsync().Result;
                myWriter = new StreamWriter(aa);
                myWriter.Write(aa);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                myWriter.Dispose();
            }

            WebResponse objResponse = objRequest.GetResponseAsync().Result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;

        }

        public dynamic autoSerachMapMyindia(String query)
        {
            String ApiKey = "https://atlas.mapmyindia.com/api/places/search/json?query="+query+ "&location=18.5204,73.8567&pod = CITY";
            dynamic tokenDtls =getTokenMapMyIndia();
            authResponse accessTO = JsonConvert.DeserializeObject<authResponse>(tokenDtls);
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(ApiKey);
            String result;
            objRequest.Method = "GET";
          //  objRequest.ContentType = "application/x-www-form-urlencoded";

            objRequest.Headers.Add(HttpRequestHeader.Authorization,"Bearer"+" "+accessTO.Access_token);
            WebResponse objResponse = objRequest.GetResponseAsync().Result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        public dynamic getMatrixAPIUsingMapMyIndia(List<TblUserLocationTO> userLocList)
        {
            String key = "x6a9yupqaxh2ppibyu7847wwfdj8fn9p";
            String latlongStr = "";
            userLocList.ForEach(ele =>
            {
                if (String.IsNullOrEmpty(latlongStr))
                {
                    latlongStr = ele.Longitude + "," + ele.Latitude;
                }
                else
                {
                    latlongStr = latlongStr +";" + ele.Longitude + "," + ele.Latitude;
                }

            });

            String apiKey = "https://apis.mapmyindia.com/advancedmaps/v1/"+key+"/route_adv/driving/"+ latlongStr+ "?alternatives="+3;
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(apiKey);
            String result;
            objRequest.Method = "POST";
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                Stream aa = objRequest.GetRequestStreamAsync().Result;
                myWriter = new StreamWriter(aa);
                myWriter.Write(aa);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                myWriter.Dispose();
            }

            WebResponse objResponse = objRequest.GetResponseAsync().Result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }


        public dynamic getMatrixAPI(List<TblUserLocationTO> userLocList)
        {
            List<TblUserLocationTO> destinations = new List<TblUserLocationTO>();
            destinations.AddRange(userLocList);
            List<TblUserLocationTO> origin = new List<TblUserLocationTO>();
            origin.AddRange(userLocList);
            String key = "AIzaSyCkLbSDnkG5FxxMMTFwaBzs9JticPPMsRM";
            destinations.RemoveAt(0);
            origin.RemoveAt(origin.Count-1);
            String originKey = null;
            if (origin !=null && origin.Count > 0)
            {
                origin.ForEach(element => {
                    if (originKey == null)
                        originKey = element.Latitude + ',' + element.Longitude;
                    else
                        originKey = originKey + '|' + element.Latitude + ',' + element.Longitude;
                });
            }
            String destKey = null;
    if (destinations !=null && destinations.Count > 0)
            {
                destinations.ForEach(element => {
                    if (destKey == null)
                        destKey = element.Latitude + ',' + element.Longitude;
                    else
                        destKey = destKey + '|' + element.Latitude + ',' + element.Longitude;
                });
            }
            String apiKey = "https://maps.googleapis.com/maps/api/distancematrix/json?origins="
    + originKey + "&destinations=" + destKey + "&key="+ key;
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(apiKey);
            String result;
            objRequest.Method = "POST";
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                Stream aa = objRequest.GetRequestStreamAsync().Result;
                myWriter = new StreamWriter(aa);
                myWriter.Write(aa);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                myWriter.Dispose();
            }

            WebResponse objResponse = objRequest.GetResponseAsync().Result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }
        public List<nearBymeTo> getNearBycustomer(int distance, int siteType, string currentLat, string currentLng, DateTime? visitDate,Int32 userId)
        {
            List<int> userIds = null;
            string useridStr = "";
            useridStr = "" + userId;
           userIds = (List<int>)_iTblOrgStructureBL.ChildUserListOnUserId(userId, 1, 1);
            if(userIds !=null && userIds.Count>0)
            userIds.ForEach(ele =>
            {
                if (String.IsNullOrEmpty(useridStr))
                {
                    useridStr = useridStr+ele;
                }
                else
                {
                    useridStr = useridStr + "," + ele;

                }
            });
          

            return _iTblUserLocationDAO.getNearBycustomer(distance, siteType, currentLat, currentLng,visitDate,useridStr);
        }
        
        /// <summary>
        /// Sudhir[14-AUG-2018] Added For Showing Distance Wise Dealer.
        /// </summary>
        /// <returns></returns>
        public List<nearBymeTo> SelectNearByDealer(int distance, Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        {
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(StaticStuff.Constants.CP_DEFAULT_MATE_COMP_ORGID);
            if (tblConfigParamsTO != null)
            {
                if (cnfId.ToString() == tblConfigParamsTO.ConfigParamVal)
                {
                    cnfId = 0;
                }
            }
            return _iTblUserLocationDAO.SelectNearByDealer(distance, cnfId, tblUserRoleTO);//TblOrganizationDAO.SelectDealerListForDropDownForCRM(cnfId, tblUserRoleTO);
        }
        #endregion

        #region Insertion
        public int InsertTblUserLocation(TblUserLocationTO tblUserLocationTO)
        {
            try
            {
                int result = 0;
                result = DeleteTblUserLocationPreviousDays(tblUserLocationTO); //Sudhir[14-08-2018] Added For Delete Previous Record By Day Configuration and UserId.
                return _iTblUserLocationDAO.InsertTblUserLocation(tblUserLocationTO);
            }
            catch (Exception ex)
            {
                return -1;
            }
            
        }

        public int InsertTblUserLocation(TblUserLocationTO tblUserLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserLocationDAO.InsertTblUserLocation(tblUserLocationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblUserLocation(TblUserLocationTO tblUserLocationTO)
        {
            return _iTblUserLocationDAO.UpdateTblUserLocation(tblUserLocationTO);
        }

        public int UpdateTblUserLocation(TblUserLocationTO tblUserLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserLocationDAO.UpdateTblUserLocation(tblUserLocationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
   

        public int DeleteTblUserLocation(Int32 idlocation, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserLocationDAO.DeleteTblUserLocation(idlocation, conn, tran);
        }

        public int DeleteTblUserLocationPreviousDays(TblUserLocationTO tblUserLocationTO)
        {
            try
            {
                if (tblUserLocationTO != null)
                {
                    return _iTblUserLocationDAO.DeleteTblUserLocationPreviousDays(tblUserLocationTO);
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {

            }
        }
        #endregion
        
    }
}
