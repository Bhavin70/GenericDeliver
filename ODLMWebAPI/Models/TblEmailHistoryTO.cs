using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class TblEmailHistoryTO
    {
        #region Declarations
        Int32 idEmailHistory;
        Int32 createdBy;
        Int32 invoiceId;
        DateTime sendOn;
        String sendBy;
        String sendTo;
        String cc;
        #endregion

        #region Constructor
        public TblEmailHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdEmailHistory
        {
            get { return idEmailHistory; }
            set { idEmailHistory = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 InvoiceId
        {
            get { return invoiceId; }
            set { invoiceId = value; }
        }
        public DateTime SendOn
        {
            get { return sendOn; }
            set { sendOn = value; }
        }
        public String SendBy
        {
            get { return sendBy; }
            set { sendBy = value; }
        }
        public String SendTo
        {
            get { return sendTo; }
            set { sendTo = value; }
        }
        public String Cc
        {
            get { return cc; }
            set { cc = value; }
        }
        #endregion
    }
}
