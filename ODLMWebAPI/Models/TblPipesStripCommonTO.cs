using static ODLMWebAPI.StaticStuff.Constants;
using System;

namespace ODLMWebAPI.Models
{
    public class TblPipesStripCommonTO
    {
        #region Declarations

        Int32 idPipesStripCommon;
        Int32 size;
        Int32 thickness;
        Int32 isActive;
        DateTime createdOn;
        Int32 categoryType;

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
       
        #endregion
    }
}
