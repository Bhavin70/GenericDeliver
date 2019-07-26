using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{  
    public interface ITblProductItemBL
    {
        List<TblProductItemTO> SelectAllTblProductItemList(Int32 specificationId = 0);
        TblProductItemTO SelectTblProductItemTO(Int32 idProdItem);
        TblProductItemTO SelectTblProductItemTO(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire);
        List<TblProductItemTO> SelectProductItemListStockTOList(int activeYn);
        List<TblProductItemTO> SelectProductItemList(Int32 idProdClass);
        List<DropDownTO> SelectProductItemListIsParity(Int32 isParity);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO, TblPurchaseItemMasterTO tblPurchaseItemMasterTO);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO, TblPurchaseItemMasterTO tblPurchaseItemMasterTO);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProductItem(Int32 idProdItem);
        int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        TblPurchaseItemMasterTO SelectTblPurchaseItemMasterTO(Int32 prodItemId);
        int InsertTblPurchaseItemMaster(TblPurchaseItemMasterTO tblPurchaseItemMasterTO);
        int UpdateTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO);
    }
}