using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;


namespace ODLMWebAPI.Models
{
    public class TblLoadingSlipRemovedItemsTO
    {
        #region Declarations
        Int32 idLoadingSlipRemovedItems;
        Int32 bookingId;
        Int32 loadingLayerid;
        Int32 materialId;
        Int32 prodCatId;
        Int32 prodSpecId;
        Int32 parityDtlId;
        Int32 brandId;
        Int32 updatedBy;
        DateTime updatedOn;
        Double ratePerMT;
        Int32 loadingSlipExtId;
        Int32 createdBy;
        Double loadingQty;
        Int32 prodItemId;    //[05-09-2018]Vijaymala added to set other item id
        #endregion

        #region Constructor
        public TblLoadingSlipRemovedItemsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadingSlipRemovedItems
        {
            get { return idLoadingSlipRemovedItems; }
            set { idLoadingSlipRemovedItems = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 LoadingLayerid
        {
            get { return loadingLayerid; }
            set { loadingLayerid = value; }
        }
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public Int32 ProdCatId
        {
            get { return prodCatId; }
            set { prodCatId = value; }
        }
        public Int32 ProdSpecId
        {
            get { return prodSpecId; }
            set { prodSpecId = value; }
        }
        public Int32 ParityDtlId
        {
            get { return parityDtlId; }
            set { parityDtlId = value; }
        }
        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
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
        public Double RatePerMT
        {
            get { return ratePerMT; }
            set { ratePerMT = value; }
        }
        public Int32 LoadingSlipExtId
        {
            get { return loadingSlipExtId; }
            set { loadingSlipExtId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public Double LoadingQty
        {
            get { return loadingQty; }
            set { loadingQty = value; }
        }
        //[05-09-2018]Vijaymala added to set other item id
        public int ProdItemId {
            get { return prodItemId; }
            set { prodItemId = value; }
        }

        #endregion
    }
}
