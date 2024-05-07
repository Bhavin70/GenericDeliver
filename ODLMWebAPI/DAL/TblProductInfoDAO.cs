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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SkiaSharp;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ODLMWebAPI.DAL
{
    public class TblProductInfoDAO : ITblProductInfoDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblProductInfoDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = "  SELECT product.*, " +
                                  "  material.materialSubType, " +
                                  "  prodCat.prodCateDesc, " +
                                  "  prodSpec.prodSpecDesc " +
                                  "  FROM tblProductInfo product " +
                                  "  LEFT JOIN tblMaterial material " +
                                  "  ON product.materialId = material.idMaterial " +
                                  "  LEFT JOIN dimProdCat prodCat " +
                                  "  ON prodCat.idProdCat = product.prodCatId " +
                                  "  LEFT JOIN dimProdSpec prodSpec " +
                                  "  ON prodSpec.idProdSpec = product.prodSpecId";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblProductInfoTO> SelectAllTblProductInfo()
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
                List<TblProductInfoTO> list = ConvertDTToList(sqlReader);
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
        public List<TblProductInfoTO> SelectAllTblProductInfo(SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductInfoTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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
        public List<TblProductInfoTO> SelectAllLatestProductInfo(SqlConnection conn, SqlTransaction tran,int CategoryType=1)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                if (CategoryType == 1 )
                {
                    cmdSelect.CommandText = SqlSelectQuery() +
                                       "  INNER JOIN " +
                                       "  ( " +
                                       "  SELECT materialId, prodCatId, prodSpecId, brandId, MAX(createdOn) createdOn " +
                                       "  FROM tblProductInfo " +
                                       "   GROUP BY materialId, prodCatId, prodSpecId, brandId " +
                                       "  ) latest_info " +
                                       "  ON latest_info.materialId = product.materialId " +
                                       "  AND latest_info.prodCatId = product.prodCatId " +
                                       "  AND latest_info.prodSpecId = product.prodSpecId " +
                                       "  AND latest_info.createdOn = product.createdOn" +
                                       " AND latest_info.brandId = product.brandId ";
                }
                else if (CategoryType == 3)
                {
                    string Query = " SELECT product.*, prodCat.prodCateDesc, inch.inch,size.size,thickness.thickness  " +
                                    " FROM tblProductInfo product   " +
                                    " LEFT JOIN dimProdCat prodCat ON prodCat.idProdCat = product.prodCatId " +
                                    " LEFT JOIN tblinch inch ON inch.idInch = product.inchId " +
                                    " LEFT JOIN tblSize size ON size.idSize = product.sizeId " +
                                    " LEFT JOIN tblThickness thickness   ON thickness.idThickness = product.thicknessId  " +
                                    " INNER JOIN   " +
                                    " (SELECT  prodCatId,inchId,sizeId,thicknessId, brandId, MAX(createdOn) createdOn " +
                                    " FROM tblProductInfo   " +
                                    " GROUP BY prodCatId,inchId,sizeId,thicknessId, brandId) " +
                                    " latest_info   " +
                                    " ON latest_info.prodCatId = product.prodCatId  " +
                                    " AND latest_info.inchId = product.inchId  " +
                                    " AND latest_info.sizeId = product.sizeId  " +
                                    " AND latest_info.thicknessId = product.thicknessId " +
                                    " AND latest_info.createdOn = product.createdOn " +
                                    " AND latest_info.brandId = product.brandId ";
                    cmdSelect.CommandText = Query;

                }
                else if (CategoryType == 4)
                {
                    string Query = " SELECT product.*, prodCat.prodCateDesc, strips.grade,size.size,thickness.thickness  " +
                                    " FROM tblProductInfo product    " +
                                    " LEFT JOIN dimProdCat prodCat ON prodCat.idProdCat = product.prodCatId " +
                                    " LEFT JOIN tblStrips strips ON strips.idStrip = product.stripId " +
                                    " LEFT JOIN tblSize size ON size.idSize = product.sizeId " +
                                    " LEFT JOIN tblThickness thickness   ON thickness.idThickness = product.thicknessId  " +
                                    " INNER JOIN   " +
                                    " (SELECT  prodCatId,stripId,sizeId,thicknessId, brandId, MAX(createdOn) createdOn " +
                                    " FROM tblProductInfo   " +
                                    " GROUP BY prodCatId,stripId,sizeId,thicknessId, brandId) " +
                                    " latest_info   " +
                                    " ON latest_info.prodCatId = product.prodCatId  " +
                                    " AND latest_info.stripId = product.stripId  " +
                                    " AND latest_info.sizeId = product.sizeId " +
                                    " AND latest_info.thicknessId = product.thicknessId " +
                                    " AND latest_info.createdOn = product.createdOn " +
                                    " AND latest_info.brandId = product.brandId ";
                    cmdSelect.CommandText = Query;

                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductInfoTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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
        public List<TblProductInfoTO> SelectTblProductInfoLatest()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() +
                                       "  INNER JOIN " +
                                       "  ( " +
                                       "  SELECT materialId, prodCatId, prodSpecId, brandId, MAX(createdOn) createdOn " +
                                       "  FROM tblProductInfo " +
                                       "   GROUP BY materialId, prodCatId, prodSpecId, brandId " +
                                       "  ) latest_info " +
                                       "  ON latest_info.materialId = product.materialId " +
                                       "  AND latest_info.prodCatId = product.prodCatId " +
                                       "  AND latest_info.prodSpecId = product.prodSpecId " +
                                       "  AND latest_info.createdOn = product.createdOn" +
                                       " AND latest_info.brandId = product.brandId ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductInfoTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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
        public TblProductInfoTO SelectTblProductInfo(Int32 idProduct)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idProduct = " + idProduct +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductInfoTO> list = ConvertDTToList(reader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblProductInfoTO> SelectEmptyProductDetailsTemplate(int prodCatId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT idProdCat AS prodCatId, idBrand AS brandId , prodCateDesc, prodSpec.idProdSpec AS prodSpecId,prodSpecDesc, idMaterial AS materialId,materialSubType,  " +
                                        " latestProdDetails.idProduct,latestProdDetails.secWt,latestProdDetails.avgSecWt, " +
                                        " latestProdDetails.stdLength,latestProdDetails.noOfPcs,latestProdDetails.avgBundleWt, " +
                                        " latestProdDetails.createdBy,latestProdDetails.createdOn,latestProdDetails.updatedBy,latestProdDetails.updatedOn " +
                                        "   FROM dimProdSpec prodSpec " +
                                        "    FULL OUTER JOIN tblMaterial material ON 1 = 1 and material.isActive = 1 " +
                                        "    FULL OUTER JOIN dimProdCat prodCat ON 1 = 1 and prodCat.isActive = 1 " +
                                        "  FULL OUTER JOIN dimbrand brand ON 1 = 1 and brand.isActive = 1 " +
                                        "    LEFT JOIN " +
                                        "    ( " +
                                        "    SELECT mainProd.*, material.materialSubType as materialDesc FROM tblProductInfo mainProd " +
                                        "    INNER JOIN " +
                                        "    ( " +
                                        "       SELECT prodCatId, materialId, prodSpecId,brandId, MAX(createdOn) createdOn " +
                                        "        FROM tblProductInfo loadingQuota " +
                                        "        GROUP BY prodCatId, materialId, prodSpecId,brandId " +
                                        "    ) latestProd " +
                                        "    ON mainProd.prodCatId = latestProd.prodCatId AND mainProd.materialId = latestProd.materialId " +
                                        "    AND mainProd.prodSpecId = latestProd.prodSpecId " +
                                        "    AND mainProd.brandId = latestProd.brandId " +
                                        "    AND mainProd.createdOn = latestProd.createdOn " +
                                        "    LEFT JOIN tblMaterial material " +
                                        "    ON material.idMaterial = mainProd.materialId " +
                                        "    ) latestProdDetails " +
                                        "    ON prodSpec.idProdSpec = latestProdDetails.prodSpecId " +
                                        "    AND latestProdDetails.prodCatId = prodCat.idProdCat " +
                                        "    AND latestProdDetails.materialId = material.idMaterial " +
                                        "    AND latestProdDetails.brandId = brand.idBrand " +
                                        " WHERE idProdSpec <> 0 and prodSpec.isActive = 1 and idProdCat <> 0 AND idProdCat=" + prodCatId +
                                        " ORDER BY prodSpec.displaySequence";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductInfoTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblProductInfoTO> SelectProductInfoListByLoadingSlipExtIds(string strLoadingSlipExtIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() +
                                        " WHERE product.idProduct IN(" +
                                        " SELECT productId FROM tblStockConsumption" +
                                        " INNER JOIN tblStockDetails ON idStockDtl = stockDtlId" +
                                        " WHERE loadingSlipExtId IN("+ strLoadingSlipExtIds + "))";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductInfoTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblProductInfoTO> ConvertReaderToList(SqlDataReader tblStockDetailsTODT)
        {
            List<TblProductInfoTO> tblProductInfoTOTOList = new List<TblProductInfoTO>();
            if (tblStockDetailsTODT != null)
            {
                while (tblStockDetailsTODT.Read())
                {
                    TblProductInfoTO tTblProductInfoTONew = new TblProductInfoTO();

                    if (tblStockDetailsTODT["prodCatId"] != DBNull.Value)
                        tTblProductInfoTONew.ProdCatId = Convert.ToInt32(tblStockDetailsTODT["prodCatId"].ToString());
                    if (tblStockDetailsTODT["materialId"] != DBNull.Value)
                        tTblProductInfoTONew.MaterialId = Convert.ToInt32(tblStockDetailsTODT["materialId"].ToString());
                    if (tblStockDetailsTODT["prodSpecId"] != DBNull.Value)
                        tTblProductInfoTONew.ProdSpecId = Convert.ToInt32(tblStockDetailsTODT["prodSpecId"].ToString());
                    if (tblStockDetailsTODT["prodSpecDesc"] != DBNull.Value)
                        tTblProductInfoTONew.ProdSpecDesc = Convert.ToString(tblStockDetailsTODT["prodSpecDesc"].ToString());
                    if (tblStockDetailsTODT["prodCateDesc"] != DBNull.Value)
                        tTblProductInfoTONew.ProdCatDesc = Convert.ToString(tblStockDetailsTODT["prodCateDesc"].ToString());
                    if (tblStockDetailsTODT["materialSubType"] != DBNull.Value)
                        tTblProductInfoTONew.MaterialDesc = Convert.ToString(tblStockDetailsTODT["materialSubType"].ToString());

                    tblProductInfoTOTOList.Add(tTblProductInfoTONew);
                }
            }
            return tblProductInfoTOTOList;
        }
        public List<TblProductInfoTO> ConvertDTToList(SqlDataReader tblProductInfoTODT)
        {
            List<TblProductInfoTO> tblProductInfoTOList = new List<TblProductInfoTO>();
            
            if (tblProductInfoTODT != null)
            {
                while (tblProductInfoTODT.Read())
                {
                    TblProductInfoTO tblProductInfoTONew = new TblProductInfoTO();
                    if (tblProductInfoTODT["idProduct"] != DBNull.Value)
                        tblProductInfoTONew.IdProduct = Convert.ToInt32(tblProductInfoTODT["idProduct"].ToString());
                    if (tblProductInfoTODT["materialId"] != DBNull.Value)
                        tblProductInfoTONew.MaterialId = Convert.ToInt32(tblProductInfoTODT["materialId"].ToString());
                    if (tblProductInfoTODT["prodCatId"] != DBNull.Value)
                        tblProductInfoTONew.ProdCatId = Convert.ToInt32(tblProductInfoTODT["prodCatId"].ToString());
                    if (tblProductInfoTODT["prodSpecId"] != DBNull.Value)
                        tblProductInfoTONew.ProdSpecId = Convert.ToInt32(tblProductInfoTODT["prodSpecId"].ToString());
                    if (tblProductInfoTODT["createdBy"] != DBNull.Value)
                        tblProductInfoTONew.CreatedBy = Convert.ToInt32(tblProductInfoTODT["createdBy"].ToString());
                    if (tblProductInfoTODT["updatedBy"] != DBNull.Value)
                        tblProductInfoTONew.UpdatedBy = Convert.ToInt32(tblProductInfoTODT["updatedBy"].ToString());
                    if (tblProductInfoTODT["createdOn"] != DBNull.Value)
                        tblProductInfoTONew.CreatedOn = Convert.ToDateTime(tblProductInfoTODT["createdOn"].ToString());
                    if (tblProductInfoTODT["updatedOn"] != DBNull.Value)
                        tblProductInfoTONew.UpdatedOn = Convert.ToDateTime(tblProductInfoTODT["updatedOn"].ToString());
                    try
                    {
                        if (tblProductInfoTODT["secWt"] != DBNull.Value)
                            tblProductInfoTONew.SecWt = Convert.ToDouble(tblProductInfoTODT["secWt"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["avgSecWt"] != DBNull.Value)
                            tblProductInfoTONew.AvgSecWt = Convert.ToDouble(tblProductInfoTODT["avgSecWt"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["stdLength"] != DBNull.Value)
                            tblProductInfoTONew.StdLength = Convert.ToDouble(tblProductInfoTODT["stdLength"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["noOfPcs"] != DBNull.Value)
                            tblProductInfoTONew.NoOfPcs = Convert.ToDouble(tblProductInfoTODT["noOfPcs"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["avgBundleWt"] != DBNull.Value)
                            tblProductInfoTONew.AvgBundleWt = Convert.ToDouble(tblProductInfoTODT["avgBundleWt"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["prodSpecDesc"] != DBNull.Value)
                            tblProductInfoTONew.ProdSpecDesc = Convert.ToString(tblProductInfoTODT["prodSpecDesc"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["prodCateDesc"] != DBNull.Value)
                            tblProductInfoTONew.ProdCatDesc = Convert.ToString(tblProductInfoTODT["prodCateDesc"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["materialSubType"] != DBNull.Value)
                            tblProductInfoTONew.MaterialDesc = Convert.ToString(tblProductInfoTODT["materialSubType"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["inchId"] != DBNull.Value)
                            tblProductInfoTONew.InchId = Convert.ToInt32(tblProductInfoTODT["inchId"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["stripId"] != DBNull.Value)
                            tblProductInfoTONew.StripId = Convert.ToInt32(tblProductInfoTODT["stripId"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["sizeId"] != DBNull.Value)
                            tblProductInfoTONew.SizeId = Convert.ToInt32(tblProductInfoTODT["sizeId"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["thicknessId"] != DBNull.Value)
                            tblProductInfoTONew.ThicknessId = Convert.ToInt32(tblProductInfoTODT["thicknessId"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        //Saket [2017-11-23] Added
                        if (tblProductInfoTODT["brandId"] != DBNull.Value)
                            tblProductInfoTONew.BrandId = Convert.ToInt32(tblProductInfoTODT["brandId"].ToString());
                    }
                    catch (Exception ex) { }


                    tblProductInfoTOList.Add(tblProductInfoTONew);
                }
            }
            return tblProductInfoTOList;
        }
        public TblProductInfoTO GetNoOfPcesAndQtyAginsCatagory(SqlConnection conn, SqlTransaction tran, int CategoryType = 1)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                if (CategoryType == 1)
                {
                    cmdSelect.CommandText = " SELECT TOP 1 tblProductInfo.noOfPcs,tblProductInfo.stripId,tblProductInfo.secWt,tblProductInfo.inchId, "+
                                            " tblProductInfo.sizeId,tblProductInfo.thicknessId "+
                                            " FROM tblProductInfo "+
                                            " LEFT JOIN tblStrips ON tblProductInfo.stripId = tblStrips.idStrip AND tblStrips.isActive = 1 "+
                                            " LEFT JOIN tblSize ON tblProductInfo.sizeId = tblSize.idSize AND tblSize.isActive = 1 "+
                                            " LEFT JOIN tblThickness ON tblProductInfo.thicknessId = tblThickness.idThickness AND tblThickness.isActive = 1 "+
                                            " LEFT JOIN ( "+
                                            " SELECT mainProd.*FROM tblProductInfo mainProd "+
                                            " INNER JOIN "+
                                            " ( "+
                                                 "SELECT prodCatId, materialId, prodSpecId, brandId, MAX(createdOn) createdOn "+
                                                 "FROM tblProductInfo loadingQuota  "+ 
                                                 "GROUP BY prodCatId, materialId, prodSpecId, brandId  "+
                                            " ) AS latestProd  "+
                                            " ON mainProd.prodCatId = latestProd.prodCatId AND mainProd.materialId = latestProd.materialId  "+
                                            " AND mainProd.prodSpecId = latestProd.prodSpecId  "+
                                            " AND mainProd.brandId = latestProd.brandId  "+
                                            " AND mainProd.createdOn = latestProd.createdOn  "+
                                            " ) AS latestProdDetails  "+
                                            " ON latestProdDetails.prodCatId = 1  "+
                                            " WHERE tblProductInfo.prodCatId = 1 " ;

                }
                else if (CategoryType == 3)
                {
                    cmdSelect.CommandText = " SELECT TOP 1 tblProductInfo.noOfPcs,tblProductInfo.stripId,tblProductInfo.secWt,tblProductInfo.inchId, " +
                                             " tblProductInfo.sizeId,tblProductInfo.thicknessId " +
                                             " FROM tblProductInfo " +
                                             " LEFT JOIN tblStrips ON tblProductInfo.stripId = tblStrips.idStrip AND tblStrips.isActive = 1 " +
                                             " LEFT JOIN tblSize ON tblProductInfo.sizeId = tblSize.idSize AND tblSize.isActive = 1 " +
                                             " LEFT JOIN tblThickness ON tblProductInfo.thicknessId = tblThickness.idThickness AND tblThickness.isActive = 1 " +
                                             " LEFT JOIN ( " +
                                             " SELECT mainProd.*FROM tblProductInfo mainProd " +
                                             " INNER JOIN " +
                                             " ( " +
                                                  "SELECT prodCatId, materialId, prodSpecId, brandId, MAX(createdOn) createdOn " +
                                                  "FROM tblProductInfo loadingQuota  " +
                                                  "GROUP BY prodCatId, materialId, prodSpecId, brandId  " +
                                             " ) AS latestProd  " +
                                             " ON mainProd.prodCatId = latestProd.prodCatId AND mainProd.materialId = latestProd.materialId  " +
                                             " AND mainProd.prodSpecId = latestProd.prodSpecId  " +
                                             " AND mainProd.brandId = latestProd.brandId  " +
                                             " AND mainProd.createdOn = latestProd.createdOn  " +
                                             " ) AS latestProdDetails  " +
                                             " ON latestProdDetails.prodCatId = 3  " +
                                             " WHERE tblProductInfo.prodCatId = 3 ";

                }
                else if (CategoryType == 4)
                {
                    cmdSelect.CommandText = " SELECT TOP 1 tblProductInfo.noOfPcs,tblProductInfo.stripId,tblProductInfo.secWt,tblProductInfo.inchId, " +
                                            " tblProductInfo.sizeId,tblProductInfo.thicknessId " +
                                            " FROM tblProductInfo " +
                                            " LEFT JOIN tblStrips ON tblProductInfo.stripId = tblStrips.idStrip AND tblStrips.isActive = 1 " +
                                            " LEFT JOIN tblSize ON tblProductInfo.sizeId = tblSize.idSize AND tblSize.isActive = 1 " +
                                            " LEFT JOIN tblThickness ON tblProductInfo.thicknessId = tblThickness.idThickness AND tblThickness.isActive = 1 " +
                                            " LEFT JOIN ( " +
                                            " SELECT mainProd.*FROM tblProductInfo mainProd " +
                                            " INNER JOIN " +
                                            " ( " +
                                                 "SELECT prodCatId, materialId, prodSpecId, brandId, MAX(createdOn) createdOn " +
                                                 "FROM tblProductInfo loadingQuota  " +
                                                 "GROUP BY prodCatId, materialId, prodSpecId, brandId  " +
                                            " ) AS latestProd  " +
                                            " ON mainProd.prodCatId = latestProd.prodCatId AND mainProd.materialId = latestProd.materialId  " +
                                            " AND mainProd.prodSpecId = latestProd.prodSpecId  " +
                                            " AND mainProd.brandId = latestProd.brandId  " +
                                            " AND mainProd.createdOn = latestProd.createdOn  " +
                                            " ) AS latestProdDetails  " +
                                            " ON latestProdDetails.prodCatId = 4 " +
                                            " WHERE tblProductInfo.prodCatId = 4 ";
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductInfoTO> list = ConvertDTToCategoryList(reader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblProductInfoTO> ConvertDTToCategoryList(SqlDataReader tblProductInfoTODT)
        {
            List<TblProductInfoTO> tblProductInfoTOList = new List<TblProductInfoTO>();

            if (tblProductInfoTODT != null)
            {
                while (tblProductInfoTODT.Read())
                {
                    TblProductInfoTO tblProductInfoTONew = new TblProductInfoTO();
                    try
                    {
                        if (tblProductInfoTODT["noOfPcs"] != DBNull.Value)
                            tblProductInfoTONew.NoOfPcs = Convert.ToDouble(tblProductInfoTODT["noOfPcs"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["secWt"] != DBNull.Value)
                            tblProductInfoTONew.SecWt = Convert.ToDouble(tblProductInfoTODT["secWt"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["stripId"] != DBNull.Value)
                            tblProductInfoTONew.StripId = Convert.ToInt32(tblProductInfoTODT["stripId"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["inchId"] != DBNull.Value)
                            tblProductInfoTONew.InchId = Convert.ToInt32(tblProductInfoTODT["inchId"].ToString());
                    }
                    catch (Exception ex) { }
                   
                    try
                    {
                        if (tblProductInfoTODT["sizeId"] != DBNull.Value)
                            tblProductInfoTONew.SizeId = Convert.ToInt32(tblProductInfoTODT["sizeId"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblProductInfoTODT["thicknessId"] != DBNull.Value)
                            tblProductInfoTONew.ThicknessId = Convert.ToInt32(tblProductInfoTODT["thicknessId"].ToString());
                    }
                    catch (Exception ex) { }

                   tblProductInfoTOList.Add(tblProductInfoTONew);
                }
            }
            return tblProductInfoTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProductInfoTO, cmdInsert);
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

        public int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProductInfoTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblProductInfoTO tblProductInfoTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProductInfo]( " + 
                                "  [materialId]" +
                                " ,[prodCatId]" +
                                " ,[prodSpecId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[secWt]" +
                                " ,[avgSecWt]" +
                                " ,[stdLength]" +
                                " ,[noOfPcs]" +
                                " ,[avgBundleWt]" +
                                " ,[brandId]" +
                                " )" +
                    " VALUES (" +
                                "  @MaterialId " +
                                " ,@ProdCatId " +
                                " ,@ProdSpecId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@SecWt " +
                                " ,@AvgSecWt " +
                                " ,@StdLength " +
                                " ,@NoOfPcs " +
                                " ,@AvgBundleWt " +
                                " ,@BrandId " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdProduct", System.Data.SqlDbType.Int).Value = tblProductInfoTO.IdProduct;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblProductInfoTO.MaterialId;
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblProductInfoTO.ProdCatId;
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblProductInfoTO.ProdSpecId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProductInfoTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductInfoTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProductInfoTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductInfoTO.UpdatedOn);
            cmdInsert.Parameters.Add("@SecWt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductInfoTO.SecWt);
            cmdInsert.Parameters.Add("@AvgSecWt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductInfoTO.AvgSecWt);
            cmdInsert.Parameters.Add("@StdLength", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductInfoTO.StdLength);
            cmdInsert.Parameters.Add("@NoOfPcs", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductInfoTO.NoOfPcs);
            cmdInsert.Parameters.Add("@AvgBundleWt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductInfoTO.AvgBundleWt);
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblProductInfoTO.BrandId;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblProductInfoTO.IdProduct = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProductInfoTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProductInfoTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblProductInfoTO tblProductInfoTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductInfo] SET " + 
            "  [idProduct] = @IdProduct" +
            " ,[materialId]= @MaterialId" +
            " ,[prodCatId]= @ProdCatId" +
            " ,[prodSpecId]= @ProdSpecId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[secWt]= @SecWt" +
            " ,[avgSecWt]= @AvgSecWt" +
            " ,[stdLength]= @StdLength" +
            " ,[noOfPcs]= @NoOfPcs" +
            " ,[avgBundleWt] = @AvgBundleWt" +
            " ,[brandId] = @BrandId " +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProduct", System.Data.SqlDbType.Int).Value = tblProductInfoTO.IdProduct;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblProductInfoTO.MaterialId;
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblProductInfoTO.ProdCatId;
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblProductInfoTO.ProdSpecId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProductInfoTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblProductInfoTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProductInfoTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblProductInfoTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@SecWt", System.Data.SqlDbType.NVarChar).Value = tblProductInfoTO.SecWt;
            cmdUpdate.Parameters.Add("@AvgSecWt", System.Data.SqlDbType.NVarChar).Value = tblProductInfoTO.AvgSecWt;
            cmdUpdate.Parameters.Add("@StdLength", System.Data.SqlDbType.NVarChar).Value = tblProductInfoTO.StdLength;
            cmdUpdate.Parameters.Add("@NoOfPcs", System.Data.SqlDbType.NVarChar).Value = tblProductInfoTO.NoOfPcs;
            cmdUpdate.Parameters.Add("@AvgBundleWt", System.Data.SqlDbType.NVarChar).Value = tblProductInfoTO.AvgBundleWt;
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblProductInfoTO.BrandId;

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblProductInfo(Int32 idProduct)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProduct, cmdDelete);
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

        public int DeleteTblProductInfo(Int32 idProduct, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProduct, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idProduct, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProductInfo] " +
            " WHERE idProduct = " + idProduct +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProduct", System.Data.SqlDbType.Int).Value = tblProductInfoTO.IdProduct;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
