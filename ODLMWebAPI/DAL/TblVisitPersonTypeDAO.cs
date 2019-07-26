using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblVisitPersonTypeDAO : ITblVisitPersonTypeDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblVisitPersonTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVisitPersonType]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public DataTable SelectAllTblVisitPersonType()
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

        public DataTable SelectTblVisitPersonType(Int32 idPersonType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPersonType = " + idPersonType + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

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

        public DataTable SelectAllTblVisitPersonType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
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
        public int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVisitPersonTypeTO, cmdInsert);
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

        public int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVisitPersonTypeTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitPersonType]( " +
            "  [idPersonType]" +
            " ,[isActive]" +
            " ,[personTypeName]" +
            " ,[personRoleDesc]" +
            " )" +
" VALUES (" +
            "  @IdPersonType " +
            " ,@IsActive " +
            " ,@PersonTypeName " +
            " ,@PersonRoleDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPersonType", System.Data.SqlDbType.Int).Value = tblVisitPersonTypeTO.IdPersonType;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVisitPersonTypeTO.IsActive;
            cmdInsert.Parameters.Add("@PersonTypeName", System.Data.SqlDbType.NVarChar).Value = tblVisitPersonTypeTO.PersonTypeName;
            cmdInsert.Parameters.Add("@PersonRoleDesc", System.Data.SqlDbType.NVarChar).Value = tblVisitPersonTypeTO.PersonRoleDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitPersonTypeTO, cmdUpdate);
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

        public int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitPersonTypeTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVisitPersonType] SET " +
            "  [idPersonType] = @IdPersonType" +
            " ,[isActive]= @IsActive" +
            " ,[personTypeName]= @PersonTypeName" +
            " ,[personRoleDesc] = @PersonRoleDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPersonType", System.Data.SqlDbType.Int).Value = tblVisitPersonTypeTO.IdPersonType;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVisitPersonTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@PersonTypeName", System.Data.SqlDbType.NVarChar).Value = tblVisitPersonTypeTO.PersonTypeName;
            cmdUpdate.Parameters.Add("@PersonRoleDesc", System.Data.SqlDbType.NVarChar).Value = tblVisitPersonTypeTO.PersonRoleDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblVisitPersonType(Int32 idPersonType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPersonType, cmdDelete);
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

        public int DeleteTblVisitPersonType(Int32 idPersonType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPersonType, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idPersonType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitPersonType] " +
            " WHERE idPersonType = " + idPersonType + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPersonType", System.Data.SqlDbType.Int).Value = tblVisitPersonTypeTO.IdPersonType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
