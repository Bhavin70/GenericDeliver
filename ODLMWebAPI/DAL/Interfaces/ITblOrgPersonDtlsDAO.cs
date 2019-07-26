using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblOrgPersonDtlsDAO
    {
        String SqlSelectQuery();
        List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtls();
        TblOrgPersonDtlsTO SelectTblOrgPersonDtls(Int32 idOrgPersonDtl);
        List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtls(SqlConnection conn, SqlTransaction tran);
        int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO);
        int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlCommand cmdInsert);
        int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO);
        int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl);
        int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOrgPersonDtl, SqlCommand cmdDelete);
        List<TblOrgPersonDtlsTO> ConvertDTToList(SqlDataReader sqlDataReader);

    }
}