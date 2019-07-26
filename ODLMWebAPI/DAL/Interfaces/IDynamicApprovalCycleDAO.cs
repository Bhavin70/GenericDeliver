using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDynamicApprovalCycleDAO
    { 
       
  List<DimDynamicApprovalTO> SelectAllApprovalList();
  
    DataTable SelectAllList(int seqNo);
     int UpdateStatus(Dictionary<string, string> tableData,int status,int userId,int seqNo);
    }
}