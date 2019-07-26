using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class TblSupportDetailsTO
    {
        #region Declarations
        Int32 idsupport;
        Int32 moduleId;
        Int32 formid;
        Int32 fromUser;
        Int32 createdBy;
        DateTime createdOn;
        String description;
        Int32 requireTime;
        String comments;
        #endregion

        #region Constructor
        public TblSupportDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 Idsupport
        {
            get { return idsupport; }
            set { idsupport = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public Int32 Formid
        {
            get { return formid; }
            set { formid = value; }
        }
        public Int32 FromUser
        {
            get { return fromUser; }
            set { fromUser = value; }
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
        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public int RequireTime { get => requireTime; set => requireTime = value; }
        public string Comments { get => comments; set => comments = value; }
        #endregion
    }
}
