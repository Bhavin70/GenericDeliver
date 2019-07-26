using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblItemBroadCategoriesBL : ITblItemBroadCategoriesBL
    {
        private readonly ITblItemBroadCategoriesDAO _iTblItemBroadCategoriesDAO;
        public TblItemBroadCategoriesBL(ITblItemBroadCategoriesDAO iTblItemBroadCategoriesDAO)
        {
            _iTblItemBroadCategoriesDAO = iTblItemBroadCategoriesDAO;
        }
        #region Selection
        public List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategories()
        {
            return _iTblItemBroadCategoriesDAO.SelectAllTblItemBroadCategories();
        }

        public List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategoriesList()
        {
            List<TblItemBroadCategoriesTO> tblItemBroadCategoriesToList = _iTblItemBroadCategoriesDAO.SelectAllTblItemBroadCategories();
            if (tblItemBroadCategoriesToList != null && tblItemBroadCategoriesToList.Count > 0)
                return tblItemBroadCategoriesToList;
            else
                return null;
        }

        public TblItemBroadCategoriesTO SelectTblItemBroadCategoriesTO(Int32 iditemBroadCategories)
        {
            TblItemBroadCategoriesTO tblItemBroadCategoriesTODT = _iTblItemBroadCategoriesDAO.SelectTblItemBroadCategories(iditemBroadCategories);
            if (tblItemBroadCategoriesTODT != null)
                return tblItemBroadCategoriesTODT;
            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO)
        {
            return _iTblItemBroadCategoriesDAO.InsertTblItemBroadCategories(tblItemBroadCategoriesTO);
        }

        public int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemBroadCategoriesDAO.InsertTblItemBroadCategories(tblItemBroadCategoriesTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO)
        {
            return _iTblItemBroadCategoriesDAO.UpdateTblItemBroadCategories(tblItemBroadCategoriesTO);
        }

        public int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemBroadCategoriesDAO.UpdateTblItemBroadCategories(tblItemBroadCategoriesTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblItemBroadCategories(Int32 iditemBroadCategories)
        {
            return _iTblItemBroadCategoriesDAO.DeleteTblItemBroadCategories(iditemBroadCategories);
        }

        public int DeleteTblItemBroadCategories(Int32 iditemBroadCategories, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemBroadCategoriesDAO.DeleteTblItemBroadCategories(iditemBroadCategories, conn, tran);
        }

        #endregion
    }
}