using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ISendMailBL
    {
        ResultMessage SendEmail(SendMail tblsendTO, String fileName);
        int SendEmail(SendMail tblsendTO);
        int SendEmailNotification(SendMail tblsendTO);
    }
}
