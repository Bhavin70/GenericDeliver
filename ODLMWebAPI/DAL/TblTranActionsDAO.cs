using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblTranActionsDAO : ITblTranActionsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblTranActionsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblTranActions] tblTranActions"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblTranActionsTO> SelectAllTblTranActions()
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
                List<TblTranActionsTO> list = ConvertDTToList(reader);
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

        public List<TblTranActionsTO> SelectAllTblTranActionsList(TblTranActionsTO tblTranActionsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string whereCon = string.Empty;

            try
            {
                conn.Open();
                cmdSelect.CommandText =  SqlSelectQuery() + " WHERE ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                if (tblTranActionsTO.TransId > 0)
                    whereCon += " tblTranActions.transId =" + tblTranActionsTO.TransId + " AND" ;
                if (tblTranActionsTO.TransTypeId > 0)
                    whereCon += " tblTranActions.transTypeId =" + tblTranActionsTO.TransTypeId +" AND";
                if (tblTranActionsTO.UserId > 0)
                    whereCon += " tblTranActions.userId =" + tblTranActionsTO.UserId + " AND";
                if (tblTranActionsTO.TranActionTypeId > 0)
                    whereCon += " tblTranActions.tranActionTypeId =" + tblTranActionsTO.TranActionTypeId + " AND";

                whereCon = whereCon.Substring(0, whereCon.Length - 3);

                cmdSelect.CommandText += whereCon;


                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTranActionsTO> list = ConvertDTToList(reader);
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


        public TblTranActionsTO SelectTblTranActions(Int32 idTranActions)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTranActions = " + idTranActions + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTranActionsTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblTranActionsTO> ConvertDTToList(SqlDataReader tblTranActionsTODT)
        {
            List<TblTranActionsTO> tblTranActionsTOList = new List<TblTranActionsTO>();
            if (tblTranActionsTODT != null)
            {
                while (tblTranActionsTODT.Read())
                {
                    TblTranActionsTO tblTranActionsTONew = new TblTranActionsTO();
                    if (tblTranActionsTODT["idTranActions"] != DBNull.Value)
                        tblTranActionsTONew.IdTranActions = Convert.ToInt32(tblTranActionsTODT["idTranActions"].ToString());
                    if (tblTranActionsTODT["userId"] != DBNull.Value)
                        tblTranActionsTONew.UserId = Convert.ToInt32(tblTranActionsTODT["userId"].ToString());
                    if (tblTranActionsTODT["tranActionTypeId"] != DBNull.Value)
                        tblTranActionsTONew.TranActionTypeId = Convert.ToInt32(tblTranActionsTODT["tranActionTypeId"].ToString());
                    if (tblTranActionsTODT["transId"] != DBNull.Value)
                        tblTranActionsTONew.TransId = Convert.ToInt32(tblTranActionsTODT["transId"].ToString());
                    if (tblTranActionsTODT["transTypeId"] != DBNull.Value)
                        tblTranActionsTONew.TransTypeId = Convert.ToInt32(tblTranActionsTODT["transTypeId"].ToString());
                    tblTranActionsTOList.Add(tblTranActionsTONew);
                }
            }
            return tblTranActionsTOList;
        }

        public List<TblTranActionsTO> SelectAllTblTranActions(Int32 idTranActions, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTranActionsTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        #endregion
        
        #region Insertion
        public int InsertTblTranActions(TblTranActionsTO tblTranActionsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblTranActionsTO, cmdInsert);
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

        public int InsertTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblTranActionsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblTranActionsTO tblTranActionsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblTranActions]( " + 
           // "  [idTranActions]" +
            " [userId]" +
            " ,[tranActionTypeId]" +
            " ,[transId]" +
            " ,[transTypeId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " )" +
" VALUES (" +
          //  "  @IdTranActions " +
            " @UserId " +
            " ,@TranActionTypeId " +
            " ,@TransId " +
            " ,@TransTypeId " + 
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

          //  cmdInsert.Parameters.Add("@IdTranActions", System.Data.SqlDbType.Int).Value = tblTranActionsTO.IdTranActions;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblTranActionsTO.UserId;
            cmdInsert.Parameters.Add("@TranActionTypeId", System.Data.SqlDbType.Int).Value = tblTranActionsTO.TranActionTypeId;
            cmdInsert.Parameters.Add("@TransId", System.Data.SqlDbType.Int).Value = tblTranActionsTO.TransId;
            cmdInsert.Parameters.Add("@TransTypeId", System.Data.SqlDbType.Int).Value = tblTranActionsTO.TransTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblTranActionsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblTranActionsTO.CreatedOn;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblTranActionsTO, cmdUpdate);
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

        public int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblTranActionsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblTranActionsTO tblTranActionsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblTranActions] SET " + 
            "  [idTranActions] = @IdTranActions" +
            " ,[userId]= @UserId" +
            " ,[tranActionTypeId]= @TranActionTypeId" +
            " ,[transId]= @TransId" +
            " ,[transTypeId] = @TransTypeId" +
            " ,[createdBy] = @CreatedBy " +
            " ,[createdOn] = @CreateOn " +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTranActions", System.Data.SqlDbType.Int).Value = tblTranActionsTO.IdTranActions;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblTranActionsTO.UserId;
            cmdUpdate.Parameters.Add("@TranActionTypeId", System.Data.SqlDbType.Int).Value = tblTranActionsTO.TranActionTypeId;
            cmdUpdate.Parameters.Add("@TransId", System.Data.SqlDbType.Int).Value = tblTranActionsTO.TransId;
            cmdUpdate.Parameters.Add("@TransTypeId", System.Data.SqlDbType.Int).Value = tblTranActionsTO.TransTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblTranActionsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblTranActionsTO.CreatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblTranActions(Int32 idTranActions)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTranActions, cmdDelete);
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

        public int DeleteTblTranActions(Int32 idTranActions, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTranActions, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idTranActions, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblTranActions] " +
            " WHERE idTranActions = " + idTranActions +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTranActions", System.Data.SqlDbType.Int).Value = tblTranActionsTO.IdTranActions;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
