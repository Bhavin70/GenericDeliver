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
    public class TblPaymentTermsForBookingDAO : ITblPaymentTermsForBookingDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPaymentTermsForBookingDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPaymentTermsForBooking] WHERE isActive =1"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPaymentTermsForBookingTO> SelectAllTblPaymentTermsForBooking()
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
                List<TblPaymentTermsForBookingTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
                return tblPaymentTermsForBookingTOList;
               
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

        public TblPaymentTermsForBookingTO SelectTblPaymentTermsForBooking(Int32 idPaymentTerm)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPaymentTerm = " + idPaymentTerm +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermsForBookingTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
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

        public List<TblPaymentTermsForBookingTO> SelectAllTblPaymentTermsForBooking(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPaymentTerm", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.IdPaymentTerm;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermsForBookingTO> tblPaymentTermsForBookingTOList = ConvertDTToList(sqlReader);
                return tblPaymentTermsForBookingTOList;
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
        public List<TblPaymentTermsForBookingTO> ConvertDTToList(SqlDataReader tblPaymentTermsForBookingTODT)
        {
            List<TblPaymentTermsForBookingTO> tblPaymentTermsForBookingTOList = new List<TblPaymentTermsForBookingTO>();
            if (tblPaymentTermsForBookingTODT != null)
            {
                while (tblPaymentTermsForBookingTODT.Read())
                {
                    TblPaymentTermsForBookingTO tblPaymentTermsForBookingTONew = new TblPaymentTermsForBookingTO();
                    if (tblPaymentTermsForBookingTODT["idPaymentTerm"] != DBNull.Value)
                        tblPaymentTermsForBookingTONew.IdPaymentTerm = Convert.ToInt32(tblPaymentTermsForBookingTODT["idPaymentTerm"].ToString());
                    if (tblPaymentTermsForBookingTODT["isActive"] != DBNull.Value)
                        tblPaymentTermsForBookingTONew.IsActive = Convert.ToInt32(tblPaymentTermsForBookingTODT["isActive"].ToString());
                    if (tblPaymentTermsForBookingTODT["createdBy"] != DBNull.Value)
                        tblPaymentTermsForBookingTONew.CreatedBy = Convert.ToInt32(tblPaymentTermsForBookingTODT["createdBy"].ToString());
                    if (tblPaymentTermsForBookingTODT["updatedBy"] != DBNull.Value)
                        tblPaymentTermsForBookingTONew.UpdatedBy = Convert.ToInt32(tblPaymentTermsForBookingTODT["updatedBy"].ToString());
                    if (tblPaymentTermsForBookingTODT["createdOn"] != DBNull.Value)
                        tblPaymentTermsForBookingTONew.CreatedOn = Convert.ToDateTime(tblPaymentTermsForBookingTODT["createdOn"].ToString());
                    if (tblPaymentTermsForBookingTODT["updatedOn"] != DBNull.Value)
                        tblPaymentTermsForBookingTONew.UpdatedOn = Convert.ToDateTime(tblPaymentTermsForBookingTODT["updatedOn"].ToString());
                    if (tblPaymentTermsForBookingTODT["paymentTerm"] != DBNull.Value)
                        tblPaymentTermsForBookingTONew.PaymentTerm = Convert.ToString(tblPaymentTermsForBookingTODT["paymentTerm"].ToString());
                    if (tblPaymentTermsForBookingTODT["paymentTermDesc"] != DBNull.Value)
                        tblPaymentTermsForBookingTONew.PaymentTermDesc = Convert.ToString(tblPaymentTermsForBookingTODT["paymentTermDesc"].ToString());
                    tblPaymentTermsForBookingTOList.Add(tblPaymentTermsForBookingTONew);
                }
            }
            return tblPaymentTermsForBookingTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPaymentTermsForBookingTO, cmdInsert);
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

        public int InsertTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPaymentTermsForBookingTO, cmdInsert);
            }
            catch(Exception ex)
            {
               
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPaymentTermsForBooking]( " + 
            //"  [idPaymentTerm]" +
            " [isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[paymentTerm]" +
            " ,[paymentTermDesc]" +
            " )" +
" VALUES (" +
            //"  @IdPaymentTerm " +
            " @IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@PaymentTerm " +
            " ,@PaymentTermDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPaymentTerm", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.IdPaymentTerm;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermsForBookingTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermsForBookingTO.UpdatedOn;
            cmdInsert.Parameters.Add("@PaymentTerm", System.Data.SqlDbType.VarChar).Value = tblPaymentTermsForBookingTO.PaymentTerm;
            cmdInsert.Parameters.Add("@PaymentTermDesc", System.Data.SqlDbType.VarChar).Value = tblPaymentTermsForBookingTO.PaymentTermDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPaymentTermsForBookingTO, cmdUpdate);
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

        public int UpdateTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPaymentTermsForBookingTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPaymentTermsForBooking] SET " + 
            //"  [idPaymentTerm] = @IdPaymentTerm" +
            " [isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[paymentTerm]= @PaymentTerm" +
            " ,[paymentTermDesc] = @PaymentTermDesc" +
            " WHERE [idPaymentTerm] = @IdPaymentTerm"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPaymentTerm", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.IdPaymentTerm;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermsForBookingTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermsForBookingTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@PaymentTerm", System.Data.SqlDbType.VarChar).Value = tblPaymentTermsForBookingTO.PaymentTerm;
            cmdUpdate.Parameters.Add("@PaymentTermDesc", System.Data.SqlDbType.VarChar).Value = tblPaymentTermsForBookingTO.PaymentTermDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblPaymentTermsForBooking(Int32 idPaymentTerm)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPaymentTerm, cmdDelete);
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

        public int DeleteTblPaymentTermsForBooking(Int32 idPaymentTerm, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPaymentTerm, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPaymentTerm, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPaymentTermsForBooking] " +
            " WHERE idPaymentTerm = " + idPaymentTerm +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPaymentTerm", System.Data.SqlDbType.Int).Value = tblPaymentTermsForBookingTO.IdPaymentTerm;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
