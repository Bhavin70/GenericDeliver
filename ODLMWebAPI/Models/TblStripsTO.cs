using System;
using System.Drawing;

namespace ODLMWebAPI.Models
{
    public class TblStripsTO
    {
        #region Declarations
        Int32 idStrips;
        Int32 grade;
        Int32 idSize;
        string size;
        Int32 idThickness;
        decimal thickness;
        Int32 isActive;
        DateTime createdOn;

        #endregion

        #region Constructor
        public TblStripsTO()
        {

        }
        #endregion

        #region GetSet
        public Int32 IdStrips
        {
            get { return idStrips; }
            set { idStrips = value; }
        }
        public Int32 Grade
        {
            get { return grade; }
            set { grade = value; }
        }
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
    public class TblStripsGradeDropDownTo
    {
        Int32 idStrip;
        Int32 grade;
        public Int32 IdStrip
        {
            get { return idStrip; }
            set { idStrip = value; }
        }
        public Int32 Grade
        {
            get { return grade; }
            set { grade = value; }
        }
    }

}
