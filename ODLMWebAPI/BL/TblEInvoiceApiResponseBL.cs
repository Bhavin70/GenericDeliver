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
    public class TblEInvoiceApiResponseBL: ITblEInvoiceApiResponseBL
    {
        private readonly ITblEInvoiceApiResponseDAO _iTblEInvoiceApiResponseDAO;

        public TblEInvoiceApiResponseBL(ITblEInvoiceApiResponseDAO iTblEInvoiceApiResponseDAO)
        {
            _iTblEInvoiceApiResponseDAO = iTblEInvoiceApiResponseDAO;
        }

        #region Selection

        public List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponseList()
        {
            return _iTblEInvoiceApiResponseDAO.SelectAllTblEInvoiceApiResponse();
        }

        public List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponseList(Int32 apiId)
        {
            return _iTblEInvoiceApiResponseDAO.SelectAllTblEInvoiceApiResponse(apiId);
        }

        public List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseList(int idResponse)
        {
            return _iTblEInvoiceApiResponseDAO.SelectTblEInvoiceApiResponseList(idResponse);
        }
        public List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseListForInvoiceId(int invoiceId)
        {
            return _iTblEInvoiceApiResponseDAO.SelectTblEInvoiceApiResponseListForInvoiceId(invoiceId);
        }
        public List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseListForInvoiceId(int invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEInvoiceApiResponseDAO.SelectTblEInvoiceApiResponseListForInvoiceId(invoiceId, conn, tran);
        }

        #endregion

        #region Insertion

        public int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO)
        {
            return _iTblEInvoiceApiResponseDAO.InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO);
        }
        public int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEInvoiceApiResponseDAO.InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO, conn, tran);
        }

        #endregion

        #region Deletion

        public int DeleteTblEInvoiceApiResponse(Int32 idApi)
        {
            return _iTblEInvoiceApiResponseDAO.DeleteTblEInvoiceApiResponse(idApi);
        }
        public int DeleteTblEInvoiceApiResponse(Int32 idApi, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEInvoiceApiResponseDAO.DeleteTblEInvoiceApiResponse(idApi, conn, tran);
        }

        #endregion
    }
}
