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
    public class TblStockDetailsDAO : ITblStockDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblStockDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT stockDtl.* ,prodSpec.prodSpecDesc ,prodCateDesc,locationDesc,material.materialSubType,inch.inch,strips.grade,size.size,thickness.thickness" +
                                  " FROM tblstockDetails stockDtl " +
                                  " LEFT JOIN dimProdSpec prodSpec " +
                                  " ON stockDtl.prodSpecId = prodSpec.idProdSpec " +
                                  " LEFT JOIN tblLocation location " +
                                  " ON stockDtl.locationId = location.idLocation " +
                                  " LEFT JOIN dimProdCat prodCat " +
                                  " ON stockDtl.prodCatId = prodCat.idProdCat " +
                                  " LEFT JOIN tblMaterial material " +
                                  " ON stockDtl.materialId = material.idMaterial " +
                                  " LEFT JOIN tblStockSummary stkSummary " +
                                  " ON stockDtl.stockSummaryId = stkSummary.idStockSummary " +
                                  " LEFT JOIN tblProductItem prodItem ON stockDtl.prodItemId=prodItem.idProdItem " +
                                  " LEFT JOIN tblInch inch ON stockDtl.inchId=inch.idInch" +
                                  " LEFT JOIN tblStrips strips ON stockDtl.stripId=strips.idStrip" +
                                  " LEFT JOIN tblSize size ON stockDtl.sizeId=size.idSize" +
                                  " LEFT JOIN tblThickness thickness ON stockDtl.thicknessId=thickness.idThickness";//Vijaymala[23-08-2018] Added for ProductItemId.

            return sqlSelectQry;
        }

        public String SqlSelectLocationQuery()
        {
            String sqlSelectQry = " SELECT loc.*, parentLoc.locationDesc AS parentLocDesc FROM tblLocation loc " +
                                  " LEFT JOIN tblLocation parentLoc ON loc.parentLocId = parentLoc.idLocation ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblStockDetailsTO> SelectAllTblStockDetails()
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
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblStockDetailsTO> SelectAllTblStockDetailsConsolidated(Int32 isConsolidated, Int32 brandId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + "WHERE stockDtl.isConsolidatedStock = " + isConsolidated + " AND brandId = " + brandId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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
        
        public List<TblStockDetailsTO> SelectAllTblStockDetailsConsolidated(Int32 isConsolidated, Int32 brandId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + "WHERE stockDtl.isConsolidatedStock = " + isConsolidated + " AND brandId = " + brandId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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
                cmdSelect.Dispose();
            }
        }

        public List<TblStockDetailsTO> SelectAllTblStockDetails(Int64 stockSummaryId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE stockDtl.stockSummaryId=" + stockSummaryId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public List<TblStockDetailsTO> SelectAllTblStockDetails(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate, Int32 brandId, int compartmentId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE stockDtl.prodCatId=" + prodCatId + " AND stockDtl.prodSpecId=" + prodSpecId + " AND stockDtl.brandId=" + brandId;

                // Vaibhav [26-Mar-2018] Added to get compartment wise stock
                if (compartmentId != 0)
                {
                    cmdSelect.CommandText += " AND stockDtl.locationId=" + compartmentId;
                }

                if(stockDate != new DateTime())
                    cmdSelect.CommandText += " AND DAY(stockDate)=" + stockDate.Day + " AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate)=" + stockDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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

        /// <summary>
        ///                     //[05-09-2018]Vijaymala added to get stock of regular or other item  
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <param name="prodSpecId"></param>
        /// <param name="prodItemId"></param>
        /// <param name="brandId"></param>
        /// <param name="compartmentId"></param>
        /// <param name="stockDate"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>

        public List<TblStockDetailsTO> SelectAllTblStockDetailsOther(Int32 prodCatId, Int32 prodSpecId, Int32 prodItemId, Int32 brandId, int compartmentId, DateTime stockDate,  SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE ISNULL(stockDtl.prodCatId,0)=" + prodCatId + " AND ISNULL(stockDtl.prodSpecId,0)=" + prodSpecId + " AND ISNULL(stockDtl.brandId,0)=" + brandId +
                " AND ISNULL(stockDtl.prodItemId,0)=" + prodItemId;
                // Vaibhav [26-Mar-2018] Added to get compartment wise stock
                if (compartmentId != 0)
                {
                    cmdSelect.CommandText += " AND stockDtl.locationId=" + compartmentId;
                }

                if (stockDate != new DateTime())
                    cmdSelect.CommandText += " AND DAY(stockDate)=" + stockDate.Day + " AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate)=" + stockDate.Year;

                //" AND DAY(stockDate)=" + stockDate.Day + " AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate)=" + stockDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblStockDetailsTO> SelectAllTblStockDetails(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE stockDtl.prodCatId=" + prodCatId + " AND stockDtl.prodSpecId=" + prodSpecId +
                                        " AND DAY(stockDate)=" + stockDate.Day + " AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate)=" + stockDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblStockDetailsTO> SelectAllTblStockDetails(Int32 prodCatId, Int32 prodSpecId, int materialId,DateTime stockDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE stockDtl.prodCatId=" + prodCatId + " AND stockDtl.prodSpecId=" + prodSpecId + " AND stockDtl.materialId=" + materialId +
                                        " AND DAY(stockDate)=" + stockDate.Day + " AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate)=" + stockDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblStockDetailsTO> SelectAllTblStockDetails(int locationId, int prodCatId,DateTime stockDate, int brandId,int inchId,int stripId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE stockDtl.locationId=" + locationId + "AND stockDtl.prodCatId=" + prodCatId +
                //                    //" AND stkSummary.stockDate=@stockDt AND stockDtl.brandId = " + brandId;
                //                    " AND stockDtl.brandId = " + brandId;

                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE stockDtl.locationId=" + locationId + // "AND stockDtl.prodCatId=" + prodCatId +
                //                    //" AND stkSummary.stockDate=@stockDt AND stockDtl.brandId = " + brandId;
                //                    " AND stockDtl.brandId = " + brandId + "AND stockDtl.inchId= " + inchId + "AND stockDtl.stripId= "+ stripId +" "+
                //    " order by stockDtl.idStockDtl desc";
                string select = SqlSelectQuery() + " WHERE stockDtl.locationId=" + locationId + // "AND stockDtl.prodCatId=" + prodCatId +
                                                                                                //" AND stkSummary.stockDate=@stockDt AND stockDtl.brandId = " + brandId;
                                  " AND stockDtl.brandId = " + brandId;
                if (inchId > 0)
                    select = select + "AND stockDtl.inchId= " + inchId;
                if (stripId > 0)
                    select = select + "AND stockDtl.stripId= " + stripId;

                cmdSelect.CommandText = select + " order by stockDtl.idStockDtl desc";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@stockDt", System.Data.SqlDbType.DateTime).Value = stockDate;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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
        public List<TblStockDetailsTO> SelectAllTblStockDetailsForAutoInsert()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
               
                cmdSelect.CommandText = SqlSelectQuery() + " order by stockDtl.idStockDtl desc";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

               // cmdSelect.Parameters.Add("@stockDt", System.Data.SqlDbType.DateTime).Value = stockDate;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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
        public List<TblLocationTO> SelectAllTblLocation()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectLocationQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToListLoc(sqlReader);
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
        public List<TblLocationTO> ConvertDTToListLoc(SqlDataReader tblLocationTODT)
        {
            List<TblLocationTO> tblLocationTOList = new List<TblLocationTO>();
            if (tblLocationTODT != null)
            {
                while (tblLocationTODT.Read())
                {
                    TblLocationTO tblLocationTONew = new TblLocationTO();
                    if (tblLocationTODT["idLocation"] != DBNull.Value)
                        tblLocationTONew.IdLocation = Convert.ToInt32(tblLocationTODT["idLocation"].ToString());
                    if (tblLocationTODT["parentLocId"] != DBNull.Value)
                        tblLocationTONew.ParentLocId = Convert.ToInt32(tblLocationTODT["parentLocId"].ToString());
                    if (tblLocationTODT["createdBy"] != DBNull.Value)
                        tblLocationTONew.CreatedBy = Convert.ToInt32(tblLocationTODT["createdBy"].ToString());
                    if (tblLocationTODT["updatedBy"] != DBNull.Value)
                        tblLocationTONew.UpdatedBy = Convert.ToInt32(tblLocationTODT["updatedBy"].ToString());
                    if (tblLocationTODT["createdOn"] != DBNull.Value)
                        tblLocationTONew.CreatedOn = Convert.ToDateTime(tblLocationTODT["createdOn"].ToString());
                    if (tblLocationTODT["updatedOn"] != DBNull.Value)
                        tblLocationTONew.UpdatedOn = Convert.ToDateTime(tblLocationTODT["updatedOn"].ToString());
                    if (tblLocationTODT["locationDesc"] != DBNull.Value)
                        tblLocationTONew.LocationDesc = Convert.ToString(tblLocationTODT["locationDesc"].ToString());
                    if (tblLocationTODT["parentLocDesc"] != DBNull.Value)
                        tblLocationTONew.ParentLocationDesc = Convert.ToString(tblLocationTODT["parentLocDesc"].ToString());
                    tblLocationTOList.Add(tblLocationTONew);
                }
            }
            return tblLocationTOList;
        }
        public List<TblStockDetailsTO> SelectEmptyStockDetailsTemplate(int prodCatId,int locationId, int brandId, Int32 isConsolidate, int inchId, int stripId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                if (isConsolidate == 1)
                {

                    cmdSelect.CommandText = " SELECT idProdCat AS prodCatId, prodCateDesc, prodSpec.idProdSpec AS prodSpecId,prodSpecDesc, idMaterial AS materialId,materialSubType  " +
                                            " ,brand.idBrand AS brandId" +
                                            " FROM dimProdSpec prodSpec " +
                                            " FULL OUTER JOIN tblMaterial material ON 1 = 1 AND material.isActive = 1 " +
                                            " FULL OUTER JOIN dimProdCat prodCat ON 1 = 1 " +
                                            " FULL OUTER JOIN dimBrand brand ON 1 = 1 " +
                                            " LEFT JOIN tblStockConfig tblStockConfig  " +
                                            " ON prodSpec.idProdSpec = tblStockConfig.prodSpecId AND material.idMaterial = tblStockConfig.materialId " +
                                            " AND tblStockConfig.brandId = brand.idBrand  AND tblStockConfig.prodCatId = prodCat.idProdCat " +
                                            " WHERE idProdSpec <> 0 AND idProdCat <> 0 AND idBrand <> 0 AND idProdCat=" + prodCatId +
                                            " AND idBrand =" + brandId + " AND tblStockConfig.isItemizedStock = 1 AND prodSpec.isActive = 1  " + //Saket [2018-01-30] Added
                                            " ORDER BY prodSpec.displaySequence";
                }
                else if (inchId > 0)
                {
                    string Query = "SELECT idProdCat AS prodCatId, prodCateDesc, tblInch. idinch,  inch,idSize ,size ,idThickness ,thickness " +
                        "    FROM  dimProdCat prodCat " +
                        "  left join tblInch on tblInch.prodCatId=  idProdCat  " +
                        "  left join tblSize  on tblSize.prodCatId=  idProdCat  and tblInch.idInch =tblSize.idInch " +
                        "  left join tblThickness  on tblThickness.prodCatId=  idProdCat " +
                        //" left join tblStrips  on tblStrips.prodCatId=  idProdCat " +
                        " WHERE  idProdCat <> 0  AND idProdCat= "+prodCatId +"";
                    if (inchId > 0)
                        Query = Query + "   and tblInch.idInch =" + inchId + "";
                    //if (stripId  > 0)
                    //    Query = Query + "   and  tblStrips .idStrip  " + stripId + "";
                    cmdSelect.CommandText = Query;

                }
                else if ( stripId > 0)
                {
                    string Query = "SELECT idProdCat AS prodCatId, prodCateDesc,idSize ,size ,idThickness ,thickness,idStrip  ,grade " +
                        "    FROM  dimProdCat prodCat " +
                        //"  left join tblInch on tblInch.prodCatId=  idProdCat  " +
                        "  left join tblSize  on tblSize.prodCatId=  idProdCat " +
                        "  left join tblThickness  on tblThickness.prodCatId=  idProdCat " +
                        " left join tblStrips  on tblStrips.prodCatId=  idProdCat " +
                        " WHERE  idProdCat <> 0  AND idProdCat= " + prodCatId + "";
                    //if (inchId > 0)
                    //    Query = Query + "   and tblInch.idInch =" + inchId + "";
                    if (stripId > 0)
                        Query = Query + "   and  tblStrips .idStrip  " + stripId + "";
                    cmdSelect.CommandText = Query;

                }
                else
                {
                    cmdSelect.CommandText = " SELECT idProdCat AS prodCatId, prodCateDesc, prodSpec.idProdSpec AS prodSpecId,prodSpecDesc, idMaterial AS materialId,materialSubType  " +
                                            " ,brand.idBrand AS brandId" +
                                            " FROM dimProdSpec prodSpec " +
                                            " FULL OUTER JOIN tblMaterial material ON 1 = 1 AND material.isActive = 1 " +
                                            " FULL OUTER JOIN dimProdCat prodCat ON 1 = 1 " +
                                            " FULL OUTER JOIN dimBrand brand ON 1 = 1 " +
                                            //" LEFT JOIN tblStockConfig tblStockConfig  " +
                                            //" ON prodSpec.idProdSpec = tblStockConfig.prodSpecId AND material.idMaterial = tblStockConfig.materialId " +
                                            //" AND tblStockConfig.brandId = brand.idBrand  AND tblStockConfig.prodCatId = prodCat.idProdCat " +
                                            " WHERE idProdSpec <> 0 AND idProdCat <> 0 AND idBrand <> 0 AND idProdCat=" + prodCatId +
                                            //" AND idBrand =" + brandId + " AND tblStockConfig.isItemizedStock = 1 " +
                                            " AND idBrand =" + brandId + " AND prodSpec.isActive = 1  " +
                                            " ORDER BY prodSpec.displaySequence";
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertReaderToList(sqlReader, locationId);
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
        
        public List<TblStockDetailsTO> SelectEmptyStockDetailsTemplateForAutoInsert(Int32 isConsolidate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                if (isConsolidate == 1)
                {

                    cmdSelect.CommandText = " SELECT idProdCat AS prodCatId, prodCateDesc, prodSpec.idProdSpec AS prodSpecId,prodSpecDesc, idMaterial AS materialId,materialSubType  " +
                                            " ,brand.idBrand AS brandId" +
                                            " FROM dimProdSpec prodSpec " +
                                            " FULL OUTER JOIN tblMaterial material ON 1 = 1 AND material.isActive = 1 " +
                                            " FULL OUTER JOIN dimProdCat prodCat ON 1 = 1 " +
                                            " FULL OUTER JOIN dimBrand brand ON 1 = 1 " +
                                            " LEFT JOIN tblStockConfig tblStockConfig  " +
                                            " ON prodSpec.idProdSpec = tblStockConfig.prodSpecId AND material.idMaterial = tblStockConfig.materialId " +
                                            " AND tblStockConfig.brandId = brand.idBrand  AND tblStockConfig.prodCatId = prodCat.idProdCat " +
                                            " WHERE idProdSpec <> 0 AND idProdCat <> 0 AND idBrand <> 0 AND tblStockConfig.isItemizedStock = 1 AND prodSpec.isActive = 1  " +                                           
                                            " ORDER BY prodSpec.displaySequence";
                }
                else
                {
                    cmdSelect.CommandText = " SELECT idProdCat AS prodCatId, prodCateDesc, prodSpec.idProdSpec AS prodSpecId,prodSpecDesc, idMaterial AS materialId,materialSubType  " +
                                            " ,brand.idBrand AS brandId" +
                                            " FROM dimProdSpec prodSpec " +
                                            " FULL OUTER JOIN tblMaterial material ON 1 = 1 AND material.isActive = 1 " +
                                            " FULL OUTER JOIN dimProdCat prodCat ON 1 = 1 " +
                                            " FULL OUTER JOIN dimBrand brand ON 1 = 1 " +
                                            " WHERE idProdSpec <> 0 AND idProdCat <> 0 AND idBrand <> 0   AND prodSpec.isActive = 1  " +                                             
                                            " ORDER BY prodSpec.displaySequence";
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertReaderToList(sqlReader);
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
        public TblStockDetailsTO SelectTblStockDetails(Int64 idStockDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idStockDtl = " + idStockDtl + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(reader);
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
            }
        }

        public TblStockDetailsTO SelectTblStockDetails(TblRunningSizesTO runningSizeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE stockDtl.locationId = " + runningSizeTO.LocationId + " AND stockDtl.prodCatId=" + runningSizeTO.ProdCatId +
                                        " AND stockDtl.materialId=" + runningSizeTO.MaterialId + " AND stockDtl.prodSpecId=" + runningSizeTO.ProdSpecId +
                                        " AND DAY(stockDtl.createdOn)=" + runningSizeTO.CreatedOn.Day + " AND MONTH(stockDtl.createdOn)=" + runningSizeTO.CreatedOn.Month +
                                        " AND YEAR(stockDtl.createdOn)=" + runningSizeTO.CreatedOn.Year;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(reader);
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
            }
        }

        public List<TblStockDetailsTO> ConvertDTToList(SqlDataReader tblStockDetailsTODT)
        {
            List<TblStockDetailsTO> tblStockDetailsTOList = new List<TblStockDetailsTO>();
            if (tblStockDetailsTODT != null)
            {
                while (tblStockDetailsTODT.Read())
                {
                    TblStockDetailsTO tblStockDetailsTONew = new TblStockDetailsTO();
                    if (tblStockDetailsTODT["idStockDtl"] != DBNull.Value)
                        tblStockDetailsTONew.IdStockDtl = Convert.ToInt32(tblStockDetailsTODT["idStockDtl"].ToString());
                    if (tblStockDetailsTODT["stockSummaryId"] != DBNull.Value)
                        tblStockDetailsTONew.StockSummaryId = Convert.ToInt32(tblStockDetailsTODT["stockSummaryId"].ToString());
                    if (tblStockDetailsTODT["locationId"] != DBNull.Value)
                        tblStockDetailsTONew.LocationId = Convert.ToInt32(tblStockDetailsTODT["locationId"].ToString());
                    if (tblStockDetailsTODT["prodCatId"] != DBNull.Value)
                        tblStockDetailsTONew.ProdCatId = Convert.ToInt32(tblStockDetailsTODT["prodCatId"].ToString());
                    if (tblStockDetailsTODT["materialId"] != DBNull.Value)
                        tblStockDetailsTONew.MaterialId = Convert.ToInt32(tblStockDetailsTODT["materialId"].ToString());
                    if (tblStockDetailsTODT["prodSpecId"] != DBNull.Value)
                        tblStockDetailsTONew.ProdSpecId = Convert.ToInt32(tblStockDetailsTODT["prodSpecId"].ToString());
                    if (tblStockDetailsTODT["createdBy"] != DBNull.Value)
                        tblStockDetailsTONew.CreatedBy = Convert.ToInt32(tblStockDetailsTODT["createdBy"].ToString());
                    if (tblStockDetailsTODT["updatedBy"] != DBNull.Value)
                        tblStockDetailsTONew.UpdatedBy = Convert.ToInt32(tblStockDetailsTODT["updatedBy"].ToString());
                    if (tblStockDetailsTODT["createdOn"] != DBNull.Value)
                        tblStockDetailsTONew.CreatedOn = Convert.ToDateTime(tblStockDetailsTODT["createdOn"].ToString());
                    if (tblStockDetailsTODT["updatedOn"] != DBNull.Value)
                        tblStockDetailsTONew.UpdatedOn = Convert.ToDateTime(tblStockDetailsTODT["updatedOn"].ToString());
                    if (tblStockDetailsTODT["noOfBundles"] != DBNull.Value)
                        tblStockDetailsTONew.NoOfBundles = Convert.ToDouble(tblStockDetailsTODT["noOfBundles"].ToString());
                    if (tblStockDetailsTODT["totalStock"] != DBNull.Value)
                        tblStockDetailsTONew.TotalStock = Convert.ToDouble(tblStockDetailsTODT["totalStock"].ToString());
                    if (tblStockDetailsTODT["loadedStock"] != DBNull.Value)
                        tblStockDetailsTONew.LoadedStock = Convert.ToDouble(tblStockDetailsTODT["loadedStock"].ToString());
                    if (tblStockDetailsTODT["balanceStock"] != DBNull.Value)
                        tblStockDetailsTONew.BalanceStock = Convert.ToDouble(tblStockDetailsTODT["balanceStock"].ToString());
                    if (tblStockDetailsTODT["prodSpecDesc"] != DBNull.Value)
                        tblStockDetailsTONew.ProdSpecDesc = Convert.ToString(tblStockDetailsTODT["prodSpecDesc"].ToString());
                    if (tblStockDetailsTODT["prodCateDesc"] != DBNull.Value)
                        tblStockDetailsTONew.ProdCatDesc = Convert.ToString(tblStockDetailsTODT["prodCateDesc"].ToString());
                    if (tblStockDetailsTODT["locationDesc"] != DBNull.Value)
                        tblStockDetailsTONew.LocationName = Convert.ToString(tblStockDetailsTODT["locationDesc"].ToString());
                    if (tblStockDetailsTODT["materialSubType"] != DBNull.Value)
                        tblStockDetailsTONew.MaterialDesc = Convert.ToString(tblStockDetailsTODT["materialSubType"].ToString());
                    if (tblStockDetailsTODT["productId"] != DBNull.Value)
                        tblStockDetailsTONew.ProductId = Convert.ToInt32(tblStockDetailsTODT["productId"].ToString());
                    if (tblStockDetailsTODT["removedStock"] != DBNull.Value)
                        tblStockDetailsTONew.RemovedStock = Convert.ToDouble(tblStockDetailsTODT["removedStock"].ToString());
                    if (tblStockDetailsTODT["todaysStock"] != DBNull.Value)
                        tblStockDetailsTONew.TodaysStock = Convert.ToDouble(tblStockDetailsTODT["todaysStock"].ToString());

                    if (tblStockDetailsTODT["brandId"] != DBNull.Value)
                        tblStockDetailsTONew.BrandId = Convert.ToInt32(tblStockDetailsTODT["brandId"].ToString());
                    if (tblStockDetailsTODT["isConsolidatedStock"] != DBNull.Value)
                        tblStockDetailsTONew.IsConsolidatedStock = Convert.ToInt32(tblStockDetailsTODT["isConsolidatedStock"].ToString());

                    if (tblStockDetailsTODT["isInMT"] != DBNull.Value)
                        tblStockDetailsTONew.IsInMT = Convert.ToInt32(tblStockDetailsTODT["isInMT"].ToString());

                    if (tblStockDetailsTODT["prodItemId"] != DBNull.Value)
                        tblStockDetailsTONew.ProdItemId = Convert.ToInt32(tblStockDetailsTODT["prodItemId"].ToString());
                    if (tblStockDetailsTODT["prodNoOfBundles"] != DBNull.Value)
                        tblStockDetailsTONew.ProdNoOfBundles = Convert.ToDouble(tblStockDetailsTODT["prodNoOfBundles"].ToString());
                    if (tblStockDetailsTODT["prodtotalStock"] != DBNull.Value)
                        tblStockDetailsTONew.ProdtotalStock = Convert.ToDouble(tblStockDetailsTODT["prodtotalStock"].ToString());

                    if (tblStockDetailsTODT["inchId"] != DBNull.Value)
                        tblStockDetailsTONew.InchId = Convert.ToInt32(tblStockDetailsTODT["inchId"].ToString());
                    if (tblStockDetailsTODT["stripId"] != DBNull.Value)
                        tblStockDetailsTONew.StripId = Convert.ToInt32(tblStockDetailsTODT["stripId"].ToString());
                    if (tblStockDetailsTODT["sizeId"] != DBNull.Value)
                        tblStockDetailsTONew.SizeId = Convert.ToInt32(tblStockDetailsTODT["sizeId"].ToString());
                    if (tblStockDetailsTODT["thicknessId"] != DBNull.Value)
                        tblStockDetailsTONew.ThicknessId = Convert.ToInt32(tblStockDetailsTODT["thicknessId"].ToString());

                    if (tblStockDetailsTODT["inch"] != DBNull.Value)
                        tblStockDetailsTONew.Inch = Convert.ToString(tblStockDetailsTODT["inch"].ToString());
                    if (tblStockDetailsTODT["grade"] != DBNull.Value)
                        tblStockDetailsTONew.Grade = Convert.ToString(tblStockDetailsTODT["grade"].ToString());
                    if (tblStockDetailsTODT["size"] != DBNull.Value)
                        tblStockDetailsTONew.Size = Convert.ToString(tblStockDetailsTODT["size"].ToString());
                    if (tblStockDetailsTODT["thickness"] != DBNull.Value)
                        tblStockDetailsTONew.Thickness = Convert.ToString(tblStockDetailsTODT["thickness"].ToString());
                    tblStockDetailsTOList.Add(tblStockDetailsTONew);
                }
            }
            return tblStockDetailsTOList;
        }

        public List<TblStockDetailsTO> ConvertReaderToList(SqlDataReader tblStockDetailsTODT,int locationId)
        {
            List<TblStockDetailsTO> tblStockDetailsTOList = new List<TblStockDetailsTO>();
            if (tblStockDetailsTODT != null)
            {
                while (tblStockDetailsTODT.Read())
                {
                    TblStockDetailsTO tblStockDetailsTONew = new TblStockDetailsTO();

                    tblStockDetailsTONew.LocationId = locationId;
                    try
                    { 
                    if (tblStockDetailsTODT["prodCatId"] != DBNull.Value)
                        tblStockDetailsTONew.ProdCatId = Convert.ToInt32(tblStockDetailsTODT["prodCatId"].ToString());
                    }
                    catch (Exception ex) { };
                    try
                    {
                        if (tblStockDetailsTODT["materialId"] != DBNull.Value)
                        tblStockDetailsTONew.MaterialId = Convert.ToInt32(tblStockDetailsTODT["materialId"].ToString());
                    }
                    catch (Exception ex) { };
                    try
                    {
                        if (tblStockDetailsTODT["prodSpecId"] != DBNull.Value)
                        tblStockDetailsTONew.ProdSpecId = Convert.ToInt32(tblStockDetailsTODT["prodSpecId"].ToString());
                    }
                    catch (Exception ex) { };
                    try
                    {
                        if (tblStockDetailsTODT["prodSpecDesc"] != DBNull.Value)
                        tblStockDetailsTONew.ProdSpecDesc = Convert.ToString(tblStockDetailsTODT["prodSpecDesc"].ToString());
                    }
                    catch (Exception ex) { };
                    try
                    {
                        if (tblStockDetailsTODT["prodCateDesc"] != DBNull.Value)
                        tblStockDetailsTONew.ProdCatDesc = Convert.ToString(tblStockDetailsTODT["prodCateDesc"].ToString());
                    }
                    catch (Exception ex) { };
                    try
                    {
                        if (tblStockDetailsTODT["materialSubType"] != DBNull.Value)
                        tblStockDetailsTONew.MaterialDesc = Convert.ToString(tblStockDetailsTODT["materialSubType"].ToString());
                    }
                    catch (Exception ex) { };
                    try
                    {
                        if (tblStockDetailsTODT["brandId"] != DBNull.Value)
                            tblStockDetailsTONew.BrandId = Convert.ToInt32(tblStockDetailsTODT["brandId"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblStockDetailsTODT["idinch"] != DBNull.Value)
                            tblStockDetailsTONew.InchId = Convert.ToInt32(tblStockDetailsTODT["idinch"].ToString());

                    }
                    catch (Exception ex) { };
                    try
                    {
                        if (tblStockDetailsTODT["idSize"] != DBNull.Value)
                            tblStockDetailsTONew.SizeId = Convert.ToInt32(tblStockDetailsTODT["idSize"].ToString());

                    }
                    catch (Exception ex) { };
                    try
                    {
                        if (tblStockDetailsTODT["idThickness"] != DBNull.Value)
                            tblStockDetailsTONew.ThicknessId = Convert.ToInt32(tblStockDetailsTODT["idThickness"].ToString());

                    }
                    catch (Exception ex) { };
                    try
                    {
                        if (tblStockDetailsTODT["idStrip"] != DBNull.Value)
                            tblStockDetailsTONew.StripId = Convert.ToInt32(tblStockDetailsTODT["idStrip"].ToString());

                    }
                    catch (Exception ex) { };
                    tblStockDetailsTOList.Add(tblStockDetailsTONew);
                }
            }
            return tblStockDetailsTOList;
        }

        public List<TblStockDetailsTO> ConvertReaderToList(SqlDataReader tblStockDetailsTODT)
        {
            List<TblStockDetailsTO> tblStockDetailsTOList = new List<TblStockDetailsTO>();
            if (tblStockDetailsTODT != null)
            {
                while (tblStockDetailsTODT.Read())
                {
                    TblStockDetailsTO tblStockDetailsTONew = new TblStockDetailsTO();

                    //tblStockDetailsTONew.LocationId = locationId;
                    if (tblStockDetailsTODT["prodCatId"] != DBNull.Value)
                        tblStockDetailsTONew.ProdCatId = Convert.ToInt32(tblStockDetailsTODT["prodCatId"].ToString());
                    if (tblStockDetailsTODT["materialId"] != DBNull.Value)
                        tblStockDetailsTONew.MaterialId = Convert.ToInt32(tblStockDetailsTODT["materialId"].ToString());
                    if (tblStockDetailsTODT["prodSpecId"] != DBNull.Value)
                        tblStockDetailsTONew.ProdSpecId = Convert.ToInt32(tblStockDetailsTODT["prodSpecId"].ToString());
                    if (tblStockDetailsTODT["prodSpecDesc"] != DBNull.Value)
                        tblStockDetailsTONew.ProdSpecDesc = Convert.ToString(tblStockDetailsTODT["prodSpecDesc"].ToString());
                    if (tblStockDetailsTODT["prodCateDesc"] != DBNull.Value)
                        tblStockDetailsTONew.ProdCatDesc = Convert.ToString(tblStockDetailsTODT["prodCateDesc"].ToString());
                    if (tblStockDetailsTODT["materialSubType"] != DBNull.Value)
                        tblStockDetailsTONew.MaterialDesc = Convert.ToString(tblStockDetailsTODT["materialSubType"].ToString());

                    if (tblStockDetailsTODT["brandId"] != DBNull.Value)
                        tblStockDetailsTONew.BrandId = Convert.ToInt32(tblStockDetailsTODT["brandId"].ToString());

                    tblStockDetailsTOList.Add(tblStockDetailsTONew);
                }
            }
            return tblStockDetailsTOList;
        }

        public List<SizeSpecWiseStockTO> ConvertReaderToStockList(SqlDataReader tblStockDetailsTODT)
        {
            List<SizeSpecWiseStockTO> sizeSpecWiseStockTOList = new List<SizeSpecWiseStockTO>();
            if (tblStockDetailsTODT != null)
            {
                while (tblStockDetailsTODT.Read())
                {
                    SizeSpecWiseStockTO sizeSpecWiseStockTO = new SizeSpecWiseStockTO();

                    //if (tblStockDetailsTODT["stockSummaryId"] != DBNull.Value)
                    //    sizeSpecWiseStockTO.StockSummaryId = Convert.ToInt32(tblStockDetailsTODT["stockSummaryId"].ToString());
                    if (tblStockDetailsTODT["materialId"] != DBNull.Value)
                        sizeSpecWiseStockTO.MaterialId = Convert.ToInt32(tblStockDetailsTODT["materialId"].ToString());
                    if (tblStockDetailsTODT["prodSpecId"] != DBNull.Value)
                        sizeSpecWiseStockTO.ProdSpecId = Convert.ToInt32(tblStockDetailsTODT["prodSpecId"].ToString());
                    if (tblStockDetailsTODT["confirmedBy"] != DBNull.Value)
                        sizeSpecWiseStockTO.ConfirmedBy = Convert.ToInt32(tblStockDetailsTODT["confirmedBy"].ToString());
                    if (tblStockDetailsTODT["confirmedOn"] != DBNull.Value)
                        sizeSpecWiseStockTO.ConfirmedOn = Convert.ToDateTime(tblStockDetailsTODT["confirmedOn"].ToString());
                    if (tblStockDetailsTODT["noOfBundles"] != DBNull.Value)
                        sizeSpecWiseStockTO.NoOfBundles = Convert.ToDouble(tblStockDetailsTODT["noOfBundles"].ToString());
                    if (tblStockDetailsTODT["totalStock"] != DBNull.Value)
                        sizeSpecWiseStockTO.TotalStock = Convert.ToDouble(tblStockDetailsTODT["totalStock"].ToString());
                    if (tblStockDetailsTODT["prodSpecDesc"] != DBNull.Value)
                        sizeSpecWiseStockTO.ProdSpecDesc = Convert.ToString(tblStockDetailsTODT["prodSpecDesc"].ToString());
                    if (tblStockDetailsTODT["materialDesc"] != DBNull.Value)
                        sizeSpecWiseStockTO.MaterialDesc = Convert.ToString(tblStockDetailsTODT["materialDesc"].ToString());
                    if (tblStockDetailsTODT["balanceStock"] != DBNull.Value)
                        sizeSpecWiseStockTO.BalanceStock = Convert.ToDouble(tblStockDetailsTODT["balanceStock"].ToString());
                    if (tblStockDetailsTODT["todaysStock"] != DBNull.Value)
                        sizeSpecWiseStockTO.TodaysStock = Convert.ToDouble(tblStockDetailsTODT["todaysStock"].ToString());
                    if (tblStockDetailsTODT["brandId"] != DBNull.Value)
                        sizeSpecWiseStockTO.BrandId = Convert.ToInt32(tblStockDetailsTODT["brandId"].ToString());

                    if (tblStockDetailsTODT["isConsolidatedStock"] != DBNull.Value)
                        sizeSpecWiseStockTO.IsConsolidatedStock = Convert.ToInt32(tblStockDetailsTODT["isConsolidatedStock"].ToString());
                    if (tblStockDetailsTODT["prodItemId"] != DBNull.Value)
                        sizeSpecWiseStockTO.ProductItemId = Convert.ToInt32(tblStockDetailsTODT["prodItemId"].ToString());
                    if (tblStockDetailsTODT["displayName"] != DBNull.Value)
                        sizeSpecWiseStockTO.ItemDisplayName = tblStockDetailsTODT["displayName"].ToString();
                    
                    sizeSpecWiseStockTOList.Add(sizeSpecWiseStockTO);
                }
            }
            return sizeSpecWiseStockTOList;
        }

        public List<SizeSpecWiseStockTO> SelectSizeAndSpecWiseStockSummary(DateTime stockDate,DateTime FromDate, DateTime ToDate,int compartmentId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                // Vaibhav [12-April-2018] Added to select stock compartment wise.           
                String compCondition = String.Empty;
                //compCondition = " WHERE  CAST(stockSummary.stockDate  AS DATE) BETWEEN '" + Convert.ToString(FromDate) + "'  AND '" + Convert.ToString(ToDate) + "' ";
                if (compartmentId > 0)
                {
                    compCondition = " WHERE  locationId = " + compartmentId;
                }

                cmdSelect.CommandText = " SELECT prodClass.displayName +'/'+productItem.itemDesc as displayName ,  " +
                                                           " stockSummary.confirmedBy, " +
                                                           " stockSummary.confirmedOn, " +
                                                           " prodSpec.prodSpecDesc, " +
                                                           " material.materialSubType as materialDesc " +
                                                           " , prodItemId, prodSpecId, materialId, brandId , isConsolidatedStock , SUM(stockDtl.noOfBundles) noOfBundles, SUM(todaysStock) todaysStock , SUM(stockDtl.totalStock) totalStock ,SUM(balanceStock) balanceStock   " +
                                                           " FROM tblStockSummary stockSummary " +
                                                           //Reshma Commented For Gajkesari Requirement
                                                           //" INNER JOIN " +
                                                           //" ( " +
                                                           //"    SELECT stockSummaryId,prodItemId, prodSpecId, materialId, brandId , isConsolidatedStock ,  " +
                                                           //"    SUM(noOfBundles) noOfBundles, SUM(todaysStock) todaysStock , SUM(totalStock) totalStock ,SUM(balanceStock) balanceStock" +
                                                           //"    FROM tblStockDetails " +

                                                           //" "+ compCondition +" " +

                                                           //"    GROUP BY stockSummaryId, prodSpecId, materialId , brandId , isConsolidatedStock,prodItemId" +
                                                           //" ) stockDtl " +
                                                           //" ON stockSummary.idStockSummary = stockDtl.stockSummaryId " +
                                                           " left join tblStockDetails stockDtl on  stockSummary.idStockSummary = stockDtl.stockSummaryId  "+
                                                           " LEFT JOIN dimProdSpec prodSpec " +
                                                           " ON prodSpec.idProdSpec = stockDtl.prodSpecId " +
                                                           " LEFT JOIN tblMaterial material " +
                                                           " ON material.idMaterial = stockDtl.materialId " +
                                                           " LEFT JOIN tblProductItem productItem ON  stockDtl.prodItemId=productItem.idProdItem " +
                                                           " LEFT JOIN tblProdClassification prodClass ON productItem.prodClassId = prodClass.idProdClass";



                if (stockDate != new DateTime())
                {
                    cmdSelect.CommandText += " WHERE DAY(stockSummary.stockDate)=" + stockDate.Day + " AND MONTH(stockSummary.stockDate) = " + stockDate.Month +
                                       " AND YEAR(stockSummary.stockDate) = " + stockDate.Year;
                                       
                }
                cmdSelect.CommandText += " where CAST(stockSummary.stockDate  AS DATE) BETWEEN @fromDate  AND  @toDate" +
                    " group by prodSpecId, materialId , brandId , isConsolidatedStock,prodItemId ,prodClass.displayName,+productItem.itemDesc ,   stockSummary.confirmedBy,  " +
                    "   stockSummary.confirmedOn,  prodSpec.prodSpecDesc,  material.materialSubType   , prodItemId, prodSpecId, materialId, brandId , isConsolidatedStock   ";
                   // " ORDER BY prodSpec.displaySequence";
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = FromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = ToDate;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<SizeSpecWiseStockTO> list = ConvertReaderToStockList(sqlReader);
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
        public Int64 GetLastIdStockDtl(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = "select isnull(max(isnull(idStockDtl,0)),0) +  1  as Id from tblStockDetails";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return Convert.ToInt64(cmdSelect.ExecuteScalar());

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        
        public Double  SelectTotalBalanceStock(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 brandId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select COALESCE(SUM(balanceStock),0)as totalBalanceStock from tblstockDetails WHERE materialId=" + materialId + "AND prodCatId=" + prodCatId +
                                    " AND  prodSpecId = " + prodSpecId + " AND brandId = " + brandId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                sqlReader.Read();
              
                    Double totalBalanceStock = Convert.ToDouble(sqlReader["totalBalanceStock"]);
                   

                if (sqlReader != null)
                    sqlReader.Dispose();

                return totalBalanceStock;

              
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblStockDetailsTO> SelectTblStockDetailsList(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 brandId,int compartmentId,int prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                // Vaibhav [08-April-2018] Added to select other item stock.
                if (prodItemId == 0)
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE materialId=" + materialId + "AND prodCatId=" + prodCatId +
                                        " AND  prodSpecId = " + prodSpecId + " AND brandId = " + brandId;
                }
                else
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodItemId = " + prodItemId;
                }

                // Vaibhav [08-April-2018] Added to select stock from compartment.
                if (compartmentId != 0)
                {
                    cmdSelect.CommandText+= " AND stockDtl.locationId=" + compartmentId; 
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockDetailsTO> list = ConvertDTToList(sqlReader);
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

        #endregion

        #region Insertion
        public int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblStockDetailsTO, cmdInsert);
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

        public int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblStockDetailsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblStockDetailsTO tblStockDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblStockDetails]( " +
                                
                                "  [stockSummaryId]" +
                                " ,[locationId]" +
                                " ,[prodCatId]" +
                                " ,[materialId]" +
                                " ,[prodSpecId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[noOfBundles]" +
                                " ,[totalStock]" +
                                " ,[loadedStock]" +
                                " ,[balanceStock]" +
                                " ,[productId]" +
                                " ,[removedStock]" +
                                " ,[brandId]" +
                                " ,[isConsolidatedStock]" +
                                " ,[isInMT]" +
                                " ,[prodItemId]"+
                                " ,[prodNoOfBundles]" +
                                " ,[prodtotalStock]" +

                                " )" +
                    " VALUES (" +
                               
                                "  @StockSummaryId " +
                                " ,@LocationId " +
                                " ,@ProdCatId " +
                                " ,@MaterialId " +
                                " ,@ProdSpecId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@NoOfBundles " +
                                " ,@TotalStock " +
                                " ,@LoadedStock " +
                                " ,@BalanceStock " +
                                " ,@productId " +
                                " ,@removedStock " +
                                " ,@BrandId " +
                                " ,@IsConsolidatedStock " +
                                " ,@IsInMT " +
                                " ,@ProdItemId" +
                                " ,@ProdNoOfBundles " +
                                " ,@ProdtotalStock " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdStockDtl", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.IdStockDtl;
            cmdInsert.Parameters.Add("@StockSummaryId", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.StockSummaryId;
            cmdInsert.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.LocationId);
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.ProdCatId);
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.MaterialId);
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.ProdSpecId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStockDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@NoOfBundles", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.NoOfBundles;
            cmdInsert.Parameters.Add("@TotalStock", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.TotalStock;
            cmdInsert.Parameters.Add("@LoadedStock", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.LoadedStock);
            cmdInsert.Parameters.Add("@BalanceStock", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.BalanceStock);
            cmdInsert.Parameters.Add("@productId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.ProductId);
            cmdInsert.Parameters.Add("@removedStock", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.RemovedStock);
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.BrandId);
            cmdInsert.Parameters.Add("@IsConsolidatedStock", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.IsConsolidatedStock);
            cmdInsert.Parameters.Add("@IsInMT", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.IsInMT;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.ProdItemId;
            cmdInsert.Parameters.Add("@ProdNoOfBundles", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.ProdNoOfBundles;
            cmdInsert.Parameters.Add("@ProdtotalStock", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.ProdtotalStock;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                //cmdInsert.CommandText = Constants.IdentityColumnQuery;
                //tblStockDetailsTO.IdStockDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                cmdInsert.CommandText = Constants.SQL_SELECT_IDENTITY_QUERY;
                tblStockDetailsTO.IdStockDtl = Convert.ToInt64(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblStockDetailsTO, cmdUpdate);
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

        public int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblStockDetailsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblStockDetailsTO tblStockDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStockDetails] SET " +
                            "  [stockSummaryId]= @StockSummaryId" +
                            " ,[locationId]= @LocationId" +
                            " ,[prodCatId]= @ProdCatId" +
                            " ,[materialId]= @MaterialId" +
                            " ,[prodSpecId]= @ProdSpecId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[noOfBundles]= @NoOfBundles" +
                            " ,[totalStock]= @TotalStock" +
                            " ,[loadedStock]= @LoadedStock" +
                            " ,[balanceStock] = @BalanceStock" +
                            " ,[productId] = @productId" +
                            " ,[removedStock] = @removedStock" +
                            " ,[todaysStock] = @todaysStock" +
                            " ,[brandId] = @BrandId " +
                            " ,[isConsolidatedStock] = @IsConsolidatedStock " +
                            " ,[isInMT] = @IsInMT " +
                            " ,[prodItemId] = @ProdItemId " +
                            " ,[prodNoOfBundles]= @ProdNoOfBundles" +
                            " ,[prodtotalStock]= @ProdtotalStock" +
                            " ,[inchId]= @InchId" +
                            " ,[stripId]= @StripId" +
                            " ,[sizeId]= @SizeId" +
                            " ,[thicknessId]= @ThicknessId" +
                            " ,[categoryType]= @CategoryType" +
                            " WHERE  [idStockDtl] = @IdStockDtl";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStockDtl", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.IdStockDtl;
            cmdUpdate.Parameters.Add("@StockSummaryId", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.StockSummaryId;
            cmdUpdate.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.LocationId);
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.ProdCatId);
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.MaterialId);
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblStockDetailsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@NoOfBundles", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.NoOfBundles;
            cmdUpdate.Parameters.Add("@TotalStock", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.TotalStock;
            cmdUpdate.Parameters.Add("@LoadedStock", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.LoadedStock;
            cmdUpdate.Parameters.Add("@BalanceStock", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.BalanceStock;
            cmdUpdate.Parameters.Add("@productId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.ProductId);
            cmdUpdate.Parameters.Add("@removedStock", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.RemovedStock;
            cmdUpdate.Parameters.Add("@todaysStock", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.TodaysStock;
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.BrandId);
            cmdUpdate.Parameters.Add("@IsConsolidatedStock", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.IsConsolidatedStock);
            cmdUpdate.Parameters.Add("@IsInMT", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.IsInMT;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.ProdItemId;
            cmdUpdate.Parameters.Add("@ProdNoOfBundles", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.ProdNoOfBundles;
            cmdUpdate.Parameters.Add("@ProdtotalStock", System.Data.SqlDbType.NVarChar).Value = tblStockDetailsTO.ProdtotalStock;
            cmdUpdate.Parameters.Add("@inchId", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.InchId;
            cmdUpdate.Parameters.Add("@stripId", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.StripId;
            cmdUpdate.Parameters.Add("@sizeId", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.SizeId;
            cmdUpdate.Parameters.Add("@thicknessId", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.ThicknessId;
            cmdUpdate.Parameters.Add("@categoryType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockDetailsTO.CategoryType);
            
            return cmdUpdate.ExecuteNonQuery();
        }

        public int IsExistStockLocwise( int locationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select top 1 1 as cnt from tblStockDetails where locationId = " + locationId + " ";
                cmdSelect.Connection = conn;               
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return Convert.ToInt32(cmdSelect.ExecuteScalar());

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        #endregion

        #region Deletion
        public int DeleteTblStockDetails(Int32 idStockDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStockDtl, cmdDelete);
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

        public int DeleteTblStockDetails(Int32 idStockDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStockDtl, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idStockDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblStockDetails] " +
            " WHERE idStockDtl = " + idStockDtl + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStockDtl", System.Data.SqlDbType.Int).Value = tblStockDetailsTO.IdStockDtl;
            return cmdDelete.ExecuteNonQuery();
        }

       

        #endregion

    }
}
