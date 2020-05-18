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
    public class TblUnLoadingDAO : ITblUnLoadingDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblUnLoadingDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblUnLoading]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblUnLoadingTO> SelectAllTblUnLoading(DateTime startDate, DateTime endDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                if (startDate != null && endDate != null)
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE CAST(createdOn AS DATE) BETWEEN @fromDate AND @toDate" +
                        " AND statusId NOT IN ( " + (int)Constants.TranStatusE.UNLOADING_CANCELED+" )";
                }
                else
                {
                    cmdSelect.CommandText = SqlSelectQuery();
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = startDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = endDate;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnLoadingTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblUnLoading");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblUnLoadingTO SelectTblUnLoading(Int32 idUnLoading)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idUnLoading = " + idUnLoading + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnLoadingTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();

                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblUnLoading");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblUnLoadingTO> ConvertDTToList(SqlDataReader tblUnLoadingTODT)
        {
            List<TblUnLoadingTO> tblUnLoadingTOList = new List<TblUnLoadingTO>();
            if (tblUnLoadingTODT != null)
            {
                while (tblUnLoadingTODT.Read())
                {
                    TblUnLoadingTO tblUnLoadingTONew = new TblUnLoadingTO();
                    if (tblUnLoadingTODT["idUnLoading"] != DBNull.Value)
                        tblUnLoadingTONew.IdUnLoading = Convert.ToInt32(tblUnLoadingTODT["idUnLoading"]);
                    if (tblUnLoadingTODT["refNo"] != DBNull.Value)
                        tblUnLoadingTONew.RefNo = tblUnLoadingTODT["refNo"].ToString();
                    if (tblUnLoadingTODT["SupplierOrgId"] != DBNull.Value)
                        tblUnLoadingTONew.SupplierOrgId = Convert.ToInt32(tblUnLoadingTODT["SupplierOrgId"].ToString());
                    if (tblUnLoadingTODT["vehicleNo"] != DBNull.Value)
                        tblUnLoadingTONew.VehicleNo = tblUnLoadingTODT["vehicleNo"].ToString();
                    if (tblUnLoadingTODT["descriptionId"] != DBNull.Value)
                        tblUnLoadingTONew.DescriptionId = Convert.ToInt32(tblUnLoadingTODT["descriptionId"].ToString());
                    if (tblUnLoadingTODT["description"] != DBNull.Value)
                        tblUnLoadingTONew.Description = tblUnLoadingTODT["description"].ToString();
                    if (tblUnLoadingTODT["remark"] != DBNull.Value)
                        tblUnLoadingTONew.Remark = tblUnLoadingTODT["remark"].ToString();
                    if (tblUnLoadingTODT["totalUnLoadingQty"] != DBNull.Value)
                        tblUnLoadingTONew.TotalUnLoadingQty = Convert.ToDouble(tblUnLoadingTODT["totalUnLoadingQty"]);
                    if (tblUnLoadingTODT["createdOn"] != DBNull.Value)
                        tblUnLoadingTONew.CreatedOn = Convert.ToDateTime(tblUnLoadingTODT["createdOn"]);
                    if (tblUnLoadingTODT["createdBy"] != DBNull.Value)
                        tblUnLoadingTONew.CreatedBy = Convert.ToInt32(tblUnLoadingTODT["createdBy"]);
                    if (tblUnLoadingTODT["updatedOn"] != DBNull.Value)
                        tblUnLoadingTONew.CreatedOn = Convert.ToDateTime(tblUnLoadingTODT["updatedOn"]);
                    if (tblUnLoadingTODT["updatedBy"] != DBNull.Value)
                        tblUnLoadingTONew.CreatedBy = Convert.ToInt32(tblUnLoadingTODT["updatedBy"]);
                    if (tblUnLoadingTODT["statusId"] != DBNull.Value)
                        tblUnLoadingTONew.StatusId = Convert.ToInt32(tblUnLoadingTODT["statusId"]);
                    if (tblUnLoadingTODT["statusDate"] != DBNull.Value)
                        tblUnLoadingTONew.StatusDate = Convert.ToDateTime(tblUnLoadingTODT["statusDate"]);
                    if (tblUnLoadingTODT["status"] != DBNull.Value)
                        tblUnLoadingTONew.Status = tblUnLoadingTODT["status"].ToString();
                    if (tblUnLoadingTODT["tranRefId"] != DBNull.Value)
                        tblUnLoadingTONew.TranRefId = tblUnLoadingTODT["tranRefId"].ToString();
                    if (tblUnLoadingTODT["moduleId"] != DBNull.Value)
                        tblUnLoadingTONew.ModuleId = Convert.ToInt32(tblUnLoadingTODT["moduleId"]);


                    tblUnLoadingTOList.Add(tblUnLoadingTONew);
                }
            }
            return tblUnLoadingTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUnLoadingTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUnLoadingTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblUnLoading");
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblUnLoadingTO tblUnLoadingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUnLoading]( " +
            " [refNo]" +
            " ,[SupplierOrgId]" +
            " ,[vehicleNo]" +
            " ,[descriptionId]" +
            " ,[description]" +
            " ,[remark]" +
            " ,[totalUnLoadingQty]" +
            " ,[createdOn]" +
            " ,[createdBy]" +
            " ,[updatedOn]" +
            " ,[updatedBy]" +
            " ,[statusId]" +
            " ,[statusDate]" +
            " ,[status]" +
            " )" +
            " VALUES (" +
            " @RefNo " +
            " ,@SupplierOrgId " +
            " ,@VehicleNo " +
            " ,@DescriptionId " +
            " ,@Description " +
            " ,@Remark " +
            " ,@TotalUnLoadingQty " +
            " ,@CreatedOn " +
            " ,@CreatedBy " +
            " ,@UpdatedOn " +
            " ,@UpdatedBy " +
            " ,@statusId " +
            " ,@statusDate " +
            " ,@status " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@RefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.RefNo);
            cmdInsert.Parameters.Add("@SupplierOrgId", System.Data.SqlDbType.Int).Value = tblUnLoadingTO.SupplierOrgId;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.VehicleNo);
            cmdInsert.Parameters.Add("@DescriptionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.DescriptionId);
            cmdInsert.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.Description);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.Remark);
            cmdInsert.Parameters.Add("@TotalUnLoadingQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.TotalUnLoadingQty);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUnLoadingTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.UpdatedOn);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.UpdatedBy);
            cmdInsert.Parameters.Add("@statusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.StatusId);
            cmdInsert.Parameters.Add("@statusDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.StatusDate);
            cmdInsert.Parameters.Add("@status", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.Status);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblUnLoadingTO.IdUnLoading = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblUnLoading(TblUnLoadingTO tblUnLoadingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUnLoadingTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblUnLoading(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUnLoadingTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblUnLoading");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblUnLoadingTO tblUnLoadingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUnLoading] SET " +
            " [SupplierOrgId]= @SupplierOrgId" +
            " ,[descriptionId]= @DescriptionId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[totalUnLoadingQty]= @TotalUnLoadingQty" +
            " ,[refNo]= @RefNo" +
            " ,[vehicleNo]= @VehicleNo" +
            " ,[description]= @Description" +
            " ,[remark] = @Remark" +
             " ,[statusId] =@StatusId" +
            " ,[statusDate] =@StatusDate" +
             " ,[status] =@status" +
            " WHERE [IdUnLoading] = @IdUnLoading ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUnLoading", System.Data.SqlDbType.Int).Value = tblUnLoadingTO.IdUnLoading;
            cmdUpdate.Parameters.Add("@SupplierOrgId", System.Data.SqlDbType.Int).Value = tblUnLoadingTO.SupplierOrgId;
            cmdUpdate.Parameters.Add("@DescriptionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.DescriptionId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUnLoadingTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@TotalUnLoadingQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.TotalUnLoadingQty);
            cmdUpdate.Parameters.Add("@RefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.RefNo);
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.VehicleNo);
            cmdUpdate.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.Description);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.Remark);
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.StatusId);
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.StatusDate);
            cmdUpdate.Parameters.Add("@status", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingTO.Status);
            return cmdUpdate.ExecuteNonQuery();
        }

        // Vaibhav [14-Sep-2017] Update total unloading qty for perticular unloading transaction
        public int UpdateUnLoadingQty(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationQtyCommand(tblUnLoadingTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateUnLoadingQty");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationQtyCommand(TblUnLoadingTO tblUnLoadingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE tblUnLoading SET totalUnLoadingQty = @TotalUnLoadingQty WHERE IdUnLoading = @IdUnLoading ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUnLoading", System.Data.SqlDbType.Int).Value = tblUnLoadingTO.IdUnLoading;
            cmdUpdate.Parameters.Add("@TotalUnLoadingQty", System.Data.SqlDbType.Decimal).Value = Convert.ToDecimal(tblUnLoadingTO.TotalUnLoadingQty);
            return cmdUpdate.ExecuteNonQuery();
        }


        // Vaibhav [12-Oct-2017] added to deacivate unloading slip 
        public int DeactivateUnLoadingSlip(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteDeactivateUnLoadingSlipCommand(tblUnLoadingTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactivateUnLoadingSlip");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteDeactivateUnLoadingSlipCommand(TblUnLoadingTO tblUnLoadingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE tblUnLoading SET statusId = "+ (int)Constants.TranStatusE.UNLOADING_CANCELED +" WHERE IdUnLoading = @IdUnLoading ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUnLoading", System.Data.SqlDbType.Int).Value = tblUnLoadingTO.IdUnLoading;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblUnLoading(Int32 idUnLoading)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUnLoading, cmdDelete);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblUnLoading(Int32 idUnLoading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUnLoading, cmdDelete);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idUnLoading, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUnLoading] " +
            " WHERE idUnLoading = " + idUnLoading + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUnLoading", System.Data.SqlDbType.Int).Value = tblUnLoadingTO.IdUnLoading;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
