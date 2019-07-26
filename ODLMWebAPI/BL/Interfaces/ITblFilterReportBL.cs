using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblFilterReportBL
    {
        List<TblFilterReportTO> SelectAllTblFilterReport();
        List<TblFilterReportTO> SelectAllTblFilterReportList();
        TblFilterReportTO SelectTblFilterReportTO(Int32 idFilterReport);
        List<TblFilterReportTO> SelectTblFilterReportList(Int32 reportId);
        int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO);
        int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO);
        int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblFilterReport(Int32 idFilterReport);
        int DeleteTblFilterReport(Int32 idFilterReport, SqlConnection conn, SqlTransaction tran);
    }
}
