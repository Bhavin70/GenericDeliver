using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingVehDocExtDAO
    {
        String SqlSelectQuery();
        List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExt();
        List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExtList(Int32 loadingId, Int32 ActiveYnAll);
        TblLoadingVehDocExtTO SelectTblLoadingVehDocExt(Int32 idLoadingVehDocExt);
        List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExt(SqlConnection conn, SqlTransaction tran);
        List<TblLoadingVehDocExtTO> ConvertDTToList(SqlDataReader tblLoadingVehDocExtTODT);
        int InsertTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO);
        int InsertTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlCommand cmdInsert);
        int UpdateTblLoadingVehDocExtActiveYn(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO);
        int UpdateTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingVehDocExt(Int32 idLoadingVehDocExt);
        int DeleteTblLoadingVehDocExt(Int32 idLoadingVehDocExt, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadingVehDocExt, SqlCommand cmdDelete);

    }
}