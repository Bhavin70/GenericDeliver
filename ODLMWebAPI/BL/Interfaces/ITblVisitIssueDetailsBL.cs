using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVisitIssueDetailsBL
    {
        DataTable SelectAllTblVisitIssueDetails();
        List<TblVisitIssueDetailsTO> SelectAllTblVisitIssueDetailsList();
        TblVisitIssueDetailsTO SelectTblVisitIssueDetailsTO(Int32 idIssue);
        List<TblVisitIssueDetailsTO> ConvertDTToList(DataTable tblVisitIssueDetailsTODT);
        List<TblVisitIssueDetailsTO> SelectVisitIssueDetailsTOList(Int32 visitId);
        int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO);
        int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveVisitIssueDetails(List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTO, Int32 createdBy, Int32 visitId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO);
        int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateVisitIssueDetails(List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTO, Int32 updatedBy, Int32 visitId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVisitIssueDetails(Int32 idIssue);
        int DeleteTblVisitIssueDetails(Int32 idIssue, SqlConnection conn, SqlTransaction tran);
    }
}