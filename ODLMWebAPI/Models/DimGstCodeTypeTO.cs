using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class DimGstCodeTypeTO
    {
        #region Declarations
        Int32 idCodeType;
        DateTime createdOn;
        String codeDesc;
        #endregion

        #region Constructor
        public DimGstCodeTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdCodeType
        {
            get { return idCodeType; }
            set { idCodeType = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String CodeDesc
        {
            get { return codeDesc; }
            set { codeDesc = value; }
        }
        #endregion
    }
}
