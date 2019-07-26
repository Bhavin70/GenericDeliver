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
    public class TblGroupDAO : ITblGroupDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblGroupDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblGroup]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblGroupTO> SelectAllTblGroup()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+" WHERE isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupTO> list = ConvertDTToList(sqlReader);
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

        public TblGroupTO SelectTblGroup(Int32 idGroup)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idGroup = " + idGroup + " AND isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupTO> list = ConvertDTToList(sqlReader);
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

        public List<TblGroupTO> SelectAllTblGroup(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery()+" WHERE isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupTO> list = ConvertDTToList(sqlReader);
                return list;
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


        public List<TblGroupTO> ConvertDTToList(SqlDataReader tblGroupTODT)
        {
            List<TblGroupTO> tblGroupTOList = new List<TblGroupTO>();
            if (tblGroupTODT != null)
            {
                while (tblGroupTODT.Read())
                {
                    TblGroupTO tblGroupTONew = new TblGroupTO();
                    if (tblGroupTODT["idGroup"] != DBNull.Value)
                        tblGroupTONew.IdGroup = Convert.ToInt32(tblGroupTODT["idGroup"].ToString());
                    if (tblGroupTODT["createdBy"] != DBNull.Value)
                        tblGroupTONew.CreatedBy = Convert.ToInt32(tblGroupTODT["createdBy"].ToString());
                    if (tblGroupTODT["isActive"] != DBNull.Value)
                        tblGroupTONew.IsActive = Convert.ToInt32(tblGroupTODT["isActive"].ToString());
                    if (tblGroupTODT["createdOn"] != DBNull.Value)
                        tblGroupTONew.CreatedOn = Convert.ToDateTime(tblGroupTODT["createdOn"].ToString());
                    if (tblGroupTODT["groupName"] != DBNull.Value)
                        tblGroupTONew.GroupName = Convert.ToString(tblGroupTODT["groupName"].ToString());
                    if (tblGroupTODT["updatedBy"] != DBNull.Value)
                        tblGroupTONew.UpdatedBy = Convert.ToInt32(tblGroupTODT["updatedBy"].ToString());
                    if (tblGroupTODT["updatedOn"] != DBNull.Value)
                        tblGroupTONew.UpdatedOn = Convert.ToDateTime(tblGroupTODT["updatedOn"].ToString());
                    if (tblGroupTODT["lowerLimit"] != DBNull.Value)
                        tblGroupTONew.LowerLimit = Convert.ToDouble(tblGroupTODT["lowerLimit"].ToString());
                    if (tblGroupTODT["upperLimit"] != DBNull.Value)
                        tblGroupTONew.UpperLimit = Convert.ToDouble(tblGroupTODT["upperLimit"].ToString());
                    tblGroupTOList.Add(tblGroupTONew);
                }
            }
            return tblGroupTOList;
        }

        public List<TblGroupTO> SelectAllGroupList(TblGroupTO tblGroupTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE groupName = '" + tblGroupTO.GroupName + "'";
                if (tblGroupTO.IdGroup > 0)
                {
                    cmdSelect.CommandText += " AND idGroup != " + tblGroupTO.IdGroup+ " AND isActive = 1";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupTO> list = ConvertDTToList(sqlReader);
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


        public List<TblGroupTO> SelectAllActiveGroupList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where isActive=1 "; 
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupTO> list = ConvertDTToList(sqlReader);
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
        #endregion

        #region Insertion
        public int InsertTblGroup(TblGroupTO tblGroupTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblGroupTO, cmdInsert);
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

        public int InsertTblGroup(TblGroupTO tblGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblGroupTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblGroupTO tblGroupTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblGroup]( " + 
            //"  [idGroup]" +
            "  [createdBy]" +
            " ,[isActive]"  +
            " ,[createdOn]" +
            " ,[groupName]" +
            " ,[lowerLimit]" +
            " ,[upperLimit]" +
            " ,[updatedBy]" +
            " ,[updatedOn]" +

            " )" +
" VALUES (" +
            //"  @IdGroup " +
            "  @CreatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@GroupName " +
            " ,@LowerLimit"+
            " ,@UpperLimit"+
            " ,@UpdatedBy " +
            ", @UpdatedOn " +
          
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdGroup", System.Data.SqlDbType.Int).Value = tblGroupTO.IdGroup;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGroupTO.CreatedBy;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblGroupTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGroupTO.CreatedOn;
            cmdInsert.Parameters.Add("@GroupName", System.Data.SqlDbType.NVarChar).Value = tblGroupTO.GroupName;
            cmdInsert.Parameters.Add("@LowerLimit", System.Data.SqlDbType.NVarChar).Value = tblGroupTO.LowerLimit;
            cmdInsert.Parameters.Add("@UpperLimit", System.Data.SqlDbType.NVarChar).Value = tblGroupTO.UpperLimit;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGroupTO.UpdatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGroupTO.UpdatedOn);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblGroupTO.IdGroup = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblGroup(TblGroupTO tblGroupTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblGroupTO, cmdUpdate);
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

        public int UpdateTblGroup(TblGroupTO tblGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblGroupTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblGroupTO tblGroupTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblGroup] SET " +

            " [createdBy]= @CreatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[lowerLimit]= @LowerLimit" +
            " ,[upperLimit]= @UpperLimit" +
            " ,[groupName] = @GroupName" +
            " WHERE   [idGroup] = @IdGroup" ;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGroup", System.Data.SqlDbType.Int).Value = tblGroupTO.IdGroup;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGroupTO.CreatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblGroupTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGroupTO.CreatedOn;
            cmdUpdate.Parameters.Add("@GroupName", System.Data.SqlDbType.NVarChar).Value = tblGroupTO.GroupName;
            cmdUpdate.Parameters.Add("@LowerLimit", System.Data.SqlDbType.NVarChar).Value = tblGroupTO.LowerLimit;
            cmdUpdate.Parameters.Add("@UpperLimit", System.Data.SqlDbType.NVarChar).Value = tblGroupTO.UpperLimit;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblGroupTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblGroupTO.UpdatedOn;


            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblGroup(Int32 idGroup)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGroup, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblGroup(Int32 idGroup, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGroup, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idGroup, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblGroup] " +
            " WHERE idGroup = " + idGroup +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGroup", System.Data.SqlDbType.Int).Value = tblGroupTO.IdGroup;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
