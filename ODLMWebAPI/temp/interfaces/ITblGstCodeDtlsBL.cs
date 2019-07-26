using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblGstCodeDtlsBL
    {
        List<TblGstCodeDtlsTO> SelectAllTblGstCodeDtlsList();
        TblGstCodeDtlsTO SelectTblGstCodeDtlsTO(Int32 idGstCode);
        TblGstCodeDtlsTO SelectTblGstCodeDtlsTO(Int32 idGstCode, SqlConnection conn, SqlTransaction tran);
        TblGstCodeDtlsTO SelectGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId);
        int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO);
        int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO);
        int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblGstCodeDtls(Int32 idGstCode);
        int DeleteTblGstCodeDtls(Int32 idGstCode, SqlConnection conn, SqlTransaction tran);
    }
}
