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
    public class TblEntityRangeBL : ITblEntityRangeBL
    {
        private readonly ITblEntityRangeDAO _iTblEntityRangeDAO;
        public TblEntityRangeBL(ITblEntityRangeDAO iTblEntityRangeDAO)
        {
            _iTblEntityRangeDAO = iTblEntityRangeDAO;
        }
        #region Selection

        public List<TblEntityRangeTO> SelectAllTblEntityRangeList()
        {
            return  _iTblEntityRangeDAO.SelectAllTblEntityRange();
        }

        public TblEntityRangeTO SelectTblEntityRangeTO(Int32 idEntityRange)
        {
            return  _iTblEntityRangeDAO.SelectTblEntityRange(idEntityRange);
        }
        public TblEntityRangeTO SelectTblEntityRangeTOByEntityName(string entityName)
        {
            return _iTblEntityRangeDAO.SelectTblEntityRangeByEntityName(entityName);
        }
        #region @Kiran Migration Of Invoice 
         
        public TblEntityRangeTO SelectEntityRangeTOFromInvoiceType(String entityName, int finYearId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityName, finYearId, conn, tran);
        }
        #endregion 
        public TblEntityRangeTO SelectEntityRangeTOFromInvoiceType(Int32 invoiceTypeId,int finYearId, SqlConnection conn,SqlTransaction tran)
        {
            return _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(invoiceTypeId,finYearId, conn, tran);
        }

        // Vaibhav [07-Jan-2018] Added t select entity data
        public TblEntityRangeTO SelectTblEntityRangeTOByEntityName(string entityName, int finYearId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEntityRangeDAO.SelectTblEntityRangeByEntityName(entityName, finYearId, conn, tran);
        }
        #endregion

        #region Insertion
        public int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            return _iTblEntityRangeDAO.InsertTblEntityRange(tblEntityRangeTO);
        }

        public int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEntityRangeDAO.InsertTblEntityRange(tblEntityRangeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            return _iTblEntityRangeDAO.UpdateTblEntityRange(tblEntityRangeTO);
        }

        public int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEntityRangeDAO.UpdateTblEntityRange(tblEntityRangeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblEntityRange(Int32 idEntityRange)
        {
            return _iTblEntityRangeDAO.DeleteTblEntityRange(idEntityRange);
        }

        public int DeleteTblEntityRange(Int32 idEntityRange, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEntityRangeDAO.DeleteTblEntityRange(idEntityRange, conn, tran);
        }

        #endregion
        
    }
}
