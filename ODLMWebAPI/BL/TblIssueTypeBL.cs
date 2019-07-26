using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblIssueTypeBL : ITblIssueTypeBL
    {
        private readonly ITblIssueTypeDAO _iTblIssueTypeDAO;
        public TblIssueTypeBL(ITblIssueTypeDAO iTblIssueTypeDAO)
        {
            _iTblIssueTypeDAO = iTblIssueTypeDAO;
        }
        #region Selection
        public DataTable SelectAllTblIssueType()
        {
            return _iTblIssueTypeDAO.SelectAllTblIssueType();
        }

        public List<TblIssueTypeTO> SelectAllTblIssueTypeList()
        {
            DataTable tblIssueTypeTODT = _iTblIssueTypeDAO.SelectAllTblIssueType();
            return ConvertDTToList(tblIssueTypeTODT);
        }

        public TblIssueTypeTO SelectTblIssueTypeTO(Int32 idIssueType)
        {
            DataTable tblIssueTypeTODT = _iTblIssueTypeDAO.SelectTblIssueType(idIssueType);
            List<TblIssueTypeTO> tblIssueTypeTOList = ConvertDTToList(tblIssueTypeTODT);
            if (tblIssueTypeTOList != null && tblIssueTypeTOList.Count == 1)
                return tblIssueTypeTOList[0];
            else
                return null;
        }

        public List<TblIssueTypeTO> ConvertDTToList(DataTable tblIssueTypeTODT)
        {
            List<TblIssueTypeTO> tblIssueTypeTOList = new List<TblIssueTypeTO>();
            if (tblIssueTypeTODT != null)
            {
            }
            return tblIssueTypeTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblIssueType(TblIssueTypeTO tblIssueTypeTO)
        {
            return _iTblIssueTypeDAO.InsertTblIssueType(tblIssueTypeTO);
        }

        public int InsertTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblIssueTypeDAO.InsertTblIssueType(tblIssueTypeTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO)
        {
            return _iTblIssueTypeDAO.UpdateTblIssueType(tblIssueTypeTO);
        }

        public int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblIssueTypeDAO.UpdateTblIssueType(tblIssueTypeTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblIssueType(Int32 idIssueType)
        {
            return _iTblIssueTypeDAO.DeleteTblIssueType(idIssueType);
        }

        public int DeleteTblIssueType(Int32 idIssueType, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblIssueTypeDAO.DeleteTblIssueType(idIssueType, conn, tran);
        }

        #endregion

    }
}
