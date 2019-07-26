using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblItemBroadCategoriesBL : ITblItemBroadCategoriesBL
    {
        #region Selection
        public List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategories()
        {
            return TblItemBroadCategoriesDAO.SelectAllTblItemBroadCategories();
        }

        public List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategoriesList()
        {
            List<TblItemBroadCategoriesTO> tblItemBroadCategoriesToList = TblItemBroadCategoriesDAO.SelectAllTblItemBroadCategories();
            if (tblItemBroadCategoriesToList != null && tblItemBroadCategoriesToList.Count > 0)
                return tblItemBroadCategoriesToList;
            else
                return null;
        }

        public TblItemBroadCategoriesTO SelectTblItemBroadCategoriesTO(Int32 iditemBroadCategories)
        {
            TblItemBroadCategoriesTO tblItemBroadCategoriesTODT = TblItemBroadCategoriesDAO.SelectTblItemBroadCategories(iditemBroadCategories);
            if (tblItemBroadCategoriesTODT != null)
                return tblItemBroadCategoriesTODT;
            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO)
        {
            return TblItemBroadCategoriesDAO.InsertTblItemBroadCategories(tblItemBroadCategoriesTO);
        }

        public int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblItemBroadCategoriesDAO.InsertTblItemBroadCategories(tblItemBroadCategoriesTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO)
        {
            return TblItemBroadCategoriesDAO.UpdateTblItemBroadCategories(tblItemBroadCategoriesTO);
        }

        public int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblItemBroadCategoriesDAO.UpdateTblItemBroadCategories(tblItemBroadCategoriesTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblItemBroadCategories(Int32 iditemBroadCategories)
        {
            return TblItemBroadCategoriesDAO.DeleteTblItemBroadCategories(iditemBroadCategories);
        }

        public int DeleteTblItemBroadCategories(Int32 iditemBroadCategories, SqlConnection conn, SqlTransaction tran)
        {
            return TblItemBroadCategoriesDAO.DeleteTblItemBroadCategories(iditemBroadCategories, conn, tran);
        }

        #endregion
    }
}