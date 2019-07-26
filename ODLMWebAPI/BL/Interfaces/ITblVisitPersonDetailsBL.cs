using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface ITblVisitPersonDetailsBL
    {
        List<TblVisitPersonDetailsTO> SelectAllTblVisitPersonDetailsList(int visitTypeId);
        TblVisitPersonDetailsTO SelectTblPersonVisitDetailsTO(Int32 personId);
        List<TblVisitPersonDetailsTO> ConvertDTToList(DataTable tblVisitPersonDetailsTODT);
        int SelectVisitPersonCount(int visitId, int personId, int personTypeId);
        List<DropDownTO> SelectAllVisitPersonTypeList();
        List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType);
        List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType, int? organizationId);
        int InsertTblPersonVisitDetails(TblVisitPersonDetailsTO tblPersonVisitDetailsTO);
        int InsertTblVisitPersonDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewVisitPersonDetail(TblVisitPersonDetailsTO tblVisitPersonDetailsTO);
        int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblPersonVisitDetailsTO);
        int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblPersonVisitDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPersonVisitDetails(Int32 personId);
        int DeleteTblPersonVisitDetails(Int32 personId, SqlConnection conn, SqlTransaction tran);
        List<TblVisitPersonDetailsTO> SelectPersonDetailsForOffline(String personTypes);
    }
}