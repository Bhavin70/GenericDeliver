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
    public class TblBookingParitiesDAO : ITblBookingParitiesDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblBookingParitiesDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            //Sudhir[23-MARCH-2018] Commented on New Parity Logic Aded Brand Id Column to tblBookingParies so no need JOIN. 
            //String sqlSelectQry = " SELECT [tblBookingParities].*, tblParitySummary.brandId FROM [tblBookingParities] [tblBookingParities] " +
            //                      " LEFT JOIN tblParitySummary ON[tblBookingParities].parityId = tblParitySummary.idParity"; 


            String sqlSelectQry = " SELECT [tblBookingParities].* FROM [tblBookingParities] [tblBookingParities] ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblBookingParitiesTO> SelectAllTblBookingParities()
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
                List<TblBookingParitiesTO> list = ConvertDTToList(reader);
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

        public TblBookingParitiesTO SelectTblBookingParities(Int32 idBookingParity)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idBookingParity = " + idBookingParity +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingParitiesTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
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

        public List<TblBookingParitiesTO> SelectTblBookingParitiesByBookingId(Int32 bookingId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingId = " + bookingId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingParitiesTO> list = ConvertDTToList(reader);
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
          public List<TblBookingParitiesTO> SelectTblBookingParitiesByBookingId(Int32 bookingId)
        {
           String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingId = " + bookingId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingParitiesTO> list = ConvertDTToList(reader);
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


        public List<TblBookingParitiesTO> ConvertDTToList(SqlDataReader tblBookingParitiesTODT)
        {
            List<TblBookingParitiesTO> tblBookingParitiesTOList = new List<TblBookingParitiesTO>();
            if (tblBookingParitiesTODT != null)
            {
                while (tblBookingParitiesTODT.Read())
                {
                    TblBookingParitiesTO tblBookingParitiesTONew = new TblBookingParitiesTO();
                    if (tblBookingParitiesTODT["idBookingParity"] != DBNull.Value)
                        tblBookingParitiesTONew.IdBookingParity = Convert.ToInt32(tblBookingParitiesTODT["idBookingParity"].ToString());
                    if (tblBookingParitiesTODT["bookingId"] != DBNull.Value)
                        tblBookingParitiesTONew.BookingId = Convert.ToInt32(tblBookingParitiesTODT["bookingId"].ToString());
                    if (tblBookingParitiesTODT["parityId"] != DBNull.Value)
                        tblBookingParitiesTONew.ParityId = Convert.ToInt32(tblBookingParitiesTODT["parityId"].ToString());
                    if (tblBookingParitiesTODT["bookingRate"] != DBNull.Value)
                        tblBookingParitiesTONew.BookingRate = Convert.ToDouble(tblBookingParitiesTODT["bookingRate"].ToString());

                    if (tblBookingParitiesTODT["brandId"] != DBNull.Value)
                        tblBookingParitiesTONew.BrandId = Convert.ToInt32(tblBookingParitiesTODT["brandId"].ToString());

                    tblBookingParitiesTOList.Add(tblBookingParitiesTONew);
                }
            }
            return tblBookingParitiesTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingParitiesTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingParitiesTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblBookingParitiesTO tblBookingParitiesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingParities]( " +
                                "  [bookingId]" +
                                " ,[parityId]" +
                                " ,[bookingRate]" +
                                " ,[brandId]"+
                                " )" +
                    " VALUES (" +
                                "  @BookingId " +
                                " ,@ParityId " +
                                " ,@BookingRate " +
                                " , @BrandId"+
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdBookingParity", System.Data.SqlDbType.Int).Value = tblBookingParitiesTO.IdBookingParity;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingParitiesTO.BookingId;
            cmdInsert.Parameters.Add("@ParityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingParitiesTO.ParityId);
            cmdInsert.Parameters.Add("@BookingRate", System.Data.SqlDbType.Decimal).Value = tblBookingParitiesTO.BookingRate;
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblBookingParitiesTO.BrandId;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblBookingParitiesTO.IdBookingParity = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingParitiesTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingParitiesTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblBookingParitiesTO tblBookingParitiesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingParities] SET " + 
                            "  [bookingId]= @BookingId" +
                            " ,[parityId] = @ParityId" +
                            " ,[bookingRate] = @BookingRate" +
                            " ,[brandId]= @BrandId"+
                            " WHERE [idBookingParity] = @IdBookingParity"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBookingParity", System.Data.SqlDbType.Int).Value = tblBookingParitiesTO.IdBookingParity;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingParitiesTO.BookingId;
            cmdUpdate.Parameters.Add("@ParityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingParitiesTO.ParityId);
            cmdUpdate.Parameters.Add("@BookingRate", System.Data.SqlDbType.Decimal).Value = tblBookingParitiesTO.BookingRate;
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblBookingParitiesTO.BrandId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblBookingParities(Int32 idBookingParity)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBookingParity, cmdDelete);
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

        public int DeleteTblBookingParities(Int32 idBookingParity, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBookingParity, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idBookingParity, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingParities] " +
            " WHERE idBookingParity = " + idBookingParity +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookingParity", System.Data.SqlDbType.Int).Value = tblBookingParitiesTO.IdBookingParity;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
