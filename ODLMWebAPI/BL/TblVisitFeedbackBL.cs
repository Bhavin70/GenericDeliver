using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblVisitFeedbackBL : ITblVisitFeedbackBL
    {
        private readonly ITblVisitFeedbackDAO _iTblVisitFeedbackDAO;
        public TblVisitFeedbackBL(ITblVisitFeedbackDAO iTblVisitFeedbackDAO)
        {
            _iTblVisitFeedbackDAO = iTblVisitFeedbackDAO;
        }
        #region Selection
        public DataTable SelectAllTblVisitFeedback()
        {
            return _iTblVisitFeedbackDAO.SelectAllTblVisitFeedback();
        }

        public List<TblVisitFeedbackTO> SelectAllTblVisitFeedbackList()
        {
            DataTable tblVisitFeedbackTODT = _iTblVisitFeedbackDAO.SelectAllTblVisitFeedback();
            return ConvertDTToList(tblVisitFeedbackTODT);
        }

        public TblVisitFeedbackTO SelectTblVisitFeedbackTO()
        {
            DataTable tblVisitFeedbackTODT = _iTblVisitFeedbackDAO.SelectTblVisitFeedback();
            List<TblVisitFeedbackTO> tblVisitFeedbackTOList = ConvertDTToList(tblVisitFeedbackTODT);
            if (tblVisitFeedbackTOList != null && tblVisitFeedbackTOList.Count == 1)
                return tblVisitFeedbackTOList[0];
            else
                return null;
        }

        public List<TblVisitFeedbackTO> ConvertDTToList(DataTable tblVisitFeedbackTODT)
        {
            List<TblVisitFeedbackTO> tblVisitFeedbackTOList = new List<TblVisitFeedbackTO>();
            if (tblVisitFeedbackTODT != null)
            {

            }
            return tblVisitFeedbackTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO)
        {
            return _iTblVisitFeedbackDAO.InsertTblVisitFeedback(tblVisitFeedbackTO);
        }

        public int InsertTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitFeedbackDAO.InsertTblVisitFeedback(tblVisitFeedbackTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO)
        {
            return _iTblVisitFeedbackDAO.UpdateTblVisitFeedback(tblVisitFeedbackTO);
        }

        public int UpdateTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitFeedbackDAO.UpdateTblVisitFeedback(tblVisitFeedbackTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblVisitFeedback()
        {
            return _iTblVisitFeedbackDAO.DeleteTblVisitFeedback();
        }

        public int DeleteTblVisitFeedback( SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitFeedbackDAO.DeleteTblVisitFeedback( conn, tran);
        }

        #endregion

    }
}
