﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class DimVisitIssueReasonsBL : IDimVisitIssueReasonsBL
    {
        #region Selection
        public DataTable SelectAllDimVisitIssueReasons()
        {
            return DimVisitIssueReasonsDAO.SelectAllDimVisitIssueReasons();
        }

        public List<DimVisitIssueReasonsTO> SelectAllDimVisitIssueReasonsList()
        {
            DataTable dimVisitIssueReasonsTODT = DimVisitIssueReasonsDAO.SelectAllDimVisitIssueReasons();
            return ConvertDTToList(dimVisitIssueReasonsTODT);
        }

        public DimVisitIssueReasonsTO SelectDimVisitIssueReasonsTO(Int32 idVisitIssueReasons)
        {
            DataTable dimVisitIssueReasonsTODT = DimVisitIssueReasonsDAO.SelectDimVisitIssueReasons(idVisitIssueReasons);
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
            return DimVisitIssueReasonsDAO.InsertDimVisitIssueReasons(dimVisitIssueReasonsTO);
        }

        public int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVisitIssueReasonsDAO.InsertDimVisitIssueReasons(dimVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO)
        {
            return DimVisitIssueReasonsDAO.UpdateDimVisitIssueReasons(dimVisitIssueReasonsTO);
        }

        public int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVisitIssueReasonsDAO.UpdateDimVisitIssueReasons(dimVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons)
        {
            return DimVisitIssueReasonsDAO.DeleteDimVisitIssueReasons(idVisitIssueReasons);
        }

        public int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran)
        {
            return DimVisitIssueReasonsDAO.DeleteDimVisitIssueReasons(idVisitIssueReasons, conn, tran);
        }

        #endregion

    }
}
