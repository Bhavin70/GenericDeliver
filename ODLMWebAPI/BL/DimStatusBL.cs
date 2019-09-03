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
    public class DimStatusBL : IDimStatusBL
    {
        private readonly IDimStatusDAO _iDimStatusDAO;
        private readonly IConnectionString _iConnectionString;
        public DimStatusBL(IDimStatusDAO iDimStatusDAO, IConnectionString iConnectionString)
        {
            _iDimStatusDAO = iDimStatusDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<DimStatusTO> SelectAllDimStatusList()
        {
            return _iDimStatusDAO.SelectAllDimStatus();
        }

        /// <summary>
        /// Sanjay [2017-03-07] Returns list of status against given transaction type
        /// If param value= 0 then return all statuses
        /// </summary>
        /// <param name="txnTypeId"></param>
        /// <returns></returns>
        public List<DimStatusTO> SelectAllDimStatusList(Int32 txnTypeId)
        {
            return _iDimStatusDAO.SelectAllDimStatus(txnTypeId);
        }

        
        public DimStatusTO SelectDimStatusOnOrgId(int orgId)
        {
              return _iDimStatusDAO.SelectDimStatusOnOrgId(orgId);     
        }

        public DimStatusTO SelectDimStatusTO(Int32 idStatus)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iDimStatusDAO.SelectDimStatus(idStatus, conn, tran);
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

        #endregion
        
        #region Insertion
        public int InsertDimStatus(DimStatusTO dimStatusTO)
        {
            return _iDimStatusDAO.InsertDimStatus(dimStatusTO);
        }

        public int InsertDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimStatusDAO.InsertDimStatus(dimStatusTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimStatus(DimStatusTO dimStatusTO)
        {
            return _iDimStatusDAO.UpdateDimStatus(dimStatusTO);
        }

        public int UpdateDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimStatusDAO.UpdateDimStatus(dimStatusTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimStatus(Int32 idStatus)
        {
            return _iDimStatusDAO.DeleteDimStatus(idStatus);
        }

        public int DeleteDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimStatusDAO.DeleteDimStatus(idStatus, conn, tran);
        }

        #endregion
        
    }
}
