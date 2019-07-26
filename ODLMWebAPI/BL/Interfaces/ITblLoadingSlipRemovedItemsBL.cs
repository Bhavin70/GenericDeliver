using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingSlipRemovedItemsBL
    {
        List<TblLoadingSlipRemovedItemsTO> SelectAllTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems);
        List<TblLoadingSlipRemovedItemsTO> SelectAllTblLoadingSlipRemovedItemsList(Int32 idLoadingSlipRemovedItems, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipRemovedItemsTO SelectTblLoadingSlipRemovedItemsTO(Int32 idLoadingSlipRemovedItems);
        int InsertTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO);
        int InsertTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO);
        int UpdateTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems);
        int DeleteTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems, SqlConnection conn, SqlTransaction tran);
    }
}