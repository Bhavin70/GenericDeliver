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
    public class TblStockSummaryDAO : ITblStockSummaryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblStockSummaryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblStockSummary]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblStockSummaryTO> SelectAllTblStockSummary()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockSummaryTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
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

        public String SelectLastStockUpdatedDateTime(Int32 compartmentId, Int32 prodCatId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT MAX(createdOn) createdDate , MAX(updatedOn) updatedDate FROM tblStockDetails WHERE locationId=" + compartmentId + " AND prodCatId=" + prodCatId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DateTime createdDate = new DateTime();
                DateTime updatedDate = new DateTime();
                while (sqlReader.Read())
                {
                    if (sqlReader["createdDate"] != DBNull.Value)
                        createdDate = Convert.ToDateTime(sqlReader["createdDate"].ToString());
                    if (sqlReader["updatedDate"] != DBNull.Value)
                        updatedDate = Convert.ToDateTime(sqlReader["updatedDate"].ToString());
                }

                if (createdDate == DateTime.MinValue && updatedDate == DateTime.MinValue)
                    return "";
                else if (createdDate > updatedDate)
                    return createdDate.ToString(Constants.DefaultDateFormat);
                else
                    return updatedDate.ToString(Constants.DefaultDateFormat);
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                if (sqlReader != null) sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblStockSummaryTO SelectTblStockSummary(Int32 idStockSummary,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idStockSummary = " + idStockSummary +" ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockSummaryTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
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

        public TblStockSummaryTO SelectTblStockSummary(DateTime stocDate, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            Boolean isConnection = false;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                    isConnection = true;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                if (stocDate != new DateTime())
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE stockDate= @stockDate and transactionType=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery();

                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                if (stocDate != new DateTime())
                    cmdSelect.Parameters.Add("@stockDate", System.Data.SqlDbType.DateTime).Value = stocDate.Date;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockSummaryTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

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
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
                if (!isConnection)
                    conn.Close();
            }
        }
        public TblStockSummaryTO SelectTblProdStockSummary(DateTime stocDate, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            Boolean isConnection = false;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                    isConnection = true;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                cmdSelect.CommandText = "select * from tblStockSummary where  transactionType=2 and stockDate= @stockDate";//SqlSelectQuery() + " WHERE stockDate= @stockDate";


                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                if (stocDate != new DateTime())
                    cmdSelect.Parameters.Add("@stockDate", System.Data.SqlDbType.DateTime).Value = stocDate.Date;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockSummaryTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

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
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
                if (!isConnection)
                    conn.Close();
            }
        }
        public TblStockSummaryTO SelectTblProdStockSummaryExist(DateTime stocDate, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            Boolean isConnection = false;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                    isConnection = true;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                cmdSelect.CommandText = "select * from tblStockSummary where  transactionType=2 and stockDate= @stockDate";//SqlSelectQuery() + " WHERE stockDate= @stockDate";


                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                if (stocDate != new DateTime())
                    cmdSelect.Parameters.Add("@stockDate", System.Data.SqlDbType.DateTime).Value = stocDate.Date;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockSummaryTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

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
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
                if (!isConnection)
                    conn.Close();
            }
        }
        public TblStockSummaryTO SelectTblStockSummaryExist(DateTime stocDate, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            Boolean isConnection = false;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                    isConnection = true;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                cmdSelect.CommandText = "select top 1 * from tblStockSummary where  transactionType=1 and stockDate= @stockDate";


                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                if (stocDate != new DateTime())
                    cmdSelect.Parameters.Add("@stockDate", System.Data.SqlDbType.DateTime).Value = stocDate.Date;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockSummaryTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

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
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
                if (!isConnection)
                    conn.Close();
            }
        }

        public ODLMWebAPI.DashboardModels.StockUpdateInfo SelectDashboardStockUpdateInfo(DateTime sysDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblLoadingTODT = null;
            try
            {
                conn.Open();

                //Saket [2018-02-08] Commented and added new queery.
                //cmdSelect.CommandText = " SELECT * FROM  " +
                //                        " ( " +
                //                        " SELECT totalStock, createdOn FROM tblStockSummary " +
                //                        " WHERE DAY(createdOn) = "+sysDate.Day+" AND MONTH(createdOn) = "+sysDate.Month + " AND YEAR(createdOn) = "+sysDate.Year +
                //                        " ) AS stockInfo " +
                //                        " LEFT JOIN " +
                //                        " ( " +
                //                        " SELECT stockInMT, stockFactor , createdOn FROM tblStockAsPerBooks " +
                //                        " WHERE DAY(createdOn) = "+sysDate.Day+" AND MONTH(createdOn) = "+sysDate.Month + " AND YEAR(createdOn) = "+sysDate.Year +
                //                        " ) As bookStock " +
                //                        " ON CAST(stockInfo.createdOn as date) = CAST(bookStock.createdOn as date)";


                cmdSelect.CommandText = " SELECT * FROM(SELECT totalStock, createdOn ,idStockSummary FROM tblStockSummary " +
                                        " ) AS stockInfo " +
                                        " LEFT JOIN(SELECT stockInMT, stockFactor , createdOn FROM tblStockAsPerBooks " +
                                        " ) As bookStock " +
                                        " ON CAST(stockInfo.createdOn as date) = CAST(bookStock.createdOn as date) " +
                                        " left join (select sum(balanceStock ) as PendingStock,stockSummaryId from tblStockDetails group by stockSummaryId  ) as PendingStock " +
                                        "   on PendingStock.stockSummaryId=stockInfo.idStockSummary  ";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblLoadingTODT.Read())
                {
                    ODLMWebAPI.DashboardModels.StockUpdateInfo stockUpdateInfoNew = new ODLMWebAPI.DashboardModels.StockUpdateInfo();
                    if (tblLoadingTODT["totalStock"] != DBNull.Value)
                        stockUpdateInfoNew.TotalSysStock = Convert.ToDouble(tblLoadingTODT["totalStock"].ToString());
                    if (tblLoadingTODT["stockFactor"] != DBNull.Value)
                        stockUpdateInfoNew.StockFactor = Convert.ToDouble(tblLoadingTODT["stockFactor"].ToString());
                    if (tblLoadingTODT["stockInMT"] != DBNull.Value)
                        stockUpdateInfoNew.TotalBooksStock = Convert.ToDouble(tblLoadingTODT["stockInMT"].ToString());
                    if (tblLoadingTODT["PendingStock"] != DBNull.Value)
                        stockUpdateInfoNew.TodaysPendingStock = Convert.ToDouble(tblLoadingTODT["PendingStock"].ToString());
                    return stockUpdateInfoNew;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblLoadingTODT != null)
                    tblLoadingTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblStockSummaryTO> ConvertDTToList(SqlDataReader tblStockSummaryTODT)
        {
            List<TblStockSummaryTO> tblStockSummaryTOList = new List<TblStockSummaryTO>();
            if (tblStockSummaryTODT != null)
            {
                while (tblStockSummaryTODT.Read())
                {
                    TblStockSummaryTO tblStockSummaryTONew = new TblStockSummaryTO();
                    if (tblStockSummaryTODT["idStockSummary"] != DBNull.Value)
                        tblStockSummaryTONew.IdStockSummary = Convert.ToInt32(tblStockSummaryTODT["idStockSummary"].ToString());
                    if (tblStockSummaryTODT["confirmedBy"] != DBNull.Value)
                        tblStockSummaryTONew.ConfirmedBy = Convert.ToInt32(tblStockSummaryTODT["confirmedBy"].ToString());
                    if (tblStockSummaryTODT["createdBy"] != DBNull.Value)
                        tblStockSummaryTONew.CreatedBy = Convert.ToInt32(tblStockSummaryTODT["createdBy"].ToString());
                    if (tblStockSummaryTODT["updatedBy"] != DBNull.Value)
                        tblStockSummaryTONew.UpdatedBy = Convert.ToInt32(tblStockSummaryTODT["updatedBy"].ToString());
                    if (tblStockSummaryTODT["stockDate"] != DBNull.Value)
                        tblStockSummaryTONew.StockDate = Convert.ToDateTime(tblStockSummaryTODT["stockDate"].ToString());
                    if (tblStockSummaryTODT["confirmedOn"] != DBNull.Value)
                        tblStockSummaryTONew.ConfirmedOn = Convert.ToDateTime(tblStockSummaryTODT["confirmedOn"].ToString());
                    if (tblStockSummaryTODT["createdOn"] != DBNull.Value)
                        tblStockSummaryTONew.CreatedOn = Convert.ToDateTime(tblStockSummaryTODT["createdOn"].ToString());
                    if (tblStockSummaryTODT["updatedOn"] != DBNull.Value)
                        tblStockSummaryTONew.UpdatedOn = Convert.ToDateTime(tblStockSummaryTODT["updatedOn"].ToString());
                    if (tblStockSummaryTODT["noOfBundles"] != DBNull.Value)
                        tblStockSummaryTONew.NoOfBundles = Convert.ToDouble(tblStockSummaryTODT["noOfBundles"].ToString());
                    if (tblStockSummaryTODT["totalStock"] != DBNull.Value)
                        tblStockSummaryTONew.TotalStock = Convert.ToDouble(tblStockSummaryTODT["totalStock"].ToString());
                    tblStockSummaryTOList.Add(tblStockSummaryTONew);
                }
            }
            return tblStockSummaryTOList;
        }
        //Aniket
        public StockSummaryTO GetLastStockSummaryDetails()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader stocksummaryTO = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select stock.updatedOn, stock.updatedBy, u.userDisplayName from tblStockSummary stock "+
                                        " LEFT JOIN tblUser u on u.idUser = stock.updatedBy ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                stocksummaryTO = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if(stocksummaryTO.HasRows)
                {
                    if(stocksummaryTO.Read())
                    {
                        StockSummaryTO stocksummary = new StockSummaryTO();
                        if (stocksummaryTO["updatedOn"] != DBNull.Value)
                            stocksummary.UpdatedOn = Convert.ToDateTime(stocksummaryTO["updatedOn"]);
                        if (stocksummaryTO["updatedBy"] != DBNull.Value)
                            stocksummary.UpdatedBy = Convert.ToInt32(stocksummaryTO["updatedBy"]);
                        if (stocksummaryTO["userDisplayName"] != DBNull.Value)
                            stocksummary.UserDisplayName = Convert.ToString(stocksummaryTO["userDisplayName"]);

                        return stocksummary;

                    }
                    else
                    {
                       return null;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Insertion
        public int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblStockSummaryTO, cmdInsert);
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

        public int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblStockSummaryTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblStockSummaryTO tblStockSummaryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblStockSummary]( " + 
                                "  [confirmedBy]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[stockDate]" +
                                " ,[confirmedOn]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[noOfBundles]" +
                                " ,[totalStock]" +
                                " ,[transactionType]" +
                                " )" +
                    " VALUES (" +
                                "  @ConfirmedBy " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@StockDate " +
                                " ,@ConfirmedOn " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@NoOfBundles " +
                                " ,@TotalStock " +
                                " ,@TransactionType " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdStockSummary", System.Data.SqlDbType.Int).Value = tblStockSummaryTO.IdStockSummary;
            cmdInsert.Parameters.Add("@ConfirmedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockSummaryTO.ConfirmedBy);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStockSummaryTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockSummaryTO.UpdatedBy);
            cmdInsert.Parameters.Add("@StockDate", System.Data.SqlDbType.DateTime).Value = tblStockSummaryTO.StockDate;
            cmdInsert.Parameters.Add("@ConfirmedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockSummaryTO.ConfirmedOn);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStockSummaryTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockSummaryTO.UpdatedOn);
            cmdInsert.Parameters.Add("@NoOfBundles", System.Data.SqlDbType.NVarChar).Value = tblStockSummaryTO.NoOfBundles;
            cmdInsert.Parameters.Add("@TotalStock", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockSummaryTO.TotalStock);
            cmdInsert.Parameters.Add("@TransactionType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockSummaryTO.TransactionType);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                //cmdInsert.CommandText = Constants.IdentityColumnQuery;
                //tblStockSummaryTO.IdStockSummary = Convert.ToInt32(cmdInsert.ExecuteScalar());
                cmdInsert.CommandText = Constants.SQL_SELECT_IDENTITY_QUERY;
                tblStockSummaryTO.IdStockSummary = Convert.ToInt64(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblStockSummaryTO, cmdUpdate);
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

        public int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblStockSummaryTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblStockSummaryTO tblStockSummaryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStockSummary] SET " + 
                            "  [confirmedBy]= @ConfirmedBy" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[stockDate]= @StockDate" +
                            " ,[confirmedOn]= @ConfirmedOn" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[noOfBundles]= @NoOfBundles" +
                            " ,[totalStock] = @TotalStock" +
                            " WHERE  [idStockSummary] = @IdStockSummary"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStockSummary", System.Data.SqlDbType.Int).Value = tblStockSummaryTO.IdStockSummary;
            cmdUpdate.Parameters.Add("@ConfirmedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockSummaryTO.ConfirmedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblStockSummaryTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@StockDate", System.Data.SqlDbType.DateTime).Value = tblStockSummaryTO.StockDate;
            cmdUpdate.Parameters.Add("@ConfirmedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockSummaryTO.ConfirmedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblStockSummaryTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@NoOfBundles", System.Data.SqlDbType.NVarChar).Value = tblStockSummaryTO.NoOfBundles;
            cmdUpdate.Parameters.Add("@TotalStock", System.Data.SqlDbType.NVarChar).Value = tblStockSummaryTO.TotalStock;

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblStockSummary(Int32 idStockSummary)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStockSummary, cmdDelete);
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

        public int DeleteTblStockSummary(Int32 idStockSummary, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStockSummary, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idStockSummary, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblStockSummary] " +
            " WHERE idStockSummary = " + idStockSummary +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStockSummary", System.Data.SqlDbType.Int).Value = tblStockSummaryTO.IdStockSummary;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
