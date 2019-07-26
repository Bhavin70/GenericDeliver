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
namespace ODLMWebAPI.BL
{
    public class TblItemTallyRefDtlsBL : ITblItemTallyRefDtlsBL
    {
        #region Selection

        public List<TblItemTallyRefDtlsTO> SelectAllTblItemTallyRefDtlsList()
        {
            return TblItemTallyRefDtlsDAO.SelectAllTblItemTallyRefDtls();
        }

        public TblItemTallyRefDtlsTO SelectTblItemTallyRefDtlsTO(Int32 idItemTallyRef)
        {
            return TblItemTallyRefDtlsDAO.SelectTblItemTallyRefDtls(idItemTallyRef);
        }
        public List<TblItemTallyRefDtlsTO> SelectExistingAllTblOrganizationByRefIds(String overdueRefId, String enqRefId)
        {
            return TblItemTallyRefDtlsDAO.SelectExistingAllItemTallyByRefIds( overdueRefId, enqRefId);
        }

        public List<TblItemTallyRefDtlsTO> SelectEmptyTblItemTallyRefDtlsTOTemplate(Int32 brandId)
        {
            return TblItemTallyRefDtlsDAO.SelectEmptyTblItemTallyRefDtlsTOTemplate(brandId);
        }

        public List<TblItemTallyRefDtlsTO> SelectAllTallyRefDtlTOList(int brandId)
        {

            List<TblItemTallyRefDtlsTO> emptyStkTemplateList = TblItemTallyRefDtlsBL.SelectEmptyTblItemTallyRefDtlsTOTemplate(brandId);
            List<TblItemTallyRefDtlsTO> existingList = TblItemTallyRefDtlsBL.SelectAllTblItemTallyRefDtlsList();
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
            List<TblProductItemTO> productItemTOList = TblProductItemBL.SelectProductItemListStockTOList(1);
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
            return TblItemTallyRefDtlsDAO.SelectExistingAllTblItemRef(tblItemTallyRefDtlsTO);
        }


        #endregion

        #region Insertion
        public int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO)
        {
            return TblItemTallyRefDtlsDAO.InsertTblItemTallyRefDtls(tblItemTallyRefDtlsTO);
        }

        public int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblItemTallyRefDtlsDAO.InsertTblItemTallyRefDtls(tblItemTallyRefDtlsTO, conn, tran);
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
            return TblItemTallyRefDtlsDAO.UpdateTblItemTallyRefDtls(tblItemTallyRefDtlsTO);
        }

        public int UpdateTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblItemTallyRefDtlsDAO.UpdateTblItemTallyRefDtls(tblItemTallyRefDtlsTO, conn, tran);
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
            return TblItemTallyRefDtlsDAO.DeleteTblItemTallyRefDtls(idItemTallyRef);
        }

        public int DeleteTblItemTallyRefDtls(Int32 idItemTallyRef, SqlConnection conn, SqlTransaction tran)
        {
            return TblItemTallyRefDtlsDAO.DeleteTblItemTallyRefDtls(idItemTallyRef, conn, tran);
        }

        #endregion
        
    }
}
