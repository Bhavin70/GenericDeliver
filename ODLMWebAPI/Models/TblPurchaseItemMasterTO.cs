using System;

namespace ODLMWebAPI.Models
{
    public class TblPurchaseItemMasterTO
    {
        #region Declarations
 
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 isActive;
        Int32 codeTypeId;
       
        #endregion

        #region Constructor
        public TblPurchaseItemMasterTO()
        {
        }

        #endregion

        #region GetSet
        public int IdPurchaseItemMaster { get; set; }
        public int CodeTypeId { get => codeTypeId; set => codeTypeId = value; }
        public string Supplier { get; set; }
        public decimal Priority { get; set; }
        public decimal CurrencyRate { get; set; }
        public decimal BasicRate { get; set; }
        public string GSTItemCode { get; set; }
        public decimal Discount { get; set; }
        public decimal PF { get; set; }
        public decimal Freight { get; set; }
        public decimal DeliveryPeriodInDays { get; set; }
        public decimal MultiplicationFactor { get; set; }
        public decimal MinimumOrderQty { get; set; }
        public decimal SupplierAddress { get; set; }
        public string MfgCatlogNo { get; set; }
        public decimal ItemPerPurchaseUnit { get; set; }
        public decimal Length_mm { get; set; }
        public decimal Width_mm { get; set; }
        public decimal Height_mm { get; set; }
        public decimal Volume_ccm { get; set; }
        public decimal Weight_kg { get; set; }
        public Int32 CurrencyId { get; set; }
        public Int32 SupplierOrgId { get; set; }
        public Int32 GstCodeTypeId { get; set; }
        public Int32 WeightMeasurUnitId { get; set; }
        public Int32 ProdItemId { get; set; }
        public Int32 IsUpdatePurchase { get; set; }
        public Int32 IsAddNewPurchase { get; set; }
        public int IsActive { get => isActive; set => isActive = value; }
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
