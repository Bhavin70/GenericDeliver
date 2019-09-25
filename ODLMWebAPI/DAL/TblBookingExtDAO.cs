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
    public class TblBookingExtDAO : ITblBookingExtDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblBookingExtDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT  bookingDtl.* ,material.materialSubType,booking.bookingDatetime,isConfirmed,isJointDelivery,cdStructure,noOfDeliveries " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc,booking.brandId, tblBookingSchedule.scheduleDate," +
                                  "  brand.brandName AS brandDesc ,item.itemName,prodClass.displayName,tblBookingSchedule.loadingLayerId ,item.conversionFactor " +
                                  //" , loadingLayer.layerDesc 
                                  " FROM tblBookingExt bookingDtl " +
                                  " LEFT JOIN tblBookings booking " +
                                  " ON booking.idBooking = bookingDtl.bookingId " +
                                  " LEFT JOIN tblMaterial material " +
                                  " ON material.idMaterial = bookingDtl.materialId " +
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=bookingDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=bookingDtl.prodSpecId" +
                                  " LEFT JOIN  dimBrand brand ON brand.idBrand=bookingDtl.brandId" +
                                  //" LEFT JOIN  dimLoadingLayers loadingLayer ON loadingLayer.idLoadingLayer=bookingDtl.loadinglayerId" +
                                  " LEFT JOIN [tblBookingSchedule] tblBookingSchedule ON tblBookingSchedule.idSchedule=bookingDtl.scheduleId" +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = bookingDtl.prodItemId " +
                                   " LEFT JOIN tblProdClassification prodClass ON item.prodClassId = prodClass.idProdClass ";
                                  

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblBookingExtTO> SelectAllTblBookingExt()
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

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
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

        public List<TblBookingExtTO> SelectAllTblBookingExt(int bookingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingDtl.bookingId=" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
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

        public List<TblBookingExtTO> SelectAllTblBookingExt(int bookingId, SqlConnection conn, SqlTransaction trans)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingDtl.bookingId=" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = trans;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null) sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblBookingExtTO> SelectEmptyBookingExtList(int prodCatgId, Int32 prodSpecId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT tblMaterial.idMaterial AS materialId, tblMaterial.materialSubType , " +
                                        " dimProdCat.idProdCat AS prodCatId,dimProdCat.prodCateDesc prodCatDesc, " +
                                        " dimProdSpec.idProdSpec AS prodSpecId ,dimProdSpec.prodSpecDesc " +
                                        " FROM tblMaterial " +
                                        " FULL OUTER JOIN dimProdCat ON 1 = 1 " +
                                        " FULL OUTER JOIN dimProdSpec ON 1 = 1" +
                                        " WHERE idProdCat=" + prodCatgId + " AND idProdSpec=" + prodSpecId;


                // cmdSelect.CommandText = " select materialSubType,p.avgBundleWt,p.prodCatId,p.prodSpecId,p.materialId,prodCatDesc,prodSpecDesc from( " +
                //                         " SELECT m.idMaterial AS materialId, m.materialSubType as materialSubType ,  " +
                //                         " cat.idProdCat AS prodCatId, spec.idProdSpec AS prodSpecId, cat.prodCateDesc as prodCatDesc, " +
                //                         "spec.prodSpecDesc As prodSpecDesc FROM tblMaterial m " +
                //                         " inner JOIN dimProdCat cat ON 1 = 1  " +
                //                         " inner JOIN dimProdSpec spec ON 1 = 1 " +
                //                         " WHERE idProdCat=" + prodCatgId + " AND idProdSpec=" + prodSpecId + " )" +
                //                         " sql  join tblProductInfo p on p.materialId=sql.materialId " +
                //                         " WHERE p.prodCatId=" + prodCatgId + " AND p.prodSpecId=" + prodSpecId;




                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> tblBookingExtTOList = new List<Models.TblBookingExtTO>();

                while (sqlReader.Read())
                {
                    TblBookingExtTO tblBookingExtTONew = new TblBookingExtTO();
                    if (sqlReader["materialId"] != DBNull.Value)
                        tblBookingExtTONew.MaterialId = Convert.ToInt32(sqlReader["materialId"].ToString());
                    if (sqlReader["materialSubType"] != DBNull.Value)
                        tblBookingExtTONew.MaterialSubType = Convert.ToString(sqlReader["materialSubType"].ToString());
                    if (sqlReader["prodCatId"] != DBNull.Value)
                        tblBookingExtTONew.ProdCatId = Convert.ToInt32(sqlReader["prodCatId"].ToString());
                    if (sqlReader["prodSpecId"] != DBNull.Value)
                        tblBookingExtTONew.ProdSpecId = Convert.ToInt32(sqlReader["prodSpecId"].ToString());
                    if (sqlReader["prodCatDesc"] != DBNull.Value)
                        tblBookingExtTONew.ProdCatDesc = Convert.ToString(sqlReader["prodCatDesc"].ToString());
                    if (sqlReader["prodSpecDesc"] != DBNull.Value)
                        tblBookingExtTONew.ProdSpecDesc = Convert.ToString(sqlReader["prodSpecDesc"].ToString());
                    // if (sqlReader["avgBundleWt"] != DBNull.Value)
                    //     tblBookingExtTONew.AvgBundleWt = Convert.ToDouble(sqlReader["avgBundleWt"].ToString());
                    tblBookingExtTOList.Add(tblBookingExtTONew);
                }
                return tblBookingExtTOList;
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

        public List<TblBookingExtTO> SelectBookingDetails(Int32 dealerId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE dealerOrgId=" + dealerId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
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

        public TblBookingExtTO SelectTblBookingExt(Int32 idBookingExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBookingExt = " + idBookingExt + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(reader);
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


        public List<TblBookingExtTO> ConvertDTToList(SqlDataReader tblBookingExtTODT)
        {
            List<TblBookingExtTO> tblBookingExtTOList = new List<TblBookingExtTO>();
            if (tblBookingExtTODT != null)
            {
                while (tblBookingExtTODT.Read())
                {
                    TblBookingExtTO tblBookingExtTONew = new TblBookingExtTO();
                    if (tblBookingExtTODT["idBookingExt"] != DBNull.Value)
                        tblBookingExtTONew.IdBookingExt = Convert.ToInt32(tblBookingExtTODT["idBookingExt"].ToString());
                    if (tblBookingExtTODT["bookingId"] != DBNull.Value)
                        tblBookingExtTONew.BookingId = Convert.ToInt32(tblBookingExtTODT["bookingId"].ToString());
                    if (tblBookingExtTODT["materialId"] != DBNull.Value)
                        tblBookingExtTONew.MaterialId = Convert.ToInt32(tblBookingExtTODT["materialId"].ToString());
                    if (tblBookingExtTODT["bookedQty"] != DBNull.Value)
                        tblBookingExtTONew.BookedQty = Convert.ToDouble(tblBookingExtTODT["bookedQty"].ToString());
                    if (tblBookingExtTODT["rate"] != DBNull.Value)
                        tblBookingExtTONew.Rate = Convert.ToDouble(tblBookingExtTODT["rate"].ToString());

                    if (tblBookingExtTODT["materialSubType"] != DBNull.Value)
                        tblBookingExtTONew.MaterialSubType = Convert.ToString(tblBookingExtTODT["materialSubType"].ToString());

                    if (tblBookingExtTODT["bookingDatetime"] != DBNull.Value)
                        tblBookingExtTONew.BookingDatetime = Convert.ToDateTime(tblBookingExtTODT["bookingDatetime"].ToString());

                    if (tblBookingExtTODT["isConfirmed"] != DBNull.Value)
                        tblBookingExtTONew.IsConfirmed = Convert.ToInt32(tblBookingExtTODT["isConfirmed"].ToString());

                    if (tblBookingExtTODT["noOfDeliveries"] != DBNull.Value)
                        tblBookingExtTONew.NoOfDeliveries = Convert.ToInt32(tblBookingExtTODT["noOfDeliveries"].ToString());

                    if (tblBookingExtTODT["isJointDelivery"] != DBNull.Value)
                        tblBookingExtTONew.IsJointDelivery = Convert.ToInt32(tblBookingExtTODT["isJointDelivery"].ToString());

                    if (tblBookingExtTODT["cdStructure"] != DBNull.Value)
                        tblBookingExtTONew.CdStructure = Convert.ToDouble(tblBookingExtTODT["cdStructure"].ToString());
                    if (tblBookingExtTODT["prodCatId"] != DBNull.Value)
                        tblBookingExtTONew.ProdCatId = Convert.ToInt32(tblBookingExtTODT["prodCatId"].ToString());
                    if (tblBookingExtTODT["prodSpecId"] != DBNull.Value)
                        tblBookingExtTONew.ProdSpecId = Convert.ToInt32(tblBookingExtTODT["prodSpecId"].ToString());
                    if (tblBookingExtTODT["prodCatDesc"] != DBNull.Value)
                        tblBookingExtTONew.ProdCatDesc = Convert.ToString(tblBookingExtTODT["prodCatDesc"].ToString());
                    if (tblBookingExtTODT["prodSpecDesc"] != DBNull.Value)
                        tblBookingExtTONew.ProdSpecDesc = Convert.ToString(tblBookingExtTODT["prodSpecDesc"].ToString());

                    //Vijaymala [13-12-2017]Added
                    if (tblBookingExtTODT["brandId"] != DBNull.Value)
                        tblBookingExtTONew.BrandId = Convert.ToInt32(tblBookingExtTODT["brandId"].ToString());
                    if (tblBookingExtTODT["scheduleId"] != DBNull.Value)
                        tblBookingExtTONew.ScheduleId = Convert.ToInt32(tblBookingExtTODT["scheduleId"].ToString());
                    if (tblBookingExtTODT["balanceQty"] != DBNull.Value)
                        tblBookingExtTONew.BalanceQty = Convert.ToDouble(tblBookingExtTODT["balanceQty"].ToString());

                    if (tblBookingExtTODT["scheduleDate"] != DBNull.Value)
                        tblBookingExtTONew.ScheduleDate = Convert.ToDateTime(tblBookingExtTODT["scheduleDate"].ToString());

                    if (tblBookingExtTODT["brandDesc"] != DBNull.Value)
                        tblBookingExtTONew.BrandDesc = Convert.ToString(tblBookingExtTODT["brandDesc"].ToString());

                    //Vijaymala [02-1-2017]Added
                    if (tblBookingExtTODT["loadingLayerId"] != DBNull.Value)
                        tblBookingExtTONew.LoadingLayerId = Convert.ToInt32(tblBookingExtTODT["loadingLayerId"].ToString());
                    //if (tblBookingExtTODT["layerDesc"] != DBNull.Value)
                    //    tblBookingExtTONew.LayerDesc = Convert.ToString(tblBookingExtTODT["layerDesc"].ToString());
                    //[05-09-2018]Vijaymala added for other item
                    if (tblBookingExtTODT["prodItemId"] != DBNull.Value)
                        tblBookingExtTONew.ProdItemId = Convert.ToInt32(tblBookingExtTODT["prodItemId"].ToString());

                    if (tblBookingExtTODT["itemName"] != DBNull.Value)
                        tblBookingExtTONew.ItemName = Convert.ToString(tblBookingExtTODT["itemName"].ToString());

                    if (tblBookingExtTODT["displayName"] != DBNull.Value)
                        tblBookingExtTONew.DisplayName = Convert.ToString(tblBookingExtTODT["displayName"].ToString());

                    if (tblBookingExtTONew.ProdItemId > 0)
                    {
                        tblBookingExtTONew.DisplayName = tblBookingExtTONew.DisplayName + "-" + tblBookingExtTONew.ItemName;
                    }
                    else
                    {
                        tblBookingExtTONew.DisplayName = tblBookingExtTONew.MaterialSubType + "-" + tblBookingExtTONew.ProdCatDesc + "-" + tblBookingExtTONew.ProdSpecDesc
                            + "(" + tblBookingExtTONew.BrandDesc + ")";
                    }
                    //Aniket [13-6-2019]
                    if (tblBookingExtTODT["discount"] != DBNull.Value)
                        tblBookingExtTONew.Discount = Convert.ToDouble(tblBookingExtTODT["discount"]);
                    if (tblBookingExtTODT["conversionFactor"] != DBNull.Value)
                        tblBookingExtTONew.ConversionFactor = Convert.ToDouble(tblBookingExtTODT["conversionFactor"]);
                    if (tblBookingExtTODT["uomQty"] != DBNull.Value)
                        tblBookingExtTONew.UomQty = Convert.ToDouble(tblBookingExtTODT["uomQty"]);
                    if (tblBookingExtTODT["pendingUomQty"] != DBNull.Value)
                        tblBookingExtTONew.PendingUomQty = Convert.ToDouble(tblBookingExtTODT["pendingUomQty"]);
                    tblBookingExtTOList.Add(tblBookingExtTONew);
                }
            }
            return tblBookingExtTOList;
        }

        /// <summary>
        /// [15-12-2017]Vijaymala:Added to get booking extension list according to schedule
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(int scheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE scheduleId=" + scheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
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

        public List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(int scheduleId, SqlConnection conn, SqlTransaction trans)
        {

            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                //conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE scheduleId=" + scheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Transaction = trans;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //conn.Close();
                cmdSelect.Dispose();
            }
        }


        #endregion

        #region Insertion
        public int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingExtTO, cmdInsert);
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

        public int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingExtTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblBookingExtTO tblBookingExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingExt]( " +
                            "  [bookingId]" +
                            " ,[materialId]" +
                            " ,[bookedQty]" +
                            " ,[rate]" +
                            " ,[prodCatId]" +
                            " ,[prodSpecId]" +
                            " ,[scheduleId]" +
                            " ,[brandId]" +
                            " ,[balanceQty]" +
                            " ,[prodItemId]" +
                            " ,[discount]" +
                            " ,[uomQty]" +
                            " ,[pendingUomQty]" +
                            //" ,[loadingLayerId]" +
                            " )" +
                " VALUES (" +
                            "  @BookingId " +
                            " ,@MaterialId " +
                            " ,@BookedQty " +
                            " ,@Rate " +
                            " ,@prodCatId " +
                            " ,@prodSpecId " +
                            " ,@ScheduleId " +
                            " ,@BrandId " +
                            " ,@BalanceQty " +
                            " ,@ProdItemId " +
                            " ,@discount " +
                            " ,@uomQty " +
                            " ,@pendingUomQty " +
                            //" ,@LoadingLayerId" +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingExtTO.BookingId;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.MaterialId);
            cmdInsert.Parameters.Add("@BookedQty", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.BookedQty;
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.Rate;
            cmdInsert.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdCatId);
            cmdInsert.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdSpecId);
            cmdInsert.Parameters.Add("@ScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ScheduleId);
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.BrandId);
            cmdInsert.Parameters.Add("@BalanceQty", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.BalanceQty;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdItemId);
            cmdInsert.Parameters.Add("@discount", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.Discount);
            cmdInsert.Parameters.Add("@uomQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.UomQty);
            cmdInsert.Parameters.Add("@pendingUomQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.PendingUomQty);

            //cmdInsert.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = tblBookingExtTO.LoadingLayerId;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblBookingExtTO.IdBookingExt = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingExtTO, cmdUpdate);
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

        public int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingExtTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblBookingExtTO tblBookingExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingExt] SET " +
                            "  [bookingId]= @BookingId" +
                            " ,[materialId]= @MaterialId" +
                            " ,[bookedQty]= @BookedQty" +
                            " ,[rate] = @Rate" +
                            " ,[prodCatId] = @prodCatId" +
                            " ,[prodSpecId] = @prodSpecId" +
                            " ,[scheduleId] = @ScheduleId" +
                            " ,[brandId] = @BrandId" +
                            " ,[balanceQty] = @BalanceQty" +
                            " ,[prodItemId] = @ProdItemId" +
                            " ,[discount] = @discount" +
                            " ,[uomQty] = @uomQty" +
                            " ,[pendingUomQty] = @pendingUomQty" +
                            //" ,[loadingLayerId] = @LoadingLayerId" +
                            " WHERE [idBookingExt] = @IdBookingExt ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingExtTO.BookingId;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.MaterialId);
            cmdUpdate.Parameters.Add("@BookedQty", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.BookedQty;
            cmdUpdate.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.Rate;
            cmdUpdate.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdCatId);
            cmdUpdate.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@ScheduleId", System.Data.SqlDbType.Int).Value = tblBookingExtTO.ScheduleId;
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.BrandId);
            cmdUpdate.Parameters.Add("@BalanceQty", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.BalanceQty;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdItemId);
            cmdUpdate.Parameters.Add("@discount", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.Discount);
            cmdUpdate.Parameters.Add("@uomQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.UomQty);
            cmdUpdate.Parameters.Add("@pendingUomQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.PendingUomQty);

            //cmdUpdate.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = tblBookingExtTO.LoadingLayerId;

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblBookingExt(Int32 idBookingExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBookingExt, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblBookingExt(Int32 idBookingExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBookingExt, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idBookingExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingExt] " +
            " WHERE idBookingExt = " + idBookingExt + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
            return cmdDelete.ExecuteNonQuery();
        }
        public int DeleteTblBookingExtBySchedule(Int32 scheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommandByScheduleId(scheduleId, cmdDelete);
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

        public int ExecuteDeletionCommandByScheduleId(Int32 scheduleId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingExt] " +
            " WHERE scheduleId = " + scheduleId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
            return cmdDelete.ExecuteNonQuery();
        }

        #endregion

    }
}
