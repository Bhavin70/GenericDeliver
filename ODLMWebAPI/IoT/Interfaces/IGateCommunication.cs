using ODLMWebAPI.Models;
using SalesTrackerAPI.IoT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ODLMWebAPI.IoT.Interfaces
{
    public interface IGateCommunication
    {
        GateIoTResult DeleteAllLoadingFromGateIoT();

        GateIoTResult DeleteSingleLoadingFromGateIoT(TblLoadingTO tblLoadingTO);

        GateIoTResult GetAllLoadingSlipsByStatusFromIoT(Int32 statusId, TblGateTO tblGateTO, Int32 startLoadingId = 1);

        GateIoTResult GetLoadingSlipsByStatusFromIoT(Int32 statusId, TblGateTO tblGateTO, Int32 startLoadingId = 1);

        GateIoTResult GetLoadingStatusHistoryDataFromGateIoT(TblLoadingTO tblLoadingTO);

        int PostGateApiCalls(TblLoadingTO tblLoadingTO, object[] writeData, HttpWebRequest tRequest);

        int PostGateAPIDataToModbusTcpApiForLoadingSlip(TblLoadingTO tblLoadingTO, Object[] writeData);
    }
}
