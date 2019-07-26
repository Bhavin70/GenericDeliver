using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblORCReportTO
    {
        #region Declarations

        String dealer;
        String cnfName;
        String statusName;
        Int32 idBooking;
        DateTime createdOn;
        Double bookingQty;
        Double bookingRate;
        String comment;
        Double orcAmt;
        String orcMeasure;
        String rateCalcDesc;

        #endregion

        #region Constructor
        public TblORCReportTO()
        {
        }

        public string RateCalcDesc { get => rateCalcDesc; set => rateCalcDesc = value; }

        #endregion

        #region GetSet
        public String Dealer
        {
            get { return dealer; }
            set { dealer = value; }
        }
        public String CnfName
        {
            get { return cnfName; }
            set { cnfName = value; }
        }
        public String StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
        public Int32 IdBooking
        {
            get { return idBooking; }
            set { idBooking = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        public Double BookingQty
        {
            get { return bookingQty; }
            set { bookingQty = value; }
        }
        public Double BookingRate
        {
            get { return bookingRate; }
            set { bookingRate = value; }
        }
        public string Comment { get => comment; set => comment = value; }

        public Double OrcAmt
        {
            get
            {
                return orcAmt;
            }
            set
            {
                orcAmt = value;
            }
        }
        public String OrcMeasure
        {
            get
            {
                return orcMeasure;
            }
            set
            {
                orcMeasure = value;
            }
        }
        public String RateCalDesc
        {
            get
            {
                return rateCalcDesc;
            }
            set
            {
                rateCalcDesc = value;
            }
        }

        #endregion
    }
}

