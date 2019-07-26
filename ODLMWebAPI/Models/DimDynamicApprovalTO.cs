using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimDynamicApprovalTO
    {
       public int IdApproval{get;set;}
       public string ApprovalName{get;set;}
       public int SequenceNo{get;set;}
       public int ApprovalTypeId{get;set;}
       public int CurrentStatusId{get;set;}
       public int AuthorisedStatusId{get;set;}
       public int RejectStatusId{get;set;}
       public int IsActive{get;set;}
       public string BootstrapIconName{get;set;}
       public int SysElementId{get;set;}
       public int ModuleId{get;set;}
       public string SelectQuery{get;set;}
       public string UpdateQuery{get;set;}
public string NavigateTo{get;set;}

    }
}
