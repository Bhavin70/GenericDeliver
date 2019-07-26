using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{  
    public class TblProdClassificationBL : ITblProdClassificationBL
    {
        private readonly ITblProdClassificationDAO _iTblProdClassificationDAO;
        private readonly ITblProductItemDAO _iTblProductItemDAO;
        private readonly IConnectionString _iConnectionString;
        public TblProdClassificationBL(ITblProdClassificationDAO iTblProdClassificationDAO, ITblProductItemDAO iTblProductItemDAO, IConnectionString iConnectionString)
        {
            _iTblProdClassificationDAO = iTblProdClassificationDAO;
            _iTblProductItemDAO = iTblProductItemDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public List<TblProdClassificationTO> SelectAllTblProdClassificationList(string prodClassType = "")
        {
            return  _iTblProdClassificationDAO.SelectAllTblProdClassification(prodClassType);
        }
        public List<TblProdClassificationTO> SelectAllTblProdClassificationList(SqlConnection conn, SqlTransaction tran, string prodClassType = "")
        {
            return _iTblProdClassificationDAO.SelectAllTblProdClassification(conn,tran, prodClassType);
        }
        public List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId)
        {
            return _iTblProdClassificationDAO.SelectAllProdClassificationForDropDown(parentClassId);

        }
        public TblProdClassificationTO SelectTblProdClassificationTO(Int32 idProdClass)
        {
            return  _iTblProdClassificationDAO.SelectTblProdClassification(idProdClass);
        }


        public List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            return _iTblProdClassificationDAO.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);
        }
        #endregion

        #region Product Classification DisplayName
        public void SetProductClassificationDisplayName(TblProdClassificationTO tblProdClassificationTO, List<TblProdClassificationTO> allProdClassificationList)
        {
            String DisplayName = String.Empty;
            List<TblProdClassificationTO> DisplayNameList = new List<TblProdClassificationTO>();
            if (tblProdClassificationTO != null)
            {
                //List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                GetDisplayName(allProdClassificationList, tblProdClassificationTO.ParentProdClassId, DisplayNameList);
                DisplayNameList=DisplayNameList.OrderBy(x => x.IdProdClass).ToList();
                if(DisplayNameList != null && DisplayNameList.Count > 0)
                {
                    for (int ele = 0; ele < DisplayNameList.Count; ele++)
                    {
                        TblProdClassificationTO tempTo = DisplayNameList[ele];
                        DisplayName += tempTo.ProdClassDesc + "/";
                    }
                }
                else if(DisplayNameList.Count == 0)
                {

                }
                else
                {
                    DisplayName += DisplayNameList[0].ProdClassDesc + "/";
                }
                tblProdClassificationTO.DisplayName = DisplayName + tblProdClassificationTO.ProdClassDesc;
            }
        }
        public void GetDisplayName(List<TblProdClassificationTO> allProdClassificationList, int parentId, List<TblProdClassificationTO> DisplayNameList)
        {

            if (allProdClassificationList != null && allProdClassificationList.Count > 0)
            {
                List<TblProdClassificationTO> tempList = allProdClassificationList.Where(ele => ele.IdProdClass == parentId).ToList();
                if (tempList != null && tempList.Count > 0)
                {
                    if (tempList[0].ParentProdClassId == 0)
                    {
                        TblProdClassificationTO ProdClassificationTO = tempList[0];
                        DisplayNameList.Add(tempList[0]);
                    }
                    else
                    {
                        TblProdClassificationTO ProdClassificationTO = tempList[0];
                        DisplayNameList.Add(tempList[0]);
                        GetDisplayName(allProdClassificationList, tempList[0].ParentProdClassId, DisplayNameList);
                    }
                }
            }
        }



        //Sudhir[15-March-2018] Added for getList classification  based on  Categeory or Subcategory or specification.
        public string SelectProdtClassificationListOnType(Int32 idProdClass)
        {
            try
            {
                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                TblProdClassificationTO tblProdClassificationTO = allProdClassificationList.Where(ele => ele.IdProdClass == idProdClass).FirstOrDefault();
                String tempids = String.Empty;
                String idsProdClass = String.Empty;
                if (allProdClassificationList != null && tblProdClassificationTO != null)
                {
                    GetIdsofProductClassification(allProdClassificationList, idProdClass,ref tempids);
                }
                idsProdClass = tempids.TrimEnd(',');
                return idsProdClass;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Sudhir[15-March-2018] Added for getIds of productClassification.
        public void GetIdsofProductClassification(List<TblProdClassificationTO> allList, int parentId,ref String ids)
        {
            ids += parentId + ",";
            List<TblProdClassificationTO> childList=allList.Where(ele => ele.ParentProdClassId == parentId).ToList();
            if (childList != null && childList.Count > 0)
            {
                foreach (TblProdClassificationTO item in childList)
                {
                    GetIdsofProductClassification(allList, item.IdProdClass,ref ids);
                }
            }
        }
        //[05-09-2018]Vijaymala added to get product classification data
        public List<TblProdClassificationTO> SelectProductClassificationListByProductItemId(Int32 prodItemId)
        {
            TblProductItemTO tblProductItemTO = _iTblProductItemDAO.SelectTblProductItem(prodItemId);
            List<TblProdClassificationTO> tblProdClassificationTOlist = new List<TblProdClassificationTO>();
            if (tblProductItemTO!=null)
            {
         

                TblProdClassificationTO tblProdClassificationTO = SelectTblProdClassificationTO(tblProductItemTO.ProdClassId);
                if (tblProdClassificationTO != null)
                {
                    tblProdClassificationTO.ProdClassType = tblProdClassificationTO.ProdClassType.Trim();
                    tblProdClassificationTOlist.Add(tblProdClassificationTO);
                    tblProdClassificationTOlist = SelectProductChildList(tblProdClassificationTOlist, tblProdClassificationTO.ParentProdClassId);
                }
            }
            return tblProdClassificationTOlist;
        }

        public List<TblProdClassificationTO> SelectProductChildList(List<TblProdClassificationTO> tblProdClassificationTOlist,Int32 parentId)
        {
            TblProdClassificationTO tblProdClassificationTO = SelectTblProdClassificationTO(parentId);
            if (tblProdClassificationTO != null)
            {
                tblProdClassificationTO.ProdClassType = tblProdClassificationTO.ProdClassType.Trim();
                tblProdClassificationTOlist.Add(tblProdClassificationTO);
                SelectProductChildList(tblProdClassificationTOlist, tblProdClassificationTO.ParentProdClassId);
            }
            return tblProdClassificationTOlist;
        }
        #endregion


        #region Insertion

        //Sudhir[12-Jan-2018] Added for Set the DisplayName of Product Classification.
        public int InsertProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
            SetProductClassificationDisplayName(tblProdClassificationTO, allProdClassificationList);
            if (tblProdClassificationTO.IsSetDefault == 1)
            {
                int result = 0;
                result = _iTblProdClassificationDAO.SetIsDefaultByClassificationType(tblProdClassificationTO);
                if (result == -1)
                {
                    return -1;
                }
            }
            return _iTblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO);
        }
        //@Kiran To Get GetDefaultProductClassification List
        public List<Int32> GetDefaultProductClassification()
        {
            List<Int32> defualtList = new List<Int32>();
            List<TblProdClassificationTO> allProductClassificationList = SelectAllTblProdClassificationList("");
            if(allProductClassificationList.Count > 0 && allProductClassificationList != null)
            {
                TblProdClassificationTO ProdutCategoryTo = allProductClassificationList.Where(w => w.ProdClassType.Trim().Equals("C") && w.IsSetDefault == 1).FirstOrDefault();
                if (ProdutCategoryTo != null)
                {
                    defualtList.Add(ProdutCategoryTo.IdProdClass);
                }
                else
                {
                    defualtList.Add(0);
                    return defualtList;
                }
                TblProdClassificationTO ProdSubCategoryTo = allProductClassificationList.Where(w => w.ProdClassType.Trim().Equals("SC") && w.IsSetDefault == 1 && w.ParentProdClassId == ProdutCategoryTo.IdProdClass).FirstOrDefault();
                if (ProdSubCategoryTo != null)
                {
                    defualtList.Add(ProdSubCategoryTo.IdProdClass);
                }
                else
                {
                    defualtList.Add(0);
                    return defualtList;
                }
                TblProdClassificationTO ProdSpecificationTo = allProductClassificationList.Where(w => w.ProdClassType.Trim().Equals("S") && w.IsSetDefault == 1 && w.ParentProdClassId == ProdSubCategoryTo.IdProdClass).FirstOrDefault();
                if (ProdSpecificationTo != null)
                {
                    defualtList.Add(ProdSpecificationTo.IdProdClass);
                }
                else
                    defualtList.Add(0);
            }
            return defualtList;
        }
        public int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            return _iTblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO);
        }

        public int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO, conn, tran);
        }

        #endregion

        //Sudhir[12-Jan-2018] Added for Updating DisplayName Recursively.
        public int UpdateDisplayName(List<TblProdClassificationTO> allProdClassificationList, TblProdClassificationTO ProdClassificationTO, ref String idClassStr ,SqlConnection conn,SqlTransaction tran)
        {
            int result = 0;
            List<TblProdClassificationTO> childList = allProdClassificationList.Where(ele => ele.ParentProdClassId == ProdClassificationTO.IdProdClass).ToList();
            if (childList != null && childList.Count > 0)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    TblProdClassificationTO tempTo = childList[i];
                    tempTo.UpdatedOn = childList[i].CreatedOn;
                    tempTo.UpdatedBy = childList[i].CreatedBy;
                    tempTo.CodeTypeId = ProdClassificationTO.CodeTypeId;                        //Priyanka [21-05-18]
                    SetProductClassificationDisplayName(tempTo, allProdClassificationList);
                    result = UpdateTblProdClassification(tempTo, conn,tran);

                    idClassStr += tempTo.IdProdClass + ",";

                    if (result >= 0)
                    {
                        result = UpdateDisplayName(allProdClassificationList, tempTo, ref idClassStr, conn, tran);
                    }
                    else
                        return -1;
                }
                if(idClassStr != String.Empty)
                {
                    idClassStr= idClassStr.TrimEnd(',');
                    result = _iTblProductItemDAO.UpdateTblProductItemTaxType(idClassStr, ProdClassificationTO.CodeTypeId, conn, tran);
                }
            }
            return result;
        }

        #region Updation

        //Sudhir[12-Jan-2018] Added for updating productclassificaiton and its Displayname where its refrences.
        public int UpdateProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                SetProductClassificationDisplayName(tblProdClassificationTO, allProdClassificationList);
                conn.Open();
                tran = conn.BeginTransaction();
                if (tblProdClassificationTO.IsSetDefault == 1)
                {
                    result = _iTblProdClassificationDAO.SetIsDefaultByClassificationType(tblProdClassificationTO);
                    if (result == -1)
                    {
                        return -1;
                    }
                }
                result =_iTblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO,conn,tran);
                if(result > 0)
                {
                   allProdClassificationList= SelectAllTblProdClassificationList(conn,tran,"");

                    String updatedIds = String.Empty;

                    result = UpdateDisplayName(allProdClassificationList, tblProdClassificationTO, ref updatedIds, conn, tran);
                    if(result >= 0)
                    {
                        result = 1;
                        tran.Commit();
                    }
                    else
                    {
                        return -1;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }
        }

        public int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            return _iTblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO);
        }

        public int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblProdClassification(Int32 idProdClass)
        {
            return _iTblProdClassificationDAO.DeleteTblProdClassification(idProdClass);
        }

        public int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdClassificationDAO.DeleteTblProdClassification(idProdClass, conn, tran);
        }

        #endregion
        
    }
}
