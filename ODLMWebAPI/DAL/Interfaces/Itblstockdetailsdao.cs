using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblStockDetailsDAO
    {
        String SqlSelectQuery();
        List<TblStockDetailsTO> SelectAllTblStockDetails();
        List<TblStockDetailsTO> SelectAllTblStockDetailsConsolidated(Int32 isConsolidated, Int32 brandId);
        List<TblStockDetailsTO> SelectAllTblStockDetailsConsolidated(Int32 isConsolidated, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectAllTblStockDetails(Int32 stockSummaryId, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectAllTblStockDetails(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate, Int32 brandId, int compartmentId, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectAllTblStockDetailsOther(Int32 prodCatId, Int32 prodSpecId, Int32 prodItemId, Int32 brandId, int compartmentId, DateTime stockDate, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectAllTblStockDetails(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectAllTblStockDetails(Int32 prodCatId, Int32 prodSpecId, int materialId, DateTime stockDate, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> SelectAllTblStockDetails(int locationId, int prodCatId, DateTime stockDate, int brandId);
        List<TblStockDetailsTO> SelectEmptyStockDetailsTemplate(int prodCatId, int locationId, int brandId, Int32 isConsolidate);
        TblStockDetailsTO SelectTblStockDetails(Int32 idStockDtl, SqlConnection conn, SqlTransaction tran);
        TblStockDetailsTO SelectTblStockDetails(TblRunningSizesTO runningSizeTO, SqlConnection conn, SqlTransaction tran);
        List<TblStockDetailsTO> ConvertDTToList(SqlDataReader tblStockDetailsTODT);
        List<TblStockDetailsTO> ConvertReaderToList(SqlDataReader tblStockDetailsTODT, int locationId);
        List<SizeSpecWiseStockTO> ConvertReaderToStockList(SqlDataReader tblStockDetailsTODT);
        List<SizeSpecWiseStockTO> SelectSizeAndSpecWiseStockSummary(DateTime stockDate, int compartmentId);
        Double SelectTotalBalanceStock(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 brandId);
        List<TblStockDetailsTO> SelectTblStockDetailsList(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 brandId, int compartmentId, int prodItemId);
        int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO);
        int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblStockDetailsTO tblStockDetailsTO, SqlCommand cmdInsert);
        int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO);
        int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblStockDetailsTO tblStockDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblStockDetails(Int32 idStockDtl);
        int DeleteTblStockDetails(Int32 idStockDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStockDtl, SqlCommand cmdDelete);

    }
}