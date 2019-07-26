using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblEntityRangeBL
    {
        List<TblEntityRangeTO> SelectAllTblEntityRangeList();
        TblEntityRangeTO SelectTblEntityRangeTO(Int32 idEntityRange);
        TblEntityRangeTO SelectTblEntityRangeTOByEntityName(string entityName);
        TblEntityRangeTO SelectEntityRangeTOFromInvoiceType(String entityName, int finYearId, SqlConnection conn, SqlTransaction tran);
        TblEntityRangeTO SelectEntityRangeTOFromInvoiceType(Int32 invoiceTypeId, int finYearId, SqlConnection conn, SqlTransaction tran);
        TblEntityRangeTO SelectTblEntityRangeTOByEntityName(string entityName, int finYearId, SqlConnection conn, SqlTransaction tran);
        int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO);
        int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO);
        int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblEntityRange(Int32 idEntityRange);
        int DeleteTblEntityRange(Int32 idEntityRange, SqlConnection conn, SqlTransaction tran);
    }
}
