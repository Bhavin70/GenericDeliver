using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblRunningSizesDAO
    {
        String SqlSelectQuery();
        List<TblRunningSizesTO> SelectAllTblRunningSizes();
        List<TblRunningSizesTO> SelectAllTblRunningSizes(DateTime stockDate, SqlConnection conn, SqlTransaction tran);
        TblRunningSizesTO SelectTblRunningSizes(Int32 idRunningSize);
        List<TblRunningSizesTO> ConvertDTToList(SqlDataReader tblRunningSizesTODT);
        int InsertTblRunningSizes(TblRunningSizesTO tblRunningSizesTO);
        int InsertTblRunningSizes(TblRunningSizesTO tblRunningSizesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblRunningSizesTO tblRunningSizesTO, SqlCommand cmdInsert);
        int UpdateTblRunningSizes(TblRunningSizesTO tblRunningSizesTO);
        int UpdateTblRunningSizes(TblRunningSizesTO tblRunningSizesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblRunningSizesTO tblRunningSizesTO, SqlCommand cmdUpdate);
        int DeleteTblRunningSizes(Int32 idRunningSize);
        int DeleteTblRunningSizes(Int32 idRunningSize, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idRunningSize, SqlCommand cmdDelete);

    }
}