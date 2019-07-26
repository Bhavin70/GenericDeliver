using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimSmsConfigDAO : IDimSmsConfigDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimSmsConfigDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimSmsConfig]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public DimSmsConfigTO SelectAllDimSmsConfig()
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

                SqlDataReader da = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimSmsConfigTO> dimSmsConfigTOList = ConvertDTToList(da);
                if(dimSmsConfigTOList != null && dimSmsConfigTOList.Count > 0)
                {
                    return dimSmsConfigTOList[0];
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

        //public DimSmsConfigTO SelectDimSmsConfig()
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = SqlSelectQuery() + "  ";
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        //public DataTable SelectAllDimSmsConfig(SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {
        //        cmdSelect.CommandText = SqlSelectQuery();
        //        cmdSelect.Connection = conn;
        //        cmdSelect.Transaction = tran;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return null;
        //    }
        //    finally
        //    {
        //        cmdSelect.Dispose();
        //    }
        //}

        #endregion

        #region Insertion
        //public int InsertDimSmsConfig(DimSmsConfigTO dimSmsConfigTO)
        //{
        //    String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdInsert = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdInsert.Connection = conn;
        //        return ExecuteInsertionCommand(dimSmsConfigTO, cmdInsert);
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return 0;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdInsert.Dispose();
        //    }
        //}

        //public int InsertDimSmsConfig(DimSmsConfigTO dimSmsConfigTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdInsert = new SqlCommand();
        //    try
        //    {
        //        cmdInsert.Connection = conn;
        //        cmdInsert.Transaction = tran;
        //        return ExecuteInsertionCommand(dimSmsConfigTO, cmdInsert);
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return 0;
        //    }
        //    finally
        //    {
        //        cmdInsert.Dispose();
        //    }
        //}

//        public int ExecuteInsertionCommand(DimSmsConfigTO dimSmsConfigTO, SqlCommand cmdInsert)
//        {
//            String sqlQuery = @" INSERT INTO [dimSmsConfig]( " +
//            "  [idSmsConfig]" +
//            " ,[isActive]" +
//            " ,[smsConfigUrl]" +
//            " ,[brand]" +
//            " )" +
//" VALUES (" +
//            "  @IdSmsConfig " +
//            " ,@IsActive " +
//            " ,@SmsConfigUrl " +
//            " ,@Brand " +
//            " )";
//            cmdInsert.CommandText = sqlQuery;
//            cmdInsert.CommandType = System.Data.CommandType.Text;

//            cmdInsert.Parameters.Add("@IdSmsConfig", System.Data.SqlDbType.Int).Value = dimSmsConfigTO.IdSmsConfig;
//            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimSmsConfigTO.IsActive;
//            cmdInsert.Parameters.Add("@SmsConfigUrl", System.Data.SqlDbType.VarChar).Value = dimSmsConfigTO.SmsConfigUrl;
//            cmdInsert.Parameters.Add("@Brand", System.Data.SqlDbType.VarChar).Value = dimSmsConfigTO.Brand;
//            return cmdInsert.ExecuteNonQuery();
//        }
        #endregion

        #region Updation
        //public int UpdateDimSmsConfig(DimSmsConfigTO dimSmsConfigTO)
        //{
        //    String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdUpdate = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdUpdate.Connection = conn;
        //        return ExecuteUpdationCommand(dimSmsConfigTO, cmdUpdate);
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return 0;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdUpdate.Dispose();
        //    }
        //}

        //public int UpdateDimSmsConfig(DimSmsConfigTO dimSmsConfigTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdUpdate = new SqlCommand();
        //    try
        //    {
        //        cmdUpdate.Connection = conn;
        //        cmdUpdate.Transaction = tran;
        //        return ExecuteUpdationCommand(dimSmsConfigTO, cmdUpdate);
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return 0;
        //    }
        //    finally
        //    {
        //        cmdUpdate.Dispose();
        //    }
        //}

        //public int ExecuteUpdationCommand(DimSmsConfigTO dimSmsConfigTO, SqlCommand cmdUpdate)
        //{
        //    String sqlQuery = @" UPDATE [dimSmsConfig] SET " +
        //    "  [idSmsConfig] = @IdSmsConfig" +
        //    " ,[isActive]= @IsActive" +
        //    " ,[smsConfigUrl]= @SmsConfigUrl" +
        //    " ,[brand] = @Brand" +
        //    " WHERE 1 = 2 ";

        //    cmdUpdate.CommandText = sqlQuery;
        //    cmdUpdate.CommandType = System.Data.CommandType.Text;

        //    cmdUpdate.Parameters.Add("@IdSmsConfig", System.Data.SqlDbType.Int).Value = dimSmsConfigTO.IdSmsConfig;
        //    cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimSmsConfigTO.IsActive;
        //    cmdUpdate.Parameters.Add("@SmsConfigUrl", System.Data.SqlDbType.VarChar).Value = dimSmsConfigTO.SmsConfigUrl;
        //    cmdUpdate.Parameters.Add("@Brand", System.Data.SqlDbType.VarChar).Value = dimSmsConfigTO.Brand;
        //    return cmdUpdate.ExecuteNonQuery();
        //}
        #endregion

        #region Deletion
        //public int DeleteDimSmsConfig()
        //{
        //    String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdDelete = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdDelete.Connection = conn;
        //        return ExecuteDeletionCommand(, cmdDelete);
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return 0;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdDelete.Dispose();
        //    }
        //}

        //public int DeleteDimSmsConfig(, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdDelete = new SqlCommand();
        //    try
        //    {
        //        cmdDelete.Connection = conn;
        //        cmdDelete.Transaction = tran;
        //        return ExecuteDeletionCommand(, cmdDelete);
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return 0;
        //    }
        //    finally
        //    {
        //        cmdDelete.Dispose();
        //    }
        //}

        //public int ExecuteDeletionCommand(, SqlCommand cmdDelete)
        //{
        //    cmdDelete.CommandText = "DELETE FROM [dimSmsConfig] " +
        //    " ";
        //    cmdDelete.CommandType = System.Data.CommandType.Text;

        //    return cmdDelete.ExecuteNonQuery();
        //}
        #endregion


        public List<DimSmsConfigTO> ConvertDTToList(SqlDataReader dimSmsConfigTODT)
        {
            List<DimSmsConfigTO> dimSmsConfigTOList = new List<DimSmsConfigTO>();
            if (dimSmsConfigTODT != null)
            {
                while(dimSmsConfigTODT.Read())
                {
                    DimSmsConfigTO dimSmsConfigTONew = new DimSmsConfigTO();
                    if (dimSmsConfigTODT["idSmsConfig"] != DBNull.Value)
                        dimSmsConfigTONew.IdSmsConfig = Convert.ToInt32(dimSmsConfigTODT["idSmsConfig"].ToString());
                    if (dimSmsConfigTODT["isActive"] != DBNull.Value)
                        dimSmsConfigTONew.IsActive = Convert.ToInt32(dimSmsConfigTODT["isActive"].ToString());
                    if (dimSmsConfigTODT["smsConfigUrl"] != DBNull.Value)
                        dimSmsConfigTONew.SmsConfigUrl = Convert.ToString(dimSmsConfigTODT["smsConfigUrl"].ToString());
                    if (dimSmsConfigTODT["brand"] != DBNull.Value)
                        dimSmsConfigTONew.Brand = Convert.ToString(dimSmsConfigTODT["brand"].ToString());
                    if (dimSmsConfigTODT["isFilter"] != DBNull.Value)
                        dimSmsConfigTONew.IsFilter = Convert.ToInt32(dimSmsConfigTODT["isFilter"].ToString());
                    dimSmsConfigTOList.Add(dimSmsConfigTONew);
                }
            }
            return dimSmsConfigTOList;
        }

    }
}
