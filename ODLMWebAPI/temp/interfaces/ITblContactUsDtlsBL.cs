using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblContactUsDtlsBL
    {
        List<IGrouping<int, TblContactUsDtls>> SelectContactUsDtls(int IsActive);
    }
}
