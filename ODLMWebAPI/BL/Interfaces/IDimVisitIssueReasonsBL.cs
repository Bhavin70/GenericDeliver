using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimVisitIssueReasonsBL
    {
        DataTable SelectAllDimVisitIssueReasons();
        List<DimVisitIssueReasonsTO> SelectAllDimVisitIssueReasonsList();
        DimVisitIssueReasonsTO SelectDimVisitIssueReasonsTO(Int32 idVisitIssueReasons);
        List<DimVisitIssueReasonsTO> ConvertDTToList(DataTable dimVisitIssueReasonsTODT);
        int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO);
        int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO);
        int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons);
        int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran);
    }
}
