using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblStockAsPerBooksDAO
    {
        String SqlSelectQuery();
        List<TblStockAsPerBooksTO> SelectAllTblStockAsPerBooks();
        TblStockAsPerBooksTO SelectTblStockAsPerBooks(Int32 idStockAsPerBooks);
        TblStockAsPerBooksTO SelectTblStockAsPerBooks(DateTime stockDate);
        List<TblStockAsPerBooksTO> ConvertDTToList(SqlDataReader tblStockAsPerBooksTODT);
        int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO);
        int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlCommand cmdInsert);
        int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO);
        int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlCommand cmdUpdate);
        int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks);
        int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStockAsPerBooks, SqlCommand cmdDelete);

    }
}