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
    public class TblSupportDetailsDAO : ITblSupportDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblSupportDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSupportDetails]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblSupportDetailsTO> SelectAllTblSupportDetails()
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

                //cmdSelect.Parameters.Add("@idsupport", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.Idsupport;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSupportDetailsTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        public TblSupportDetailsTO SelectTblSupportDetails(Int32 idsupport)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idsupport = " + idsupport + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idsupport", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.Idsupport;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSupportDetailsTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();

                if (list != null && list.Count == 1)
                    return list[0];
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

        public List<TblSupportDetailsTO> SelectAllTblSupportDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idsupport", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.Idsupport;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSupportDetailsTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        public List<TblSupportDetailsTO> ConvertDTToList(SqlDataReader tblSupportDetailsTODT)
        {
            List<TblSupportDetailsTO> tblSupportDetailsTOList = new List<TblSupportDetailsTO>();
            if (tblSupportDetailsTODT != null)
            {
                while (tblSupportDetailsTODT.Read())
                {
                    TblSupportDetailsTO tblSupportDetailsTONew = new TblSupportDetailsTO();
                    if (tblSupportDetailsTODT["idsupport"] != DBNull.Value)
                        tblSupportDetailsTONew.Idsupport = Convert.ToInt32(tblSupportDetailsTODT["idsupport"].ToString());
                    if (tblSupportDetailsTODT["moduleId"] != DBNull.Value)
                        tblSupportDetailsTONew.ModuleId = Convert.ToInt32(tblSupportDetailsTODT["moduleId"].ToString());
                    if (tblSupportDetailsTODT["formid"] != DBNull.Value)
                        tblSupportDetailsTONew.Formid = Convert.ToInt32(tblSupportDetailsTODT["formid"].ToString());
                    if (tblSupportDetailsTODT["fromUser"] != DBNull.Value)
                        tblSupportDetailsTONew.FromUser = Convert.ToInt32(tblSupportDetailsTODT["fromUser"].ToString());
                    if (tblSupportDetailsTODT["createdBy"] != DBNull.Value)
                        tblSupportDetailsTONew.CreatedBy = Convert.ToInt32(tblSupportDetailsTODT["createdBy"].ToString());
                    if (tblSupportDetailsTODT["createdOn"] != DBNull.Value)
                        tblSupportDetailsTONew.CreatedOn = Convert.ToDateTime(tblSupportDetailsTODT["createdOn"].ToString());
                    if (tblSupportDetailsTODT["description"] != DBNull.Value)
                        tblSupportDetailsTONew.Description = Convert.ToString(tblSupportDetailsTODT["description"].ToString());
                    if (tblSupportDetailsTODT["requireTime"] != DBNull.Value)
                        tblSupportDetailsTONew.RequireTime = Convert.ToInt32(tblSupportDetailsTODT["requireTime"].ToString());
                    if (tblSupportDetailsTODT["comments"] != DBNull.Value)
                        tblSupportDetailsTONew.Comments = Convert.ToString(tblSupportDetailsTODT["comments"].ToString());
                    tblSupportDetailsTOList.Add(tblSupportDetailsTONew);
                }
            }
            return tblSupportDetailsTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSupportDetailsTO, cmdInsert);
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
        public int InsertTblStopServiceHistory(TblLoginTO tblLoginTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return InsertTblStopService(tblLoginTO, cmdInsert);
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
        public int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSupportDetailsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblSupportDetailsTO tblSupportDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSupportDetails]( " +
            //  "  [idsupport]" +
            " [moduleId]" +
            " ,[formid]" +
            " ,[fromUser]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[description]" +
            " ,[requireTime]" +
            " ,[comments]" +
            " )" +
" VALUES (" +
            //"  @Idsupport " +
            " @ModuleId " +
            " ,@Formid " +
            " ,@FromUser " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@Description " +
            " ,@RequireTime" +
            " ,@Comments" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@Idsupport", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.Idsupport;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.ModuleId;
            cmdInsert.Parameters.Add("@Formid", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.Formid;
            cmdInsert.Parameters.Add("@FromUser", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.FromUser;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSupportDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblSupportDetailsTO.Description;
            cmdInsert.Parameters.Add("@RequireTime", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.RequireTime;
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = tblSupportDetailsTO.Comments;
            return cmdInsert.ExecuteNonQuery();
        }

        public int InsertTblStopService(TblLoginTO tblLoginTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblstopservicehistory]( " +
                                "  [userId]" +
                                " ,[stopDate]" +
                                " ,[loginIP]" +
                                " )" +
                    " VALUES (" +
                                "  @UserId " +
                                " ,@StopDate " +
                                " ,@LoginIP " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblLoginTO.UserId;
            cmdInsert.Parameters.Add("@StopDate", System.Data.SqlDbType.DateTime).Value = tblLoginTO.LoginDate;
            cmdInsert.Parameters.Add("@LoginIP", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoginTO.LoginIP);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoginTO.IdLogin = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }


        #endregion

        #region Updation
        public int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSupportDetailsTO, cmdUpdate);
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

        public int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSupportDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblSupportDetailsTO tblSupportDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSupportDetails] SET " +
            "  [idsupport] = @Idsupport" +
            " ,[moduleId]= @ModuleId" +
            " ,[formid]= @Formid" +
            " ,[fromUser]= @FromUser" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[description] = @Description" +
            " ,[requireTime]=@RequireTime" +
            " ,[comments]=@Comments" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@Idsupport", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.Idsupport;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.ModuleId;
            cmdUpdate.Parameters.Add("@Formid", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.Formid;
            cmdUpdate.Parameters.Add("@FromUser", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.FromUser;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSupportDetailsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblSupportDetailsTO.Description;
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = tblSupportDetailsTO.Comments;
            cmdUpdate.Parameters.Add("@RequireTime", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.RequireTime;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblSupportDetails(Int32 idsupport)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idsupport, cmdDelete);
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

        public int DeleteTblSupportDetails(Int32 idsupport, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idsupport, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idsupport, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSupportDetails] " +
            " WHERE idsupport = " + idsupport + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idsupport", System.Data.SqlDbType.Int).Value = tblSupportDetailsTO.Idsupport;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
    }
}
