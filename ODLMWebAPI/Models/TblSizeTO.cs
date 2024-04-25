using System;

namespace ODLMWebAPI.Models
{
    public class TblSizeTO
    {
        #region Declarations

        Int32 idSize;
        string size;
        Int32 isActive;
        DateTime createdOn;
        Int32 idInch;
        #endregion

        #region Constructor
        public TblSizeTO()
        {

        }
        #endregion

        #region GetSet
        public Int32 IdSize
        {
            get { return idSize; }
            set { idSize = value; }
        }
        public string Size
        {
            get { return size; }
            set { size = value; }
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
        public Int32 IdInch
        {
            get { return idInch; }
            set { idInch = value; }
        }
        
        #endregion
    }
    public class TblSizeDropDownTO
    {
        #region Declarations

        Int32 idSize;
        decimal size;
        Int32 isActive;
        DateTime createdOn;
        Int32 idInch;
        Int32 inch;

        #endregion

        #region GetSet
        public Int32 IdSize
        {
            get { return idSize; }
            set { idSize = value; }
        }
        public decimal Size
        {
            get { return size; }
            set { size = value; }
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
        public Int32 IdInch
        {
            get { return idInch; }
            set { idInch = value; }
        }
        public Int32 Inch
        {
            get { return inch; }
            set { inch = value; }
        }
        #endregion
    }
}

