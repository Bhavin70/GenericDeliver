using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblSiteTypeBL : ITblSiteTypeBL
    {
        private readonly ITblSiteTypeDAO _iTblSiteTypeDAO;
        public TblSiteTypeBL(ITblSiteTypeDAO iTblSiteTypeDAO)
        {
            _iTblSiteTypeDAO = iTblSiteTypeDAO;
        }
        #region Selection

        public List<TblSiteTypeTO> SelectAllTblSiteTypeList()
        {
            List<TblSiteTypeTO> siteTypeTOList = _iTblSiteTypeDAO.SelectAllTblSiteTypeList();
            if (siteTypeTOList != null)
                return siteTypeTOList;
            else
                return null;
        }

        public TblSiteTypeTO SelectTblSiteTypeTO(Int32 idSiteType)
        {
            DataTable tblSiteTypeTODT = _iTblSiteTypeDAO.SelectTblSiteType(idSiteType);
            List<TblSiteTypeTO> tblSiteTypeTOList = ConvertDTToList(tblSiteTypeTODT);
            if (tblSiteTypeTOList != null && tblSiteTypeTOList.Count == 1)
                return tblSiteTypeTOList[0];
            else
                return null;
        }

        public List<TblSiteTypeTO> ConvertDTToList(DataTable tblSiteTypeTODT)
        {
            List<TblSiteTypeTO> tblSiteTypeTOList = new List<TblSiteTypeTO>();
            if (tblSiteTypeTODT != null)
            {
              
            }
            return tblSiteTypeTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblSiteType(TblSiteTypeTO tblSiteTypeTO)
        {
            return _iTblSiteTypeDAO.InsertTblSiteType(tblSiteTypeTO);
        }

        public int InsertTblSiteType(ref TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSiteTypeDAO.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO)
        {
            return _iTblSiteTypeDAO.UpdateTblSiteType(tblSiteTypeTO);
        }

        public int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSiteTypeDAO.UpdateTblSiteType(tblSiteTypeTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblSiteType(Int32 idSiteType)
        {
            return _iTblSiteTypeDAO.DeleteTblSiteType(idSiteType);
        }

        public int DeleteTblSiteType(Int32 idSiteType, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSiteTypeDAO.DeleteTblSiteType(idSiteType, conn, tran);
        }

        #endregion

    }
}
