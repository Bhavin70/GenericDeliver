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
    public class TblTransportSlipDAO : ITblTransportSlipDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblTransportSlipDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblTransportSlip]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblTransportSlipTO> SelectAllTblTransportSlip(DateTime tDate, int isLink)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                string cond = "";
                if (tDate != null && isLink ==1)
                {
                    cond += "where loadingId IS NULL And CONVERT (DATE,createdOn,103) = @toDate ";
                }
                if(tDate != null &&  isLink == 2)
                {
                    cond += "where loadingId IS NOT NULL And CONVERT (DATE,createdOn,103) = @toDate ";
                }
            
                cmdSelect.CommandText = SqlSelectQuery() + cond;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = tDate.Date.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTransportSlipTO> tblTransportSlipTOList = ConvertDTToList(sqlReader);
                if(tblTransportSlipTOList != null)
                {
                    return tblTransportSlipTOList;
                }
                return null;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblSiteStatus");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblTransportSlipTO SelectTblTransportSlip(Int32 idTransportSlip)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTransportSlip = " + idTransportSlip + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTransportSlipTO> tblTransportSlipTOList = ConvertDTToList(sqlReader);
                if (tblTransportSlipTOList != null)
                {
                    return tblTransportSlipTOList[0];
                }
                return null;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblSiteStatus");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblTransportSlipTO> SelectAllTblTransportSlip(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTransportSlipTO> tblTransportSlipTOList = ConvertDTToList(sqlReader);
                if (tblTransportSlipTOList != null)
                {
                    return tblTransportSlipTOList;
                }
                return null;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblSiteStatus");
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public List<TblTransportSlipTO> ConvertDTToList(SqlDataReader tblTransportSlipTODT)
        {
            List<TblTransportSlipTO> tblTransportSlipTOList = new List<TblTransportSlipTO>();
            if (tblTransportSlipTODT != null)
            {
                while(tblTransportSlipTODT.Read())
                {
                    TblTransportSlipTO tblTransportSlipTONew = new TblTransportSlipTO();
                    if (tblTransportSlipTODT["idTransportSlip"] != DBNull.Value)
                        tblTransportSlipTONew.IdTransportSlip = Convert.ToInt32(tblTransportSlipTODT["idTransportSlip"].ToString());
                    if (tblTransportSlipTODT["partyOrgId"] != DBNull.Value)
                        tblTransportSlipTONew.PartyOrgId = Convert.ToInt32(tblTransportSlipTODT["partyOrgId"].ToString());
                    if (tblTransportSlipTODT["transporterOrgId"] != DBNull.Value)
                        tblTransportSlipTONew.TransporterOrgId = Convert.ToInt32(tblTransportSlipTODT["transporterOrgId"].ToString());
                    if (tblTransportSlipTODT["vehicleTypeId"] != DBNull.Value)
                        tblTransportSlipTONew.VehicleTypeId = Convert.ToInt32(tblTransportSlipTODT["vehicleTypeId"].ToString());
                    if (tblTransportSlipTODT["isFromDealer"] != DBNull.Value)
                        tblTransportSlipTONew.IsFromDealer = Convert.ToInt32(tblTransportSlipTODT["isFromDealer"].ToString());
                    if (tblTransportSlipTODT["createdBy"] != DBNull.Value)
                        tblTransportSlipTONew.CreatedBy = Convert.ToInt32(tblTransportSlipTODT["createdBy"].ToString());
                    if (tblTransportSlipTODT["updatedBy"] != DBNull.Value)
                        tblTransportSlipTONew.UpdatedBy = Convert.ToInt32(tblTransportSlipTODT["updatedBy"].ToString());
                    if (tblTransportSlipTODT["createdOn"] != DBNull.Value)
                        tblTransportSlipTONew.CreatedOn = Convert.ToDateTime(tblTransportSlipTODT["createdOn"].ToString());
                    if (tblTransportSlipTODT["updatedOn"] != DBNull.Value)
                        tblTransportSlipTONew.UpdatedOn = Convert.ToDateTime(tblTransportSlipTODT["updatedOn"].ToString());
                    if (tblTransportSlipTODT["partyName"] != DBNull.Value)
                        tblTransportSlipTONew.PartyName = Convert.ToString(tblTransportSlipTODT["partyName"].ToString());
                    if (tblTransportSlipTODT["transporterName"] != DBNull.Value)
                        tblTransportSlipTONew.TransporterName = Convert.ToString(tblTransportSlipTODT["transporterName"].ToString());
                    if (tblTransportSlipTODT["destination"] != DBNull.Value)
                        tblTransportSlipTONew.Destination = Convert.ToString(tblTransportSlipTODT["destination"].ToString());
                    if (tblTransportSlipTODT["vehicleNo"] != DBNull.Value)
                        tblTransportSlipTONew.VehicleNo = Convert.ToString(tblTransportSlipTODT["vehicleNo"].ToString());
                    if (tblTransportSlipTODT["driverName"] != DBNull.Value)
                        tblTransportSlipTONew.DriverName = Convert.ToString(tblTransportSlipTODT["driverName"].ToString());
                    if (tblTransportSlipTODT["contactNo"] != DBNull.Value)
                        tblTransportSlipTONew.ContactNo = Convert.ToString(tblTransportSlipTODT["contactNo"].ToString());
                    if (tblTransportSlipTODT["comments"] != DBNull.Value)
                        tblTransportSlipTONew.Comments = Convert.ToString(tblTransportSlipTODT["comments"].ToString());
                    if (tblTransportSlipTODT["refNo"] != DBNull.Value)
                        tblTransportSlipTONew.RefNo = Convert.ToString(tblTransportSlipTODT["refNo"].ToString());
                    if (tblTransportSlipTODT["loadingId"] != DBNull.Value)
                        tblTransportSlipTONew.LoadingId = Convert.ToInt32(tblTransportSlipTODT["loadingId"].ToString());

                    if (tblTransportSlipTODT["maxWeighingOty"] != DBNull.Value)
                        tblTransportSlipTONew.MaxWeighingOty = Convert.ToSingle(tblTransportSlipTODT["maxWeighingOty"].ToString());

                    tblTransportSlipTOList.Add(tblTransportSlipTONew);
                }
            }
            return tblTransportSlipTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblTransportSlip(TblTransportSlipTO tblTransportSlipTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblTransportSlipTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblTransportSlip");
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblTransportSlip(TblTransportSlipTO tblTransportSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblTransportSlipTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblTransportSlip");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblTransportSlipTO tblTransportSlipTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblTransportSlip]( " +
            //"  [idTransportSlip]" +
            " [partyOrgId]" +
            " ,[transporterOrgId]" +
            " ,[vehicleTypeId]" +
            " ,[isFromDealer]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[partyName]" +
            " ,[transporterName]" +
            " ,[destination]" +
            " ,[vehicleNo]" +
            " ,[driverName]" +
            " ,[contactNo]" +
            " ,[comments]" +
            " ,[refNo]" +
            " ,[loadingId]" +
            " ,[maxWeighingOty]" +

            " )" +
" VALUES (" +
            //"  @IdTransportSlip " +
            " @PartyOrgId " +
            " ,@TransporterOrgId " +
            " ,@VehicleTypeId " +
            " ,@IsFromDealer " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@PartyName " +
            " ,@TransporterName " +
            " ,@Destination " +
            " ,@VehicleNo " +
            " ,@DriverName " +
            " ,@ContactNo " +
            " ,@Comments " +
            " ,@RefNo " +
            " ,@loadingId " +
            " ,@MaxWeighingOty " +            
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdTransportSlip", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.IdTransportSlip;
            cmdInsert.Parameters.Add("@PartyOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.PartyOrgId);
            cmdInsert.Parameters.Add("@TransporterOrgId", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.TransporterOrgId;
            cmdInsert.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.VehicleTypeId);
            cmdInsert.Parameters.Add("@IsFromDealer", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.IsFromDealer;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblTransportSlipTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.UpdatedOn);
            cmdInsert.Parameters.Add("@PartyName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.PartyName);
            cmdInsert.Parameters.Add("@TransporterName", System.Data.SqlDbType.NVarChar).Value = tblTransportSlipTO.TransporterName;
            cmdInsert.Parameters.Add("@Destination", System.Data.SqlDbType.NVarChar).Value = tblTransportSlipTO.Destination;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblTransportSlipTO.VehicleNo;
            cmdInsert.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = tblTransportSlipTO.DriverName;
            cmdInsert.Parameters.Add("@ContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.ContactNo);
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.Comments); 
            cmdInsert.Parameters.Add("@RefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.RefNo);
            cmdInsert.Parameters.Add("@loadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.LoadingId);
            cmdInsert.Parameters.Add("@MaxWeighingOty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.MaxWeighingOty);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblTransportSlipTO.IdTransportSlip = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
            {
                return -1;
            }
            //return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblTransportSlip(TblTransportSlipTO tblTransportSlipTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblTransportSlipTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblTransportSlip");
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblTransportSlip(TblTransportSlipTO tblTransportSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblTransportSlipTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblTransportSlip");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblTransportSlipTO tblTransportSlipTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblTransportSlip] SET " +
           // "  [idTransportSlip] = @IdTransportSlip" +
            "[partyOrgId]= @PartyOrgId" +
            " ,[transporterOrgId]= @TransporterOrgId" +
            " ,[vehicleTypeId]= @VehicleTypeId" +
            " ,[isFromDealer]= @IsFromDealer" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[partyName]= @PartyName" +
            " ,[transporterName]= @TransporterName" +
            " ,[destination]= @Destination" +
            " ,[vehicleNo]= @VehicleNo" +
            " ,[driverName]= @DriverName" +
            " ,[contactNo]= @ContactNo" +
            " ,[comments]= @Comments" +
            " ,[refNo] = @RefNo" +
            " ,[loadingId] = @loadingId" +
            " ,[maxWeighingOty] = @MaxWeighingOty" +
            " WHERE [IdTransportSlip] = @IdTransportSlip";


            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@PartyOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.PartyOrgId);

           // cmdUpdate.Parameters.Add("@PartyOrgId", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.PartyOrgId;
            cmdUpdate.Parameters.Add("@TransporterOrgId", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.TransporterOrgId;
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.VehicleTypeId);
            cmdUpdate.Parameters.Add("@IsFromDealer", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.IsFromDealer;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblTransportSlipTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblTransportSlipTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@PartyName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.PartyName);

            //cmdUpdate.Parameters.Add("@PartyName", System.Data.SqlDbType.NVarChar).Value = tblTransportSlipTO.PartyName;
            cmdUpdate.Parameters.Add("@TransporterName", System.Data.SqlDbType.NVarChar).Value = tblTransportSlipTO.TransporterName;
            cmdUpdate.Parameters.Add("@Destination", System.Data.SqlDbType.NVarChar).Value = tblTransportSlipTO.Destination;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblTransportSlipTO.VehicleNo;
            cmdUpdate.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = tblTransportSlipTO.DriverName;
            cmdUpdate.Parameters.Add("@ContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.ContactNo);
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.Comments); 
            cmdUpdate.Parameters.Add("@RefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.RefNo); 
            cmdUpdate.Parameters.Add("@IdTransportSlip", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.IdTransportSlip;
            cmdUpdate.Parameters.Add("@loadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.LoadingId);
            cmdUpdate.Parameters.Add("@MaxWeighingOty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransportSlipTO.MaxWeighingOty);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblTransportSlip(Int32 idTransportSlip)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTransportSlip, cmdDelete);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTblTransportSlip");
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblTransportSlip(Int32 idTransportSlip, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTransportSlip, cmdDelete);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTblTransportSlip");
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idTransportSlip, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblTransportSlip] " +
            " WHERE idTransportSlip = " + idTransportSlip + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTransportSlip", System.Data.SqlDbType.Int).Value = tblTransportSlipTO.IdTransportSlip;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
