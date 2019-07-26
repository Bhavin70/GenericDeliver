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
    public class TblPaymentTermOptionRelationBL : ITblPaymentTermOptionRelationBL
    {
        private readonly ITblPaymentTermOptionRelationDAO _iTblPaymentTermOptionRelationDAO;
       public TblPaymentTermOptionRelationBL(ITblPaymentTermOptionRelationDAO iTblPaymentTermOptionRelationDAO)
        {
            _iTblPaymentTermOptionRelationDAO = iTblPaymentTermOptionRelationDAO;
        }
        #region Selection
        public List<TblPaymentTermOptionRelationTO> SelectAllTblPaymentTermOptionRelation()
        {
            return _iTblPaymentTermOptionRelationDAO.SelectAllTblPaymentTermOptionRelation();
        }

        public List<TblPaymentTermOptionRelationTO> SelectAllTblPaymentTermOptionRelationList()
        {
           return _iTblPaymentTermOptionRelationDAO.SelectAllTblPaymentTermOptionRelation();
        }

        public TblPaymentTermOptionRelationTO SelectTblPaymentTermOptionRelationTO(Int32 idPaymentTermOptionRelation)
        {
            TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelation(idPaymentTermOptionRelation);
            if (tblPaymentTermOptionRelationTO != null)
                return tblPaymentTermOptionRelationTO;
            else
                return null;
        }

        //Priyanka [21-09-2019] 
        public TblPaymentTermOptionRelationTO SelectTblPaymentTermOptionRelationTOByBookingId(Int32 bookingId, Int32 paymentTermId)
        {
            List<TblPaymentTermOptionRelationTO> tblPaymentTermOptionRelationTOLst = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelationByBookingIdForUpdate(bookingId,paymentTermId);
            if (tblPaymentTermOptionRelationTOLst != null)
                return tblPaymentTermOptionRelationTOLst[0];
            else
                return null;
        }

        //Priyanka [22-01-2019]
        public TblPaymentTermOptionRelationTO SelectTblPaymentTermOptionRelationTOByInvoiceId(Int32 invoiceId, Int32 paymentTermId)
        {
            List<TblPaymentTermOptionRelationTO> tblPaymentTermOptionRelationTOLst = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelationByInvoiceIdForUpdate(invoiceId, paymentTermId);
            if (tblPaymentTermOptionRelationTOLst != null)
                return tblPaymentTermOptionRelationTOLst[0];
            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO)
        {
            return _iTblPaymentTermOptionRelationDAO.InsertTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTO);
        }

        public int InsertTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermOptionRelationDAO.InsertTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO)
        {
            return _iTblPaymentTermOptionRelationDAO.UpdateTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTO);
        }

        public int UpdateTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermOptionRelationDAO.UpdateTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation)
        {
            return _iTblPaymentTermOptionRelationDAO.DeleteTblPaymentTermOptionRelation(idPaymentTermOptionRelation);
        }

        public int DeleteTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermOptionRelationDAO.DeleteTblPaymentTermOptionRelation(idPaymentTermOptionRelation, conn, tran);
        }

        #endregion
        
    }
}
