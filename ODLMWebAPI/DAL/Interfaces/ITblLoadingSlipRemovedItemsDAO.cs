using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingSlipRemovedItemsDAO
    {
        String SqlSelectQuery();
        List<TblLoadingSlipRemovedItemsTO> SelectAllTblLoadingSlipRemovedItems();
        TblLoadingSlipRemovedItemsTO SelectTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems);
        List<TblLoadingSlipRemovedItemsTO> ConvertDTToList(SqlDataReader tblLoadingSlipRemovedItemsTODT);
        List<TblLoadingSlipRemovedItemsTO> SelectAllTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO);
        int InsertTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlCommand cmdInsert);
        int UpdateTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO);
        int UpdateTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems);
        int DeleteTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadingSlipRemovedItems, SqlCommand cmdDelete);

    }
}