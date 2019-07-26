using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class PendingBookingRptTO
    {
        #region Declaration

        String cnfName;
        String dealerName;
        Int32 bookingId;
        Int32 cnfOrgId;
        Int32 dealerOrgId;
        DateTime bookingDate;
        String bookingDateStr;
        Double bookingRate;
        Double openingBalanceMT;
        Double todaysBookingMT;
        Double todaysLoadingQtyMT;
        Double loadAndDelBookingQty;
        Double todaysDelBookingQty;
        Double avgPrice;
        Double pendingQty;
        Double closingBalance;
        Int32 noOfDayElapsed;

        // Vaibhav [21-Mar-2018] Added to show in view pending booking report.
        int isConfirmed;
        string isConfirmedStr;
        int transporterScopeYn;
        string transporterScopeYnStr;
        int bookingType;

        #endregion

        #region Get Set

        public string CnfName { get => cnfName; set => cnfName = value; }
        public string DealerName { get => dealerName; set => dealerName = value; }
        public double OpeningBalanceMT { get => openingBalanceMT; set => openingBalanceMT = value; }
        public double TodaysBookingMT { get => todaysBookingMT; set => todaysBookingMT = value; }
        public double TodaysLoadingQtyMT { get => todaysLoadingQtyMT; set => todaysLoadingQtyMT = value; }
        public double AvgPrice { get => avgPrice; set => avgPrice = value; }
        public double PendingQty { get => pendingQty; set => pendingQty = value; }
        public double LoadAndDelBookingQty { get => loadAndDelBookingQty; set => loadAndDelBookingQty = value; }
        public int BookingId { get => bookingId; set => bookingId = value; }
        public DateTime BookingDate { get => bookingDate; set => bookingDate = value; }
        public String BookingDateStr { get { return bookingDate.ToString(Constants.DefaultDateFormat); } }
        public Double BookingRate { get => bookingRate; set => bookingRate = value; }
        public double ClosingBalance { get => closingBalance; set => closingBalance = value; }
        public int CnfOrgId { get => cnfOrgId; set => cnfOrgId = value; }
        public int DealerOrgId { get => dealerOrgId; set => dealerOrgId = value; }
        public double TodaysDelBookingQty { get => todaysDelBookingQty; set => todaysDelBookingQty = value; }
        public int NoOfDayElapsed { get => noOfDayElapsed; set => noOfDayElapsed = value; }


        public int IsConfirmed { get => isConfirmed; set => isConfirmed = value; }
        public string IsConfirmedStr
        {
            get
            {
                if (isConfirmed == 1)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }

            set { isConfirmedStr = value; }

        }

        public int TransporterScopeYn { get => transporterScopeYn; set => transporterScopeYn = value; }
        public string TransporterScopeYnStr
        {
            get
            {
                if (transporterScopeYn == 1)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }

            set { transporterScopeYnStr = value; }

        }

        public Int32 BookingType { get => bookingType; set => bookingType = value; }

        #endregion
    }
}
