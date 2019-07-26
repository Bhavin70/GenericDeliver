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
    public class TblVisitPersonTypeBL : ITblVisitPersonTypeBL
    {
        private readonly ITblVisitPersonTypeDAO _iTblVisitPersonTypeDAO;
        public TblVisitPersonTypeBL(ITblVisitPersonTypeDAO iTblVisitPersonTypeDAO)
        {
            _iTblVisitPersonTypeDAO = iTblVisitPersonTypeDAO;
        }
        #region Selection
        public DataTable SelectAllTblVisitPersonType()
        {
            return _iTblVisitPersonTypeDAO.SelectAllTblVisitPersonType();
        }

        public List<TblVisitPersonTypeTO> SelectAllTblVisitPersonTypeList()
        {
            DataTable tblVisitPersonTypeTODT = _iTblVisitPersonTypeDAO.SelectAllTblVisitPersonType();
            return ConvertDTToList(tblVisitPersonTypeTODT);
        }

        public TblVisitPersonTypeTO SelectTblVisitPersonTypeTO(Int32 idPersonType)
        {
            DataTable tblVisitPersonTypeTODT = _iTblVisitPersonTypeDAO.SelectTblVisitPersonType(idPersonType);
            List<TblVisitPersonTypeTO> tblVisitPersonTypeTOList = ConvertDTToList(tblVisitPersonTypeTODT);
            if (tblVisitPersonTypeTOList != null && tblVisitPersonTypeTOList.Count == 1)
                return tblVisitPersonTypeTOList[0];
            else
                return null;
        }

        public List<TblVisitPersonTypeTO> ConvertDTToList(DataTable tblVisitPersonTypeTODT)
        {
            List<TblVisitPersonTypeTO> tblVisitPersonTypeTOList = new List<TblVisitPersonTypeTO>();
            if (tblVisitPersonTypeTODT != null)
            {
            }
            return tblVisitPersonTypeTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO)
        {
            return _iTblVisitPersonTypeDAO.InsertTblVisitPersonType(tblVisitPersonTypeTO);
        }

        public int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitPersonTypeDAO.InsertTblVisitPersonType(tblVisitPersonTypeTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO)
        {
            return _iTblVisitPersonTypeDAO.UpdateTblVisitPersonType(tblVisitPersonTypeTO);
        }

        public int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitPersonTypeDAO.UpdateTblVisitPersonType(tblVisitPersonTypeTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblVisitPersonType(Int32 idPersonType)
        {
            return _iTblVisitPersonTypeDAO.DeleteTblVisitPersonType(idPersonType);
        }

        public int DeleteTblVisitPersonType(Int32 idPersonType, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitPersonTypeDAO.DeleteTblVisitPersonType(idPersonType, conn, tran);
        }

        #endregion

    }
}
