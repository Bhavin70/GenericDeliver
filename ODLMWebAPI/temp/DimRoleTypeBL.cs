using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class DimRoleTypeBL : IDimRoleTypeBL
    {
        #region Selection

        public List<DimRoleTypeTO> SelectAllDimRoleTypeList()
        {
            return  DimRoleTypeDAO.SelectAllDimRoleTypeList();
        }

        public DimRoleTypeTO SelectDimRoleTypeTO(Int32 idRoleType)
        {
            return  DimRoleTypeDAO.SelectDimRoleType(idRoleType);           
        }

       

        #endregion
        
        #region Insertion
        public int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO)
        {
            return DimRoleTypeDAO.InsertDimRoleType(dimRoleTypeTO);
        }

        public int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimRoleTypeDAO.InsertDimRoleType(dimRoleTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO)
        {
            return DimRoleTypeDAO.UpdateDimRoleType(dimRoleTypeTO);
        }

        public int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimRoleTypeDAO.UpdateDimRoleType(dimRoleTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimRoleType(Int32 idRoleType)
        {
            return DimRoleTypeDAO.DeleteDimRoleType(idRoleType);
        }

        public int DeleteDimRoleType(Int32 idRoleType, SqlConnection conn, SqlTransaction tran)
        {
            return DimRoleTypeDAO.DeleteDimRoleType(idRoleType, conn, tran);
        }

        #endregion
        
    }
}
