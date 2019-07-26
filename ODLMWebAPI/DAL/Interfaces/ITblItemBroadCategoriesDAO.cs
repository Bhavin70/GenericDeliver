using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblItemBroadCategoriesDAO
    {
        String SqlSelectQuery();
        List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategories();
        TblItemBroadCategoriesTO SelectTblItemBroadCategories(Int32 iditemBroadCategories);
        List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategories(SqlConnection conn, SqlTransaction tran);
        int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO);
        int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlCommand cmdInsert);
        int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO);
        int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlCommand cmdUpdate);
        int DeleteTblItemBroadCategories(Int32 iditemBroadCategories);
        int DeleteTblItemBroadCategories(Int32 iditemBroadCategories, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 iditemBroadCategories, SqlCommand cmdDelete);
        List<TblItemBroadCategoriesTO> ConvertDTToList(SqlDataReader tblItemBroadCategoriesTODR);

    }
}