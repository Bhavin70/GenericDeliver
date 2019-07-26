using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimConfigurePageDAO
    {
        List<DimConfigurePageTO> GetConfigurationByPageId(int pageId);
    }
}
