using Microsoft.AspNetCore.Server.Kestrel.Internal.System.IO.Pipelines;
using System;

namespace ODLMWebAPI.Models
{
    public class TblPipesTO
    {
        #region Declarations
        Int32 idInch;
        decimal inch;
        Int32 idSize;
        string size;
        Int32 idThickness;
        decimal thickness;
        Int32 isActive;
        DateTime createdOn;

        #endregion

        #region Constructor
        public TblPipesTO()
        {

        }
        #endregion

        #region GetSet
        public Int32 IdInch
        {
            get { return idInch; }
            set { idInch = value; }
        }
        public decimal Inch
        {
            get { return inch; }
            set { inch = value; }
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
    public class TblPipesDropDownTo
    {

    }
}
