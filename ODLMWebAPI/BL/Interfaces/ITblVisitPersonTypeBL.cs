using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVisitPersonTypeBL
    {
        DataTable SelectAllTblVisitPersonType();
        List<TblVisitPersonTypeTO> SelectAllTblVisitPersonTypeList();
        TblVisitPersonTypeTO SelectTblVisitPersonTypeTO(Int32 idPersonType);
        List<TblVisitPersonTypeTO> ConvertDTToList(DataTable tblVisitPersonTypeTODT);
        int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO);
        int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO);
        int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVisitPersonType(Int32 idPersonType);
        int DeleteTblVisitPersonType(Int32 idPersonType, SqlConnection conn, SqlTransaction tran);
    }
}