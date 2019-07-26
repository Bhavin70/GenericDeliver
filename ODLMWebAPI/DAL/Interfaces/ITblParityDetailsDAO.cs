using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface ITblParityDetailsDAO
    { 
        String SqlSelectQuery();
        String SqlSimpleSelectQuery();
        List<TblParityDetailsTO> SelectAllTblParityDetails(int parityId, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran);
        List<TblParityDetailsTO> SelectAllTblParityDetails(String parityIds, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran);
        List<TblParityDetailsTO> SelectAllParityDetailsListByIds(String parityDtlIds, SqlConnection conn, SqlTransaction tran);
        List<TblParityDetailsTO> SelectAllLatestParityDetails(Int32 stateId, Int32 prodSpecId, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        TblParityDetailsTO SelectTblParityDetails(Int32 idParityDtl);
        TblParityDetailsTO SelectTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran);
        List<TblParityDetailsTO> SelectAllParityDetails();
        List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 brandId, Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo, Int32 productSpecForRegular);
        List<TblParityDetailsTO> SelectParityDetailToListOnBooking(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 brandId, Int32 stateId, DateTime boookingDate);
        List<TblParityDetailsTO> ConvertDTToList(SqlDataReader tblParityDetailsTODT);
        int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO);
        int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblParityDetailsTO tblParityDetailsTO, SqlCommand cmdInsert);
        int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO);
        int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblParityDetailsTO tblParityDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblParityDetails(Int32 idParityDtl);
        int DeleteTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idParityDtl, SqlCommand cmdDelete);
        int DeactivateAllParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateAllParityDetailsForUpdate(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        List<TblParityDetailsTO> SelectParityDetailsForBrand(Int32 fromBrand, Int32 toBrand, Int32 currencyId, Int32 categoryId, Int32 stateId);
        List<TblParityDetailsTO> GetParityDetailsForBrand(Int32 brandId);
    }
}