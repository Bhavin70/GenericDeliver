using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface ITblParityDetailsBL
    {
        List<TblParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, Int32 prodSpecId, Int32 stateId, Int32 brandId);
        List<TblParityDetailsTO> SelectAllEmptyParityDetailsList(Int32 prodSpecId, Int32 stateId, Int32 brandId);
        List<TblParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, int prodSpecId, SqlConnection conn, SqlTransaction tran);
        List<TblParityDetailsTO> SelectAllTblParityDetailsList(String parityIds, int prodSpecId, SqlConnection conn, SqlTransaction tran);
        TblParityDetailsTO SelectTblParityDetailsTO(Int32 idParityDtl);
        TblParityDetailsTO SelectTblParityDetailsTO(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran);
        List<TblParityDetailsTO> SelectAllParityDetailsListByIds(String parityDtlIds, SqlConnection conn, SqlTransaction tran);
        List<TblParityDetailsTO> SelectAllParityDetailsList();
        ResultMessage SaveParityDetailsOtherItem(TblParitySummaryTO tblParitySummaryTO, Int32 isForUpdate);
        Int32 DeactivateParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        Int32 DeactivateParityDetailsForUpdate(TblParityDetailsTO parityDetailsTO, SqlConnection conn, SqlTransaction tran);
        List<TblParityDetailsTO> SelectAllParityDetailsToList(Int32 brandId, Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo, Int32 productSpecForRegular, Int32 districtId, Int32 talukaId);
        List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 brandId, Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo, Int32 productSpecForRegular, Int32 districtId, Int32 talukaId);
        TblParityDetailsTO SelectParityDetailToListOnBooking(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 brandId, Int32 stateId, DateTime boookingDate, Int32 districtId, Int32 talukaId); //02-12-2020 Dhananjay added districtId and talukaId
        TblParityDetailsTO CreateEmptyParityDetailsTo(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 brandId, Int32 stateId, DateTime boookingDate);
        TblParityDetailsTO GetParityDetailToOnBooking(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 brandId, Int32 stateId, DateTime boookingDate, Int32 districtId, Int32 talukaId, Int32 parityLevel);//29-12-2020 Dhananjay added
        List<TblParityDetailsTO> GetParityDetailToListOnBooking(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 brandId, Int32 stateId, DateTime boookingDate, Int32 districtId, Int32 talukaId, Int32 parityLevel);//29-12-2020 Dhananjay added
        int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO);
        int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO);
        int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblParityDetails(Int32 idParityDtl);
        int DeleteTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran);
        ResultMessage GetParityDetialsForCopyBrand(Int32 brandId, List<DropDownToForParity> selectedBrands, List<DropDownToForParity> selectedStates);
        List<TblParityDetailsTO> GetCurrentParityDetailToListOnBooking(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 brandId, Int32 stateId, DateTime boookingDate, Int32 districtId, Int32 talukaId, Int32 parityLevel);
    }
}