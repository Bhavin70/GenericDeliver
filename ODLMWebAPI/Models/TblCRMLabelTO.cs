using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblCRMLabelTO
    {
        #region Declarations
        Int32 idLabel;
        Int32 lagId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 pageId;
        DateTime createdOn;
        DateTime updatedOn;
        String keyLabel;
        String valueLabel;
        #endregion

        #region Constructor
        public TblCRMLabelTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLabel
        {
            get { return idLabel; }
            set { idLabel = value; }
        }
        public Int32 LagId
        {
            get { return lagId; }
            set { lagId = value; }
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
        public Int32 PageId
        {
            get { return pageId; }
            set { pageId = value; }
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
        public String KeyLabel
        {
            get { return keyLabel; }
            set { keyLabel = value; }
        }
        public String ValueLabel
        {
            get { return valueLabel; }
            set { valueLabel = value; }
        }
        #endregion
    }
}
