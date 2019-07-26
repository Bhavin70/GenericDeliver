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
    public class TblFreightUpdateBL : ITblFreightUpdateBL
    {
        #region Selection
        
        public List<TblFreightUpdateTO> SelectAllTblFreightUpdateList()
        {
            return TblFreightUpdateDAO.SelectAllTblFreightUpdate();
        }

        public TblFreightUpdateTO SelectTblFreightUpdateTO(Int32 idFreightUpdate)
        {
            return TblFreightUpdateDAO.SelectTblFreightUpdate(idFreightUpdate);
        }

        internal static List<TblFreightUpdateTO> SelectAllTblFreightUpdateList(DateTime frmDt, DateTime toDt, int districtId, int talukaId)
        {
            return TblFreightUpdateDAO.SelectAllTblFreightUpdate(frmDt,toDt, districtId,talukaId);

        }


        #endregion

        #region Insertion
        public int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO)
        {
            return TblFreightUpdateDAO.InsertTblFreightUpdate(tblFreightUpdateTO);
        }

        public int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFreightUpdateDAO.InsertTblFreightUpdate(tblFreightUpdateTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO)
        {
            return TblFreightUpdateDAO.UpdateTblFreightUpdate(tblFreightUpdateTO);
        }

        public int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFreightUpdateDAO.UpdateTblFreightUpdate(tblFreightUpdateTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblFreightUpdate(Int32 idFreightUpdate)
        {
            return TblFreightUpdateDAO.DeleteTblFreightUpdate(idFreightUpdate);
        }

        public int DeleteTblFreightUpdate(Int32 idFreightUpdate, SqlConnection conn, SqlTransaction tran)
        {
            return TblFreightUpdateDAO.DeleteTblFreightUpdate(idFreightUpdate, conn, tran);
        }

       
        #endregion

    }
}
