using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDynamicApprovalCYcleBL
    {
              List<DimDynamicApprovalTO> SelectAllApprovalList();
              
             DataTable SelectAllList(int seqNo);
             int UpdateStatus(Dictionary<string, string> tableData,int status,int userId,int seqNo);
    }
}
