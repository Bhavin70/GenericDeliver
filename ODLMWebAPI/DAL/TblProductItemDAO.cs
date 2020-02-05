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
    public class TblProductItemDAO : ITblProductItemDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblProductItemDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = "  SELECT p.*,uom.weightMeasurUnitDesc,conversionuom.weightMeasurUnitDesc as conversionweightMeasurUnitDesc FROM tblProductItem p " +
                                  " LEFT JOIN dimunitmeasures uom on uom.idWeightMeasurUnit = p.weightMeasureUnitId " +
                                  " LEFT JOIN dimunitmeasures conversionuom on conversionuom.idWeightMeasurUnit = p.conversionUnitOfMeasure "; 
            return sqlSelectQry;
        }
        public String SqlSelectQueryFortblProductItemPurchaseExt()
        {
            String sqlSelectQry = " SELECT * FROM [tblProductItemPurchaseExt]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblProductItemTO> SelectAllTblProductItem(Int32 specificationId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (specificationId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE p.isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE p.isActive=1 AND prodClassId=" + specificationId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
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
         
        public TblProductItemTO SelectTblProductItem(Int32 idProdItem,SqlConnection conn = null,SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idProdItem = " + idProdItem +" ";
                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                if (tran == null)
                    conn.Close();
            }
        }

        public List<TblProductItemTO> ConvertDTToList(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["prodClassId"] != DBNull.Value)
                        tblProductItemTONew.ProdClassId = Convert.ToInt32(tblProductItemTODT["prodClassId"].ToString());
                    if (tblProductItemTODT["createdBy"] != DBNull.Value)
                        tblProductItemTONew.CreatedBy = Convert.ToInt32(tblProductItemTODT["createdBy"].ToString());
                    if (tblProductItemTODT["updatedBy"] != DBNull.Value)
                        tblProductItemTONew.UpdatedBy = Convert.ToInt32(tblProductItemTODT["updatedBy"].ToString());
                    if (tblProductItemTODT["createdOn"] != DBNull.Value)
                        tblProductItemTONew.CreatedOn = Convert.ToDateTime(tblProductItemTODT["createdOn"].ToString());
                    if (tblProductItemTODT["updatedOn"] != DBNull.Value)
                        tblProductItemTONew.UpdatedOn = Convert.ToDateTime(tblProductItemTODT["updatedOn"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
                    if (tblProductItemTODT["remark"] != DBNull.Value)
                        tblProductItemTONew.Remark = Convert.ToString(tblProductItemTODT["remark"].ToString());
                    if (tblProductItemTODT["isActive"] != DBNull.Value)
                        tblProductItemTONew.IsActive = Convert.ToInt32(tblProductItemTODT["isActive"].ToString());
                    if (tblProductItemTODT["weightMeasureUnitId"] != DBNull.Value)
                        tblProductItemTONew.WeightMeasureUnitId = Convert.ToInt32(tblProductItemTODT["weightMeasureUnitId"]);
                    if (tblProductItemTODT["conversionUnitOfMeasure"] != DBNull.Value)
                        tblProductItemTONew.ConversionUnitOfMeasure = Convert.ToInt32(tblProductItemTODT["conversionUnitOfMeasure"]);
                    if (tblProductItemTODT["conversionFactor"] != DBNull.Value)
                        tblProductItemTONew.ConversionFactor = Convert.ToDouble(tblProductItemTODT["conversionFactor"]);
                    if (tblProductItemTODT["isStockRequire"] != DBNull.Value)
                        tblProductItemTONew.IsStockRequire = Convert.ToInt32(tblProductItemTODT["isStockRequire"].ToString());
                    if (tblProductItemTODT["isParity"] != DBNull.Value)
                        tblProductItemTONew.IsParity = Convert.ToInt32(tblProductItemTODT["isParity"].ToString());
                    if (tblProductItemTODT["basePrice"] != DBNull.Value)
                        tblProductItemTONew.BasePrice = Convert.ToDouble(tblProductItemTODT["basePrice"].ToString());
                    if (tblProductItemTODT["isBaseItemForRate"] != DBNull.Value)
                        tblProductItemTONew.IsBaseItemForRate = Convert.ToInt32(tblProductItemTODT["isBaseItemForRate"].ToString());
                    if (tblProductItemTODT["isNonCommercialItem"] != DBNull.Value)
                        tblProductItemTONew.IsNonCommercialItem = Convert.ToInt32(tblProductItemTODT["isNonCommercialItem"].ToString());
                    //Priyanka [16-05-2018]
                    if (tblProductItemTODT["codeTypeId"] != DBNull.Value)
                        tblProductItemTONew.CodeTypeId = Convert.ToInt32(tblProductItemTODT["codeTypeId"].ToString());
                    //Aniket [6-6-2019]
                    if (tblProductItemTODT["weightMeasurUnitDesc"] != DBNull.Value)
                        tblProductItemTONew.WeightMeasureUnitDesc = Convert.ToString(tblProductItemTODT["weightMeasurUnitDesc"].ToString());

                    if (tblProductItemTODT["conversionweightMeasurUnitDesc"] != DBNull.Value)
                        tblProductItemTONew.ConversionweightMeasurUnitDesc = Convert.ToString(tblProductItemTODT["conversionweightMeasurUnitDesc"].ToString());

                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }

        public List<TblProductItemTO> ConvertDTToListForUpdate(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["prodClassId"] != DBNull.Value)
                        tblProductItemTONew.ProdClassId = Convert.ToInt32(tblProductItemTODT["prodClassId"].ToString());
                    if (tblProductItemTODT["createdBy"] != DBNull.Value)
                        tblProductItemTONew.CreatedBy = Convert.ToInt32(tblProductItemTODT["createdBy"].ToString());
                    if (tblProductItemTODT["updatedBy"] != DBNull.Value)
                        tblProductItemTONew.UpdatedBy = Convert.ToInt32(tblProductItemTODT["updatedBy"].ToString());
                    if (tblProductItemTODT["createdOn"] != DBNull.Value)
                        tblProductItemTONew.CreatedOn = Convert.ToDateTime(tblProductItemTODT["createdOn"].ToString());
                    if (tblProductItemTODT["updatedOn"] != DBNull.Value)
                        tblProductItemTONew.UpdatedOn = Convert.ToDateTime(tblProductItemTODT["updatedOn"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
                    if (tblProductItemTODT["remark"] != DBNull.Value)
                        tblProductItemTONew.Remark = Convert.ToString(tblProductItemTODT["remark"].ToString());
                    if (tblProductItemTODT["isActive"] != DBNull.Value)
                        tblProductItemTONew.IsActive = Convert.ToInt32(tblProductItemTODT["isActive"].ToString());
                    if (tblProductItemTODT["weightMeasureUnitId"] != DBNull.Value)
                        tblProductItemTONew.WeightMeasureUnitId = Convert.ToInt32(tblProductItemTODT["weightMeasureUnitId"]);
                    if (tblProductItemTODT["conversionUnitOfMeasure"] != DBNull.Value)
                        tblProductItemTONew.ConversionUnitOfMeasure = Convert.ToInt32(tblProductItemTODT["conversionUnitOfMeasure"]);
                    if (tblProductItemTODT["conversionFactor"] != DBNull.Value)
                        tblProductItemTONew.ConversionFactor = Convert.ToDouble(tblProductItemTODT["conversionFactor"]);
                    if (tblProductItemTODT["isStockRequire"] != DBNull.Value)
                        tblProductItemTONew.IsStockRequire = Convert.ToInt32(tblProductItemTODT["isStockRequire"].ToString());
                    if (tblProductItemTODT["displayName"] != DBNull.Value)
                        tblProductItemTONew.ProdClassDisplayName = tblProductItemTODT["displayName"].ToString();
                    if (tblProductItemTODT["isBaseItemForRate"] != DBNull.Value)
                        tblProductItemTONew.IsBaseItemForRate = Convert.ToInt32(tblProductItemTODT["isBaseItemForRate"].ToString());
                    if (tblProductItemTODT["isNonCommercialItem"] != DBNull.Value)
                        tblProductItemTONew.IsNonCommercialItem = Convert.ToInt32(tblProductItemTODT["isNonCommercialItem"].ToString());
                    //if (tblProductItemTODT["isParity"] != DBNull.Value)
                    //    tblProductItemTONew.IsParity = Convert.ToInt32(tblProductItemTODT["isParity"].ToString());

                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }

        //sudhir[12-jan-2018] added for getlistof items whose stockupdate is require.
        public List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (isStockRequire == 0)
                    cmdSelect.CommandText = " SELECT ProdClassification.displayName,* FROM [tblProductItem] ProductItem " +
                                            " INNER JOIN tblProdClassification ProdClassification ON" +
                                            " ProductItem.prodClassId = ProdClassification.idProdClass WHERE ProductItem.isActive=1 AND isStockRequire=0";
                else
                    cmdSelect.CommandText = " SELECT ProdClassification.displayName,* FROM [tblProductItem] ProductItem "+
                                            " INNER JOIN tblProdClassification ProdClassification ON" +
                                            " ProductItem.prodClassId = ProdClassification.idProdClass WHERE ProductItem.isActive=1 AND ProductItem.isStockRequire=1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToListForUpdate(reader);
                return list;
            }
            catch (Exception ex)
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

        public List<TblProductItemTO> SelectProductItemListStockTOList(int activeYn)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT ProdClassification.displayName,* FROM [tblProductItem] ProductItem " +
                                            " INNER JOIN tblProdClassification ProdClassification ON" +
                                            " ProductItem.prodClassId = ProdClassification.idProdClass " +
                                            " WHERE ISNULL(ProductItem.isActive, 0) = " + activeYn;
            

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToListForUpdate(reader);
                return list;
            }
            catch (Exception ex)
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

        //Sudhir[15-MAR-2018] Added for get List of ProductItem based on Category/SubCategory/Specification
        public List<TblProductItemTO> SelectListOfProductItemTOOnprdClassId(string prodClassIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClassId IN (" + prodClassIds + ")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
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
                conn.Close();
            }
        }

        //Sudhir[20-MAR-2018] Added for Get ProductItem List which has Parity Yes.
        public List<DropDownTO> SelectProductItemListIsParity(Int32 isParity)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isParity=" + isParity ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                while (reader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (reader["idProdItem"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(reader["idProdItem"].ToString());
                    if (reader["itemName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(reader["itemName"].ToString());

                    list.Add(dropDownTONew);
                }
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
                conn.Close();
            }
        }

        #endregion

        #region Insertion
        public int InsertTblProductItem(TblProductItemTO tblProductItemTO,TblPurchaseItemMasterTO tblPurchaseItemMasterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProductItemTO, cmdInsert);
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

        public int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProductItemTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProductItem]( " + 
                            "  [prodClassId]" +
                            " ,[createdBy]" +
                            " ,[updatedBy]" +
                            " ,[createdOn]" +
                            " ,[updatedOn]" +
                            " ,[itemName]" +
                            " ,[itemDesc]" +
                            " ,[remark]" +
                            " ,[isActive]" +
                            " ,[weightMeasureUnitId]" +
                            " ,[conversionUnitOfMeasure]" +
                            " ,[conversionFactor]" +
                            ", [isStockRequire]"+
                            ", [isParity]" +
                            ", [basePrice]" +
                            ", [codeTypeId]" +        
                             ",[isBaseItemForRate]" +
                             ",[IsNonCommercialItem]" +
                             " )" +
                " VALUES (" +
                            "  @ProdClassId " +
                            " ,@CreatedBy " +
                            " ,@UpdatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedOn " +
                            " ,@ItemName " +
                            " ,@ItemDesc " +
                            " ,@Remark " +
                            " ,@isActive " +
                            " ,@weightMeasureUnitId " +
                            " ,@conversionUnitOfMeasure " +
                            " ,@conversionFactor " +
                            " ,@isStockRequire"+
                            " ,@IsParity" +
                            ", @BasePrice" +
                            ", @CodeTypeId" +
                            ", @isBase" +
                            ", @nonCommertial" +
                            //Priyanka [16-05-208]
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblProductItemTO.ProdClassId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProductItemTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProductItemTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.UpdatedOn);
            cmdInsert.Parameters.Add("@ItemName", System.Data.SqlDbType.NVarChar).Value = tblProductItemTO.ItemName;
            cmdInsert.Parameters.Add("@ItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemDesc);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.Remark);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value =  tblProductItemTO.IsActive;
            cmdInsert.Parameters.Add("@weightMeasureUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.WeightMeasureUnitId);
            cmdInsert.Parameters.Add("@conversionUnitOfMeasure", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionUnitOfMeasure);
            cmdInsert.Parameters.Add("@conversionFactor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionFactor);
            cmdInsert.Parameters.Add("@isStockRequire", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsStockRequire;
            cmdInsert.Parameters.Add("@isParity", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsParity;
            cmdInsert.Parameters.Add("@BasePrice", System.Data.SqlDbType.Decimal).Value = tblProductItemTO.BasePrice;
            cmdInsert.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CodeTypeId);
            cmdInsert.Parameters.Add("@isBase", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsBaseItemForRate);
            cmdInsert.Parameters.Add("@nonCommertial", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsNonCommercialItem);

            if (cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblProductItemTO.IdProdItem = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        public int InsertTblPurchaseItemMaster(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommandForPurchaseItemMasterTO(tblPurchaseItemMasterTO, cmdInsert);
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
        public int ExecuteInsertionCommandForPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlCommand cmdInsert)
        {
           
            String sqlQuery = @" INSERT INTO [tblProductItemPurchaseExt]( " +
                         "  [gstCodeTypeId]" +
                         " ,[createdBy]" +
                         " ,[updatedBy]" +
                         " ,[createdOn]" +
                         " ,[updatedOn]" +
                         " ,[isActive]" +
                         " ,[priority]" +
                         " ,[currencyId]" +
                         " ,[supplierOrgId]" +
                         " ,[currencyRate]" +
                         " ,[basicRate]" +
                         ", [gstItemCode]" +
                         ", [discount]" +
                         ", [codeTypeId]" +
                          ",[pf]" +
                          ",[freight]" +
                          ",[deliveryPeriodInDays]" +
                          ",[multiplicationFactor]" +
                          ",[minimumOrderQty]" +
                          ",[supplierAddress]" +
                          ",[mfgCatlogNo]" +
                          ",[weightMeasurUnitId]" +
                          ",[prodItemId]" +
                          ",[itemPerPurchaseUnit]" +
                          ",[lengthmm]" +
                          ",[widthmm]" +
                          ",[heightmm]" +
                          ",[volumeccm]" +
                          ",[weightkg]" +
                          " )" +
             " VALUES (" +
                         "  @GstCodeTypeId " +
                         " ,@CreatedBy " +
                         " ,@UpdatedBy " +
                         " ,@CreatedOn " +
                         " ,@UpdatedOn " +
                         " ,@isActive " +
                         " ,@Priority " +
                         " ,@CurrencyId " +
                         " ,@SupplierOrgId " +
                         " ,@Currency_Rate " +
                         " ,@Basic_Rate " +
                         " ,@Gst_Item_Code" +
                         ", @Discount" +
                         ", @CodeTypeId" +
                         ", @Pf" +
                         ", @Freight" +
                         ", @Delivery_Period_In_Days" +
                         ", @Multiplication_Factor" +
                         ", @Minimum_Order_Qty" +
                         ", @Supplier_Address" +
                         ", @Mfg_Catlog_No" +
                         ", @WeightMeasurUnitId" +
                         ", @ProdItemId" +
                         ", @Item_Per_Purchase_Unit" +
                         ", @length_mm" +
                         ", @width_mm" +
                         ", @height_mm" +
                         ", @volume_ccm" +
                         ", @weight_kg" +
                         " )";
            //Hudeakr Priyanka [22-feb-19]
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@GstCodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.GstCodeTypeId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseItemMasterTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.UpdatedOn);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.IsActive);

            cmdInsert.Parameters.Add("@Priority", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Priority);
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CurrencyId);

            cmdInsert.Parameters.Add("@SupplierOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierOrgId);
            cmdInsert.Parameters.Add("@Currency_Rate", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.CurrencyRate;
            cmdInsert.Parameters.Add("@Basic_Rate", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.BasicRate;
           
            cmdInsert.Parameters.Add("@Gst_Item_Code", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.GSTItemCode);
            cmdInsert.Parameters.Add("@Discount", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.Discount;
            cmdInsert.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CodeTypeId);
            cmdInsert.Parameters.Add("@Pf", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.PF;
            cmdInsert.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.Freight;
            cmdInsert.Parameters.Add("@Delivery_Period_In_Days", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.DeliveryPeriodInDays);
            cmdInsert.Parameters.Add("@Multiplication_Factor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MultiplicationFactor);
            cmdInsert.Parameters.Add("@Minimum_Order_Qty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MinimumOrderQty);
            cmdInsert.Parameters.Add("@Supplier_Address", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierAddress);
            cmdInsert.Parameters.Add("@Mfg_Catlog_No", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MfgCatlogNo);
            cmdInsert.Parameters.Add("@WeightMeasurUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.WeightMeasurUnitId);
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.ProdItemId);
            cmdInsert.Parameters.Add("@Item_Per_Purchase_Unit", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.ItemPerPurchaseUnit);
            cmdInsert.Parameters.Add("@length_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Length_mm);
            cmdInsert.Parameters.Add("@width_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Width_mm);
            cmdInsert.Parameters.Add("@height_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Height_mm);
            cmdInsert.Parameters.Add("@volume_ccm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Volume_ccm);
            cmdInsert.Parameters.Add("@weight_kg", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Weight_kg);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                return 1;
            }

            return 0;
        }
        #region Updation

        //remove previous Base Items while adding or updating new
        public int updatePreviousBase(SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                " [isBaseItemForRate] = " + 0 +                //Priyanka [16-05-2018] 
                                " WHERE [isBaseItemForRate] = " + 1  ;

                cmdUpdate.CommandText = sqlQuery;
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
    
        public int UpdateTblProductItem(TblProductItemTO tblProductItemTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProductItemTO, cmdUpdate);
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

        public int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProductItemTO, cmdUpdate);
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
        //Priyanka [22-05-2018] : Added to update  product item tax type.
        public int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdateTblProductItemTaxType(idClassStr, codeTypeId, cmdUpdate);
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
        public int ExecuteUpdationCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET " + 
                                "  [prodClassId]= @ProdClassId" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[itemName]= @ItemName" +
                                " ,[itemDesc]= @ItemDesc" +
                                " ,[remark] = @Remark" +
                                " ,[isActive] = @isActive" +
                                " ,[weightMeasureUnitId] = @weightMeasureUnitId " +
                                " ,[conversionUnitOfMeasure] = @conversionUnitOfMeasure " +
                                " ,[conversionFactor] = @conversionFactor " +
                                " ,[isStockRequire] = @isStockRequire " +
                                " ,[isParity] = @IsParity " +
                                " ,[basePrice] = @BasePrice " +
                                " ,[codeTypeId] = @CodeTypeId" +                //Priyanka [16-05-2018] 
                                " ,[isBaseItemForRate] = @isBase" +
                                " ,[isNonCommercialItem] = @nonCommertial" +
                                " WHERE [idProdItem] = @IdProdItem "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblProductItemTO.ProdClassId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblProductItemTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblProductItemTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@ItemName", System.Data.SqlDbType.NVarChar).Value = tblProductItemTO.ItemName;
            cmdUpdate.Parameters.Add("@ItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemDesc);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.Remark);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsActive;
            cmdUpdate.Parameters.Add("@weightMeasureUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.WeightMeasureUnitId);
            cmdUpdate.Parameters.Add("@conversionUnitOfMeasure", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionUnitOfMeasure);
            cmdUpdate.Parameters.Add("@conversionFactor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionFactor);
            cmdUpdate.Parameters.Add("@isStockRequire", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsStockRequire;
            cmdUpdate.Parameters.Add("@IsParity", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsParity;
            cmdUpdate.Parameters.Add("@BasePrice", System.Data.SqlDbType.Decimal).Value = tblProductItemTO.BasePrice;
            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CodeTypeId);             //Priyanka [16-05-2018]
            cmdUpdate.Parameters.Add("@isBase", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsBaseItemForRate);
            cmdUpdate.Parameters.Add("@nonCommertial", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsNonCommercialItem);

            return cmdUpdate.ExecuteNonQuery();
        }

        public int ExecuteUpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId ,SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                " [codeTypeId] = "+ codeTypeId +                //Priyanka [16-05-2018] 
                                " WHERE [prodClassId] IN (" + idClassStr +")";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(codeTypeId);             //Priyanka [16-05-2018]
            return cmdUpdate.ExecuteNonQuery();
        }

        //@ Hudekar Priyanka [04-march-19]
        public int UpdateTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                //if(tblPurchaseItemMasterTO.IdPurchaseItemMaster==0 && tblPurchaseItemMasterTO.IsAddNewPurchase==0)
                //{
                //    return ExecuteDeactivateCommandForTblPurchaseItemMasterTO(tblPurchaseItemMasterTO, cmdUpdate);
                //}
                return ExecuteUpdationCommandForTblPurchaseItemMasterTO(tblPurchaseItemMasterTO, cmdUpdate);
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
        //public int ExecuteDeactivateCommandForTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlCommand cmdDeactivate)
        //{
        //    String sqlQuery = @" UPDATE [tblProductItemPurchaseExt] SET " +
        //                      " [prodItemId] = @prodItemId" +
        //                      " WHERE [idPurchaseItemMaster] = @idPurchaseItemMaster ";

        //    cmdDeactivate.CommandText = sqlQuery;

        //    cmdDeactivate.CommandType = System.Data.CommandType.Text;

        //    return cmdDeactivate.ExecuteNonQuery();
        //}

        public int DeactivateTblPurchaseItemMaster( int prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                
                String sqlQuery = @" UPDATE [tblProductItemPurchaseExt] SET " +
                                " [isActive] = 0" +
                                " WHERE [prodItemId] = @prodItemId ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.AddWithValue("@prodItemId", DbType.Int32).Value = prodItemId;
              //  cmdUpdate.Parameters.AddWithValue("@idPurchaseItemMaster", DbType.Int32).Value = _idPurchaseItemMaster;

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

        //@  Hudekar Priyanka [04-march-2019]
        public int ExecuteUpdationCommandForTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItemPurchaseExt] SET " +
                                "  [gstCodeTypeId]= @GstCodeTypeId" +
                                " ,[createdBy]= @CreatedBy " +
                                " ,[updatedBy]= @UpdatedBy " +
                                " ,[createdOn]= @CreatedOn " +
                                " ,[updatedOn]= @UpdatedOn " +
                                " ,[isActive] = @isActive " +
                                " ,[priority] = @Priority " +
                                " ,[currencyId] = @CurrencyId  " +
                                " ,[supplierOrgId] = @SupplierOrgId  " +
                                " ,[currencyRate] = @Currency_Rate" +
                                " ,[basicRate] = @Basic_Rate" +
                                " ,[gstItemCode] = @Gst_Item_Code" +
                                " ,[discount] = @Discount " +
                                " ,[codeTypeId] = @CodeTypeId" +
                                " ,[pf] = @Pf" +
                                " ,[freight] = @Freight" +
                                " ,[deliveryPeriodInDays] = @Delivery_Period_In_Days" +
                                " ,[multiplicationFactor] = @Multiplication_Factor " +
                                " ,[minimumOrderQty] = @Minimum_Order_Qty" +
                                " ,[supplierAddress] = @Supplier_Address" +
                                " ,[mfgCatlogNo] = @Mfg_Catlog_No" +
                                " ,[weightMeasurUnitId] = @WeightMeasurUnitId" +
                                " ,[prodItemId] = @ProdItemId " +
                                " ,[itemPerPurchaseUnit] = @Item_Per_Purchase_Unit" +
                                " ,[lengthmm] = @length_mm" +
                                " ,[widthmm] = @width_mm" +
                                " ,[heightmm] = @height_mm" +
                                " ,[volumeccm] = @volume_ccm" +
                                " ,[weightkg] = @weight_kg" +
                                " WHERE [idPurchaseItemMaster] = @IdPurchaseItemMaster";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseItemMaster", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.IdPurchaseItemMaster;

            cmdUpdate.Parameters.Add("@GstCodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.GstCodeTypeId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseItemMasterTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value =tblPurchaseItemMasterTO.IsActive;
            
            cmdUpdate.Parameters.Add("@Priority", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Priority);
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CurrencyId);

            cmdUpdate.Parameters.Add("@SupplierOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierOrgId);
            cmdUpdate.Parameters.Add("@Currency_Rate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CurrencyRate);
            cmdUpdate.Parameters.Add("@Basic_Rate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.BasicRate);
           
            cmdUpdate.Parameters.Add("@Gst_Item_Code", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.GSTItemCode);
            cmdUpdate.Parameters.Add("@Discount", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Discount);
            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CodeTypeId);
            cmdUpdate.Parameters.Add("@Pf", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.PF);
            cmdUpdate.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Freight);
            cmdUpdate.Parameters.Add("@Delivery_Period_In_Days", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.DeliveryPeriodInDays);
            cmdUpdate.Parameters.Add("@Multiplication_Factor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MultiplicationFactor);
            cmdUpdate.Parameters.Add("@Minimum_Order_Qty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MinimumOrderQty);
            cmdUpdate.Parameters.Add("@Supplier_Address", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierAddress);
            cmdUpdate.Parameters.Add("@Mfg_Catlog_No", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MfgCatlogNo);
            cmdUpdate.Parameters.Add("@WeightMeasurUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.WeightMeasurUnitId);
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.ProdItemId);
            cmdUpdate.Parameters.Add("@Item_Per_Purchase_Unit", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.ItemPerPurchaseUnit);
            cmdUpdate.Parameters.Add("@length_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Length_mm);
            cmdUpdate.Parameters.Add("@width_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Width_mm);
            cmdUpdate.Parameters.Add("@height_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Height_mm);
            cmdUpdate.Parameters.Add("@volume_ccm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Volume_ccm);
            cmdUpdate.Parameters.Add("@weight_kg", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Weight_kg);


            return cmdUpdate.ExecuteNonQuery();
        }
        //@Hudekar Priyanka [04-march-19]
        public TblPurchaseItemMasterTO SelectTblPurchaseItemMaster(Int32 prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryForTblPurchaseItemMaster() + " WHERE prodItemId = @prodItemId";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@prodItemId", SqlDbType.Int).Value = prodItemId;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                TblPurchaseItemMasterTO tblPurchaseItemMasterData = ConvertDTToListForTblPurchaseItemMaster(reader);
                if (tblPurchaseItemMasterData != null)
                    return tblPurchaseItemMasterData;

                return null;
            }
            catch (Exception ex)
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
        public String SqlSelectQueryForTblPurchaseItemMaster()
        {
            String sqlSelectQry = " SELECT * FROM [tblProductItemPurchaseExt] prodItemPurchaseClass " +
                                   " LEFT JOIN tblProductItem itemProdCatg ON itemProdCatg.idProdItem = prodItemPurchaseClass.ProdItemId";
            return sqlSelectQry;
        }
        public TblPurchaseItemMasterTO ConvertDTToListForTblPurchaseItemMaster(SqlDataReader tblPurchaseItemMasterTODT)
        {
            TblPurchaseItemMasterTO tblPurchaseItemMasterTORow = new TblPurchaseItemMasterTO();
            if (tblPurchaseItemMasterTODT != null)
            {
                if (tblPurchaseItemMasterTODT.Read())
                {
                    TblPurchaseItemMasterTO tblPurchaseItemMasterTONew = new TblPurchaseItemMasterTO();
                    if (tblPurchaseItemMasterTODT["idPurchaseItemMaster"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IdPurchaseItemMaster = Convert.ToInt32(tblPurchaseItemMasterTODT["idPurchaseItemMaster"].ToString());
                    if (tblPurchaseItemMasterTODT["priority"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Priority = Convert.ToDecimal(tblPurchaseItemMasterTODT["priority"].ToString());

                    if (tblPurchaseItemMasterTODT["createdBy"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CreatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["createdBy"].ToString());
                    if (tblPurchaseItemMasterTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.UpdatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["updatedBy"].ToString());
                    if (tblPurchaseItemMasterTODT["createdOn"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CreatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["createdOn"].ToString());
                    if (tblPurchaseItemMasterTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["updatedOn"].ToString());
                    if (tblPurchaseItemMasterTODT["isActive"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IsActive = Convert.ToInt32(tblPurchaseItemMasterTODT["isActive"].ToString());

                    if (tblPurchaseItemMasterTODT["currencyId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CurrencyId = Convert.ToInt32(tblPurchaseItemMasterTODT["currencyId"].ToString());

                    if (tblPurchaseItemMasterTODT["supplierOrgId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierOrgId = Convert.ToInt32(tblPurchaseItemMasterTODT["supplierOrgId"].ToString());

                    if (tblPurchaseItemMasterTODT["gstCodeTypeId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.GstCodeTypeId = Convert.ToInt32(tblPurchaseItemMasterTODT["gstCodeTypeId"].ToString());

                    if (tblPurchaseItemMasterTODT["currencyRate"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CurrencyRate = Convert.ToDecimal(tblPurchaseItemMasterTODT["currencyRate"].ToString());

                    if (tblPurchaseItemMasterTODT["basicRate"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.BasicRate = Convert.ToDecimal(tblPurchaseItemMasterTODT["basicRate"].ToString());

                    if (tblPurchaseItemMasterTODT["gstItemCode"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.GSTItemCode = Convert.ToString(tblPurchaseItemMasterTODT["gstItemCode"].ToString());
                    if (tblPurchaseItemMasterTODT["discount"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Discount = Convert.ToDecimal(tblPurchaseItemMasterTODT["discount"].ToString());

                    if (tblPurchaseItemMasterTODT["pf"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.PF = Convert.ToDecimal(tblPurchaseItemMasterTODT["pf"].ToString());

                    if (tblPurchaseItemMasterTODT["freight"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Freight = Convert.ToDecimal(tblPurchaseItemMasterTODT["freight"].ToString());
                    if (tblPurchaseItemMasterTODT["deliveryPeriodInDays"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.DeliveryPeriodInDays = Convert.ToDecimal(tblPurchaseItemMasterTODT["deliveryPeriodInDays"].ToString());

                    if (tblPurchaseItemMasterTODT["multiplicationFactor"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.MultiplicationFactor = Convert.ToDecimal(tblPurchaseItemMasterTODT["multiplicationFactor"].ToString());
                    if (tblPurchaseItemMasterTODT["minimumOrderQty"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.MinimumOrderQty = Convert.ToDecimal(tblPurchaseItemMasterTODT["minimumOrderQty"].ToString());

                    if (tblPurchaseItemMasterTODT["supplierAddress"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierAddress = Convert.ToInt32(tblPurchaseItemMasterTODT["supplierAddress"].ToString());

                    if (tblPurchaseItemMasterTODT["mfgCatlogNo"] != DBNull.Value)
                        //tblPurchaseItemMasterTONew.mfgCatlogNo = Convert.ToString(tblPurchaseItemMasterTODT["mfgCatlogNo"]).ToString());
                        tblPurchaseItemMasterTONew.MfgCatlogNo = Convert.ToString(tblPurchaseItemMasterTODT["mfgCatlogNo"].ToString());
                    if (tblPurchaseItemMasterTODT["weightMeasurUnitId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.WeightMeasurUnitId = Convert.ToInt32(tblPurchaseItemMasterTODT["weightMeasurUnitId"].ToString());
                    if (tblPurchaseItemMasterTODT["prodItemId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.ProdItemId = Convert.ToInt32(tblPurchaseItemMasterTODT["prodItemId"].ToString());

                    if (tblPurchaseItemMasterTODT["itemPerPurchaseUnit"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.ItemPerPurchaseUnit = Convert.ToDecimal(tblPurchaseItemMasterTODT["itemPerPurchaseUnit"].ToString());
                    //   if (tblPurchaseItemMasterTODT["lengthmm"] != DBNull.Value)
                    //      tblPurchaseItemMasterTONew.length_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["lengthmm"].ToString());

                    if (tblPurchaseItemMasterTODT["lengthmm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Length_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["lengthmm"].ToString());
                    if (tblPurchaseItemMasterTODT["widthmm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Width_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["widthmm"].ToString());
                    if (tblPurchaseItemMasterTODT["heightmm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Height_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["heightmm"].ToString());
                    if (tblPurchaseItemMasterTODT["volumeccm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Volume_ccm = Convert.ToDecimal(tblPurchaseItemMasterTODT["volumeccm"].ToString());
                    if (tblPurchaseItemMasterTODT["weightkg"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Weight_kg = Convert.ToDecimal(tblPurchaseItemMasterTODT["weightkg"].ToString());


                    if (tblPurchaseItemMasterTODT["codeTypeId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CodeTypeId = Convert.ToInt32(tblPurchaseItemMasterTODT["codeTypeId"].ToString());

                    // tblPurchaseItemMasterTOList.Add(tblPurchaseItemMasterTONew);
                    tblPurchaseItemMasterTORow = tblPurchaseItemMasterTONew;
                }
                else
                {
                    return null;
                }
            }
            return tblPurchaseItemMasterTORow;
        }
        


        #endregion

        #region Deletion
        public int DeleteTblProductItem(Int32 idProdItem)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdItem, cmdDelete);
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

        public int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdItem, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idProdItem, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProductItem] " +
            " WHERE idProdItem = " + idProdItem +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
