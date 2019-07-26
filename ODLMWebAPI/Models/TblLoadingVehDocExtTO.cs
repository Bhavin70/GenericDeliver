using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblLoadingVehDocExtTO
    {
        #region Declarations
        Int32 idLoadingVehDocExt;
        Int32 vehDocTypeId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String vehDocNo;
        Int32 loadingId;
        String vehDocTypeName;
        Int32 sequenceNo;
        Int32 isAvailable;

        #endregion

        #region Constructor
        public TblLoadingVehDocExtTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadingVehDocExt
        {
            get { return idLoadingVehDocExt; }
            set { idLoadingVehDocExt = value; }
        }
        public Int32 VehDocTypeId
        {
            get { return vehDocTypeId; }
            set { vehDocTypeId = value; }
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
        public String VehDocNo
        {
            get { return vehDocNo; }
            set { vehDocNo = value; }
        }

        public int LoadingId { get => loadingId; set => loadingId = value; }
        public string VehDocTypeName { get => vehDocTypeName; set => vehDocTypeName = value; }
        public int SequenceNo { get => sequenceNo; set => sequenceNo = value; }
        public int IsAvailable { get => isAvailable; set => isAvailable = value; }
        #endregion
    }
}
