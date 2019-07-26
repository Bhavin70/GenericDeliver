using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimDistrictBL
    {
        int InsertDimDistrict(StateMasterTO dimDistrictTO);
        int UpdateDimDistrict(StateMasterTO dimDistrictTO);
    }
}
