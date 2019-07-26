using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Controllers
{

    /// <summary>
    /// Sanjay [25-Feb-User Subscription Policy Management]
    /// </summary>
    [Produces("application/json")]
    [Route("api/Subscription")]
    public class SubscriptionController : Controller
    {
        private readonly ITblSysElementsBL _iTblSysElementsBL;
        private readonly ITblModuleBL _iTblModuleBL;
        public SubscriptionController(ITblSysElementsBL iTblSysElementsBL, ITblModuleBL iTblModuleBL)
        {
            _iTblSysElementsBL = iTblSysElementsBL;
            _iTblModuleBL = iTblModuleBL;
        }
        /// <summary>
        /// Sanjay [25-Feb-2019] It will retrive all active modules/applications of suite. 
        /// </summary>
        /// <returns></returns>
        [Route("GetAllApplicationsList")]
        [HttpGet]
        public List<TblModuleTO> GetAllApplicationsList()
        {
            return _iTblModuleBL.SelectTblModuleList();
        }
        /// <summary>
        /// Sanjay [25-Feb-2019] It will retrive all active modules/applications of suite. 
        /// </summary>
        /// <returns></returns>
        [Route("getAllUsersWithModulePermission")]
        [HttpGet]
        public List<PermissionTO> getAllUsersWithModulePermission(int moduleId,int roleId=0,int DeptId=0)
        {
            return _iTblSysElementsBL.getAllUsersWithModulePermission(moduleId,roleId,DeptId);
        }

        
        [Route("PostUpdateModuleDetails")]
        [HttpPost]
        public ResultMessage PostUpdateModuleDetails([FromBody] TblModuleTO tblModuleTO)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (tblModuleTO == null)
                {
                    resultMessage.DefaultBehaviour("tblModuleTO found null");
                    return resultMessage;
                }

                int result= _iTblModuleBL.UpdateTblModule(tblModuleTO);
                if(result!=1)
                {
                    resultMessage.DefaultBehaviour("Error while updating module settings.");
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateModuleDetails");
                return resultMessage;
            }
        }

        [HttpPost("AddModuleEntry")]
 public IActionResult AddModuleEntry([FromBody]TblModuleCommHisTO tblModuleCommHisTO)
 {
ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (tblModuleCommHisTO == null || !ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }

               
                
                int result =_iTblModuleBL.InsertTblModuleCommHis(tblModuleCommHisTO,null,null);
                if (result >= 0)
                {

                 // resultMessage.Result=tblModuleCommHisTO.IdModuleCommHis;
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage=tblModuleCommHisTO.IdModuleCommHis.ToString();
                 //  resultMessage.DisplayMessage ="Settlement Completed By  - #" + tblexpenseTO.UserName + " ";
                    return Ok(resultMessage);

                }
                return Ok();
            }
            catch (Exception exc)
            { resultMessage.DefaultExceptionBehaviour(exc, "AddModuleEntry");
                return Ok(resultMessage);
            }


 }

  [HttpPost("UpdateModuleEntry")]
 public IActionResult UpdateModuleEntry([FromBody]TblModuleCommHisTO tblModuleCommHisTO)
 {
ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (tblModuleCommHisTO == null || !ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }

               
                
                int result = _iTblModuleBL.UpdateInsertTblModuleCommHis(tblModuleCommHisTO,null,null);
                if (result >= 0)
                {

                 // resultMessage.Result=tblModuleCommHisTO.IdModuleCommHis;
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage=tblModuleCommHisTO.IdModuleCommHis.ToString();
                 //  resultMessage.DisplayMessage ="Settlement Completed By  - #" + tblexpenseTO.UserName + " ";
                    return Ok(resultMessage);

                }
                return Ok();
            }
            catch (Exception exc)
            { resultMessage.DefaultExceptionBehaviour(exc, "UpdateModuleEntry");
                return Ok(resultMessage);
            }


 }
 [HttpPost("UpdateModuleEntryBeforeLogOut")]
 public IActionResult UpdateModuleEntryBeforeLogOut([FromBody]TblModuleCommHisTO tblModuleCommHisTO)
 {
ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (tblModuleCommHisTO == null || !ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }

               
                
                int result = _iTblModuleBL.UpdateTblModuleCommHis(tblModuleCommHisTO,null,null);
                if (result >= 0)
                {

                 // resultMessage.Result=tblModuleCommHisTO.IdModuleCommHis;
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage=tblModuleCommHisTO.IdModuleCommHis.ToString();
                 //  resultMessage.DisplayMessage ="Settlement Completed By  - #" + tblexpenseTO.UserName + " ";
                    return Ok(resultMessage);

                }
                return Ok();
            }
            catch (Exception exc)
            { resultMessage.DefaultExceptionBehaviour(exc, "AddModuleEntry");
                return Ok(resultMessage);
            }


 }
[HttpPost("UpdateModuleEntryBeforeLogIn")]
 public IActionResult UpdateModuleEntryBeforeLogIn([FromBody]TblModuleCommHisTO tblModuleCommHisTO)
 {
ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (tblModuleCommHisTO == null || !ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }

               
                
                int result = _iTblModuleBL.UpdateTblModuleCommHisBeforeLogin(tblModuleCommHisTO,null,null);
                if (result >= 0)
                {

                 // resultMessage.Result=tblModuleCommHisTO.IdModuleCommHis;
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage=tblModuleCommHisTO.IdModuleCommHis.ToString();
                 //  resultMessage.DisplayMessage ="Settlement Completed By  - #" + tblexpenseTO.UserName + " ";
                    return Ok(resultMessage);

                }
                return Ok();
            }
            catch (Exception exc)
            { resultMessage.DefaultExceptionBehaviour(exc, "AddModuleEntry");
                return Ok(resultMessage);
            }


 }
 [HttpPost("UpdateSessionOutModuleEntry")]
 public IActionResult UpdateSessionOutAllModuleEntry([FromBody]TblModuleCommHisTO tblModuleCommHis)
 {
ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (tblModuleCommHis == null || !ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }

               
                
                int result = _iTblModuleBL.UpdateAllTblModuleCommHis(tblModuleCommHis,null,null);
                if (result >= 0)
                {

                 // resultMessage.Result=tblModuleCommHisTO.IdModuleCommHis;
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage=tblModuleCommHis.IdModuleCommHis.ToString();
                 //  resultMessage.DisplayMessage ="Settlement Completed By  - #" + tblexpenseTO.UserName + " ";
                    return Ok(resultMessage);

                }
                return Ok();
            }
            catch (Exception exc)
            { resultMessage.DefaultExceptionBehaviour(exc, "UpdateSessionOutModuleEntry");
                return Ok(resultMessage);
            }


 }
 // [vipul] UserTracking[18/03/2019]
  [Route("getAllTblModuleCommHis")]
        [HttpGet]
        public List<TblModuleCommHisTO> getAllTblModuleCommHis(int userId)
        {
            return _iTblModuleBL.GetAlltblModuleCommHis(userId);
        }

        // [vipul] UserTracking[25/03/2019]
  [Route("GetActiveAllowedCnt")]
        [HttpGet]
        public TblModuleTO GetActiveAllowedCnt(int idModule,int userId,int loginId)
        {
            return _iTblModuleBL.GetAllActiveAllowedCnt(idModule,userId,loginId);
        }

        [Route("GetActiveCntDetails")]
        [HttpGet]
        public List<TblModuleCommHisTO> GetActiveCntDetails(int idModule)
        {
            return _iTblModuleBL.GetActiveCntDetails(idModule);
        }

         [HttpPost("CheckVipUserBeforeLogout")]
 public IActionResult CheckVipUserBeforeLogout([FromBody]TblModuleCommHisTO tblModuleCommHis)
 {
ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (tblModuleCommHis == null || !ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }

                              
                int result =_iTblModuleBL.FindLatestLoginIdForLogout(tblModuleCommHis,null,null);
                if (result >= 0)
                {

                 // resultMessage.Result=tblModuleCommHisTO.IdModuleCommHis;
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage=tblModuleCommHis.IdModuleCommHis.ToString();
                 //  resultMessage.DisplayMessage ="Settlement Completed By  - #" + tblexpenseTO.UserName + " ";
                    return Ok(resultMessage);

                }
                return Ok();
            }
            catch (Exception exc)
            { resultMessage.DefaultExceptionBehaviour(exc, "CheckVipUserBeforeLogout");
                return Ok(resultMessage);
            }


 }

    }
}