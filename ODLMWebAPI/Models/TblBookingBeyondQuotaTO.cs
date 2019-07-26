using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblBookingBeyondQuotaTO
    {
        #region Declarations
        Int32 idBookingAuth;
        Int32 bookingId;
        Int32 statusId;
        DateTime statusDate;
        Double rate;
        Double quantity;
        Double deliveryPeriod;
        Int32 createdBy;
        DateTime createdOn;
        String createdUserName;
        String statusDesc;
        Double orcAmt;
        Int32 cdStructureId;
        Double cdStructure;
        String remark;
        String statusRemark;
        #endregion

        #region Constructor
        public TblBookingBeyondQuotaTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBookingAuth
        {
            get { return idBookingAuth; }
            set { idBookingAuth = value; }
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
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public Double Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public Double DeliveryPeriod
        {
            get { return deliveryPeriod; }
            set { deliveryPeriod = value; }
        }
        public Constants.TranStatusE TranStatusE
        {
            get
            {
                Constants.TranStatusE tranStatusE = Constants.TranStatusE.BOOKING_NEW;
                if (Enum.IsDefined(typeof(Constants.TranStatusE), statusId))
                {
                    tranStatusE = (Constants.TranStatusE)statusId;
                }
                return tranStatusE;

            }
            set
            {
                statusId = (int)value;
            }
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

        public string CreatedUserName
        {
            get
            {
                return createdUserName;
            }

            set
            {
                createdUserName = value;
            }
        }

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public String StatusDateStr
        {
            get { return statusDate.ToString("dd-MM-yy HH:mm tt"); }
        }

        public string StatusDesc
        {
            get
            {
                return statusDesc;
            }

            set
            {
                statusDesc = value;
            }
        }

        public double OrcAmt { get => orcAmt; set => orcAmt = value; }
        public int CdStructureId { get => cdStructureId; set => cdStructureId = value; }
        public Double CdStructure { get => cdStructure; set => cdStructure = value; }
        public string Remark { get => remark; set => remark = value; }
        public string StatusRemark { get => statusRemark; set => statusRemark = value; }
        #endregion
    }
}
