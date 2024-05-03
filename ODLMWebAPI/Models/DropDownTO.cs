using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{

    /// <summary>
    /// Sanjay [2017-02-10] This Model is used to return the values to caller
    /// when dimensions needs to be shown in DropDown
    /// </summary>
    public class DropDownTO
    {

        #region

        Int32 value;
        String text;
        Object tag;
        
        #endregion

        #region Get Set

        /// <summary>
        /// Sanjay [2017-02-10] To Hold the Id of the Dropdown
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Hold the Text to be shown in dropdown
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public object Tag
        {
            get
            {
                return tag;
            }

            set
            {
                tag = value;
            }
        }

        #endregion
    }

    public class DimBrand
    {
       public Int32 IdBrand { get; set; }
        public string BrandName { get; set; }
        public DateTime  CreatedOn { get; set; }
        public Int32 IsActive { get; set; }
        public Int32 IsDefault { get; set; }
        public string ShortNm { get; set; }
        public Int32 isTaxInclusive { get; set; }
        public string prodCatIdStr { get; set; }
        public string materialIdStr { get; set; }
        public string specificationIdStr { get; set; }
        public Int32 isAutoSelect { get; set; }
        public Int32 isBothTaxType { get; set; }
        public Int32 categoryType { get; set; }




    }
    public class DropDownToForParity
    {
        #region

        Int32 id;
        String itemName;


        #endregion
        #region Get
        public string ItemName
        {
            get
            {
                return itemName;
            }

            set
            {
                itemName = value;
            }
        }
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                this.id = value;
            }
        }
        #endregion
    }
}
