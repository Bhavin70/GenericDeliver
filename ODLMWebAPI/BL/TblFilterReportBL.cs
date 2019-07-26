using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblFilterReportBL : ITblFilterReportBL
    {
        private readonly ITblFilterReportDAO _iTblFilterReportDAO;
        public TblFilterReportBL(ITblFilterReportDAO iTblFilterReportDAO)
        {
            _iTblFilterReportDAO = iTblFilterReportDAO;
        }
        #region Selection
        public List<TblFilterReportTO> SelectAllTblFilterReport()
        {
            return _iTblFilterReportDAO.SelectAllTblFilterReport();
        }

        public List<TblFilterReportTO> SelectAllTblFilterReportList()
        {
            List<TblFilterReportTO> tblFilterReportTODT = _iTblFilterReportDAO.SelectAllTblFilterReport();
            return tblFilterReportTODT;
        }

        public TblFilterReportTO SelectTblFilterReportTO(Int32 idFilterReport)
        {
            TblFilterReportTO tblFilterReportTODT = _iTblFilterReportDAO.SelectTblFilterReport(idFilterReport);
            if (tblFilterReportTODT != null)
                return tblFilterReportTODT;
            else
                return null;
        }

        public List<TblFilterReportTO> SelectTblFilterReportList(Int32 reportId)
        {
            List<TblFilterReportTO> tblFilterReportTODTList = _iTblFilterReportDAO.SelectTblFilterReportList(reportId);
            if (tblFilterReportTODTList != null)
                return tblFilterReportTODTList;
            else
                return null;
        }




        #endregion

        #region Insertion
        public int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO)
        {
            return _iTblFilterReportDAO.InsertTblFilterReport(tblFilterReportTO);
        }

        public int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFilterReportDAO.InsertTblFilterReport(tblFilterReportTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO)
        {
            return _iTblFilterReportDAO.UpdateTblFilterReport(tblFilterReportTO);
        }

        public int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFilterReportDAO.UpdateTblFilterReport(tblFilterReportTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblFilterReport(Int32 idFilterReport)
        {
            return _iTblFilterReportDAO.DeleteTblFilterReport(idFilterReport);
        }

        public int DeleteTblFilterReport(Int32 idFilterReport, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFilterReportDAO.DeleteTblFilterReport(idFilterReport, conn, tran);
        }

        #endregion
    }
}
