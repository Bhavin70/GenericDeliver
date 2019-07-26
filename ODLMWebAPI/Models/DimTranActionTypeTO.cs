using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class DimTranActionTypeTO
    {
        #region Declarations
        Int32 idTranActionType;
        Int32 isActive;
        String transName;
        #endregion

        #region Constructor
        public DimTranActionTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTranActionType
        {
            get { return idTranActionType; }
            set { idTranActionType = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String TransName
        {
            get { return transName; }
            set { transName = value; }
        }
        #endregion
    }
}
