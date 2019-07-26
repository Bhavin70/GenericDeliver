using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class CnFWiseReportTO
    {
        #region Declarations
        String cnfName;
        Int32 confirmbooking;
        Int32 notconfirmbooking;
        #endregion

        #region GetSet
        public string CnfName { get => cnfName; set => cnfName = value; }
        public int ConfirmBooking { get => confirmbooking; set => confirmbooking = value; }
        public int NotConfirmBooking { get => notconfirmbooking; set => notconfirmbooking = value; }
        #endregion
    }
}
