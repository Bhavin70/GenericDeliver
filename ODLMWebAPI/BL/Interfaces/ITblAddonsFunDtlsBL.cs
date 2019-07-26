using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblAddonsFunDtlsBL
    {
        List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtlsList();
        TblAddonsFunDtlsTO SelectTblAddonsFunDtlsTO(int idAddonsfunDtls);
        List<TblAddonsFunDtlsTO> SelectAddonDetails(int transId, int ModuleId, String TransactionType, String PageElementId, String transIds);
        ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO);
        ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO);
        ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAddonsFunDtls(int idAddonsfunDtls);
        int DeleteTblAddonsFunDtls(int idAddonsfunDtls, SqlConnection conn, SqlTransaction tran);
    }
}
