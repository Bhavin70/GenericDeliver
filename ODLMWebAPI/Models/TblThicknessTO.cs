using System;

namespace ODLMWebAPI.Models
{
    public class TblThicknessTO
    {
        #region Declarations

        Int32 idThickness;
        decimal thickness;
        Int32 isActive;
        DateTime createdOn;

        #endregion

        #region Constructor
        public TblThicknessTO()
        {

        }
        #endregion

        #region GetSet
        public Int32 IdThickness
        {
            get { return idThickness; }
            set { idThickness = value; }
        }
        public decimal Thickness
        {
            get { return thickness; }
            set { thickness = value; }
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

        #endregion
    }
    public class TblThicknessDropDownTO
    {
        #region Declarations

        Int32 idThickness;
        decimal thickness;

        #endregion

        #region GetSet
        public Int32 IdThickness
        {
            get { return idThickness; }
            set { idThickness = value; }
        }
        public decimal Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }
        #endregion
    }
}

