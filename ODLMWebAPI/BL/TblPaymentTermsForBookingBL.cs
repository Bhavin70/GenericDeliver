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
    public class TblPaymentTermsForBookingBL : ITblPaymentTermsForBookingBL
    {
        private readonly ITblPaymentTermsForBookingDAO _iTblPaymentTermsForBookingDAO;
        private readonly ITblPaymentTermOptionRelationDAO _iTblPaymentTermOptionRelationDAO;
        private readonly ITblPaymentTermOptionsDAO _iTblPaymentTermOptionsDAO;
        public TblPaymentTermsForBookingBL(ITblPaymentTermOptionsDAO iTblPaymentTermOptionsDAO, ITblPaymentTermOptionRelationDAO iTblPaymentTermOptionRelationDAO, ITblPaymentTermsForBookingDAO iTblPaymentTermsForBookingDAO)
        {
            _iTblPaymentTermsForBookingDAO = iTblPaymentTermsForBookingDAO;
            _iTblPaymentTermOptionRelationDAO = iTblPaymentTermOptionRelationDAO;
            _iTblPaymentTermOptionsDAO = iTblPaymentTermOptionsDAO;
        }
        #region Selection
        public List<TblPaymentTermsForBookingTO> SelectAllTblPaymentTermsForBooking()
        {
            return _iTblPaymentTermsForBookingDAO.SelectAllTblPaymentTermsForBooking();
        }

        public TblPaymentTermsForBookingTO SelectTblPaymentTermsForBookingTO(Int32 idPaymentTerm)
        {
            return _iTblPaymentTermsForBookingDAO.SelectTblPaymentTermsForBooking(idPaymentTerm);
        }
        /// <summary>
        /// Priyanka [18-01-2019]
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public List<TblPaymentTermsForBookingTO> SelectAllTblPaymentTermsForBookingFromBookingId(Int32 bookingId, Int32 invoiceId)
        {
            List<TblPaymentTermOptionRelationTO> tblPaymentTermOptionRelationTOList = new List<TblPaymentTermOptionRelationTO>(); 
            if(invoiceId > 0)
            {
                tblPaymentTermOptionRelationTOList = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelationByInvoiceId(invoiceId);
            }
            if (bookingId > 0)
            {
                tblPaymentTermOptionRelationTOList = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelationByBookingId(bookingId);
            }
            List<TblPaymentTermsForBookingTO> tblPaymentTermsForBookingTOList = _iTblPaymentTermsForBookingDAO.SelectAllTblPaymentTermsForBooking();
            if(tblPaymentTermsForBookingTOList != null && tblPaymentTermsForBookingTOList.Count > 0)
            {
                for(int i = 0; i < tblPaymentTermsForBookingTOList.Count; i++)
                {
                    List<TblPaymentTermOptionsTO> tblPaymentTermOptionsTOList = _iTblPaymentTermOptionsDAO.SelectTblPaymentTermOptionRelationBypaymentTermId(tblPaymentTermsForBookingTOList[i].IdPaymentTerm);
                    if (tblPaymentTermOptionsTOList != null && tblPaymentTermOptionsTOList.Count > 0)
                    {
                        tblPaymentTermsForBookingTOList[i].PaymentTermOptionList = tblPaymentTermOptionsTOList;

                        if (tblPaymentTermOptionRelationTOList != null && tblPaymentTermOptionRelationTOList.Count > 0)
                        {
                            for (int j = 0; j < tblPaymentTermOptionsTOList.Count; j++)
                            {
                                TblPaymentTermOptionsTO tblPaymentTermOptionsTO = tblPaymentTermOptionsTOList[j];
                                TblPaymentTermOptionRelationTO temp = tblPaymentTermOptionRelationTOList.Where(e => e.PaymentTermOptionId == tblPaymentTermOptionsTO.IdPaymentTermOption).FirstOrDefault();
                                if(temp != null)
                                {
                                    tblPaymentTermOptionsTO.IsSelected = 1;
                                    if(!string.IsNullOrEmpty(temp.PaymentTermsDescription))
                                    tblPaymentTermOptionsTO.PaymentTermsDescription = temp.PaymentTermsDescription;
                                    break;
                                }

                            }
                        }
                      
                    }
                }
            }

            return tblPaymentTermsForBookingTOList;
        }
        

        #endregion

        #region Insertion
        public int InsertTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO)
        {
            return _iTblPaymentTermsForBookingDAO.InsertTblPaymentTermsForBooking(tblPaymentTermsForBookingTO);
        }

        public int InsertTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermsForBookingDAO.InsertTblPaymentTermsForBooking(tblPaymentTermsForBookingTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO)
        {
            return _iTblPaymentTermsForBookingDAO.UpdateTblPaymentTermsForBooking(tblPaymentTermsForBookingTO);
        }

        public int UpdateTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermsForBookingDAO.UpdateTblPaymentTermsForBooking(tblPaymentTermsForBookingTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblPaymentTermsForBooking(Int32 idPaymentTerm)
        {
            return _iTblPaymentTermsForBookingDAO.DeleteTblPaymentTermsForBooking(idPaymentTerm);
        }

        public int DeleteTblPaymentTermsForBooking(Int32 idPaymentTerm, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermsForBookingDAO.DeleteTblPaymentTermsForBooking(idPaymentTerm, conn, tran);
        }

        #endregion
        
    }
}
