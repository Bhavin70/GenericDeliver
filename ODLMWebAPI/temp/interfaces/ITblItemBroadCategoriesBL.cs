using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblItemBroadCategoriesBL
    {
        List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategories();
        List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategoriesList();
        TblItemBroadCategoriesTO SelectTblItemBroadCategoriesTO(Int32 iditemBroadCategories);
        int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO);
        int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO);
        int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblItemBroadCategories(Int32 iditemBroadCategories);
        int DeleteTblItemBroadCategories(Int32 iditemBroadCategories, SqlConnection conn, SqlTransaction tran);
    }
}
