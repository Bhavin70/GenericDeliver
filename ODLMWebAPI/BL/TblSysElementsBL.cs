using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using System.Linq;

namespace ODLMWebAPI.BL
{  
    public class TblSysElementsBL : ITblSysElementsBL
    {
        private readonly ITblSysElementsDAO _iTblSysElementsDAO;
        private readonly ITblSysEleRoleEntitlementsBL _iTblSysEleRoleEntitlementsBL;
        private readonly ITblSysEleRoleEntitlementsDAO _iTblSysEleRoleEntitlementsDAO;
        private readonly ITblSysEleUserEntitlementsBL _iTblSysEleUserEntitlementsBL;
        private readonly ITblSysEleUserEntitlementsDAO _iTblSysEleUserEntitlementsDAO;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblUserBL _iTblUserBL;
        private readonly ITblUserRoleDAO _iTblUserRoleDAO;
         private readonly IDimensionDAO _iDimentionDAO;
          private readonly ITblUserRoleBL _iTblUserRoleBL;
          //private readonly ITblSysElementsBL _iTblSysElementBL;
        private readonly IConnectionString _iConnectionString;
        public TblSysElementsBL(
            ITblUserBL _iTblUserBL,
            ITblUserRoleBL iTblUserRoleBL,
            IDimensionDAO iDimentionDAO,
            ITblUserRoleDAO iTblUserRoleDAO,
            ITblUserDAO iTblUserDAO,
            IConnectionString iConnectionString,
            ITblSysEleUserEntitlementsDAO iTblSysEleUserEntitlementsDAO,
            ITblSysEleUserEntitlementsBL iTblSysEleUserEntitlementsBL, 
            ITblSysElementsDAO iTblSysElementsDAO,
            ITblSysEleRoleEntitlementsBL iTblSysEleRoleEntitlementsBL, 
            ITblSysEleRoleEntitlementsDAO iTblSysEleRoleEntitlementsDAO
            )
        {
            _iTblSysElementsDAO = iTblSysElementsDAO;
            _iTblSysEleRoleEntitlementsBL = iTblSysEleRoleEntitlementsBL;
            _iTblSysEleRoleEntitlementsDAO = iTblSysEleRoleEntitlementsDAO;
            _iTblSysEleUserEntitlementsBL = iTblSysEleUserEntitlementsBL;
            _iTblSysEleUserEntitlementsDAO = iTblSysEleUserEntitlementsDAO;
            _iTblUserDAO = iTblUserDAO;
            _iConnectionString = iConnectionString;
            _iTblUserRoleDAO = iTblUserRoleDAO;
            _iDimentionDAO = iDimentionDAO;
            //_iTblSysElementBL =iTblSysElementBL;
            _iTblUserRoleBL = iTblUserRoleBL;
            // this._iTblUserBL=_iTblUserBL;
        }
        #region Selection
        //public List<TblSysElementsTO> SelectAllTblSysElementsList(int menuPgId)
        //{
        //    return  TblSysElementsDAO.SelectAllTblSysElements(menuPgId);
        //}

        public TblSysElementsTO SelectTblSysElementsTO(Int32 idSysElement)
        {
            return _iTblSysElementsDAO.SelectTblSysElements(idSysElement);
        }


        /// <summary>
        /// Sanjay [2017-04-20] Following function will return the dictionary of element with its permissions details
        /// for given user and role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Dictionary<int, String> SelectSysElementUserEntitlementDCT(int userId, int roleId)
        {
            Dictionary<int, String> roleEntitlementDict = _iTblSysEleRoleEntitlementsDAO.SelectAllTblSysEleRoleEntitlementsDCT(roleId);
            List<TblSysEleUserEntitlementsTO> userEntitlementList = _iTblSysEleUserEntitlementsBL.SelectAllTblSysEleUserEntitlementsList(userId);
            return SinkUpDictionaryAndList(ref roleEntitlementDict, userEntitlementList);
        }

        public Dictionary<int, String> SelectSysElementUserMultiRoleEntitlementDCT(int userId, String roleId ,int? moduleId)
        {
            //passed null to get all permissions
            Dictionary<int, String> roleEntitlementDict = _iTblSysEleRoleEntitlementsDAO.SelectAllTblSysEleMultipleRoleEntitlementsDCT(roleId,null);
            List<TblSysEleUserEntitlementsTO> userEntitlementList = _iTblSysEleUserEntitlementsBL.SelectAllTblSysEleUserEntitlementsList(userId,null);
            return SinkUpDictionaryAndList(ref roleEntitlementDict, userEntitlementList);
        }



        private Dictionary<int, string> SinkUpDictionaryAndList(ref Dictionary<int, String> roleEntitlementDict, List<TblSysEleUserEntitlementsTO> userEntitlementList)
        {
            if (userEntitlementList != null && userEntitlementList.Count > 0)
            {
                if (roleEntitlementDict != null && roleEntitlementDict.Count > 0)
                {
                    for (int i = 0; i < userEntitlementList.Count; i++)
                    {
                        //if key present then override else insert
                        if (roleEntitlementDict.ContainsKey(userEntitlementList[i].SysEleId))
                        {
                            roleEntitlementDict[userEntitlementList[i].SysEleId] = userEntitlementList[i].Permission;
                        }
                        else
                        {
                            roleEntitlementDict.Add(userEntitlementList[i].SysEleId, userEntitlementList[i].Permission);
                        }
                    }
                }
                else // create new dictionary and add all user entitlement
                {
                    roleEntitlementDict = new Dictionary<int, string>();
                    for (int i = 0; i < userEntitlementList.Count; i++)
                    {
                        //if key not present then insert else override
                        if (!roleEntitlementDict.ContainsKey(userEntitlementList[i].SysEleId))
                        {
                            roleEntitlementDict.Add(userEntitlementList[i].SysEleId, userEntitlementList[i].Permission);
                        }
                        else
                        {
                            roleEntitlementDict[userEntitlementList[i].SysEleId] = userEntitlementList[i].Permission;
                        }
                    }
                }
            }
            return roleEntitlementDict;
        }

        public List<PermissionTO>  getAllUsersWithModulePermission(int moduleId,int role_Id,int DeptId)
        {
            int cnt=0;
            List<PermissionTO> permissionTOList = new List<PermissionTO>();
             List<DropDownTO> userList=new List<DropDownTO>();
                     if(role_Id!=0)
                    {
                       userList= _iTblUserRoleDAO.SelectUsersFromRoleForDropDown(role_Id);                      
                    }
                   else if(DeptId!=0){
                     userList=_iDimentionDAO.GetUserListDepartmentWise(DeptId.ToString());
                    }
                    else{
                     userList = _iTblUserDAO.SelectAllActiveUsersForDropDown();
                         }
               
            
            List<TblSysElementsTO> list = _iTblSysElementsDAO.SelectAllTblSysElements(0, "M", 0);
            TblSysElementsTO tblSysElementsTO = list.Where(w => w.ModuleId == moduleId).FirstOrDefault();
            if (userList != null && userList.Count > 0 && tblSysElementsTO!=null)
            {
                foreach(var item in userList )
                {                     
                     var UserRoleList = _iTblUserRoleBL.SelectAllActiveUserRoleList(item.Value);
                     int[] roleList = UserRoleList.Where(a => a.IsActive == 1).Select(s => s.RoleId).ToArray();
                     String  roleId = string.Join(",", roleList.ToArray());
                    
                 
                      var SysEleAccessDCT = SelectSysElementUserMultiRoleEntitlementDCT(item.Value, roleId,tblSysElementsTO.IdSysElement);
                      
                      
                           PermissionTO permissionTO = new PermissionTO();
                       
                        permissionTO.IdSysElement = tblSysElementsTO.IdSysElement;
                        permissionTO.MenuId = tblSysElementsTO.MenuId;
                        permissionTO.PageElementId = tblSysElementsTO.PageElementId;
                        permissionTO.Type = tblSysElementsTO.Type;
                        permissionTO.UserId = item.Value;
                        permissionTO.UserName = item.Text;
// Imp person
  if( SysEleAccessDCT.Count==0)  // if Record is Not Exist
  {
      permissionTO.IsboolImpPerson=false;
  } else{
      permissionTO.IsImpPerson=_iTblSysElementsDAO.SelectIsImportantPerson(item.Value, permissionTO.IdSysElement);
   if(permissionTO.IsImpPerson==1) {
       permissionTO.IsboolImpPerson=true;
   } else{
     permissionTO.IsboolImpPerson=false;   
   } 
  }
//End

                       if( SysEleAccessDCT.Count==0)  // if Record is Not Exist
                     {  permissionTO.EffectivePermission = "NA";
                       permissionTO.IsPermission=false;
                     }
                       else
                       {
                       var isModulePermission=   SysEleAccessDCT.Where(m=>m.Key==tblSysElementsTO.IdSysElement).FirstOrDefault();
                       if(isModulePermission.Key>0)
                       {
                        permissionTO.EffectivePermission = SysEleAccessDCT[tblSysElementsTO.IdSysElement];
                       }                                           
                     
                        if(permissionTO.EffectivePermission=="RW")
                        {
                             permissionTO.IsPermission=true; 
                             cnt+=1;
                        }
                        else{
                            permissionTO.IsPermission=false;
                        }
                     
                          
                       }
                       permissionTOList.Add(permissionTO);
                     
                }
                           
               
               
            }
            if(permissionTOList!=null && permissionTOList.Count>0)
            {
                permissionTOList[0].ConfigLicenseCnt=cnt.ToString();
            }
            return permissionTOList;
        }
        public List<PermissionTO> SelectAllPermissionList(int menuPgId, int roleId, int userId,int moduleId)
        {
            List<PermissionTO> permissionTOList = new List<PermissionTO>();
            var type = "MI";
            if((roleId != 0 || userId != 0) && moduleId == 0)
            {
                type = "M";
            }
            List<TblSysElementsTO> list = _iTblSysElementsDAO.SelectAllTblSysElements(menuPgId, type, moduleId);
            if (list != null)
            {
                Dictionary<int, String> permissionDCT = SelectSysElementUserEntitlementDCT(userId, roleId);

                for (int i = 0; i < list.Count; i++)
                {
                    PermissionTO permissionTO = new PermissionTO();
                    permissionTO.IdSysElement = list[i].IdSysElement;
                    permissionTO.MenuId = list[i].MenuId;
                    permissionTO.PageElementId = list[i].PageElementId;
                    permissionTO.Type = list[i].Type;
                    permissionTO.RoleId = roleId;
                    permissionTO.UserId = userId;
                    permissionTO.ElementName = list[i].ElementName;
                    permissionTO.ElementDesc = list[i].ElementDesc;

                    if (permissionDCT != null && permissionDCT.ContainsKey(list[i].IdSysElement))
                    {
                        permissionTO.EffectivePermission = permissionDCT[list[i].IdSysElement];
                    }
                    else
                        permissionTO.EffectivePermission = "NA";

                    permissionTOList.Add(permissionTO);

                }
            }

            return permissionTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            return _iTblSysElementsDAO.InsertTblSysElements(tblSysElementsTO);
        }

        public int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysElementsDAO.InsertTblSysElements(tblSysElementsTO, conn, tran);
        }

// added By Vipul for UserTracking
 public  ResultMessage SaveAllUserPermission(PermissionTO permissionTO)
 {
      ResultMessage resultMsg = new ResultMessage();
     List<DropDownTO> userList = _iTblUserBL.SelectAllActiveUsersForDropDown();
    for(int i=0;i<userList.Count;i++)
        {
               permissionTO.UserId=userList[i].Value;
              resultMsg= SaveRoleOrUserPermission(permissionTO);
         }
  return resultMsg;
 }
//END
        public ResultMessage SaveRoleOrUserPermission(PermissionTO permissionTO)
        {
            ResultMessage resultMsg = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (permissionTO.UserId > 0)
                {
                    TblSysEleUserEntitlementsTO userPermissionTO = _iTblSysEleUserEntitlementsDAO.SelectUserSysEleUserEntitlements(permissionTO.UserId, permissionTO.IdSysElement, conn, tran);
                    if (userPermissionTO == null)
                    {
                        // Insert New Entry
                        userPermissionTO = new TblSysEleUserEntitlementsTO();
                        userPermissionTO.UserId = permissionTO.UserId;
                        userPermissionTO.Permission = permissionTO.EffectivePermission;
                        userPermissionTO.SysEleId = permissionTO.IdSysElement;
                        userPermissionTO.CreatedBy = permissionTO.CreatedBy;
                        userPermissionTO.CreatedOn = permissionTO.CreatedOn;
                         // Newly Added
                        userPermissionTO.IsImpPerson=permissionTO.IsImpPerson;
                        result = _iTblSysEleUserEntitlementsBL.InsertTblSysEleUserEntitlements(userPermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.MessageType = ResultMessageE.Error;
                            resultMsg.Result = 0;
                            resultMsg.Text = "Error while Inserting User Permission";
                            resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                            return resultMsg;
                        }
                    }
                    else
                    {
                        userPermissionTO.Permission = permissionTO.EffectivePermission;
                        // Newly Added
                        userPermissionTO.IsImpPerson=permissionTO.IsImpPerson;
                        result = _iTblSysEleUserEntitlementsBL.UpdateTblSysEleUserEntitlements(userPermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.MessageType = ResultMessageE.Error;
                            resultMsg.Result = 0;
                            resultMsg.Text = "Error while Updating User Permission";
                            resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                            return resultMsg;
                        }
                    }
                }
                else
                {
                    TblSysEleRoleEntitlementsTO rolePermissionTO = _iTblSysEleRoleEntitlementsDAO.SelectRoleSysEleUserEntitlements(permissionTO.RoleId, permissionTO.IdSysElement, conn, tran);
                    if (rolePermissionTO == null)
                    {
                        // Insert New Entry
                        rolePermissionTO = new TblSysEleRoleEntitlementsTO();
                        rolePermissionTO.RoleId = permissionTO.RoleId;
                        rolePermissionTO.Permission = permissionTO.EffectivePermission;
                        rolePermissionTO.SysEleId = permissionTO.IdSysElement;
                        rolePermissionTO.CreatedBy = permissionTO.CreatedBy;
                        rolePermissionTO.CreatedOn = permissionTO.CreatedOn;
                        result = _iTblSysEleRoleEntitlementsBL.InsertTblSysEleRoleEntitlements(rolePermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.MessageType = ResultMessageE.Error;
                            resultMsg.Result = 0;
                            resultMsg.Text = "Error while Inserting role Permission";
                            resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                            return resultMsg;
                        }
                    }
                    else
                    {
                        rolePermissionTO.Permission = permissionTO.EffectivePermission;
                        result = _iTblSysEleRoleEntitlementsBL.UpdateTblSysEleRoleEntitlements(rolePermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.MessageType = ResultMessageE.Error;
                            resultMsg.Result = 0;
                            resultMsg.Text = "Error while Updating role Permission";
                            resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                            return resultMsg;
                        }
                    }
                }


                tran.Commit();
                resultMsg.MessageType = ResultMessageE.Information;
                resultMsg.Result = 1;
                resultMsg.Text = "Permission Updated Successfully";
                resultMsg.DisplayMessage = "Permission Updated Successfully";
                return resultMsg;
            }
            catch (Exception ex)
            {
                resultMsg.MessageType = ResultMessageE.Error;
                resultMsg.Exception = ex;
                resultMsg.Result = -1;
                resultMsg.Text = "Exception Error While SaveRoleOrUserPermission ";
                resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                return resultMsg;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        
        #region Updation
        public int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            return _iTblSysElementsDAO.UpdateTblSysElements(tblSysElementsTO);
        }

        public int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysElementsDAO.UpdateTblSysElements(tblSysElementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblSysElements(Int32 idSysElement)
        {
            return _iTblSysElementsDAO.DeleteTblSysElements(idSysElement);
        }

        public int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysElementsDAO.DeleteTblSysElements(idSysElement, conn, tran);
        }

        //Harshala [20-07-2019] : Added to give all permissions to user or role.
        public ResultMessage SavegiveAllPermission(PermissionTO permissionTO)
        {

            ResultMessage resultMsg = new ResultMessage();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
               conn.Open();
                tran = conn.BeginTransaction();
                
                if(permissionTO.UserId>0 || permissionTO.RoleId>0)
                {
                    List<TblSysElementsTO> list = _iTblSysElementsDAO.SelectgiveAllTblSysElements(); 
                     if (list != null)
                     {
                        for (int i = 0; i < list.Count; i++)
                        {
                            permissionTO.IdSysElement=list[i].IdSysElement;
                          result=  SaveRoleOrUserPermission(permissionTO,conn,tran);
                            if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.MessageType = ResultMessageE.Error;
                            resultMsg.Result = 0;
                            resultMsg.Text = "Error while Inserting role Permission";
                            resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                            return resultMsg;
                        }
                          
                        }
                     }
                }

                tran.Commit();
                 resultMsg.DefaultSuccessBehaviour();
                 return resultMsg;

            }
            catch (Exception ex)
            {
                resultMsg.MessageType = ResultMessageE.Error;
                resultMsg.Exception = ex;
                resultMsg.Result = -1;
                resultMsg.Text = "Exception Error While SaveRoleOrUserPermission ";
                resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                return resultMsg;
            }
            finally
            {
               conn.Close();
            }


        }

        public  int SaveRoleOrUserPermission(PermissionTO permissionTO, SqlConnection con, SqlTransaction trann)
        {
            SqlConnection conn = con;
            SqlTransaction tran = trann;
            int result = 0;
            try
            {
              //  conn.Open();
              //  tran = conn.BeginTransaction();

                if (permissionTO.UserId > 0)
                {
                    TblSysEleUserEntitlementsTO userPermissionTO = _iTblSysEleUserEntitlementsDAO.SelectUserSysEleUserEntitlements(permissionTO.UserId, permissionTO.IdSysElement, conn, tran);
                    if (userPermissionTO == null)
                    {
                        // Insert New Entry
                        userPermissionTO = new TblSysEleUserEntitlementsTO();
                        userPermissionTO.UserId = permissionTO.UserId;
                        userPermissionTO.Permission = permissionTO.EffectivePermission;
                        userPermissionTO.SysEleId = permissionTO.IdSysElement;
                        userPermissionTO.CreatedBy = permissionTO.CreatedBy;
                        userPermissionTO.CreatedOn = permissionTO.CreatedOn;
                        result = _iTblSysEleUserEntitlementsBL.InsertTblSysEleUserEntitlements(userPermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            return -1;
                           
                        }
                    }
                    else
                    {
                        userPermissionTO.Permission = permissionTO.EffectivePermission;
                        result = _iTblSysEleUserEntitlementsBL.UpdateTblSysEleUserEntitlements(userPermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            return -1;
                        }
                    }
                }
                else
                {
                    TblSysEleRoleEntitlementsTO rolePermissionTO = _iTblSysEleRoleEntitlementsDAO.SelectRoleSysEleUserEntitlements(permissionTO.RoleId, permissionTO.IdSysElement, conn, tran);
                    if (rolePermissionTO == null)
                    {
                        // Insert New Entry
                        rolePermissionTO = new TblSysEleRoleEntitlementsTO();
                        rolePermissionTO.RoleId = permissionTO.RoleId;
                        rolePermissionTO.Permission = permissionTO.EffectivePermission;
                        rolePermissionTO.SysEleId = permissionTO.IdSysElement;
                        rolePermissionTO.CreatedBy = permissionTO.CreatedBy;
                        rolePermissionTO.CreatedOn = permissionTO.CreatedOn;
                        result =_iTblSysEleRoleEntitlementsBL.InsertTblSysEleRoleEntitlements(rolePermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            return -1;
                        }
                    }
                    else
                    {
                        rolePermissionTO.Permission = permissionTO.EffectivePermission;
                        result =_iTblSysEleRoleEntitlementsBL.UpdateTblSysEleRoleEntitlements(rolePermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            
                            return -1;
                        }
                    }
                }


              //  tran.Commit();
                
                return result;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return -1;

            }
            finally
            {
            }
        }

        //Harshala
        public int checkUserOrRolePermissions(int roleId, int userId)
        {

         List<TblSysElementsTO> AllpermissionsList = _iTblSysElementsDAO.SelectgiveAllTblSysElements();
         List<TblSysEleUserEntitlementsTO> ListOfUserPermission = new List<TblSysEleUserEntitlementsTO>();
         List<TblSysEleRoleEntitlementsTO> ListOfRolePermission = new List<TblSysEleRoleEntitlementsTO>();
        if(roleId != 0 && userId == 0)
         {
             ListOfRolePermission = _iTblSysEleRoleEntitlementsDAO.SelectAllTblSysEleRoleEntitlementsOnlyRW(roleId);  
             if(AllpermissionsList.Count == ListOfRolePermission.Count)
                return 1;
         }
         else
         {
            ListOfUserPermission = _iTblSysEleUserEntitlementsDAO.SelectAllTblSysEleUserEntitlementsOnlyRW(userId);
            if(AllpermissionsList.Count == ListOfUserPermission.Count)
                return 1;
         }
         return 0;
    }

    //harshala
     public  List<tblViewPermissionTO> selectPermissionswrtRole(int roleId,int userId)
        {
            
            List<tblViewPermissionTO> list = _iTblSysElementsDAO.selectPermissionswrtRole(roleId,userId); //gives modules list
           
            if(list.Count>0)
            {
                for(int i=0;i<list.Count;i++)
                {
              
                   list[i].MenuTOs=_iTblSysElementsDAO.SelectMenuPermission(roleId,userId, list[i].IdModule); //add menu's to list list
                   list[i].ElementList=_iTblSysElementsDAO.SelectElementPermission(roleId,userId,list[i].IdModule); //add elements to list
                    
                }
                
            }
            return list;
            
        }     

        /// <summary>
        /// Harshala [2019-08-08] Following function will return the list of all permissions
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectAllPermissionDropdownList()
        {
            return _iTblSysElementsDAO.SelectAllPermissionList();
        } 

           /// <summary>
        /// Harshala [2019-08-09] Following function will return the list of users and roles which are having selected permissions from dropdown
        /// </summary>
        /// <returns></returns>
        public tblViewPermissionTO SelectAllUserRolewrtPermission(int idSysElement)
        {
             tblViewPermissionTO tblViewPermissionTo=new tblViewPermissionTO();

            List<tblRoleUserTO> RoleList = _iTblSysElementsDAO.SelectAllRolewrtPermission(idSysElement);
            List<tblRoleUserTO> UserList = _iTblSysElementsDAO.SelectAllUserwrtPermission(idSysElement);
            
            if(RoleList !=null && RoleList.Count>0)
            {
                    tblViewPermissionTo.RoleList=RoleList;
            }
            
             if(UserList !=null && UserList.Count>0)
             {
             tblViewPermissionTo.UserList=UserList;
             }
            return tblViewPermissionTo;
            
        }





        #endregion
        
    }
}
