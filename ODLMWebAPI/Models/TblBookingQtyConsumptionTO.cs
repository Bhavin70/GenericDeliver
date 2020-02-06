using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblBookingQtyConsumptionTO
    {
        #region Declarations
        Int32 idBookQtyConsuption;
        Int32 bookingId;
        Int32 statusId;
        Int32 createdBy;
        DateTime createdOn;
        Double consumptionQty;
        String weightTolerance;
        String remark;
        String statusName;
        String userDisplayName;
        #endregion

        #region Constructor
        public TblBookingQtyConsumptionTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBookQtyConsuption
        {
            get { return idBookQtyConsuption; }
            set { idBookQtyConsuption = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double ConsumptionQty
        {
            get { return consumptionQty; }
            set { consumptionQty = value; }
        }
        public String WeightTolerance
        {
            get { return weightTolerance; }
            set { weightTolerance = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public String StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
        public String UserDisplayName
        {
            get { return userDisplayName; }
            set { userDisplayName = value; }
        }
        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        #endregion
    }
}
