using System;
using System.Drawing;

namespace ODLMWebAPI.Models
{
    public class TblStripsTO
    {
        #region Declarations
        Int32 idStrip;
        Int32 grade;
        Int32 width;
        Int32 isActive;
        DateTime createdOn;
        Int32 idPipesStripCommon;
        Int32 categoryType;
        Int32 size;
        Int32 thickness;

        #endregion

        #region Constructor
        public TblStripsTO()
        {

        }
        #endregion

        #region GetSet
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
        public Int32 Width
        {
            get { return width; }
            set { width = value; }
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
        public Int32 IdPipesStripCommon
        {
            get { return idPipesStripCommon; }
            set { idPipesStripCommon = value; }
        }
        public Int32 CategoryType
        {
            get { return categoryType; }
            set { categoryType = value; }
        }
        public Int32 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Int32 Thickness
        {
            get { return thickness; }
            set { thickness = value; }
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
