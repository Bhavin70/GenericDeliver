using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{   
    public interface ITblPersonBL
    { 
        List<TblPersonTO> SelectAllTblPersonList();
        TblPersonTO SelectTblPersonTO(Int32 idPerson);
        List<TblPersonTO> SelectAllPersonListByOrganization(int organizationId);
        List<TblPersonTO> SelectAllPersonListByOrganizationV2(int organizationId, Int32 personTypeId);
        List<DropDownTO> SelectDropDownListOnPersonId(Int32 idPerson);
        List<DropDownTO> SelectPersonBasedOnOrgType(Int32 OrgType);
        List<TblPersonTO> SelectAllTblPersonByRoleType(Int32 roleTypeId);
        ResultMessage AddNewPerson(TblPersonTO tblPersonTO);
        ResultMessage AddNewPersonWithAddressDetails(TblPersonTO tblPersonTO, TblAddressTO tblAddressTO);
        int InsertTblPerson(TblPersonTO tblPersonTO);
        int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewPersonAgainstOrganization(TblPersonTO tblPersonTO, Int32 organizationId, Int32 personTypeId);
        int UpdateTblPerson(TblPersonTO tblPersonTO);
        int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPerson(Int32 idPerson);
        int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran);
        List<BirthdayAlertTO> SelectAllPersonBirthday(DateTime Today, Int32 UpcomingDays, Int32 IsBirthday);
        TblPersonTO GetPersonOnUserId(int userId);
        List<TblPersonTO> selectPersonsForOffline();
        List<DropDownTO> selectPersonsDropdownForOffline();
        List<TblPersonTOEmail> SelectAllPersonListByOrganizationForEmail(int organizationId);
    }
}