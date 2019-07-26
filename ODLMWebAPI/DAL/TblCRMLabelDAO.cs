using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.DAL
{
    public class TblCRMLabelDAO : ITblCRMLabelDAO
    {
        IConnectionString _iConnectionString;
        public TblCRMLabelDAO(IConnectionString iConnetionString)
        {
            _iConnectionString = iConnetionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblCRMLabel] where isActive = 1 "; 
            return sqlSelectQry;
        }
        #endregion


        #region Selection
        public  List<TblCRMLabelTO> SelectAllTblCRMLabelList(int pageId,int langId)
        {

            String whereLangCluase = " AND lagId= "+langId;
            String wherepageCluase = " AND pageId= " + pageId;

              
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                if (pageId > 0)
                    cmdSelect.CommandText = cmdSelect.CommandText + wherepageCluase;
                if (langId > 0)
                    cmdSelect.CommandText = cmdSelect.CommandText + whereLangCluase;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
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

        public TblCRMLabelTO SelectTblCRMLabel(Int32 idLabel)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idLabel = " + idLabel +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLabel", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.IdLabel;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCRMLabelTO> list= ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;
            

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

      
        public  List<TblCRMLabelTO> ConvertDTToList(SqlDataReader tblCRMLabelTODT)
        {
            List<TblCRMLabelTO> tblCRMLabelTOList = new List<TblCRMLabelTO>();
            if (tblCRMLabelTODT != null)
            {
                while( tblCRMLabelTODT.Read())
                {
                    TblCRMLabelTO tblCRMLabelTONew = new TblCRMLabelTO();
                    if (tblCRMLabelTODT["idLabel"] != DBNull.Value)
                        tblCRMLabelTONew.IdLabel = Convert.ToInt32(tblCRMLabelTODT["idLabel"].ToString());
                    if (tblCRMLabelTODT["lagId"] != DBNull.Value)
                        tblCRMLabelTONew.LagId = Convert.ToInt32(tblCRMLabelTODT["lagId"].ToString());
                    if (tblCRMLabelTODT["isActive"] != DBNull.Value)
                        tblCRMLabelTONew.IsActive = Convert.ToInt32(tblCRMLabelTODT["isActive"].ToString());
                    if (tblCRMLabelTODT["createdBy"] != DBNull.Value)
                        tblCRMLabelTONew.CreatedBy = Convert.ToInt32(tblCRMLabelTODT["createdBy"].ToString());
                    if (tblCRMLabelTODT["updatedBy"] != DBNull.Value)
                        tblCRMLabelTONew.UpdatedBy = Convert.ToInt32(tblCRMLabelTODT["updatedBy"].ToString());
                    if (tblCRMLabelTODT["pageId"] != DBNull.Value)
                        tblCRMLabelTONew.PageId = Convert.ToInt32(tblCRMLabelTODT["pageId"].ToString());
                    if (tblCRMLabelTODT["createdOn"] != DBNull.Value)
                        tblCRMLabelTONew.CreatedOn = Convert.ToDateTime(tblCRMLabelTODT["createdOn"].ToString());
                    if (tblCRMLabelTODT["updatedOn"] != DBNull.Value)
                        tblCRMLabelTONew.UpdatedOn = Convert.ToDateTime(tblCRMLabelTODT["updatedOn"].ToString());
                    if (tblCRMLabelTODT["keyLabel"] != DBNull.Value)
                        tblCRMLabelTONew.KeyLabel = Convert.ToString(tblCRMLabelTODT["keyLabel"].ToString());
                    if (tblCRMLabelTODT["valueLabel"] != DBNull.Value)
                        tblCRMLabelTONew.ValueLabel = Convert.ToString(tblCRMLabelTODT["valueLabel"].ToString());
                    tblCRMLabelTOList.Add(tblCRMLabelTONew);
                }
            }
            return tblCRMLabelTOList;
        }

        #endregion

        //        #region Insertion
        //        public static int InsertTblCRMLabel(TblCRMLabelTO tblCRMLabelTO)
        //        {
        //            String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
        //            SqlConnection conn = new SqlConnection(sqlConnStr);
        //            SqlCommand cmdInsert = new SqlCommand();
        //            try
        //            {
        //                conn.Open();
        //                cmdInsert.Connection = conn;
        //                return ExecuteInsertionCommand(tblCRMLabelTO, cmdInsert);
        //            }
        //            catch(Exception ex)
        //            {
        //                String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //                String userName = System.Windows.Forms.SystemInformation.UserName;
        //                return 0;
        //            }
        //            finally
        //            {
        //                conn.Close();
        //                cmdInsert.Dispose();
        //            }
        //        }

        //        public static int InsertTblCRMLabel(TblCRMLabelTO tblCRMLabelTO, SqlConnection conn, SqlTransaction tran)
        //        {
        //            SqlCommand cmdInsert = new SqlCommand();
        //            try
        //            {
        //                cmdInsert.Connection = conn;
        //                cmdInsert.Transaction = tran;
        //                return ExecuteInsertionCommand(tblCRMLabelTO, cmdInsert);
        //            }
        //            catch(Exception ex)
        //            {
        //                String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //                String userName = System.Windows.Forms.SystemInformation.UserName;
        //                return 0;
        //            }
        //            finally
        //            {
        //                cmdInsert.Dispose();
        //            }
        //        }

        //        public static int ExecuteInsertionCommand(TblCRMLabelTO tblCRMLabelTO, SqlCommand cmdInsert)
        //        {
        //            String sqlQuery = @" INSERT INTO [tblCRMLabel]( " + 
        //            "  [idLabel]" +
        //            " ,[lagId]" +
        //            " ,[isActive]" +
        //            " ,[createdBy]" +
        //            " ,[updatedBy]" +
        //            " ,[pageId]" +
        //            " ,[createdOn]" +
        //            " ,[updatedOn]" +
        //            " ,[keyLabel]" +
        //            " ,[valueLabel]" +
        //            " )" +
        //" VALUES (" +
        //            "  @IdLabel " +
        //            " ,@LagId " +
        //            " ,@IsActive " +
        //            " ,@CreatedBy " +
        //            " ,@UpdatedBy " +
        //            " ,@PageId " +
        //            " ,@CreatedOn " +
        //            " ,@UpdatedOn " +
        //            " ,@KeyLabel " +
        //            " ,@ValueLabel " + 
        //            " )";
        //            cmdInsert.CommandText = sqlQuery;
        //            cmdInsert.CommandType = System.Data.CommandType.Text;

        //            cmdInsert.Parameters.Add("@IdLabel", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.IdLabel;
        //            cmdInsert.Parameters.Add("@LagId", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.LagId;
        //            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.IsActive;
        //            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.CreatedBy;
        //            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.UpdatedBy;
        //            cmdInsert.Parameters.Add("@PageId", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.PageId;
        //            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblCRMLabelTO.CreatedOn;
        //            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblCRMLabelTO.UpdatedOn;
        //            cmdInsert.Parameters.Add("@KeyLabel", System.Data.SqlDbType.NVarChar).Value = tblCRMLabelTO.KeyLabel;
        //            cmdInsert.Parameters.Add("@ValueLabel", System.Data.SqlDbType.NVarChar).Value = tblCRMLabelTO.ValueLabel;
        //            return cmdInsert.ExecuteNonQuery();
        //        }
        //        #endregion

        //        #region Updation
        //        public static int UpdateTblCRMLabel(TblCRMLabelTO tblCRMLabelTO)
        //        {
        //            String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
        //            SqlConnection conn = new SqlConnection(sqlConnStr);
        //            SqlCommand cmdUpdate = new SqlCommand();
        //            try
        //            {
        //                conn.Open();
        //                cmdUpdate.Connection = conn;
        //                return ExecuteUpdationCommand(tblCRMLabelTO, cmdUpdate);
        //            }
        //            catch(Exception ex)
        //            {
        //                String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //                String userName = System.Windows.Forms.SystemInformation.UserName;
        //                return 0;
        //            }
        //            finally
        //            {
        //                conn.Close();
        //                cmdUpdate.Dispose();
        //            }
        //        }

        //        public static int UpdateTblCRMLabel(TblCRMLabelTO tblCRMLabelTO, SqlConnection conn, SqlTransaction tran)
        //        {
        //            SqlCommand cmdUpdate = new SqlCommand();
        //            try
        //            {
        //                cmdUpdate.Connection = conn;
        //                cmdUpdate.Transaction = tran;
        //                return ExecuteUpdationCommand(tblCRMLabelTO, cmdUpdate);
        //            }
        //            catch(Exception ex)
        //            {
        //                String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //                String userName = System.Windows.Forms.SystemInformation.UserName;
        //                return 0;
        //            }
        //            finally
        //            {
        //                cmdUpdate.Dispose();
        //            }
        //        }

        //        public static int ExecuteUpdationCommand(TblCRMLabelTO tblCRMLabelTO, SqlCommand cmdUpdate)
        //        {
        //            String sqlQuery = @" UPDATE [tblCRMLabel] SET " + 
        //            "  [idLabel] = @IdLabel" +
        //            " ,[lagId]= @LagId" +
        //            " ,[isActive]= @IsActive" +
        //            " ,[createdBy]= @CreatedBy" +
        //            " ,[updatedBy]= @UpdatedBy" +
        //            " ,[pageId]= @PageId" +
        //            " ,[createdOn]= @CreatedOn" +
        //            " ,[updatedOn]= @UpdatedOn" +
        //            " ,[keyLabel]= @KeyLabel" +
        //            " ,[valueLabel] = @ValueLabel" +
        //            " WHERE 1 = 2 "; 

        //            cmdUpdate.CommandText = sqlQuery;
        //            cmdUpdate.CommandType = System.Data.CommandType.Text;

        //            cmdUpdate.Parameters.Add("@IdLabel", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.IdLabel;
        //            cmdUpdate.Parameters.Add("@LagId", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.LagId;
        //            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.IsActive;
        //            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.CreatedBy;
        //            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.UpdatedBy;
        //            cmdUpdate.Parameters.Add("@PageId", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.PageId;
        //            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblCRMLabelTO.CreatedOn;
        //            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblCRMLabelTO.UpdatedOn;
        //            cmdUpdate.Parameters.Add("@KeyLabel", System.Data.SqlDbType.NVarChar).Value = tblCRMLabelTO.KeyLabel;
        //            cmdUpdate.Parameters.Add("@ValueLabel", System.Data.SqlDbType.NVarChar).Value = tblCRMLabelTO.ValueLabel;
        //            return cmdUpdate.ExecuteNonQuery();
        //        }
        //        #endregion

        //        #region Deletion
        //        public static int DeleteTblCRMLabel(Int32 idLabel)
        //        {
        //            String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
        //            SqlConnection conn = new SqlConnection(sqlConnStr);
        //            SqlCommand cmdDelete = new SqlCommand();
        //            try
        //            {
        //                conn.Open();
        //                cmdDelete.Connection = conn;
        //                return ExecuteDeletionCommand(idLabel, cmdDelete);
        //            }
        //            catch(Exception ex)
        //            {
        //                String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //                String userName = System.Windows.Forms.SystemInformation.UserName;
        //                return 0;
        //            }
        //            finally
        //            {
        //                conn.Close();
        //                cmdDelete.Dispose();
        //            }
        //        }

        //        public static int DeleteTblCRMLabel(Int32 idLabel, SqlConnection conn, SqlTransaction tran)
        //        {
        //            SqlCommand cmdDelete = new SqlCommand();
        //            try
        //            {
        //                cmdDelete.Connection = conn;
        //                cmdDelete.Transaction = tran;
        //                return ExecuteDeletionCommand(idLabel, cmdDelete);
        //            }
        //            catch(Exception ex)
        //            {
        //                String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //                String userName = System.Windows.Forms.SystemInformation.UserName;
        //                return 0;
        //            }
        //            finally
        //            {
        //                cmdDelete.Dispose();
        //            }
        //        }

        //        public static int ExecuteDeletionCommand(Int32 idLabel, SqlCommand cmdDelete)
        //        {
        //            cmdDelete.CommandText = "DELETE FROM [tblCRMLabel] " +
        //            " WHERE idLabel = " + idLabel +"";
        //            cmdDelete.CommandType = System.Data.CommandType.Text;

        //            //cmdDelete.Parameters.Add("@idLabel", System.Data.SqlDbType.Int).Value = tblCRMLabelTO.IdLabel;
        //            return cmdDelete.ExecuteNonQuery();
        //        }
        //        #endregion

    }
}
