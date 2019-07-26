using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TblStockDetailsBL : ITblStockDetailsBL
    {
        private readonly ITblStockDetailsDAO _iTblStockDetailsDAO;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblProductItemBL _iTblProductItemBL;
        private readonly IConnectionString _iConnectionString;
        public TblStockDetailsBL(IConnectionString iConnectionString, ITblStockDetailsDAO iTblStockDetailsDAO, ITblConfigParamsBL iTblConfigParamsBL, ITblProductItemBL iTblProductItemBL)
        {
            _iTblStockDetailsDAO = iTblStockDetailsDAO;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblProductItemBL = iTblProductItemBL;
            _iConnectionString = iConnectionString;
        }

        #region Selection

        public List<TblStockDetailsTO> SelectAllTblStockDetailsList()
        {
            return  _iTblStockDetailsDAO.SelectAllTblStockDetails();
        }

        public List<TblStockDetailsTO> SelectAllTblStockDetailsListConsolidated(Int32 isConsolidated, Int32 brandId)
        {
            return _iTblStockDetailsDAO.SelectAllTblStockDetailsConsolidated(isConsolidated, brandId);
        }

        public List<TblStockDetailsTO> SelectAllTblStockDetailsListConsolidated(Int32 isConsolidated, Int32 brandId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.SelectAllTblStockDetailsConsolidated(isConsolidated, brandId, conn, tran);
        }

        public List<TblStockDetailsTO> SelectStockDetailsListByProdCatgAndSpec(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblStockDetailsDAO.SelectAllTblStockDetails(prodCatId, prodSpecId, stockDate,conn, tran);
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

        public List<TblStockDetailsTO> SelectStockDetailsListByProdCatgSpecAndMaterial(Int32 prodCatId, Int32 prodSpecId,Int32 materialId, DateTime stockDate)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblStockDetailsDAO.SelectAllTblStockDetails(prodCatId, prodSpecId, materialId, stockDate, conn, tran);
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

        public List<TblStockDetailsTO> SelectStockDetailsListByProdCatgSpecAndMaterial(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, DateTime stockDate, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.SelectAllTblStockDetails(prodCatId, prodSpecId, materialId, stockDate, conn, tran);
        }

        public List<TblStockDetailsTO> SelectAllTblStockDetailsList(Int32 stockSummaryId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblStockDetailsList(stockSummaryId, conn, tran);
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

        public List<TblStockDetailsTO> SelectAllTblStockDetailsList(Int32 stockSummaryId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.SelectAllTblStockDetails(stockSummaryId,conn,tran);
        }

        public TblStockDetailsTO SelectTblStockDetailsTO(Int32 idStockDtl)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblStockDetailsDAO.SelectTblStockDetails(idStockDtl, conn, tran);
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

        public TblStockDetailsTO SelectTblStockDetailsTO(Int32 idStockDtl,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.SelectTblStockDetails(idStockDtl,conn,tran);
        }

        public TblStockDetailsTO SelectTblStockDetailsTO(TblRunningSizesTO runningSizeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.SelectTblStockDetails(runningSizeTO, conn, tran);
        }

        public List<TblStockDetailsTO> SelectAllEmptyStockTemplateList( int prodCatId,int locationId, int brandId,Int32 isConsolidateStk)
        {
            return _iTblStockDetailsDAO.SelectEmptyStockDetailsTemplate(prodCatId, locationId, brandId, isConsolidateStk);
        }

        public List<TblStockDetailsTO> SelectAllTblStockDetailsList(int locationId, int prodCatId, DateTime stockDate, int brandId)
        {

            Int32 isConsolidateStk = _iTblConfigParamsBL.GetStockConfigIsConsolidate();

            List<TblStockDetailsTO> emptyStkTemplateList = SelectAllEmptyStockTemplateList(prodCatId, locationId, brandId, isConsolidateStk);
            List<TblStockDetailsTO> existingList = _iTblStockDetailsDAO.SelectAllTblStockDetails(locationId, prodCatId, stockDate, brandId);
            if (emptyStkTemplateList != null && emptyStkTemplateList.Count > 0)
            {
                if (existingList != null && existingList.Count > 0)
                {
                    for (int i = 0; i < emptyStkTemplateList.Count; i++)
                    {
                        TblStockDetailsTO existingStockDetailsTO = existingList.Where(a => a.ProdCatId == emptyStkTemplateList[i].ProdCatId && a.ProdSpecId == emptyStkTemplateList[i].ProdSpecId && a.MaterialId == emptyStkTemplateList[i].MaterialId 
                        && a.LocationId == locationId && a.BrandId == brandId && a.IsConsolidatedStock == 0).FirstOrDefault();
                        if (existingStockDetailsTO != null)
                        {
                            emptyStkTemplateList[i].LocationId = locationId;
                            emptyStkTemplateList[i].StockSummaryId = existingStockDetailsTO.StockSummaryId;
                            emptyStkTemplateList[i].IdStockDtl = existingStockDetailsTO.IdStockDtl;
                            emptyStkTemplateList[i].NoOfBundles = existingStockDetailsTO.NoOfBundles;
                            emptyStkTemplateList[i].TotalStock = existingStockDetailsTO.TotalStock;
                            emptyStkTemplateList[i].LoadedStock = existingStockDetailsTO.LoadedStock;
                            emptyStkTemplateList[i].BalanceStock = existingStockDetailsTO.BalanceStock;
                            emptyStkTemplateList[i].CreatedBy = existingStockDetailsTO.CreatedBy;
                            emptyStkTemplateList[i].CreatedOn = existingStockDetailsTO.CreatedOn;
                            emptyStkTemplateList[i].UpdatedBy = existingStockDetailsTO.UpdatedBy;
                            emptyStkTemplateList[i].UpdatedOn = existingStockDetailsTO.UpdatedOn;
                            emptyStkTemplateList[i].ProductId = existingStockDetailsTO.ProductId;
                            emptyStkTemplateList[i].BrandId = existingStockDetailsTO.BrandId;
                            emptyStkTemplateList[i].IsInMT = existingStockDetailsTO.IsInMT;

                        }
                    }
                }

            }

            #region OtherItemStock
            //sudhir[12-jan-2018] Added for otheritemStock if found then add else create empty.
            List<TblProductItemTO> productItemTOList = _iTblProductItemBL.SelectProductItemListStockUpdateRequire(1);
             
            if (productItemTOList != null && productItemTOList.Count > 0)
            {
                for (int i = 0; i < productItemTOList.Count; i++)
                {
                    //TblStockDetailsTO StockDetailsTO = existingList.Where(x => x.ProdItemId == productItemTOList[i].IdProdItem && x.LocationId == locationId && x.ProdCatId==prodCatId && x.BrandId==brandId ).FirstOrDefault(); 
                    TblStockDetailsTO StockDetailsTO = existingList.Where(x => x.ProdItemId == productItemTOList[i].IdProdItem && x.LocationId == locationId && x.BrandId == brandId).FirstOrDefault();
                    if (StockDetailsTO != null)
                    {
                        StockDetailsTO.MaterialDesc = productItemTOList[i].ProdClassDisplayName + "/" + productItemTOList[i].ItemDesc;
                        StockDetailsTO.OtherItem = 1;
                        emptyStkTemplateList.Add(StockDetailsTO);

                    }
                    else //Add Empty Stock 
                    {
                        TblStockDetailsTO emptyItemStockTO = new TblStockDetailsTO();
                        emptyItemStockTO.ProdItemId = productItemTOList[i].IdProdItem;
                        emptyItemStockTO.MaterialDesc = productItemTOList[i].ProdClassDisplayName+"/"+ productItemTOList[i].ItemDesc;
                        emptyItemStockTO.LocationId = locationId;
                        emptyItemStockTO.IsInMT = 1;
                        emptyItemStockTO.OtherItem = 1;
                        
                        // Saket - To disable tmt other items.
                        //emptyItemStockTO.ProdCatId = prodCatId;
                        emptyItemStockTO.BrandId = brandId;
                        emptyStkTemplateList.Add(emptyItemStockTO);
                    }
                }
            }
            #endregion


            #region Consolidated Stock Entry

            if (isConsolidateStk == 1)
            {
                List<TblStockDetailsTO> tblStockDetailsTOListConsolidated = SelectAllTblStockDetailsListConsolidated(1, brandId);



                if (tblStockDetailsTOListConsolidated != null && tblStockDetailsTOListConsolidated.Count > 0)
                {
                    tblStockDetailsTOListConsolidated.ForEach(e => e.IsInMT = 1);  //For consolidation alway stock In MT.
                    emptyStkTemplateList.AddRange(tblStockDetailsTOListConsolidated);
                }
                else
                {
                    //Add Emty consolidate TO if not present
                    TblStockDetailsTO emptyConsoidatedTO = new TblStockDetailsTO();
                    emptyConsoidatedTO.IsConsolidatedStock = 1;
                    emptyConsoidatedTO.IsInMT = 1;
                    emptyConsoidatedTO.BrandId = brandId;
                    emptyStkTemplateList.Add(emptyConsoidatedTO);
                }
            }
            #endregion

            return emptyStkTemplateList;
        }

        public List<SizeSpecWiseStockTO> SelectSizeAndSpecWiseStockSummary(DateTime stockDate,int compartmentId)
        {
            return _iTblStockDetailsDAO.SelectSizeAndSpecWiseStockSummary(stockDate, compartmentId);
        }

        // Vaibhav [26-Mar-2018] Added compartmentId filter.
        public List<TblStockDetailsTO> SelectAllTblStockDetails(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate, Int32 brandId, int compartmentId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.SelectAllTblStockDetails(prodCatId, prodSpecId, stockDate, brandId, compartmentId,conn, tran);
        }

        //Vijaymala[05-09-2018]addded to get otheritem stock
        public List<TblStockDetailsTO> SelectAllTblStockDetailsOther(Int32 prodCatId, Int32 prodSpecId, Int32 prodItemId, DateTime stockDate, Int32 brandId, int compartmentId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.SelectAllTblStockDetailsOther(prodCatId, prodSpecId, prodItemId,  brandId, compartmentId, stockDate, conn, tran);
        }

        public Double SelectTotalBalanceStock(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 brandId)
        {
            return _iTblStockDetailsDAO.SelectTotalBalanceStock(materialId,prodCatId, prodSpecId, brandId);
        }

        // Vaibhav [08-April-2018] Added compartmentId filter.
        public List<TblStockDetailsTO> SelectTblStockDetailsList(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 brandId , int compartmentId=0, int prodItemId=0)
        {
            return _iTblStockDetailsDAO.SelectTblStockDetailsList(materialId, prodCatId, prodSpecId, brandId,compartmentId, prodItemId);
        }
        #endregion

        #region Insertion
        public int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO)
        {
            return _iTblStockDetailsDAO.InsertTblStockDetails(tblStockDetailsTO);
        }

        public int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.InsertTblStockDetails(tblStockDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO)
        {
            return _iTblStockDetailsDAO.UpdateTblStockDetails(tblStockDetailsTO);
        }

        public int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblStockDetails(Int32 idStockDtl)
        {
            return _iTblStockDetailsDAO.DeleteTblStockDetails(idStockDtl);
        }

        public int DeleteTblStockDetails(Int32 idStockDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockDetailsDAO.DeleteTblStockDetails(idStockDtl, conn, tran);
        }

        #endregion
        
    }
}
