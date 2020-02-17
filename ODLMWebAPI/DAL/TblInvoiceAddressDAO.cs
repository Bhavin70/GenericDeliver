using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblInvoiceAddressDAO : ITblInvoiceAddressDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblInvoiceAddressDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tempInvoiceAddress]" +

                                  // Vaibhav [10-Jan-2018] Added to select from finalInvoiceAddress
                                  " UNION ALL " +
                                  " SELECT * FROM [finalInvoiceAddress]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblInvoiceAddressTO> SelectAllTblInvoiceAddress()
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
                List<TblInvoiceAddressTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
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

        public TblInvoiceAddressTO SelectTblInvoiceAddress(Int32 idInvoiceAddr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery()+ ")sq1 WHERE idInvoiceAddr = " + idInvoiceAddr +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceAddressTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
            }
            catch(Exception ex)
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
        public List<TblInvoiceAddressTO> SelectTblInvoiceAddressByDealerId(Int32 dealerOrgId, String addrSrcTypeString,Int32 topRecordcnt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                String addrTypeCon = null;
                //if (!String.IsNullOrEmpty(addrSrcTypeString))
                //{
                //    addrTypeCon = " AND txnAddrTypeId IN (" + addrSrcTypeString + ")";
                //}
                //chetan[12-feb-2020] added for get disting address
                if (topRecordcnt > 0)
                {
                    cmdSelect.CommandText = "select top("+ topRecordcnt + ") billingName,gstinNo,panNo,aadharNo,contactNo,[address],taluka,talukaId,district,districtId,[state],stateId, "+
                                             " pinCode,CountryId,village from tempInvoiceAddress addr where addr.billingOrgId = @dealerOrgId and addr.txnAddrTypeId in("+ addrSrcTypeString + " )"+
                                             " group by billingName, gstinNo, panNo, aadharNo, contactNo,[address], taluka, talukaId, district, districtId,[state], "+
                                             " stateId,  pinCode, CountryId, village ";
                }
                else
                {
                    cmdSelect.CommandText = " select addr.* from tempInvoiceAddress addr where addr.billingOrgId=@dealerOrgId and addr.txnAddrTypeId in(" + addrSrcTypeString + ") " +
                    "  order by addr.invoiceId desc";
                }
                cmdSelect.Parameters.AddWithValue("@dealerOrgId",DbType.Int32).Value=dealerOrgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceAddressTO> list = ConvertInvoiceAddressDTToList(sqlReader);
                return list;
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
        public List<TblInvoiceAddressTO> SelectAllTblInvoiceAddress(Int32 invoiceId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE invoiceId=" + invoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceAddressTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblInvoiceAddressTO> ConvertInvoiceAddressDTToList(SqlDataReader tblInvoiceAddressTODT)
        {
            List<TblInvoiceAddressTO> tblInvoiceAddressTOList = new List<TblInvoiceAddressTO>();
            if (tblInvoiceAddressTODT != null)
            {
                while (tblInvoiceAddressTODT.Read())
                {
                    TblInvoiceAddressTO tblInvoiceAddressTONew = new TblInvoiceAddressTO();
                    //if (tblInvoiceAddressTODT["idInvoiceAddr"] != DBNull.Value)
                    //    tblInvoiceAddressTONew.IdInvoiceAddr = Convert.ToInt32(tblInvoiceAddressTODT["idInvoiceAddr"].ToString());
                    //if (tblInvoiceAddressTODT["invoiceId"] != DBNull.Value)
                    //    tblInvoiceAddressTONew.InvoiceId = Convert.ToInt32(tblInvoiceAddressTODT["invoiceId"].ToString());
                    //if (tblInvoiceAddressTODT["txnAddrTypeId"] != DBNull.Value)
                    //    tblInvoiceAddressTONew.TxnAddrTypeId = Convert.ToInt32(tblInvoiceAddressTODT["txnAddrTypeId"].ToString());
                    //if (tblInvoiceAddressTODT["billingOrgId"] != DBNull.Value)
                    //    tblInvoiceAddressTONew.BillingOrgId = Convert.ToInt32(tblInvoiceAddressTODT["billingOrgId"].ToString());
                    if (tblInvoiceAddressTODT["talukaId"] != DBNull.Value)
                        tblInvoiceAddressTONew.TalukaId = Convert.ToInt32(tblInvoiceAddressTODT["talukaId"].ToString());
                    if (tblInvoiceAddressTODT["districtId"] != DBNull.Value)
                        tblInvoiceAddressTONew.DistrictId = Convert.ToInt32(tblInvoiceAddressTODT["districtId"].ToString());
                    if (tblInvoiceAddressTODT["stateId"] != DBNull.Value)
                        tblInvoiceAddressTONew.StateId = Convert.ToInt32(tblInvoiceAddressTODT["stateId"].ToString());
                    if (tblInvoiceAddressTODT["countryId"] != DBNull.Value)
                        tblInvoiceAddressTONew.CountryId = Convert.ToInt32(tblInvoiceAddressTODT["countryId"].ToString());
                    if (tblInvoiceAddressTODT["billingName"] != DBNull.Value)
                        tblInvoiceAddressTONew.BillingName = Convert.ToString(tblInvoiceAddressTODT["billingName"].ToString());
                    if (tblInvoiceAddressTODT["gstinNo"] != DBNull.Value)
                        tblInvoiceAddressTONew.GstinNo = Convert.ToString(tblInvoiceAddressTODT["gstinNo"].ToString());
                    if (tblInvoiceAddressTODT["panNo"] != DBNull.Value)
                        tblInvoiceAddressTONew.PanNo = Convert.ToString(tblInvoiceAddressTODT["panNo"].ToString());

                    //Saket [2019-09-26] Added
                    if (!String.IsNullOrEmpty(tblInvoiceAddressTONew.GstinNo))
                        tblInvoiceAddressTONew.GstinNo = tblInvoiceAddressTONew.GstinNo.ToUpper();
                    if (!String.IsNullOrEmpty(tblInvoiceAddressTONew.PanNo))
                        tblInvoiceAddressTONew.PanNo = tblInvoiceAddressTONew.PanNo.ToUpper();

                    if (tblInvoiceAddressTODT["aadharNo"] != DBNull.Value)
                        tblInvoiceAddressTONew.AadharNo = Convert.ToString(tblInvoiceAddressTODT["aadharNo"].ToString());
                    if (tblInvoiceAddressTODT["contactNo"] != DBNull.Value)
                        tblInvoiceAddressTONew.ContactNo = Convert.ToString(tblInvoiceAddressTODT["contactNo"].ToString());
                    if (tblInvoiceAddressTODT["address"] != DBNull.Value)
                        tblInvoiceAddressTONew.Address = Convert.ToString(tblInvoiceAddressTODT["address"].ToString());
                    if (tblInvoiceAddressTODT["taluka"] != DBNull.Value)
                        tblInvoiceAddressTONew.Taluka = Convert.ToString(tblInvoiceAddressTODT["taluka"].ToString());
                    if (tblInvoiceAddressTODT["district"] != DBNull.Value)
                        tblInvoiceAddressTONew.District = Convert.ToString(tblInvoiceAddressTODT["district"].ToString());
                    if (tblInvoiceAddressTODT["state"] != DBNull.Value)
                        tblInvoiceAddressTONew.State = Convert.ToString(tblInvoiceAddressTODT["state"].ToString());
                    if (tblInvoiceAddressTODT["pinCode"] != DBNull.Value)
                        tblInvoiceAddressTONew.PinCode = Convert.ToString(tblInvoiceAddressTODT["pinCode"].ToString());
                    //if (tblInvoiceAddressTODT["addrSourceTypeId"] != DBNull.Value)
                    //    tblInvoiceAddressTONew.AddrSourceTypeId = Convert.ToInt32(tblInvoiceAddressTODT["addrSourceTypeId"]);

                    //Sanjay 17-09-2019 Prev village was not included in master.
                    if (tblInvoiceAddressTODT["village"] != DBNull.Value)
                        tblInvoiceAddressTONew.VillageName = Convert.ToString(tblInvoiceAddressTODT["village"].ToString());

                    tblInvoiceAddressTOList.Add(tblInvoiceAddressTONew);
                }
            }
            return tblInvoiceAddressTOList;
        }

        public List<TblInvoiceAddressTO> ConvertDTToList(SqlDataReader tblInvoiceAddressTODT)
        {
            List<TblInvoiceAddressTO> tblInvoiceAddressTOList = new List<TblInvoiceAddressTO>();
            if (tblInvoiceAddressTODT != null)
            {
                while (tblInvoiceAddressTODT.Read())
                {
                    TblInvoiceAddressTO tblInvoiceAddressTONew = new TblInvoiceAddressTO();
                    if (tblInvoiceAddressTODT["idInvoiceAddr"] != DBNull.Value)
                        tblInvoiceAddressTONew.IdInvoiceAddr = Convert.ToInt32(tblInvoiceAddressTODT["idInvoiceAddr"].ToString());
                    if (tblInvoiceAddressTODT["invoiceId"] != DBNull.Value)
                        tblInvoiceAddressTONew.InvoiceId = Convert.ToInt32(tblInvoiceAddressTODT["invoiceId"].ToString());
                    if (tblInvoiceAddressTODT["txnAddrTypeId"] != DBNull.Value)
                        tblInvoiceAddressTONew.TxnAddrTypeId = Convert.ToInt32(tblInvoiceAddressTODT["txnAddrTypeId"].ToString());
                    if (tblInvoiceAddressTODT["billingOrgId"] != DBNull.Value)
                        tblInvoiceAddressTONew.BillingOrgId = Convert.ToInt32(tblInvoiceAddressTODT["billingOrgId"].ToString());
                    if (tblInvoiceAddressTODT["talukaId"] != DBNull.Value)
                        tblInvoiceAddressTONew.TalukaId = Convert.ToInt32(tblInvoiceAddressTODT["talukaId"].ToString());
                    if (tblInvoiceAddressTODT["districtId"] != DBNull.Value)
                        tblInvoiceAddressTONew.DistrictId = Convert.ToInt32(tblInvoiceAddressTODT["districtId"].ToString());
                    if (tblInvoiceAddressTODT["stateId"] != DBNull.Value)
                        tblInvoiceAddressTONew.StateId = Convert.ToInt32(tblInvoiceAddressTODT["stateId"].ToString());
                    if (tblInvoiceAddressTODT["countryId"] != DBNull.Value)
                        tblInvoiceAddressTONew.CountryId = Convert.ToInt32(tblInvoiceAddressTODT["countryId"].ToString());
                    if (tblInvoiceAddressTODT["billingName"] != DBNull.Value)
                        tblInvoiceAddressTONew.BillingName = Convert.ToString(tblInvoiceAddressTODT["billingName"].ToString());
                    if (tblInvoiceAddressTODT["gstinNo"] != DBNull.Value)
                        tblInvoiceAddressTONew.GstinNo = Convert.ToString(tblInvoiceAddressTODT["gstinNo"].ToString());
                    if (tblInvoiceAddressTODT["panNo"] != DBNull.Value)
                        tblInvoiceAddressTONew.PanNo = Convert.ToString(tblInvoiceAddressTODT["panNo"].ToString());

                    //Saket [2019-09-26] Added
                    if (!String.IsNullOrEmpty(tblInvoiceAddressTONew.GstinNo))
                        tblInvoiceAddressTONew.GstinNo = tblInvoiceAddressTONew.GstinNo.ToUpper();
                    if (!String.IsNullOrEmpty(tblInvoiceAddressTONew.PanNo))
                        tblInvoiceAddressTONew.PanNo = tblInvoiceAddressTONew.PanNo.ToUpper();

                    if (tblInvoiceAddressTODT["aadharNo"] != DBNull.Value)
                        tblInvoiceAddressTONew.AadharNo = Convert.ToString(tblInvoiceAddressTODT["aadharNo"].ToString());
                    if (tblInvoiceAddressTODT["contactNo"] != DBNull.Value)
                        tblInvoiceAddressTONew.ContactNo = Convert.ToString(tblInvoiceAddressTODT["contactNo"].ToString());
                    if (tblInvoiceAddressTODT["address"] != DBNull.Value)
                        tblInvoiceAddressTONew.Address = Convert.ToString(tblInvoiceAddressTODT["address"].ToString());
                    if (tblInvoiceAddressTODT["taluka"] != DBNull.Value)
                        tblInvoiceAddressTONew.Taluka = Convert.ToString(tblInvoiceAddressTODT["taluka"].ToString());
                    if (tblInvoiceAddressTODT["district"] != DBNull.Value)
                        tblInvoiceAddressTONew.District = Convert.ToString(tblInvoiceAddressTODT["district"].ToString());
                    if (tblInvoiceAddressTODT["state"] != DBNull.Value)
                        tblInvoiceAddressTONew.State = Convert.ToString(tblInvoiceAddressTODT["state"].ToString());
                    if (tblInvoiceAddressTODT["pinCode"] != DBNull.Value)
                        tblInvoiceAddressTONew.PinCode = Convert.ToString(tblInvoiceAddressTODT["pinCode"].ToString());
                    if (tblInvoiceAddressTODT["addrSourceTypeId"] != DBNull.Value)
                        tblInvoiceAddressTONew.AddrSourceTypeId = Convert.ToInt32(tblInvoiceAddressTODT["addrSourceTypeId"]);

                    //Sanjay 17-09-2019 Prev village was not included in master.
                    if (tblInvoiceAddressTODT["village"] != DBNull.Value)
                        tblInvoiceAddressTONew.VillageName = Convert.ToString(tblInvoiceAddressTODT["village"].ToString());

                    tblInvoiceAddressTOList.Add(tblInvoiceAddressTONew);
                }
            }
            return tblInvoiceAddressTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceAddressTO, cmdInsert);
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

        public int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceAddressTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblInvoiceAddressTO tblInvoiceAddressTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempInvoiceAddress]( " + 
                                "  [invoiceId]" +
                                " ,[txnAddrTypeId]" +
                                " ,[billingOrgId]" +
                                " ,[talukaId]" +
                                " ,[districtId]" +
                                " ,[stateId]" +
                                " ,[countryId]" +
                                " ,[billingName]" +
                                " ,[gstinNo]" +
                                " ,[panNo]" +
                                " ,[aadharNo]" +
                                " ,[contactNo]" +
                                " ,[address]" +
                                " ,[taluka]" +
                                " ,[district]" +
                                " ,[state]" +
                                " ,[pinCode]" +
                                " ,[addrSourceTypeId]" +
                                " ,[village]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceId " +
                                " ,@TxnAddrTypeId " +
                                " ,@BillingOrgId " +
                                " ,@TalukaId " +
                                " ,@DistrictId " +
                                " ,@StateId " +
                                " ,@CountryId " +
                                " ,@BillingName " +
                                " ,@GstinNo " +
                                " ,@PanNo " +
                                " ,@AadharNo " +
                                " ,@ContactNo " +
                                " ,@Address " +
                                " ,@Taluka " +
                                " ,@District " +
                                " ,@State " +
                                " ,@PinCode " +
                                " ,@addrSourceTypeId " +
                                " ,@village " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoiceAddr", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.IdInvoiceAddr;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.InvoiceId;
            cmdInsert.Parameters.Add("@TxnAddrTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.TxnAddrTypeId);
            cmdInsert.Parameters.Add("@BillingOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.BillingOrgId);
            cmdInsert.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.TalukaId);
            cmdInsert.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.DistrictId);
            cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.StateId);
            cmdInsert.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.CountryId);
            cmdInsert.Parameters.Add("@BillingName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.BillingName);
            cmdInsert.Parameters.Add("@GstinNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.GstinNo);
            cmdInsert.Parameters.Add("@PanNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.PanNo);
            cmdInsert.Parameters.Add("@AadharNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.AadharNo);
            cmdInsert.Parameters.Add("@ContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.ContactNo);
            cmdInsert.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.Address);
            cmdInsert.Parameters.Add("@Taluka", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.Taluka);
            cmdInsert.Parameters.Add("@District", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.District);
            cmdInsert.Parameters.Add("@State", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.State);
            cmdInsert.Parameters.Add("@PinCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.PinCode);
            cmdInsert.Parameters.Add("@addrSourceTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.AddrSourceTypeId);
            cmdInsert.Parameters.Add("@village", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.VillageName);

            if (cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceAddressTO.IdInvoiceAddr = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblInvoiceAddressTO, cmdUpdate);
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

        public int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblInvoiceAddressTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblInvoiceAddressTO tblInvoiceAddressTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempInvoiceAddress] SET " + 
                                "  [invoiceId]= @InvoiceId" +
                                " ,[txnAddrTypeId]= @TxnAddrTypeId" +
                                " ,[billingOrgId]= @BillingOrgId" +
                                " ,[talukaId]= @TalukaId" +
                                " ,[districtId]= @DistrictId" +
                                " ,[stateId]= @StateId" +
                                " ,[countryId]= @CountryId" +
                                " ,[billingName]= @BillingName" +
                                " ,[gstinNo]= @GstinNo" +
                                " ,[panNo]= @PanNo" +
                                " ,[aadharNo]= @AadharNo" +
                                " ,[contactNo]= @ContactNo" +
                                " ,[address]= @Address" +
                                " ,[taluka]= @Taluka" +
                                " ,[district]= @District" +
                                " ,[state]= @State" +
                                " ,[pinCode] = @PinCode" +
                                " ,[addrSourceTypeId] = @addrSourceTypeId " +
                                " ,[village] = @village" +

                                " WHERE [idInvoiceAddr] = @IdInvoiceAddr"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdInvoiceAddr", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.IdInvoiceAddr;
            cmdUpdate.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.InvoiceId;
            cmdUpdate.Parameters.Add("@TxnAddrTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.TxnAddrTypeId);
            cmdUpdate.Parameters.Add("@BillingOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.BillingOrgId);
            cmdUpdate.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.TalukaId);
            cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.DistrictId);
            cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.StateId);
            cmdUpdate.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.CountryId);
            cmdUpdate.Parameters.Add("@BillingName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.BillingName);
            cmdUpdate.Parameters.Add("@GstinNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.GstinNo);
            cmdUpdate.Parameters.Add("@PanNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.PanNo);
            cmdUpdate.Parameters.Add("@AadharNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.AadharNo);
            cmdUpdate.Parameters.Add("@ContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.ContactNo);
            cmdUpdate.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.Address);
            cmdUpdate.Parameters.Add("@Taluka", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.Taluka);
            cmdUpdate.Parameters.Add("@District", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.District);
            cmdUpdate.Parameters.Add("@State", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.State);
            cmdUpdate.Parameters.Add("@PinCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.PinCode);
            cmdUpdate.Parameters.Add("@addrSourceTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.AddrSourceTypeId);
            cmdUpdate.Parameters.Add("@village", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.VillageName);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblInvoiceAddress(Int32 idInvoiceAddr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idInvoiceAddr, cmdDelete);
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
        public int DeleteTblInvoiceAddressByinvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                cmdDelete.CommandText = "DELETE FROM [tempInvoiceAddress] " +
                " WHERE invoiceId = " + invoiceId + "";
                cmdDelete.CommandType = System.Data.CommandType.Text;

                //cmdDelete.Parameters.Add("@idInvoiceAddr", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.IdInvoiceAddr;
                return cmdDelete.ExecuteNonQuery();

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
        public int DeleteTblInvoiceAddress(Int32 idInvoiceAddr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idInvoiceAddr, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idInvoiceAddr, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoiceAddress] " +
            " WHERE idInvoiceAddr = " + idInvoiceAddr +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvoiceAddr", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.IdInvoiceAddr;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
