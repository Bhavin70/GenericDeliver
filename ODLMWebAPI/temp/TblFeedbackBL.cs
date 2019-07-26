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
    public class TblFeedbackBL : ITblFeedbackBL
    {
        #region Selection

        public List<TblFeedbackTO> SelectAllTblFeedbackList()
        {
            return TblFeedbackDAO.SelectAllTblFeedback();
        }

        public TblFeedbackTO SelectTblFeedbackTO(Int32 idFeedback)
        {
            return  TblFeedbackDAO.SelectTblFeedback(idFeedback);
        }

        internal static List<TblFeedbackTO> SelectAllTblFeedbackList(int userId, DateTime frmDt, DateTime toDt)
        {
            return TblFeedbackDAO.SelectAllTblFeedback(userId,frmDt,toDt);

        }

        #endregion

        #region Insertion
        public int InsertTblFeedback(TblFeedbackTO tblFeedbackTO)
        {
            return TblFeedbackDAO.InsertTblFeedback(tblFeedbackTO);
        }

        public int InsertTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFeedbackDAO.InsertTblFeedback(tblFeedbackTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO)
        {
            return TblFeedbackDAO.UpdateTblFeedback(tblFeedbackTO);
        }

        public int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFeedbackDAO.UpdateTblFeedback(tblFeedbackTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblFeedback(Int32 idFeedback)
        {
            return TblFeedbackDAO.DeleteTblFeedback(idFeedback);
        }

        public int DeleteTblFeedback(Int32 idFeedback, SqlConnection conn, SqlTransaction tran)
        {
            return TblFeedbackDAO.DeleteTblFeedback(idFeedback, conn, tran);
        }

       

        #endregion

    }
}
