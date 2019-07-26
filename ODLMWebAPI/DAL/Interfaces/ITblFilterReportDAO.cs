using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblFilterReportDAO
    {
        String SqlSelectQuery();
        List<TblFilterReportTO> SelectAllTblFilterReport();
        TblFilterReportTO SelectTblFilterReport(Int32 idFilterReport);
        List<TblFilterReportTO> SelectTblFilterReportList(Int32 reportId);
        List<TblFilterReportTO> SelectAllTblFilterReport(SqlConnection conn, SqlTransaction tran);
        int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO);
        int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblFilterReportTO tblFilterReportTO, SqlCommand cmdInsert);
        int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO);
        int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblFilterReportTO tblFilterReportTO, SqlCommand cmdUpdate);
        int DeleteTblFilterReport(Int32 idFilterReport);
        int DeleteTblFilterReport(Int32 idFilterReport, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idFilterReport, SqlCommand cmdDelete);
        List<TblFilterReportTO> ConvertDTToList(SqlDataReader tblFilterReportTODT);

    }
}