using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblIssueTypeBL
    {
        DataTable SelectAllTblIssueType();
        List<TblIssueTypeTO> SelectAllTblIssueTypeList();
        TblIssueTypeTO SelectTblIssueTypeTO(Int32 idIssueType);
        List<TblIssueTypeTO> ConvertDTToList(DataTable tblIssueTypeTODT);
        int InsertTblIssueType(TblIssueTypeTO tblIssueTypeTO);
        int InsertTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO);
        int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblIssueType(Int32 idIssueType);
        int DeleteTblIssueType(Int32 idIssueType, SqlConnection conn, SqlTransaction tran);
    }
}
