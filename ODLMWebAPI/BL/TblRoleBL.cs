using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblRoleBL : ITblRoleBL
    {
        private readonly ITblRoleDAO _iTblRoleDAO;
        public TblRoleBL(ITblRoleDAO iTblRoleDAO)
        {
            _iTblRoleDAO = iTblRoleDAO;
        }
        #region Selection
        public TblRoleTO SelectAllTblRole()
        {
            return _iTblRoleDAO.SelectAllTblRole();
        }

        public List<TblRoleTO> SelectAllTblRoleList()
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.SelectAllTblRole();
            return ConvertDTToList(tblRoleTODT);
        }

        public TblRoleTO SelectTblRoleTO(Int32 idRole)
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.SelectTblRole(idRole);
            List<TblRoleTO> tblRoleTOList = ConvertDTToList(tblRoleTODT);
            if (tblRoleTOList != null && tblRoleTOList.Count == 1)
                return tblRoleTOList[0];
            else
                return null;
        }

        public List<TblRoleTO> ConvertDTToList(TblRoleTO tblRoleTODT)
        {
            List<TblRoleTO> tblRoleTOList = new List<TblRoleTO>();
            if (tblRoleTODT != null)
            {
            }
            return tblRoleTOList;
        }

        public TblRoleTO SelectTblRoleOnOrgStructureId(Int32 orgStructutreId)
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.SelectTblRoleOnOrgStructureId(orgStructutreId);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }


        /// <summary>
        /// Sudhir[22-AUG-2018] Added Connection ,Transaction
        /// </summary>
        /// <param name="orgStructutreId"></param>
        /// <returns></returns>
        public TblRoleTO SelectTblRoleOnOrgStructureId(Int32 orgStructutreId, SqlConnection conn, SqlTransaction tran)
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.SelectTblRoleOnOrgStructureId(orgStructutreId, conn, tran);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }

        public TblRoleTO getDepartmentIdFromUserId(Int32 userId)
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.getDepartmentIdFromUserId(userId);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }


        #endregion

        #region Insertion
        public int InsertTblRole(TblRoleTO tblRoleTO)
        {
            return _iTblRoleDAO.InsertTblRole(tblRoleTO);
        }

        public int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.InsertTblRole(tblRoleTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblRole(TblRoleTO tblRoleTO)
        {
            return _iTblRoleDAO.UpdateTblRole(tblRoleTO);
        }

        public int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.UpdateTblRole(tblRoleTO, conn, tran);
        }


        public ResultMessage UpdateRoleType(TblOrgStructureTO tblOrgStructureTO)
        {
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();

            TblRoleTO tblRoleTO = SelectTblRoleOnOrgStructureId(tblOrgStructureTO.IdOrgStructure);
            if (tblRoleTO != null)
            {
                tblRoleTO.RoleTypeId = tblOrgStructureTO.RoleTypeId;
                result = UpdateTblRole(tblRoleTO);
                if (result < 1)
                {
                    //tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Updating Role Type");
                   // return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
               // return resultMessage;
            }
            return resultMessage;
        }
        #endregion

        #region Deletion
        public int DeleteTblRole(Int32 idRole)
        {
            return _iTblRoleDAO.DeleteTblRole(idRole);
        }

        public int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.DeleteTblRole(idRole, conn, tran);
        }

        #endregion

    }
}
