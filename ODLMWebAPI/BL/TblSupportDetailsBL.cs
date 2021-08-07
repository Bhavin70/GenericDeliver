using Newtonsoft.Json.Linq;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblSupportDetailsBL : ITblSupportDetailsBL
    {
        private readonly ITblSupportDetailsDAO _iTblSupportDetailsDAO;
        private readonly ITblUserBL _iTblUserBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblWeighingMeasuresBL _iTblWeighingMeasuresBL;
        private readonly ITblInvoiceBL _iTblInvoiceBL;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblSupportDetailsBL(ICommon iCommon, IConnectionString iConnectionString, ITblInvoiceBL iTblInvoiceBL, ITblSupportDetailsDAO iTblSupportDetailsDAO, ITblUserBL iTblUserBL, ITblConfigParamsBL iTblConfigParamsBL, ITblWeighingMeasuresBL iTblWeighingMeasuresBL)
        {
            _iTblSupportDetailsDAO = iTblSupportDetailsDAO;
            _iTblUserBL = iTblUserBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblWeighingMeasuresBL = iTblWeighingMeasuresBL;
            _iTblInvoiceBL = iTblInvoiceBL;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection
        public List<TblSupportDetailsTO> SelectAllTblSupportDetails()
        {
            return _iTblSupportDetailsDAO.SelectAllTblSupportDetails();
        }

        public List<TblSupportDetailsTO> SelectAllTblSupportDetailsList()
        {
            List<TblSupportDetailsTO> tblSupportDetailsList = _iTblSupportDetailsDAO.SelectAllTblSupportDetails();
            if (tblSupportDetailsList != null)
                return tblSupportDetailsList;
            else
                return null;
        }

        public TblSupportDetailsTO SelectTblSupportDetailsTO(Int32 idsupport)
        {
            TblSupportDetailsTO tblSupportDetailsTO = _iTblSupportDetailsDAO.SelectTblSupportDetails(idsupport);
            if (tblSupportDetailsTO != null)
                return tblSupportDetailsTO;
            else
                return null;
        }

        public ResultMessage StopService(TblUserTO tblUserTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblUserTO userExistUserTO = _iTblUserBL.SelectTblUserTO(tblUserTO.UserLogin, tblUserTO.UserPasswd);
                if (userExistUserTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Invalid Credentials";
                    return resultMessage;
                }
                //Sanjay [25-Feb-2019] To Identify between invalid credentials and inactive account
                if (userExistUserTO.IsActive == 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Inactive Account since :" + userExistUserTO.DeactivatedOn;
                    resultMessage.DisplayMessage = "This account is inactive. Please contact your technical administrative authorities";
                    return resultMessage;
                }

                userExistUserTO.LoginTO = tblUserTO.LoginTO;
                userExistUserTO.LoginTO.LoginDate = _iCommon.ServerDateTime;
                userExistUserTO.LoginTO.UserId = userExistUserTO.IdUser;
                int result = InsertTblStopServiceHistory(userExistUserTO.LoginTO);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Could not Insert Records. Some error occured while Inserting";
                    resultMessage.Tag = "Error While InsertTblStopService In Method Login";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                try
                {
                    #region API

                    string client_id = "";
                    string client_secret = "";
                    string resourceName = "";
                    string appName = "";

                    TblConfigParamsTO tblConfigParamsTOKeys = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.STOP_WEB_API_SERVICE_KEYS);
                    if (tblConfigParamsTOKeys != null)
                    {
                        client_id = tblConfigParamsTOKeys.ConfigParamVal.Split(",")[0];
                        client_secret = tblConfigParamsTOKeys.ConfigParamVal.Split(",")[1];
                        resourceName = tblConfigParamsTOKeys.ConfigParamVal.Split(",")[2];
                        appName = tblConfigParamsTOKeys.ConfigParamVal.Split(",")[3];
                    }

                    //Stop Web API Services
                    var access_token_client = new RestSharp.RestClient("https://login.microsoftonline.com/75005177-0a8b-4c2a-990b-e299cae56dbb/oauth2/token");
                    var request1 = new RestSharp.RestRequest(RestSharp.Method.POST);
                    request1.AddParameter("grant_type", "client_credentials");
                    request1.AddParameter("client_id", client_id);
                    request1.AddParameter("client_secret", client_secret);
                    request1.AddParameter("resource", "https://management.azure.com/");
                    RestSharp.IRestResponse response = access_token_client.Execute(request1);
                    JObject json = JObject.Parse(response.Content);
                    string access = (string)json["access_token"];

                    var client = new HttpClient();
                    String url = "https://management.azure.com/subscriptions/d3f31a43-b546-4560-a72b-2a62668d157c/resourceGroups/" + resourceName + "/providers/Microsoft.Web/sites/" + appName + "/stop?api-version=2016-08-01";
                    String access_result;
                    System.IO.StreamWriter myWriter = null;
                    System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                    request.UseDefaultCredentials = true;
                    request.PreAuthenticate = true;
                    request.Headers.Add("Authorization", "Bearer " + access);
                    request.Method = "POST";
                    request.ContentLength = 0;
                    request.ContentType = "application/json";
                    System.Net.WebResponse objResponse = request.GetResponseAsync().Result;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
                    {
                        access_result = sr.ReadToEnd();
                        sr.Dispose();
                    }

                    #endregion

                }
                catch (Exception ex)
                {

                }
                try
                {
                    #region GUI

                    //Stop Web GUI Services

                    string client_id_gui = "";
                    string client_secret_gui = "";
                    string resourceName_gui = "";
                    string appName_gui = "";

                    TblConfigParamsTO tblConfigParamsTOKeysGui = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.STOP_WEB_GUI_SERVICE_KEYS);
                    if (tblConfigParamsTOKeysGui != null)
                    {
                        client_id_gui = tblConfigParamsTOKeysGui.ConfigParamVal.Split(",")[0];
                        client_secret_gui = tblConfigParamsTOKeysGui.ConfigParamVal.Split(",")[1];
                        resourceName_gui = tblConfigParamsTOKeysGui.ConfigParamVal.Split(",")[2];
                        appName_gui = tblConfigParamsTOKeysGui.ConfigParamVal.Split(",")[3];
                    }

                    var access_token = new RestSharp.RestClient("https://login.microsoftonline.com/75005177-0a8b-4c2a-990b-e299cae56dbb/oauth2/token");
                    var request_gui = new RestSharp.RestRequest(RestSharp.Method.POST);
                    request_gui.AddParameter("grant_type", "client_credentials");
                    request_gui.AddParameter("client_id", client_id_gui);
                    request_gui.AddParameter("client_secret", client_secret_gui);
                    request_gui.AddParameter("resource", "https://management.azure.com/");
                    RestSharp.IRestResponse response_gui = access_token.Execute(request_gui);
                    JObject json_gui = JObject.Parse(response_gui.Content);
                    string access_gui = (string)json_gui["access_token"];

                    var client_gui = new HttpClient();
                    String url_gui = "https://management.azure.com/subscriptions/d3f31a43-b546-4560-a72b-2a62668d157c/resourceGroups/" + resourceName_gui + "/providers/Microsoft.Web/sites/" + appName_gui + "/stop?api-version=2016-08-01";
                    String access_result_gui;
                    System.IO.StreamWriter myWriter1 = null;
                    System.Net.WebRequest request_gui1 = System.Net.WebRequest.Create(url_gui);
                    request_gui1.UseDefaultCredentials = true;
                    request_gui1.PreAuthenticate = true;
                    //request.Headers.Add("Authorization", "Bearer " + "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjdfWnVmMXR2a3dMeFlhSFMzcTZsVWpVWUlHdyIsImtpZCI6IjdfWnVmMXR2a3dMeFlhSFMzcTZsVWpVWUlHdyJ9.eyJhdWQiOiJodHRwczovL21hbmFnZW1lbnQuYXp1cmUuY29tLyIsImlzcyI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0Lzc1MDA1MTc3LTBhOGItNGMyYS05OTBiLWUyOTljYWU1NmRiYi8iLCJpYXQiOjE1MzUwOTA0MzEsIm5iZiI6MTUzNTA5MDQzMSwiZXhwIjoxNTM1MDk0MzMxLCJhaW8iOiI0MkJnWUFoTldNVjVZSmZJM2RYbmRxOFBtRklrQVFBPSIsImFwcGlkIjoiYzkzMTg5NTItMWIzNy00NGUzLWI3MzktNTFhNjMyMmZiNGVhIiwiYXBwaWRhY3IiOiIxIiwiaWRwIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvNzUwMDUxNzctMGE4Yi00YzJhLTk5MGItZTI5OWNhZTU2ZGJiLyIsIm9pZCI6IjVhYzhlOTlkLTIxZGYtNGIxNi04ZWRhLTVmNzMzZWE1NGQzOSIsInN1YiI6IjVhYzhlOTlkLTIxZGYtNGIxNi04ZWRhLTVmNzMzZWE1NGQzOSIsInRpZCI6Ijc1MDA1MTc3LTBhOGItNGMyYS05OTBiLWUyOTljYWU1NmRiYiIsInV0aSI6IjhyTU9mUnZiNmtDR1QyOGc2S3dsQUEiLCJ2ZXIiOiIxLjAifQ.ciFESh5BN96Mo9JRlYg89uhpMhrIECh68RjMPYRwPi3vCumfHU59EHVu6SOkSlGKmITh59I_LMyE0VLLjZAwIuZsNLMCykl_U6lUGOwYIogfh2EOal2q8MxxWBkJOJ8eCnI0iW_Xhtyj5lIoSLpM6TP98RlB59rGQy5fvwnU87fhWcW4DR9tSj5Y4NzS2PMxq22SZkYySUSJJnwxe6f-97gYlXc2OmCP5PYBbZoajUDWvbGkx6z4qLos2fIJU9sx5O2tVpQCqtFmbkbb-KeCAyTWDonB538s5s_HxBBayPc4LwvwDClICxiNNaaRzN_ZEGb-jb0n1QLar2b8kdRt3g");
                    request_gui1.Headers.Add("Authorization", "Bearer " + access_gui);
                    request_gui1.Method = "POST";
                    request_gui1.ContentLength = 0;
                    request_gui1.ContentType = "application/json";
                    System.Net.WebResponse objResponse_gui = request_gui1.GetResponseAsync().Result;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse_gui.GetResponseStream()))
                    {
                        access_result_gui = sr.ReadToEnd();
                        sr.Dispose();
                    }

                    #endregion

                }
                catch (Exception ex)
                {

                }
                try
                {
                    #region VM

                    //Stop Web GUI Services

                    string client_id_vm = "";
                    string client_secret_vm = "";
                    string resourceName_vm = "";
                    string appName_vm = "";

                    TblConfigParamsTO tblConfigParamsTOKeysVm = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.STOP_WEB_VM_SERVICE_KEYS);
                    if (tblConfigParamsTOKeysVm != null)
                    {
                        client_id_vm = tblConfigParamsTOKeysVm.ConfigParamVal.Split(",")[0];
                        client_secret_vm = tblConfigParamsTOKeysVm.ConfigParamVal.Split(",")[1];
                        resourceName_vm = tblConfigParamsTOKeysVm.ConfigParamVal.Split(",")[2];
                        appName_vm = tblConfigParamsTOKeysVm.ConfigParamVal.Split(",")[3];
                    }

                    var access_token_vm = new RestSharp.RestClient("https://login.microsoftonline.com/75005177-0a8b-4c2a-990b-e299cae56dbb/oauth2/token");
                    var request_vm = new RestSharp.RestRequest(RestSharp.Method.POST);
                    request_vm.AddParameter("grant_type", "client_credentials");
                    request_vm.AddParameter("client_id", client_id_vm);
                    request_vm.AddParameter("client_secret", client_secret_vm);
                    request_vm.AddParameter("resource", "https://management.azure.com/");
                    RestSharp.IRestResponse response_vm = access_token_vm.Execute(request_vm);
                    JObject json_vm = JObject.Parse(response_vm.Content);
                    string access_vm = (string)json_vm["access_token"];

                    var client_vm = new HttpClient();
                    //String url_vm = "https://management.azure.com/subscriptions/d3f31a43-b546-4560-a72b-2a62668d157c/resourceGroups/" + resourceName_vm + "/providers/Microsoft.Web/sites/" + appName_vm + "/stop?api-version=2016-08-01";

                    String url_vm = "https://management.azure.com/subscriptions/d3f31a43-b546-4560-a72b-2a62668d157c/resourceGroups/" + resourceName_vm + "/providers/Microsoft.Compute/virtualMachines/" + appName_vm + "/powerOff?api-version=2021-03-01";

                    String access_result_vm;
                    System.IO.StreamWriter myWriter2 = null;
                    System.Net.WebRequest request_vm2 = System.Net.WebRequest.Create(url_vm);
                    request_vm2.UseDefaultCredentials = true;
                    request_vm2.PreAuthenticate = true;
                    //request.Headers.Add("Authorization", "Bearer " + "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjdfWnVmMXR2a3dMeFlhSFMzcTZsVWpVWUlHdyIsImtpZCI6IjdfWnVmMXR2a3dMeFlhSFMzcTZsVWpVWUlHdyJ9.eyJhdWQiOiJodHRwczovL21hbmFnZW1lbnQuYXp1cmUuY29tLyIsImlzcyI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0Lzc1MDA1MTc3LTBhOGItNGMyYS05OTBiLWUyOTljYWU1NmRiYi8iLCJpYXQiOjE1MzUwOTA0MzEsIm5iZiI6MTUzNTA5MDQzMSwiZXhwIjoxNTM1MDk0MzMxLCJhaW8iOiI0MkJnWUFoTldNVjVZSmZJM2RYbmRxOFBtRklrQVFBPSIsImFwcGlkIjoiYzkzMTg5NTItMWIzNy00NGUzLWI3MzktNTFhNjMyMmZiNGVhIiwiYXBwaWRhY3IiOiIxIiwiaWRwIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvNzUwMDUxNzctMGE4Yi00YzJhLTk5MGItZTI5OWNhZTU2ZGJiLyIsIm9pZCI6IjVhYzhlOTlkLTIxZGYtNGIxNi04ZWRhLTVmNzMzZWE1NGQzOSIsInN1YiI6IjVhYzhlOTlkLTIxZGYtNGIxNi04ZWRhLTVmNzMzZWE1NGQzOSIsInRpZCI6Ijc1MDA1MTc3LTBhOGItNGMyYS05OTBiLWUyOTljYWU1NmRiYiIsInV0aSI6IjhyTU9mUnZiNmtDR1QyOGc2S3dsQUEiLCJ2ZXIiOiIxLjAifQ.ciFESh5BN96Mo9JRlYg89uhpMhrIECh68RjMPYRwPi3vCumfHU59EHVu6SOkSlGKmITh59I_LMyE0VLLjZAwIuZsNLMCykl_U6lUGOwYIogfh2EOal2q8MxxWBkJOJ8eCnI0iW_Xhtyj5lIoSLpM6TP98RlB59rGQy5fvwnU87fhWcW4DR9tSj5Y4NzS2PMxq22SZkYySUSJJnwxe6f-97gYlXc2OmCP5PYBbZoajUDWvbGkx6z4qLos2fIJU9sx5O2tVpQCqtFmbkbb-KeCAyTWDonB538s5s_HxBBayPc4LwvwDClICxiNNaaRzN_ZEGb-jb0n1QLar2b8kdRt3g");
                    request_vm2.Headers.Add("Authorization", "Bearer " + access_vm);
                    request_vm2.Method = "POST";
                    request_vm2.ContentLength = 0;
                    request_vm2.ContentType = "application/json";
                    System.Net.WebResponse objResponse_vm = request_vm2.GetResponseAsync().Result;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse_vm.GetResponseStream()))
                    {
                        access_result_vm = sr.ReadToEnd();
                        sr.Dispose();
                    }


                    #endregion
                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {
                return null;

            }
            resultMessage.MessageType = ResultMessageE.Information;
            resultMessage.Result = 1;
            resultMessage.Text = "Application Stopped";
            resultMessage.DisplayMessage = "Application Stopped";
            return resultMessage;

        }


        #endregion

        #region Insertion
        public int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO)
        {
            return _iTblSupportDetailsDAO.InsertTblSupportDetails(tblSupportDetailsTO);
        }

        public int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSupportDetailsDAO.InsertTblSupportDetails(tblSupportDetailsTO, conn, tran);
        }


        public ResultMessage PostDeleteWeighingMeasureForSupport(TblWeighingMeasuresTO tblWeighingMeasuresTO, Int32 fromUser)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            TblWeighingMeasuresTO previousWeighingMeasuresTO = new TblWeighingMeasuresTO();
            int result = 0;
            try
            {
                previousWeighingMeasuresTO = _iTblWeighingMeasuresBL.SelectTblWeighingMeasuresTO(tblWeighingMeasuresTO.IdWeightMeasure);
                //tblUserTO = _iTblUserBL.SelectTblUserTO(fromUser);
                conn.Open();
                tran = conn.BeginTransaction();
                result = _iTblWeighingMeasuresBL.DeleteTblWeighingMeasures(tblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in UpdateInvoiceforSupport");
                    return resultMessage;
                }
                else
                {
                    String description = String.Empty;
                    description = "In TblWeighingMeasures Id " + tblWeighingMeasuresTO.IdWeightMeasure + " Related Entry Deleted";

                    String Comment = String.Empty;
                    Comment = "Previous Record Information of WeighingMeasuresId=#" + tblWeighingMeasuresTO.IdWeightMeasure + "|| LoadingId is=" + previousWeighingMeasuresTO.LoadingId +
                              " || WeighingMachineId Is" + previousWeighingMeasuresTO.WeighingMachineId + " || Vehicle No Is " + previousWeighingMeasuresTO.VehicleNo +
                              " || WeightMeasurTypeId Is " + previousWeighingMeasuresTO.WeightMeasurTypeId + "|| WeightMT is " + previousWeighingMeasuresTO.WeightMT +
                              " || Created By" + previousWeighingMeasuresTO.CreatedBy;


                    TblSupportDetailsTO tblSupportDetailsTO = new TblSupportDetailsTO();
                    tblSupportDetailsTO.ModuleId = 1;//By Defalult Module id is Set to Commercial;
                    tblSupportDetailsTO.Formid = Convert.ToInt32(Constants.SupportPageTypE.LOADING_SLIP);
                    tblSupportDetailsTO.FromUser = fromUser;
                    tblSupportDetailsTO.CreatedBy = tblWeighingMeasuresTO.UpdatedBy;
                    tblSupportDetailsTO.CreatedOn = _iCommon.ServerDateTime;
                    tblSupportDetailsTO.Description = description;
                    tblSupportDetailsTO.RequireTime = 30;//HardCoded 30 Minutes;
                    tblSupportDetailsTO.Comments = Comment;

                    result = InsertTblSupportDetails(tblSupportDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error in InsertTblSupportDetails");
                        return resultMessage;
                    }
                    tran.Commit();
                    resultMessage.Tag = description;
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateInvoiceForSupport");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        public int InsertTblStopServiceHistory(TblLoginTO tblLoginTO)
        {
            return _iTblSupportDetailsDAO.InsertTblStopServiceHistory(tblLoginTO);
            //return TblLoginDAO.InsertTblStopServiceHistory(tblLoginTO);
        }

        #endregion

        #region Updation
        public int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO)
        {
            return _iTblSupportDetailsDAO.UpdateTblSupportDetails(tblSupportDetailsTO);
        }

        public ResultMessage UpdateInvoiceForSupport(TblInvoiceTO tblInvoiceTO, Int32 fromUser, String Comments)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            TblInvoiceTO previousInvoiceTO = new TblInvoiceTO();
            //TblUserTO tblUserTO = new TblUserTO();
            int result = 0;
            try
            {
                previousInvoiceTO = _iTblInvoiceBL.SelectTblInvoiceTO(tblInvoiceTO.IdInvoice);

                if (previousInvoiceTO == null)
                {
                    return null;
                }

                TblInvoiceTO updateTblInvoiceTO = previousInvoiceTO.DeepCopy();

                updateTblInvoiceTO.InvoiceNo = tblInvoiceTO.InvoiceNo;
                updateTblInvoiceTO.InvoiceModeId = tblInvoiceTO.InvoiceModeId;
                updateTblInvoiceTO.StatusId = tblInvoiceTO.StatusId;

                //tblUserTO = _iTblUserBL.SelectTblUserTO(fromUser);
                conn.Open();
                tran = conn.BeginTransaction();
                result = _iTblInvoiceBL.UpdateTblInvoice(updateTblInvoiceTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in UpdateInvoiceforSupport");
                    return resultMessage;
                }
                else
                {
                    String description = String.Empty;
                    description = "Invoice Id (" + tblInvoiceTO.IdInvoice + ") ,";

                    if (previousInvoiceTO.InvoiceNo != tblInvoiceTO.InvoiceNo)
                        description += " Invoice No  Pre (" + previousInvoiceTO.InvoiceNo + ") New (" + tblInvoiceTO.InvoiceNo + ") ,";

                    if (previousInvoiceTO.StatusId != tblInvoiceTO.StatusId)
                    {
                        description += " Status change from " + previousInvoiceTO.InvoiceStatusE.ToString() + " to " + tblInvoiceTO.InvoiceStatusE.ToString() + " ,";
                    }
                    if (previousInvoiceTO.InvoiceModeId != tblInvoiceTO.InvoiceModeId)
                    {
                        description += " Mode change from " + previousInvoiceTO.InvoiceModeE.ToString() + " to " + tblInvoiceTO.InvoiceModeE.ToString();
                    }


                    TblSupportDetailsTO tblSupportDetailsTO = new TblSupportDetailsTO();
                    tblSupportDetailsTO.ModuleId = 1;//By Defalult Module id is Set to Commercial;
                    tblSupportDetailsTO.Formid = Convert.ToInt32(Constants.SupportPageTypE.BILLING);
                    tblSupportDetailsTO.FromUser = fromUser;
                    tblSupportDetailsTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                    tblSupportDetailsTO.CreatedOn = _iCommon.ServerDateTime;
                    tblSupportDetailsTO.Description = description;
                    tblSupportDetailsTO.RequireTime = 30;//HardCoded 30 Minutes;
                    tblSupportDetailsTO.Comments = Comments;

                    result = InsertTblSupportDetails(tblSupportDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error in InsertTblSupportDetails");
                        return resultMessage;
                    }
                    tran.Commit();
                    resultMessage.Tag = description;
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateInvoiceForSupport");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }




        public int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSupportDetailsDAO.UpdateTblSupportDetails(tblSupportDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblSupportDetails(Int32 idsupport)
        {
            return _iTblSupportDetailsDAO.DeleteTblSupportDetails(idsupport);
        }

        public int DeleteTblSupportDetails(Int32 idsupport, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSupportDetailsDAO.DeleteTblSupportDetails(idsupport, conn, tran);
        }

        #endregion
    }
}
