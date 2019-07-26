using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IVitplSMS
    {
        string SendSMSAsync(ODLMWebAPI.Models.TblSmsTO smsTO);
        Task<string> SendSMSViasmsLaneAsync(ODLMWebAPI.Models.TblSmsTO smsTO);
        DimSmsConfigTO GetSmsConfiguration();
    }
}