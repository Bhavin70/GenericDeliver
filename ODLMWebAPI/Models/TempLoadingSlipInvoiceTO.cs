using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TempLoadingSlipInvoiceTO
    {
        #region Declarations
        Int32 idLoadingSlipInvoice;
        Int32 loadingSlipId;
        Int32 invoiceId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        #endregion

        #region Constructor
        public TempLoadingSlipInvoiceTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadingSlipInvoice
        {
            get { return idLoadingSlipInvoice; }
            set { idLoadingSlipInvoice = value; }
        }
        public Int32 LoadingSlipId
        {
            get { return loadingSlipId; }
            set { loadingSlipId = value; }
        }
        public Int32 InvoiceId
        {
            get { return invoiceId; }
            set { invoiceId = value; }
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
        #endregion
    }
}
