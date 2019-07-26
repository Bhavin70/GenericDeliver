using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TblRoleOrgSettingBL : ITblRoleOrgSettingBL
    {
          
        private readonly ITblRoleOrgSettingDAO _iTblRoleOrgSettingDAO;
        private readonly IConnectionString _iConnectionString;
        public TblRoleOrgSettingBL(IConnectionString iConnectionString, ITblRoleOrgSettingDAO iTblRoleOrgSettingDAO)
        {
            _iTblRoleOrgSettingDAO = iTblRoleOrgSettingDAO;
            _iConnectionString = iConnectionString;
        }
        public ResultMessage SaveRolesAndOrg(List<RoleOrgTO> roleOrgTOList)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Int32 updateOrCreatedUser = 0;
            try
            {
                conn.Open();

                if (roleOrgTOList == null || roleOrgTOList.Count == 0)
                {
                    resultMessage.Text = "Error,roleOrgTOList Found NULL : Method SaveRolesAndOrg";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                #region Save List 

                for (int i = 0; i < roleOrgTOList.Count; i++)
                {
                    int res = _iTblRoleOrgSettingDAO.CheckIfExists(roleOrgTOList[i]);

                    if (res != 0)
                    {
                        result = _iTblRoleOrgSettingDAO.UpdateRolesAndOrg(roleOrgTOList[i], conn);
                    }
                    else
                    {
                        result = _iTblRoleOrgSettingDAO.SaveRolesAndOrg(roleOrgTOList[i], conn);
                    }
                    if (result != 1)
                    {
                        resultMessage.Text = "Error While InsertTblLoadingQuotaDeclaration : ConfirmStockSummary";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }

                #endregion

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.DisplayMessage = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : ConfirmStockSummary";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<DropDownTO> SelectAllSystemRoleOrgListForDropDown(int visitTypeId, int personTypeId)
        {
            return _iTblRoleOrgSettingDAO.SelectAllSystemRoleOrgListForDropDown(visitTypeId, personTypeId);
        }

        public List<DropDownTO> SelectSavedRoles(int visitTypeId, int personTypeId)
        {
            return _iTblRoleOrgSettingDAO.SelectSavedRoles(visitTypeId, personTypeId);

        }

        public List<DropDownTO> SelectSavedOrg(int visitTypeId, int personTypeId)
        {
            return _iTblRoleOrgSettingDAO.SelectSavedOrg(visitTypeId, personTypeId);

        }
    }
}
