using ODLMWebAPI.Models;
using ODLMWebAPI.IoT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.IoT.Interfaces
{
    public interface IIotCommunication
    {
        TblLoadingTO GetItemDataFromIotAndMerge(TblLoadingTO tblLoadingTO, Boolean loadingWithDtls, Boolean getStatusHistory = false, Int32 isWeighing = 0);

        void GetItemDataFromIotForGivenLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO);

        DateTime IoTDateTimeStringToDate(string statusDate);
        List<TblLoadingTO> GetLoadingData(List<TblLoadingTO> tblLoadingTOList);

        //void GetWeighingMeasuresFromIoT(string loadingId, bool isUnloading, List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList, SqlConnection conn, SqlTransaction tran);

        int PostGateAPIDataToModbusTcpApi(TblLoadingTO tblLoadingTO, Object[] writeData);

        int UpdateLoadingStatusOnGateAPIToModbusTcpApi(TblLoadingTO tblLoadingTO, Object[] writeData);

        NodeJsResult DeleteSingleLoadingFromWeightIoTByModBusRefId(TblLoadingTO tblLoadingTO);

        GateIoTResult GetLoadingSlipsByStatusFromIoTByStatusId(String statusIds, TblGateTO tblGateTO);

        List<int[]> GenerateFrameData(TblLoadingTO loadingTO, TblWeighingMeasuresTO weighingMeasureTo, List<TblLoadingSlipExtTO> wtTakentLoadingExtToList);


        List<object[]> GenerateGateIoTFrameData(TblLoadingTO loadingTO, string vehicleNo, Int32 statusId, Int32 transportorId);

        List<object[]> GenerateGateIoTStatusFrameData(TblLoadingTO loadingTO, Int32 statusId);

        void FormatIoTFrameToWrite(TblLoadingTO loadingTO, TblWeighingMeasuresTO weighingMeasureTo, List<TblLoadingSlipExtTO> wtTakentLoadingExtToList, List<int[]> frameList, int i);

        Int32 GetDateToTimestap();

        void FormatStdGateIoTFrameToWrite(TblLoadingTO loadingTO, string vehicleNo, Int32 statusId, Int32 transportorId, List<object[]> frameList);

        void FormatStatusUpdateGateIoTFrameToWrite(TblLoadingTO loadingTO, Int32 statusId, List<object[]> frameList);


        string GetIotEncodedStatusIdsForGivenStatus(string statusIds);


        string GetIotDecodedStatusIdsForGivenStatus(string statusIds);

        GateIoTResult GetDecryptedLoadingId(string dataFrame, string methodName, string URL);
       string GetVehicleNumbers(string vehicleNo, Boolean isForSelect);//chetan[10-feb-2020] added for allow Old vehicle on Iot


    }
}
