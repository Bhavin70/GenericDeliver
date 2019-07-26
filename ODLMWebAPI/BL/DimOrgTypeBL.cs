using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.BL
{
    public class DimOrgTypeBL : IDimOrgTypeBL
    {
        #region Selection
        private readonly IDimOrgTypeDAO _iDimOrgTypeDAO;
        private readonly IConnectionString _iConnectionString;
        public DimOrgTypeBL(IDimOrgTypeDAO iDimOrgTypeDAO, IConnectionString iConnectionString)
        {
            _iDimOrgTypeDAO = iDimOrgTypeDAO;
            _iConnectionString = iConnectionString;
        }
        public List<DimOrgTypeTO> SelectAllDimOrgTypeList()
        {
            return _iDimOrgTypeDAO.SelectAllDimOrgType();
        }

        public DimOrgTypeTO SelectDimOrgTypeTO(Int32 idOrgType,SqlConnection conn,SqlTransaction tran)
        {
           return _iDimOrgTypeDAO.SelectDimOrgType(idOrgType,conn,tran);
        }

        public DimOrgTypeTO SelectDimOrgTypeTO(Int32 idOrgType)
        {
            SqlConnection connection = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                return _iDimOrgTypeDAO.SelectDimOrgType(idOrgType, connection, transaction);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #region Insertion
        public int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO)
        {
            return _iDimOrgTypeDAO.InsertDimOrgType(dimOrgTypeTO);
        }

        public int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimOrgTypeDAO.InsertDimOrgType(dimOrgTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO)
        {
            return _iDimOrgTypeDAO.UpdateDimOrgType(dimOrgTypeTO);
        }

        public int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimOrgTypeDAO.UpdateDimOrgType(dimOrgTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimOrgType(Int32 idOrgType)
        {
            return _iDimOrgTypeDAO.DeleteDimOrgType(idOrgType);
        }

        public int DeleteDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimOrgTypeDAO.DeleteDimOrgType(idOrgType, conn, tran);
        }

        #endregion
        
    }
}
