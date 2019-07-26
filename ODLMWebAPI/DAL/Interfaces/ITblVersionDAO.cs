using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVersionDAO
    {
        String SqlSelectQuery();
        List<TblVersionTO> SelectAllTblVersion();
        TblVersionTO SelectTblVersion(Int32 idVersion);
        TblVersionTO SelectLatestVersionTO();
        List<TblVersionTO> SelectAllTblVersion(SqlConnection conn, SqlTransaction tran);
        List<TblVersionTO> ConvertDTToList(SqlDataReader tblUserRoleTODT);
        int InsertTblVersion(TblVersionTO tblVersionTO);
        int InsertTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVersionTO tblVersionTO, SqlCommand cmdInsert);
        int UpdateTblVersion(TblVersionTO tblVersionTO);
        int UpdateTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVersionTO tblVersionTO, SqlCommand cmdUpdate);
        int DeleteTblVersion(Int32 idVersion);
        int DeleteTblVersion(Int32 idVersion, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVersion, SqlCommand cmdDelete);

    }
}