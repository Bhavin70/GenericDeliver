using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblRoleDAO : ITblRoleDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblRoleDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblRole]";
            return sqlSelectQry;
        }
        public String SqlGetDepartmentFromUserId()
        {
            String sqlSelectQry = "select ur.*,r.roleTypeId from tblUserRole ur " +
                "Join tblRole r on r.idRole = ur.roleId " +
                "INNER JOIN tblOrgStructure tos on tos.idOrgStructure = r.orgStructureId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public TblRoleTO SelectAllTblRole()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

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

        public TblRoleTO SelectTblRole(Int32 idRole)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idRole = " + idRole + " ";
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

        public TblRoleTO SelectAllTblRole(SqlConnection conn, SqlTransaction tran)
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

        public TblRoleTO SelectTblRoleOnOrgStructureId(Int32 OrgStructureId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgStructureId = " + OrgStructureId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader dr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRoleTO> tblRoleTOList = ConvertDTToList(dr);
                if (tblRoleTOList != null && tblRoleTOList.Count == 1)
                {
                    return tblRoleTOList[0];
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

        public List<TblRoleTO> ConvertDTToList(SqlDataReader tblRoleTODT)
        {
            List<TblRoleTO> tblRoleTOList = new List<TblRoleTO>();
            if (tblRoleTODT != null)
            {
                while (tblRoleTODT.Read())
                {
                    TblRoleTO tblRoleTONew = new TblRoleTO();
                    if (tblRoleTODT["idRole"] != DBNull.Value)
                        tblRoleTONew.IdRole = Convert.ToInt32(tblRoleTODT["idRole"].ToString());
                    if (tblRoleTODT["isActive"] != DBNull.Value)
                        tblRoleTONew.IsActive = Convert.ToInt32(tblRoleTODT["isActive"].ToString());
                    if (tblRoleTODT["isSystem"] != DBNull.Value)
                        tblRoleTONew.IsSystem = Convert.ToInt32(tblRoleTODT["isSystem"].ToString());
                    if (tblRoleTODT["createdBy"] != DBNull.Value)
                        tblRoleTONew.CreatedBy = Convert.ToInt32(tblRoleTODT["createdBy"].ToString());
                    if (tblRoleTODT["enableAreaAlloc"] != DBNull.Value)
                        tblRoleTONew.EnableAreaAlloc = Convert.ToInt32(tblRoleTODT["enableAreaAlloc"].ToString());
                    if (tblRoleTODT["orgStructureId"] != DBNull.Value)
                        tblRoleTONew.OrgStructureId = Convert.ToInt32(tblRoleTODT["orgStructureId"].ToString());
                    if (tblRoleTODT["createdOn"] != DBNull.Value)
                        tblRoleTONew.CreatedOn = Convert.ToDateTime(tblRoleTODT["createdOn"].ToString());
                    if (tblRoleTODT["roleDesc"] != DBNull.Value)
                        tblRoleTONew.RoleDesc = Convert.ToString(tblRoleTODT["roleDesc"].ToString());
                    if (tblRoleTODT["roleTypeId"] != DBNull.Value)
                        tblRoleTONew.RoleTypeId = Convert.ToInt32(tblRoleTODT["roleTypeId"].ToString());
                    tblRoleTOList.Add(tblRoleTONew);
                }
            }
            return tblRoleTOList;
        }
        public List<TblRoleTO> ConvertDepartmentDTToList(SqlDataReader tblRoleTODT)
        {
            List<TblRoleTO> tblRoleTOList = new List<TblRoleTO>();
            if (tblRoleTODT != null)
            {
                while (tblRoleTODT.Read())
                {
                    TblRoleTO tblRoleTONew = new TblRoleTO();
                    if (tblRoleTODT["idRole"] != DBNull.Value)
                        tblRoleTONew.IdRole = Convert.ToInt32(tblRoleTODT["idRole"].ToString());
                    if (tblRoleTODT["isActive"] != DBNull.Value)
                        tblRoleTONew.IsActive = Convert.ToInt32(tblRoleTODT["isActive"].ToString());
                    if (tblRoleTODT["isSystem"] != DBNull.Value)
                        tblRoleTONew.IsSystem = Convert.ToInt32(tblRoleTODT["isSystem"].ToString());
                    if (tblRoleTODT["createdBy"] != DBNull.Value)
                        tblRoleTONew.CreatedBy = Convert.ToInt32(tblRoleTODT["createdBy"].ToString());
                    if (tblRoleTODT["enableAreaAlloc"] != DBNull.Value)
                        tblRoleTONew.EnableAreaAlloc = Convert.ToInt32(tblRoleTODT["enableAreaAlloc"].ToString());
                    if (tblRoleTODT["orgStructureId"] != DBNull.Value)
                        tblRoleTONew.OrgStructureId = Convert.ToInt32(tblRoleTODT["orgStructureId"].ToString());
                    if (tblRoleTODT["createdOn"] != DBNull.Value)
                        tblRoleTONew.CreatedOn = Convert.ToDateTime(tblRoleTODT["createdOn"].ToString());
                    if (tblRoleTODT["roleDesc"] != DBNull.Value)
                        tblRoleTONew.RoleDesc = Convert.ToString(tblRoleTODT["roleDesc"].ToString());
                    if (tblRoleTODT["deptId"] != DBNull.Value)
                        tblRoleTONew.DeptId = Convert.ToInt32(tblRoleTODT["deptId"].ToString());
                    if (tblRoleTODT["roleTypeId"] != DBNull.Value)
                        tblRoleTONew.RoleTypeId = Convert.ToInt32(tblRoleTODT["roleTypeId"].ToString());
                    tblRoleTOList.Add(tblRoleTONew);
                }
            }
            return tblRoleTOList;
        }


        /// <summary>
        /// Sudhir[22-AUG-2018] Added Connection , Trannsaction
        /// </summary>
        /// <param name="OrgStructureId"></param>
        /// <returns></returns>
        public TblRoleTO SelectTblRoleOnOrgStructureId(Int32 OrgStructureId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            SqlDataReader dr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgStructureId = " + OrgStructureId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                dr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRoleTO> tblRoleTOList = ConvertDTToList(dr);
                if (tblRoleTOList != null && tblRoleTOList.Count == 1)
                {
                    return tblRoleTOList[0];
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
                if (dr != null)
                    dr.Dispose();
                cmdSelect.Dispose();
            }
        }
        public TblRoleTO getDepartmentIdFromUserId(Int32 userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlGetDepartmentFromUserId() + " WHERE userId = " + userId + " and r.isActive = 1 and ur.isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader dr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRoleTO> tblRoleTOList = ConvertDepartmentDTToList(dr);
                if (tblRoleTOList != null && tblRoleTOList.Count == 1)
                {
                    return tblRoleTOList[0];
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

        #endregion

        #region Insertion
        public int InsertTblRole(TblRoleTO tblRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblRoleTO, cmdInsert);
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

        public int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblRoleTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblRole");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblRoleTO tblRoleTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblRole]( " +
            "  [isActive]" +
            " ,[isSystem]" +
            " ,[createdBy]" +
            " ,[enableAreaAlloc]" +
            " ,[orgStructureId]" +
            " ,[createdOn]" +
            " ,[roleDesc]" +
            " ,[roleTypeId]" +  // added by aniket
            " )" +
            " VALUES (" +
            "  @IsActive " +
            " ,@IsSystem " +
            " ,@CreatedBy " +
            " ,@EnableAreaAlloc " +
            " ,@OrgStructureId " +
            " ,@CreatedOn " +
            " ,@RoleDesc " +
            " ,@roleTypeId " + // added by aniket
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdRole", System.Data.SqlDbType.Int).Value = tblRoleTO.IdRole;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblRoleTO.IsActive;
            cmdInsert.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = tblRoleTO.IsSystem;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRoleTO.CreatedBy;
            cmdInsert.Parameters.Add("@EnableAreaAlloc", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblRoleTO.EnableAreaAlloc);
            cmdInsert.Parameters.Add("@OrgStructureId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblRoleTO.OrgStructureId);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRoleTO.CreatedOn;
            cmdInsert.Parameters.Add("@RoleDesc", System.Data.SqlDbType.NVarChar).Value = tblRoleTO.RoleDesc;
            cmdInsert.Parameters.Add("@roleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblRoleTO.RoleTypeId);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblRole(TblRoleTO tblRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblRoleTO, cmdUpdate);
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

        public int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblRoleTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblRoleTO tblRoleTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblRole] SET " +
            //"  [idRole] = @IdRole" +
            " [isActive]= @IsActive" +
            " ,[isSystem]= @IsSystem" +
            " ,[createdBy]= @CreatedBy" +
            " ,[enableAreaAlloc]= @EnableAreaAlloc" +
            " ,[orgStructureId]= @OrgStructureId" +
            " ,[createdOn]= @CreatedOn" +
            " ,[roleDesc] = @RoleDesc" +
            " ,[roleTypeId] = @roleTypeId" +    // added by aniket
            " WHERE idRole= @IdRole";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdRole", System.Data.SqlDbType.Int).Value = tblRoleTO.IdRole;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblRoleTO.IsActive;
            cmdUpdate.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = tblRoleTO.IsSystem;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRoleTO.CreatedBy;
            cmdUpdate.Parameters.Add("@EnableAreaAlloc", System.Data.SqlDbType.Int).Value = tblRoleTO.EnableAreaAlloc;
            cmdUpdate.Parameters.Add("@OrgStructureId", System.Data.SqlDbType.Int).Value = tblRoleTO.OrgStructureId;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRoleTO.CreatedOn;
            cmdUpdate.Parameters.Add("@RoleDesc", System.Data.SqlDbType.NVarChar).Value = tblRoleTO.RoleDesc;
            cmdUpdate.Parameters.Add("@roleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblRoleTO.RoleTypeId); // added by aniket
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblRole(Int32 idRole)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idRole, cmdDelete);
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

        public int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idRole, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idRole, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblRole] " +
            " WHERE idRole = " + idRole + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idRole", System.Data.SqlDbType.Int).Value = tblRoleTO.IdRole;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
