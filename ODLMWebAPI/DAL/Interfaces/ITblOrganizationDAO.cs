using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface ITblOrganizationDAO
    {
          String SqlSelectQuery();
          List<TblOrganizationTO> SelectAllTblOrganization();
          List<TblOrganizationTO> SelectAllTblOrganization(Int32 orgTypeId, Int32 parentId);
          List<TblOrganizationTO> SelectExistingAllTblOrganizationByRefIds(Int32 orgId, String overdueRefId,String enqRefId);
          List<TblOrganizationTO> SelectSaleAgentOrganizationList();
          List<TblOrganizationTO> SelectAllTblOrganization(StaticStuff.Constants.OrgTypeE orgTypeE);
          List<TblOrganizationTO> SelectAllTblOrganization(StaticStuff.Constants.OrgTypeE orgTypeE, SqlConnection conn, SqlTransaction tran);
          List<DropDownTO> SelectAllOrganizationListForDropDown(StaticStuff.Constants.OrgTypeE orgTypeE, TblUserRoleTO userRoleTO);
          List<DropDownTO> SelectAllOrganizationListForDropDownForRM(StaticStuff.Constants.OrgTypeE orgTypeE,Int32 RMId, TblUserRoleTO userRoleTO);
          List<DropDownTO> SelectAllSpecialCnfListForDropDown(TblUserRoleTO userRoleTO);
          List<DropDownTO> SelectDealerListForDropDown(Int32 cnfId, TblUserRoleTO tblUserRoleTO);
          List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId);
          TblOrganizationTO SelectTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran);
          Dictionary<int, string> SelectRegisteredMobileNoDCT(String orgIds, SqlConnection conn, SqlTransaction tran);
          Dictionary<int, string> SelectRegisteredMobileNoDCTByOrgType(String orgTypeId, SqlConnection conn, SqlTransaction tran);
          String SelectFirmNameOfOrganiationById(Int32 organizationId);
          List<TblOrganizationTO> ConvertDTToList(SqlDataReader tblOrganizationTODT);
          List<OrgExportRptTO> SelectAllOrgListToExport(Int32 orgTypeId, Int32 parentId);
          List<DropDownTO> SelectDealerListForDropDownForCRM(Int32 cnfId, TblUserRoleTO tblUserRoleTO);
          List<OrgExportRptTO> ConvertReaderToList(SqlDataReader tblOrganizationTODT);
          List<TblOrganizationTO> SelectOrganizationListByRegion(Int32 orgTypeId,Int32 districtId);
          TblOrganizationTO SelectTblOrganizationTOByEnqRefId(String enq_ref_id, SqlConnection conn, SqlTransaction tran);
          List<TblOrganizationTO> SelectAllOrganizationListV2();
          List<TblOrganizationTO> SimpleConvertDTtoList(SqlDataReader tblOrganizationTODT);
          int InsertTblOrganization(TblOrganizationTO tblOrganizationTO);
          int InsertTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteInsertionCommand(TblOrganizationTO tblOrganizationTO, SqlCommand cmdInsert);
          int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO);
          int UpdateTblOrganizationRefIds(TblOrganizationTO tblOrganizationTO);
          int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran);
          int PostUpdateOverdueExistOrNot(Int32 dealerOrgId, Int32 isOverdueExist);
          int ExecuteUpdationCommand(TblOrganizationTO tblOrganizationTO, SqlCommand cmdUpdate);
          int DeleteTblOrganization(Int32 idOrganization);
          int DeleteTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran);
          int ExecuteDeletionCommand(Int32 idOrganization, SqlCommand cmdDelete);
          TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization);
          List<DropDownTO> SelectSalesEngineerListForDropDown(Int32 orgId);
    }
}