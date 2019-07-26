using System;
using System.Collections.Generic;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace ODLMWebAPI.BL
{ 
    public class DynamicApprovalCycleBL : IDynamicApprovalCYcleBL
    {
       private readonly IDynamicApprovalCycleDAO iDynamicApprovalCYcleDAO;
        public DynamicApprovalCycleBL(IDynamicApprovalCycleDAO iDynamicApprovalCYcleDAO)
        {
        this.iDynamicApprovalCYcleDAO=iDynamicApprovalCYcleDAO;
        }

        #region selection
       public List<DimDynamicApprovalTO> SelectAllApprovalList()
       {
           return iDynamicApprovalCYcleDAO.SelectAllApprovalList();
       }

        #endregion
         public DataTable SelectAllList(int seqNo)
       {
            return iDynamicApprovalCYcleDAO.SelectAllList(seqNo);
       }
       public int UpdateStatus(Dictionary<string, string> tableData,int status,int userId,int seqNo)
       {
return iDynamicApprovalCYcleDAO.UpdateStatus(tableData,status,userId,seqNo);
       }

    }
}