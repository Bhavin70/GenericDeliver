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
    public class TblFreightUpdateBL : ITblFreightUpdateBL
    {
        private readonly ITblFreightUpdateDAO _iTblFreightUpdateDAO;
        public TblFreightUpdateBL(ITblFreightUpdateDAO iTblFreightUpdateDAO)
        {
            _iTblFreightUpdateDAO = iTblFreightUpdateDAO;
        }
        #region Selection
        public List<TblFreightUpdateTO> SelectAllTblFreightUpdateList()
        {
            return _iTblFreightUpdateDAO.SelectAllTblFreightUpdate();
        }

        public TblFreightUpdateTO SelectTblFreightUpdateTO(Int32 idFreightUpdate)
        {
            return _iTblFreightUpdateDAO.SelectTblFreightUpdate(idFreightUpdate);
        }

        public List<TblFreightUpdateTO> SelectAllTblFreightUpdateList(DateTime frmDt, DateTime toDt, int districtId, int talukaId)
        {
            return _iTblFreightUpdateDAO.SelectAllTblFreightUpdate(frmDt,toDt, districtId,talukaId);

        }


        #endregion

        #region Insertion
        public int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO)
        {
            return _iTblFreightUpdateDAO.InsertTblFreightUpdate(tblFreightUpdateTO);
        }

        public int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFreightUpdateDAO.InsertTblFreightUpdate(tblFreightUpdateTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO)
        {
            return _iTblFreightUpdateDAO.UpdateTblFreightUpdate(tblFreightUpdateTO);
        }

        public int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFreightUpdateDAO.UpdateTblFreightUpdate(tblFreightUpdateTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblFreightUpdate(Int32 idFreightUpdate)
        {
            return _iTblFreightUpdateDAO.DeleteTblFreightUpdate(idFreightUpdate);
        }

        public int DeleteTblFreightUpdate(Int32 idFreightUpdate, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFreightUpdateDAO.DeleteTblFreightUpdate(idFreightUpdate, conn, tran);
        }

       
        #endregion

    }
}
