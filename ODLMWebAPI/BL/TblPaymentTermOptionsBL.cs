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
    public class TblPaymentTermOptionsBL : ITblPaymentTermOptionsBL
    {
        private readonly ITblPaymentTermOptionsDAO _iTblPaymentTermOptionsDAO;
        public TblPaymentTermOptionsBL(ITblPaymentTermOptionsDAO iTblPaymentTermOptionsDAO)
        {
            _iTblPaymentTermOptionsDAO = iTblPaymentTermOptionsDAO;
        }
        #region Selection
        public List<TblPaymentTermOptionsTO> SelectAllTblPaymentTermOptions()
        {
            return _iTblPaymentTermOptionsDAO.SelectAllTblPaymentTermOptions();
        }

        public List<TblPaymentTermOptionsTO> SelectAllTblPaymentTermOptionsList()
        {
           return _iTblPaymentTermOptionsDAO.SelectAllTblPaymentTermOptions();
        }

        public TblPaymentTermOptionsTO SelectTblPaymentTermOptionsTO(Int32 idPaymentTermOption)
        {
            TblPaymentTermOptionsTO tblPaymentTermOptionsTO= _iTblPaymentTermOptionsDAO.SelectTblPaymentTermOptions(idPaymentTermOption);
            if(tblPaymentTermOptionsTO != null)
                return tblPaymentTermOptionsTO;
            else
                return null;
        }

     

        #endregion
        
        #region Insertion
        public int InsertTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO)
        {
            return _iTblPaymentTermOptionsDAO.InsertTblPaymentTermOptions(tblPaymentTermOptionsTO);
        }

        public int InsertTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermOptionsDAO.InsertTblPaymentTermOptions(tblPaymentTermOptionsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO)
        {
            return _iTblPaymentTermOptionsDAO.UpdateTblPaymentTermOptions(tblPaymentTermOptionsTO);
        }

        public int UpdateTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermOptionsDAO.UpdateTblPaymentTermOptions(tblPaymentTermOptionsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblPaymentTermOptions(Int32 idPaymentTermOption)
        {
            return _iTblPaymentTermOptionsDAO.DeleteTblPaymentTermOptions(idPaymentTermOption);
        }

        public int DeleteTblPaymentTermOptions(Int32 idPaymentTermOption, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermOptionsDAO.DeleteTblPaymentTermOptions(idPaymentTermOption, conn, tran);
        }

        #endregion
        
    }
}
