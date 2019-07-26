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

namespace ODLMWebAPI.BL
{     
    public class TblUserBL : ITblUserBL
    {
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblPersonDAO _iTblPersonDAO;
        private readonly ITblAddressDAO _iTblAddressDAO;
        private readonly ITblUserExtDAO _iTblUserExtDAO;
        private readonly ITblOrgStructureBL _iTblOrgStructureBL;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITblUserPwdHistoryDAO _iTblUserPwdHistoryDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblLoginDAO _iTblLoginDAO;
            private readonly ITblModuleBL _iTblModuleBL;
        private readonly ICommon _iCommon;
        //
        public TblUserBL(ITblModuleBL _iTblModuleBL,ITblLoginDAO iTblLoginDAO, ITblUserRoleBL iTblUserRoleBL, ITblOrgStructureBL iTblOrgStructureBL, ICommon iCommon, IConnectionString iConnectionString, ITblUserPwdHistoryDAO iTblUserPwdHistoryDAO, ITblUserExtDAO iTblUserExtDAO, ITblAddressDAO iTblAddressDAO, ITblPersonDAO iTblPersonDAO, ITblUserDAO iTblUserDAO)
        {
            _iTblUserDAO = iTblUserDAO;
            _iTblPersonDAO = iTblPersonDAO;
            _iTblAddressDAO = iTblAddressDAO;
            _iTblUserExtDAO = iTblUserExtDAO;
            _iTblOrgStructureBL = iTblOrgStructureBL;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblUserPwdHistoryDAO = iTblUserPwdHistoryDAO;
            _iConnectionString = iConnectionString;
            _iTblLoginDAO = iTblLoginDAO;
            _iCommon = iCommon;
            this._iTblModuleBL=_iTblModuleBL;
        }
        #region Selection
        //public List<DropDownTO> SelectAllTblModuleList()
        //{
        //    return _iTblModuleDAO.SelectAllTblModule();
        //}
        public List<TblUserTO> SelectAllTblUserList(Boolean onlyActiveYn)
        {
            return _iTblUserDAO.SelectAllTblUser(onlyActiveYn);
        }

        public TblUserTO SelectTblUserTO(Int32 idUser)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblUserDAO.SelectTblUser(idUser,conn,tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }            
        }

        public int SelectUserByImeiNumber(string idDevice)
        {
            TblUserTO tblUserTo = new TblUserTO();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                tblUserTo =  _iTblUserDAO.SelectUserByImeiNumber(idDevice, conn, tran);
                if (tblUserTo != null)
                    return tblUserTo.IdUser;
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }

        public TblUserTO SelectTblUserTO(Int32 idUser,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.SelectTblUser(idUser, conn, tran);

        }

        public TblUserTO SelectTblUserTO(String userID,String password)
        {
            return _iTblUserDAO.SelectTblUser(userID,password);
        }

        public Boolean IsThisUserExists(String userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                Boolean result = IsThisUserExists(userId, conn, tran);
                tran.Commit();
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public Boolean IsThisUserExists(String userId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.IsThisUserExists(userId, conn,tran);
        }

        public Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserMobileNoDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }
        // added by aniket for email configuration
        public Dictionary<int, string> SelectUserEmailDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserEmailDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }
        public Dictionary<int, List<string>> SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }

        public Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserDeviceRegNoDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }
        public Dictionary<int, string> SelectUserUsingRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserUsingRole(userOrRoleIds, isUser, conn, tran);

        }
        public List<TblUserTO> SelectAllTblUserList(Int32 orgId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.SelectAllTblUser(orgId,conn,tran);
        }

        public List<DropDownTO> SelectAllActiveUsersForDropDown()
        {
            return _iTblUserDAO.SelectAllActiveUsersForDropDown();
        }

        //Sudhir[08-MAR-2018] Added for Get Users for Organizations Structre Based on User ID's
        public List<DropDownTO> SelectUsersOnUserIds(string userIds)
        {
            return _iTblUserDAO.SelectUsersOnUserIds(userIds);
        }


        public List<TblUserTO> SelectAllTblUserByRoleType(Int32 roleTypeId)
        {
            return _iTblUserDAO.SelectAllTblUserByRoleType(roleTypeId);
        }


        #endregion

        #region Insertion
        public int InsertTblUser(TblUserTO tblUserTO)
        {
            return _iTblUserDAO.InsertTblUser(tblUserTO);
        }

        public int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.InsertTblUser(tblUserTO, conn, tran);
        }

        public String CreateUserName(string firstName,string lastName, SqlConnection conn,SqlTransaction tran)
        {
            try
            {
                String userName = string.Empty;
                userName = firstName.TrimEnd(' ') + "." + lastName.TrimEnd(' ');
                Boolean isUserExist = true;
                for (int i = 0; i < 5; i++) //Max 5 Is Considered
                {
                    if(i==0)
                    {
                        isUserExist = IsThisUserExists(userName, conn, tran);
                        if (!isUserExist)
                            return userName;
                        else continue;
                    }
                    else
                    {
                        string newUser = userName + i;
                        isUserExist = IsThisUserExists(newUser, conn, tran);
                        if (!isUserExist)
                            return newUser;
                        else continue;
                    }
                }

                userName = userName + _iCommon.ServerDateTime.ToString("ddMMyyyy");
                return userName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public Boolean GetUsersQuata(SqlConnection conn, SqlTransaction tran)
        {
            Boolean isValid = true;
            dimUserConfigrationTO dimUserConfigrationTO = _iTblLoginDAO.GetUsersConfigration((int)Constants.UsersConfigration.USER_CONFIG, conn, tran);
            if (dimUserConfigrationTO != null)
            {
                List<TblUserTO> list = _iTblUserDAO.SelectAllTblUser(true);
                if (list != null && list.Count > 0)
                {
                    if (Convert.ToInt32(dimUserConfigrationTO.ConfigValue) <= list.Count)
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
        public ResultMessage SaveNewUser(TblUserTO tblUserTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                #region Check User Creation Limit Added By @KM 13/02/2019
                Boolean isProceedToCreate = GetUsersQuata(conn, tran);
                if(!isProceedToCreate)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = "User Quota Exceeded, Please contact your administrative";
                    rMessage.Text = "User Quota Exceeded, Please contact your administrative";
                    return rMessage;
                }
                #endregion
                String userId = CreateUserName(tblUserTO.UserPersonTO.FirstName, tblUserTO.UserPersonTO.LastName, conn, tran);
                userId = userId.ToLower();
                String pwd = Constants.DefaultPassword;

                if (tblUserTO.UserPersonTO.DobDay > 0 && tblUserTO.UserPersonTO.DobMonth > 0 && tblUserTO.UserPersonTO.DobYear > 0)
                {
                    tblUserTO.UserPersonTO.DateOfBirth = new DateTime(tblUserTO.UserPersonTO.DobYear, tblUserTO.UserPersonTO.DobMonth, tblUserTO.UserPersonTO.DobDay);
                }
                else
                {
                    tblUserTO.UserPersonTO.DateOfBirth = DateTime.MinValue;
                }

                tblUserTO.UserPersonTO.CreatedBy = loginUserId;
                tblUserTO.UserPersonTO.CreatedOn = serverDateTime;
                int result = _iTblPersonDAO.InsertTblPerson(tblUserTO.UserPersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While InsertTblPerson for Users in Method SaveNewUser ";
                    return rMessage;
                }

                tblUserTO.UserDisplayName = tblUserTO.UserPersonTO.FirstName + " " + tblUserTO.UserPersonTO.LastName;
                tblUserTO.IsActive = 1;
                tblUserTO.UserLogin = userId;
                tblUserTO.UserPasswd = pwd;
                result =InsertTblUser(tblUserTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While InsertTblUser for Users in Method SaveNewUser ";
                    return rMessage;
                }

                tblUserTO.UserExtTO = new TblUserExtTO();
                tblUserTO.UserExtTO.CreatedBy = loginUserId;
                tblUserTO.UserExtTO.CreatedOn = serverDateTime;
                tblUserTO.UserExtTO.PersonId = tblUserTO.UserPersonTO.IdPerson;
                tblUserTO.UserExtTO.UserId = tblUserTO.IdUser;
                tblUserTO.UserExtTO.OrganizationId = tblUserTO.OrganizationId;

                TblAddressTO addressTO = _iTblAddressDAO.SelectOrgAddressWrtAddrType(tblUserTO.OrganizationId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                if (addressTO == null)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error..Record could not be saved. Address Details for the organization + " + tblUserTO.OrganizationName + " is not set";
                    rMessage.DisplayMessage = "Error..Record could not be saved. Address Details for the organization + " + tblUserTO.OrganizationName + " is not set";
                    return rMessage;
                }
                tblUserTO.UserExtTO.AddressId = addressTO.IdAddr;

                result = _iTblUserExtDAO.InsertTblUserExt(tblUserTO.UserExtTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUserExt for Users in Method SaveNewUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                if (tblUserTO.OrgStructId != 0 && tblUserTO.OrgStructId > 0)
                {
                    TblUserReportingDetailsTO tblUserReportingDetailsTO = new TblUserReportingDetailsTO();
                    tblUserReportingDetailsTO.CreatedBy = loginUserId;
                    tblUserReportingDetailsTO.OrgStructureId = tblUserTO.OrgStructId;
                    tblUserReportingDetailsTO.ReportingTo = tblUserTO.ReportingTo;
                    tblUserReportingDetailsTO.UserId = tblUserTO.IdUser;
                    tblUserReportingDetailsTO.CreatedOn = serverDateTime;
                    tblUserReportingDetailsTO.LevelId = tblUserTO.LevelId;
                    tblUserReportingDetailsTO.IsActive = 1;
                    rMessage = _iTblOrgStructureBL.AttachNewUserToOrgStructure(tblUserReportingDetailsTO,null, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        rMessage.Text = "Error While Attch New User to Organization Structure in AttachNewUserToOrgStructure Method";
                        return rMessage;
                    }
                }
                else
                {
                    tblUserTO.UserRoleList[0].UserId = tblUserTO.IdUser;
                    tblUserTO.UserRoleList[0].IsActive = 1;
                    tblUserTO.UserRoleList[0].CreatedBy = loginUserId;
                    tblUserTO.UserRoleList[0].CreatedOn = serverDateTime;

                    result = _iTblUserRoleBL.InsertTblUserRole(tblUserTO.UserRoleList[0], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        rMessage.Text = "Error While InsertTblUserRole for C&F Users in Method SaveNewUser ";
                        return rMessage;
                    }
                }

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                rMessage.Text = "Error While InsertTblUserRole for C&F Users in Method SaveNewUser ";
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        
        #region Updation
        public int UpdateTblUser(TblUserTO tblUserTO)
        {
            return _iTblUserDAO.UpdateTblUser(tblUserTO);
        }

        /// <summary>
        /// Saket [2018-03-06] Added to save the history for PWd
        /// </summary>
        /// <param name="tblUserTO"></param>
        /// <param name="tblUserPwdHistoryTO"></param>
        /// <returns></returns>
        public ResultMessage UpdateTblUser(TblUserTO tblUserTO, TblUserPwdHistoryTO tblUserPwdHistoryTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                int result = UpdateTblUser(tblUserTO, conn, tran);
                if (result != 1)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Result = 0;
                    rMessage.Text = "Error In Updating Password";
                    return rMessage;
                }

                if (tblUserPwdHistoryTO != null)
                {
                    result = _iTblUserPwdHistoryDAO.InsertTblUserPwdHistory(tblUserPwdHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Result = 0;
                        rMessage.Text = "Error In Inserting Password history";
                        return rMessage;
                    }
                }


                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.Text = "Exception Error While UpdateUser Method UpdateUser";
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.UpdateTblUser(tblUserTO, conn, tran);
        }

        public ResultMessage UpdateUser(TblUserTO tblUserTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = _iCommon.ServerDateTime;
            try
            {
                  // add Logout Code For Session Out
                         string LoginIds="";
                    if(tblUserTO.IsSetDeviceId)
                    {
                         LoginIds= _iCommon.SelectApKLoginArray(tblUserTO.IdUser);
                    }
              
            // end
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                if (tblUserTO.UserPersonTO != null)
                {
                    if (tblUserTO.UserPersonTO.DobDay > 0 && tblUserTO.UserPersonTO.DobMonth > 0 && tblUserTO.UserPersonTO.DobYear > 0)
                    {
                        tblUserTO.UserPersonTO.DateOfBirth = new DateTime(tblUserTO.UserPersonTO.DobYear, tblUserTO.UserPersonTO.DobMonth, tblUserTO.UserPersonTO.DobDay);
                    }
                    else
                    {
                        tblUserTO.UserPersonTO.DateOfBirth = DateTime.MinValue;
                    }

                    result = _iTblPersonDAO.UpdateTblPerson(tblUserTO.UserPersonTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While UpdateTblPerson for Users in Method UpdateUser ";
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return rMessage;
                    }

                    tblUserTO.UserDisplayName = tblUserTO.UserPersonTO.FirstName + " " + tblUserTO.UserPersonTO.LastName;

                }

                result = UpdateTblUser(tblUserTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUser for Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                tblUserTO.UserExtTO.PersonId = tblUserTO.UserPersonTO.IdPerson;
                tblUserTO.UserExtTO.UserId = tblUserTO.IdUser;
                tblUserTO.UserExtTO.OrganizationId = tblUserTO.OrganizationId;

                result = _iTblUserExtDAO.UpdateTblUserExt(tblUserTO.UserExtTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUserExt for Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                result = _iTblUserRoleBL.UpdateTblUserRole(tblUserTO.UserRoleList[0], conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While UpdateTblUserRole for C&F Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;

                    return rMessage;
                }



                //Hrushikesh Commented for update User

                //Update User Reporting Details
                //if (tblUserTO.UserReportingId > 0)
                //{
                //    TblUserReportingDetailsTO tblUserReportingDetailsTO = _iTblOrgStructureBL.SelectUserReportingDetailsTO(tblUserTO.UserReportingId, conn, tran);
                //    if (tblUserReportingDetailsTO != null)
                //    {
                //        tblUserReportingDetailsTO.UpdatedBy = loginUserId;
                //        tblUserReportingDetailsTO.UpdatedOn = serverDateTime;
                //        tblUserReportingDetailsTO.UserId = tblUserTO.IdUser;
                //        tblUserReportingDetailsTO.ReportingTo = tblUserTO.ReportingTo;
                //        tblUserReportingDetailsTO.OrgStructureId = tblUserTO.OrgStructId;
                //        tblUserReportingDetailsTO.LevelId = tblUserTO.LevelId;
                //        tblUserReportingDetailsTO.IsActive = 1;
                //        result = _iTblOrgStructureBL.UpdateUserReportingDetail(tblUserReportingDetailsTO, conn, tran);
                //        if (result != 1)
                //        {
                //            tran.Rollback();
                //            rMessage.MessageType = ResultMessageE.Error;
                //            rMessage.Text = "Error While UpdateTblUserRole for C&F Users in Method UpdateUser ";
                //            rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                //            return rMessage;
                //        }
                //    }
                //}
   // add Logout Code For Session Out

              if(!string.IsNullOrEmpty(LoginIds))
              {
                     string[] x=LoginIds.Split(','); 
                      foreach(string item in x){
                      TblModuleCommHisTO TblModuleCommHis=new TblModuleCommHisTO();
                      TblModuleCommHis.LoginId=Convert.ToInt32(item);
                  result= _iTblModuleBL.UpdateTblModuleCommHisBeforeLoginForAPK(TblModuleCommHis,conn,tran);
                             if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While UpdateTblLogin  Method UpdateUser ";
                            rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return rMessage;
                        }
                          }
              }
            // end
                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.Text = "Exception Error While UpdateUser Method UpdateUser ";
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Deletion
        public int DeleteTblUser(Int32 idUser)
        {
            return _iTblUserDAO.DeleteTblUser(idUser);
        }

        public int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.DeleteTblUser(idUser, conn, tran);
        }

        #endregion
        
    }
}
