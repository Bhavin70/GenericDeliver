using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblOrgOverdueHistoryTO
    {
        #region Declarations
        Int32 idOrgOverdueHistory;
        Int32 organizationId;
        Int32 isOverdueExist;
        Int32 createdBy;
        DateTime createdOn;
        Int32 bookingId;
        #endregion

        #region Constructor
        public TblOrgOverdueHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOrgOverdueHistory
        {
            get { return idOrgOverdueHistory; }
            set { idOrgOverdueHistory = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public Int32 IsOverdueExist
        {
            get { return isOverdueExist; }
            set { isOverdueExist = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        #endregion
    }
}
