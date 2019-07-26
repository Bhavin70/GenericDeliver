using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblAlertUsersBL : ITblAlertUsersBL
    {
        #region Selection
        public List<TblAlertUsersTO> SelectAllTblAlertUsersList()
        {
            return  TblAlertUsersDAO.SelectAllTblAlertUsers();
        }

        public TblAlertUsersTO SelectTblAlertUsersTO(Int32 idAlertUser)
        {
           return  TblAlertUsersDAO.SelectTblAlertUsers(idAlertUser);
        }

        public List<TblAlertUsersTO> SelectAllActiveNotAKAlertList(Int32 userId,Int32 roleId)
        {
            return TblAlertUsersDAO.SelectAllActiveNotAKAlertList(userId,roleId);
        }

        public List<TblAlertUsersTO> SelectAllActiveAlertList(Int32 userId, List<TblUserRoleTO> tblUserRoleToList)
        {
            String roleIds = String.Empty;
            if(tblUserRoleToList!=null && tblUserRoleToList.Count >0)
            {
                var stringsArray = tblUserRoleToList.Select(i => i.RoleId.ToString()).ToArray();
                roleIds = string.Join(",", stringsArray);
            }

            List<TblAlertUsersTO> list = TblAlertUsersDAO.SelectAllActiveAlertList(userId, roleIds);
            List<TblAlertActionDtlTO> alertActionDtlTOList = BL.TblAlertActionDtlBL.SelectAllTblAlertActionDtlList(userId);
            if (alertActionDtlTOList != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var isAck = alertActionDtlTOList.Where(a => a.AlertInstanceId == list[i].AlertInstanceId).LastOrDefault();
                    if (isAck != null)
                    {
                        if (isAck.ResetDate != DateTime.MinValue)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                        else
                            list[i].IsAcknowledged = 1;
                    }
                }


                //list = list.OrderByDescending(a => a.IsAcknowledged==0).ThenBy(a=>a.AlertInstanceId).ToList();
                list = list.OrderByDescending(a => a.RaisedOn).ThenBy(a => a.AlertInstanceId).ToList();
                if(list!=null && list.Count >0)
                {
                    List<TblAlertUsersTO> accList = list.Where(w => w.IsAcknowledged == 1).ToList();

                    List<TblAlertUsersTO> notAccList = list.Where(w => w.IsAcknowledged == 0).ToList();

                    list = new List<TblAlertUsersTO>();

                    list.AddRange(notAccList);
                    list.AddRange(accList);
                }
             
            }
            if(list!=null && list.Count >0)
            {
              
                list = list.GroupBy(ele => ele.AlertInstanceId).Select(s => s.FirstOrDefault()).ToList();
            }
            return list;
        }

        #endregion

        #region Insertion
        public int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            return TblAlertUsersDAO.InsertTblAlertUsers(tblAlertUsersTO);
        }

        public int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertUsersDAO.InsertTblAlertUsers(tblAlertUsersTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            return TblAlertUsersDAO.UpdateTblAlertUsers(tblAlertUsersTO);
        }

        public int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertUsersDAO.UpdateTblAlertUsers(tblAlertUsersTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAlertUsers(Int32 idAlertUser)
        {
            return TblAlertUsersDAO.DeleteTblAlertUsers(idAlertUser);
        }

        public int DeleteTblAlertUsers(Int32 idAlertUser, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertUsersDAO.DeleteTblAlertUsers(idAlertUser, conn, tran);
        }

        #endregion
        
    }
}
