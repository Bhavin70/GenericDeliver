using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblGroupItemBL
    {
        List<TblGroupItemTO> SelectAllTblGroupItemList();
        TblGroupItemTO SelectTblGroupItemTO(Int32 idGroupItem);
        TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId);
        TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId, SqlConnection conn, SqlTransaction tran);
        List<TblGroupItemTO> SelectAllTblGroupItemDtlsList(Int32 groupId = 0);
        int InsertTblGroupItem(TblGroupItemTO tblGroupItemTO);
        int InsertTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO);
        int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblGroupItem(Int32 idGroupItem);
        int DeleteTblGroupItem(Int32 idGroupItem, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateProductGroupITem(List<TblGroupItemTO> tblGroupItemTOList, int loginUserId);
    }
}
