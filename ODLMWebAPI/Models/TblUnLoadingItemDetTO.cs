using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class TblUnLoadingItemDetTO
    {
        #region Declarations
        Int32 idUnloadingItemDet;
        Int32 unLoadingId;
        Int32 productCatId;
        Int32 weightMeasurUnitId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double unLoadingQty;
        Double weightedQty;
        String productCatName;

        int compartmentId;
        int materialId;
        int productSpecId;
        int brandId;
        int productId;

        double grossWT;
        double tareWt;
        int weightMeasureId;
        double loadedWeight;
        double unLoadedWT;
        Boolean isLastItem;
        #endregion

        #region Constructor
        public TblUnLoadingItemDetTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUnloadingItemDet
        {
            get { return idUnloadingItemDet; }
            set { idUnloadingItemDet = value; }
        }
        public Int32 UnLoadingId
        {
            get { return unLoadingId; }
            set { unLoadingId = value; }
        }
        public Int32 ProductCatId
        {
            get { return productCatId; }
            set { productCatId = value; }
        }
        public Int32 WeightMeasurUnitId
        {
            get { return weightMeasurUnitId; }
            set { weightMeasurUnitId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Double UnLoadingQty
        {
            get { return unLoadingQty; }
            set { unLoadingQty = value; }
        }
        public Double WeightedQty
        {
            get { return weightedQty; }
            set { weightedQty = value; }
        }

        public String ProductCatName
        {
            get { return productCatName; }
            set { productCatName = value; }
        }

        public int CompartmentId
        {
            get { return compartmentId; }
            set { compartmentId = value; }
        }

        public double GrossWT
        {
            get { return grossWT; }
            set { grossWT = value; }
        }

        public double TareWt
        {
            get { return tareWt; }
            set { tareWt = value; }
        }


        public int WeightMeasureId
        {
            get { return weightMeasureId; }
            set { weightMeasureId = value; }
        }


        public double LoadedWeight
        {
            get { return loadedWeight; }
            set { loadedWeight = value; }
        }
        public double UnLoadedWT
        {
            get { return unLoadedWT; }
            set { unLoadedWT = value; }
        }

        public Boolean IsLastItem
        {
            get { return isLastItem; }
            set { isLastItem = value; }
        }

        public int MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }

        public int ProductSpecId
        {
            get { return productSpecId; }
            set { productSpecId = value; }
        }

        public int BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        #endregion
    }
}
