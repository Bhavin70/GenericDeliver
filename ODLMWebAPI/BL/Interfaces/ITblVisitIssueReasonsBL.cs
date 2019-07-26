using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVisitIssueReasonsBL
    {
        DataTable SelectAllTblVisitIssueReasons();
        List<TblVisitIssueReasonsTO> SelectAllTblVisitIssueReasonsList();
        List<TblVisitIssueReasonsTO> SelectAllVisitIssueReasonsListForDropDown();
        TblVisitIssueReasonsTO SelectTblVisitIssueReasonsTO(Int32 idVisitIssueReasons);
        List<TblVisitIssueReasonsTO> ConvertDTToList(DataTable tblVisitIssueReasonsTODT);
        int InsertTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO);
        int InsertTblVisitIssueReasons(ref TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO);
        int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons);
        int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran);
    }
}