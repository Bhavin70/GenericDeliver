using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class DimVehDocTypeTO
    {
        #region Declarations
        Int32 idVehDocType;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String vehDocTypeName;
        String vehDocTypeDesc;
        Int32 sequenceNo;
        #endregion

        #region Constructor
        public DimVehDocTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVehDocType
        {
            get { return idVehDocType; }
            set { idVehDocType = value; }
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
        public String VehDocTypeName
        {
            get { return vehDocTypeName; }
            set { vehDocTypeName = value; }
        }
        public String VehDocTypeDesc
        {
            get { return vehDocTypeDesc; }
            set { vehDocTypeDesc = value; }
        }

        public int SequenceNo { get => sequenceNo; set => sequenceNo = value; }
        #endregion
    }
}
