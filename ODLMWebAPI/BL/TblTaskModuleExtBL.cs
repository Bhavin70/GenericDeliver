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
    public class TblTaskModuleExtBL : ITblTaskModuleExtBL
    {

        private readonly ITblTaskModuleExtDAO _iTblTaskModuleExtDAO;
        public TblTaskModuleExtBL(ITblTaskModuleExtDAO iTblTaskModuleExtDAO)
        {
            _iTblTaskModuleExtDAO = iTblTaskModuleExtDAO;
        }
        #region Selection
        public List<TblTaskModuleExtTO> SelectAllTblTaskModuleExt()
        {
            return _iTblTaskModuleExtDAO.SelectAllTblTaskModuleExt();
        }

        //public List<TblTaskModuleExtTO> SelectAllTblTaskModuleExtList()
        //{
        //   return _iTblTaskModuleExtDAO.SelectAllTblTaskModuleExt();
        //}

        public TblTaskModuleExtTO SelectTblTaskModuleExtTO(Int32 idTaskModuleExt)
        {
            TblTaskModuleExtTO tblTaskModuleExtTODT = _iTblTaskModuleExtDAO.SelectTblTaskModuleExt(idTaskModuleExt);
            if (tblTaskModuleExtTODT != null)
                return tblTaskModuleExtTODT;
            else
                return null;
        }

        public List<TblTaskModuleExtTO> SelectTaskModuleDetailsByEntityId(Int32 EntityId)
        {
            return _iTblTaskModuleExtDAO.SelectTaskModuleDetailsByEntityId(EntityId);
        }

        #endregion

        #region Insertion
        public int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO)
        {
            return _iTblTaskModuleExtDAO.InsertTblTaskModuleExt(tblTaskModuleExtTO);
        }

        public int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTaskModuleExtDAO.InsertTblTaskModuleExt(tblTaskModuleExtTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO)
        {
            return _iTblTaskModuleExtDAO.UpdateTblTaskModuleExt(tblTaskModuleExtTO);
        }

        public int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTaskModuleExtDAO.UpdateTblTaskModuleExt(tblTaskModuleExtTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblTaskModuleExt(Int32 idTaskModuleExt)
        {
            return _iTblTaskModuleExtDAO.DeleteTblTaskModuleExt(idTaskModuleExt);
        }

        public int DeleteTblTaskModuleExt(Int32 idTaskModuleExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTaskModuleExtDAO.DeleteTblTaskModuleExt(idTaskModuleExt, conn, tran);
        }

        #endregion
    }
}
