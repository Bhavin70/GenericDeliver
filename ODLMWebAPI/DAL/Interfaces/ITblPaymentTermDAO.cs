using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblPaymentTermDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblPaymentTerm();
        List<DropDownTO> SelecPaymentTermForDropDown();
        DataTable SelectTblPaymentTerm(Int32 idPaymentTerm);
        DataTable SelectAllTblPaymentTerm(SqlConnection conn, SqlTransaction tran);
        int InsertTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO);
        int InsertTblPaymentTerm(ref TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPaymentTermTO tblPaymentTermTO, SqlCommand cmdInsert);
        int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO);
        int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPaymentTermTO tblPaymentTermTO, SqlCommand cmdUpdate);
        int DeleteTblPaymentTerm(Int32 idPaymentTerm);
        int DeleteTblPaymentTerm(Int32 idPaymentTerm, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPaymentTerm, SqlCommand cmdDelete);

    }
}