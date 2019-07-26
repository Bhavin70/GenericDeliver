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
    public class TblPagesBL : ITblPagesBL
    {
        private readonly ITblPagesDAO _iTblPagesDAO;
        public TblPagesBL(ITblPagesDAO iTblPagesDAO)
        {
            _iTblPagesDAO = iTblPagesDAO;
        }
        #region Selection
        public List<TblPagesTO> SelectAllTblPagesList()
        {
            return  _iTblPagesDAO.SelectAllTblPages();
        }

        public List<TblPagesTO> SelectAllTblPagesList(int moduleId)
        {
            return _iTblPagesDAO.SelectAllTblPages(moduleId);
        }

        public TblPagesTO SelectTblPagesTO(Int32 idPage)
        {
            return  _iTblPagesDAO.SelectTblPages(idPage);
        }

        #endregion
        
        #region Insertion
        public int InsertTblPages(TblPagesTO tblPagesTO)
        {
            return _iTblPagesDAO.InsertTblPages(tblPagesTO);
        }

        public int InsertTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPagesDAO.InsertTblPages(tblPagesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblPages(TblPagesTO tblPagesTO)
        {
            return _iTblPagesDAO.UpdateTblPages(tblPagesTO);
        }

        public int UpdateTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPagesDAO.UpdateTblPages(tblPagesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblPages(Int32 idPage)
        {
            return _iTblPagesDAO.DeleteTblPages(idPage);
        }

        public int DeleteTblPages(Int32 idPage, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPagesDAO.DeleteTblPages(idPage, conn, tran);
        }

       

        #endregion

    }
}
