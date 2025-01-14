using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblOrgStructureDAO
    {
        String SqlSelectQuery();
        String SqlSelectQueryForChild();
        String SqlSelectQueryUserReportingBom();
        String SqlSelectQueryUserReporting();
        List<TblOrgStructureTO> SelectAllOrgStructureHierarchy(SqlConnection conn, SqlTransaction tran);
        List<TblUserReportingDetailsTO> SelectOrgStructureUserDetailsForBom(int orgStructureId);
        TblUserReportingDetailsTO SelectUserReportingDetailsTOBom(int idUserReportingDetails, SqlConnection conn, SqlTransaction tran);
        List<TblOrgStructureTO> SelectAllOrgStructureHierarchy();
        List<TblOrgStructureTO> SelectAllOrgStructureHierarchy(int reportingTypeId);
        TblOrgStructureTO SelectTblOrgStructure(Int32 idOrgStructure);
        TblOrgStructureTO SelectAllTblOrgStructure(SqlConnection conn, SqlTransaction tran);
        TblOrgStructureTO SelectBOMOrgStructure();
        List<TblUserReportingDetailsTO> SelectUserReportingListOnUserId(int userId);
        TblUserReportingDetailsTO SelectUserReportingDetailsTO(int idUserReportingDetails);
        TblUserReportingDetailsTO SelectUserReportingDetailsTO(int idUserReportingDetails, SqlConnection conn, SqlTransaction tran);
        TblOrgStructureHierarchyTO SelectTblOrgStructureHierarchyForDelink(int selfOrgStructureId, int parentOrgStrucureId);
        List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(string orgStructureId);
        List<TblUserReportingDetailsTO> SelectUserReportingOnuserIds(string userIds, int reportingTo);
        List<TblOrgStructureHierarchyTO> SelectAllTblOrgStructureHierarchy();
        TblOrgStructureHierarchyTO SelectAllTblOrgStructureHierarchy(int orgStrctureId, int parentOrgStrctureId, int reportingTypeId, SqlConnection conn, SqlTransaction tran);
        List<TblOrgStructureHierarchyTO> SelectTblOrgStructureHierarchyOnReportingType(int reportingTypeId);
        List<TblOrgStructureHierarchyTO> SelectTblOrgStructureHierarchyOnOrgStructutreId(int orgStructutreId);
        List<TblUserReportingDetailsTO> SelectAllUserReportingDetails();
        List<TblUserReportingDetailsTO> SelectAllUserReportingDetails(SqlConnection conn, SqlTransaction tran);
        List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(int orgStructureId);
        List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(int orgStructureId, SqlConnection conn, SqlTransaction tran);
        List<TblUserReportingDetailsTO> SelectOrgStructureUserDetailsByReportingType(int orgStructureId, int reportingTypeId);
        String SelectAllOrgStructureIdList(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectReportingToUserList(string orgStructureId, int repTypeId);
        List<DimLevelsTO> SelectAllDimLevels();
        List<TblOrgStructureTO> SelectChildPositionList(string deptIds, int idOrgStructure);
        List<TblOrgStructureTO> SelectAllNotLinkedPositionsList();
        List<TblOrgStructureTO> SelectPositionLinkDetails(int idOrgStructure);
        List<TblOrgStructureTO> SelectChildPositionsDetails(int idOrgStructure);
        int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO);
        int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO);
        int InsertTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdInsert);
        int ExecuteInsertionCommandForOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlCommand cmdInsert);
        int InsertTblUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlCommand cmdInsert);
        int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO);
        int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        int UpdateChildTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate);
        int UpdateTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO);
        int UpdateTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommandForOrgHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlCommand cmdUpdate);
        int ExecuteChildUpdationCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate);
        int DeactivateOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeactivateOrgSTructureCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate);
        int DeactivateOrgStructureEmployees(String orgStructureIdList, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeactivateOrgSTructureEmployeesCommand(String orgStructureIdList, SqlCommand cmdUpdate);
        int UpdateUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO);
        int UpdateUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdateUserReportingDetailsCommand(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblOrgStructure(Int32 idOrgStructure);
        int DeleteTblOrgStructure(Int32 idOrgStructure, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOrgStructure, SqlCommand cmdDelete);
        List<TblUserReportingDetailsTO> ConvertDTToListUserReportingDetails(SqlDataReader userDT);
        List<TblUserReportingDetailsTO> ConvertDTToListUserReportingDetailsForBom(SqlDataReader userDT);
        List<DimLevelsTO> ConvertDTToListLevels(SqlDataReader levelsDT);
        List<TblOrgStructureHierarchyTO> ConvertDTToListOrgStructHierarchy(SqlDataReader tblOrgStructureHierarchyTODT);
        List<TblOrgStructureTO> ConvertDTToListForChild(SqlDataReader orgStructureDT);
        List<TblOrgStructureTO> ConvertDTToList(SqlDataReader orgStructureDT);

    }
}