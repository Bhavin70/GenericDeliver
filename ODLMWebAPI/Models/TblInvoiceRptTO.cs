using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class TblInvoiceRptTO
    {
        #region Declarations
        Int32 idInvoice;
        String invoiceNo;
        String vehicleNo;
        DateTime invoiceDate;
        String partyName;
        String cnfName;
        Double bookingRate;
        Int32 invoiceItemId;
        String prodItemDesc;
        String bundles;
        Double rate;
        Double cdStructure;
        Double orcAmt;
        Double cdAmt;
        Double taxRatePct;
        Int32 taxTypeId;
        Double freightAmt;
        Double tcsAmt;
        Double invoiceQty;
        Double taxableAmt;
        Double taxAmt;
        DateTime createdOn;

        Int32 billingTypeId;
        String buyer;
        String buyerGstNo;
        Int32 consigneeTypeId;
        String consignee;
        String consigneeGstNo;
        String deliveryLocation;
        Double basicAmt;
        Double cgstAmt;
        Double sgstAmt;
        Double igstAmt;
        double grandTotal;
        Int32 gstinCodeNo;
        Int32 stateId;
        Int32 otherTaxId;
        Int32 isConfirmed;
        Int32 statusId;
        Int32 stateOrUTCode;
        DateTime statusDate;
        String narration;


        String buyerState;
        String consigneeAddress;
        String consigneeDistict;
        String consigneePinCode;
        String consigneeState;
        String materialName;
        String transporterName;
        String contactNo;

        Double cgstPct;
        Double sgstPct;
        Double igstPct;
        String cnfMobNo;
        String dealerMobNo;
        DateTime lrDate;
        String lrNumber;

        Int32 loadingId;

        //Vijaymala Added[14-03-2018]
        String enq_ref_id;
        String overdue_ref_id;
        String buyer_overdue_ref_id;
        Double invoiceTaxableAmt;
        Double invoiceDiscountAmt;
        String overdueTallyRefId;
        String enquiryTallyRefId;
        DateTime deliveredOn;
        String salesEngineer;
        String orcMeasure;
        Double loadingQty;
        Double totalItemQty;

        //Gokul Added [03-02-2021]
        string ownerPersonFirstName;
        string ownerPersonLastName;
        string contactName;

        //Added by minal
        String panNo;       
        String dealers;
        String paymentTerms;
        String orderNoandDate;
        String termsofDelivery;
        String deliveryNoteAndNo;
        String dispatchDocNo;
        String voucherClass;  
        String narrationConcat;
        String prodCateDesc;
        Double basicRate;
        String loadingSlipDate;
        String transactionDate;
        string invoiceMode;
        Double tdsAmt;
        string salesLedger;
        String orgGstNo;
        #endregion

        #region Constructor


        #endregion

        #region GetSet

        public string OwnerPersonFirstName
        {
            get { return ownerPersonFirstName; }
            set { ownerPersonFirstName = value; }
        }

        public string OwnerPersonLastName
        {
            get { return ownerPersonLastName; }
            set { ownerPersonLastName = value; }
        }

        public string ContactName
        {
            get { return contactName; }
            set {contactName = value;
            }
        }
        public Int32 IdInvoice
        {
            get { return idInvoice; }
            set { idInvoice = value; }
        }
        public String InvoiceNo
        {
            get { return invoiceNo; }
            set { invoiceNo = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { invoiceDate = value; }
        }
        public String PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }
        public String CnfName
        {
            get { return cnfName; }
            set { cnfName = value; }
        }
        public Double BookingRate
        {
            get { return bookingRate; }
            set { bookingRate = value; }
        }
        public Int32 InvoiceItemId
        {
            get { return invoiceItemId; }
            set { invoiceItemId = value; }
        }
        public String ProdItemDesc
        {
            get { return prodItemDesc; }
            set { prodItemDesc = value; }
        }
        public String Bundles
        {
            get { return bundles; }
            set { bundles = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public Double CdStructure
        {
            get { return cdStructure; }
            set { cdStructure = value; }
        }
        public Double CdAmt
        {
            get { return cdAmt; }
            set { cdAmt = value; }
        }
        public Double TaxRatePct
        {
            get { return taxRatePct; }
            set { taxRatePct = value; }
        }
        public Int32 TaxTypeId
        {
            get { return taxTypeId; }
            set { taxTypeId = value; }
        }
        public Double FreightAmt
        {
            get { return freightAmt; }
            set { freightAmt = value; }
        }
        public Double InvoiceQty
        {
            get { return invoiceQty; }
            set { invoiceQty = value; }
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
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String OrgGstNo
        {
            get { return orgGstNo; }
            set { orgGstNo = value; }
        }

        
        public String InvoiceDateStr
        {
            get { return invoiceDate.ToString("dd-MM-yyyy"); }
        }
        public String statusDateStr
        {
            get { return statusDate.ToString("dd-MM-yyyy"); }
        }

        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }


        public String InvoiceNoWrtDate
        {
            get
            {

                return createdOn.ToString("ddMMyyyyHHmmss") + "N" + idInvoice + "  ";
            }
        }
        public Int32 BillingTypeId
        {
            get { return billingTypeId; }
            set { billingTypeId = value; }
        }
        public String Buyer
        {
            get { return buyer; }
            set { buyer = value; }
        }
        public String BuyerGstNo
        {
            get { return buyerGstNo; }
            set { buyerGstNo = value; }
        }
        public Int32 ConsigneeTypeId
        {
            get { return consigneeTypeId; }
            set { consigneeTypeId = value; }
        }
        public String Consignee
        {
            get { return consignee; }
            set { consignee = value; }
        }
        public  String ConsigneeGstNo
         {
            get { return consigneeGstNo; }
            set { consigneeGstNo = value; }
        }
        public String DeliveryLocation
        {
            get { return deliveryLocation; }
            set { deliveryLocation = value; }
        }
        public Double BasicAmt
        {
            get { return basicAmt; }
            set { basicAmt = value; }
        }
        public Double CgstTaxAmt
         {
            get { return cgstAmt; }
            set { cgstAmt = value; }
        }
        public Double SgstTaxAmt
        {
            get { return sgstAmt; }
            set { sgstAmt = value; }
        }
        public Double IgstTaxAmt
        {
            get { return igstAmt; }
            set { igstAmt = value; }
        }
        public Double GrandTotal
        {
            get { return grandTotal; }
            set { grandTotal = value; }
        }
        public Int32 GstinCodeNo
        {
            get { return gstinCodeNo; }
            set { gstinCodeNo = value; }
        }
        public Int32 StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }

        public Int32 IsConfirmed
        {
            get { return isConfirmed; }
            set { isConfirmed = value; }
        }
        public Int32 OtherTaxId
        {
            get { return otherTaxId; }
            set { otherTaxId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 StateOrUTCode
        {
            get { return stateOrUTCode; }
            set { stateOrUTCode = value; }
        }

        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }

        public String BuyerState
        {
            get { return buyerState; }
            set { buyerState = value; }
        }

        public String ConsigneeAddress
        {
            get { return consigneeAddress; }
            set { consigneeAddress = value; }
        }
        public String ConsigneeDistict
        {
            get { return consigneeDistict; }
            set { consigneeDistict = value; }
        }

        public String ConsigneePinCode
        {
            get { return consigneePinCode; }
            set { consigneePinCode = value; }
        }

        public String ConsigneeState
        {
            get { return consigneeState; }
            set { consigneeState = value; }
        }

        public String MaterialName
        {
            get { return materialName; }
            set { materialName = value; }
        }


        public String TransporterName
        {
            get { return transporterName; }
            set { transporterName = value; }
        }

        public String ContactNo
        {
            get { return contactNo; }
            set { contactNo = value; }
        }

        public double CgstPct
        {
            get { return cgstPct; }
            set { cgstPct = value; }
        }

        public double SgstPct
        {
            get { return sgstPct; }
            set { sgstPct = value; }
        }

        public double IgstPct
        {
            get { return igstPct; }
            set { igstPct = value; }
        }
        public String StatusDateStrNew
        {
            get { return statusDate.ToString("dd-MMM-yy"); }
        }

        public String CnfMobNo
        {
            get { return cnfMobNo; }
            set { cnfMobNo = value; }
        }

        public String DealerMobNo
        {
            get { return dealerMobNo; }
            set { dealerMobNo = value; }
        }
        public DateTime LrDate
        {
            get { return lrDate; }
            set { lrDate = value; }
        }
        public String LrNumber
        {
            get { return lrNumber; }
            set { lrNumber = value; }
        }
        public String LrDateStr
        {

            get
            {
                if (lrDate != new DateTime())
                {
                    return lrDate.ToString("dd-MMM-yy");
                }
                else
                {
                    return null;
                }
            }
        }
        public Int32 LoadingId
        {
            get { return loadingId; }
            set { loadingId = value; }
        }

        public string Enq_ref_id { get => enq_ref_id; set => enq_ref_id = value; }
        public string Overdue_ref_id { get => overdue_ref_id; set => overdue_ref_id = value; }
        public string Buyer_overdue_ref_id { get => buyer_overdue_ref_id; set => buyer_overdue_ref_id = value; }
        public double InvoiceTaxableAmt { get => invoiceTaxableAmt; set => invoiceTaxableAmt = value; }
        public double InvoiceDiscountAmt { get => invoiceDiscountAmt; set => invoiceDiscountAmt = value; }
        public string OverdueTallyRefId { get => overdueTallyRefId; set => overdueTallyRefId = value; }
        public string EnquiryTallyRefId { get => enquiryTallyRefId; set => enquiryTallyRefId = value; }
        public DateTime DeliveredOn { get => deliveredOn; set => deliveredOn = value; }
        public string SalesEngineer { get => salesEngineer; set => salesEngineer = value; }
        public double TcsAmt { get => tcsAmt; set => tcsAmt = value; }
        public double OrcAmt { get => orcAmt; set => orcAmt = value; }
        public string OrcMeasure { get => orcMeasure; set => orcMeasure = value; }
        public double LoadingQty { get => loadingQty; set => loadingQty = value; }
        public double TotalItemQty { get => totalItemQty; set => totalItemQty = value; }
        public String PanNo { get => panNo; set => panNo = value; }       
        public String Dealers { get => dealers; set => dealers = value; }
        public String PaymentTerms { get => paymentTerms; set => paymentTerms = value; }
        public String OrderNoandDate
        {
             get => orderNoandDate; set => orderNoandDate = value; 
        }
        public String TermsofDelivery { get => termsofDelivery; set => termsofDelivery = value; }
        public String DeliveryNoteAndNo { get => deliveryNoteAndNo; set => deliveryNoteAndNo = value; }

        public String DispatchDocNo
        {
            get { return dispatchDocNo; }
            set { dispatchDocNo = value; }
        }
        public String SalesLedger
        {
            get { return salesLedger; }
            set { salesLedger = value; }
        }

        


        //public String DispatchDocNo { get => dispatchDocNo; set => dispatchDocNo = value; }
        public String VoucherClass { get => voucherClass; set => voucherClass = value; }
        

        public String NarrationConcat { get => narrationConcat; set => narrationConcat = value; }

        public String ProdCateDesc { get => prodCateDesc; set => prodCateDesc = value; }

        public Double BasicRate { get => basicRate; set => basicRate = value; }
        public String LoadingSlipDate { get => loadingSlipDate; set => loadingSlipDate = value; }
        public String TransactionDate { get => transactionDate; set => transactionDate = value; }

        public string InvoiceMode { get => invoiceMode; set => invoiceMode = value; }

        public string ElectronicRefNo { get; set; }
        public string IrnNo { get; set; }
        public string CodeNumber { get; set; }
        public double TareWeight { get; set; }
        public double GrossWeight { get; set; }
        public double NetWeight { get; set; }
        public double RoundOffAmt { get; set; }
        public string InvoiceTypeDesc { get; set; }
        
        public string BuyerAddress { get; set; }
        public string BuyerDistrict { get; set; }
        public string BuyerPinCode { get; set; }
        public string BuyerTaluka { get; set; }
        public string ConsigneeTaluka { get; set; }
        public string InvFromOrgName { get; set; }
        public string Orgpincode { get; set; }
        public string OrgareaName { get; set; }
        public string OrgplotNo { get; set; }
        public string OrgvillageName { get; set; }
        public string OrgdistrictName { get; set; }
        public string OrgstateName { get; set; }
        public string OrgcountryName { get; set; }
        public double TdsAmt { get => tdsAmt; set => tdsAmt = value; }
        #endregion
    }
}
