using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class DimOrgTypeBL : IDimOrgTypeBL
    {
        #region Selection

        public List<DimOrgTypeTO> SelectAllDimOrgTypeList()
        {
            return DimOrgTypeDAO.SelectAllDimOrgType();
        }

        public DimOrgTypeTO SelectDimOrgTypeTO(Int32 idOrgType,SqlConnection conn,SqlTransaction tran)
        {
           return DimOrgTypeDAO.SelectDimOrgType(idOrgType,conn,tran);
        }

        public DimOrgTypeTO SelectDimOrgTypeTO(Int32 idOrgType)
        {
            SqlConnection connection = new SqlConnection(Startup.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                return DimOrgTypeDAO.SelectDimOrgType(idOrgType, connection, transaction);
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
            return DimOrgTypeDAO.InsertDimOrgType(dimOrgTypeTO);
        }

        public int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimOrgTypeDAO.InsertDimOrgType(dimOrgTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO)
        {
            return DimOrgTypeDAO.UpdateDimOrgType(dimOrgTypeTO);
        }

        public int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimOrgTypeDAO.UpdateDimOrgType(dimOrgTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimOrgType(Int32 idOrgType)
        {
            return DimOrgTypeDAO.DeleteDimOrgType(idOrgType);
        }

        public int DeleteDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran)
        {
            return DimOrgTypeDAO.DeleteDimOrgType(idOrgType, conn, tran);
        }

        #endregion
        
    }
}
