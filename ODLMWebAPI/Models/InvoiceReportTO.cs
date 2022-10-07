using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class InvoiceReportTO
    {
        #region Declarations

        String invoiceNo;
        String partyName;
        String consigneeName;
        DateTime invoiceDate;
        Dictionary<String, string > prodMaterialQtyDCT = new Dictionary<string, string >();

        Int32 invoiceId;
        String materialSubType;
        String prodCat;
        Double invoiceQty;
        Double totalQty;
        Double grandTotalQty;
        String superwiserName;
        #endregion

        #region Constructor
        public InvoiceReportTO()
        {

        }
        public String SuperwiserName { get => superwiserName; set => superwiserName = value; }
        public string InvoiceNo { get => invoiceNo; set => invoiceNo = value; }
        public string PartyName { get => partyName; set => partyName = value; }
        public string ConsigneeName { get => consigneeName; set => consigneeName = value; }
        public DateTime InvoiceDate { get => invoiceDate; set => invoiceDate = value; }
        public Dictionary<string, string> ProdMaterialQtyDCT { get => prodMaterialQtyDCT; set => prodMaterialQtyDCT = value; }
        public int InvoiceId { get => invoiceId; set => invoiceId = value; }
        public string MaterialSubType { get => materialSubType; set => materialSubType = value; }
        public string ProdCat { get => prodCat; set => prodCat = value; }
        public double InvoiceQty { get => invoiceQty; set => invoiceQty = value; }
        public double TotalQty { get => totalQty; set => totalQty = value; }
        public double GrandTotalQty { get => grandTotalQty; set => grandTotalQty = value; }
        public string VehicleNo { get ; set ; }//
        public string SaleEngineer { get; set; }
        public string ItemDecscription { get; set; }
        public double Rate { get; set; }
        public double BVCAmt { get; set; }
        public double CDPct { get; set; }
        public double TotalAmt { get; set; }

        public double TCS { get; set; }
        public double Freight { get; set; }
        public double OtherAmt { get; set; }


        #endregion

        #region GetSet

        #endregion
    }
}
