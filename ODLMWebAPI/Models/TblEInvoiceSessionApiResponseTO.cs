using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblEInvoiceSessionApiResponseTO
    {
        #region Declarations

        Int32 idResponse;
        Int32 apiId;
        string accessToken;
        DateTime tokenExpiresAt;
        string responseStatus;
        string response;
        Int32 createdBy;
        DateTime createdOn;

        #endregion

        #region Constructor

        public TblEInvoiceSessionApiResponseTO()
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
        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
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
