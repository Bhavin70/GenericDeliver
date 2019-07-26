using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblItemTallyRefDtlsDAO
    {
        String SqlSelectQuery();
        List<TblItemTallyRefDtlsTO> SelectAllTblItemTallyRefDtls();
        TblItemTallyRefDtlsTO SelectTblItemTallyRefDtls(Int32 idItemTallyRef);
        List<TblItemTallyRefDtlsTO> SelectAllTblItemTallyRefDtls(SqlConnection conn, SqlTransaction tran);
        List<TblItemTallyRefDtlsTO> ConvertDTToList(SqlDataReader tblItemTallyRefDtlsTODT);
        List<TblItemTallyRefDtlsTO> SelectEmptyTblItemTallyRefDtlsTOTemplate(Int32 brandId);
        List<TblItemTallyRefDtlsTO> ConvertDTToListEmty(SqlDataReader tblItemTallyRefDtlsTODT);
        List<TblItemTallyRefDtlsTO> SelectExistingAllItemTallyByRefIds(String overdueTallyRefId, String enquiryTallyRefId);
        TblItemTallyRefDtlsTO SelectExistingAllTblItemRef(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO);
        int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO);
        int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlCommand cmdInsert);
        int UpdateTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO);
        int UpdateTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblItemTallyRefDtls(Int32 idItemTallyRef);
        int DeleteTblItemTallyRefDtls(Int32 idItemTallyRef, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idItemTallyRef, SqlCommand cmdDelete);

    }
}