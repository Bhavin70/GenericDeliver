using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblPaymentTermOptionRelationTO
    {
        #region Declarations
        Int32 idPaymentTermOptionRelation;
        Int32 paymentTermOptionId;
        Int32 bookingId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 paymentTermId;
        Int32 invoiceId;
        Int32 loadingId;
        String paymentTermOption;
        String paymentTerm;
        String paymentTermsDescription;
        #endregion

        #region Constructor
        public TblPaymentTermOptionRelationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPaymentTermOptionRelation
        {
            get { return idPaymentTermOptionRelation; }
            set { idPaymentTermOptionRelation = value; }
        }
        public Int32 PaymentTermOptionId
        {
            get { return paymentTermOptionId; }
            set { paymentTermOptionId = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }

        public int PaymentTermId { get => paymentTermId; set => paymentTermId = value; }
        public int InvoiceId { get => invoiceId; set => invoiceId = value; }
        public int LoadingId { get => loadingId; set => loadingId = value; }
        public string PaymentTermOption { get => paymentTermOption; set => paymentTermOption = value; }
        public string PaymentTerm { get => paymentTerm; set => paymentTerm = value; }
        public string PaymentTermsDescription { get => paymentTermsDescription; set => paymentTermsDescription = value; }

        #endregion
    }
}
