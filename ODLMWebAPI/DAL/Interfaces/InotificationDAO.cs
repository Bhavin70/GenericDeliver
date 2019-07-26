using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
   public interface InotificationDAO
    {
         NotificationTO SelectNotificationTO(Int32 idAlertInstance);
    }
}
