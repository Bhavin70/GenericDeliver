using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TblItemTallyRefDtlsBL : ITblItemTallyRefDtlsBL
    {
        private readonly ITblItemTallyRefDtlsDAO _iTblItemTallyRefDtlsDAO;
        private readonly ITblProductItemBL _iTblProductItemBL;
        public TblItemTallyRefDtlsBL(ITblItemTallyRefDtlsDAO iTblItemTallyRefDtlsDAO, ITblProductItemBL iTblProductItemBL)
        {
            _iTblItemTallyRefDtlsDAO = iTblItemTallyRefDtlsDAO;
            _iTblProductItemBL = iTblProductItemBL;
        }
        #region Selection

        public List<TblItemTallyRefDtlsTO> SelectAllTblItemTallyRefDtlsList()
        {
            return _iTblItemTallyRefDtlsDAO.SelectAllTblItemTallyRefDtls();
        }

        public TblItemTallyRefDtlsTO SelectTblItemTallyRefDtlsTO(Int32 idItemTallyRef)
        {
            return _iTblItemTallyRefDtlsDAO.SelectTblItemTallyRefDtls(idItemTallyRef);
        }
        public List<TblItemTallyRefDtlsTO> SelectExistingAllTblOrganizationByRefIds(String overdueRefId, String enqRefId)
        {
            return _iTblItemTallyRefDtlsDAO.SelectExistingAllItemTallyByRefIds( overdueRefId, enqRefId);
        }

        public List<TblItemTallyRefDtlsTO> SelectEmptyTblItemTallyRefDtlsTOTemplate(Int32 brandId)
        {
            return _iTblItemTallyRefDtlsDAO.SelectEmptyTblItemTallyRefDtlsTOTemplate(brandId);
        }

        public List<TblItemTallyRefDtlsTO> SelectAllTallyRefDtlTOList(int brandId)
        {

            List<TblItemTallyRefDtlsTO> emptyStkTemplateList = SelectEmptyTblItemTallyRefDtlsTOTemplate(brandId);
            List<TblItemTallyRefDtlsTO> existingList = SelectAllTblItemTallyRefDtlsList();
            if (existingList != null && existingList.Count > 0)
            {
                existingList = existingList.Where(w => w.IsActive == 1).ToList();

            }
            else
            {
                existingList = new List<TblItemTallyRefDtlsTO>();
            }

            if (emptyStkTemplateList != null && emptyStkTemplateList.Count > 0)
            {


                for (int i = 0; i < emptyStkTemplateList.Count; i++)
                {
                    TblItemTallyRefDtlsTO existTblItemTallyRefDtlsTO = existingList.Where(a => a.ProdCatId == emptyStkTemplateList[i].ProdCatId && a.ProdSpecId == emptyStkTemplateList[i].ProdSpecId && a.MaterialId == emptyStkTemplateList[i].MaterialId
                    && a.BrandId == emptyStkTemplateList[i].BrandId).FirstOrDefault();
                    if (existTblItemTallyRefDtlsTO != null )
                    {
                        existTblItemTallyRefDtlsTO.DisplayName = emptyStkTemplateList[i].DisplayName;
                        emptyStkTemplateList[i] = existTblItemTallyRefDtlsTO;
                    }
                }


            }

            #region OtherItemStock
            List<TblProductItemTO> productItemTOList = _iTblProductItemBL.SelectProductItemListStockTOList(1);
            if (productItemTOList != null && productItemTOList.Count > 0)
            {

                for (int i = 0; i < productItemTOList.Count; i++)
                {
                    TblItemTallyRefDtlsTO existTblItemTallyRefDtlsTO = existingList.Where(x => x.ProdItemId == productItemTOList[i].IdProdItem && x.BrandId == brandId).FirstOrDefault();
                    if (existTblItemTallyRefDtlsTO != null)
                    {

                        existTblItemTallyRefDtlsTO.DisplayName = productItemTOList[i].ProdClassDisplayName + "/" + productItemTOList[i].ItemDesc;
                        existTblItemTallyRefDtlsTO.OtherItem = 1;
                        emptyStkTemplateList.Add(existTblItemTallyRefDtlsTO);

                    }
                    else //Add Empty Stock 
                    {
                        TblItemTallyRefDtlsTO eTblItemTallyRefDtlsTO = new TblItemTallyRefDtlsTO();
                        eTblItemTallyRefDtlsTO.OtherItem = 1;

                        eTblItemTallyRefDtlsTO.ProdItemId = productItemTOList[i].IdProdItem;
                        eTblItemTallyRefDtlsTO.DisplayName = productItemTOList[i].ProdClassDisplayName + "/" + productItemTOList[i].ItemDesc;
                        eTblItemTallyRefDtlsTO.BrandId = brandId;
                        emptyStkTemplateList.Add(eTblItemTallyRefDtlsTO);
                    }
                }
            }
            #endregion

            return emptyStkTemplateList;
        }
        public TblItemTallyRefDtlsTO SelectExistingAllTblItemRef(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO)
        {
            return _iTblItemTallyRefDtlsDAO.SelectExistingAllTblItemRef(tblItemTallyRefDtlsTO);
        }


        #endregion

        #region Insertion
        public int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO)
        {
            return _iTblItemTallyRefDtlsDAO.InsertTblItemTallyRefDtls(tblItemTallyRefDtlsTO);
        }

        public int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemTallyRefDtlsDAO.InsertTblItemTallyRefDtls(tblItemTallyRefDtlsTO, conn, tran);
        }

        public ResultMessage SaveNewItemTallyRef(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                result = InsertTblItemTallyRefDtls(tblItemTallyRefDtlsTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("SaveNewItemTallyRef");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewItemTallyRef");
                return resultMessage;
            }
            finally
            {

            }
        }

        #endregion

        #region Updation
        public int UpdateTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO)
        {
            return _iTblItemTallyRefDtlsDAO.UpdateTblItemTallyRefDtls(tblItemTallyRefDtlsTO);
        }

        public int UpdateTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemTallyRefDtlsDAO.UpdateTblItemTallyRefDtls(tblItemTallyRefDtlsTO, conn, tran);
        }

        public ResultMessage UpdateItemTallyRef(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                result = UpdateTblItemTallyRefDtls(tblItemTallyRefDtlsTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("UpdateItemTallyRef");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateItemTallyRef");
                return resultMessage;
            }
            finally
            {

            }
        }

        #endregion

        #region Deletion
        public int DeleteTblItemTallyRefDtls(Int32 idItemTallyRef)
        {
            return _iTblItemTallyRefDtlsDAO.DeleteTblItemTallyRefDtls(idItemTallyRef);
        }

        public int DeleteTblItemTallyRefDtls(Int32 idItemTallyRef, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemTallyRefDtlsDAO.DeleteTblItemTallyRefDtls(idItemTallyRef, conn, tran);
        }

        #endregion
        
    }
}
