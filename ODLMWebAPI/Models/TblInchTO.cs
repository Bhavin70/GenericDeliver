using Microsoft.AspNetCore.Server.Kestrel.Internal.System.IO.Pipelines;
using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;

namespace ODLMWebAPI.Models
{
    public class TblInchTO
    {
        #region Declarations

        Int32 idInch;
        decimal inch;
        Int32 isActive;
        DateTime createdOn;

        #endregion

        #region Constructor
        public TblInchTO()
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
    public class TblInchDropDownTO
    {
        #region Declarations

        Int32 idInch;
        decimal inch;

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
        #endregion
    }

}
