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
    public class TblUnLoadingItemDetBL : ITblUnLoadingItemDetBL
    {
        private readonly ITblUnLoadingItemDetDAO _iTblUnLoadingItemDetDAO;
        public TblUnLoadingItemDetBL(ITblUnLoadingItemDetDAO iTblUnLoadingItemDetDAO)
        {
            _iTblUnLoadingItemDetDAO = iTblUnLoadingItemDetDAO;
        }
        #region Selection

        /// <summary>
        /// Vaibhav [13-Sep-2017] Get all unloading item details 
        /// </summary>
        /// <param name="unLoadingId"></param>
        /// <returns></returns>
        public List<TblUnLoadingItemDetTO> SelectAllUnLoadingItemDetailsList(int unLoadingId = 0)
        {
            ResultMessage resultMessage = new ResultMessage();
           try
            {
                return _iTblUnLoadingItemDetDAO.SelectAllTblUnLoadingItemDetails(unLoadingId);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnLoadingItemDetailsList");
                return null;
            }
        }


        public TblUnLoadingItemDetTO SelectTblUnLoadingItemDetTO(Int32 idUnloadingItemDet)
        {
            return _iTblUnLoadingItemDetDAO.SelectTblUnLoadingItemDet(idUnloadingItemDet);
        }
        #endregion

        #region Insertion
        public int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO)
        {
            return _iTblUnLoadingItemDetDAO.InsertTblUnLoadingItemDet(tblUnLoadingItemDetTO);
        }

        public int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUnLoadingItemDetDAO.InsertTblUnLoadingItemDet(tblUnLoadingItemDetTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO)
        {
            return _iTblUnLoadingItemDetDAO.UpdateTblUnLoadingItemDet(tblUnLoadingItemDetTO);
        }

        public int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUnLoadingItemDetDAO.UpdateTblUnLoadingItemDet(tblUnLoadingItemDetTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet)
        {
            return _iTblUnLoadingItemDetDAO.DeleteTblUnLoadingItemDet(idUnloadingItemDet);
        }

        public int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUnLoadingItemDetDAO.DeleteTblUnLoadingItemDet(idUnloadingItemDet, conn, tran);
        }

        #endregion

    }
}
