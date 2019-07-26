using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblStockDetailsBL
    {
        List<TblStockDetailsTO> SelectAllTblStockDetailsList();
        List<TblStockDetailsTO> SelectAllTblStockDetailsListConsolidated(Int32 isConsolidated, Int32 brandId);
        List<TblStockDetailsTO> SelectAllTblStockDetailsListConsolidated(Int32 isConsolidated, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectStockDetailsListByProdCatgAndSpec(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate);
        List<TblStockDetailsTO> SelectStockDetailsListByProdCatgSpecAndMaterial(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, DateTime stockDate);
        List<TblStockDetailsTO> SelectStockDetailsListByProdCatgSpecAndMaterial(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, DateTime stockDate, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectAllTblStockDetailsList(Int32 stockSummaryId);
        List<TblStockDetailsTO> SelectAllTblStockDetailsList(Int32 stockSummaryId, SqlConnection conn, SqlTransaction tran);
        TblStockDetailsTO SelectTblStockDetailsTO(Int32 idStockDtl);
        TblStockDetailsTO SelectTblStockDetailsTO(Int32 idStockDtl, SqlConnection conn, SqlTransaction tran);
        TblStockDetailsTO SelectTblStockDetailsTO(TblRunningSizesTO runningSizeTO, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectAllEmptyStockTemplateList(int prodCatId, int locationId, int brandId, Int32 isConsolidateStk);
        List<TblStockDetailsTO> SelectAllTblStockDetailsList(int locationId, int prodCatId, DateTime stockDate, int brandId);
        List<SizeSpecWiseStockTO> SelectSizeAndSpecWiseStockSummary(DateTime stockDate, int compartmentId);
        List<TblStockDetailsTO> SelectAllTblStockDetails(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate, Int32 brandId, int compartmentId, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectAllTblStockDetailsOther(Int32 prodCatId, Int32 prodSpecId, Int32 prodItemId, DateTime stockDate, Int32 brandId, int compartmentId, SqlConnection conn, SqlTransaction tran);
        Double SelectTotalBalanceStock(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 brandId);
        List<TblStockDetailsTO> SelectTblStockDetailsList(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 brandId, int compartmentId = 0, int prodItemId = 0);
        int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO);
        int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO);
        int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblStockDetails(Int32 idStockDtl);
        int DeleteTblStockDetails(Int32 idStockDtl, SqlConnection conn, SqlTransaction tran);
    }
}