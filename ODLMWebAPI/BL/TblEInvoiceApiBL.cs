using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblEInvoiceApiBL: ITblEInvoiceApiBL
    {
        private readonly ITblEInvoiceApiDAO _iTblEInvoiceApiDAO;

        public TblEInvoiceApiBL(ITblEInvoiceApiDAO iTblEInvoiceApiDAO)
        {
            _iTblEInvoiceApiDAO = iTblEInvoiceApiDAO;
        }

        #region Selection

        public List<TblEInvoiceApiTO> SelectAllTblEInvoiceApiList()
        {
            return _iTblEInvoiceApiDAO.SelectAllTblEInvoiceApi();
        }

        public List<TblEInvoiceApiTO> SelectAllTblEInvoiceApiList(Int32 idApi)
        {
            return _iTblEInvoiceApiDAO.SelectAllTblEInvoiceApi(idApi);
        }

        public List<TblEInvoiceApiTO> SelectTblEInvoiceApiList(string apiName)
        {
            return _iTblEInvoiceApiDAO.SelectTblEInvoiceApi(apiName);
        }

        #endregion

        #region Insertion

        public int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO)
        {
            return _iTblEInvoiceApiDAO.InsertTblEInvoiceApi(tblEInvoiceApiTO);
        }
        public int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEInvoiceApiDAO.InsertTblEInvoiceApi(tblEInvoiceApiTO, conn, tran);
        }

        #endregion

        #region Updation

        public int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO)
        {
            return _iTblEInvoiceApiDAO.UpdateTblEInvoiceApi(tblEInvoiceApiTO);
        }
        public int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEInvoiceApiDAO.UpdateTblEInvoiceApi(tblEInvoiceApiTO, conn, tran);
        }

        #endregion

        #region Deletion

        public int DeleteTblEInvoiceApi(Int32 idApi)
        {
            return _iTblEInvoiceApiDAO.DeleteTblEInvoiceApi(idApi);
        }
        public int DeleteTblEInvoiceApi(Int32 idApi, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEInvoiceApiDAO.DeleteTblEInvoiceApi(idApi, conn, tran);
        }
        #endregion
    }
}
