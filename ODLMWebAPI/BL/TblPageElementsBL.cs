using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblPageElementsBL : ITblPageElementsBL
    {
        private readonly ITblPageElementsDAO _iTblPageElementsDAO;
        public TblPageElementsBL(ITblPageElementsDAO iTblPageElementsDAO)
        {
            _iTblPageElementsDAO = iTblPageElementsDAO;
        }
        #region Selection

        public List<TblPageElementsTO> SelectAllTblPageElementsList()
        {
           return  _iTblPageElementsDAO.SelectAllTblPageElements();
        }

        public TblPageElementsTO SelectTblPageElementsTO(Int32 idPageElement)
        {
            return  _iTblPageElementsDAO.SelectTblPageElements(idPageElement);
        }

        public List<TblPageElementsTO> SelectAllTblPageElementsList(int pageId)
        {
            return _iTblPageElementsDAO.SelectAllTblPageElements(pageId);
        }

        #endregion

        #region Insertion
        public int InsertTblPageElements(TblPageElementsTO tblPageElementsTO)
        {
            return _iTblPageElementsDAO.InsertTblPageElements(tblPageElementsTO);
        }

        public int InsertTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPageElementsDAO.InsertTblPageElements(tblPageElementsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO)
        {
            return _iTblPageElementsDAO.UpdateTblPageElements(tblPageElementsTO);
        }

        public int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPageElementsDAO.UpdateTblPageElements(tblPageElementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblPageElements(Int32 idPageElement)
        {
            return _iTblPageElementsDAO.DeleteTblPageElements(idPageElement);
        }

        public int DeleteTblPageElements(Int32 idPageElement, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPageElementsDAO.DeleteTblPageElements(idPageElement, conn, tran);
        }

       

        #endregion

    }
}
