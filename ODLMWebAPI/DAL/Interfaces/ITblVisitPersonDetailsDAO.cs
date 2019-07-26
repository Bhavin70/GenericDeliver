using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface ITblVisitPersonDetailsDAO
    {
        String SqlSelectQuery();
        List<TblVisitPersonDetailsTO> SelectAllTblVisitPersonDetails(int visitTypeId);
        List<DropDownTO> SelectAllVisitPersonTypeList();
        List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType);
        DataTable SelectTblPersonVisitDetails(Int32 personId);
        DataTable SelectAllTblPersonVisitDetails(SqlConnection conn, SqlTransaction tran);
        List<TblVisitPersonDetailsTO> ConvertDTToList(SqlDataReader visitPersonDetailsDT);
        int SelectVisitPersonCount(int visitId, int personId, int persontypeId);
        List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType, int? organizationId);
        int InsertTblPersonVisitDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO);
        int InsertTblVisitPersonDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlCommand cmdInsert);
        int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO);
        int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblPersonVisitDetails(Int32 personId);
        int DeleteTblPersonVisitDetails(Int32 personId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 personId, SqlCommand cmdDelete);
        List<TblVisitPersonDetailsTO> SelectPersonDetailsForOffline(String ids);

    }
}