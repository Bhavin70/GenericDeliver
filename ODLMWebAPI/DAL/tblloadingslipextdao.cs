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
    public class TblLoadingSlipExtDAO : ITblLoadingSlipExtDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblLoadingSlipExtDAO(ICommon iCommon, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc,brand.brandName as brandDesc, prodSpec.isWeighingAllow " +
                                  " ,item.itemName,prodClass.displayName FROM tempLoadingSlipExt loadDtl  " +
                                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                                  "  LEFT JOIN tblMaterial material " +
                                  "  ON material.idMaterial = loadDtl.materialId " +
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId" +
                                  " LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId " +
                                  " LEFT JOIN tblProdClassification prodClass ON item.prodClassId = prodClass.idProdClass " +

            // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipExt

            " UNION ALL " +

                                  " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc,brand.brandName as brandDesc, prodSpec.isWeighingAllow " +
                                  " ,item.itemName,prodClass.displayName  FROM finalLoadingSlipExt loadDtl  " +
                                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                                  "  LEFT JOIN tblMaterial material " +
                                  "  ON material.idMaterial = loadDtl.materialId " +
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId" +
                                  " LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId " +
                                  " LEFT JOIN tblProdClassification prodClass ON item.prodClassId = prodClass.idProdClass ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        //Aniket [22-3-2019] added to get loadingSlipExt details against 
        public List<TblLoadingSlipExtTO> GetAllLoadingExtByBookingId(int bookingId,string configval)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string condition = "";
            try
            {
                conn.Open();
                if (!String.IsNullOrEmpty(configval))
                {
                     condition = " AND statusId NOT IN (" + configval + ")";
                }
                cmdSelect.CommandText = "select t.statusId, e.* from tempLoadingSlipExt e " +
                                       " left join tempLoadingSlip t on t.idLoadingSlip=e.loadingSlipId " +
                                       " where bookingId=@bookingId " + condition +
                                       " union "+
                                        " select t.statusId, e.* from finalLoadingSlipExt e "+
                                        " left join tempLoadingSlip t on t.idLoadingSlip=e.loadingSlipId "+
                                        " where bookingId =@bookingId" +condition;
                
                
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.AddWithValue("@bookingId", DbType.Int32).Value = bookingId;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTTo(sqlReader);
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
        //Aniket [22-03-2019] added ConvertTODTList for LoadingSlipExtDAO
        public List<TblLoadingSlipExtTO> ConvertDTTo(SqlDataReader tblLoadingSlipExtTODT)
        {
            List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
            if (tblLoadingSlipExtTODT != null)
            {
                while (tblLoadingSlipExtTODT.Read())
                {
                    TblLoadingSlipExtTO tblLoadingSlipExtTONew = new TblLoadingSlipExtTO();
                    if (tblLoadingSlipExtTODT["idLoadingSlipExt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.IdLoadingSlipExt = Convert.ToInt32(tblLoadingSlipExtTODT["idLoadingSlipExt"].ToString());
                    if (tblLoadingSlipExtTODT["bookingId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BookingId = Convert.ToInt32(tblLoadingSlipExtTODT["bookingId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingSlipId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingSlipId = Convert.ToInt32(tblLoadingSlipExtTODT["loadingSlipId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingLayerid"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingLayerid = Convert.ToInt32(tblLoadingSlipExtTODT["loadingLayerid"].ToString());
                    if (tblLoadingSlipExtTODT["materialId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialId = Convert.ToInt32(tblLoadingSlipExtTODT["materialId"].ToString());
                    if (tblLoadingSlipExtTODT["bookingExtId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BookingExtId = Convert.ToInt32(tblLoadingSlipExtTODT["bookingExtId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingQty"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingQty = Convert.ToDouble(tblLoadingSlipExtTODT["loadingQty"].ToString());
                    if (tblLoadingSlipExtTODT["bundles"] != DBNull.Value)
                        tblLoadingSlipExtTONew.Bundles = Convert.ToDouble(tblLoadingSlipExtTODT["bundles"].ToString());
       
                    tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTONew);
                }
            }
            return tblLoadingSlipExtTOList;
        }
        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExt()
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
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(String loadingIds, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                // cmdSelect.CommandText = SqlSelectQuery() + " WHERE loadingSlipId IN (SELECT idLoadingSlip FROM tempLoadingSlip WHERE loadingId IN(" + loadingIds + ") )";

                // Vaibhav [09-Jan-2018] Commented and added to select finalLoadingSlip

                cmdSelect.CommandText = " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc,brand.brandName as brandDesc, prodSpec.isWeighingAllow " +
                                  " ,item.itemName,prodClass.displayName" +
                                  "  FROM tempLoadingSlipExt loadDtl " +
                                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                                  "  LEFT JOIN tblMaterial material " +
                                  "  ON material.idMaterial = loadDtl.materialId " +
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId" +
                                  " LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId " +
                                  " LEFT JOIN tblProdClassification prodClass ON item.prodClassId = prodClass.idProdClass " +
                                  " WHERE loadingSlipId IN (SELECT idLoadingSlip FROM tempLoadingSlip WHERE loadingId IN(" + loadingIds + ") )" +

                                  " UNION ALL " +

                                   " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc,brand.brandName as brandDesc, prodSpec.isWeighingAllow " +
                                  "  ,item.itemName,prodClass.displayName " +
                                  "  FROM finalLoadingSlipExt loadDtl " +
                                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                                  "  LEFT JOIN tblMaterial material " +
                                  "  ON material.idMaterial = loadDtl.materialId " +
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId" +
                                  " LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId " +
                                  " LEFT JOIN tblProdClassification prodClass ON item.prodClassId = prodClass.idProdClass " +
                                  " WHERE loadingSlipId IN (SELECT idLoadingSlip FROM finalLoadingSlip WHERE loadingId IN(" + loadingIds + ") )";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromBookingId(String BookingIds, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.bookingId IN ( " + BookingIds + " ) ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public Dictionary<Int32, Double> SelectLoadingQuotaWiseApprovedLoadingQtyDCT(String loadingQuotaIds, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String statusIds = (int)Constants.TranStatusE.LOADING_CONFIRM + "," + (int)Constants.TranStatusE.LOADING_COMPLETED + "," +
                         (int)Constants.TranStatusE.LOADING_DELIVERED + "," + (int)Constants.TranStatusE.LOADING_GATE_IN + "," +
                         (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING;
            try
            {
                cmdSelect.CommandText = " SELECT ext.loadingQuotaId, SUM(loadingQty) approvedLoadingQty " +
                                        " FROM tempLoadingSlipExt ext " +
                                        " LEFT JOIN tempLoadingSlip slip ON slip.idLoadingSlip = ext.loadingSlipId " +
                                        " LEFT JOIN tempLoading loading ON loading.idLoading = slip.loadingId " +
                                        " WHERE ext.loadingQuotaId IN(" + loadingQuotaIds + ") AND loading.statusId IN(" + statusIds + ") " +
                                        " GROUP BY ext.loadingQuotaId";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (sqlReader != null)
                {
                    Dictionary<Int32, Double> DCT = new Dictionary<int, double>();
                    while (sqlReader.Read())
                    {
                        Int32 loadingQuotaId = 0;
                        Double approvedQty = 0;

                        if (sqlReader["loadingQuotaId"] != DBNull.Value)
                            loadingQuotaId = Convert.ToInt32(sqlReader["loadingQuotaId"].ToString());
                        if (sqlReader["approvedLoadingQty"] != DBNull.Value)
                            approvedQty = Convert.ToDouble(sqlReader["approvedLoadingQty"].ToString());

                        if (loadingQuotaId > 0 && approvedQty > 0)
                            DCT.Add(loadingQuotaId, approvedQty);
                    }

                    return DCT;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExt(Int32 prodCatId)
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
                                        " WHERE idProdCat=" + prodCatId;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblLoadingSlipExtTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<Models.TblLoadingSlipExtTO>();

                while (tblLoadingSlipExtTODT.Read())
                {
                    TblLoadingSlipExtTO tblLoadingSlipExtTONew = new TblLoadingSlipExtTO();
                    if (tblLoadingSlipExtTODT["materialId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialId = Convert.ToInt32(tblLoadingSlipExtTODT["materialId"].ToString());
                    if (tblLoadingSlipExtTODT["materialSubType"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialDesc = Convert.ToString(tblLoadingSlipExtTODT["materialSubType"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatId = Convert.ToInt32(tblLoadingSlipExtTODT["prodCatId"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecId = Convert.ToInt32(tblLoadingSlipExtTODT["prodSpecId"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatDesc = Convert.ToString(tblLoadingSlipExtTODT["prodCatDesc"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecDesc = Convert.ToString(tblLoadingSlipExtTODT["prodSpecDesc"].ToString());

                    tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTONew);
                }

                return tblLoadingSlipExtTOList;
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

        public List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExt(Int32 prodCatId, int prodSpecId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String whereCond = string.Empty;
            try
            {
                conn.Open();
                if (prodCatId > 0 && prodSpecId > 0)
                {
                    whereCond = "WHERE idProdCat=" + prodCatId + " AND idProdSpec=" + prodSpecId;
                }
                cmdSelect.CommandText = " SELECT tblMaterial.idMaterial AS materialId, tblMaterial.materialSubType , " +
                                        " dimProdCat.idProdCat AS prodCatId,dimProdCat.prodCateDesc prodCatDesc, " +
                                        " dimProdSpec.idProdSpec AS prodSpecId ,dimProdSpec.prodSpecDesc " +
                                        " ,dimBrand.idBrand AS brandId, dimBrand.brandName AS brandDesc " +
                                        " FROM tblMaterial " +
                                        " FULL OUTER JOIN dimProdCat ON 1 = 1 " +
                                        " FULL OUTER JOIN dimProdSpec ON 1 = 1" +
                                        " FULL OUTER JOIN dimBrand ON 1 = 1" +
                                        whereCond;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblLoadingSlipExtTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<Models.TblLoadingSlipExtTO>();

                while (tblLoadingSlipExtTODT.Read())
                {
                    TblLoadingSlipExtTO tblLoadingSlipExtTONew = new TblLoadingSlipExtTO();
                    if (tblLoadingSlipExtTODT["materialId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialId = Convert.ToInt32(tblLoadingSlipExtTODT["materialId"].ToString());
                    if (tblLoadingSlipExtTODT["materialSubType"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialDesc = Convert.ToString(tblLoadingSlipExtTODT["materialSubType"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatId = Convert.ToInt32(tblLoadingSlipExtTODT["prodCatId"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecId = Convert.ToInt32(tblLoadingSlipExtTODT["prodSpecId"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatDesc = Convert.ToString(tblLoadingSlipExtTODT["prodCatDesc"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecDesc = Convert.ToString(tblLoadingSlipExtTODT["prodSpecDesc"].ToString());
                    if (tblLoadingSlipExtTODT["brandId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BrandId = Convert.ToInt32(tblLoadingSlipExtTODT["brandId"].ToString());
                    if (tblLoadingSlipExtTODT["brandDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BrandDesc = Convert.ToString(tblLoadingSlipExtTODT["brandDesc"].ToString());

                    //[05-09-2018]Vijaymala added to set regular or other item name 
                    if (tblLoadingSlipExtTONew.ProdCatDesc != String.Empty && tblLoadingSlipExtTONew.ProdSpecDesc != String.Empty
                         && tblLoadingSlipExtTONew.MaterialDesc != String.Empty)
                    {
                        tblLoadingSlipExtTONew.DisplayName = tblLoadingSlipExtTONew.MaterialDesc + "-" + tblLoadingSlipExtTONew.ProdCatDesc + "-" + tblLoadingSlipExtTONew.ProdSpecDesc + "(" + tblLoadingSlipExtTONew.BrandDesc + ")";
                    }

                    tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTONew);
                }

                return tblLoadingSlipExtTOList;
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

        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExt(Int32 loadingSlipId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId=" + loadingSlipId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExt(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId=" + loadingSlipId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
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


        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtByWeighingMeasureId(Int32 WeighingMeasureId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE weightMeasureId =" + WeighingMeasureId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
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

        public TblLoadingSlipExtTO SelectTblLoadingSlipExt(Int32 idLoadingSlipExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);

            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectTblLoadingSlipExt(idLoadingSlipExt, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public TblLoadingSlipExtTO SelectTblLoadingSlipExt(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idLoadingSlipExt = " + idLoadingSlipExt + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null) sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblLoadingSlipExtTO> ConvertDTToList(SqlDataReader tblLoadingSlipExtTODT)
        {
            List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
            if (tblLoadingSlipExtTODT != null)
            {
                while (tblLoadingSlipExtTODT.Read())
                {
                    TblLoadingSlipExtTO tblLoadingSlipExtTONew = new TblLoadingSlipExtTO();
                    if (tblLoadingSlipExtTODT["idLoadingSlipExt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.IdLoadingSlipExt = Convert.ToInt32(tblLoadingSlipExtTODT["idLoadingSlipExt"].ToString());
                    if (tblLoadingSlipExtTODT["bookingId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BookingId = Convert.ToInt32(tblLoadingSlipExtTODT["bookingId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingSlipId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingSlipId = Convert.ToInt32(tblLoadingSlipExtTODT["loadingSlipId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingLayerid"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingLayerid = Convert.ToInt32(tblLoadingSlipExtTODT["loadingLayerid"].ToString());
                    if (tblLoadingSlipExtTODT["materialId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialId = Convert.ToInt32(tblLoadingSlipExtTODT["materialId"].ToString());
                    if (tblLoadingSlipExtTODT["bookingExtId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BookingExtId = Convert.ToInt32(tblLoadingSlipExtTODT["bookingExtId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingQty"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingQty = Convert.ToDouble(tblLoadingSlipExtTODT["loadingQty"].ToString());
                    if (tblLoadingSlipExtTODT["layerDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingLayerDesc = Convert.ToString(tblLoadingSlipExtTODT["layerDesc"].ToString());
                    if (tblLoadingSlipExtTODT["materialSubType"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialDesc = Convert.ToString(tblLoadingSlipExtTODT["materialSubType"].ToString());

                    if (tblLoadingSlipExtTODT["quotaBforeLoading"] != DBNull.Value)
                        tblLoadingSlipExtTONew.QuotaBforeLoading = Convert.ToDouble(tblLoadingSlipExtTODT["quotaBforeLoading"].ToString());
                    if (tblLoadingSlipExtTODT["quotaAfterLoading"] != DBNull.Value)
                        tblLoadingSlipExtTONew.QuotaAfterLoading = Convert.ToDouble(tblLoadingSlipExtTODT["quotaAfterLoading"].ToString());

                    if (tblLoadingSlipExtTODT["prodCatId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatId = Convert.ToInt32(tblLoadingSlipExtTODT["prodCatId"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecId = Convert.ToInt32(tblLoadingSlipExtTODT["prodSpecId"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatDesc = Convert.ToString(tblLoadingSlipExtTODT["prodCatDesc"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecDesc = Convert.ToString(tblLoadingSlipExtTODT["prodSpecDesc"].ToString());
                    if (tblLoadingSlipExtTODT["loadingQuotaId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingQuotaId = Convert.ToInt32(tblLoadingSlipExtTODT["loadingQuotaId"].ToString());

                    if (tblLoadingSlipExtTODT["bundles"] != DBNull.Value)
                        tblLoadingSlipExtTONew.Bundles = Convert.ToDouble(tblLoadingSlipExtTODT["bundles"].ToString());
                    if (tblLoadingSlipExtTODT["parityDtlId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ParityDtlId = Convert.ToInt32(tblLoadingSlipExtTODT["parityDtlId"].ToString());
                    if (tblLoadingSlipExtTODT["ratePerMT"] != DBNull.Value)
                        tblLoadingSlipExtTONew.RatePerMT = Convert.ToDouble(tblLoadingSlipExtTODT["ratePerMT"].ToString());

                    if (tblLoadingSlipExtTODT["rateCalcDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.RateCalcDesc = Convert.ToString(tblLoadingSlipExtTODT["rateCalcDesc"].ToString());

                    if (tblLoadingSlipExtTODT["loadedWeight"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadedWeight = Convert.ToDouble(tblLoadingSlipExtTODT["loadedWeight"]);
                    if (tblLoadingSlipExtTODT["loadedBundles"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadedBundles = Convert.ToDouble(tblLoadingSlipExtTODT["loadedBundles"]);
                    if (tblLoadingSlipExtTODT["calcTareWeight"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CalcTareWeight = Convert.ToDouble(tblLoadingSlipExtTODT["calcTareWeight"]);
                    if (tblLoadingSlipExtTODT["weightMeasureId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.WeightMeasureId = Convert.ToInt32(tblLoadingSlipExtTODT["weightMeasureId"]);
                    if (tblLoadingSlipExtTODT["isAllowWeighingMachine"] != DBNull.Value)
                        tblLoadingSlipExtTONew.IsAllowWeighingMachine = Convert.ToInt32(tblLoadingSlipExtTODT["isAllowWeighingMachine"]);
                    if (tblLoadingSlipExtTODT["updatedBy"] != DBNull.Value)
                        tblLoadingSlipExtTONew.UpdatedBy = Convert.ToInt32(tblLoadingSlipExtTODT["updatedBy"]);
                    if (tblLoadingSlipExtTODT["updatedOn"] != DBNull.Value)
                        tblLoadingSlipExtTONew.UpdatedOn = Convert.ToDateTime(tblLoadingSlipExtTODT["updatedOn"]);
                    if (tblLoadingSlipExtTODT["cdStructureId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CdStructureId = Convert.ToInt32(tblLoadingSlipExtTODT["cdStructureId"]);
                    if (tblLoadingSlipExtTODT["cdStructure"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CdStructure = Convert.ToDouble(tblLoadingSlipExtTODT["cdStructure"]);
                    if (tblLoadingSlipExtTODT["prodItemDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdItemDesc = Convert.ToString(tblLoadingSlipExtTODT["prodItemDesc"]);
                    if (tblLoadingSlipExtTODT["prodItemId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdItemId = Convert.ToInt32(tblLoadingSlipExtTODT["prodItemId"]);
                    if (tblLoadingSlipExtTODT["taxableRateMT"] != DBNull.Value)
                        tblLoadingSlipExtTONew.TaxableRateMT = Convert.ToDouble(tblLoadingSlipExtTODT["taxableRateMT"]);

                    if (tblLoadingSlipExtTODT["freExpOtherAmt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.FreExpOtherAmt = Convert.ToDouble(tblLoadingSlipExtTODT["freExpOtherAmt"]);
                    if (tblLoadingSlipExtTODT["cdApplicableAmt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CdApplicableAmt = Convert.ToDouble(tblLoadingSlipExtTODT["cdApplicableAmt"]);

                    if (tblLoadingSlipExtTODT["brandId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BrandId = Convert.ToInt32(tblLoadingSlipExtTODT["brandId"].ToString());
                    if (tblLoadingSlipExtTODT["brandDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BrandDesc = Convert.ToString(tblLoadingSlipExtTODT["brandDesc"].ToString());
                    if (tblLoadingSlipExtTODT["isWeighingAllow"] != DBNull.Value)
                        tblLoadingSlipExtTONew.IsWeighingAllow = Convert.ToInt32(tblLoadingSlipExtTODT["isWeighingAllow"]);


                    if (tblLoadingSlipExtTODT["compartmentId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CompartmentId = Convert.ToInt32(tblLoadingSlipExtTODT["compartmentId"]);

                    //Priyanka [28-05-2018]
                    if (tblLoadingSlipExtTODT["mstLoadedBundles"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MstLoadedBundles = Convert.ToDouble(tblLoadingSlipExtTODT["mstLoadedBundles"]);

                    //[05-09-2018]Vijaymala added to set regular or other item name 
                    if (tblLoadingSlipExtTODT["itemName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ItemName = Convert.ToString(tblLoadingSlipExtTODT["itemName"].ToString());

                    if (tblLoadingSlipExtTODT["displayName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.DisplayName = Convert.ToString(tblLoadingSlipExtTODT["displayName"].ToString());

                    if (tblLoadingSlipExtTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ModbusRefId = Convert.ToInt32(tblLoadingSlipExtTODT["modbusRefId"]);

                    if (tblLoadingSlipExtTONew.ProdItemId > 0)
                    {
                        tblLoadingSlipExtTONew.DisplayName = tblLoadingSlipExtTONew.DisplayName + "-" + tblLoadingSlipExtTONew.ItemName;
                    }
                    else
                    {
                        tblLoadingSlipExtTONew.DisplayName = tblLoadingSlipExtTONew.MaterialDesc + "-" + tblLoadingSlipExtTONew.ProdCatDesc + "-" + tblLoadingSlipExtTONew.ProdSpecDesc
                            + "(" + tblLoadingSlipExtTONew.BrandDesc + ")";
                    }

                    //if (tblLoadingSlipExtTONew.MstLoadedBundles > 0)
                    //    tblLoadingSlipExtTONew.DisplayLoadedBundles += tblLoadingSlipExtTONew.MstLoadedBundles.ToString() + ',';
                    //if (tblLoadingSlipExtTONew.LoadedBundles > 0)
                    //    tblLoadingSlipExtTONew.DisplayLoadedBundles += tblLoadingSlipExtTONew.LoadedBundles.ToString() + ',';

                    //if (!String.IsNullOrEmpty(tblLoadingSlipExtTONew.DisplayLoadedBundles))
                    //    tblLoadingSlipExtTONew.DisplayLoadedBundles = tblLoadingSlipExtTONew.DisplayLoadedBundles.TrimEnd(',');

                    //tblLoadingSlipExtTONew.DisplayLoadedBundles = Convert.ToString(tblLoadingSlipExtTONew.MstLoadedBundles.ToString()
                    //                                           + "," + tblLoadingSlipExtTONew.LoadedBundles.ToString());
                    tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTONew);
                }
            }
            return tblLoadingSlipExtTOList;
        }


        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtByDate(DateTime frmDt, DateTime toDt, String statusStr,string selectedOrgStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string sqlQuery = string.Empty;
            try
            {
                conn.Open();
                ////sqlQuery = SqlSelectQuery() + " LEFT JOIN tempLoadingSlip loadingSlip  ON loadingSlip.idLoadingSlip = loadDtl.loadingSlipId  " +
                ////     " LEFT JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId ";

                //// vaibhav [09-Jan-2018] Commented and added to select from finalLoadingSlipExt.
                //sqlQuery = " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                //                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc,brand.brandName as brandDesc, prodSpec.isWeighingAllow " +
                //                  "  FROM tempLoadingSlipExt loadDtl " +
                //                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                //                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                //                  "  LEFT JOIN tblMaterial material " +
                //                  "  ON material.idMaterial = loadDtl.materialId " +
                //                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                //                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId" +
                //                  " LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                //    " LEFT JOIN tempLoadingSlip loadingSlip  ON loadingSlip.idLoadingSlip = loadDtl.loadingSlipId  " +
                //    " LEFT JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId " +

                //     " UNION ALL " +

                //      " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                //                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc,brand.brandName as brandDesc, prodSpec.isWeighingAllow " +
                //                  "  FROM finalLoadingSlipExt loadDtl " +
                //                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                //                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                //                  "  LEFT JOIN tblMaterial material " +
                //                  "  ON material.idMaterial = loadDtl.materialId " +
                //                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                //                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId" +
                //                  " LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                //     " LEFT JOIN finalLoadingSlip loadingSlip  ON loadingSlip.idLoadingSlip = loadDtl.loadingSlipId  " +
                //     " LEFT JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId ";

                ////" select loadingSlipExt.*,org.firmName as cnfName,orginfo.firmName  as dealerName , " +
                ////         " material.materialSubType,prodCat.prodCateDesc,prodSpec.prodSpecDesc from tblLoadingSlipExt loadingSlipExt " +
                ////         " LEFT JOIN tblLoadingSlip loadingSlip  ON loadingSlip.idLoadingSlip = loadingSlipExt.loadingSlipId  " +
                ////         " LEFT JOIN tblOrganization orginfo ON orginfo.idOrganization = loadingSlip.dealerOrgId  " +
                ////         " LEFT JOIN tblLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                ////         " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId  " +
                ////         " LEFT JOIN tblMaterial material ON material.idMaterial = loadingSlipExt.materialId  " +
                ////         " LEFT JOIN dimProdCat prodCat ON prodCat.idProdCat = loadingSlipExt.prodCatId " +
                ////         " LEFT JOIN dimProdSpec prodSpec ON material.idMaterial = loadingSlipExt.materialId  " +
                ////         " LEFT JOIN dimBrand brand ON brand.idBrand = loadingSlipExt.brandId  ";



                //cmdSelect.CommandText = sqlQuery + " WHERE  CAST(loadingSlip.statusDate AS DATE) BETWEEN @fromDate AND @toDate";
                //cmdSelect.Connection = conn;

                //sqlQuery = " SELECT SUM(loadDtl.loadingQty) AS loadingQty," +
                //    "material.materialSubType  , prodCat.prodCateDesc AS prodCatDesc , " +
                //    "prodSpec.prodSpecDesc,brand.brandName as brandDesc   " +
                //    "FROM tempLoadingSlipExt loadDtl" +
                //    " LEFT JOIN dimLoadingLayers loadLayers ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer" +
                //    " LEFT JOIN tblMaterial material   ON material.idMaterial = loadDtl.materialId" +
                //    " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat = loadDtl.prodCatId " +
                //    "LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec = loadDtl.prodSpecId " +
                //    "LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                //    "LEFT JOIN tempLoadingSlip loadingSlip  ON loadingSlip.idLoadingSlip = loadDtl.loadingSlipId " +
                //    "LEFT JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                //    " Group BY material.materialSubType , prodCat.prodCateDesc ,prodSpec.prodSpecDesc,brand.brandName "+

                //    "UNION ALL SELECT SUM(loadDtl.loadingQty) AS LoadingQty," +
                //    "material.materialSubType  , prodCat.prodCateDesc AS prodCatDesc , " +
                //    "prodSpec.prodSpecDesc,brand.brandName as brandDesc  " +
                //    " FROM finalLoadingSlipExt loadDtl " +
                //    "LEFT JOIN dimLoadingLayers loadLayers ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                //    "LEFT JOIN tblMaterial material   ON material.idMaterial = loadDtl.materialId " +
                //    "LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat = loadDtl.prodCatId " +
                //    "LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec = loadDtl.prodSpecId " +
                //    "LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                //    "LEFT JOIN finalLoadingSlip loadingSlip  ON loadingSlip.idLoadingSlip = loadDtl.loadingSlipId " +
                //    "LEFT JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId ";

                //cmdSelect.CommandText = sqlQuery + " WHERE  CAST(loadingSlip.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND ( "+
                //                        " loadingSlip.statusId="+Convert.ToInt32(Constants.TranStatusE.LOADING_COMPLETED)+
                //                        "  OR loadingSlip.statusId="+ Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED)+" )";
                //cmdSelect.CommandText += " Group BY material.materialSubType , prodCat.prodCateDesc ,prodSpec.prodSpecDesc,brand.brandName "+
                //                        "  ORDER BY brandDesc, prodCatDesc, prodSpecDesc,materialSubType";

                String whereCondition = " WHERE  CAST(loadingSlip.statusDate AS DATE) BETWEEN @fromDate AND @toDate ";

                if (!String.IsNullOrEmpty(statusStr))
                {
                    whereCondition += " AND loadingSlip.statusId IN (" + statusStr + ")";
                }
                if(!string.IsNullOrEmpty(selectedOrgStr))
                {
                    whereCondition += " AND loadingSlip.fromOrgId IN (" + selectedOrgStr + ")";
                }
                //[05-09-2018]Vijaymala modified to change statistic report for regular or other loading
                sqlQuery = "select SUM(AA.loadingQty) AS loadingQty ,AA.materialSubType , AA.brandDesc ,AA.prodCatDesc,AA.prodSpecDesc," +
                           " AA.itemName,AA.prodItemId,AA.displayName ,AA.sequenceNo   from " +
                        " ( " +
                         " SELECT SUM(loadDtl.loadingQty) AS loadingQty, material.materialSubType, prodCat.prodCateDesc AS prodCatDesc, " +
                         " prodSpec.prodSpecDesc, brand.brandName as brandDesc,item.itemName,loadDtl.prodItemId,prodClass.displayName," +
                         " material.sequenceNo" +
                         " FROM tempLoadingSlipExt loadDtl " +
                         " LEFT JOIN dimLoadingLayers loadLayers ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                         " LEFT JOIN tblMaterial material   ON material.idMaterial = loadDtl.materialId " +
                         " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat = loadDtl.prodCatId " +
                         " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec = loadDtl.prodSpecId " +
                         " LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                         " LEFT JOIN tempLoadingSlip loadingSlip  ON loadingSlip.idLoadingSlip = loadDtl.loadingSlipId " +
                         " LEFT JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                         " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId " +
                         " LEFT JOIN tblProdClassification prodClass ON item.prodClassId = prodClass.idProdClass " +
                         " " + whereCondition +
                         " Group BY  material.materialSubType, prodCat.prodCateDesc, prodSpec.prodSpecDesc, brand.brandName" +
                         " ,item.itemName,loadDtl.prodItemId,prodClass.displayName ,material.sequenceNo " +
                         " UNION ALL " +
                         " SELECT  SUM(loadDtl.loadingQty) AS LoadingQty, material.materialSubType, " +
                         " prodCat.prodCateDesc AS prodCatDesc, prodSpec.prodSpecDesc, brand.brandName as brandDesc " +
                         " ,item.itemName,loadDtl.prodItemId,prodClass.displayName,material.sequenceNo " +
                         " FROM finalLoadingSlipExt loadDtl LEFT JOIN dimLoadingLayers loadLayers " +
                         " ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                         " LEFT JOIN tblMaterial material   ON material.idMaterial = loadDtl.materialId " +
                         " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat = loadDtl.prodCatId " +
                         " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec = loadDtl.prodSpecId " +
                         " LEFT JOIN dimBrand brand ON brand.idBrand = loadDtl.brandId " +
                         " LEFT JOIN finalLoadingSlip loadingSlip  ON loadingSlip.idLoadingSlip = loadDtl.loadingSlipId " +
                         " LEFT JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                         " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId " +
                         " LEFT JOIN tblProdClassification prodClass ON item.prodClassId = prodClass.idProdClass " +
                        " " + whereCondition +
                         "  Group BY material.materialSubType, prodCat.prodCateDesc, prodSpec.prodSpecDesc, brand.brandName " +
                         "  ,item.itemName,loadDtl.prodItemId,prodClass.displayName,material.sequenceNo" +
                         "  ) as AA " +
                         "  Group BY  AA.materialSubType , AA.brandDesc ,AA.prodCatDesc,AA.prodSpecDesc " +
                         "  ,AA.itemName,AA.prodItemId,AA.displayName,AA.sequenceNo order by prodItemId,sequenceNo asc";


                cmdSelect.CommandText = sqlQuery;



                cmdSelect.Connection = conn;

                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
                SqlDataReader tblLoadingSlipExtTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = new List<TblLoadingSlipExtTO>();

                if (tblLoadingSlipExtTODT != null)
                {
                    while (tblLoadingSlipExtTODT.Read())
                    {
                        TblLoadingSlipExtTO tblLoadingSlipExtTONew = new TblLoadingSlipExtTO();
                        if (tblLoadingSlipExtTODT["loadingQty"] != DBNull.Value)
                            tblLoadingSlipExtTONew.LoadingQty = Convert.ToDouble(tblLoadingSlipExtTODT["loadingQty"].ToString());
                        
                        //if (tblLoadingSlipExtTODT["layerDesc"] != DBNull.Value)
                        //    tblLoadingSlipExtTONew.LoadingLayerDesc = Convert.ToString(tblLoadingSlipExtTODT["layerDesc"].ToString());
                        if (tblLoadingSlipExtTODT["materialSubType"] != DBNull.Value)
                            tblLoadingSlipExtTONew.MaterialDesc = Convert.ToString(tblLoadingSlipExtTODT["materialSubType"].ToString());
                        if (tblLoadingSlipExtTODT["prodCatDesc"] != DBNull.Value)
                            tblLoadingSlipExtTONew.ProdCatDesc = Convert.ToString(tblLoadingSlipExtTODT["prodCatDesc"].ToString());
                        if (tblLoadingSlipExtTODT["prodSpecDesc"] != DBNull.Value)
                            tblLoadingSlipExtTONew.ProdSpecDesc = Convert.ToString(tblLoadingSlipExtTODT["prodSpecDesc"].ToString());
                        if (tblLoadingSlipExtTODT["brandDesc"] != DBNull.Value)
                            tblLoadingSlipExtTONew.BrandDesc = Convert.ToString(tblLoadingSlipExtTODT["brandDesc"].ToString());
                        
                        //[05-09-2018]Vijaymala added to set regular or other item name 
                        if (tblLoadingSlipExtTODT["prodItemId"] != DBNull.Value)
                            tblLoadingSlipExtTONew.ProdItemId = Convert.ToInt32(tblLoadingSlipExtTODT["prodItemId"]);

                        if (tblLoadingSlipExtTODT["itemName"] != DBNull.Value)
                            tblLoadingSlipExtTONew.ItemName = Convert.ToString(tblLoadingSlipExtTODT["itemName"].ToString());

                        if (tblLoadingSlipExtTODT["displayName"] != DBNull.Value)
                            tblLoadingSlipExtTONew.DisplayName = Convert.ToString(tblLoadingSlipExtTODT["displayName"].ToString());

                        if (tblLoadingSlipExtTONew.ProdItemId > 0)
                        {
                            tblLoadingSlipExtTONew.DisplayName = tblLoadingSlipExtTONew.DisplayName + "-" + tblLoadingSlipExtTONew.ItemName;
                        }
                        else
                        {
                            tblLoadingSlipExtTONew.DisplayName = tblLoadingSlipExtTONew.MaterialDesc + "-" + tblLoadingSlipExtTONew.ProdCatDesc 
                                + "-" + tblLoadingSlipExtTONew.ProdSpecDesc + "(" + tblLoadingSlipExtTONew.BrandDesc + ")"; ;
                        }

                        if (tblLoadingSlipExtTODT["sequenceNo"] != DBNull.Value)
                            tblLoadingSlipExtTONew.SequenceNo = Convert.ToInt32(tblLoadingSlipExtTODT["sequenceNo"].ToString());
                        list.Add(tblLoadingSlipExtTONew);
                    }
                }
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

        /// <summary>
        ///[03-01-2017] Vijaymala Added :To get loading slip extension list
        /// </summary>
        /// <param name="idLoadingSlipExt"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblLoadingSlipExtTO> SelectTblLoadingSlipExtByIds(String idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idLoadingSlipExt IN (" + idLoadingSlipExt + " )";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
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
        #endregion

        #region Insertion
        public int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingSlipExtTO, cmdInsert);
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

        public int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingSlipExtTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingSlipExt]( " +
                            "  [bookingId]" +
                            " ,[loadingSlipId]" +
                            " ,[loadingLayerid]" +
                            " ,[materialId]" +
                            " ,[bookingExtId]" +
                            " ,[loadingQty]" +
                            " ,[prodCatId]" +
                            " ,[prodSpecId]" +
                            " ,[quotaBforeLoading]" +
                            " ,[quotaAfterLoading]" +
                            " ,[loadingQuotaId]" +
                            " ,[bundles]" +
                            " ,[parityDtlId]" +
                            " ,[ratePerMT]" +
                            " ,[rateCalcDesc]" +
                            " ,[loadedWeight]" +
                            " ,[loadedBundles]" +
                            " ,[calcTareWeight]" +
                            " ,[weightMeasureId]" +
                            " ,[updatedBy]" +
                            " ,[updatedOn]" +
                            " ,[cdStructureId]" +
                            " ,[cdStructure]" +
                            ", [isAllowWeighingMachine] " +
                            " ,[prodItemDesc]" +
                            " ,[prodItemId]" +
                            " ,[taxableRateMT]" +
                            " ,[freExpOtherAmt]" +
                            " ,[cdApplicableAmt]" +
                            " ,[brandId]" +
                            " ,[mstLoadedBundles]" +                   //Priyanka [28-05-2018]
                            " ,[compartmentId]" +
                             ",[modbusRefId]" +
                            " )" +
                " VALUES (" +
                            "  @BookingId " +
                            " ,@LoadingSlipId " +
                            " ,@LoadingLayerid " +
                            " ,@MaterialId " +
                            " ,@BookingExtId " +
                            " ,@LoadingQty " +
                            " ,@prodCatId " +
                            " ,@prodSpecId " +
                            " ,@quotaBforeLoading " +
                            " ,@quotaAfterLoading " +
                            " ,@loadingQuotaId " +
                            " ,@bundles " +
                            " ,@parityDtlId " +
                            " ,@ratePerMT " +
                            " ,@rateCalcDesc " +
                            " ,@loadedWeight " +
                            " ,@loadedBundles " +
                            " ,@calcTareWeight " +
                            " ,@weightMeasureId " +
                            " ,@updatedBy " +
                            " ,@updatedOn " +
                            " ,@cdStructureId " +
                            " ,@cdStructure " +
                            " ,@isAllowWeighingMachine" +
                            " ,@prodItemDesc " +
                            " ,@prodItemId " +
                            " ,@taxableRateMT " +
                            " ,@freExpOtherAmt " +
                            " ,@cdApplicableAmt " +
                            " ,@BrandId " +
                            " ,@mstLoadedBundles" +
                            " ,@CompartmentId " +
                             " ,@modbusRefId" +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            // cmdInsert.Parameters.Add("@IdLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BookingId);
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingSlipId;
            cmdInsert.Parameters.Add("@LoadingLayerid", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingLayerid;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.MaterialId);
            cmdInsert.Parameters.Add("@BookingExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BookingExtId);
            cmdInsert.Parameters.Add("@LoadingQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.LoadingQty;
            cmdInsert.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdCatId);
            cmdInsert.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdSpecId);
            cmdInsert.Parameters.Add("@quotaBforeLoading", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.QuotaBforeLoading;
            cmdInsert.Parameters.Add("@quotaAfterLoading", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.QuotaAfterLoading;
            cmdInsert.Parameters.Add("@loadingQuotaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadingQuotaId);
            cmdInsert.Parameters.Add("@bundles", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.Bundles;
            cmdInsert.Parameters.Add("@parityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ParityDtlId);
            cmdInsert.Parameters.Add("@ratePerMT", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipExtTO.RatePerMT;
            cmdInsert.Parameters.Add("@rateCalcDesc", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.RateCalcDesc);
            cmdInsert.Parameters.Add("@loadedWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedWeight);
            cmdInsert.Parameters.Add("@loadedBundles", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedBundles);
            cmdInsert.Parameters.Add("@calcTareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CalcTareWeight);
            cmdInsert.Parameters.Add("@weightMeasureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.WeightMeasureId);
            cmdInsert.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.UpdatedBy);
            cmdInsert.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.UpdatedOn);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructureId);
            cmdInsert.Parameters.Add("@cdStructure", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructure);
            cmdInsert.Parameters.Add("@isAllowWeighingMachine", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.IsAllowWeighingMachine);
            cmdInsert.Parameters.Add("@prodItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemDesc);
            cmdInsert.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemId);
            cmdInsert.Parameters.Add("@taxableRateMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.TaxableRateMT);
            cmdInsert.Parameters.Add("@freExpOtherAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.FreExpOtherAmt);
            cmdInsert.Parameters.Add("@cdApplicableAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdApplicableAmt);
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BrandId);

            cmdInsert.Parameters.Add("@CompartmentId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CompartmentId);
            cmdInsert.Parameters.Add("@mstLoadedBundles", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.MstLoadedBundles);         //Priyanka [28-05-2018]
            cmdInsert.Parameters.Add("@modbusRefId", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ModbusRefId);


            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipExtTO.IdLoadingSlipExt = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation

        //Aniket [13-8-2019]
        public  int UpdateLoadingSlipExtSeqNumber(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = @" UPDATE [tempLoadingSlipExt] SET " +
                            " [weighingSequenceNumber] = @weighingSequenceNumber " +
                            " WHERE [idLoadingSlipExt] = @IdLoadingSlipExt ";

                cmdUpdate.Parameters.Add("@weighingSequenceNumber", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.WeighingSequenceNumber);
                cmdUpdate.Parameters.Add("@IdLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                return cmdUpdate.ExecuteNonQuery();

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
        public int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingSlipExtTO, cmdUpdate);
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

        public int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingSlipExtTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingSlipExt] SET " +
                            "  [bookingId]= @BookingId" +
                            " ,[loadingSlipId]= @LoadingSlipId" +
                            " ,[loadingLayerid]= @LoadingLayerid" +
                            " ,[materialId]= @MaterialId" +
                            //" ,[bookingExtId]= @BookingExtId" +
                            " ,[loadingQty] = @LoadingQty" +
                            " ,[prodCatId] = @prodCatId" +
                            " ,[prodSpecId] = @prodSpecId" +
                            " ,[quotaBforeLoading] = @quotaBforeLoading" +
                            " ,[quotaAfterLoading] = @quotaAfterLoading" +
                            " ,[loadingQuotaId] = @loadingQuotaId" +
                            " ,[bundles] = @bundles" +
                            " ,[parityDtlId] = @parityDtlId" +
                            " ,[ratePerMT] = @ratePerMT" +
                            " ,[rateCalcDesc] = @rateCalcDesc" +
                            " ,[loadedWeight] = @loadedWeight " +
                            " ,[loadedBundles] = @loadedBundles " +
                            " ,[calcTareWeight] = @calcTareWeight " +
                            " ,[weightMeasureId] = @weightMeasureId " +
                            " ,[updatedBy] = @updatedBy " +
                            " ,[updatedOn] = @updatedOn " +
                            " ,[isAllowWeighingMachine] = @isAllowWeighingMachine " +
                            " ,[cdStructureId] = @cdStructureId " +
                            " ,[cdStructure] = @cdStructure " +
                            " ,[prodItemDesc] = @prodItemDesc " +
                            " ,[prodItemId] = @prodItemId " +
                            " ,[taxableRateMT] = @taxableRateMT " +
                            " ,[freExpOtherAmt] = @freExpOtherAmt " +
                            " ,[cdApplicableAmt] = @cdApplicableAmt " +
                            " ,[brandId] = @BrandId" +

                            " ,[mstLoadedBundles] = @mstLoadedBundles " +                 //Priyanka [28-05-2018]
                            " WHERE [idLoadingSlipExt] = @IdLoadingSlipExt ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BookingId);
            cmdUpdate.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingSlipId;
            cmdUpdate.Parameters.Add("@LoadingLayerid", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingLayerid;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.MaterialId);
            //cmdUpdate.Parameters.Add("@BookingExtId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.BookingExtId;
            cmdUpdate.Parameters.Add("@LoadingQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.LoadingQty;
            cmdUpdate.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdCatId);
            cmdUpdate.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@quotaBforeLoading", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.QuotaBforeLoading);
            cmdUpdate.Parameters.Add("@quotaAfterLoading", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.QuotaAfterLoading);
            cmdUpdate.Parameters.Add("@loadingQuotaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadingQuotaId);
            cmdUpdate.Parameters.Add("@bundles", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.Bundles);
            cmdUpdate.Parameters.Add("@parityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ParityDtlId);
            cmdUpdate.Parameters.Add("@ratePerMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.RatePerMT);
            cmdUpdate.Parameters.Add("@rateCalcDesc", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.RateCalcDesc);
            cmdUpdate.Parameters.Add("@loadedWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedWeight);
            cmdUpdate.Parameters.Add("@loadedBundles", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedBundles);
            cmdUpdate.Parameters.Add("@calcTareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CalcTareWeight);
            cmdUpdate.Parameters.Add("@weightMeasureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.WeightMeasureId);
            cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = _iCommon.ServerDateTime;
            cmdUpdate.Parameters.Add("@isAllowWeighingMachine", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IsAllowWeighingMachine;
            cmdUpdate.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructureId);
            cmdUpdate.Parameters.Add("@cdStructure", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructure);
            cmdUpdate.Parameters.Add("@prodItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemDesc);
            cmdUpdate.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemId);
            cmdUpdate.Parameters.Add("@taxableRateMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.TaxableRateMT);
            cmdUpdate.Parameters.Add("@freExpOtherAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.FreExpOtherAmt);
            cmdUpdate.Parameters.Add("@cdApplicableAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdApplicableAmt);
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BrandId);
            cmdUpdate.Parameters.Add("@mstLoadedBundles", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.MstLoadedBundles);     //Priyanka [28-05-2018]

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingSlipExt, cmdDelete);
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

        public int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingSlipExt, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idLoadingSlipExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlipExt] " +
            " WHERE idLoadingSlipExt = " + idLoadingSlipExt + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
