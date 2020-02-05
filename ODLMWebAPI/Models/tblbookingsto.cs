using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblBookingsTO
    {
        #region Declarations
        Int32 idBooking;
        Int32 cnFOrgId;
        Int32 dealerOrgId;
        Int32 deliveryDays;
        Int32 noOfDeliveries;
        Int32 isConfirmed;
        Int32 isJointDelivery;
        Int32 isSpecialRequirement;
        Double cdStructure;
        Int32 statusId;
        Int32 isWithinQuotaLimit;
        Int32 globalRateId;
        Int32 quotaDeclarationId;
        Int32 quotaQtyBforBooking;
        Int32 quotaQtyAftBooking;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime bookingDatetime;
        DateTime statusDate;
        DateTime updatedOn;
        Double bookingQty;
        Double bookingRate;
        String comments;

        String cnfName;
        String dealerName;

        String dealerCat;
        String colorCode;

        List<TblBookingDelAddrTO> deliveryAddressLst;
        List<TblBookingExtTO> orderDetailsLst;
        List<TblBookingExtTO> orderDetailsLstForItemWise;
        String status;
        Double pendingQty;
        Double loadingQty;

        Int32 isDeleted;
        String authReasons;
        Int32 cdStructureId;
        Int32 parityId;
        Double orcAmt;
        String orcMeasure;
        String billingName;
        String poNo;
        String statusRemark;
        Int32 transporterScopeYn;

        double overdueAmt;
        String vehicleNo;
        Int32 brandId;
        String brandName;
        Double freightAmt;
        String pOFileBase64;
        double enquiryAmt;

        String projectName;

        List<TblBookingScheduleTO> bookingScheduleTOLst;

        //[26-02-2018]Vijaymala Added
        DateTime poDate;
        string orcPersonName;

        string createdByName;
        string updatedByName;
        Int32 isOverdueExist;           //Priyanka [08-06-2018] : Added for SHIVANGI.
        Double sizesQty;                //Priyanka [21-06-2018] : Added for SHIVANGI.
        String directorRemark;          //Priyanka [25-06-2018]
        Int32 isOrgOverDue;             //Priyanka [24-07-2018]
       
        Int32 statusBy;
        Int32 tranActionTypeId;         //Priyanka [10-08-2018]

        Int32 bookingType; //Vijaymala[17-08-2018]

        Int32 isSez;
        //Aniket
       
        double uomQty;
        double pendingUomQty;
        int isInUom;
        int isItemized;

        List<TblPaymentTermOptionRelationTO> paymentTermOptionRelationTOLst;
        //Aniket [23-7-2019]
        int stateId;

        Double creditLimit;
        int otherNewBooking;
        #endregion

        #region Constructor
        public TblBookingsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBooking
        {
            get { return idBooking; }
            set { idBooking = value; }
        }
        public Int32 CnFOrgId
        {
            get { return cnFOrgId; }
            set { cnFOrgId = value; }
        }
        public Int32 DealerOrgId
        {
            get { return dealerOrgId; }
            set { dealerOrgId = value; }
        }
        public Int32 DeliveryDays
        {
            get { return deliveryDays; }
            set { deliveryDays = value; }
        }
        public Int32 NoOfDeliveries
        {
            get { return noOfDeliveries; }
            set { noOfDeliveries = value; }
        }
        public Int32 IsConfirmed
        {
            get { return isConfirmed; }
            set { isConfirmed = value; }
        }
        public Int32 IsJointDelivery
        {
            get { return isJointDelivery; }
            set { isJointDelivery = value; }
        }
        public Int32 IsSpecialRequirement
        {
            get { return isSpecialRequirement; }
            set { isSpecialRequirement = value; }
        }
        public Double CdStructure
        {
            get { return cdStructure; }
            set { cdStructure = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 IsWithinQuotaLimit
        {
            get { return isWithinQuotaLimit; }
            set { isWithinQuotaLimit = value; }
        }
        public Int32 GlobalRateId
        {
            get { return globalRateId; }
            set { globalRateId = value; }
        }
        public Int32 QuotaDeclarationId
        {
            get { return quotaDeclarationId; }
            set { quotaDeclarationId = value; }
        }
        public Int32 QuotaQtyBforBooking
        {
            get { return quotaQtyBforBooking; }
            set { quotaQtyBforBooking = value; }
        }
        public Int32 QuotaQtyAftBooking
        {
            get { return quotaQtyAftBooking; }
            set { quotaQtyAftBooking = value; }
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
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime BookingDatetime
        {
            get { return bookingDatetime; }
            set { bookingDatetime = value; }
        }
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
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
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        //Priyanka [25-06-2018] : Added for adding director remark while  new booking.
        public String DirectorRemark
        {
            get { return directorRemark; }
            set { directorRemark = value; }
        }
        public String CnfName
        {
            get { return cnfName; }
            set { cnfName = value; }
        }

        public String DealerName
        {
            get { return dealerName; }
            set { dealerName = value; }
        }

        public String Status
        {
            get { return status; }
            set { status = value; }
        }


        /// <summary>
        /// Sanjay [2017-02-17] This is pending qty for loading
        /// </summary>
        public Double PendingQty
        {
            get { return pendingQty; }
            set { pendingQty = value; }
        }

        public Double LoadingQty
        {
            get { return loadingQty; }
            set { loadingQty = value; }
        }

        public Int32 IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }
        /// <summary>
        /// Sanjay [2017-02-11] To Get data In Post action
        /// </summary>
        public List<TblBookingDelAddrTO> DeliveryAddressLst
        {
            get
            {
                return deliveryAddressLst;
            }

            set
            {
                deliveryAddressLst = value;
            }
        }

        /// <summary>
        /// Sanjay [2017-02-11] To Get data In Post action
        /// </summary>
        public List<TblBookingExtTO> OrderDetailsLst
        {
            get
            {
                return orderDetailsLst;
            }

            set
            {
                orderDetailsLst = value;
            }
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

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }


        public String DealerCat
        {
             get
            {
                return dealerCat;
            }

            set
            {
                dealerCat = value;
            }
        }

        public String ColorCode
        {
            get
            {
                return colorCode;
            }

            set
            {
                colorCode = value;
            }
        }

        public String StatusDateStr
        {
            get { return statusDate.ToString(Constants.DefaultDateFormat); }
        }
        public String UpdatedOnStr
        {
            get { return updatedOn.ToString(Constants.DefaultDateFormat); }
        }

        public string AuthReasons
        {
            get
            {
                return authReasons;
            }

            set
            {
                authReasons = value;
            }
        }

        public int CdStructureId
        {
            get
            {
                return cdStructureId;
            }

            set
            {
                cdStructureId = value;
            }
        }

        /// <summary>
        /// Sanjay [2017-04-21] added after alpha release
        /// after discussion with Customer (Nitin Kabra)
        /// New Requirement
        /// </summary>
        public int ParityId
        {
            get
            {
                return parityId;
            }

            set
            {
                parityId = value;
            }
        }

        public double OrcAmt { get => orcAmt; set => orcAmt = value; }
        public string OrcMeasure { get => orcMeasure; set => orcMeasure = value; }
        public string BillingName { get => billingName; set => billingName = value; }
        public string PoNo { get => poNo; set => poNo = value; }
        public string StatusRemark { get => statusRemark; set => statusRemark = value; }

        public Int32 TransporterScopeYn { get => transporterScopeYn; set => transporterScopeYn = value; }

        public double OverdueAmt { get => overdueAmt; set => overdueAmt = value; }
        public string VehicleNo { get => vehicleNo; set => vehicleNo = value; }
        public int BrandId { get => brandId; set => brandId = value; }
        public string BrandName { get => brandName; set => brandName = value; }

        public double FreightAmt { get => freightAmt; set => freightAmt = value; }

        public string PoFileBase64 { get => pOFileBase64; set => pOFileBase64 = value; }

        public double EnquiryAmt { get => enquiryAmt; set => enquiryAmt = value; }

        public string ProjectName { get => projectName; set => projectName = value; }

        /// <summary>
        /// Vijaymala [2017-14-12] To Get data In Post action
        /// </summary>
        public List<TblBookingScheduleTO> BookingScheduleTOLst
        {
            get
            {
                return bookingScheduleTOLst;
            }

            set
            {
                bookingScheduleTOLst = value;
            }
        }
        //[27-02-2018]Vijaymala Added
        public DateTime PoDate { get => poDate; set => poDate = value; }

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
                else
                    poDate = new DateTime();
            }
        }

        public string ORCPersonName
        {
            get { return orcPersonName; }
            set { orcPersonName = value; }
        }

        //Priyanka [08-06-2018] : Added For SHIVANGI.
        public Int32 IsOverdueExist
        {
            get { return isOverdueExist; }
            set { isOverdueExist = value; }
        }

        public string CreatedByName { get => createdByName; set => createdByName = value; }
        public string UpdatedByName { get => updatedByName; set => updatedByName = value; }

        //Priyanka [21-06-2018] : Added for SHIVANGI.
        public Double SizesQty
        {
            get { return sizesQty; }
            set { sizesQty = value; }
        }

        public int IsOrgOverDue { get => isOrgOverDue; set => isOrgOverDue = value; }
        public int StatusBy { get => statusBy; set => statusBy = value; }
        public int TranActionTypeId { get => tranActionTypeId; set => tranActionTypeId = value; }
        public int BookingType { get => bookingType; set => bookingType = value; }
        public int IsSez { get => isSez; set => isSez = value; }
        public List<TblPaymentTermOptionRelationTO> PaymentTermOptionRelationTOLst { get => paymentTermOptionRelationTOLst; set => paymentTermOptionRelationTOLst = value; }
        public double UomQty { get => uomQty; set => uomQty = value; }
        public double PendingUomQty { get => pendingUomQty; set => pendingUomQty = value; }
        public int IsInUom { get => isInUom; set => isInUom = value; }
        public int IsItemized { get => isItemized; set => isItemized = value; }
        public int StateId { get => stateId; set => stateId = value; }
        public List<TblBookingExtTO> OrderDetailsLstForItemWise { get => orderDetailsLstForItemWise; set => orderDetailsLstForItemWise = value; }
        public double CreditLimit { get => creditLimit; set => creditLimit = value; }
        public int OtherNewBooking { get => otherNewBooking; set => otherNewBooking = value; }


        #endregion

        #region Methods

        public TblBookingBeyondQuotaTO GetBookingBeyondQuotaTO()
        {
            TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO = new Models.TblBookingBeyondQuotaTO();
            tblBookingBeyondQuotaTO.BookingId = this.idBooking;
            tblBookingBeyondQuotaTO.DeliveryPeriod = this.deliveryDays;
            tblBookingBeyondQuotaTO.Quantity = this.bookingQty;
            tblBookingBeyondQuotaTO.CdStructure = this.cdStructure;
            tblBookingBeyondQuotaTO.CdStructureId = this.cdStructureId;
            tblBookingBeyondQuotaTO.OrcAmt = this.orcAmt;
            tblBookingBeyondQuotaTO.Quantity = this.bookingQty;
            tblBookingBeyondQuotaTO.Rate = this.bookingRate;
            tblBookingBeyondQuotaTO.StatusId = this.statusId;
            tblBookingBeyondQuotaTO.StatusDate = this.statusDate;
            return tblBookingBeyondQuotaTO;
        }
        #endregion
    }

    //Aniket [20-3-2019]
    public class TblBookingPendingRptTO
    {
        #region Declarations
        int dealerId;
        string dealerName;
        int idBooking;
        string totalQty;
        double totalBookedQty;
        string cnfName;
        double pendingQty;
        List<Dictionary<int, double>> bookingDictionaryList = new List<Dictionary<int, double>>();
        List<Dictionary<int, double>> loadingDictionaryList = new List<Dictionary<int, double>>();
        List<Dictionary<string, string>> finalDictionaryList = new List<Dictionary<string, string>>();


        #endregion
        #region GetSet
        public int DealerId { get => dealerId; set => dealerId = value; }
        public string DealerName { get => dealerName; set => dealerName = value; }
        public List<Dictionary<int, double>> BookingDictionaryList { get => bookingDictionaryList; set => bookingDictionaryList = value; }
        public int IdBooking { get => idBooking; set => idBooking = value; }
        public List<Dictionary<int, double>> LoadingDictionaryList { get => loadingDictionaryList; set => loadingDictionaryList = value; }
        public List<Dictionary<string, string>> FinalDictionaryList { get => finalDictionaryList; set => finalDictionaryList = value; }
        public string TotalQty { get => totalQty; set => totalQty = value; }
        public double TotalBookedQty { get => totalBookedQty; set => totalBookedQty = value; }
        public string CnfName { get => cnfName; set => cnfName = value; }
        public double PendingQty { get => pendingQty; set => pendingQty = value; }

        #endregion
    }
}
