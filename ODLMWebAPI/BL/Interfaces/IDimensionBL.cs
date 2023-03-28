using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface IDimensionBL
    { 
        List<DropDownTO> GetAllOrganizationType();
        List<DropDownTO> SelectDeliPeriodForDropDown();
        List<StateMasterTO> GetDistrictsForStateMaster(int stateId);
        List<DropDownTO> SelectCDStructureForDropDown(Int32 moduleId);
        DropDownTO SelectCDDropDown(Int32 cdStructureId);
        List<Dictionary<string, string>> GetColumnName(string tableName, Int32 tableValue);
        List<DropDownTO> SelectCountriesForDropDown();
        List<DropDownTO> SelectStatesForDropDown(int countryId);
        List<DropDownTO> SelectDistrictForDropDown(int stateId);
        List<DropDownTO> SelectTalukaForDropDown(int districtId);
        List<StateMasterTO> GetTalukasForStateMaster(int districtId);
        List<DropDownTO> SelectOrgLicensesForDropDown();
        List<DropDownTO> SelectSalutationsForDropDown();
        //Priyanka [23-04-2019]
        List<DropDownTO> GetSAPMasterDropDown(int dimensionId);
        
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
        List<DimensionTO> SelectAllMasterDimensionList();
        List<DropDownTO> SelectDefaultRoleListForDropDown();
        Int32 saveNewDimensional(Dictionary<string, string> tableData, string tableName);
        Int32 UpdateDimensionalData(Dictionary<string, string> tableData, string tableName);
        Int32 getIdentityOfTable(string columnName, string tableName);
        DimFinYearTO GetCurrentFinancialYear(DateTime curDate, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetReportingType();
        List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList();
        List<DropDownTO> SelectBrandList();
        List<DimBrandTO> SelectBrandListV2();
        List<DropDownTO> SelectLoadingLayerList();
        List<DropDownTO> GetBookingTaxCategoryList();
        List<DropDownTO> GetBookingCommentCategoryList();
        DropDownTO SelectStateCode(Int32 stateId);
        List<DropDownTO> GetItemProductCategoryListForDropDown();
        List<DropDownTO> GetInvoiceStatusDropDown();
        List<DropDownTO> GetAllFirmTypesForDropDown();
        List<DropDownTO> GetAllInfluencerTypesForDropDown();
        List<DropDownTO> SelectAllEnquiryChannels();
        List<DropDownTO> SelectAllIndustrySector();
        List<DropDownTO> GetCallBySelfForDropDown();
        List<DropDownTO> GetArrangeForDropDown();
        List<DropDownTO> GetArrangeVisitToDropDown();
        List<DropDownTO> GetLocationWiseCompartmentList();
        List<DropDownTO> SelectAddressTypeListForDropDown();
        List<RoleOrgTO> SelectAllSystemRoleListForTbl(int visitTypeId, int personTypeId);
        List<RoleOrgTO> SelectAllSystemOrgListForTbl(int visitTypeId, int personTypeId);
        List<DropDownTO> SelectAllVisitTypeListForDropDown();
        List<DropDownTO> GetFixedDropDownValues();
        List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId);
        int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran);
        TblEntityRangeTO SelectEntityRangeTOFromVisitType(string entityName, DateTime createdOn);
        List<DropDownTO> SelectStatesForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate);
        List<DropDownTO> SelectDistrictForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate);
        List<DropDownTO> GetUserListDepartmentWise(string deptId);
        List<DropDownTO> SelectInvoiceCopyList();

        DimFinYearTO GetCurrentFinancialYear(DateTime curDate);
        List<DimExportTypeTO> GetExportTypeList();
        List<DimIndustrySegmentTO> GetIndustryTypeList();
        List<DimIndustrySegmentTypeTO> GetIndustrySegmentTypeList(Int32 industrySegmentId);
    }
}
