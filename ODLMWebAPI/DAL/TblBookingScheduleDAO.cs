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
    public class TblBookingScheduleDAO : ITblBookingScheduleDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblBookingDelAddrDAO _iTblBookingDelAddrDAO;
        private readonly ITblBookingExtDAO _iTblBookingExtDAO;
        public TblBookingScheduleDAO(IConnectionString iConnectionString, ITblBookingDelAddrDAO iTblBookingDelAddrDAO, ITblBookingExtDAO iTblBookingExtDAO)
        {
            _iConnectionString = iConnectionString;
            _iTblBookingDelAddrDAO = iTblBookingDelAddrDAO;
            _iTblBookingExtDAO = iTblBookingExtDAO;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT bookingSchedule.*,b.isItemized,loadingLayer.layerDesc as loadingLayerDesc FROM [tblBookingSchedule] bookingSchedule" +
                " Left join dimLoadingLayers loadingLayer on bookingSchedule.loadingLayerId= loadingLayer.idLoadingLayer"+
                " left join tblbookings b on b.idbooking=bookingSchedule.bookingid "; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblBookingScheduleTO> SelectAllTblBookingSchedule()
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

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
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

        public TblBookingScheduleTO SelectTblBookingSchedule(Int32 idSchedule)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSchedule = " + idSchedule +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblBookingScheduleTO SelectAllTblBookingSchedule(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
                cmdSelect.Dispose();
            }
        }


        public List<TblBookingScheduleTO> ConvertDTToList(SqlDataReader tblBookingScheduleTODT)
        {
            List<TblBookingScheduleTO> tblBookingScheduleTOList = new List<TblBookingScheduleTO>();
            if (tblBookingScheduleTODT != null)
            {
                while (tblBookingScheduleTODT.Read())
                {
                    TblBookingScheduleTO tblBookingScheduleTONew = new TblBookingScheduleTO();
                    if (tblBookingScheduleTODT["idSchedule"] != DBNull.Value)
                        tblBookingScheduleTONew.IdSchedule = Convert.ToInt32(tblBookingScheduleTODT["idSchedule"].ToString());
                    if (tblBookingScheduleTODT["bookingId"] != DBNull.Value)
                        tblBookingScheduleTONew.BookingId = Convert.ToInt32(tblBookingScheduleTODT["bookingId"].ToString());
                    if (tblBookingScheduleTODT["createdBy"] != DBNull.Value)
                        tblBookingScheduleTONew.CreatedBy = Convert.ToInt32(tblBookingScheduleTODT["createdBy"].ToString());
                    if (tblBookingScheduleTODT["updatedBy"] != DBNull.Value)
                        tblBookingScheduleTONew.UpdatedBy = Convert.ToInt32(tblBookingScheduleTODT["updatedBy"].ToString());
                    if (tblBookingScheduleTODT["scheduleDate"] != DBNull.Value)
                        tblBookingScheduleTONew.ScheduleDate = Convert.ToDateTime(tblBookingScheduleTODT["scheduleDate"].ToString());
                    if (tblBookingScheduleTODT["createdOn"] != DBNull.Value)
                        tblBookingScheduleTONew.CreatedOn = Convert.ToDateTime(tblBookingScheduleTODT["createdOn"].ToString());
                    if (tblBookingScheduleTODT["updatedOn"] != DBNull.Value)
                        tblBookingScheduleTONew.UpdatedOn = Convert.ToDateTime(tblBookingScheduleTODT["updatedOn"].ToString());
                    if (tblBookingScheduleTODT["Qty"] != DBNull.Value)
                        tblBookingScheduleTONew.Qty = Convert.ToDouble(tblBookingScheduleTODT["Qty"].ToString());
                    if (tblBookingScheduleTODT["remark"] != DBNull.Value)
                        tblBookingScheduleTONew.Remark = Convert.ToString(tblBookingScheduleTODT["remark"].ToString());
                    if (tblBookingScheduleTODT["loadingLayerId"] != DBNull.Value)
                        tblBookingScheduleTONew.LoadingLayerId = Convert.ToInt32(tblBookingScheduleTODT["loadingLayerId"].ToString());
                    if (tblBookingScheduleTODT["loadingLayerDesc"] != DBNull.Value)
                        tblBookingScheduleTONew.LoadingLayerDesc = Convert.ToString(tblBookingScheduleTODT["loadingLayerDesc"].ToString());
                    if (tblBookingScheduleTODT["noOfLayers"] != DBNull.Value)
                        tblBookingScheduleTONew.NoOfLayers = Convert.ToInt32(tblBookingScheduleTODT["noOfLayers"].ToString());
                    if (tblBookingScheduleTODT["scheduleGroupId"] != DBNull.Value)
                        tblBookingScheduleTONew.ScheduleGroupId = Convert.ToInt32(tblBookingScheduleTODT["scheduleGroupId"].ToString());
                    if (tblBookingScheduleTODT["isItemized"] != DBNull.Value)
                        tblBookingScheduleTONew.IsItemized = Convert.ToInt32(tblBookingScheduleTODT["isItemized"]);
                    tblBookingScheduleTONew.ScheduleDateStr = tblBookingScheduleTONew.ScheduleDate.ToString("dd/MMM/yyyy");

                    tblBookingScheduleTOList.Add(tblBookingScheduleTONew);
                }
            }
            return tblBookingScheduleTOList;
        }



        public List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingId =" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
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

        public List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId,SqlConnection conn , SqlTransaction tran)
        {
           
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
               
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingId =" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
               
                cmdSelect.Dispose();
            }
        }




        #endregion

        #region Insertion
        public int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingScheduleTO, cmdInsert);
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

        public int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingScheduleTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblBookingScheduleTO tblBookingScheduleTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingSchedule]( " + 
            //"  [idSchedule]" +
            " [bookingId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[scheduleDate]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[qty]" +
            " ,[remark]" +
            " ,[loadingLayerId]" +
            " ,[noOfLayers]" +
            " ,[scheduleGroupId]" +
            " )" +
" VALUES (" +
            //"  @IdSchedule " +
            " @BookingId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@ScheduleDate " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Qty " +
            " ,@Remark " +
            " ,@LoadingLayerId " +
             ",@NoOfLayers " +
             ",@ScheduleGroupId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            //cmdInsert.Parameters.Add("@IdSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.BookingId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.UpdatedBy);
            cmdInsert.Parameters.Add("@ScheduleDate", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.ScheduleDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.UpdatedOn);
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.Qty);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.Remark);
            cmdInsert.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.LoadingLayerId);
            cmdInsert.Parameters.Add("@NoOfLayers", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.NoOfLayers);
            cmdInsert.Parameters.Add("@ScheduleGroupId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.ScheduleGroupId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblBookingScheduleTO.IdSchedule = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingScheduleTO, cmdUpdate);
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

        public int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingScheduleTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblBookingScheduleTO tblBookingScheduleTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingSchedule] SET " + 
            " [bookingId]= @BookingId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[scheduleDate]= @ScheduleDate" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[qty]= @Qty" +
            " ,[remark] = @Remark" +
             " [loadingLayerId]= @LoadingLayerId" +
             " [noOfLayers]= @NoOfLayers" +
             " [scheduleGroupId]= @ScheduleGroupId" +
            " WHERE   [idSchedule] = @IdSchedule";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.BookingId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@ScheduleDate", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.ScheduleDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = tblBookingScheduleTO.Qty;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblBookingScheduleTO.Remark;
            cmdUpdate.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.LoadingLayerId);
            cmdUpdate.Parameters.Add("@NoOfLayers", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.NoOfLayers);
            cmdUpdate.Parameters.Add("@ScheduleGroupId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.ScheduleGroupId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblBookingSchedule(Int32 idSchedule)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSchedule, cmdDelete);
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

        public int DeleteTblBookingSchedule(Int32 idSchedule, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSchedule, cmdDelete);
            }
            catch(Exception ex)
            {
               
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idSchedule, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingSchedule] " +
            " WHERE idSchedule = " + idSchedule +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
