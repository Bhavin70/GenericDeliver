using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblFilterReportBL : ITblFilterReportBL
    {
        #region Selection
        public List<TblFilterReportTO> SelectAllTblFilterReport()
        {
            return TblFilterReportDAO.SelectAllTblFilterReport();
        }

        public List<TblFilterReportTO> SelectAllTblFilterReportList()
        {
            List<TblFilterReportTO> tblFilterReportTODT = TblFilterReportDAO.SelectAllTblFilterReport();
            return tblFilterReportTODT;
        }

        public TblFilterReportTO SelectTblFilterReportTO(Int32 idFilterReport)
        {
            TblFilterReportTO tblFilterReportTODT = TblFilterReportDAO.SelectTblFilterReport(idFilterReport);
            if (tblFilterReportTODT != null)
                return tblFilterReportTODT;
            else
                return null;
        }

        public List<TblFilterReportTO> SelectTblFilterReportList(Int32 reportId)
        {
            List<TblFilterReportTO> tblFilterReportTODTList = TblFilterReportDAO.SelectTblFilterReportList(reportId);
            if (tblFilterReportTODTList != null)
                return tblFilterReportTODTList;
            else
                return null;
        }




        #endregion

        #region Insertion
        public int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO)
        {
            return TblFilterReportDAO.InsertTblFilterReport(tblFilterReportTO);
        }

        public int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFilterReportDAO.InsertTblFilterReport(tblFilterReportTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO)
        {
            return TblFilterReportDAO.UpdateTblFilterReport(tblFilterReportTO);
        }

        public int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFilterReportDAO.UpdateTblFilterReport(tblFilterReportTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblFilterReport(Int32 idFilterReport)
        {
            return TblFilterReportDAO.DeleteTblFilterReport(idFilterReport);
        }

        public int DeleteTblFilterReport(Int32 idFilterReport, SqlConnection conn, SqlTransaction tran)
        {
            return TblFilterReportDAO.DeleteTblFilterReport(idFilterReport, conn, tran);
        }

        #endregion
    }
}
