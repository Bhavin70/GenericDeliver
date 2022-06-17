using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
 
namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOrgStructureBL
    {
        List<TblOrgStructureTO> SelectAllTblOrgStructureList();
        TblOrgStructureTO SelectBOMOrgStructure();
        List<TblOrgStructureTO> SelectAllOrgStructureList();
        List<TblUserReportingDetailsTO> GetOrgStructureUserDetailsForBom(Int16 orgStructureId);
        List<TblUserReportingDetailsTO> GetOrgStructureUserDetails(Int16 orgStructureId);
        List<TblUserReportingDetailsTO> GetOrgStructureUserDetails(Int16 orgStructureId, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateUserReportingDetailsBom(List<TblUserReportingDetailsTO> tblUserReportingDetailsTO, Int32 userReportingId);
        List<TblOrgStructureTO> SelectAllOrgStructureList(SqlConnection conn, SqlTransaction tran);
        List<DimMstDeptTO> SelectAllDimMstDeptList(SqlConnection conn, SqlTransaction tran);
        List<TblOrgStructureHierarchyTO> SelectTblOrgStructureHierarchyOnReportingType(int reportingTypeId);
        TblOrgStructureHierarchyTO SelectTblOrgStructureHierarchyTO(int orgStrctureId, int parentOrgStrctureId, int reportingTypeId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectReportingToUserList(Int32 orgStructureId, Int32 type);
        void GetParentPositionIds(int orgStructureId, ref string id, int type);
        DropDownTO SelectParentUserOnUserId(int userId, int reportingTypeId);
        List<TblOrgStructureTO> SelectOrgStuctureListForHierarchy(int reportingTypeId);
        List<TblOrgStructureHierarchyTO> SelectAllTblOrgStructureHierarchy();
        List<TblOrgStructureTO> GetOrgStructureListForDisplayTree();
        void UserwiseOrgChartTree(List<TblOrgStructureTO> OrgStructureTOList, List<TblUserReportingDetailsTO> alluserReportingDetailsTOList, String ParentId, List<TblOrgStructureTO> orgStructureTOListUserDisplay, Int32 userId, String newParent, Int32 uniquiNo);
        void GetOrgTree(String ParentId, Int32 DeptId, List<TblOrgStructureTO> OrgStructureTOList, List<DimMstDeptTO> MstDeptTOList, Int32 deptId);
        TblOrgStructureTO SelectTblOrgStructure(int idOrgStructure);
        List<DimLevelsTO> GetAllLevelsToList();
        String GetDeptIds(List<DimMstDeptTO> allDeptList, int deptId, string ids);
        List<TblOrgStructureTO> GetAllchildPositionList(int idOrgstructure, int reportingTypeId);
        List<TblOrgStructureTO> GetNotLinkedPositionsList();
        List<TblOrgStructureTO> SelectPositionLinkDetails(int idOrgStructure);
        List<TblOrgStructureTO> SelectChildPositions(int idOrgStructure);
        ResultMessage PostDelinkPosition(int SelfOrgId, int ParentPositionId);
        List<TblUserReportingDetailsTO> SelectAllUserReportingDetails();
        List<TblUserReportingDetailsTO> SelectAllUserReportingDetails(SqlConnection conn, SqlTransaction tran);
        TblUserReportingDetailsTO SelectUserReportingDetailsTO(int IdUserReportingDetails);
        TblUserReportingDetailsTO SelectUserReportingDetailsTO(int IdUserReportingDetails, SqlConnection conn, SqlTransaction tran);
        object ChildUserListOnUserId(Int32 userId, int? isObjectType, int reportingType);
        void GetUserIdsOnParentId(List<TblUserReportingDetailsTO> allUserReportinglist, Int32 parentId, ref string userIds, int reportingType);
        int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO);
        int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveOrganizationStructureHierarchy(TblOrgStructureTO tblOrgStructureTO);
        ResultMessage AttachNewUserToOrgStructure(TblUserReportingDetailsTO tblUserReportingDetailsTO, List<TblUserRoleTO> deactivatePosList);
        ResultMessage AttachNewUserToOrgStructure(TblUserReportingDetailsTO tblUserReportingDetailsTO, List<TblUserReportingDetailsTO> deactivatePosList, SqlConnection conn, SqlTransaction tran);
        int InsertTblOrgStructureHierarchy(TblOrgStructureHierarchyTO orgStructureHierarchyTO);
        int InsertTblOrgStructureHierarchy(TblOrgStructureHierarchyTO orgStructureHierarchyTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO);
        int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeactivateOrgStructure(TblOrgStructureTO tblOrgStructureTO, Int32 ReportingTypeId);
        ResultMessage UpdateUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO);
        int UpdateUserReportingDetail(TblUserReportingDetailsTO tblUserReportingDetailsTO);
        int UpdateUserReportingDetail(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateChildOrgStructure(TblOrgStructureTO orgStructureTO, Int32 reportingTypeId);
        int DeleteTblOrgStructure(Int32 idOrgStructure);
        int DeleteTblOrgStructure(Int32 idOrgStructure, SqlConnection conn, SqlTransaction tran);
        List<TblOrgStructureTO> GetOrgStructureListForDisplay(int reportingTypeId);
        void MakeMissedUserList(List<TblUserReportingDetailsTO> allUserReportingList, List<TblOrgStructureTO> displayOrgStructureList, Int32 uniquiNo);
        void UserwiseOrgChart(List<TblOrgStructureTO> OrgStructureTOList, List<TblUserReportingDetailsTO> alluserReportingDetailsTOList, String ParentId, List<TblOrgStructureTO> orgStructureTOListUserDisplay, Int32 userId, String newParent, Int32 uniquiNo);
        void GetOrg(String ParentId, Int32 DeptId, List<TblOrgStructureTO> OrgStructureTOList, List<DimMstDeptTO> MstDeptTOList, Int32 deptId, int reportingTypeId);
        void MakeOrgStructureList(TblOrgStructureTO orgstrctureTO, List<TblOrgStructureTO> allOrgStructutreList, List<TblOrgStructureHierarchyTO> hierarchyList, int reportingTypeId, List<TblOrgStructureTO> listForDisplay);
        ResultMessage DeactivateUserReporting(TblUserReportingDetailsTO tblUserReportingDetailsTO);
        ResultMessage UpdateUserReportingDetails(List<TblUserReportingDetailsTO> tblUserReportingDetailsTO, Int32 userReportingId);
        int UpdateUserReportingDetails(List<TblUserReportingDetailsTO> tblUserReportingDetailsTO, Int32 userReportingId, SqlConnection conn, SqlTransaction tran);
        List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(string ids);
        List<TblUserReportingDetailsTO> SelectUserReportingOnuserIds(string ids, int reportingTo);
        List<TblUserReportingDetailsTO> SelectDeactivateUserReportingList(TblUserReportingDetailsTO tblUserReportingDetailsTO);
        List<TblUserReportingDetailsTO> SelectDeactivateChildEmployeeList(TblUserReportingDetailsTO tblUserReportingDetailsTO);
        TblUserReportingDetailsTO SelectUserReportingDetailsTOBom(int IdUserReportingDetails, SqlConnection conn, SqlTransaction tran);
        int UpdateTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO);
        int UpdateTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlConnection conn, SqlTransaction tran);
        TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization);
    }
}