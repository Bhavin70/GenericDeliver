using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class DimBrandTO
    {
        #region Declarations
        Int32 idBrand;
        Int32 isActive;
        DateTime createdOn;
        String brandName;
        Int32 isDefault;  //[05-09-2018]Vijaymala added to set default brand
        String shortNm  ;
        Int32 isTaxInclusive;
        #endregion

        #region Constructor
        public DimBrandTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBrand
        {
            get { return idBrand; }
            set { idBrand = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }

        public Int32 IsDefault { get => isDefault; set => isDefault = value; }
        public string ShortNm { get => shortNm; set => shortNm = value; }
        public int IsTaxInclusive { get => isTaxInclusive; set => isTaxInclusive = value; }
        #endregion
    }
}
