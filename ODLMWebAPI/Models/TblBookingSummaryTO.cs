using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class TblBookingSummaryTO
    {

        #region Declarations

        string displayName;
        Double bookingQty;
        DateTime timeView;

        #endregion

        #region Constructor
        public TblBookingSummaryTO()
        {

        }

        #endregion

        #region GetSet

        public string DisplayName { get => displayName; set => displayName = value; }
        public double BookingQty { get => bookingQty; set => bookingQty = value; }
        public DateTime TimeView { get => timeView; set => timeView = value; }

        #endregion
    }
}
