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
namespace ODLMWebAPI.BL
{
    public class TblIssueTypeBL : ITblIssueTypeBL
    {
        #region Selection
        public DataTable SelectAllTblIssueType()
        {
            return TblIssueTypeDAO.SelectAllTblIssueType();
        }

        public List<TblIssueTypeTO> SelectAllTblIssueTypeList()
        {
            DataTable tblIssueTypeTODT = TblIssueTypeDAO.SelectAllTblIssueType();
            return ConvertDTToList(tblIssueTypeTODT);
        }

        public TblIssueTypeTO SelectTblIssueTypeTO(Int32 idIssueType)
        {
            DataTable tblIssueTypeTODT = TblIssueTypeDAO.SelectTblIssueType(idIssueType);
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
            return TblIssueTypeDAO.InsertTblIssueType(tblIssueTypeTO);
        }

        public int InsertTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblIssueTypeDAO.InsertTblIssueType(tblIssueTypeTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO)
        {
            return TblIssueTypeDAO.UpdateTblIssueType(tblIssueTypeTO);
        }

        public int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblIssueTypeDAO.UpdateTblIssueType(tblIssueTypeTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblIssueType(Int32 idIssueType)
        {
            return TblIssueTypeDAO.DeleteTblIssueType(idIssueType);
        }

        public int DeleteTblIssueType(Int32 idIssueType, SqlConnection conn, SqlTransaction tran)
        {
            return TblIssueTypeDAO.DeleteTblIssueType(idIssueType, conn, tran);
        }

        #endregion

    }
}
