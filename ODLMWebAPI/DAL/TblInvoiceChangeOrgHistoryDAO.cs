using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
namespace ODLMWebAPI.DAL

{
    public class TblInvoiceChangeOrgHistoryDAO : ITblInvoiceChangeOrgHistoryDAO
    {
        
                private readonly IConnectionString _iConnectionString;
        public TblInvoiceChangeOrgHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString =iConnectionString;

        }
        
        public  int InsertTblInvoiceChangeOrgHistory(TblInvoiceChangeOrgHistoryTO tblInvoiceChangeOrgHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceChangeOrgHistoryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblInvoiceChangeOrgHistory(TblInvoiceChangeOrgHistoryTO tblInvoiceChangeOrgHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceChangeOrgHistoryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
     
        public  int ExecuteInsertionCommand(TblInvoiceChangeOrgHistoryTO tblInvoiceChangeOrgHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblInvoiceChangeOrgHistory]( " + 
          //  "  [idInvoiceChangeOrgHistory]" +
            " [invoiceId]" +
            " ,[dupInvoiceId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[actionDesc]" +
            " )" +
" VALUES (" +
           // "  @IdInvoiceChangeOrgHistory " +
            " @InvoiceId " +
            " ,@DupInvoiceId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@ActionDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

         
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceChangeOrgHistoryTO.InvoiceId;
            cmdInsert.Parameters.Add("@DupInvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceChangeOrgHistoryTO.DupInvoiceId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceChangeOrgHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceChangeOrgHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@ActionDesc", System.Data.SqlDbType.NVarChar).Value = tblInvoiceChangeOrgHistoryTO.ActionDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        
      
        }

     }
