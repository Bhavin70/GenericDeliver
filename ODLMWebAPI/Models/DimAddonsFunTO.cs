using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimAddonsFunTO
    {
        #region Declarations
        Int32 idAddonsFun;
        String funName;
        #endregion

        #region Constructor
        public DimAddonsFunTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdAddonsFun
        {
            get { return idAddonsFun; }
            set { idAddonsFun = value; }
        }
        public String FunName
        {
            get { return funName; }
            set { funName = value; }
        }
        #endregion
    }
}
