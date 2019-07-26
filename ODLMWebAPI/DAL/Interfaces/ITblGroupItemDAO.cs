using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblGroupItemDAO
    {
        String SqlSelectQuery();
        List<TblGroupItemTO> SelectAllTblGroupItem();
        TblGroupItemTO SelectTblGroupItem(Int32 idGroupItem);
        TblGroupItemTO SelectAllTblGroupItem(SqlConnection conn, SqlTransaction tran);
        List<TblGroupItemTO> ConvertDTToList(SqlDataReader tblGroupItemTODT);
        TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId);
        TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId, SqlConnection conn, SqlTransaction tran);
        List<TblGroupItemTO> SelectAllTblGroupItemDtlsList(Int32 groupId, SqlConnection conn, SqlTransaction tran);
        int InsertTblGroupItem(TblGroupItemTO tblGroupItemTO);
        int InsertTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblGroupItemTO tblGroupItemTO, SqlCommand cmdInsert);
        int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO);
        int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblGroupItemTO tblGroupItemTO, SqlCommand cmdUpdate);
        int DeleteTblGroupItem(Int32 idGroupItem);
        int DeleteTblGroupItem(Int32 idGroupItem, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idGroupItem, SqlCommand cmdDelete);

    }
}