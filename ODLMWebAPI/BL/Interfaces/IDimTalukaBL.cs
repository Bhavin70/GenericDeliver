using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimTalukaBL
    {
        int UpdateDimTaluka(StateMasterTO dimtalTO);
        int InsertDimTaluka(StateMasterTO dimTalukaTO);
    }
}
