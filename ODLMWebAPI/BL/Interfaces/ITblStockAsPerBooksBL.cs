using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblStockAsPerBooksBL
    {
        List<TblStockAsPerBooksTO> SelectAllTblStockAsPerBooksList();
        TblStockAsPerBooksTO SelectTblStockAsPerBooksTO(Int32 idStockAsPerBooks);
        TblStockAsPerBooksTO SelectTblStockAsPerBooksTO(DateTime stockDate);
        int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO);
        ResultMessage SaveStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO);
        int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO);
        int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks);
        int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks, SqlConnection conn, SqlTransaction tran);
    }
}