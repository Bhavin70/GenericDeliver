using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblLoadingSlipExtTO
    {
        #region Declarations
        Int32 idLoadingSlipExt;
        Int32 bookingId;
        Int32 loadingSlipId;
        Int32 loadingLayerid;
        Int32 materialId;
        Int32 bookingExtId;
        Double loadingQty;
        String materialDesc;
        String loadingLayerDesc;
        Int32 prodCatId;
        Int32 prodSpecId;
        Double quotaBforeLoading;
        Double quotaAfterLoading;
        Int32 loadingQuotaId;
        String prodCatDesc;
        String prodSpecDesc;
        Double bundles;
        Int32 parityDtlId;
        Double ratePerMT;
        Object tag;
        String rateCalcDesc;
        Double loadedWeight;
        Double parityDetails;
        Double loadedBundles;
        Double calcTareWeight;
        Int32 weightMeasureId;
        Int32 updatedBy;
        DateTime updatedOn;
        String prodItemDesc;
        Int32 isAllowNewWeighingMachine;
        Int32 cdStructureId;
        Double cdStructure;
        Int32 prodItemId;
        Double taxableRateMT;
        Double freExpOtherAmt;
        Double cdApplicableAmt;
        Int32 brandId;
        String brandDesc;
        Int32 isWeighingAllow;
        Int32 scheduleId;
        String scheduleDateStr ;
        Double balanceQty;
        Int32 isStockRequire;

        int compartmentId;
        string compartmentName;

        Boolean isAllowPrevTareWt;

        Double mstLoadedBundles;  //Priyanka [28-05-2018]       
                                  // String displayLoadedBundles; //Priyanka [04-06-2018]
        //[05-09-2018]Vijaymala added to get other item name
        String itemName;
        String displayName;
        Int32 schedulelayerId;
        Int32 sequenceNo;
        double conversionFactor;
        double avgBundleWt;
        //Aniket [30-7-2019] added for IOT
        Int32 modbusRefId;
        Int32 weighingSequenceNumber;
        string bookingDisplayNo;        
        Double averageRate;
        String displayLoadingQty;
        String displayLoadedQty;
        String displayAverageRate;
        #endregion

        #region Constructor
        public TblLoadingSlipExtTO()
        {
        }

        public TblLoadingSlipExtTO(TblBookingExtTO tblBookingExtTO)
        {
            this.ProdCatId = tblBookingExtTO.ProdCatId;
            this.ProdCatDesc = tblBookingExtTO.ProdCatDesc;
            this.ProdSpecId = tblBookingExtTO.ProdSpecId;
            this.ProdSpecDesc = tblBookingExtTO.ProdSpecDesc;
            this.BrandId = tblBookingExtTO.BrandId;
            this.brandDesc = tblBookingExtTO.BrandDesc;
            this.MaterialId = tblBookingExtTO.MaterialId;
            this.MaterialDesc= tblBookingExtTO.MaterialSubType;
            this.scheduleDateStr = tblBookingExtTO.ScheduleDate.ToString("dd/MMM/yyyy");

            //this.BookingExtId = tblBookingExtTO.IdBookingExt;  //Saket [2017-12-20] Cunsumption will be based on size wise
            this.BalanceQty = tblBookingExtTO.BalanceQty;
            this.LoadingQty = tblBookingExtTO.BalanceQty;
            this.ProdItemId = tblBookingExtTO.ProdItemId;
            this.DisplayName = tblBookingExtTO.DisplayName;
        }


        #endregion

        #region GetSet
        public Int32 IdLoadingSlipExt
        {
            get { return idLoadingSlipExt; }
            set { idLoadingSlipExt = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 LoadingSlipId
        {
            get { return loadingSlipId; }
            set { loadingSlipId = value; }
        }
        public Int32 LoadingLayerid
        {
            get { return loadingLayerid; }
            set { loadingLayerid = value; }
        }
        public String ProdItemDesc
        {
            get { return prodItemDesc; }
            set { prodItemDesc = value; }
        }
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public Int32 BookingExtId
        {
            get { return bookingExtId; }
            set { bookingExtId = value; }
        }
        public Double LoadingQty
        {
            get { return loadingQty; }
            set { loadingQty = value; }
        }
        public String MaterialDesc
        {
            get { return materialDesc; }
            set { materialDesc = value; }
        }

        public String LoadingLayerDesc
        {
            get { return loadingLayerDesc; }
            set { loadingLayerDesc = value; }
        }
        public Constants.LoadingLayerE LoadingLayerE
        {
            get
            {
                Constants.LoadingLayerE loadingLayerE = Constants.LoadingLayerE.BOTTOM;
                if (Enum.IsDefined(typeof(Constants.LoadingLayerE), loadingLayerid))
                {
                    loadingLayerE = (Constants.LoadingLayerE)loadingLayerid;
                }
                return loadingLayerE;

            }
            set
            {
                loadingLayerid = (int)value;
            }
        }

        public int ProdCatId
        {
            get
            {
                return prodCatId;
            }

            set
            {
                prodCatId = value;
            }
        }

        public int ProdSpecId
        {
            get
            {
                return prodSpecId;
            }

            set
            {
                prodSpecId = value;
            }
        }

        public double QuotaBforeLoading
        {
            get
            {
                return quotaBforeLoading;
            }

            set
            {
                quotaBforeLoading = value;
            }
        }

        public double QuotaAfterLoading
        {
            get
            {
                return quotaAfterLoading;
            }

            set
            {
                quotaAfterLoading = value;
            }
        }

        public int LoadingQuotaId
        {
            get
            {
                return loadingQuotaId;
            }

            set
            {
                loadingQuotaId = value;
            }
        }

        public String ProdCatDesc
        {
            get { return prodCatDesc; }
            set { prodCatDesc = value; }
        }
        public String ProdSpecDesc
        {
            get { return prodSpecDesc; }
            set { prodSpecDesc = value; }
        }

        public Double Bundles
        {
            get { return bundles; }
            set { bundles = value; }
        }
        public Int32 IsAllowWeighingMachine
        {
            get { return isAllowNewWeighingMachine; }
            set { isAllowNewWeighingMachine = value; }
        }

        public int ParityDtlId
        {
            get
            {
                return parityDtlId;
            }

            set
            {
                parityDtlId = value;
            }
        }

        public double RatePerMT
        {
            get
            {
                return ratePerMT;
            }

            set
            {
                ratePerMT = value;
            }
        }

        /// <summary>
        /// Saket [2017-12-19] Added to show total amount on loading slip confirmations 
        /// RatePerMT * LoadingQty;
        /// </summary>
        public double TotalRateMT
        {
            get
            {
                return RatePerMT * LoadingQty;
            }
        }

        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }


        public object Tag { get => tag; set => tag = value; }

        public String LayerShortDesc
        {
            get
            {
                if (LoadingLayerE == Constants.LoadingLayerE.BOTTOM)
                    return "B";
                else if (LoadingLayerE == Constants.LoadingLayerE.MIDDLE1)
                    return "M1";
                else if (LoadingLayerE == Constants.LoadingLayerE.MIDDLE2)
                    return "M2";
                else if (LoadingLayerE == Constants.LoadingLayerE.MIDDLE3)
                    return "M3";
                else return "T";
            }
        }

        public string RateCalcDesc { get => rateCalcDesc; set => rateCalcDesc = value; }

        public Double LoadedWeight
        {
            get { return loadedWeight; }
            set { loadedWeight = value; }
        }
        public Double ParityDetails
        {
            get { return parityDetails; }
            set { parityDetails = value; }
        }
        public Double LoadedBundles
        {
            get { return loadedBundles; }
            set { loadedBundles = value; }
        }

        public Double CalcTareWeight
        {
            get { return calcTareWeight; }
            set { calcTareWeight = value; }
        }

        public Int32 WeightMeasureId
        {
            get { return weightMeasureId; }
            set { weightMeasureId = value; }
        }

        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Int32 CdStructureId
        {
            get { return cdStructureId; }
            set { cdStructureId = value; }
        }
        public Double CdStructure
        {
            get { return cdStructure; }
            set { cdStructure = value; }
        }
        public Double TaxableRateMT
        {
            get { return taxableRateMT; }
            set { taxableRateMT = value; }
        }
        public Double FreExpOtherAmt
        {
            get { return freExpOtherAmt; }
            set { freExpOtherAmt = value; }
        }

        public Double CdApplicableAmt
        {
            get { return cdApplicableAmt; }
            set { cdApplicableAmt = value; }
        }

        public int BrandId { get => brandId; set => brandId = value; }
        public int IsWeighingAllow { get => isWeighingAllow; set => isWeighingAllow = value; }
        
        public String BrandDesc { get => brandDesc; set => brandDesc = value; }
        public int ScheduleId { get => scheduleId; set => scheduleId = value; }
        public string ScheduleDateStr { get => scheduleDateStr; set => scheduleDateStr = value; }
        public double BalanceQty { get => balanceQty; set => balanceQty = value; }
        public int IsStockRequire { get => isStockRequire; set => isStockRequire = value; }
        public bool IsAllowPrevTareWt { get => isAllowPrevTareWt; set => isAllowPrevTareWt = value; }

        public int CompartmentId
        {
            get { return compartmentId; }
            set { compartmentId = value; }
        }

        public string CompartmentName
        {
            get { return compartmentName; }
            set { compartmentName = value; }
        }

        //Priyanka [28-05-2018] : Added to show master bundles  in loading slip
        public Double MstLoadedBundles
        {
            get { return mstLoadedBundles; }
            set { mstLoadedBundles = value; }
        }

        public string DisplayName { get => displayName; set => displayName = value; }

        public string ItemName { get => itemName; set => itemName = value; }
        public int SchedulelayerId { get => schedulelayerId; set => schedulelayerId = value; }
        public int SequenceNo { get => sequenceNo; set => sequenceNo = value; }
        public double ConversionFactor { get => conversionFactor; set => conversionFactor = value; }
        public double AvgBundleWt { get => avgBundleWt; set => avgBundleWt = value; }
        public int ModbusRefId { get => modbusRefId; set => modbusRefId = value; }
        public int WeighingSequenceNumber { get => weighingSequenceNumber; set => weighingSequenceNumber = value; }
        public string BookingDisplayNo { get => bookingDisplayNo; set => bookingDisplayNo = value; }
        public Double AverageRate { get => averageRate; set => averageRate = value; }
        public String DisplayLoadingQty { get => displayLoadingQty; set => displayLoadingQty = value; }
        public String DisplayLoadedQty { get => displayLoadedQty; set => displayLoadedQty = value; }
        public String DisplayAverageRate { get => displayAverageRate; set => displayAverageRate = value; }
        //    public String DisplayLoadedBundles
        //   {
        //       get { return displayLoadedBundles; }
        //       set { displayLoadedBundles = value; }
        //  }
        #endregion

        #region Method

        public TblLoadingSlipRemovedItemsTO GetTblLoadingSlipRemovedItemsTO()
        {
            TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO = new Models.TblLoadingSlipRemovedItemsTO();
            tblLoadingSlipRemovedItemsTO.BookingId = this.bookingId;
            tblLoadingSlipRemovedItemsTO.BrandId = this.brandId;
            tblLoadingSlipRemovedItemsTO.LoadingLayerid = this.loadingLayerid;
            tblLoadingSlipRemovedItemsTO.MaterialId = this.materialId;
            tblLoadingSlipRemovedItemsTO.ParityDtlId = this.parityDtlId;
            tblLoadingSlipRemovedItemsTO.ProdCatId = this.prodCatId;
            tblLoadingSlipRemovedItemsTO.ProdSpecId = this.prodSpecId;
            tblLoadingSlipRemovedItemsTO.RatePerMT = this.ratePerMT;
            tblLoadingSlipRemovedItemsTO.LoadingSlipExtId = this.idLoadingSlipExt;
            tblLoadingSlipRemovedItemsTO.CreatedBy = this.updatedBy;
            tblLoadingSlipRemovedItemsTO.LoadingQty = this.loadingQty;
            tblLoadingSlipRemovedItemsTO.ProdItemId = this.ProdItemId;
            return tblLoadingSlipRemovedItemsTO;
        }
        #endregion

        public TblLoadingSlipExtTO DeepCopy()
        {
            TblLoadingSlipExtTO other = (TblLoadingSlipExtTO)this.MemberwiseClone();
            return other;
        }
    }
}
