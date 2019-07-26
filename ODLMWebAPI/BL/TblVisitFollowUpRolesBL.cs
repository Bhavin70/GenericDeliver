using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblVisitFollowUpRolesBL : ITblVisitFollowUpRolesBL
    {
        private readonly ITblVisitFollowUpRolesDAO _iTblVisitFollowUpRolesDAO;
        public TblVisitFollowUpRolesBL(ITblVisitFollowUpRolesDAO iTblVisitFollowUpRolesDAO)
        {
            _iTblVisitFollowUpRolesDAO = iTblVisitFollowUpRolesDAO;
        }
        #region Selection


        public List<TblVisitFollowUpRolesTO> SelectAllTblVisitFollowUpRolesList()
        {
            List<TblVisitFollowUpRolesTO> visitFollowUpRolesTOList = _iTblVisitFollowUpRolesDAO.SelectAllTblVisitFollowUpRoles();
            if (visitFollowUpRolesTOList != null)
                return visitFollowUpRolesTOList;
            else
                return null;
        }

        public List<DropDownTO> SelectFollowUpUserRoleListForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> followUpUserList= _iTblVisitFollowUpRolesDAO.SelectFollowUpUserRoleListForDropDown();
                if (followUpUserList != null)
                    return followUpUserList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectFollowupRoleList");
                return null;
            }
        }

        public List<DropDownTO> SelectFollowUpRoleListForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> followUpUserList = _iTblVisitFollowUpRolesDAO.SelectFollowUpRoleListForDropDown();
                if (followUpUserList != null)
                    return followUpUserList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectFollowupRoleList");
                return null;
            }
        }

        public TblVisitFollowUpRolesTO SelectTblVisitFollowUpRolesTO(Int32 idVisitFollowUpRole)
        {
            DataTable tblVisitFollowUpRolesTODT = _iTblVisitFollowUpRolesDAO.SelectTblVisitFollowUpRoles(idVisitFollowUpRole);
            List<TblVisitFollowUpRolesTO> tblVisitFollowUpRolesTOList = ConvertDTToList(tblVisitFollowUpRolesTODT);
            if (tblVisitFollowUpRolesTOList != null && tblVisitFollowUpRolesTOList.Count == 1)
                return tblVisitFollowUpRolesTOList[0];
            else
                return null;
        }

        public List<TblVisitFollowUpRolesTO> ConvertDTToList(DataTable tblVisitFollowUpRolesTODT)
        {
            List<TblVisitFollowUpRolesTO> tblVisitFollowUpRolesTOList = new List<TblVisitFollowUpRolesTO>();
            if (tblVisitFollowUpRolesTODT != null)
            {

            }
            return tblVisitFollowUpRolesTOList;
        }

        // Vaibhav [08-Nov-2017] added to select visit role list
        public List<DropDownTO> SelectVisitRoleListForDropDown()
        {
            List<DropDownTO> visitRoleList = _iTblVisitFollowUpRolesDAO.SelectVisitRoleForDropDown();
            if (visitRoleList != null)
                return visitRoleList;
            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO)
        {
            return _iTblVisitFollowUpRolesDAO.InsertTblVisitFollowUpRoles(tblVisitFollowUpRolesTO);
        }

        public int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitFollowUpRolesDAO.InsertTblVisitFollowUpRoles(tblVisitFollowUpRolesTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO)
        {
            return _iTblVisitFollowUpRolesDAO.UpdateTblVisitFollowUpRoles(tblVisitFollowUpRolesTO);
        }

        public int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitFollowUpRolesDAO.UpdateTblVisitFollowUpRoles(tblVisitFollowUpRolesTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole)
        {
            return _iTblVisitFollowUpRolesDAO.DeleteTblVisitFollowUpRoles(idVisitFollowUpRole);
        }

        public int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitFollowUpRolesDAO.DeleteTblVisitFollowUpRoles(idVisitFollowUpRole, conn, tran);
        }

        #endregion

    }
}
