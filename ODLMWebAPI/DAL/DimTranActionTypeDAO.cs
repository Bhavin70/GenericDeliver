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
    public class DimTranActionTypeDAO : IDimTranActionTypeDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimTranActionTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimTranActionType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimTranActionTypeTO> SelectAllDimTranActionType()
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
                List<DimTranActionTypeTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DimTranActionTypeTO SelectDimTranActionType(Int32 idTranActionType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTranActionType = " + idTranActionType + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimTranActionTypeTO> list = ConvertDTToList(sqlReader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimTranActionTypeTO> SelectAllDimTranActionType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
           
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimTranActionTypeTO> list = ConvertDTToList(reader);
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

        public List<DimTranActionTypeTO> ConvertDTToList(SqlDataReader dimTranActionTypeTODT)
        {
            List<DimTranActionTypeTO> dimTranActionTypeTOList = new List<DimTranActionTypeTO>();
            if (dimTranActionTypeTODT != null)
            {
                while (dimTranActionTypeTODT.Read())
                {
                    
                    DimTranActionTypeTO dimTranActionTypeTONew = new DimTranActionTypeTO();
                    if (dimTranActionTypeTODT["idTranActionType"] != DBNull.Value)
                        dimTranActionTypeTONew.IdTranActionType = Convert.ToInt32(dimTranActionTypeTODT["idTranActionType"].ToString());
                    if (dimTranActionTypeTODT["isActive"] != DBNull.Value)
                        dimTranActionTypeTONew.IsActive = Convert.ToInt32(dimTranActionTypeTODT["isActive"].ToString());
                    if (dimTranActionTypeTODT["transName"] != DBNull.Value)
                        dimTranActionTypeTONew.TransName = Convert.ToString(dimTranActionTypeTODT["transName"].ToString());
                    dimTranActionTypeTOList.Add(dimTranActionTypeTONew);
                }
            }
            return dimTranActionTypeTOList;
        }
     
        #endregion
        
        #region Insertion
        public int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimTranActionTypeTO, cmdInsert);
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

        public int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimTranActionTypeTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimTranActionTypeTO dimTranActionTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimTranActionType]( " + 
            "  [idTranActionType]" +
            " ,[isActive]" +
            " ,[transName]" +
            " )" +
" VALUES (" +
            "  @IdTranActionType " +
            " ,@IsActive " +
            " ,@TransName " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdTranActionType", System.Data.SqlDbType.Int).Value = dimTranActionTypeTO.IdTranActionType;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimTranActionTypeTO.IsActive;
            cmdInsert.Parameters.Add("@TransName", System.Data.SqlDbType.NVarChar).Value = dimTranActionTypeTO.TransName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimTranActionTypeTO, cmdUpdate);
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

        public int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimTranActionTypeTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimTranActionTypeTO dimTranActionTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimTranActionType] SET " + 
            "  [idTranActionType] = @IdTranActionType" +
            " ,[isActive]= @IsActive" +
            " ,[transName] = @TransName" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTranActionType", System.Data.SqlDbType.Int).Value = dimTranActionTypeTO.IdTranActionType;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimTranActionTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@TransName", System.Data.SqlDbType.NVarChar).Value = dimTranActionTypeTO.TransName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimTranActionType(Int32 idTranActionType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTranActionType, cmdDelete);
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

        public int DeleteDimTranActionType(Int32 idTranActionType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTranActionType, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idTranActionType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimTranActionType] " +
            " WHERE idTranActionType = " + idTranActionType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
