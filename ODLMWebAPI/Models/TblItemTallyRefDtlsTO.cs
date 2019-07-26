using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblItemTallyRefDtlsTO
    {
        #region Declarations
        Int32 idItemTallyRef;
        Int32 prodCatId;
        Int32 prodSpecId;
        Int32 materialId;
        Int32 prodItemId;
        Int32 brandId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String overdueTallyRefId;
        String enquiryTallyRefId;
        Int32 isActive;
        String prodCatDesc;
        String prodSpecDesc;
        String materialDesc;

        String displayName;
        String brandName;
        Int32 otherItem;

        #endregion

        #region Constructor
        public TblItemTallyRefDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdItemTallyRef
        {
            get { return idItemTallyRef; }
            set { idItemTallyRef = value; }
        }
        public Int32 ProdCatId
        {
            get { return prodCatId; }
            set { prodCatId = value; }
        }
        public Int32 ProdSpecId
        {
            get { return prodSpecId; }
            set { prodSpecId = value; }
        }
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
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

        public String OverdueTallyRefId
        {
            get { return overdueTallyRefId; }
            set { overdueTallyRefId = value; }
        }
        public String EnquiryTallyRefId
        {
            get { return enquiryTallyRefId; }
            set { enquiryTallyRefId = value; }
        }

        public int IsActive { get => isActive; set => isActive = value; }

        public String ProdCatDesc
        {
            get { return prodCatDesc; }
            set { prodCatDesc = value; }
        }
        public String ProdSpecDesc
        {
            get { return prodSpecDesc; }
            set { prodSpecDesc = value; }
        }
        public String MaterialDesc
        {
            get { return materialDesc; }
            set { materialDesc = value; }
        }

        public string DisplayName { get => displayName; set => displayName = value; }
        public string BrandName { get => brandName; set => brandName = value; }
        public int OtherItem { get => otherItem; set => otherItem = value; }

        #endregion
    }
}
