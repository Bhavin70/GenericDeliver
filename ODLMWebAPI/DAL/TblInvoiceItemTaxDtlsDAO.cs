using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblInvoiceItemTaxDtlsDAO : ITblInvoiceItemTaxDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblInvoiceItemTaxDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblOtherTaxes.isBefore,tblOtherTaxes.isAfter,tblOtherTaxes.both,itemTax.*,invItem.gstinCodeNo,taxRate.taxTypeId,invItem.invoiceId  FROM [tempInvoiceItemTaxDtls] itemTax" +
                                  " LEFT JOIN tempInvoiceItemDetails invItem ON itemTax.invoiceItemId=invItem.idInvoiceItem" +
                                  " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId=taxRate.idTaxRate" +
                                  " LEFT JOIN tblOtherTaxes tblOtherTaxes on tblOtherTaxes.idOtherTax=invItem.otherTaxId "+
                                  // Vaibhav [10-Jan-2018] Added to select from 

                                  " UNION ALL " +
                                  " SELECT tblOtherTaxes.isBefore,tblOtherTaxes.isAfter,tblOtherTaxes.both,itemTax.*,invItem.gstinCodeNo ,taxRate.taxTypeId,invItem.invoiceId  FROM [finalInvoiceItemTaxDtls] itemTax" +
                                  " LEFT JOIN finalInvoiceItemDetails invItem ON itemTax.invoiceItemId=invItem.idInvoiceItem"+
                                  " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId=taxRate.idTaxRate"+
                                  " LEFT JOIN tblOtherTaxes tblOtherTaxes on tblOtherTaxes.idOtherTax=invItem.otherTaxId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtls()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceItemTaxDtlsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtls(Int32 invItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE invoiceItemId=" + invItemId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceItemTaxDtlsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.invoiceId=" + invoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceItemTaxDtlsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TblInvoiceItemTaxDtlsTO SelectTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idInvItemTaxDtl = " + idInvItemTaxDtl + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceItemTaxDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<TblInvoiceItemTaxDtlsTO> ConvertDTToList(SqlDataReader tblInvoiceItemTaxDtlsTODT)
        {
            List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
            if (tblInvoiceItemTaxDtlsTODT != null)
            {
                while (tblInvoiceItemTaxDtlsTODT.Read())
                {
                    TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTONew = new TblInvoiceItemTaxDtlsTO();
                    if (tblInvoiceItemTaxDtlsTODT["idInvItemTaxDtl"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.IdInvItemTaxDtl = Convert.ToInt32(tblInvoiceItemTaxDtlsTODT["idInvItemTaxDtl"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["invoiceItemId"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.InvoiceItemId = Convert.ToInt32(tblInvoiceItemTaxDtlsTODT["invoiceItemId"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["taxRateId"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.TaxRateId = Convert.ToInt32(tblInvoiceItemTaxDtlsTODT["taxRateId"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["taxPct"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.TaxPct = Convert.ToDouble(tblInvoiceItemTaxDtlsTODT["taxPct"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["taxRatePct"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.TaxRatePct = Convert.ToDouble(tblInvoiceItemTaxDtlsTODT["taxRatePct"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["taxableAmt"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.TaxableAmt = Convert.ToDouble(tblInvoiceItemTaxDtlsTODT["taxableAmt"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["taxAmt"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.TaxAmt = Convert.ToDouble(tblInvoiceItemTaxDtlsTODT["taxAmt"].ToString());

                    if (tblInvoiceItemTaxDtlsTODT["taxTypeId"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.TaxTypeId = Convert.ToInt32(tblInvoiceItemTaxDtlsTODT["taxTypeId"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["gstinCodeNo"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.GstinCodeNo = Convert.ToString(tblInvoiceItemTaxDtlsTODT["gstinCodeNo"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["isBefore"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.IsBefore = Convert.ToInt32(tblInvoiceItemTaxDtlsTODT["isBefore"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["isAfter"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.IsAfter = Convert.ToInt32(tblInvoiceItemTaxDtlsTODT["isAfter"].ToString());
                    if (tblInvoiceItemTaxDtlsTODT["both"] != DBNull.Value)
                        tblInvoiceItemTaxDtlsTONew.Both = Convert.ToInt32(tblInvoiceItemTaxDtlsTODT["both"].ToString());
                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTONew);
                }
            }
            return tblInvoiceItemTaxDtlsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceItemTaxDtlsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceItemTaxDtlsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempInvoiceItemTaxDtls]( " +
                                "  [invoiceItemId]" +
                                " ,[taxRateId]" +
                                " ,[taxPct]" +
                                " ,[taxRatePct]" +
                                " ,[taxableAmt]" +
                                " ,[taxAmt]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceItemId " +
                                " ,@TaxRateId " +
                                " ,@TaxPct " +
                                " ,@TaxRatePct " +
                                " ,@TaxableAmt " +
                                " ,@TaxAmt " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvItemTaxDtl", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.IdInvItemTaxDtl;
            cmdInsert.Parameters.Add("@InvoiceItemId", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.InvoiceItemId;
            cmdInsert.Parameters.Add("@TaxRateId", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.TaxRateId;
            cmdInsert.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxPct;
            cmdInsert.Parameters.Add("@TaxRatePct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxRatePct;
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxableAmt;
            cmdInsert.Parameters.Add("@TaxAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxAmt;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceItemTaxDtlsTO.IdInvItemTaxDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblInvoiceItemTaxDtlsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblInvoiceItemTaxDtlsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempInvoiceItemTaxDtls] SET " +
                            "  [invoiceItemId]= @InvoiceItemId" +
                            " ,[taxRateId]= @TaxRateId" +
                            " ,[taxPct]= @TaxPct" +
                            " ,[taxRatePct]= @TaxRatePct" +
                            " ,[taxableAmt]= @TaxableAmt" +
                            " ,[taxAmt] = @TaxAmt" +
                            " WHERE [idInvItemTaxDtl] = @IdInvItemTaxDtl";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdInvItemTaxDtl", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.IdInvItemTaxDtl;
            cmdUpdate.Parameters.Add("@InvoiceItemId", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.InvoiceItemId;
            cmdUpdate.Parameters.Add("@TaxRateId", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.TaxRateId;
            cmdUpdate.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxPct;
            cmdUpdate.Parameters.Add("@TaxRatePct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxRatePct;
            cmdUpdate.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxableAmt;
            cmdUpdate.Parameters.Add("@TaxAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxAmt;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idInvItemTaxDtl, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idInvItemTaxDtl, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int DeleteInvoiceItemTaxDtlsByInvId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                cmdDelete.CommandText = " DELETE FROM [tempInvoiceItemTaxDtls] " +
                                        " WHERE invoiceItemId In(SELECT idInvoiceItem from " +
                                        " tempInvoiceItemDetails WHERE invoiceId=" + invoiceId + ") ";

                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idInvItemTaxDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoiceItemTaxDtls] " +
            " WHERE idInvItemTaxDtl = " + idInvItemTaxDtl + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvItemTaxDtl", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.IdInvItemTaxDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
