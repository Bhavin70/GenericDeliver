using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using ODLMWebAPI.IoT.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.IoT;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.IoT
{
    public class IotCommunication : IIotCommunication
    {
        private readonly IGateCommunication _iGateCommunication;
        private readonly IDimStatusBL _iDimStatusBL;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        private readonly ITblWeighingMachineDAO _iTblWeighingMachineDAO;
        private readonly IWeighingCommunication _iWeighingCommunication;
        private readonly ICommon _iCommon;
        

        public IotCommunication(IWeighingCommunication iWeighingCommunication, ICommon iCommon, IGateCommunication iGateCommunication, IDimStatusBL iDimStatusBL, ITblOrganizationBL iTblOrganizationBL, ITblWeighingMachineDAO iTblWeighingMachineDAO
             )
        {
            _iGateCommunication = iGateCommunication;
            _iDimStatusBL = iDimStatusBL;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iTblWeighingMachineDAO = iTblWeighingMachineDAO;
            _iWeighingCommunication = iWeighingCommunication;
            _iCommon = iCommon;
          
        }
        public TblLoadingTO GetItemDataFromIotAndMerge(TblLoadingTO tblLoadingTO, Boolean loadingWithDtls, Boolean getStatusHistory = false, Int32 isWeighing = 0)
        {
            if (tblLoadingTO != null)
            {

                if ((tblLoadingTO.TranStatusE == StaticStuff.Constants.TranStatusE.LOADING_DELIVERED || tblLoadingTO.TranStatusE == StaticStuff.Constants.TranStatusE.LOADING_CANCEL) && tblLoadingTO.ModbusRefId == 0)
                    return tblLoadingTO;

                //Call To Gate IoT For Vehicle & Transport Details
                GateIoTResult gateIoTResult = _iGateCommunication.GetLoadingStatusHistoryDataFromGateIoT(tblLoadingTO);
                if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count != 0)
                {
                    tblLoadingTO.VehicleNo = (string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo];
                    tblLoadingTO.TransporterOrgId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.TransportorId]);
                    String statusDate = (String)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusDate];

                    Int32 statusId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusId]);

                    DimStatusTO dimStatusTO = _iDimStatusBL.SelectDimStatusTOByIotStatusId(statusId);
                    tblLoadingTO.StatusDate = IoTDateTimeStringToDate(statusDate);

                    if (dimStatusTO != null)
                    {
                        tblLoadingTO.StatusId = dimStatusTO.IdStatus;
                        tblLoadingTO.StatusDesc = dimStatusTO.StatusName;
                    }
                    if (tblLoadingTO.LoadingSlipList != null)
                        tblLoadingTO.LoadingSlipList.ForEach(f => { f.StatusId = tblLoadingTO.StatusId; f.StatusName = tblLoadingTO.StatusDesc; f.StatusDate = tblLoadingTO.StatusDate; f.VehicleNo = tblLoadingTO.VehicleNo; });
                    String transporterName = _iTblOrganizationBL.GetFirmNameByOrgId(tblLoadingTO.TransporterOrgId);
                    tblLoadingTO.TransporterOrgName = transporterName;


                    //Saket [2019-04-16] Added to get status Histoy.
                    if (getStatusHistory)
                    {
                        tblLoadingTO.LoadingStatusHistoryTOList = new List<TblLoadingStatusHistoryTO>();

                        List<DimStatusTO> statuslist = _iDimStatusBL.SelectAllDimStatusList();

                        for (int j = 0; j < gateIoTResult.Data.Count; j++)
                        {
                            TblLoadingStatusHistoryTO statusHistoryTO = new TblLoadingStatusHistoryTO();
                            statusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                            DimStatusTO dimStatusTO1 = statuslist.Where(w => w.IotStatusId == Convert.ToInt16(gateIoTResult.Data[j][(Int32)IoTConstants.GateIoTColE.StatusId])).FirstOrDefault();
                            if (dimStatusTO1 != null)
                            {
                                statusHistoryTO.StatusId = dimStatusTO1.IdStatus;
                            }
                            statusHistoryTO.StatusDate = IoTDateTimeStringToDate((String)gateIoTResult.Data[j][(int)IoTConstants.GateIoTColE.StatusDate]);
                            statusHistoryTO.StatusRemark = dimStatusTO1.StatusName;
                            tblLoadingTO.LoadingStatusHistoryTOList.Add(statusHistoryTO);
                        }

                    }
                }

                else
                {
                    throw new Exception("IoT details not found");
                }

                if (loadingWithDtls)
                {
                    // List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineList();
                    List<TblWeighingMachineTO> tblWeighingMachineList = _iTblWeighingMachineDAO.SelectAllTblWeighingMachineOfWeighingList(tblLoadingTO.IdLoading);
                    if (tblLoadingTO.LoadingSlipList != null)
                    {
                        List<TblLoadingSlipExtTO> totalLoadingSlipExtList = new List<TblLoadingSlipExtTO>();
                        foreach (var loadingSlip in tblLoadingTO.LoadingSlipList)
                        {
                            // var list = loadingSlip.LoadingSlipExtTOList.Where(w => w.LoadedWeight != 0).ToList();
                            totalLoadingSlipExtList.AddRange(loadingSlip.LoadingSlipExtTOList);
                        }

                        //var layerList = totalLoadingSlipExtList.GroupBy(x => x.LoadingLayerid).ToList();
                        List<int> totalLayerList = new List<int>();
                        if (!totalLayerList.Contains(0))
                            totalLayerList.Add(0);
                        var tLayerList = tblWeighingMachineList.GroupBy(test => test.LayerId)
                                           .Select(grp => grp.First()).ToList();

                        foreach (var item in tLayerList)
                        {
                            if (item.LayerId != 0)
                                totalLayerList.Add(item.LayerId);
                        }
                        var distinctWeighingMachineList = tblWeighingMachineList.GroupBy(test => test.IdWeighingMachine)
                                          .Select(grp => grp.First()).ToList();
                        //Addedd by kiran to avoid IoT calls 13/03/19
                        if (tblLoadingTO.StatusId == Convert.ToInt16(StaticStuff.Constants.TranStatusE.LOADING_GATE_IN) || tblLoadingTO.StatusId == Convert.ToInt16(StaticStuff.Constants.TranStatusE.LOADING_COMPLETED) || tblLoadingTO.StatusId == Convert.ToInt16(StaticStuff.Constants.TranStatusE.LOADING_COMPLETED) || tblLoadingTO.ModbusRefId > 0)
                        {
                            //Sanjay [03-June-2019] Now Layerwise call will not be required as data will be received from TCP/ip communication
                            //Now pass layerid=0. IoT Code will internally give data for all layers.
                            //for (int i = 0; i < totalLayerList.Count; i++)
                            //{
                            //int layerid = totalLayerList[i];
                            int layerid = 0;
                            Int32 loadingId = tblLoadingTO.ModbusRefId;
                            object layerData = null;
                            //var distinctWeighingMachineList = tblWeighingMachineList.GroupBy(x => x.IdWeighingMachine).ToList();//tblWeighingMachineList.Distinct().ToList();

                            if (distinctWeighingMachineList != null && distinctWeighingMachineList.Any())
                            {
                                for (int mc = 0; mc < distinctWeighingMachineList.Count; mc++)
                                {
                                    //Call to Weight IoT
                                    NodeJsResult itemList = _iWeighingCommunication.GetLoadingLayerData(loadingId, layerid, distinctWeighingMachineList[mc]);
                                    if (itemList.Data != null)
                                    {
                                        layerData = itemList.Data;
                                        //if (layerid == 0)
                                        if (false)
                                        {

                                        }
                                        else
                                        {
                                            if (tblLoadingTO.DynamicItemListDCT.ContainsKey(distinctWeighingMachineList[mc].IdWeighingMachine))
                                                tblLoadingTO.DynamicItemListDCT[distinctWeighingMachineList[mc].IdWeighingMachine].AddRange(itemList.Data);
                                            else
                                                //tblLoadingTO.DynamicItemList.AddRange(itemList.Data);
                                                tblLoadingTO.DynamicItemListDCT.Add(distinctWeighingMachineList[mc].IdWeighingMachine, itemList.Data);


                                            if (itemList.Data != null && itemList.Data.Count > 0)
                                            {
                                                for (int f = 0; f < itemList.Data.Count; f++)
                                                {
                                                    var itemRefId = itemList.Data[f][(int)IoTConstants.WeightIotColE.ItemRefNo];
                                                    var itemTO = totalLoadingSlipExtList.Where(w => w.ModbusRefId == itemRefId).FirstOrDefault();
                                                    if (itemTO != null)
                                                    {
                                                        itemTO.LoadedWeight = itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedWt];
                                                        itemTO.CalcTareWeight = itemList.Data[f][(int)IoTConstants.WeightIotColE.CalcTareWt];
                                                        itemTO.LoadedBundles = itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedBundle];
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //}
                        }
                    }
                }
            }

            return tblLoadingTO;
        }

        

        public DateTime IoTDateTimeStringToDate(string statusDate)
        {
            var dateList = statusDate.Split(',').ToList();
            DateTime serverDate = _iCommon.ServerDateTime;
            DateTime dateTime = new DateTime();
            if (dateList != null && dateList.Count == 5)
            {
                Int32 date = Convert.ToInt32(dateList[0]);
                Int32 month = Convert.ToInt32(dateList[1]);
                Int32 year = Convert.ToInt32(serverDate.Year.ToString().Substring(0, 2) + dateList[2]);
                Int32 hr = Convert.ToInt32(dateList[3]);
                Int32 min = Convert.ToInt32(dateList[4]);

                dateTime = new DateTime(year, month, date, hr, min, 0);
            }
            return dateTime;
        }

        public List<TblLoadingTO> GetLoadingData(List<TblLoadingTO> tblLoadingTOList)
        {

            if (tblLoadingTOList != null && tblLoadingTOList.Count > 0)
            {
                for (int i = 0; i < tblLoadingTOList.Count; i++)
                {
                    GetItemDataFromIotAndMerge(tblLoadingTOList[i], false);
                }
            }

            return tblLoadingTOList;
        }

       

        public int PostGateAPIDataToModbusTcpApi(TblLoadingTO tblLoadingTO, Object[] writeData)
        {

            try
            {
                if (writeData.Length != 5)
                {
                    return 0;
                }
                var tRequest = WebRequest.Create(tblLoadingTO.IoTUrl + "WriteOnGateIoTCommand") as HttpWebRequest;
                return _iGateCommunication.PostGateApiCalls(tblLoadingTO, writeData, tRequest);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int UpdateLoadingStatusOnGateAPIToModbusTcpApi(TblLoadingTO tblLoadingTO, Object[] writeData)
        {
            try
            {
                if (writeData.Length != 2)
                {
                    return 0;
                }
                //var tRequest = WebRequest.Create(Startup.GateIotApiURL + "UpdateStatusCommand") as HttpWebRequest;
                var tRequest = WebRequest.Create(tblLoadingTO.IoTUrl + "UpdateStatusCommand") as HttpWebRequest;
                return _iGateCommunication.PostGateApiCalls(tblLoadingTO, writeData, tRequest);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public NodeJsResult DeleteSingleLoadingFromWeightIoTByModBusRefId(TblLoadingTO tblLoadingTO)
        {
            NodeJsResult nodeJsResult = new NodeJsResult();
            try
            {
                if (tblLoadingTO != null && tblLoadingTO.ModbusRefId != 0)
                {
                    Int32 modBusRefId = tblLoadingTO.ModbusRefId;

                    //List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineList();

                    List<TblWeighingMachineTO> tblWeighingMachineList = _iTblWeighingMachineDAO.SelectAllTblWeighingMachineOfWeighingList(tblLoadingTO.IdLoading);

                    if (tblWeighingMachineList != null && tblWeighingMachineList.Count > 0)
                    {
                        tblWeighingMachineList = tblWeighingMachineList.GroupBy(g => g.IdWeighingMachine).Select(s => s.FirstOrDefault()).ToList();
                        for (int i = 0; i < tblWeighingMachineList.Count; i++)
                        {
                            TblWeighingMachineTO tblWeighingMachineTO = tblWeighingMachineList[i];
                            if (!String.IsNullOrEmpty(tblWeighingMachineTO.IoTUrl))
                            {
                                //Addes by kiran for retry 3 times to delete weighing data
                                int cnt2 = 0;
                                while (cnt2 < 3)
                                {
                                    nodeJsResult = _iWeighingCommunication.DeleteSingleLoadingFromWeightIoT(modBusRefId, 0, tblWeighingMachineTO);
                                    if (nodeJsResult.Code == 1)
                                    {
                                        break;
                                    }
                                    cnt2++;
                                }
                            }
                        }
                    }
                    else
                    {
                        nodeJsResult.Code = 1;
                    }
                    return nodeJsResult;
                }
                else
                {
                    nodeJsResult.DefaultErrorBehavior(0, "Loading id not found");
                    return nodeJsResult;
                }
            }
            catch (Exception ex)
            {
                nodeJsResult.DefaultErrorBehavior(0, "Error in DeleteSingleLoadingFromWeightIoT");
                return nodeJsResult;
            }
        }

        public GateIoTResult GetLoadingSlipsByStatusFromIoTByStatusId(String statusIds, TblGateTO tblGateTO)
        {
            //Sanjay [03-June-2019] Now Iot communication is shifted to TCP/IP and hence data length is increased.
            //It will return max  87 per call
            //Int32 maxRecordPerCylce = 24;
            Int32 maxRecordPerCylce = 83;

            List<DimStatusTO> dimStatusTOList = _iDimStatusBL.SelectAllDimStatusList();

            GateIoTResult gateIoTResult = new GateIoTResult();
            try
            {

                if (!String.IsNullOrEmpty(statusIds))
                {
                    List<String> statusList = statusIds.Split(',').ToList();

                    Boolean callAllFunction = false;
                    if (Convert.ToInt32(statusIds) == 101 || Convert.ToInt32(statusIds) == 102 || Convert.ToInt32(statusIds) == 103
                        || Convert.ToInt32(statusIds) == 104 || Convert.ToInt32(statusIds) == 105) // Convered to int as it has been decided that this value always be single. To Pass multiple combination it been again encoded to some number
                        callAllFunction = true;

                    for (int i = 0; i < statusList.Count; i++)
                    {
                        Int32 statusId = Convert.ToInt32(statusList[i]);
                        Int32 startLoadingId = 1;  // this is default value from which records will be search from Iot if not passed

                        DimStatusTO dimStatusTO = dimStatusTOList.Where(w => w.IdStatus == statusId).FirstOrDefault();
                        if (dimStatusTO != null || callAllFunction)
                        {
                            Int32 breakLoop = 0;
                            while (breakLoop != 1)
                            {
                                GateIoTResult gateIoTResultTemp = null;
                                if (callAllFunction)
                                    gateIoTResultTemp = _iGateCommunication.GetAllLoadingSlipsByStatusFromIoT(Convert.ToInt32(statusIds), tblGateTO, startLoadingId);
                                else
                                    gateIoTResultTemp = _iGateCommunication.GetLoadingSlipsByStatusFromIoT(dimStatusTO.IotStatusId, tblGateTO, startLoadingId);

                                if (gateIoTResultTemp != null && gateIoTResultTemp.Data != null)
                                {
                                    if (gateIoTResultTemp.Data.Count >= maxRecordPerCylce)
                                    {
                                        startLoadingId = Convert.ToInt32(gateIoTResultTemp.Data[gateIoTResultTemp.Data.Count - 1][(Int32)IoTConstants.GateIoTColE.LoadingId]);
                                        startLoadingId += 1;
                                    }
                                    else
                                    {
                                        breakLoop = 1;
                                    }
                                    gateIoTResult.Data.AddRange(gateIoTResultTemp.Data);
                                }
                                else
                                {
                                    breakLoop = 1;
                                }
                            }
                        }
                    }
                }
                return gateIoTResult;
            }
            catch (Exception ex)
            {
                gateIoTResult.DefaultErrorBehavior(0, "Error in GetLoadingStatusHistoryDataFromGateIoT");
                return gateIoTResult;
            }
        }

        public List<int[]> GenerateFrameData(TblLoadingTO loadingTO, TblWeighingMeasuresTO weighingMeasureTo, List<TblLoadingSlipExtTO> wtTakentLoadingExtToList)
        {
            List<int[]> frameList = new List<int[]>();
            if (wtTakentLoadingExtToList == null || wtTakentLoadingExtToList.Count == 0)
            {
                FormatIoTFrameToWrite(loadingTO, weighingMeasureTo, wtTakentLoadingExtToList, frameList, 0);
            }
            else
            {
                for (int i = 0; i < wtTakentLoadingExtToList.Count; i++)
                {
                    FormatIoTFrameToWrite(loadingTO, weighingMeasureTo, wtTakentLoadingExtToList, frameList, i);
                }
            }
            return frameList;
        }

        public List<object[]> GenerateGateIoTFrameData(TblLoadingTO loadingTO, string vehicleNo, Int32 statusId, Int32 transportorId)
        {
            List<object[]> frameList = new List<object[]>();
            FormatStdGateIoTFrameToWrite(loadingTO, vehicleNo, statusId, transportorId, frameList);
            return frameList;
        }

        public List<object[]> GenerateGateIoTStatusFrameData(TblLoadingTO loadingTO, Int32 statusId)
        {
            List<object[]> frameList = new List<object[]>();
            FormatStatusUpdateGateIoTFrameToWrite(loadingTO, statusId, frameList);
            return frameList;
        }

        public void FormatIoTFrameToWrite(TblLoadingTO loadingTO, TblWeighingMeasuresTO weighingMeasureTo, List<TblLoadingSlipExtTO> wtTakentLoadingExtToList, List<int[]> frameList, int i)
        {
            try
            {
                var loadingId = loadingTO.ModbusRefId;
                var MeasurTypeId = weighingMeasureTo.WeightMeasurTypeId;
                var layerId = (MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].LoadingLayerid;
                var itemId = (MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].ModbusRefId;
                var VehicleWeight = weighingMeasureTo.WeightMT;
                var LoadedWeight = (MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].LoadedWeight;
                var TareWt = (MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].CalcTareWeight;
                DateTime serverDt = _iCommon.ServerDateTime;
                var Day = serverDt.Day.ToString().Length == 1 ? "0" + serverDt.Day.ToString() : serverDt.Day.ToString();
                var Hour = serverDt.Hour.ToString().Length == 1 ? "0" + serverDt.Hour.ToString() : serverDt.Hour.ToString();
                var Minute = serverDt.Minute.ToString().Length == 1 ? "0" + serverDt.Minute.ToString() : serverDt.Minute.ToString();
                var timeStamp = Convert.ToInt32(Day + "" + Hour + "" + Minute);
                var loadedBundles = (MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].LoadedBundles;
                frameList.Add(new int[9] { loadingId, layerId, itemId, MeasurTypeId, (int)VehicleWeight, (int)LoadedWeight, (int)TareWt, (int)timeStamp, (int)loadedBundles });

            }
            catch (Exception ex)
            {

            }

        }

        public Int32 GetDateToTimestap()
        {
            Int32 timeStamp = 0;
            DateTime serverDt = _iCommon.ServerDateTime;
            var Day = serverDt.Day.ToString().Length == 1 ? "0" + serverDt.Day.ToString() : serverDt.Day.ToString();
            var Hour = serverDt.Hour.ToString().Length == 1 ? "0" + serverDt.Hour.ToString() : serverDt.Hour.ToString();
            var Minute = serverDt.Minute.ToString().Length == 1 ? "0" + serverDt.Minute.ToString() : serverDt.Minute.ToString();
            return timeStamp = Convert.ToInt32(Day + "" + Hour + "" + Minute);
        }

        public void FormatStdGateIoTFrameToWrite(TblLoadingTO loadingTO, string vehicleNo, Int32 statusId, Int32 transportorId, List<object[]> frameList)
        {
            try
            {
                var loadingId = loadingTO.ModbusRefId;
                DateTime serverDt = _iCommon.ServerDateTime;
                var day = serverDt.Day.ToString().Length == 1 ? "0" + serverDt.Day.ToString() : serverDt.Day.ToString();
                var month = serverDt.Month.ToString().Length == 1 ? "0" + serverDt.Month.ToString() : serverDt.Month.ToString();
                var year = serverDt.ToString("yy");
                var hour = serverDt.Hour.ToString().Length == 1 ? "0" + serverDt.Hour.ToString() : serverDt.Hour.ToString();
                var minute = serverDt.Minute.ToString().Length == 1 ? "0" + serverDt.Minute.ToString() : serverDt.Minute.ToString();
                var timeStamp = (day + "" + month + "" + year + "" + hour + "" + minute).ToString();
                if (timeStamp.Length != 10 || transportorId == 0)
                {
                    frameList = new List<object[]>();
                }
                else
                {
                    frameList.Add(new object[5] { loadingId, vehicleNo, statusId, timeStamp, transportorId });
                }
            }
            catch (Exception ex)
            {
                frameList = new List<object[]>();
            }
        }

        public void FormatStatusUpdateGateIoTFrameToWrite(TblLoadingTO loadingTO, Int32 statusId, List<object[]> frameList)
        {
            try
            {
                var loadingId = loadingTO.ModbusRefId;
                frameList.Add(new object[2] { loadingId, statusId });
            }
            catch (Exception ex)
            {

            }
        }

        public string GetIotEncodedStatusIdsForGivenStatus(string statusIds)
        {
            if (statusIds.Equals("7,14,20"))
                return 101 + "";
            if (statusIds.Equals("15,16,25,24"))
                return 102 + "";
            if (statusIds.Equals("15,16"))
                return 102 + "";
            if (statusIds.Equals("15,24"))
                return 103 + "";
            if (statusIds.Equals("0"))
                return 104 + "";
            if (statusIds.Equals("16,25"))
                return 105 + "";

            return statusIds;
        }

        public string GetIotDecodedStatusIdsForGivenStatus(string statusIds)
        {
            if (statusIds.Equals("101"))
                return "7,14,20";
            if (statusIds.Equals("102"))
                return "15,16,24,25";
            if (statusIds.Equals("103"))
                return "15,24";
            if (statusIds.Equals("104"))
                return 0 + "";
            if (statusIds.Equals("105"))
                return "16,25";

            return statusIds;
        }

        public GateIoTResult GetDecryptedLoadingId(string dataFrame, string methodName, string URL)
        {
            GateIoTResult gateIOTResult = new GateIoTResult();
            try
            {
                if (String.IsNullOrEmpty(dataFrame))
                {
                    gateIOTResult.DefaultErrorBehavior(0, "transaction ID not found");
                    return gateIOTResult;
                }
                String url = URL + methodName + "?data=" + dataFrame;
                String result;
                System.Net.WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                //request.Timeout = 10000;
                var response = (HttpWebResponse)request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    var resultdata = JsonConvert.DeserializeObject<GateIoTResult>(result);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (resultdata != null && resultdata.Code == 1)
                        {
                            gateIOTResult.DefaultSuccessBehavior(1, "data Found", resultdata.Data);
                        }
                    }
                    else
                    {
                        gateIOTResult.DefaultErrorBehavior(0, resultdata.Msg);
                    }
                    request.Abort();
                    sr.Dispose();
                }
                return gateIOTResult;
            }
            catch (Exception ex)
            {
                gateIOTResult.DefaultErrorBehavior(0, "Error in GetDecryptedLoadingId");
                return gateIOTResult;
            }
        }
    }
}

