using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblLoadingSlipTO
    {
        #region Declarations
        Int32 idLoadingSlip;
        Int32 dealerOrgId;
        Int32 isJointDelivery;
        Int32 noOfDeliveries;
        Int32 statusId;
        Int32 createdBy;
        DateTime statusDate;
        DateTime loadingDatetime;
        DateTime createdOn;
        Double cdStructure;
        String statusReason;
        String vehicleNo;
        String dealerOrgName;

        TblLoadingSlipDtlTO tblLoadingSlipDtlTO;
        List<TblLoadingSlipExtTO> loadingSlipExtTOList;
        List<TblLoadingSlipAddressTO> deliveryAddressTOList;
        Int32 loadingId;
        String statusName;
        Int32 statusReasonId;
        String loadingSlipNo;
        Int32 isConfirmed;
        String contactNo;
        String driverName;
        String comment;
        Int32 cdStructureId;
        Int32 cnfOrgId;

        //[26-02-2018]Vijaymala Added
        String poNo;
        DateTime poDate;

        List<TblLoadingStatusHistoryTO> loadingStatusHistoryTOList;
        LoadingStatusDateTO loadingStatusDateTO;
        public String cnfOrgName;
        Double orcAmt;          //Priyanka [05-03-2018]
        String orcMeasure;      //Priyanka [06-03-2018]

        string orcPersonName;
                                //Kiran
        List<Dictionary<string, string>> dictionary = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> todaysDictionary = new List<Dictionary<string, string>>();

        Double freightAmt; //Vijaymala[25-04-2018] added
        Int32 isFreightIncluded;  //Vijaymala[25-04-2018] addedIsFreightIncluded

        Double loadingQty;      //Priyanka [04-06-2018]

        Double forAmount;
        Int32 isForAmountIncluded;
        Double addDiscAmt;          //Priyanka [05-07-2018]

        Int32 bookingId;            //Priyanka [05-09-2018]
        Int32 bookingType;         // Aniket [9-Jan-2019] added for to check booking type
        List<TblPaymentTermOptionRelationTO> paymentTermOptionRelationTOLst;
        #endregion

        #region Constructor
        public TblLoadingSlipTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadingSlip
        {
            get { return idLoadingSlip; }
            set { idLoadingSlip = value; }
        }
        public Int32 DealerOrgId
        {
            get { return dealerOrgId; }
            set { dealerOrgId = value; }
        }
        public Int32 IsJointDelivery
        {
            get { return isJointDelivery; }
            set { isJointDelivery = value; }
        }
        public Int32 NoOfDeliveries
        {
            get { return noOfDeliveries; }
            set { noOfDeliveries = value; }
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
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public DateTime LoadingDatetime
        {
            get { return loadingDatetime; }
            set { loadingDatetime = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double CdStructure
        {
            get { return cdStructure; }
            set { cdStructure = value; }
        }
        public String StatusReason
        {
            get { return statusReason; }
            set { statusReason = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }

        public String DealerOrgName
        {
            get { return dealerOrgName; }
            set { dealerOrgName = value; }
        }

        public String StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
        public Constants.TranStatusE TranStatusE
        {
            get
            {
                Constants.TranStatusE tranStatusE = Constants.TranStatusE.LOADING_NEW;
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

        public TblLoadingSlipDtlTO TblLoadingSlipDtlTO
        {
            get
            {
                return tblLoadingSlipDtlTO;
            }

            set
            {
                tblLoadingSlipDtlTO = value;
            }
        }

        public List<TblLoadingSlipExtTO> LoadingSlipExtTOList
        {
            get
            {
                return loadingSlipExtTOList;
            }

            set
            {
                loadingSlipExtTOList = value;
            }
        }

        public Int32 LoadingId
        {
            get { return loadingId; }
            set { loadingId = value; }
        }

        /// <summary>
        /// Sanjay [2017-03-06] To Record Addresses for each loading slip.
        /// Addresses may vary accroding to loading layers
        /// </summary>
        public List<TblLoadingSlipAddressTO> DeliveryAddressTOList
        {
            get
            {
                return deliveryAddressTOList;
            }

            set
            {
                deliveryAddressTOList = value;
            }
        }


        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public String StatusDateStr
        {
            get { return statusDate.ToString(Constants.DefaultDateFormat); }
        }

        public int StatusReasonId
        {
            get
            {
                return statusReasonId;
            }

            set
            {
                statusReasonId = value;
            }
        }

        public string LoadingSlipNo
        {
            get
            {
                return loadingSlipNo;
            }

            set
            {
                loadingSlipNo = value;
            }
        }

        public int IsConfirmed
        {
            get
            {
                return isConfirmed;
            }

            set
            {
                isConfirmed = value;
            }
        }
        /// <summary>
        /// Priyanka [05-03-2018]
        /// </summary>
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
        public string ContactNo { get => contactNo; set => contactNo = value; }
        public string DriverName { get => driverName; set => driverName = value; }
        public string Comment { get => comment; set => comment = value; }
        public int CdStructureId { get => cdStructureId; set => cdStructureId = value; }

        //Sudhir[27-02-2018] Added for the StatusHistoryList
        public List<TblLoadingStatusHistoryTO> LoadingStatusHistoryTOList
        {
            get
            {
                return loadingStatusHistoryTOList;
            }

            set
            {
                loadingStatusHistoryTOList = value;
            }
        }


        public LoadingStatusDateTO LoadingStatusDateTO
        {
            get
            {
                return loadingStatusDateTO;
            }

            set
            {
                loadingStatusDateTO = value;
            }
        }

        //[26-02-2018]Vijaymala Added
        public string PoNo { get => poNo; set => poNo = value; }
        public DateTime PoDate { get => poDate; set => poDate = value; }

        public Int32 CnfOrgId
        {
            get { return cnfOrgId; }
            set { cnfOrgId = value; }
        }

        public string CnfOrgName { get => cnfOrgName; set => cnfOrgName = value; }

        public String PoDateStr
        {
            get
            {
                if (poDate != new DateTime())
                {
                    return poDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    return null;
                }

            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    poDate = Convert.ToDateTime(value);
            }
        }

        public List<Dictionary<string, string>> Dictionary { get => dictionary; set => dictionary = value; }

        public List<Dictionary<string, string>> TodaysDictionary { get => todaysDictionary; set => todaysDictionary = value; }

        #endregion


        public TblLoadingSlipTO DeepCopy()
        {
            TblLoadingSlipTO other = (TblLoadingSlipTO)this.MemberwiseClone();
            return other;
        }


        public string ORCPersonName
        {
            get { return orcPersonName; }
            set { orcPersonName = value; }
        }

        //Priyanka [04-06-2018]
        public double LoadingQty { get => loadingQty; set => loadingQty = value; }
        public double FreightAmt { get => freightAmt; set => freightAmt = value; }
        public int IsFreightIncluded { get => isFreightIncluded; set => isFreightIncluded = value; }
        public double ForAmount { get => forAmount; set => forAmount = value; }
        public int IsForAmountIncluded { get => isForAmountIncluded; set => isForAmountIncluded = value; }
        public double AddDiscAmt { get => addDiscAmt; set => addDiscAmt = value; }
        public int BookingId { get => bookingId; set => bookingId = value; }
        public int BookingType { get => bookingType; set => bookingType = value; }
        public List<TblPaymentTermOptionRelationTO> PaymentTermOptionRelationTOLst { get => paymentTermOptionRelationTOLst; set => paymentTermOptionRelationTOLst = value; }
    }
}
