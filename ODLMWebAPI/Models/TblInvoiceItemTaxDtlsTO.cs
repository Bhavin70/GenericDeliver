using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblInvoiceItemTaxDtlsTO
    {
        #region Declarations
        Int32 idInvItemTaxDtl;
        Int32 invoiceItemId;
        Int32 taxRateId;
        Double taxPct;
        Double taxRatePct;
        Double taxableAmt;
        Double taxAmt;
        Int32 taxTypeId;
        String gstinCodeNo;
        //Aniket [12-03-2019] added to remove other tax amount while displying on print invoice
        Int32 isBefore;
        Int32 isAfter;
        Int32 both;
        #endregion

        #region Constructor
        public TblInvoiceItemTaxDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdInvItemTaxDtl
        {
            get { return idInvItemTaxDtl; }
            set { idInvItemTaxDtl = value; }
        }
        public Int32 InvoiceItemId
        {
            get { return invoiceItemId; }
            set { invoiceItemId = value; }
        }
        public Int32 TaxRateId
        {
            get { return taxRateId; }
            set { taxRateId = value; }
        }
        public Double TaxPct
        {
            get { return taxPct; }
            set { taxPct = value; }
        }
        public Double TaxRatePct
        {
            get { return taxRatePct; }
            set { taxRatePct = value; }
        }
        public Double TaxableAmt
        {
            get { return taxableAmt; }
            set { taxableAmt = value; }
        }
        public Double TaxAmt
        {
            get { return taxAmt; }
            set { taxAmt = value; }
        }

        public int TaxTypeId { get => taxTypeId; set => taxTypeId = value; }
        public string GstinCodeNo { get => gstinCodeNo; set => gstinCodeNo = value; }
        public int IsBefore { get => isBefore; set => isBefore = value; }
        public int IsAfter { get => isAfter; set => isAfter = value; }
        public int Both { get => both; set => both = value; }
        #endregion

        #region MyRegion

        public TblInvoiceItemTaxDtlsTO DeepCopy()
        {
            TblInvoiceItemTaxDtlsTO other = (TblInvoiceItemTaxDtlsTO)this.MemberwiseClone();
            return other;
        }

        #endregion
    }
}
