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
    public class TblPaymentTermOptionsDAO : ITblPaymentTermOptionsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPaymentTermOptionsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPaymentTermOptions]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPaymentTermOptionsTO>SelectAllTblPaymentTermOptions()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPaymentTermOption", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.IdPaymentTermOption;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionsTO> tblPaymentTermOptionsTO = ConvertDTToList(sqlReader);
                return tblPaymentTermOptionsTO;
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

        public TblPaymentTermOptionsTO SelectTblPaymentTermOptions(Int32 idPaymentTermOption)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPaymentTermOption = " + idPaymentTermOption +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPaymentTermOption", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.IdPaymentTermOption;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionsTO> tblPaymentTermOptionsTOList = ConvertDTToList(sqlReader);
                if (tblPaymentTermOptionsTOList != null && tblPaymentTermOptionsTOList.Count > 0)
                {
                    return tblPaymentTermOptionsTOList[0];
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

        public List<TblPaymentTermOptionsTO> SelectAllTblPaymentTermOptions(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPaymentTermOption", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.IdPaymentTermOption;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionsTO> tblPaymentTermOptionsTOList = ConvertDTToList(sqlReader);
                return tblPaymentTermOptionsTOList;
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


        /// <summary>
        /// Priyanka [18-01-2019]
        /// </summary>
        /// <param name="paymentTermId"></param>
        /// <returns></returns>
        public List<TblPaymentTermOptionsTO> SelectTblPaymentTermOptionRelationBypaymentTermId(Int32 paymentTermId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE paymentTermId = " + paymentTermId + " AND isActive =1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPaymentTermOptionsTO> tblPaymentTermOptionsTOList = ConvertDTToList(sqlReader);
                if (tblPaymentTermOptionsTOList != null && tblPaymentTermOptionsTOList.Count > 0)
                {
                    return tblPaymentTermOptionsTOList;
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
        public List<TblPaymentTermOptionsTO> ConvertDTToList(SqlDataReader tblPaymentTermOptionsTODT)
        {
            List<TblPaymentTermOptionsTO> tblPaymentTermOptionsTOList = new List<TblPaymentTermOptionsTO>();
            if (tblPaymentTermOptionsTODT != null)
            {
                while (tblPaymentTermOptionsTODT.Read())
                {
                    TblPaymentTermOptionsTO tblPaymentTermOptionsTONew = new TblPaymentTermOptionsTO();
                    if (tblPaymentTermOptionsTODT["idPaymentTermOption"] != DBNull.Value)
                        tblPaymentTermOptionsTONew.IdPaymentTermOption = Convert.ToInt32(tblPaymentTermOptionsTODT["idPaymentTermOption"].ToString());
                    if (tblPaymentTermOptionsTODT["paymentTermId"] != DBNull.Value)
                        tblPaymentTermOptionsTONew.PaymentTermId = Convert.ToInt32(tblPaymentTermOptionsTODT["paymentTermId"].ToString());
                    if (tblPaymentTermOptionsTODT["isActive"] != DBNull.Value)
                        tblPaymentTermOptionsTONew.IsActive = Convert.ToInt32(tblPaymentTermOptionsTODT["isActive"].ToString());
                    if (tblPaymentTermOptionsTODT["createdBy"] != DBNull.Value)
                        tblPaymentTermOptionsTONew.CreatedBy = Convert.ToInt32(tblPaymentTermOptionsTODT["createdBy"].ToString());
                    if (tblPaymentTermOptionsTODT["updatedBy"] != DBNull.Value)
                        tblPaymentTermOptionsTONew.UpdatedBy = Convert.ToInt32(tblPaymentTermOptionsTODT["updatedBy"].ToString());
                    if (tblPaymentTermOptionsTODT["createdOn"] != DBNull.Value)
                        tblPaymentTermOptionsTONew.CreatedOn = Convert.ToDateTime(tblPaymentTermOptionsTODT["createdOn"].ToString());
                    if (tblPaymentTermOptionsTODT["updatedOn"] != DBNull.Value)
                        tblPaymentTermOptionsTONew.UpdatedOn = Convert.ToDateTime(tblPaymentTermOptionsTODT["updatedOn"].ToString());
                    if (tblPaymentTermOptionsTODT["paymentTermOption"] != DBNull.Value)
                        tblPaymentTermOptionsTONew.PaymentTermOption = Convert.ToString(tblPaymentTermOptionsTODT["paymentTermOption"].ToString());
                    if (tblPaymentTermOptionsTODT["isDescriptive"] != DBNull.Value)
                        tblPaymentTermOptionsTONew.IsDescriptive = Convert.ToInt32(tblPaymentTermOptionsTODT["isDescriptive"].ToString());
                    tblPaymentTermOptionsTOList.Add(tblPaymentTermOptionsTONew);
                }
            }
            return tblPaymentTermOptionsTOList;
        }
        #endregion

        #region Insertion
        public int InsertTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPaymentTermOptionsTO, cmdInsert);
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

        public int InsertTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPaymentTermOptionsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPaymentTermOptions]( " + 
            //"  [idPaymentTermOption]" +
            " [paymentTermId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[paymentTermOption]" +
            " )" +
" VALUES (" +
            //"  @IdPaymentTermOption " +
            " @PaymentTermId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@PaymentTermOption " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPaymentTermOption", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.IdPaymentTermOption;
            cmdInsert.Parameters.Add("@PaymentTermId", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.PaymentTermId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermOptionsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermOptionsTO.UpdatedOn;
            cmdInsert.Parameters.Add("@PaymentTermOption", System.Data.SqlDbType.VarChar).Value = tblPaymentTermOptionsTO.PaymentTermOption;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPaymentTermOptionsTO, cmdUpdate);
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

        public int UpdateTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPaymentTermOptionsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPaymentTermOptions] SET " + 
            //"  [idPaymentTermOption] = @IdPaymentTermOption" +
            " [paymentTermId]= @PaymentTermId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[paymentTermOption] = @PaymentTermOption" +
            " WHERE [idPaymentTermOption] = @IdPaymentTermOption "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPaymentTermOption", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.IdPaymentTermOption;
            cmdUpdate.Parameters.Add("@PaymentTermId", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.PaymentTermId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermOptionsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermOptionsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@PaymentTermOption", System.Data.SqlDbType.VarChar).Value = tblPaymentTermOptionsTO.PaymentTermOption;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblPaymentTermOptions(Int32 idPaymentTermOption)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPaymentTermOption, cmdDelete);
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

        public int DeleteTblPaymentTermOptions(Int32 idPaymentTermOption, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPaymentTermOption, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPaymentTermOption, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPaymentTermOptions] " +
            " WHERE idPaymentTermOption = " + idPaymentTermOption +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPaymentTermOption", System.Data.SqlDbType.Int).Value = tblPaymentTermOptionsTO.IdPaymentTermOption;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
