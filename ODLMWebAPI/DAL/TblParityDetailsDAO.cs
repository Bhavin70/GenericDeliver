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
    public class TblParityDetailsDAO : ITblParityDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblParityDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT parity.* ,  prodCat.prodCateDesc ,prodSpec.prodSpecDesc, material.materialSubType as materialDesc," +
                " ,brandDesc='', itemName='',displayName='' " +
                                  " ,tblParitySummary.brandId " +
                                  " FROM[tblParityDetails] parity" +
                                  " LEFT JOIN dimProdCat prodCat ON parity.prodCatId = prodCat.idProdCat" +
                                  " LEFT JOIN dimProdSpec prodSpec ON parity.prodSpecId = prodSpec.idProdSpec" +
                                  " LEFT JOIN tblMaterial material ON parity.materialId=material.idMaterial" +
                                  " LEFT JOIN tblParitySummary tblParitySummary ON parity.parityId=tblParitySummary.idParity";

            return sqlSelectQry;
        }

        //Sudhir[20-March-2018] Simple Select Query
        public String SqlSimpleSelectQuery()
        {
            String sqlSelectQry = "SELECT parityDtl.*,material.materialSubType AS materialDesc,prodCat.prodCateDesc As prodCateDesc " +
                ",prodSpec.prodSpecDesc As prodSpecDesc,brandDesc='' ,itemName='',displayName=''   " +
                "  FROM  tblParityDetails parityDtl  LEFT JOIN tblMaterial material " +
                " ON parityDtl.materialId = material.idMaterial LEFT JOIN dimProdCat prodCat ON parityDtl.prodCatId = prodCat.idProdCat" +
                " LEFT JOIN dimProdSpec prodSpec ON parityDtl.prodSpecId = prodSpec.idProdSpec ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblParityDetailsTO> SelectAllTblParityDetails(int parityId, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                if (prodSpecId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE parityId=" + parityId;
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE parityId=" + parityId + " AND parity.prodSpecId=" + prodSpecId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);

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

        public List<TblParityDetailsTO> SelectAllTblParityDetails(String parityIds, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                if (prodSpecId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE parityId IN (" + parityIds + ")";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE parityId IN (" + parityIds + ") AND parity.prodSpecId=" + prodSpecId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);

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

        public List<TblParityDetailsTO> SelectAllParityDetailsListByIds(String parityDtlIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE parity.idParityDtl In(" + parityDtlIds + ")";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);

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

        public List<TblParityDetailsTO> SelectAllLatestParityDetails(Int32 stateId, Int32 prodSpecId,Int32 brandId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String sqlQuery = string.Empty;
            try
            {
                if (prodSpecId == 0)
                {
                    sqlQuery = " SELECT material.idMaterial materialId , material.materialSubType AS materialDesc, dimProdCat.idProdCat AS prodCatId , " +
                                            " dimProdCat.prodCateDesc ,dimProdSpec.idProdSpec AS prodSpecId , dimProdSpec.prodSpecDesc, latestParity.idParityDtl , latestParity.parityId,latestParity.parityAmt,latestParity.nonConfParityAmt, " +
                                            " latestParity.remark,latestParity.createdOn,latestParity.createdBy, latestParity.brandId " +
                                            " FROM tblMaterial material " +
                                            " FULL OUTER JOIN dimProdCat " +
                                            " ON 1 = 1 " +
                                            " FULL OUTER JOIN dimProdSpec " +
                                            " ON 1 = 1 " +
                                            " LEFT JOIN " +
                                            " ( " +
                                            "     SELECT parityDtl.* ,paritySum.brandId FROM tblParityDetails parityDtl " +
                                            "     INNER JOIN tblParitySummary paritySum " +
                                            "     ON parityDtl.parityId = paritySum.idParity " +
                                            "     WHERE paritySum.idParity = (SELECT MAX(idParity) FROM tblParitySummary WHERE stateId=" + stateId +  " AND brandId = " + brandId + ") " +
                                            " ) latestParity " +
                                            " ON material.idMaterial = latestParity.materialId " +
                                            " AND dimProdCat.idProdCat = latestParity.prodCatId" +
                                            " AND dimProdSpec.idProdSpec = latestParity.prodSpecId";

                }
                else
                {
                    sqlQuery = " SELECT material.idMaterial materialId , material.materialSubType AS materialDesc, dimProdCat.idProdCat AS prodCatId , " +
                                           " dimProdCat.prodCateDesc ,dimProdSpec.idProdSpec AS prodSpecId , dimProdSpec.prodSpecDesc, latestParity.idParityDtl , latestParity.parityId,latestParity.parityAmt,latestParity.nonConfParityAmt, " +
                                           " latestParity.remark,latestParity.createdOn,latestParity.createdBy, latestParity.brandId " +
                                           " FROM tblMaterial material " +
                                           " FULL OUTER JOIN dimProdCat " +
                                           " ON 1 = 1 " +
                                           " FULL OUTER JOIN dimProdSpec " +
                                           " ON 1 = 1 " +
                                           " LEFT JOIN " +
                                           " ( " +
                                           "     SELECT parityDtl.*,paritySum.brandId FROM tblParityDetails parityDtl " +
                                           "     INNER JOIN tblParitySummary paritySum " +
                                           "     ON parityDtl.parityId = paritySum.idParity " +
                                           "     WHERE parityDtl.prodSpecId=" + prodSpecId + " AND paritySum.idParity = (SELECT MAX(idParity) FROM tblParitySummary WHERE stateId=" + stateId + " AND brandId = " + brandId + ") " +
                                           " ) latestParity " +
                                           " ON material.idMaterial = latestParity.materialId " +
                                           " AND dimProdCat.idProdCat = latestParity.prodCatId" +
                                           " AND dimProdSpec.idProdSpec = latestParity.prodSpecId" +
                                           " WHERE dimProdSpec.idProdSpec=" + prodSpecId;
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);

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

        public TblParityDetailsTO SelectTblParityDetails(Int32 idParityDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idParityDtl = " + idParityDtl + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToList(sqlReader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblParityDetailsTO SelectTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery()  +" WHERE idParityDtl = " + idParityDtl + " ";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToList(sqlReader);
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



        public List<TblParityDetailsTO> SelectAllParityDetails()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSimpleSelectQuery() + " WHERE parityDtl.isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToList(sqlReader);
                if (list != null)
                    return list;
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        /// <summary>
        /// Sudhir[20-MARCH-2018] Added for get List Based on productItemId for other Item Parity Details
        /// </summary>
        /// <returns></returns>
        //public List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 productItemId,Int32 brandId,Int32 prodCatId,Int32 prodSpecId,Int32 materialId)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    SqlDataReader sqlReader = null;
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = SqlSimpleSelectQuery()+ " WHERE ISNULL(parityDtl.prodItemId,0)=" + productItemId +" AND ISNULL(parityDtl.brandId,0)="+brandId
        //                                +" AND ISNULL(parityDtl.prodCatId,0)="+prodCatId+ " AND ISNULL(parityDtl.prodSpecId,0)=" + prodSpecId 
        //                                +" AND ISNULL(parityDtl.materialId,0)="+materialId + "AND parityDtl.isActive="+1;
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblParityDetailsTO> list = ConvertDTToList(sqlReader);
        //        if (list != null)
        //            return list;
        //        else return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (sqlReader != null)
        //            sqlReader.Dispose();
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

      //Aniket[21-01-2019]
      public List<TblParityDetailsTO> SelectParityDetailsForBrand(Int32 fromBrand, Int32 toBrand, Int32 currencyId,Int32 categoryId, Int32 stateId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT  latestParity.idParityDtl, latestParity.parityId, latestParity.parityAmt,latestParity.nonConfParityAmt,"+
                                                           " latestParity.remark,latestParity.createdOn, latestParity.createdBy, latestParity.baseValCorAmt, "+
                                                            " latestParity.freightAmt, latestParity.expenseAmt, latestParity.otherAmt, latestParity.prodItemId, " +
                                                            " latestParity.isActive, latestParity.currencyId  ,material.idMaterial As materialId,  " +
                                                            " material.materialSubType AS materialDesc,prodcat.idProdCat As prodCatId, itemName = '', displayName = '',  " +
                                                            " brand.idBrand As brandId,  brand.brandName As brandDesc, prodCat.prodCateDesc As prodCateDesc , " +
                                                            " prodspec.idProdSpec As prodSpecId, prodSpec.prodSpecDesc As prodSpecDesc, stateName.idState As stateId" +
                                                            " FROM tblMaterial material" +
                                                            " FULL OUTER JOIN dimProdCat prodCat ON 1 = 1 and prodCat.idProdCat = 1   and prodCat.isActive = 1" +
                                                            " FULL OUTER JOIN dimProdSpec prodSpec ON 1 = 1  and prodSpec.isActive = 1" +
                                                            " full outer join dimState stateName ON 1 = 1 and stateName.idState = "+ stateId+
                                                            " full outer join dimBrand brand ON 1 = 1 and brand.idBrand = "+fromBrand+
                                                           " full outer join dimCurrency currency ON 1 = 1 and currency.idCurrency = 1" +
                                                           " LEFT JOIN(select* from tblParityDetails where stateId= "+stateId+"   and isActive = 1 and brandId =" + fromBrand +
                                                           " and prodCatId = prodCatId   and currencyId ="+ currencyId+" ) latestParity" +
                                                     " ON material.idMaterial = latestParity.materialId AND prodCat.idProdCat = latestParity.prodCatId" +
                                                            " AND prodSpec.idProdSpec = latestParity.prodSpecId AND stateName.idState = latestParity.stateId" +
                                                           " AND currency.idCurrency = latestParity.currencyId" +
                                                          "  AND brand.idBrand = latestParity.brandId where idProdCat = prodCatId   order by prodSpec.displaySequence ,materialId";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToList(sqlReader);
                if (list != null)
                    return list;
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Priyanka [28-08-2018]
        public List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 brandId, Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo, Int32 productSpecForRegular, Int32 districtId, Int32 talukaId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String regularSpecString = String.Empty;
            String prodCatIdCondition1 = String.Empty;
            String prodCatIdCondition2 = String.Empty;
            String prodCatIdCondition3 = String.Empty;
            String districtIdCondition = String.Empty;
            String talukaIdCondition = String.Empty;
            String whereCondition = String.Empty;
            try
            {
                conn.Open();
             
                if(prodCatId>0)
                {
                    prodCatIdCondition1 = " and prodCat.idProdCat = " + prodCatId;
                    prodCatIdCondition2 = " and prodCatId = " + prodCatId;
                    prodCatIdCondition3 = " idProdCat = " + prodCatId;
                }
                if (districtId > 0)
                {
                    districtIdCondition = " and districtId = " + districtId;
                }
                if (talukaId > 0)
                {
                    talukaIdCondition = " and talukaId = " + talukaId;
                }
                if (prodCatId>0 || productSpecForRegular>0)
                {
                    whereCondition = " WHERE ";
                }
                if (productSpecForRegular != 0)
                {

                    if(prodCatId>0)
                    {
                        regularSpecString = " AND ";
                    }

                    regularSpecString += " prodSpec.idProdSpec =" + productSpecForRegular;
                }

                if (productSpecInfoListTo == 0)
                {
                    cmdSelect.CommandText = "  SELECT  latestParity.idParityDtl, latestParity.parityId, latestParity.parityAmt,latestParity.nonConfParityAmt," +
                                                           " latestParity.remark,latestParity.createdOn, latestParity.createdBy, latestParity.baseValCorAmt," +
                                                           " latestParity.freightAmt, latestParity.expenseAmt, latestParity.otherAmt, latestParity.prodItemId, " +
                                                           " latestParity.isActive, latestParity.currencyId  ,material.idMaterial As materialId, " +
                                                           " material.materialSubType AS materialDesc,prodcat.idProdCat As prodCatId, itemName = '', displayName='', " +
                                                           " brand.idBrand As brandId,  brand.brandName As brandDesc, prodCat.prodCateDesc As prodCateDesc ," +
                                                           " prodspec.idProdSpec As prodSpecId, prodSpec.prodSpecDesc As prodSpecDesc, stateName.idState As stateId " +
                                                           " FROM tblMaterial material " +
                                                           " FULL OUTER JOIN dimProdCat prodCat ON 1 = 1 "+ prodCatIdCondition1 + " and prodCat.isActive = 1 " +
                                                           " FULL OUTER JOIN dimProdSpec prodSpec ON 1 = 1  and prodSpec.isActive = 1 " +
                                                           " full outer join dimState stateName ON 1 = 1 and stateName.idState = " + stateId +
                                                           " full outer join dimBrand brand ON 1 = 1 and brand.idBrand = " + brandId +
                                                           " full outer join dimCurrency currency ON 1 = 1 and currency.idCurrency = " + currencyId +
                                                           " LEFT JOIN( select * from tblParityDetails where stateId= " + stateId + " and isActive = 1 and brandId = " + brandId +
                                                           prodCatIdCondition2 + " and currencyId = " + currencyId + districtIdCondition + talukaIdCondition + ") latestParity " +
                                                           " ON material.idMaterial = latestParity.materialId AND prodCat.idProdCat = latestParity.prodCatId " +
                                                           " AND prodSpec.idProdSpec = latestParity.prodSpecId AND stateName.idState = latestParity.stateId " +
                                                           " AND currency.idCurrency = latestParity.currencyId " +
                                                           " AND brand.idBrand = latestParity.brandId  "+ whereCondition +prodCatIdCondition3 + regularSpecString + " order by prodSpec.displaySequence ,materialId";
                }
                else
                {
                    cmdSelect.CommandText = " Select  latestParity.idParityDtl, latestParity.parityId, latestParity.parityAmt,latestParity.nonConfParityAmt, " +
                                                           " latestParity.remark,latestParity.createdOn, latestParity.createdBy, latestParity.baseValCorAmt, " +
                                                           " latestParity.freightAmt, latestParity.expenseAmt, latestParity.materialId ,materialDesc = ''," +
                                                           " prodCateDesc ='', prodSpecDesc ='',latestParity.prodSpecId, latestParity.brandId, brandDesc='', " +
                                                           " latestParity.prodCatId ,latestParity.otherAmt, " +
                                                           " latestParity.isActive, currency.idCurrency  As currencyId,prodClass.displayName ,prodItem.itemName ," +
                                                           " prodItem.idProdItem as prodItemId,stateName.idState As stateId from tblProductItem prodItem " +
                                                           " LEft Join tblProdClassification prodClass On prodClass.idProdClass = prodItem.prodClassId " +
                                                           " LEFT JOIN(select * from tblParityDetails where stateId= " + stateId + " and isActive = 1 " +
                                                           " and currencyId = " + currencyId + districtIdCondition + talukaIdCondition + ") latestParity ON prodItem.idProdItem = latestParity.prodItemId " +
                                                           " FULL outer join dimCurrency currency ON 1 = 1 and currency.idCurrency =  " + currencyId +
                                                           " FULL outer join dimState stateName ON 1 = 1 and stateName.idState =  " + stateId +
                                                           " where prodItem.prodClassId = " + productSpecInfoListTo + "and isParity = 1";

                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToList(sqlReader);
                if (list != null)
                    return list;
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        /// <summary>
        /// Sudhir[23-MARCH-2018] Added for Get ParityDetail List based on Booking DateTime and Other Combination
        /// </summary>
        /// <returns></returns>
        public List<TblParityDetailsTO> SelectParityDetailToListOnBooking(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 brandId, Int32 stateId, DateTime boookingDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSimpleSelectQuery() + " WHERE ISNULL(parityDtl.prodItemId,0)=" + productItemId + " AND ISNULL(parityDtl.brandId,0)=" + brandId
                                        + " AND ISNULL(parityDtl.prodCatId,0)=" + prodCatId + " AND ISNULL(parityDtl.prodSpecId,0)=" + prodSpecId
                                        + " AND ISNULL(parityDtl.materialId,0)=" + materialId
                                        + " AND  parityDtl.stateId=" + stateId
                                        + "  AND parityDtl.createdOn <=  @BookingDate order by parityDtl.createdOn DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@BookingDate", System.Data.SqlDbType.DateTime).Value = boookingDate;//.ToString(Constants.AzureDateFormat);
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToList(sqlReader);
                if (list != null)
                    return list;
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Aniket[29-01-2019] for copy from one brand to multiple brand, fetch parity details for selected brand
        public List<TblParityDetailsTO> GetParityDetailsForBrand(Int32 brandId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblParityDetails where brandId=@brandId and isActive=1";
                cmdSelect.Parameters.AddWithValue("@brandId", DbType.Int32).Value = brandId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTTo(sqlReader);
                if (list != null)
                    return list;
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblParityDetailsTO> ConvertDTTo(SqlDataReader tblParityDetailsTODT)
        {
            List<TblParityDetailsTO> tblParityDetailsTOList = new List<TblParityDetailsTO>();
            if (tblParityDetailsTODT != null)
            {
                while (tblParityDetailsTODT.Read())
                {
                    TblParityDetailsTO tblParityDetailsTONew = new TblParityDetailsTO();
                    if (tblParityDetailsTODT["idParityDtl"] != DBNull.Value)
                        tblParityDetailsTONew.IdParityDtl = Convert.ToInt32(tblParityDetailsTODT["idParityDtl"].ToString());
                    if (tblParityDetailsTODT["parityId"] != DBNull.Value)
                        tblParityDetailsTONew.ParityId = Convert.ToInt32(tblParityDetailsTODT["parityId"].ToString());
                    if (tblParityDetailsTODT["materialId"] != DBNull.Value)
                        tblParityDetailsTONew.MaterialId = Convert.ToInt32(tblParityDetailsTODT["materialId"].ToString());
                    if (tblParityDetailsTODT["createdBy"] != DBNull.Value)
                        tblParityDetailsTONew.CreatedBy = Convert.ToInt32(tblParityDetailsTODT["createdBy"].ToString());
                    if (tblParityDetailsTODT["createdOn"] != DBNull.Value)
                        tblParityDetailsTONew.CreatedOn = Convert.ToDateTime(tblParityDetailsTODT["createdOn"].ToString());
                    if (tblParityDetailsTODT["parityAmt"] != DBNull.Value)
                        tblParityDetailsTONew.ParityAmt = Convert.ToDouble(tblParityDetailsTODT["parityAmt"].ToString());
                    if (tblParityDetailsTODT["nonConfParityAmt"] != DBNull.Value)
                        tblParityDetailsTONew.NonConfParityAmt = Convert.ToDouble(tblParityDetailsTODT["nonConfParityAmt"].ToString());
                    if (tblParityDetailsTODT["remark"] != DBNull.Value)
                        tblParityDetailsTONew.Remark = Convert.ToString(tblParityDetailsTODT["remark"].ToString());
                    if (tblParityDetailsTODT["prodCatId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdCatId = Convert.ToInt32(tblParityDetailsTODT["prodCatId"].ToString());
                  
                  
                    if (tblParityDetailsTODT["prodSpecId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdSpecId = Convert.ToInt32(tblParityDetailsTODT["prodSpecId"].ToString());
                 
                    if (tblParityDetailsTODT["brandId"] != DBNull.Value)
                        tblParityDetailsTONew.BrandId = Convert.ToInt32(tblParityDetailsTODT["brandId"].ToString());
                    if (tblParityDetailsTODT["stateId"] != DBNull.Value)
                        tblParityDetailsTONew.StateId = Convert.ToInt32(tblParityDetailsTODT["stateId"].ToString());
                   
                    if (tblParityDetailsTODT["freightAmt"] != DBNull.Value)
                        tblParityDetailsTONew.FreightAmt = Convert.ToDouble(tblParityDetailsTODT["freightAmt"].ToString());
                    if (tblParityDetailsTODT["expenseAmt"] != DBNull.Value)
                        tblParityDetailsTONew.ExpenseAmt = Convert.ToDouble(tblParityDetailsTODT["expenseAmt"].ToString());
                    if (tblParityDetailsTODT["otherAmt"] != DBNull.Value)
                        tblParityDetailsTONew.OtherAmt = Convert.ToDouble(tblParityDetailsTODT["otherAmt"].ToString());
                    if (tblParityDetailsTODT["prodItemId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdItemId = Convert.ToInt32(tblParityDetailsTODT["prodItemId"].ToString());
                    if (tblParityDetailsTODT["isActive"] != DBNull.Value)
                        tblParityDetailsTONew.IsActive = Convert.ToInt32(tblParityDetailsTODT["isActive"].ToString());
                 

                    if (tblParityDetailsTODT["currencyId"] != DBNull.Value)
                        tblParityDetailsTONew.CurrencyId = Convert.ToInt32(tblParityDetailsTODT["currencyId"].ToString());

                    //Added by Dhananjay [2020-11-25] 
                    if (tblParityDetailsTODT["districtId"] != DBNull.Value)
                        tblParityDetailsTONew.DistrictId = Convert.ToInt32(tblParityDetailsTODT["districtId"].ToString());
                    if (tblParityDetailsTODT["talukaId"] != DBNull.Value)
                        tblParityDetailsTONew.TalukaId = Convert.ToInt32(tblParityDetailsTODT["talukaId"].ToString());

                    tblParityDetailsTOList.Add(tblParityDetailsTONew);
                }
            }
            return tblParityDetailsTOList;
        }
        public List<TblParityDetailsTO> ConvertDTToList(SqlDataReader tblParityDetailsTODT)
        {
            List<TblParityDetailsTO> tblParityDetailsTOList = new List<TblParityDetailsTO>();
            if (tblParityDetailsTODT != null)
            {
                while (tblParityDetailsTODT.Read())
                {
                    TblParityDetailsTO tblParityDetailsTONew = new TblParityDetailsTO();
                    if (tblParityDetailsTODT["idParityDtl"] != DBNull.Value)
                        tblParityDetailsTONew.IdParityDtl = Convert.ToInt32(tblParityDetailsTODT["idParityDtl"].ToString());
                    if (tblParityDetailsTODT["parityId"] != DBNull.Value)
                        tblParityDetailsTONew.ParityId = Convert.ToInt32(tblParityDetailsTODT["parityId"].ToString());
                    if (tblParityDetailsTODT["materialId"] != DBNull.Value)
                        tblParityDetailsTONew.MaterialId = Convert.ToInt32(tblParityDetailsTODT["materialId"].ToString());
                    if (tblParityDetailsTODT["createdBy"] != DBNull.Value)
                        tblParityDetailsTONew.CreatedBy = Convert.ToInt32(tblParityDetailsTODT["createdBy"].ToString());
                    if (tblParityDetailsTODT["createdOn"] != DBNull.Value)
                        tblParityDetailsTONew.CreatedOn = Convert.ToDateTime(tblParityDetailsTODT["createdOn"].ToString());
                    if (tblParityDetailsTODT["parityAmt"] != DBNull.Value)
                        tblParityDetailsTONew.ParityAmt = Convert.ToDouble(tblParityDetailsTODT["parityAmt"].ToString());
                    if (tblParityDetailsTODT["nonConfParityAmt"] != DBNull.Value)
                        tblParityDetailsTONew.NonConfParityAmt = Convert.ToDouble(tblParityDetailsTODT["nonConfParityAmt"].ToString());
                    if (tblParityDetailsTODT["remark"] != DBNull.Value)
                        tblParityDetailsTONew.Remark = Convert.ToString(tblParityDetailsTODT["remark"].ToString());
                    if (tblParityDetailsTODT["prodCatId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdCatId = Convert.ToInt32(tblParityDetailsTODT["prodCatId"].ToString());
                    if (tblParityDetailsTODT["prodCateDesc"] != DBNull.Value)
                        tblParityDetailsTONew.ProdCatDesc = Convert.ToString(tblParityDetailsTODT["prodCateDesc"].ToString());
                    if (tblParityDetailsTODT["materialDesc"] != DBNull.Value)
                        tblParityDetailsTONew.MaterialDesc = Convert.ToString(tblParityDetailsTODT["materialDesc"].ToString());
                    if (tblParityDetailsTODT["prodSpecId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdSpecId = Convert.ToInt32(tblParityDetailsTODT["prodSpecId"].ToString());
                    if (tblParityDetailsTODT["prodSpecDesc"] != DBNull.Value)
                        tblParityDetailsTONew.ProdSpecDesc = Convert.ToString(tblParityDetailsTODT["prodSpecDesc"].ToString());
                    if (tblParityDetailsTODT["brandId"] != DBNull.Value)
                        tblParityDetailsTONew.BrandId = Convert.ToInt32(tblParityDetailsTODT["brandId"].ToString());
                    if (tblParityDetailsTODT["stateId"] != DBNull.Value)
                        tblParityDetailsTONew.StateId = Convert.ToInt32(tblParityDetailsTODT["stateId"].ToString());
                    if (tblParityDetailsTODT["baseValCorAmt"] != DBNull.Value)
                        tblParityDetailsTONew.BaseValCorAmt = Convert.ToDouble(tblParityDetailsTODT["baseValCorAmt"].ToString());
                    if (tblParityDetailsTODT["freightAmt"] != DBNull.Value)
                        tblParityDetailsTONew.FreightAmt = Convert.ToDouble(tblParityDetailsTODT["freightAmt"].ToString());
                    if (tblParityDetailsTODT["expenseAmt"] != DBNull.Value)
                        tblParityDetailsTONew.ExpenseAmt = Convert.ToDouble(tblParityDetailsTODT["expenseAmt"].ToString());
                    if (tblParityDetailsTODT["otherAmt"] != DBNull.Value)
                        tblParityDetailsTONew.OtherAmt = Convert.ToDouble(tblParityDetailsTODT["otherAmt"].ToString());
                    if (tblParityDetailsTODT["prodItemId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdItemId = Convert.ToInt32(tblParityDetailsTODT["prodItemId"].ToString());
                    if (tblParityDetailsTODT["isActive"] != DBNull.Value)
                        tblParityDetailsTONew.IsActive = Convert.ToInt32(tblParityDetailsTODT["isActive"].ToString());
                    if (tblParityDetailsTODT["brandDesc"] != DBNull.Value)
                        tblParityDetailsTONew.BrandDesc = Convert.ToString(tblParityDetailsTODT["brandDesc"].ToString());
                    if (tblParityDetailsTODT["currencyId"] != DBNull.Value)
                        tblParityDetailsTONew.CurrencyId = Convert.ToInt32(tblParityDetailsTODT["currencyId"].ToString());
                    if (tblParityDetailsTODT["itemName"] != DBNull.Value)
                        tblParityDetailsTONew.ItemName = Convert.ToString(tblParityDetailsTODT["itemName"].ToString());
                    if (tblParityDetailsTODT["displayName"] != DBNull.Value)
                        tblParityDetailsTONew.DisplayName = Convert.ToString(tblParityDetailsTODT["displayName"].ToString());
                    if (tblParityDetailsTONew.ProdItemId > 0)
                    {
                        tblParityDetailsTONew.DisplayName = tblParityDetailsTONew.DisplayName + "/" + tblParityDetailsTONew.ItemName;
                    }
                    else
                    {
                        tblParityDetailsTONew.DisplayName = tblParityDetailsTONew.MaterialDesc + "-" + tblParityDetailsTONew.ProdCatDesc + "-" + tblParityDetailsTONew.ProdSpecDesc
                                + "(" + tblParityDetailsTONew.BrandDesc + ")";
                    }

                    tblParityDetailsTOList.Add(tblParityDetailsTONew);
                }
            }
            return tblParityDetailsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblParityDetailsTO, cmdInsert);
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

        public int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblParityDetailsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblParityDetailsTO tblParityDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblParityDetails]( " +
            //"  [idParityDtl]" +
            " [parityId]" +
            " ,[materialId]" +
            " ,[prodSpecId]" +
            " ,[prodCatId]" +
            " ,[createdBy]" +
            " ,[stateId]" +
            " ,[brandId]" +
            " ,[prodItemId]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[parityAmt]" +
            " ,[nonConfParityAmt]" +
            " ,[baseValCorAmt]" +
            " ,[freightAmt]" +
            " ,[expenseAmt]" +
            " ,[otherAmt]" +
            " ,[remark]" +
            " ,[currencyId]" +
            " ,[districtId]" +
            " ,[talukaId]" +
            " )" +
" VALUES (" +
            //"  @IdParityDtl " +
            " @ParityId " +
            " ,@MaterialId " +
            " ,@ProdSpecId " +
            " ,@ProdCatId " +
            " ,@CreatedBy " +
            " ,@StateId " +
            " ,@BrandId " +
            " ,@ProdItemId " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@ParityAmt " +
            " ,@NonConfParityAmt " +
            " ,@BaseValCorAmt " +
            " ,@FreightAmt " +
            " ,@ExpenseAmt " +
            " ,@OtherAmt " +
            " ,@Remark " +
            " ,@CurrencyId "+
            " ,@DistrictId " +
            " ,@TalukaId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdParityDtl", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.IdParityDtl;
            cmdInsert.Parameters.Add("@ParityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ParityId);
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.MaterialId);
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdSpecId);
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdCatId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.CreatedBy);
            cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.StateId);
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.BrandId);
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdItemId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.IsActive);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParityDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@ParityAmt", System.Data.SqlDbType.Decimal).Value = tblParityDetailsTO.ParityAmt;
            cmdInsert.Parameters.Add("@NonConfParityAmt", System.Data.SqlDbType.Decimal).Value = tblParityDetailsTO.NonConfParityAmt;
            cmdInsert.Parameters.Add("@BaseValCorAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.BaseValCorAmt);
            cmdInsert.Parameters.Add("@FreightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.FreightAmt);
            cmdInsert.Parameters.Add("@ExpenseAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ExpenseAmt);
            cmdInsert.Parameters.Add("@OtherAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.OtherAmt);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.Remark);
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.CurrencyId);
            cmdInsert.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.DistrictId);
            cmdInsert.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.TalukaId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblParityDetailsTO.IdParityDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblParityDetailsTO, cmdUpdate);
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

        public int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblParityDetailsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblParityDetailsTO tblParityDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblParityDetails] SET " +
            //"  [idParityDtl] = @IdParityDtl" +
            "  [parityId]= @ParityId" +
            " ,[materialId]= @MaterialId" +
            " ,[prodSpecId]= @ProdSpecId" +
            " ,[prodCatId]= @ProdCatId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[stateId]= @StateId" +
            " ,[brandId]= @BrandId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[parityAmt]= @ParityAmt" +
            " ,[nonConfParityAmt]= @NonConfParityAmt" +
            " ,[baseValCorAmt]= @BaseValCorAmt" +
            " ,[freightAmt]= @FreightAmt" +
            " ,[expenseAmt]= @ExpenseAmt" +
            " ,[otherAmt]= @OtherAmt" +
            " ,[remark] = @Remark" +
            " ,[currencyId] @CurrencyId" +
            " ,[districtId]= @DistrictId" +
            " ,[talukaId]= @TalukaId" +
            " WHERE [idParityDtl] = @IdParityDtl ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdParityDtl", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.IdParityDtl;
            cmdUpdate.Parameters.Add("@ParityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ParityId);
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.MaterialId);
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdCatId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.StateId);
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.BrandId);
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdItemId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.IsActive);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParityDetailsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ParityAmt", System.Data.SqlDbType.Decimal).Value = tblParityDetailsTO.ParityAmt;
            cmdUpdate.Parameters.Add("@NonConfParityAmt", System.Data.SqlDbType.Decimal).Value = tblParityDetailsTO.NonConfParityAmt;
            cmdUpdate.Parameters.Add("@BaseValCorAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.BaseValCorAmt);
            cmdUpdate.Parameters.Add("@FreightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.FreightAmt);
            cmdUpdate.Parameters.Add("@ExpenseAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ExpenseAmt);
            cmdUpdate.Parameters.Add("@OtherAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.OtherAmt);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.Remark);
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.CurrencyId);
            cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.DistrictId);
            cmdUpdate.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.TalukaId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblParityDetails(Int32 idParityDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idParityDtl, cmdDelete);
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

        public int DeleteTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idParityDtl, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idParityDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = " DELETE FROM [tblParityDetails] " +
                                     " WHERE idParityDtl = " + idParityDtl + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idParityDtl", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.IdParityDtl;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeactivateAllParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblParityDetails] SET " +
                                   " [isActive]= @IsActive" +
                                   " WHERE ISNULL(brandId,0)=@BrandId " +
                                   " AND ISNULL(prodItemId,0)=@ProductItemId" +
                                   " AND ISNULL(materialId,0)=@MaterialId" +
                                   " AND ISNULL(prodSpecId,0)=@ProdSpecId" +
                                   " AND ISNULL(prodCatId,0)=@ProdCatId " +
                                   " AND ISNULL(stateId,0)=@StateId " +
                                   " AND ISNULL(prodItemId,0)=@ProdItemId "+
                                   " AND ISNULL(districtId,0)=@DistrictId " +
                                   " AND ISNULL(talukaId,0)=@TalukaId " +
                                   " AND isActive=1";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.BrandId;
                cmdUpdate.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdItemId;
                cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.MaterialId;
                cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdSpecId;
                cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdCatId;
                cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdItemId;
                cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.StateId;
                cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.DistrictId;
                cmdUpdate.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.TalukaId;

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

        /// <summary>
        /// Sudhir[21-MARCH-2018] Added for Deactivating Related Parity Details Record  
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="productItemId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeactivateAllParityDetailsForUpdate(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblParityDetails] SET " +
                                   " [isActive]= @IsActive" +
                                   " WHERE ISNULL(brandId,0)=@BrandId " +
                                   " AND ISNULL(prodItemId,0)=@ProductItemId"+
                                   " AND ISNULL(stateId,0)=@StateId"+
                                   " AND ISNULL(materialId,0)=@MaterialId"+
                                   " AND ISNULL(prodSpecId,0)=@ProdSpecId"+
                                   " AND ISNULL(prodCatId,0)=@ProdCatId "+
                                   " AND ISNULL(districtId,0)=@DistrictId " +
                                   " AND ISNULL(talukaId,0)=@TalukaId " +
                                   " AND isActive=1";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.BrandId;
                cmdUpdate.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdItemId;
                cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.StateId;
                cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.MaterialId;
                cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdSpecId;
                cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdCatId;
                cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.DistrictId;
                cmdUpdate.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.TalukaId;

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
        #endregion

    }
}
