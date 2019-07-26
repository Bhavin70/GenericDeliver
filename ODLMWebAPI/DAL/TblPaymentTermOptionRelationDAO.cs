using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{ 
    public class TblPaymentTermOptionRelationDAO : ITblPaymentTermOptionRelationDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPaymentTermOptionRelationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = "SELECT PaymentTermRelation.*,PaymentTermOptions.paymentTermOption,PaymentTerms.paymentTerm FROM [tblPaymentTermOptionRelation] PaymentTermRelation " +
                "LEFT JOIN tblPaymentTermOptions PaymentTermOptions ON PaymentTermRelation.paymentTermOptionId = PaymentTermOptions.idPaymentTermOption " +
                "LEFT JOIN tblPaymentTermsForBooking PaymentTerms ON PaymentTermOptions.paymentTermId = PaymentTerms.idPaymentTerm"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPaymentTermOptionRelationTO> SelectAllTblPaymentTermOptionRelation()
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionRelationTO> tblPaymentTermOptionRelationTOList = ConvertDTToList(sqlReader);
                return tblPaymentTermOptionRelationTOList;

            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblPaymentTermOptionRelationTO SelectTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPaymentTermOptionRelation = " + idPaymentTermOptionRelation +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionRelationTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
                if (tblPaymentTermsForBookingTOList != null && tblPaymentTermsForBookingTOList.Count > 0)
                {
                    return tblPaymentTermsForBookingTOList[0];
                }
                else
                    return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByBookingIdForUpdate(Int32 bookingId, Int32 paymentTermId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT paymentTermOptRel.*, paymentTermOpt.paymentTermId,paymentTermOpt.paymentTermOption,paymentTerm.paymentTerm from tblPaymentTermOptionRelation paymentTermOptRel " +
                                        " Left join tblPaymentTermOptions paymentTermOpt ON paymentTermOpt.idPaymentTermOption = paymentTermOptRel.paymentTermOptionId "+
                                        " left join tblPaymentTermsForBooking paymentTerm ON paymentTerm.idPaymentTerm=paymentTermOpt.paymentTermId "+
                                        " WHERE paymentTermOptRel.bookingId= " + bookingId + " AND paymentTermOpt.paymentTermId = " + paymentTermId + " AND paymentTermOptRel.isActive=1 ";


                   // SqlSelectQuery() + " WHERE bookingId = " + bookingId + " AND isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionRelationTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
                if (tblPaymentTermsForBookingTOList != null && tblPaymentTermsForBookingTOList.Count > 0)
                {
                    return tblPaymentTermsForBookingTOList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByInvoiceIdForUpdate(Int32 invoiceId, Int32 paymentTermId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT paymentTermOptRel.*, paymentTermOpt.paymentTermId ,paymentTermOpt.paymentTermOption "+
                                        " , paymentTerm.paymentTerm from tblPaymentTermOptionRelation paymentTermOptRel " +
                                        " Left join tblPaymentTermOptions paymentTermOpt ON paymentTermOpt.idPaymentTermOption = paymentTermOptRel.paymentTermOptionId " +
                                        " left join tblPaymentTermsForBooking paymentTerm ON paymentTerm.idPaymentTerm=paymentTermOpt.paymentTermId "+
                                        " WHERE paymentTermOptRel.invoiceId= " + invoiceId + " AND paymentTermOpt.paymentTermId = " + paymentTermId + "AND paymentTermOptRel.isActive=1 ";


                // SqlSelectQuery() + " WHERE bookingId = " + bookingId + " AND isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionRelationTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
                if (tblPaymentTermsForBookingTOList != null && tblPaymentTermsForBookingTOList.Count > 0)
                {
                    return tblPaymentTermsForBookingTOList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByBookingId(Int32 bookingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE PaymentTermRelation.bookingId = " + bookingId + " AND PaymentTermRelation.isActive = 1 ";

                // SqlSelectQuery() + " WHERE bookingId = " + bookingId + " AND isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionRelationTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
                if (tblPaymentTermsForBookingTOList != null && tblPaymentTermsForBookingTOList.Count > 0)
                {
                    return tblPaymentTermsForBookingTOList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByInvoiceId(Int32 invoiceId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE PaymentTermRelation.invoiceId = " + invoiceId + " AND PaymentTermRelation.isActive = 1  ";

                // SqlSelectQuery() + " WHERE bookingId = " + bookingId + " AND isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionRelationTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
                if (tblPaymentTermsForBookingTOList != null && tblPaymentTermsForBookingTOList.Count > 0)
                {
                    return tblPaymentTermsForBookingTOList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByLoadingId(Int32 loadingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE PaymentTermRelation.loadingId = " + loadingId + " AND PaymentTermRelation.isActive = 1 ";

                // SqlSelectQuery() + " WHERE bookingId = " + bookingId + " AND isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionRelationTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
                if (tblPaymentTermsForBookingTOList != null && tblPaymentTermsForBookingTOList.Count > 0)
                {
                    return tblPaymentTermsForBookingTOList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByLoadingId(Int32 loadingId, SqlConnection conn,SqlTransaction tran)
        {
       
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE PaymentTermRelation.loadingId = " + loadingId + " AND PaymentTermRelation.isActive = 1 ";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionRelationTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
                if (tblPaymentTermsForBookingTOList != null && tblPaymentTermsForBookingTOList.Count > 0)
                {

                    return tblPaymentTermsForBookingTOList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //conn.Close();
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblPaymentTermOptionRelationTO> SelectAllTblPaymentTermOptionRelation(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPaymentTermOptionRelation", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.IdPaymentTermOptionRelation;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionRelationTO> tblPaymentTermOptionRelationTOList = ConvertDTToList(sqlReader);
                return tblPaymentTermOptionRelationTOList;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public List<TblPaymentTermOptionRelationTO> ConvertDTToList(SqlDataReader tblPaymentTermOptionRelationTODT)
        {
            List<TblPaymentTermOptionRelationTO> tblPaymentTermOptionRelationTOList = new List<TblPaymentTermOptionRelationTO>();
            if (tblPaymentTermOptionRelationTODT != null)
            {
                while(tblPaymentTermOptionRelationTODT.Read())
                {
                    TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTONew = new TblPaymentTermOptionRelationTO();
                    if (tblPaymentTermOptionRelationTODT["idPaymentTermOptionRelation"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.IdPaymentTermOptionRelation = Convert.ToInt32(tblPaymentTermOptionRelationTODT["idPaymentTermOptionRelation"].ToString());
                    if (tblPaymentTermOptionRelationTODT["paymentTermOptionId"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.PaymentTermOptionId = Convert.ToInt32(tblPaymentTermOptionRelationTODT["paymentTermOptionId"].ToString());
                    if (tblPaymentTermOptionRelationTODT["bookingId"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.BookingId = Convert.ToInt32(tblPaymentTermOptionRelationTODT["bookingId"].ToString());
                    if (tblPaymentTermOptionRelationTODT["isActive"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.IsActive = Convert.ToInt32(tblPaymentTermOptionRelationTODT["isActive"].ToString());
                    if (tblPaymentTermOptionRelationTODT["createdBy"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.CreatedBy = Convert.ToInt32(tblPaymentTermOptionRelationTODT["createdBy"].ToString());
                    if (tblPaymentTermOptionRelationTODT["updatedBy"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.UpdatedBy = Convert.ToInt32(tblPaymentTermOptionRelationTODT["updatedBy"].ToString());
                    if (tblPaymentTermOptionRelationTODT["createdOn"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.CreatedOn = Convert.ToDateTime(tblPaymentTermOptionRelationTODT["createdOn"].ToString());
                    if (tblPaymentTermOptionRelationTODT["updatedOn"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.UpdatedOn = Convert.ToDateTime(tblPaymentTermOptionRelationTODT["updatedOn"].ToString());
                    if (tblPaymentTermOptionRelationTODT["invoiceId"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.InvoiceId = Convert.ToInt32(tblPaymentTermOptionRelationTODT["invoiceId"].ToString());
                    if (tblPaymentTermOptionRelationTODT["loadingId"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.LoadingId = Convert.ToInt32(tblPaymentTermOptionRelationTODT["loadingId"].ToString());
                    if (tblPaymentTermOptionRelationTODT["paymentTermOption"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.PaymentTermOption = Convert.ToString(tblPaymentTermOptionRelationTODT["PaymentTermOption"].ToString());
                    if (tblPaymentTermOptionRelationTODT["paymentTerm"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.PaymentTerm = Convert.ToString(tblPaymentTermOptionRelationTODT["paymentTerm"].ToString());
                    if (tblPaymentTermOptionRelationTODT["paymentTermsDescription"] != DBNull.Value)
                        tblPaymentTermOptionRelationTONew.PaymentTermsDescription = Convert.ToString(tblPaymentTermOptionRelationTODT["paymentTermsDescription"].ToString());
                    tblPaymentTermOptionRelationTOList.Add(tblPaymentTermOptionRelationTONew);
                }
            }
            return tblPaymentTermOptionRelationTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPaymentTermOptionRelationTO, cmdInsert);
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

        public int InsertTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPaymentTermOptionRelationTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPaymentTermOptionRelation]( " + 
            //"  [idPaymentTermOptionRelation]" +
            " [paymentTermOptionId]" +
            " ,[bookingId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[invoiceId]" +
            " ,[loadingId]" +
            " ,[paymentTermsDescription]" +
            " )" +
" VALUES (" +
            //"  @IdPaymentTermOptionRelation " +
            " @PaymentTermOptionId " +
            " ,@BookingId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " + 
            " ,@InvoiceId " +
            " ,@LoadingId " +
            " ,@PaymentTermsDescription " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPaymentTermOptionRelation", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.IdPaymentTermOptionRelation;
            cmdInsert.Parameters.Add("@PaymentTermOptionId", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.PaymentTermOptionId;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.BookingId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermOptionRelationTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.InvoiceId);
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.LoadingId);
            cmdInsert.Parameters.Add("@PaymentTermsDescription", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.PaymentTermsDescription);

            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPaymentTermOptionRelationTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPaymentTermOptionRelationTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPaymentTermOptionRelation] SET " + 
            //"  [idPaymentTermOptionRelation] = @IdPaymentTermOptionRelation" +
            " [paymentTermOptionId]= @PaymentTermOptionId" +
            " ,[bookingId]= @BookingId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " ,[invoiceId] = @InvoiceId" +
            " ,[loadingId] = @LoadingId" +
            " ,[paymentTermsDescription] = @PaymentTermsDescription" +
            " WHERE [idPaymentTermOptionRelation] = @IdPaymentTermOptionRelation"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPaymentTermOptionRelation", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.IdPaymentTermOptionRelation;
            cmdUpdate.Parameters.Add("@PaymentTermOptionId", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.PaymentTermOptionId;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.BookingId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value =tblPaymentTermOptionRelationTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermOptionRelationTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermOptionRelationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.InvoiceId);
            cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.LoadingId);
            cmdUpdate.Parameters.Add("@PaymentTermsDescription", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermOptionRelationTO.PaymentTermsDescription);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPaymentTermOptionRelation, cmdDelete);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPaymentTermOptionRelation, cmdDelete);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idPaymentTermOptionRelation, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPaymentTermOptionRelation] " +
            " WHERE idPaymentTermOptionRelation = " + idPaymentTermOptionRelation +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPaymentTermOptionRelation", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionRelationTO.IdPaymentTermOptionRelation;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
