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
    public class DimVisitIssueReasonsBL : IDimVisitIssueReasonsBL
    {
        private readonly IDimVisitIssueReasonsDAO _iDimVisitIssueReasonsDAO;
        public DimVisitIssueReasonsBL(IDimVisitIssueReasonsDAO iDimVisitIssueReasonsDAO)
        {
            _iDimVisitIssueReasonsDAO = iDimVisitIssueReasonsDAO;
        }
        #region Selection
        public DataTable SelectAllDimVisitIssueReasons()
        {
            return _iDimVisitIssueReasonsDAO.SelectAllDimVisitIssueReasons();
        }

        public List<DimVisitIssueReasonsTO> SelectAllDimVisitIssueReasonsList()
        {
            DataTable dimVisitIssueReasonsTODT = _iDimVisitIssueReasonsDAO.SelectAllDimVisitIssueReasons();
            return ConvertDTToList(dimVisitIssueReasonsTODT);
        }

        public DimVisitIssueReasonsTO SelectDimVisitIssueReasonsTO(Int32 idVisitIssueReasons)
        {
            DataTable dimVisitIssueReasonsTODT = _iDimVisitIssueReasonsDAO.SelectDimVisitIssueReasons(idVisitIssueReasons);
            List<DimVisitIssueReasonsTO> dimVisitIssueReasonsTOList = ConvertDTToList(dimVisitIssueReasonsTODT);
            if (dimVisitIssueReasonsTOList != null && dimVisitIssueReasonsTOList.Count == 1)
                return dimVisitIssueReasonsTOList[0];
            else
                return null;
        }

        public List<DimVisitIssueReasonsTO> ConvertDTToList(DataTable dimVisitIssueReasonsTODT)
        {
            List<DimVisitIssueReasonsTO> dimVisitIssueReasonsTOList = new List<DimVisitIssueReasonsTO>();
            if (dimVisitIssueReasonsTODT != null)
            {
            }
            return dimVisitIssueReasonsTOList;
        }

        #endregion

        #region Insertion
        public int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO)
        {
            return _iDimVisitIssueReasonsDAO.InsertDimVisitIssueReasons(dimVisitIssueReasonsTO);
        }

        public int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVisitIssueReasonsDAO.InsertDimVisitIssueReasons(dimVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO)
        {
            return _iDimVisitIssueReasonsDAO.UpdateDimVisitIssueReasons(dimVisitIssueReasonsTO);
        }

        public int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVisitIssueReasonsDAO.UpdateDimVisitIssueReasons(dimVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons)
        {
            return _iDimVisitIssueReasonsDAO.DeleteDimVisitIssueReasons(idVisitIssueReasons);
        }

        public int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVisitIssueReasonsDAO.DeleteDimVisitIssueReasons(idVisitIssueReasons, conn, tran);
        }

        #endregion

    }
}
