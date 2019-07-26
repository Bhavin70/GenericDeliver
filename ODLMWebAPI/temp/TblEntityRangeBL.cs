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

namespace ODLMWebAPI.BL
{
    public class TblEntityRangeBL : ITblEntityRangeBL
    {
        #region Selection

        public List<TblEntityRangeTO> SelectAllTblEntityRangeList()
        {
            return  TblEntityRangeDAO.SelectAllTblEntityRange();
        }

        public TblEntityRangeTO SelectTblEntityRangeTO(Int32 idEntityRange)
        {
            return  TblEntityRangeDAO.SelectTblEntityRange(idEntityRange);
        }
        public TblEntityRangeTO SelectTblEntityRangeTOByEntityName(string entityName)
        {
            return TblEntityRangeDAO.SelectTblEntityRangeByEntityName(entityName);
        }
        #region @Kiran Migration Of Invoice 

        public TblEntityRangeTO SelectEntityRangeTOFromInvoiceType(String entityName, int finYearId, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityName, finYearId, conn, tran);
        }
        #endregion
        public TblEntityRangeTO SelectEntityRangeTOFromInvoiceType(Int32 invoiceTypeId,int finYearId, SqlConnection conn,SqlTransaction tran)
        {
            return TblEntityRangeDAO.SelectEntityRangeFromInvoiceType(invoiceTypeId,finYearId, conn, tran);
        }

        // Vaibhav [07-Jan-2018] Added t select entity data
        public TblEntityRangeTO SelectTblEntityRangeTOByEntityName(string entityName, int finYearId, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.SelectTblEntityRangeByEntityName(entityName, finYearId, conn, tran);
        }
        #endregion

        #region Insertion
        public int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            return TblEntityRangeDAO.InsertTblEntityRange(tblEntityRangeTO);
        }

        public int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.InsertTblEntityRange(tblEntityRangeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            return TblEntityRangeDAO.UpdateTblEntityRange(tblEntityRangeTO);
        }

        public int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.UpdateTblEntityRange(tblEntityRangeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblEntityRange(Int32 idEntityRange)
        {
            return TblEntityRangeDAO.DeleteTblEntityRange(idEntityRange);
        }

        public int DeleteTblEntityRange(Int32 idEntityRange, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.DeleteTblEntityRange(idEntityRange, conn, tran);
        }

        #endregion
        
    }
}
