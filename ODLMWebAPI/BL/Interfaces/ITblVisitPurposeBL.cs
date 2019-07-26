using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVisitPurposeBL
    {
        List<TblVisitPurposeTO> SelectAllTblVisitPurpose();
        List<DropDownTO> SelectVisitPurposeListForDropDown(int visitTypeId);
        TblVisitPurposeTO SelectTblVisitPurposeTO(Int32 idVisitPurpose);
        List<TblVisitPurposeTO> ConvertDTToList(DataTable tblVisitPurposeTODT);
        int InsertTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO);
        int InsertTblVisitPurpose(ref TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO);
        int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVisitPurpose(Int32 idVisitPurpose);
        int DeleteTblVisitPurpose(Int32 idVisitPurpose, SqlConnection conn, SqlTransaction tran);
    }
}