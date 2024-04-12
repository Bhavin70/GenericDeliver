using static ODLMWebAPI.StaticStuff.Constants;
using System;

namespace ODLMWebAPI.Models
{
    public class TblPipesStripCommonTO
    {
        #region Declarations

        Int32 idPipesStripCommon;
        decimal size;
        decimal thickness;
        Int32 isActive;
        DateTime createdOn;
        Int32 categoryType;
        Int32 quantity;

        #endregion

        #region Constructor
        public TblPipesStripCommonTO()
        {

        }
        #endregion

        #region GetSet
        public Int32 IdPipesStripCommon
        {
            get { return idPipesStripCommon; }
            set { idPipesStripCommon = value; }
        }
        public decimal Size
        {
            get { return size; }
            set { size = value; }
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
        public Int32 CategoryType
        {
            get { return categoryType; }
            set { categoryType = value; }
        }
        public Int32 Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        #endregion
    }
    public class TblPipesStripCommonSizeTO
    {

        #region

        Int32 idPipesStripCommon;
        decimal size;

        #endregion

        #region Get Set

        public Int32 IdPipesStripCommon
        {
            get { return idPipesStripCommon; }
            set { idPipesStripCommon = value; }
        }
        public decimal Size
        {
            get { return size; }
            set { size = value; }
        }

        #endregion
    }
    public class TblPipesStripCommonThicknessTO
    {

        #region

        Int32 idPipesStripCommon;
        decimal thickness;

        #endregion

        #region Get Set

        public Int32 IdPipesStripCommon
        {
            get { return idPipesStripCommon; }
            set { idPipesStripCommon = value; }
        }
        public decimal Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        #endregion
    }
    public class TblPipesStripCommonQuantityTO
    {

        #region

        Int32 idPipesStripCommon;
        Int32 quantity;

        #endregion

        #region Get Set

        public Int32 IdPipesStripCommon
        {
            get { return idPipesStripCommon; }
            set { idPipesStripCommon = value; }
        }
        public Int32 Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        #endregion
    }

}
