using System.Data.SqlClient;
using ODLMWebAPI.Models;

namespace ODLMWebAPI.DAL.Interfaces
{
 
public  interface ITblInvoiceChangeOrgHistoryDAO
{
    int InsertTblInvoiceChangeOrgHistory(TblInvoiceChangeOrgHistoryTO tblInvoiceChangeOrgHistoryTO);
    int InsertTblInvoiceChangeOrgHistory(TblInvoiceChangeOrgHistoryTO tblInvoiceChangeOrgHistoryTO, SqlConnection conn, SqlTransaction tran);
    int ExecuteInsertionCommand(TblInvoiceChangeOrgHistoryTO tblInvoiceChangeOrgHistoryTO, SqlCommand cmdInsert);
    
}
}