using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblMaterialTO
    {
        #region Declarations
        Int32 idMaterial;
        Int32 mateCompOrgId;
        Int32 mateSubCompOrgId;
        Int32 materialTypeId;
        Int32 createdBy;
        DateTime createdOn;
        String materialSubType;
        String userDisplayName;
        Int32 updatedBy;
        DateTime updatedOn;
        Int32 deactivatedBy;
        DateTime deactivatedOn;
        Int32 isActive;
        Int32 materialId;
        Int32 idInvoiceItem;
        #endregion

        #region Constructor
        public TblMaterialTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdMaterial
        {
            get { return idMaterial; }
            set { idMaterial = value; }
        }
        public Int32 MateCompOrgId
        {
            get { return mateCompOrgId; }
            set { mateCompOrgId = value; }
        }
        public Int32 MateSubCompOrgId
        {
            get { return mateSubCompOrgId; }
            set { mateSubCompOrgId = value; }
        }
        public Int32 MaterialTypeId
        {
            get { return materialTypeId; }
            set { materialTypeId = value; }
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
        public int MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public Int32 IdInvoiceItem
        {
            get { return idInvoiceItem; }
            set { idInvoiceItem = value; }
        }
        public String MaterialSubType
        {
            get { return materialSubType; }
            set { materialSubType = value; }
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
        public Int32 DeactivatedBy
        {
            get { return deactivatedBy; }
            set { deactivatedBy = value; }
        }
        public DateTime DeactivatedOn
        {
            get { return deactivatedOn; }
            set { deactivatedOn = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public string UserDisplayName { get => userDisplayName; set => userDisplayName = value; }

        public decimal ChemCE
        {
            get; set;
        }
        public decimal ChemT
        {
            get; set;
        }
        public DateTime CreateOn
        {
            get; set;
        }
        public int IdTestDtl
        {
            get; set; 
        }
       
        public DateTime TestingDate
        {
            get; set;
        }
        public decimal ChemC
        {
            get; set;
        }
        public decimal ChemS
        {
            get; set;
        }
        public decimal ChemP
        {
            get; set;
        }
        public decimal MechProof
        {
            get; set;
        }
        public decimal MechTen
        {
            get; set;
        }
        public decimal MechElon
        {
            get; set;
        }
        public decimal MechTEle
        {
            get; set;
        }
        public string CastNo
        {
            get; set;
        }
        public string Grade
        {
            get; set;
        }
        #endregion
    }
}
