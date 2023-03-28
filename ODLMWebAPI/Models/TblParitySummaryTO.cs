using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblParitySummaryTO
    {
        #region Declarations
        Int32 idParity;
        Int32 createdBy;
        Int32 isActive;
        DateTime createdOn;
        String remark;

        List<TblParityDetailsTO> parityDetailList;
        Int32 stateId;
        String stateName;
        Double baseValCorAmt;
        Double freightAmt;
        Double expenseAmt;
        Double otherAmt;
        Int32 brandId;
        #endregion

        #region Constructor
        public TblParitySummaryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdParity
        {
            get { return idParity; }
            set { idParity = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public List<TblParityDetailsTO> ParityDetailList
        {
            get
            {
                return parityDetailList;
            }

            set
            {
                parityDetailList = value;
            }
        }

        public int StateId { get => stateId; set => stateId = value; }
        public string StateName { get => stateName; set => stateName = value; }
        public double BaseValCorAmt { get => baseValCorAmt; set => baseValCorAmt = value; }
        public double FreightAmt { get => freightAmt; set => freightAmt = value; }
        public double ExpenseAmt { get => expenseAmt; set => expenseAmt = value; }
        public double OtherAmt { get => otherAmt; set => otherAmt = value; }

        /// <summary>
        /// [21-11-2017]Vijaymala :Added to modify parity changes as per brand
        /// </summary>
        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }
        #endregion
    }
    public class SizeTestingDtlTO
    {
        #region Declarations
        DateTime createOn;
        int idTestDtl;
        int materialId;
        int createdBy;
        int isActive;
        DateTime testingDate;
        decimal chemC;
        decimal chemS;
        decimal chemP;
        decimal mechProof;
        decimal mechTen;
        decimal mechElon;
        decimal mechTEle;
        string castNo;
        string grade;
        #endregion

        #region Constructor
        public SizeTestingDtlTO()
        {
        }

        #endregion

        #region GetSet
        public DateTime CreateOn
        {
            get { return createOn; }
            set { createOn = value; }
        }
        public int IdTestDtl
        {
            get { return idTestDtl; }
            set { idTestDtl = value; }
        }
        public int MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public int CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public int IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime TestingDate
        {
            get { return testingDate; }
            set { testingDate = value; }
        }
        public decimal ChemC
        {
            get { return chemC; }
            set { chemC = value; }
        }
        public decimal ChemS
        {
            get { return chemS; }
            set { chemS = value; }
        }
        public decimal ChemP
        {
            get { return chemP; }
            set { chemP = value; }
        }
        public decimal MechProof
        {
            get { return mechProof; }
            set { mechProof = value; }
        }
        public decimal MechTen
        {
            get { return mechTen; }
            set { mechTen = value; }
        }
        public decimal MechElon
        {
            get { return mechElon; }
            set { mechElon = value; }
        }
        public decimal MechTEle
        {
            get { return mechTEle; }
            set { mechTEle = value; }
        }
        public string CastNo
        {
            get { return castNo; }
            set { castNo = value; }
        }
        public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        #endregion
    }
}
