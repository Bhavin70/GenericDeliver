using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface IDimensionDAO
    {  
        List<DropDownTO> SelectDeliPeriodForDropDown();
        List<Dictionary<string, string>> GetColumnName(string tablename, Int32 tableValue);
        Int32 InsertdimentionalData(string tableQuery);
        List<DimensionTO> SelectAllMasterDimensionList();
        Int32 getidentityOfTable(string Query);
        Int32 getMaxCountOfTable(string CoulumName, string tableName);
        List<DropDownTO> SelectCDStructureForDropDown(Int32 isRsOrPerncent);
        List<DropDownTO> SelectCountriesForDropDown();
        List<DropDownTO> SelectOrgLicensesForDropDown();
        List<DropDownTO> SelectSalutationsForDropDown();
        List<StateMasterTO> SelectDistrictForStateMaster(int stateId);
        //Priyanka [23-04-2019]
        List<DropDownTO> GetSAPMasterDropDown(int dimensionId);
        
        List<DropDownTO> SelectDistrictForDropDown(int stateId);
        List<DropDownTO> SelectStatesForDropDown(int countryId);
        List<DropDownTO> SelectTalukaForDropDown(int districtId);
        List<StateMasterTO> SelectTalukaForStateMaster(int districtId);
        List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown();
        List<DropDownTO> SelectAllSystemRoleListForDropDown();
        //Aniket [1-7-2019]
        List<DropDownTO> SelectAllSystemRoleListForDropDownByUserId(Int32 userId);
        List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId);
        List<DropDownTO> SelectAllTransportModeForDropDown();
        List<DropDownTO> SelectInvoiceTypeForDropDown();
        List<DropDownTO> SelectInvoiceModeForDropDown();
        List<DropDownTO> SelectCurrencyForDropDown();
        List<DropDownTO> GetInvoiceStatusForDropDown();
        List<DimFinYearTO> SelectAllMstFinYearList(SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectReportingType();
        List<DimVisitIssueReasonsTO> SelectVisitIssueReasonsList();
        List<DropDownTO> SelectBrandList();
        List<DropDownTO> SelectLoadingLayerList();
        DropDownTO SelectStateCode(Int32 stateId);
        List<DropDownTO> GetItemProductCategoryListForDropDown();
        List<DropDownTO> GetInvoiceStatusDropDown();
        List<DropDownTO> SelectAllFirmTypesForDropDown();
        List<DropDownTO> SelectAllInfluencerTypesForDropDown();
        List<DropDownTO> SelectAllEnquiryChannels();
        List<DropDownTO> SelectAllIndustrySector();
        List<DropDownTO> GetCallBySelfForDropDown();
        List<DropDownTO> GetArrangeForDropDown();
        List<DropDownTO> GetArrangeVisitToDropDown();
        List<DropDownTO> SelectAllOrganizationType();
        List<DropDownTO> SelectAddressTypeListForDropDown();
        DropDownTO SelectCDDropDown(Int32 cdStructureId);
        List<DropDownTO> SelectAllVisitTypeListForDropDown();
        List<DropDownTO> SelectDefaultRoleListForDropDown();
        List<DropDownTO> SelectStatesForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate);
        List<DropDownTO> SelectDistrictForDropDownAccToBooking(int stateId, DateTime fromDate, DateTime toDate);
        List<DropDownTO> GetFixedDropDownList();
        List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId);
        int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran);
        int InsertMstFinYear(DimFinYearTO newMstFinYearTO, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetUserListDepartmentWise(string deptId);
        List<DropDownTO> SelectAllInvoiceCopyList();

        List<DimFinYearTO> SelectAllMstFinYearList();

        int InsertMstFinYear(DimFinYearTO newMstFinYearTO);

        List<TblProdGstCodeDtlsTO> GetSAPTaxCodeByIdProdGstCode(int idProdGstCode);

        Int64 GetProductItemIdFromGivenRMDetails(int prodCatId, int prodSpecId, int materialId, int brandId, int rmProdItemId);
    }
}