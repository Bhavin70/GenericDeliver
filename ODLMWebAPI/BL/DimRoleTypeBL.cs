using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class DimRoleTypeBL : IDimRoleTypeBL
    {
        #region Selection
        private readonly IDimRoleTypeDAO _iDimRoleTypeDAO;
        public DimRoleTypeBL(IDimRoleTypeDAO iDimRoleTypeDAO)
        {
            _iDimRoleTypeDAO = iDimRoleTypeDAO;
        }
        public List<DimRoleTypeTO> SelectAllDimRoleTypeList()
        {
            return  _iDimRoleTypeDAO.SelectAllDimRoleTypeList();
        }

        public DimRoleTypeTO SelectDimRoleTypeTO(Int32 idRoleType)
        {
            return  _iDimRoleTypeDAO.SelectDimRoleType(idRoleType);           
        }

       

        #endregion
        
        #region Insertion
        public int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO)
        {
            return _iDimRoleTypeDAO.InsertDimRoleType(dimRoleTypeTO);
        }

        public int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimRoleTypeDAO.InsertDimRoleType(dimRoleTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO)
        {
            return _iDimRoleTypeDAO.UpdateDimRoleType(dimRoleTypeTO);
        }

        public int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimRoleTypeDAO.UpdateDimRoleType(dimRoleTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimRoleType(Int32 idRoleType)
        {
            return _iDimRoleTypeDAO.DeleteDimRoleType(idRoleType);
        }

        public int DeleteDimRoleType(Int32 idRoleType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimRoleTypeDAO.DeleteDimRoleType(idRoleType, conn, tran);
        }

        #endregion
        
    }
}
