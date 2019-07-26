using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimSmsConfigTO
    {
        #region Declarations
        Int32 idSmsConfig;
        Int32 isActive;
        Int32 isFilter;
        String smsConfigUrl;
        String brand;
        #endregion

        #region Constructor
        public DimSmsConfigTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSmsConfig
        {
            get { return idSmsConfig; }
            set { idSmsConfig = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String SmsConfigUrl
        {
            get { return smsConfigUrl; }
            set { smsConfigUrl = value; }
        }
        public String Brand
        {
            get { return brand; }
            set { brand = value; }
        }
        public Int32 IsFilter
        {
            get { return isFilter; }
            set { isFilter = value; }
        }

        #endregion

    }
}
