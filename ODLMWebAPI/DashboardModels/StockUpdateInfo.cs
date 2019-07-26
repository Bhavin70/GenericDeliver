using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DashboardModels
{
    public class StockUpdateInfo
    {
        #region Declaration

        private double totalSysStock;
        private double totalBooksStock;
        private double stockFactor;
        private double soldStock;           //Priyanka [27-07-2018] : Added to show sold & unsold stock on dashboard.
        private double unsoldStock;
        private double todaysStock;         //Priyanka [02-08-2018]
        #endregion

        #region Get Set
        public double TotalSysStock { get => totalSysStock; set => totalSysStock = value; }
        public double TotalBooksStock { get => totalBooksStock; set => totalBooksStock = value; }
        public double StockFactor { get => stockFactor; set => stockFactor = value; }
        public double SoldStock { get => soldStock; set => soldStock = value; }
        public double UnsoldStock { get => unsoldStock; set => unsoldStock = value; }
        public double TodaysStock { get => todaysStock; set => todaysStock = value; }

        #endregion
    }
}
