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
    public class TblVisitIssueReasonsBL : ITblVisitIssueReasonsBL
    {
        private readonly ITblVisitIssueReasonsDAO _iTblVisitIssueReasonsDAO;
        public TblVisitIssueReasonsBL(ITblVisitIssueReasonsDAO iTblVisitIssueReasonsDAO)
        {
            _iTblVisitIssueReasonsDAO = iTblVisitIssueReasonsDAO;
        }
        #region Selection
        public DataTable SelectAllTblVisitIssueReasons()
        {
            return _iTblVisitIssueReasonsDAO.SelectAllTblVisitIssueReasons();
        }

        public List<TblVisitIssueReasonsTO> SelectAllTblVisitIssueReasonsList()
        {
            DataTable tblVisitIssueReasonsTODT = _iTblVisitIssueReasonsDAO.SelectAllTblVisitIssueReasons();
            return ConvertDTToList(tblVisitIssueReasonsTODT);
        }

        public List<TblVisitIssueReasonsTO> SelectAllVisitIssueReasonsListForDropDown()
        {
            List<TblVisitIssueReasonsTO> visitIssueReasonTOList = _iTblVisitIssueReasonsDAO.SelectAllTblVisitIssueReasonsForDropDOwn();
            if (visitIssueReasonTOList != null)
                return visitIssueReasonTOList;
            else
                return null;
        }

        public TblVisitIssueReasonsTO SelectTblVisitIssueReasonsTO(Int32 idVisitIssueReasons)
        {
            DataTable tblVisitIssueReasonsTODT = _iTblVisitIssueReasonsDAO.SelectTblVisitIssueReasons(idVisitIssueReasons);
            List<TblVisitIssueReasonsTO> tblVisitIssueReasonsTOList = ConvertDTToList(tblVisitIssueReasonsTODT);
            if (tblVisitIssueReasonsTOList != null && tblVisitIssueReasonsTOList.Count == 1)
                return tblVisitIssueReasonsTOList[0];
            else
                return null;
        }

        public List<TblVisitIssueReasonsTO> ConvertDTToList(DataTable tblVisitIssueReasonsTODT)
        {
            List<TblVisitIssueReasonsTO> tblVisitIssueReasonsTOList = new List<TblVisitIssueReasonsTO>();
            if (tblVisitIssueReasonsTODT != null)
            {
                //for (int rowCount = 0; rowCount < tblVisitIssueReasonsTODT.Rows.Count; rowCount++)
                //{
                //    TblVisitIssueReasonsTO tblVisitIssueReasonsTONew = new TblVisitIssueReasonsTO();
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["idVisitIssueReasons"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.IdVisitIssueReasons = Convert.ToInt32(tblVisitIssueReasonsTODT.Rows[rowCount]["idVisitIssueReasons"].ToString());
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["issueTypeId"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.IssueTypeId = Convert.ToInt32(tblVisitIssueReasonsTODT.Rows[rowCount]["issueTypeId"].ToString());
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.IsActive = Convert.ToInt32(tblVisitIssueReasonsTODT.Rows[rowCount]["isActive"].ToString());
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["visitIssueReasonName"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.VisitIssueReasonName = Convert.ToString(tblVisitIssueReasonsTODT.Rows[rowCount]["visitIssueReasonName"].ToString());
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["visitIssueReasonDesc"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.VisitIssueReasonDesc = Convert.ToString(tblVisitIssueReasonsTODT.Rows[rowCount]["visitIssueReasonDesc"].ToString());
                //    tblVisitIssueReasonsTOList.Add(tblVisitIssueReasonsTONew);
                //}
            }
            return tblVisitIssueReasonsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblVisitIssueReasons( TblVisitIssueReasonsTO tblVisitIssueReasonsTO)
        {
            return _iTblVisitIssueReasonsDAO.InsertTblVisitIssueReasons(tblVisitIssueReasonsTO);
        }

        public int InsertTblVisitIssueReasons(ref TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitIssueReasonsDAO.InsertTblVisitIssueReasons(ref tblVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO)
        {
            return _iTblVisitIssueReasonsDAO.UpdateTblVisitIssueReasons(tblVisitIssueReasonsTO);
        }

        public int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitIssueReasonsDAO.UpdateTblVisitIssueReasons(tblVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons)
        {
            return _iTblVisitIssueReasonsDAO.DeleteTblVisitIssueReasons(idVisitIssueReasons);
        }

        public int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitIssueReasonsDAO.DeleteTblVisitIssueReasons(idVisitIssueReasons, conn, tran);
        }

        #endregion

    }
}
