using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TblOtherSourceBL : ITblOtherSourceBL
    {
        private readonly ITblOtherSourceDAO _iTblOtherSourceDAO;
        public TblOtherSourceBL(ITblOtherSourceDAO iTblOtherSourceDAO)
        {
            _iTblOtherSourceDAO = iTblOtherSourceDAO;
        }
        #region Selection

        public List<TblOtherSourceTO> SelectAllTblOtherSourceList()
        {
            return _iTblOtherSourceDAO.SelectAllTblOtherSource();
        }

        public TblOtherSourceTO SelectTblOtherSourceTO(Int32 idOtherSource)
        {
            return _iTblOtherSourceDAO.SelectTblOtherSource(idOtherSource);
        }

        public List<TblOtherSourceTO> SelectTblOtherSourceListFromDesc(string OtherSourceDesc)
        {
            return _iTblOtherSourceDAO.SelectTblOtherSourceListFromDesc(OtherSourceDesc);
        }

        public List<DropDownTO> SelectOtherSourceOfMarketTrendForDropDown()
        {
            return _iTblOtherSourceDAO.SelectOtherSourceOfMarketTrendForDropDown();
        }

        #endregion

        #region Insertion
        public int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO)
        {
            return _iTblOtherSourceDAO.InsertTblOtherSource(tblOtherSourceTO);
        }

        public int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOtherSourceDAO.InsertTblOtherSource(tblOtherSourceTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO)
        {
            return _iTblOtherSourceDAO.UpdateTblOtherSource(tblOtherSourceTO);
        }

        public int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOtherSourceDAO.UpdateTblOtherSource(tblOtherSourceTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblOtherSource(Int32 idOtherSource)
        {
            return _iTblOtherSourceDAO.DeleteTblOtherSource(idOtherSource);
        }

        public int DeleteTblOtherSource(Int32 idOtherSource, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOtherSourceDAO.DeleteTblOtherSource(idOtherSource, conn, tran);
        }

        #endregion

    }
}
