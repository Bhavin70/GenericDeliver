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

namespace ODLMWebAPI.BL
{
    public class DimStatusBL : IDimStatusBL
    {
        #region Selection
        public List<DimStatusTO> SelectAllDimStatusList()
        {
            return DimStatusDAO.SelectAllDimStatus();
        }

        /// <summary>
        /// Sanjay [2017-03-07] Returns list of status against given transaction type
        /// If param value= 0 then return all statuses
        /// </summary>
        /// <param name="txnTypeId"></param>
        /// <returns></returns>
        public List<DimStatusTO> SelectAllDimStatusList(Int32 txnTypeId)
        {
            return DimStatusDAO.SelectAllDimStatus(txnTypeId);
        }

        public DimStatusTO SelectDimStatusTO(Int32 idStatus)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return DimStatusDAO.SelectDimStatus(idStatus, conn, tran);
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
            return DimStatusDAO.InsertDimStatus(dimStatusTO);
        }

        public int InsertDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimStatusDAO.InsertDimStatus(dimStatusTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimStatus(DimStatusTO dimStatusTO)
        {
            return DimStatusDAO.UpdateDimStatus(dimStatusTO);
        }

        public int UpdateDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimStatusDAO.UpdateDimStatus(dimStatusTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimStatus(Int32 idStatus)
        {
            return DimStatusDAO.DeleteDimStatus(idStatus);
        }

        public int DeleteDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran)
        {
            return DimStatusDAO.DeleteDimStatus(idStatus, conn, tran);
        }

        #endregion
        
    }
}
