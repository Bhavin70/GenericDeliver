using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DashboardModels
{
    public class BookingInfo
    {
        #region Declaration
     
        //Double bookedQty;
        //Double avgPrice;

        Double bookingQty;
        Double totalCost;
        Double avgPrice;
        Int32 isConfirmed;
        string brandName;
        Int32 bookingType;
        string shortNm;

        #endregion

        #region GetSet
        public double BookingQty { get => bookingQty; set => bookingQty = value; }
        public double TotalCost { get => totalCost; set => totalCost = value; }
        public double AvgPrice { get => avgPrice; set => avgPrice = value; }
        public int IsConfirmed { get => isConfirmed; set => isConfirmed = value; }
        public string BrandName { get => brandName; set => brandName = value; }
        public int BookingType { get => bookingType; set => bookingType = value; }
        public string ShortNm { get => shortNm; set => shortNm = value; }


        #endregion
    }
}
