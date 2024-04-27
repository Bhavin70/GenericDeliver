using System;

namespace ODLMWebAPI.Models
{
    public class TblWidthTO
    {
        #region Declarations

        Int32 idWidth;
        Int32 width;
        Int32 isActive;
        DateTime createdOn;
        #endregion

        #region Constructor
        public TblWidthTO()
        {

        }
        #endregion

        #region GetSet
        public Int32 IdWidth
        {
            get { return idWidth; }
            set { idWidth = value; }
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


        #endregion
    }
    public class TblWidthDropDownTO
    {
        #region Declarations

        Int32 idWidth;
        Int32 width;
      

        #endregion

        #region GetSet
        public Int32 IdWidth
        {
            get { return idWidth; }
            set { idWidth = value; }
        }
        public Int32 Width
        {
            get { return width; }
            set { width = value; }
        }
  
            #endregion
        
    }
}

