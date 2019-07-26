using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface ITblPersonDAO
    { 
        String SqlSelectQuery();
        List<TblPersonTO> SelectAllTblPerson();
        List<TblPersonTO> SelectAllTblPersonByOrganization(Int32 organizationId);
        List<TblPersonTO> SelectAllTblPersonByRoleType(Int32 roleTypeId);
        List<TblPersonTO> SelectAllPersonByOrganizations(Int32 organizationId, Int32 personTypeId);
        TblPersonTO SelectTblPerson(Int32 idPerson);
        List<DropDownTO> SelectDropDownListOnPersonId(int personId);
        List<DropDownTO> SelectPersonsBasedOnOrgType(Int32 OrgType);
        List<TblPersonTO> ConvertDTToList(SqlDataReader tblPersonTODT);
        int InsertTblPerson(TblPersonTO tblPersonTO);
        int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPersonTO tblPersonTO, SqlCommand cmdInsert);
        int UpdateTblPerson(TblPersonTO tblPersonTO);
        int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPersonTO tblPersonTO, SqlCommand cmdUpdate);
        int DeleteTblPerson(Int32 idPerson);
        int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPerson, SqlCommand cmdDelete);
        List<BirthdayAlertTO> SelectAllTblPersonByBirthdayAnniversory(DateTime Today, Int32 upcomingDays, Int32 IsBirthday);
        List<BirthdayAlertTO> ConvertBirthdayDTToList(SqlDataReader tblBirthdayPersonTODT);
        int GetPersonIdOnUserId(int userId);
        List<DropDownTO> ConvertDTToListDropdownForOffline(SqlDataReader tblPersonTODT);
        List<DropDownTO> selectPersonDropdownListOffline();
        List<TblPersonTO> selectPersonsForOffline();
        List<TblPersonTOEmail> SelectblOrganizationForEmail(Int32 organizationId);
        List<TblPersonTOEmail> SelectAllTblPersonOrganizationForEmail(Int32 organizationId);
    }
}