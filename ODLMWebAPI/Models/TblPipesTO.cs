using Microsoft.AspNetCore.Server.Kestrel.Internal.System.IO.Pipelines;
using System;

namespace ODLMWebAPI.Models
{
    public class TblPipesTO
    {
        #region Declarations
        Int32 idPipes;
        decimal inch;
        Int32 isActive;
        DateTime createdOn;
        Int32 idPipesStripCommon;
        Int32 categoryType;
        decimal size;
        decimal thickness;

        #endregion

        #region Constructor
        public TblPipesTO()
        {

        }
        #endregion

        #region GetSet
        public Int32 IdPipes
        {
            get { return idPipes; }
            set { idPipes = value; }
        }
        public decimal Inch
        {
            get { return inch; }
            set { inch = value; }
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
        #endregion
    }
    public class TblPipesDropDownTo
    {

        #region

        Int32 idPipes;
        decimal inch;

        #endregion

        #region Get Set

        public Int32 IdPipes
        {
            get { return idPipes; }
            set { idPipes = value; }
        }
        public decimal Inch
        {
            get { return inch; }
            set { inch = value; }
        }

        #endregion
    }
}
