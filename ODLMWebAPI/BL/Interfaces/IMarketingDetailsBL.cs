using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IMarketingDetailsBL
    {
        MarketingDetailsTO SelectVisitDetailsList(int visitId);
        ResultMessage SaveMarketingDetails(MarketingDetailsTO marketingDetailsTO);
        ResultMessage UpdateMarketingDetails(MarketingDetailsTO marketingDetailsTO);
    }
}
