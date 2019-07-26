using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblWeighingBL
    {
        List<TblWeighingTO> SelectAllTblWeighing();
        TblWeighingTO SelectTblWeighingTO(Int32 idWeighing);
        TblWeighingTO SelectTblWeighingByMachineIp(string ipAddr);
        int InsertTblWeighing(TblWeighingTO tblWeighingTO);
        int InsertTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblWeighing(TblWeighingTO tblWeighingTO);
        int UpdateTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblWeighing(Int32 idWeighing);
        int DeleteTblWeighingByByMachineIp(string ipAddr);
        int DeleteTblWeighing(Int32 idWeighing, SqlConnection conn, SqlTransaction tran);
    }
}