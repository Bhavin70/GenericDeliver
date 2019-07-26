using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{    
    public class TblUserRoleBL : ITblUserRoleBL
    { 
        #region Selection
        private readonly ITblUserRoleDAO _iTblUserRoleDAO;
        private readonly IDimRoleTypeDAO _iDimRoleTypeDAO;
        public TblUserRoleBL(IDimRoleTypeDAO iDimRoleTypeDAO, ITblUserRoleDAO iTblUserRoleDAO)
        {
            _iTblUserRoleDAO = iTblUserRoleDAO;
            _iDimRoleTypeDAO = iDimRoleTypeDAO;
        }
        public List<TblUserRoleTO> SelectAllTblUserRoleList()
        {
            return _iTblUserRoleDAO.SelectAllTblUserRole();
        }

        public List<TblUserRoleTO> SelectAllActiveUserRoleList(Int32 userId)
        {
            return _iTblUserRoleDAO.SelectAllActiveUserRole(userId);
        }

        public TblUserRoleTO SelectTblUserRoleTO(Int32 idUserRole)
        {
            return _iTblUserRoleDAO.SelectTblUserRole(idUserRole);
            
        }
        public Int32 IsAreaConfigurationEnabled(Int32 userId)
        {
            int isConfEn = _iTblUserRoleDAO.IsAreaAllocatedUser(userId);
            return isConfEn;

        }

        /// <summary>
        /// Sudhir[22-AUG-2018] Added Connection , Transaction
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TblUserRoleTO> SelectAllActiveUserRoleList(Int32 userId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserRoleDAO.SelectAllActiveUserRole(userId, conn, tran);
        }


        public List<DropDownTO> SelectUsersFromRoleForDropDown(Int32 roleId)
        {
            return _iTblUserRoleDAO.SelectUsersFromRoleForDropDown(roleId);

        }

        public List<DropDownTO> SelectUsersFromRoleIdsForDropDown(string roleId)
        {
            return _iTblUserRoleDAO.SelectUsersFromRoleIdsForDropDown(roleId);

        }


        public List<DropDownTO> SelectUsersFromRoleTypeForDropDown(Int32 roleTypeId)
        {
            return _iTblUserRoleDAO.SelectUsersFromRoleTypeForDropDown(roleTypeId);

        }


        public TblUserRoleTO SelectUserRoleTOAccToPriority(List<TblUserRoleTO> userRoleTOList)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            List<TblUserRoleTO> temptblUserRoleTOList = new List<TblUserRoleTO>();
  
            if (userRoleTOList != null && userRoleTOList.Count > 0 )
            {
               
                temptblUserRoleTOList = userRoleTOList.Where(ele => (ele.EnableAreaAlloc != 1) && (ele.RoleTypeId != (Convert.ToInt32(Constants.SystemRoleTypeE.C_AND_F_AGENT))) && (ele.RoleTypeId != (Convert.ToInt32(Constants.SystemRoleTypeE.Dealer)))).ToList();
                if(temptblUserRoleTOList!=null && temptblUserRoleTOList.Count >0)
                {
                    tblUserRoleTO = temptblUserRoleTOList[0];
                   // return tblUserRoleTO;
                }
                else 
                {
                    temptblUserRoleTOList = userRoleTOList.Where(ele =>ele.EnableAreaAlloc==1).ToList();
                    if (temptblUserRoleTOList != null && temptblUserRoleTOList.Count > 0)
                    {
                        tblUserRoleTO = temptblUserRoleTOList[0];
                        //return tblUserRoleTO;
                    }
                    else
                    {
                        temptblUserRoleTOList = userRoleTOList.Where(ele => ele.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.C_AND_F_AGENT)).ToList();
                        if (temptblUserRoleTOList != null && temptblUserRoleTOList.Count > 0)
                        {
                            tblUserRoleTO = temptblUserRoleTOList[0];
                           // return tblUserRoleTO;
                        }
                        else
                        {
                            temptblUserRoleTOList = userRoleTOList.Where(ele => ele.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.Dealer)).ToList();
                            if (temptblUserRoleTOList != null && temptblUserRoleTOList.Count > 0)
                            {
                                tblUserRoleTO = temptblUserRoleTOList[0];
                               // return tblUserRoleTO;
                            }

                        }
                    }
                }
            }
            return tblUserRoleTO;

        }

        public Boolean selectRolePriorityForOther(List<TblUserRoleTO> userRoleToList)
        {
            Boolean isPriority = true;
            if(userRoleToList!=null && userRoleToList.Count >0)
            {
                List<TblUserRoleTO> otherRoleTolist = userRoleToList.Where(ele => ele.RoleTypeId != Convert.ToInt32(Constants.SystemRoleTypeE.C_AND_F_AGENT)).ToList();
                if(otherRoleTolist!=null && otherRoleTolist.Count >0)
                {
                    isPriority = true;
                }
                else
                {
                    isPriority = false;
                }
            }
            return isPriority;

        }



        #endregion

        #region Insertion
        public int InsertTblUserRole(TblUserRoleTO tblUserRoleTO)
        {
            return _iTblUserRoleDAO.InsertTblUserRole(tblUserRoleTO);
        }

        public int InsertTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserRoleDAO.InsertTblUserRole(tblUserRoleTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO)
        {
            return _iTblUserRoleDAO.UpdateTblUserRole(tblUserRoleTO);
        }

        public int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserRoleDAO.UpdateTblUserRole(tblUserRoleTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblUserRole(Int32 idUserRole)
        {
            return _iTblUserRoleDAO.DeleteTblUserRole(idUserRole);
        }

        public int DeleteTblUserRole(Int32 idUserRole, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserRoleDAO.DeleteTblUserRole(idUserRole, conn, tran);
        }

        #endregion
        
    }
}
