using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class DimTaxTypeTO
    {
        #region Declarations
        Int32 idTaxType;
        Int32 isActive;
        DateTime createdOn;
        String taxTypeDesc;
        #endregion

        #region Constructor
        public DimTaxTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTaxType
        {
            get { return idTaxType; }
            set { idTaxType = value; }
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
        public String TaxTypeDesc
        {
            get { return taxTypeDesc; }
            set { taxTypeDesc = value; }
        }
        #endregion
    }
}
