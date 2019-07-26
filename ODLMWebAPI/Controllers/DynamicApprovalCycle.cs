using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using System.Threading.Tasks;
using System;
using ODLMWebAPI.BL;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class DynamicApprovalController : Controller
    {
        private readonly IDynamicApprovalCYcleBL iDynamicApprovalCYcleBL;
       
        public DynamicApprovalController(IDynamicApprovalCYcleBL iDynamicApprovalCYcleBL)
        {
            this.iDynamicApprovalCYcleBL=iDynamicApprovalCYcleBL;
        }
           [Route("GetAllApprovalList")]
        [HttpGet]
        public List<DimDynamicApprovalTO> GetAllApprovalList()
        {
           
         return iDynamicApprovalCYcleBL.SelectAllApprovalList();
        
        }

     
[Route("GetListBySequenceNo")]
         [HttpGet]
 public  DataTable GetListBySequenceNo(int seqNo)
{
return iDynamicApprovalCYcleBL.SelectAllList(seqNo);
 }
     
[Route("UpdateStatus")]
         [HttpPost]
 public  ResultMessage UpdateStatus([FromBody] JObject data)
{
    ResultMessage resultMessage=new ResultMessage();
    try{
var tableData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data["data"].ToString());
     var Status = data["status"].ToString();
     var seqNo=data["seqNo"].ToString();
     var userId=data["userId"].ToString();
     int result=iDynamicApprovalCYcleBL.UpdateStatus(tableData,Convert.ToInt32(Status),Convert.ToInt32(userId),Convert.ToInt32(seqNo));
  if(result==1)
  {
resultMessage.Result=1;
if(Status=="1")
{
resultMessage.DisplayMessage="Status Approved Successfully for Id="+tableData["Id"];
return resultMessage;
}
else{
resultMessage.DisplayMessage="Status Reject Successfully for Id="+tableData["Id"];
return resultMessage;
}


  } 
    }
    catch(Exception ex)
    {
 resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call UpdateStatus :" + ex;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
    }
      return resultMessage;
     

 }
        
      

    }

}