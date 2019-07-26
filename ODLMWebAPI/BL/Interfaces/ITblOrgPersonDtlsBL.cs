using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOrgPersonDtlsBL
    {
        List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtls();
        List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtlsList();
        TblOrgPersonDtlsTO SelectTblOrgPersonDtlsTO(Int32 idOrgPersonDtl);
        int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO);
        int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO);
        int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl);
        int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl, SqlConnection conn, SqlTransaction tran);
    }
}