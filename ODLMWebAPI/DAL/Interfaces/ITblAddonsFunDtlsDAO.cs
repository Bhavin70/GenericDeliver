using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblAddonsFunDtlsDAO
    {
        String SqlSelectQuery();
        List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtls();
        TblAddonsFunDtlsTO SelectTblAddonsFunDtls(int idAddonsfunDtls);
        List<TblAddonsFunDtlsTO> SelectAddonDetailsList(int transId, int ModuleId, String TransactionType, String PageElementId, String transIds);
        List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtls(SqlConnection conn, SqlTransaction tran);
        int InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO);
        int InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlCommand cmdInsert);
        int UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO);
        int UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblAddonsFunDtls(int idAddonsfunDtls);
        int DeleteTblAddonsFunDtls(int idAddonsfunDtls, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(int idAddonsfunDtls, SqlCommand cmdDelete);
        List<TblAddonsFunDtlsTO> ConvertDTToList(SqlDataReader tblAddonsFunDtlsTODT);
    }
}
