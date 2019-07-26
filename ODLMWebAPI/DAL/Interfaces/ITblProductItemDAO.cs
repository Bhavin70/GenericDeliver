using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{    
    public interface ITblProductItemDAO
    {
        String SqlSelectQuery();
        List<TblProductItemTO> SelectAllTblProductItem(Int32 specificationId = 0);
        TblProductItemTO SelectTblProductItem(Int32 idProdItem, SqlConnection conn = null, SqlTransaction tran = null);
        List<TblProductItemTO> ConvertDTToList(SqlDataReader tblProductItemTODT);
        List<TblProductItemTO> ConvertDTToListForUpdate(SqlDataReader tblProductItemTODT);
        List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire);
        List<TblProductItemTO> SelectProductItemListStockTOList(int activeYn);
        List<TblProductItemTO> SelectListOfProductItemTOOnprdClassId(string prodClassIds);
        List<DropDownTO> SelectProductItemListIsParity(Int32 isParity);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO, TblPurchaseItemMasterTO tblPurchaseItemMasterTO);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdInsert);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdUpdate);
        int ExecuteUpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlCommand cmdUpdate);
        int DeleteTblProductItem(Int32 idProdItem);
        int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProdItem, SqlCommand cmdDelete);
        int updatePreviousBase(SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseItemMaster(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateTblPurchaseItemMaster(int prodItemId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran);
        TblPurchaseItemMasterTO SelectTblPurchaseItemMaster(Int32 prodItemId);
    }
}