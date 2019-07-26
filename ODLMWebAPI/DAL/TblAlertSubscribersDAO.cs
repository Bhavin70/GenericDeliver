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
    public class TblAlertSubscribersDAO : ITblAlertSubscribersDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblAlertSubscribersDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = "SELECT  alertSub.*,tbluser.userDisplayName,tblrole.roleDesc from tblAlertSubscribers alertSub" +
                                  " LEFT JOIN tblUser tbluser ON tbluser.idUser = alertSub.userId " +
                                  " LEFT JOIN tblRole tblrole ON tblrole.idRole = alertSub.roleId " ;
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblAlertSubscribersTO> SelectAllTblAlertSubscribers()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Priyanka [20-09-2018] : Added to get the alert subscriber list by AlertDefId.
        public List<TblAlertSubscribersTO> SelectTblAlertSubscribersByAlertDefId(Int32 alertDefId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE alertSub.alertDefId = " + alertDefId + " AND alertSub.isActive = 1"; ;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblAlertSubscribersTO> SelectAllTblAlertSubscribers(Int32 alertDefId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE alertSub.alertDefId=" + alertDefId + " AND alertSub.isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TblAlertSubscribersTO SelectTblAlertSubscribers(Int32 idSubscription)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE alertSub.idSubscription = " + idSubscription;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertSubscribersTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblAlertSubscribersTO> ConvertDTToList(SqlDataReader tblAlertSubscribersTODT)
        {
            List<TblAlertSubscribersTO> tblAlertSubscribersTOList = new List<TblAlertSubscribersTO>();
            if (tblAlertSubscribersTODT != null)
            {
                while (tblAlertSubscribersTODT.Read())
                {
                    TblAlertSubscribersTO tblAlertSubscribersTONew = new TblAlertSubscribersTO();
                    if (tblAlertSubscribersTODT["idSubscription"] != DBNull.Value)
                        tblAlertSubscribersTONew.IdSubscription = Convert.ToInt32(tblAlertSubscribersTODT["idSubscription"].ToString());
                    if (tblAlertSubscribersTODT["alertDefId"] != DBNull.Value)
                        tblAlertSubscribersTONew.AlertDefId = Convert.ToInt32(tblAlertSubscribersTODT["alertDefId"].ToString());
                    if (tblAlertSubscribersTODT["userId"] != DBNull.Value)
                        tblAlertSubscribersTONew.UserId = Convert.ToInt32(tblAlertSubscribersTODT["userId"].ToString());
                    if (tblAlertSubscribersTODT["roleId"] != DBNull.Value)
                        tblAlertSubscribersTONew.RoleId = Convert.ToInt32(tblAlertSubscribersTODT["roleId"].ToString());
                    if (tblAlertSubscribersTODT["subscribedBy"] != DBNull.Value)
                        tblAlertSubscribersTONew.SubscribedBy = Convert.ToInt32(tblAlertSubscribersTODT["subscribedBy"].ToString());
                    if (tblAlertSubscribersTODT["subscribedOn"] != DBNull.Value)
                        tblAlertSubscribersTONew.SubscribedOn = Convert.ToDateTime(tblAlertSubscribersTODT["subscribedOn"].ToString());

                    //Priyanka [20-09-18]
                    if (tblAlertSubscribersTODT["isActive"] != DBNull.Value)
                        tblAlertSubscribersTONew.IsActive = Convert.ToInt32(tblAlertSubscribersTODT["isActive"].ToString());
                    if (tblAlertSubscribersTODT["updatedBy"] != DBNull.Value)
                        tblAlertSubscribersTONew.UpdatedBy = Convert.ToInt32(tblAlertSubscribersTODT["updatedBy"].ToString());
                    if (tblAlertSubscribersTODT["updatedOn"] != DBNull.Value)
                        tblAlertSubscribersTONew.UpdatedOn = Convert.ToDateTime(tblAlertSubscribersTODT["updatedOn"].ToString());

                    if (tblAlertSubscribersTODT["userDisplayName"] != DBNull.Value)
                        tblAlertSubscribersTONew.UserDisplayName = Convert.ToString(tblAlertSubscribersTODT["userDisplayName"].ToString());
                    if (tblAlertSubscribersTODT["roleDesc"] != DBNull.Value)
                        tblAlertSubscribersTONew.RoleDesc = Convert.ToString(tblAlertSubscribersTODT["roleDesc"].ToString());


                    tblAlertSubscribersTOList.Add(tblAlertSubscribersTONew);
                }
            }
            return tblAlertSubscribersTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAlertSubscribersTO, cmdInsert);
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

        public int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAlertSubscribersTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblAlertSubscribersTO tblAlertSubscribersTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAlertSubscribers]( " +
                            "  [alertDefId]" +
                            " ,[userId]" +
                            " ,[roleId]" +
                            " ,[subscribedBy]" +
                            " ,[subscribedOn]" +
                            " ,[isActive] " +
                            " ,[updatedBy] "+
                            " ,[updatedOn] "+
                            " )" +
                " VALUES (" +
                            "  @AlertDefId " +
                            " ,@UserId " +
                            " ,@RoleId " +
                            " ,@SubscribedBy " +
                            " ,@SubscribedOn " +
                            " ,@IsActive " +
                            " ,@UpdatedBy " +
                            " ,@UpdatedOn" +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSubscription", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.IdSubscription;
            cmdInsert.Parameters.Add("@AlertDefId", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.AlertDefId;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscribersTO.UserId);
            cmdInsert.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscribersTO.RoleId);
            cmdInsert.Parameters.Add("@SubscribedBy", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.SubscribedBy;
            cmdInsert.Parameters.Add("@SubscribedOn", System.Data.SqlDbType.DateTime).Value = tblAlertSubscribersTO.SubscribedOn;

            //Priyanka [20-09-18] Added
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.IsActive;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscribersTO.UpdatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscribersTO.UpdatedOn);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAlertSubscribersTO.IdSubscription = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAlertSubscribersTO, cmdUpdate);
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

        public int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAlertSubscribersTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblAlertSubscribersTO tblAlertSubscribersTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAlertSubscribers] SET " + 
                            "  [alertDefId]= @AlertDefId" +
                            " ,[userId]= @UserId" +
                            " ,[roleId]= @RoleId" +
                            " ,[subscribedBy]= @SubscribedBy" +
                            " ,[subscribedOn] = @SubscribedOn" +
                            " ,[isActive] = @IsActive" +
                            " ,[updatedOn] = @UpdatedOn " +
                            " ,[updatedBy] = @UpdatedBy " +
                            " WHERE [idSubscription] = @IdSubscription "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSubscription", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.IdSubscription;
            cmdUpdate.Parameters.Add("@AlertDefId", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.AlertDefId;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscribersTO.UserId);
            cmdUpdate.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscribersTO.RoleId);
            cmdUpdate.Parameters.Add("@SubscribedBy", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.SubscribedBy;
            cmdUpdate.Parameters.Add("@SubscribedOn", System.Data.SqlDbType.DateTime).Value = tblAlertSubscribersTO.SubscribedOn;

            //Priyanka[20-09-18] Added
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.IsActive;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertSubscribersTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblAlertSubscribers(Int32 idSubscription)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSubscription, cmdDelete);
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

        public int DeleteTblAlertSubscribers(Int32 idSubscription, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSubscription, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idSubscription, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAlertSubscribers] " +
            " WHERE idSubscription = " + idSubscription +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSubscription", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.IdSubscription;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
