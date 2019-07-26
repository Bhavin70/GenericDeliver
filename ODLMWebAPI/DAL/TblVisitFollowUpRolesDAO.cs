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
    public class TblVisitFollowUpRolesDAO : ITblVisitFollowUpRolesDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblVisitFollowUpRolesDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVisitFollowUpRoles] ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblVisitFollowUpRolesTO> SelectAllTblVisitFollowUpRoles()
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

                SqlDataReader visitFollowUpRoleDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitFollowUpRolesTO> list = ConvertDTToList(visitFollowUpRoleDT);
                return list;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitFollowUpRoles");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DataTable SelectTblVisitFollowUpRoles(Int32 idVisitFollowUpRole)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVisitFollowUpRole = " + idVisitFollowUpRole + " ";
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

        public DataTable SelectAllTblVisitFollowUpRoles(SqlConnection conn, SqlTransaction tran)
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

        public List<TblVisitFollowUpRolesTO> ConvertDTToList(SqlDataReader visitFollowUpRoleDT)
        {
            List<TblVisitFollowUpRolesTO> visitFollowUpRoleTOList = new List<TblVisitFollowUpRolesTO>();
            if (visitFollowUpRoleDT != null)
            {
                while (visitFollowUpRoleDT.Read())
                {
                    TblVisitFollowUpRolesTO visitFollowUpRoleTONew = new TblVisitFollowUpRolesTO();
                    if (visitFollowUpRoleDT["idVisitFollowUpRole"] != DBNull.Value)
                        visitFollowUpRoleTONew.IdVisitFollowUpRole = Convert.ToInt32(visitFollowUpRoleDT["idVisitFollowUpRole"].ToString());
                    if (visitFollowUpRoleDT["followUpActionId"] != DBNull.Value)
                        visitFollowUpRoleTONew.FollowUpActionId = Convert.ToInt32(visitFollowUpRoleDT["followUpActionId"]);
                    if (visitFollowUpRoleDT["visitFollowUpRoleName"] != DBNull.Value)
                        visitFollowUpRoleTONew.VisitFollowUpRoleName = visitFollowUpRoleDT["visitFollowUpRoleName"].ToString();
                    if (visitFollowUpRoleDT["visitFollowUpRoleDesc"] != DBNull.Value)
                        visitFollowUpRoleTONew.VisitFollowUpRoleDesc = visitFollowUpRoleDT["visitFollowUpRoleDesc"].ToString();                    
                    if (visitFollowUpRoleDT["createdBy"] != DBNull.Value)
                        visitFollowUpRoleTONew.CreatedBy = Convert.ToInt32(visitFollowUpRoleDT["createdBy"].ToString());
                    if (visitFollowUpRoleDT["createdOn"] != DBNull.Value)
                        visitFollowUpRoleTONew.CreatedOn = Convert.ToDateTime(visitFollowUpRoleDT["createdOn"].ToString());
                    if (visitFollowUpRoleDT["updatedBy"] != DBNull.Value)
                        visitFollowUpRoleTONew.UpdatedBy = Convert.ToInt32(visitFollowUpRoleDT["updatedBy"].ToString());
                    if (visitFollowUpRoleDT["updatedOn"] != DBNull.Value)
                        visitFollowUpRoleTONew.UpdatedOn = Convert.ToDateTime(visitFollowUpRoleDT["updatedOn"].ToString());
                    if (visitFollowUpRoleDT["isActive"] != DBNull.Value)
                        visitFollowUpRoleTONew.IsActive = Convert.ToInt32(visitFollowUpRoleDT["isActive"].ToString());
                    visitFollowUpRoleTOList.Add(visitFollowUpRoleTONew);
                }
            }
            return visitFollowUpRoleTOList;
        }


        public List<DropDownTO> SelectFollowUpUserRoleListForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                String sqlQuery = " SELECT roleId,roleDesc,followUpActionId FROM tblVisitFollowUpRoles tblvisifollowuprole "+
                                  " INNER JOIN tblRole tblrole ON tblrole.idRole = tblvisifollowuprole.roleId";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblUnitMeasuresTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblUnitMeasuresTODT != null)
                {
                    while (tblUnitMeasuresTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblUnitMeasuresTODT["roleId"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblUnitMeasuresTODT["roleId"].ToString());
                        if (tblUnitMeasuresTODT["roleDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblUnitMeasuresTODT["roleDesc"].ToString());
                        if (tblUnitMeasuresTODT["followUpActionId"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(tblUnitMeasuresTODT["followUpActionId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectFollowUpRoleListForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE roleId is null ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblUnitMeasuresTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblUnitMeasuresTODT != null)
                {
                    while (tblUnitMeasuresTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblUnitMeasuresTODT["idVisitFollowUpRole"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblUnitMeasuresTODT["idVisitFollowUpRole"].ToString());
                        if (tblUnitMeasuresTODT["visitFollowUpRoleName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblUnitMeasuresTODT["visitFollowUpRoleName"].ToString());
                        if (tblUnitMeasuresTODT["followUpActionId"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(tblUnitMeasuresTODT["followUpActionId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Select Visit Role List
        public List<DropDownTO> SelectVisitRoleForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();

                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                SqlCommand cmdSelect = new SqlCommand();

                try
                {
                    conn.Open();
                    String sqlQuery = " Select idVisitRole,visitRoleName from tblVisitRole WHERE isActive= 1 ";

                    cmdSelect.CommandText = sqlQuery;
                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                    SqlDataReader visitRoleDT = cmdSelect.ExecuteReader(CommandBehavior.Default);


                    List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                    if (visitRoleDT != null)
                    {
                        while (visitRoleDT.Read())
                        {
                            DropDownTO dropDownTO = new DropDownTO();
                            if (visitRoleDT["idVisitRole"] != DBNull.Value)
                                dropDownTO.Value = Convert.ToInt32(visitRoleDT["idVisitRole"].ToString());
                            if (visitRoleDT["visitRoleName"] != DBNull.Value)
                                dropDownTO.Text = Convert.ToString(visitRoleDT["visitRoleName"].ToString());
                            dropDownTOList.Add(dropDownTO);
                        }
                    }
                    return dropDownTOList;
                }
                catch (Exception ex)
                {
                    resultMessage.DefaultExceptionBehaviour(ex, "SelectVisitRole");
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
        public int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVisitFollowUpRolesTO, cmdInsert);
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

        public int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVisitFollowUpRolesTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitFollowUpRoles]( " +
            "  [idVisitFollowUpRole]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[visitFollowUpRoleName]" +
            " ,[visitFollowUpRoleDesc]" +
            " )" +
" VALUES (" +
            "  @IdVisitFollowUpRole " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@VisitFollowUpRoleName " +
            " ,@VisitFollowUpRoleDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdVisitFollowUpRole", System.Data.SqlDbType.Int).Value = tblVisitFollowUpRolesTO.IdVisitFollowUpRole;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitFollowUpRolesTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVisitFollowUpRolesTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitFollowUpRolesTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitFollowUpRolesTO.UpdatedOn;
            cmdInsert.Parameters.Add("@VisitFollowUpRoleName", System.Data.SqlDbType.NVarChar).Value = tblVisitFollowUpRolesTO.VisitFollowUpRoleName;
            cmdInsert.Parameters.Add("@VisitFollowUpRoleDesc", System.Data.SqlDbType.NVarChar).Value = tblVisitFollowUpRolesTO.VisitFollowUpRoleDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitFollowUpRolesTO, cmdUpdate);
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

        public int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitFollowUpRolesTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVisitFollowUpRoles] SET " +
            "  [idVisitFollowUpRole] = @IdVisitFollowUpRole" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[visitFollowUpRoleName]= @VisitFollowUpRoleName" +
            " ,[visitFollowUpRoleDesc] = @VisitFollowUpRoleDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVisitFollowUpRole", System.Data.SqlDbType.Int).Value = tblVisitFollowUpRolesTO.IdVisitFollowUpRole;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitFollowUpRolesTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVisitFollowUpRolesTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitFollowUpRolesTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitFollowUpRolesTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@VisitFollowUpRoleName", System.Data.SqlDbType.NVarChar).Value = tblVisitFollowUpRolesTO.VisitFollowUpRoleName;
            cmdUpdate.Parameters.Add("@VisitFollowUpRoleDesc", System.Data.SqlDbType.NVarChar).Value = tblVisitFollowUpRolesTO.VisitFollowUpRoleDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVisitFollowUpRole, cmdDelete);
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

        public int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVisitFollowUpRole, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idVisitFollowUpRole, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitFollowUpRoles] " +
            " WHERE idVisitFollowUpRole = " + idVisitFollowUpRole + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVisitFollowUpRole", System.Data.SqlDbType.Int).Value = tblVisitFollowUpRolesTO.IdVisitFollowUpRole;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
