using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblPaymentTermsForBookingTO
    {
        #region Declarations
        Int32 idPaymentTerm;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String paymentTerm;
        String paymentTermDesc;
        List<TblPaymentTermOptionsTO> paymentTermOptionList;
        #endregion

        #region Constructor
        public TblPaymentTermsForBookingTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPaymentTerm
        {
            get { return idPaymentTerm; }
            set { idPaymentTerm = value; }
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
        public String PaymentTerm
        {
            get { return paymentTerm; }
            set { paymentTerm = value; }
        }
        public String PaymentTermDesc
        {
            get { return paymentTermDesc; }
            set { paymentTermDesc = value; }
        }

        public List<TblPaymentTermOptionsTO> PaymentTermOptionList { get => paymentTermOptionList; set => paymentTermOptionList = value; }
        #endregion
    }
}
