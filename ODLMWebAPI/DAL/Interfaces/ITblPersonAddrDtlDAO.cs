using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblPersonAddrDtlDAO
    {
        String SqlSelectQuery();
        List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtl();
        TblPersonAddrDtlTO SelectTblPersonAddrDtl(Int32 idPersonAddrDtl);
        TblPersonAddrDtlTO SelectTblPersonAddrDtl(Int32 personId, Int32 addressTypeId);
        List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtl(SqlConnection conn, SqlTransaction tran);
        int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO);
        int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlCommand cmdInsert);
        int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO);
        int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlCommand cmdUpdate);
        int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl);
        int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPersonAddrDtl, SqlCommand cmdDelete);
        List<TblPersonAddrDtlTO> ConvertDTToList(SqlDataReader tblPersonAddrDtlTODT);

    }
}