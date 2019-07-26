using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

using System.Linq;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.BL
{  
    public class TblProductItemBL : ITblProductItemBL
    {
        private readonly ITblProductItemDAO _iTblProductItemDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblProdClassificationBL _iTblProdClassificationBL;
        public TblProductItemBL(ITblProdClassificationBL iTblProdClassificationBL, IConnectionString iConnectionString, ITblProductItemDAO iTblProductItemDAO)
        {
            _iTblProductItemDAO = iTblProductItemDAO;
            _iConnectionString = iConnectionString;
            _iTblProdClassificationBL = iTblProdClassificationBL;
        }
        
          #region Selection

          public List<TblProductItemTO> SelectAllTblProductItemList(Int32 specificationId = 0)
          {
              return _iTblProductItemDAO.SelectAllTblProductItem(specificationId);
          }

          public TblProductItemTO SelectTblProductItemTO(Int32 idProdItem)
          {
              SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
              SqlTransaction tran = null;
              try
              {
                  conn.Open();
                  tran = conn.BeginTransaction();
                  return _iTblProductItemDAO.SelectTblProductItem(idProdItem,conn,tran);

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

          public TblProductItemTO SelectTblProductItemTO(Int32 idProdItem,SqlConnection conn,SqlTransaction tran)
          {
              return _iTblProductItemDAO.SelectTblProductItem(idProdItem, conn, tran);
          }

          /// <summary>
          /// Sudhir[11-Jan-2017] Added for Get List of Items Based on isStockRequire Flag
          /// </summary>
          /// <param name="isStockRequire"></param>
          /// <returns></returns>
          public List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire)
          {
              return _iTblProductItemDAO.SelectProductItemListStockUpdateRequire(isStockRequire);
          }

          public List<TblProductItemTO> SelectProductItemListStockTOList(int activeYn)
          {
              return _iTblProductItemDAO.SelectProductItemListStockTOList(activeYn);
          }

          /// <summary>
          /// Sudhir[15-MAR-2018]
          /// Get List of ProductItemTO Based On Category/Subcategory or Specification
          /// </summary>
          /// <param name="tblProductItemTO"></param>
          /// <returns></returns>
          /// 
          public List<TblProductItemTO> SelectProductItemList(Int32 idProdClass)
          {
              string prodClassIds = _iTblProdClassificationBL.SelectProdtClassificationListOnType(idProdClass);
              if (prodClassIds != string.Empty)
              {
                  return _iTblProductItemDAO.SelectListOfProductItemTOOnprdClassId(prodClassIds);
              }
              else
                  return null;
          }

          /// <summary>
          /// Sudhir[20-MAR-2018]  Added for Get ProductItem List which has Parity Yes.
          /// </summary>
          /// <param name="isParity"></param>
          /// <returns></returns>
          /// 
          public List<DropDownTO> SelectProductItemListIsParity(Int32 isParity)
          {
              List<DropDownTO> list = _iTblProductItemDAO.SelectProductItemListIsParity(isParity);
              if (list != null)
              {
                  return list;
              }
              else
                  return new List<DropDownTO>();
          }
        #endregion


        #region Insertion
       
        public int InsertTblProductItem(TblProductItemTO tblProductItemTO, TblPurchaseItemMasterTO tblPurchaseItemMasterTO)
          {
            
              SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
              SqlTransaction tran = null;
              int result = 0;
            int res = 0;
              try
              {
                  conn.Open();
                  tran = conn.BeginTransaction();
                  if (tblProductItemTO.IsBaseItemForRate > 0)
                  {
                     result= _iTblProductItemDAO.updatePreviousBase(conn, tran);
                      if (result == -1)
                      {
                          tran.Rollback();
                          return result;
                      }
                  }
                 //@ Priyanka H [06-march-2019] add new item and purchase both data same time
                 if( tblProductItemTO.IdProdItem == 0 || tblPurchaseItemMasterTO.IdPurchaseItemMaster == 0)
                 {
                        res = _iTblProductItemDAO.InsertTblProductItem(tblProductItemTO, conn, tran);
                        result = res;
                    if (result != 1)
                    {
                        tran.Rollback();
                        return result;
                    }
                 }
                //@ Priyanka H [06-march-2019] if tblPurchaseItemMasterTO.IsAddNewPurchase is 1 means tblPurchaseItemMasterTO also insert if it is null or not null
                if (res == 1 && tblPurchaseItemMasterTO.IsAddNewPurchase == 1)
                {   
                    tblPurchaseItemMasterTO.ProdItemId = tblProductItemTO.IdProdItem;//imp tblPurchaseItemMasterTO.prodItemId if entry in both table
                    tblPurchaseItemMasterTO.CreatedBy = tblProductItemTO.CreatedBy;
                    tblPurchaseItemMasterTO.CreatedOn = tblProductItemTO.CreatedOn;
                  //  tblPurchaseItemMasterTO.UpdatedBy = tblProductItemTO.UpdatedBy;
                   // tblPurchaseItemMasterTO.UpdatedOn = tblProductItemTO.UpdatedOn;
                    tblPurchaseItemMasterTO.IsActive = tblProductItemTO.IsActive;
                    result = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return result;
                    }
                }
               

                //if (result != 1)
                //{
                //    tran.Rollback();
                //    return result;
                //}
             
                tran.Commit();
                return result;

            }
            catch (Exception e)
            {
                tran.Rollback();
                return result;
            }
        }

        public int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            if (tblProductItemTO.IsBaseItemForRate > 0)
            {
                result = _iTblProductItemDAO.updatePreviousBase(conn, tran);
            }
            if (result != 1)
            {
                return result;
            }
            return _iTblProductItemDAO.InsertTblProductItem(tblProductItemTO, conn, tran);
        }

        //@  Hudekar Priyanka [01-march-2019]
        public int InsertTblPurchaseItemMaster(TblPurchaseItemMasterTO tblPurchaseItemMasterTO)
        {
         //   tblPurchaseItemMasterTO.prodItemId = _ProdItemId;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                result = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTO, conn, tran);
                //   result = TblProductItemPurchaseDAO.InsertTblPurchaseItemMasters(tblPurchaseItemMasterTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    return result;
                }
               
               // var res = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTO, conn, tran);
                tran.Commit();
                return result;

            }
            catch (Exception e)
            {
                tran.Rollback();
                return result;
            }
        }
        #endregion

        #region Updation
        public int UpdateTblProductItem(TblProductItemTO tblProductItemTO,TblPurchaseItemMasterTO tblPurchaseItemMasterTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            int res = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                if (tblProductItemTO.IsBaseItemForRate > 0)
            {
                result = _iTblProductItemDAO.updatePreviousBase(conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        return result;
                    }
            }
              
               
                 if (tblProductItemTO.IdProdItem !=0)
                    {
                        res = _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTO, conn, tran);
                        result = res;
                    if (result != 1)
                    {
                        tran.Rollback();
                        return result;
                    }
                    if (tblProductItemTO.IsActive==0 && tblPurchaseItemMasterTO.IsAddNewPurchase == 0)
                    {
                        Int32 id = tblProductItemTO.IdProdItem;

                            result = _iTblProductItemDAO.DeactivateTblPurchaseItemMaster(id, conn, tran);
                            if (result == -1)
                            {
                                tran.Rollback();
                                return result;
                            }
                      
                    }
                }
                //@ Priyanka H [06-march-2019] if tblPurchaseItemMasterTO.IdPurchaseItemMaster is higher than 0 then update purchase data 
                if (res == 1 && tblPurchaseItemMasterTO.IdPurchaseItemMaster != 0)
                    {
                        tblPurchaseItemMasterTO.ProdItemId = tblProductItemTO.IdProdItem;
                        // tblPurchaseItemMasterTO.CreatedBy = tblProductItemTO.CreatedBy;
                        //tblPurchaseItemMasterTO.CreatedOn = tblProductItemTO.CreatedOn;
                        tblPurchaseItemMasterTO.UpdatedBy = tblProductItemTO.UpdatedBy;
                        tblPurchaseItemMasterTO.UpdatedOn = tblProductItemTO.UpdatedOn;
                        // tblPurchaseItemMasterTO.IsActive = tblProductItemTO.IsActive;
                        result = _iTblProductItemDAO.UpdateTblPurchaseItemMasterTO(tblPurchaseItemMasterTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return result;
                    }
                  }
                //@ Priyanka H [06-march-2019] if tblPurchaseItemMasterTO.IdPurchaseItemMaster is 0 meanse that is new record so insert new entry for purchase 
                if (tblPurchaseItemMasterTO.IdPurchaseItemMaster == 0 && tblPurchaseItemMasterTO.IsAddNewPurchase == 1)
                    {
                      tblPurchaseItemMasterTO.ProdItemId = tblProductItemTO.IdProdItem;
                      tblPurchaseItemMasterTO.IsActive = 1;
                    // tblPurchaseItemMasterTO.CreatedBy = tblPurchaseItemMasterTO.CreatedBy;
                    // tblPurchaseItemMasterTO.CreatedOn = tblPurchaseItemMasterTO.CreatedOn;
                    tblPurchaseItemMasterTO.CreatedBy = tblProductItemTO.UpdatedBy;
                    tblPurchaseItemMasterTO.CreatedOn = tblProductItemTO.UpdatedOn;

                    tblPurchaseItemMasterTO.UpdatedBy = 0;
                    tblPurchaseItemMasterTO.UpdatedOn = new DateTime();
                    result = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return result;
                    }
                }
             
                    //  result = _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTO,conn,tran);
                    //if (result != 1)
                    //{
                    //    tran.Rollback();
                    //    return result;
                    //}
                    tran.Commit();
                return result;
            }
            catch (Exception e)
            {
                tran.Rollback();
                return result;
            }
        }

        public int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            if (tblProductItemTO.IsBaseItemForRate > 0)
            {
                result = _iTblProductItemDAO.updatePreviousBase(conn, tran);
            }
            if (result != 1)
            { 
                return result;
            }
            return _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTO, conn, tran);
        }

        //Priyanka [22-05-2018]: Added to change the product item tax type(HSN/SSN)
        public int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductItemDAO.UpdateTblProductItemTaxType(idClassStr, codeTypeId, conn, tran);
        }

        //@  Hudekar Priyanka [04-march-2019]

        public int UpdateTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
               

                result = _iTblProductItemDAO.UpdateTblPurchaseItemMasterTO(tblPurchaseItemMasterTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    return result;
                }
                tran.Commit();
                return result;
            }
            catch (Exception e)
            {
                tran.Rollback();
                return result;
            }
        }
        #endregion

        #region Deletion
        public int DeleteTblProductItem(Int32 idProdItem)
        {
            return _iTblProductItemDAO.DeleteTblProductItem(idProdItem);
        }

        public int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductItemDAO.DeleteTblProductItem(idProdItem, conn, tran);
        }

      
        //@ Hudekar Priyanka [04-march-19]
        public TblPurchaseItemMasterTO SelectTblPurchaseItemMasterTO(Int32 prodItemId)
        {
           // return TblProductItemPurchaseDAO.SelectTblPurchaseItemMaster(prodItemId);
           return _iTblProductItemDAO.SelectTblPurchaseItemMaster(prodItemId);
        }
        #endregion
    }
}
