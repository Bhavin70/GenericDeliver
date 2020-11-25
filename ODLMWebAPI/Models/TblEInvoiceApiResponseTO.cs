using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblEInvoiceApiResponseTO
    {
        #region Declarations

        Int32 idResponse;
        Int32 apiId;
        Int32 invoiceId;
        string responseStatus;
        string response;
        Int32 createdBy;
        DateTime createdOn;

        #endregion

        #region Constructor

        public TblEInvoiceApiResponseTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdResponse
        {
            get { return idResponse; }
            set { idResponse = value; }
        }

        public Int32 ApiId
        {
            get { return apiId; }
            set { apiId = value; }
        }
        public Int32 InvoiceId
        {
            get { return invoiceId; }
            set { invoiceId = value; }
        }
        public string ResponseStatus
        {
            get { return responseStatus; }
            set { responseStatus = value; }
        }
        public string Response
        {
            get { return response; }
            set { response = value; }
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

        #endregion

    }
}
