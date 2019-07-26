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
    public class TblOtherDesignationsBL : ITblOtherDesignationsBL
    {
        private readonly ITblOtherDesignationsDAO _iTblOtherDesignationsDAO;
        public TblOtherDesignationsBL(ITblOtherDesignationsDAO iTblOtherDesignationsDAO)
        {
            _iTblOtherDesignationsDAO = iTblOtherDesignationsDAO;
        }
        #region Selection
        public List<TblOtherDesignationsTO> SelectAllTblOtherDesignations()
        {
            return _iTblOtherDesignationsDAO.SelectAllTblOtherDesignations();
        }

        public List<TblOtherDesignationsTO> SelectAllTblOtherDesignationsList()
        {
            List<TblOtherDesignationsTO> tblOtherDesignationsTODT = _iTblOtherDesignationsDAO.SelectAllTblOtherDesignations();
            return tblOtherDesignationsTODT;
        }

        public TblOtherDesignationsTO SelectTblOtherDesignationsTO(Int32 idOtherDesignation)
        {
            TblOtherDesignationsTO tblOtherDesignationsTO = _iTblOtherDesignationsDAO.SelectTblOtherDesignations(idOtherDesignation);
            if (tblOtherDesignationsTO != null)
                return tblOtherDesignationsTO;
            else
                return null;
        }



        #endregion

        #region Insertion
        public int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO)
        {
            return _iTblOtherDesignationsDAO.InsertTblOtherDesignations(tblOtherDesignationsTO);
        }

        public int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOtherDesignationsDAO.InsertTblOtherDesignations(tblOtherDesignationsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO)
        {
            return _iTblOtherDesignationsDAO.UpdateTblOtherDesignations(tblOtherDesignationsTO);
        }

        public int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOtherDesignationsDAO.UpdateTblOtherDesignations(tblOtherDesignationsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblOtherDesignations(Int32 idOtherDesignation)
        {
            return _iTblOtherDesignationsDAO.DeleteTblOtherDesignations(idOtherDesignation);
        }

        public int DeleteTblOtherDesignations(Int32 idOtherDesignation, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOtherDesignationsDAO.DeleteTblOtherDesignations(idOtherDesignation, conn, tran);
        }

        #endregion
    }
}