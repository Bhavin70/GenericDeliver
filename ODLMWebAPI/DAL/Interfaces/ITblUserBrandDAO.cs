using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblUserBrandDAO
    {
        String SqlSelectQuery();
        List<TblUserBrandTO> SelectAllTblUserBrand();
        List<TblUserBrandTO> SelectAllTblUserBrand(int isActive);
        List<TblUserBrandTO> SelectAllTblUserBrandByCnfId(Int32 cnfId);
        TblUserBrandTO SelectTblUserBrand(Int32 idUserBrand);
        List<TblUserBrandTO> SelectAllTblUserBrand(SqlConnection conn, SqlTransaction tran);
        List<TblUserBrandTO> ConvertDTToList(SqlDataReader tblUserBrandTODT);
        int InsertTblUserBrand(TblUserBrandTO tblUserBrandTO);
        int InsertTblUserBrand(TblUserBrandTO tblUserBrandTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserBrandTO tblUserBrandTO, SqlCommand cmdInsert);
        int UpdateTblUserBrand(TblUserBrandTO tblUserBrandTO);
        int UpdateTblUserBrand(TblUserBrandTO tblUserBrandTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUserBrandTO tblUserBrandTO, SqlCommand cmdUpdate);
        int DeleteTblUserBrand(Int32 idUserBrand);
        int DeleteTblUserBrand(Int32 idUserBrand, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idUserBrand, SqlCommand cmdDelete);

    }
}