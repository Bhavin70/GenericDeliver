using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IVitplNotify
    { 
        string NotifyToRegisteredDevices();
        string NotifyToRegisteredDevices(String[] devices, String body, String title, int instanceId);
        void NotifyToRegisteredDevicesAsync();
        void SystembrodCasting(TblAlertInstanceTO alertInstanceTO);
    }
}