using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.IoT.Interfaces
{
    public interface IWeighingCommunication
    {
        int PostDataFrommodbusTcpApi(TblLoadingTO tblLoadingTO, int[] writeData, TblWeighingMachineTO machineTO);

        NodeJsResult GetLoadingLayerData(int loadingId, int layerId, TblWeighingMachineTO machineTO);

        NodeJsResult DeleteSingleLoadingFromWeightIoT(int loadingId, int layerId, TblWeighingMachineTO machineTO);

        NodeJsResult DeleteAllLoadingFromWeightIoT(string ioTUrl);
    }
}
