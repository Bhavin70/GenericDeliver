using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblUserBrandTO
    {
        #region Declarations
        Int32 idUserBrand;
        Int32 userId;
        Int32 brandId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 cnfOrgId;
        #endregion

        #region Constructor
        public TblUserBrandTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUserBrand
        {
            get { return idUserBrand; }
            set { idUserBrand = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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

        public int CnfOrgId { get => cnfOrgId; set => cnfOrgId = value; }
        #endregion
    }
}
