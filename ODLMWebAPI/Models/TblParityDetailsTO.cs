using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblParityDetailsTO
    {
        #region Declarations
        Int32 idParityDtl;
        Int32 parityId;
        Int32 materialId;
        Int32 createdBy;
        DateTime createdOn;
        Double parityAmt;
        Double nonConfParityAmt;
        String remark;
        Int32 prodCatId;
        String prodCatDesc;
        String materialDesc;
        String brandDesc;
        Int32 prodSpecId;
        String prodSpecDesc;
        Int32 brandId;
        Int32 stateId;
        Double baseValCorAmt;
        Double freightAmt;
        Double expenseAmt;
        Double otherAmt;
        Int32 prodItemId;
        Int32 isActive;
        String stateName;
        Int32 isForUpdate;
        String displayName;
        Int32 currencyId;
        String itemName;
        Int32 districtId; //Dhananjay [2020-11-25] to store District
        Int32 talukaId; //Dhananjay [2020-11-25] to store Taluka
        #endregion

        #region Constructor
        public TblParityDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdParityDtl
        {
            get { return idParityDtl; }
            set { idParityDtl = value; }
        }
        public Int32 ParityId
        {
            get { return parityId; }
            set { parityId = value; }
        }
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
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
        public Double ParityAmt
        {
            get { return parityAmt; }
            set { parityAmt = value; }
        }
        public Double NonConfParityAmt
        {
            get { return nonConfParityAmt; }
            set { nonConfParityAmt = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
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

        public string ProdCatDesc
        {
            get
            {
                return prodCatDesc;
            }

            set
            {
                prodCatDesc = value;
            }
        }

        public string MaterialDesc
        {
            get
            {
                return materialDesc;
            }

            set
            {
                materialDesc = value;
            }
        }

        public String CreatedOnStr
        {
            get
            {
                if (createdOn == DateTime.MinValue)
                    return "-";
                else
                    return createdOn.ToString(Constants.DefaultDateFormat);
            }
        }

        public int ProdSpecId { get => prodSpecId; set => prodSpecId = value; }
        public string ProdSpecDesc { get => prodSpecDesc; set => prodSpecDesc = value; }
        public int BrandId { get => brandId; set => brandId = value; }
        public int StateId { get => stateId; set => stateId = value; }
        public double FreightAmt { get => freightAmt; set => freightAmt = value; }
        public double ExpenseAmt { get => expenseAmt; set => expenseAmt = value; }
        public double OtherAmt { get => otherAmt; set => otherAmt = value; }
        public int ProdItemId { get => prodItemId; set => prodItemId = value; }
        public double BaseValCorAmt { get => baseValCorAmt; set => baseValCorAmt = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public string StateName { get => stateName; set => stateName = value; }
        public int IsForUpdate { get => isForUpdate; set => isForUpdate = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string BrandDesc { get => brandDesc; set => brandDesc = value; }
        public int CurrencyId { get => currencyId; set => currencyId = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public int DistrictId { get => districtId; set => districtId = value; } //Dhananjay [2020-11-25] to store District
        public int TalukaId { get => talukaId; set => talukaId = value; } //Dhananjay [2020-11-25] to store Taluka
        #endregion
    }
}
