using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblStatusReasonTO
    {
        #region Declarations
        Int32 idStatusReason;
        Int32 statusId;
        DateTime createdOn;
        String reasonDesc;
        Int32 isOtherComment;
        #endregion

        #region Constructor
        public TblStatusReasonTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStatusReason
        {
            get { return idStatusReason; }
            set { idStatusReason = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String ReasonDesc
        {
            get { return reasonDesc; }
            set { reasonDesc = value; }
        }

        public int IsOtherComment { get => isOtherComment; set => isOtherComment = value; }
        #endregion
    }
}
