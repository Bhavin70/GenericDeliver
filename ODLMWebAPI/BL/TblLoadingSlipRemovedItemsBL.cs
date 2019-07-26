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
    public class TblLoadingSlipRemovedItemsBL : ITblLoadingSlipRemovedItemsBL
    {
        private readonly ITblLoadingSlipRemovedItemsDAO _iTblLoadingSlipRemovedItemsDAO;
        private readonly IConnectionString _iConnectionString;
        public TblLoadingSlipRemovedItemsBL(IConnectionString iConnectionString, ITblLoadingSlipRemovedItemsDAO iTblLoadingSlipRemovedItemsDAO)
        {
            _iTblLoadingSlipRemovedItemsDAO = iTblLoadingSlipRemovedItemsDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblLoadingSlipRemovedItemsTO> SelectAllTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblLoadingSlipRemovedItemsDAO.SelectAllTblLoadingSlipRemovedItems(idLoadingSlipRemovedItems, conn, tran);
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

       
        public List<TblLoadingSlipRemovedItemsTO> SelectAllTblLoadingSlipRemovedItemsList(Int32 idLoadingSlipRemovedItems, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                return _iTblLoadingSlipRemovedItemsDAO.SelectAllTblLoadingSlipRemovedItems(idLoadingSlipRemovedItems, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       

        public TblLoadingSlipRemovedItemsTO SelectTblLoadingSlipRemovedItemsTO(Int32 idLoadingSlipRemovedItems)
        {
            return _iTblLoadingSlipRemovedItemsDAO.SelectTblLoadingSlipRemovedItems(idLoadingSlipRemovedItems);
        }

        #endregion

        #region Insertion
        public int InsertTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO)
        {
            return _iTblLoadingSlipRemovedItemsDAO.InsertTblLoadingSlipRemovedItems(tblLoadingSlipRemovedItemsTO);
        }

        public int InsertTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipRemovedItemsDAO.InsertTblLoadingSlipRemovedItems(tblLoadingSlipRemovedItemsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO)
        {
            return _iTblLoadingSlipRemovedItemsDAO.UpdateTblLoadingSlipRemovedItems(tblLoadingSlipRemovedItemsTO);
        }

        public int UpdateTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipRemovedItemsDAO.UpdateTblLoadingSlipRemovedItems(tblLoadingSlipRemovedItemsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems)
        {
            return _iTblLoadingSlipRemovedItemsDAO.DeleteTblLoadingSlipRemovedItems(idLoadingSlipRemovedItems);
        }

        public int DeleteTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipRemovedItemsDAO.DeleteTblLoadingSlipRemovedItems(idLoadingSlipRemovedItems, conn, tran);
        }

        #endregion
        
    }
}
