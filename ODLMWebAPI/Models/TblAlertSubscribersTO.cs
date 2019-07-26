using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.Models
{
    public class TblAlertSubscribersTO
    {
        #region Declarations
        Int32 idSubscription;
        Int32 alertDefId;
        Int32 userId;
        Int32 roleId;
        Int32 subscribedBy;
        DateTime subscribedOn;
        List<TblAlertSubscriptSettingsTO> alertSubscriptSettingsTOList;

        //Priyanka [20-09-18]
        Int32 updatedBy;
        DateTime updatedOn;
        Int32 isActive;
        String userDisplayName;
        String roleDesc;

        #endregion

        #region Constructor
        public TblAlertSubscribersTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSubscription
        {
            get { return idSubscription; }
            set { idSubscription = value; }
        }
        public Int32 AlertDefId
        {
            get { return alertDefId; }
            set { alertDefId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }
        public Int32 SubscribedBy
        {
            get { return subscribedBy; }
            set { subscribedBy = value; }
        }
        public DateTime SubscribedOn
        {
            get { return subscribedOn; }
            set { subscribedOn = value; }
        }

        public List<TblAlertSubscriptSettingsTO> AlertSubscriptSettingsTOList
        {
            get
            {
                return alertSubscriptSettingsTOList;
            }

            set
            {
                alertSubscriptSettingsTOList = value;
            }
        }

        public int IsActive { get => isActive; set => isActive = value; }
        public int UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public string UserDisplayName { get => userDisplayName; set => userDisplayName = value; }
        public string RoleDesc { get => roleDesc; set => roleDesc = value; }
        #endregion
    }
}
