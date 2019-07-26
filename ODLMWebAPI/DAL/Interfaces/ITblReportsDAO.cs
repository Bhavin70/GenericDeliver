using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblReportsDAO
    {
        String SqlSelectQuery();
        List<TblReportsTO> SelectAllTblReports();
        TblReportsTO SelectTblReports(Int32 idReports);
        List<TblReportsTO> SelectAllTblReports(SqlConnection conn, SqlTransaction tran);
        int InsertTblReports(TblReportsTO tblReportsTO);
        int InsertTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblReportsTO tblReportsTO, SqlCommand cmdInsert);
        int UpdateTblReports(TblReportsTO tblReportsTO);
        int UpdateTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblReportsTO tblReportsTO, SqlCommand cmdUpdate);
        int DeleteTblReports(Int32 idReports);
        int DeleteTblReports(Int32 idReports, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idReports, SqlCommand cmdDelete);
        List<TblReportsTO> ConvertDTToList(SqlDataReader tblReportsTODT);
        List<DynamicReportTO> GetDynamicSqlData(string connectionstring, string sql);
        List<DynamicReportTO> SqlDataReaderToList(SqlDataReader reader);

    }
}