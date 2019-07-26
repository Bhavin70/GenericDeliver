using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblTranActionsTO
    {
        #region Declarations
        Int32 idTranActions;
        Int32 userId;
        Int32 tranActionTypeId;
        Int32 transId;
        Int32 transTypeId;
        Int32 createdBy;
        DateTime createdOn;
        #endregion

        #region Constructor
        public TblTranActionsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTranActions
        {
            get { return idTranActions; }
            set { idTranActions = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 TranActionTypeId
        {
            get { return tranActionTypeId; }
            set { tranActionTypeId = value; }
        }
        public Int32 TransId
        {
            get { return transId; }
            set { transId = value; }
        }
        public Int32 TransTypeId
        {
            get { return transTypeId; }
            set { transTypeId = value; }
        }

        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        #endregion
    }
}
