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
    public class TblEInvoiceSessionApiResponseBL: ITblEInvoiceSessionApiResponseBL
    {
        private readonly ITblEInvoiceSessionApiResponseDAO _iTblEInvoiceSessionApiResponseDAO;

        public TblEInvoiceSessionApiResponseBL(ITblEInvoiceSessionApiResponseDAO iTblEInvoiceSessionApiResponseDAO)
        {
            _iTblEInvoiceSessionApiResponseDAO = iTblEInvoiceSessionApiResponseDAO;
        }

        #region Selection

        public List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponseList()
        {
            return _iTblEInvoiceSessionApiResponseDAO.SelectAllTblEInvoiceSessionApiResponse();
        }

        public List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponseList(Int32 apiId)
        {
            return _iTblEInvoiceSessionApiResponseDAO.SelectAllTblEInvoiceSessionApiResponse(apiId);
        }

        public List<TblEInvoiceSessionApiResponseTO> SelectTblEInvoiceSessionApiResponseList(int idResponse)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Insertion

        public int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO)
        {
            return _iTblEInvoiceSessionApiResponseDAO.InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO);
        }
        public int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEInvoiceSessionApiResponseDAO.InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO, conn, tran);
        }

        #endregion

        #region Deletion

        public int DeleteTblEInvoiceSessionApiResponse(Int32 idApi)
        {
            return _iTblEInvoiceSessionApiResponseDAO.DeleteTblEInvoiceSessionApiResponse(idApi);
        }
        public int DeleteTblEInvoiceSessionApiResponse(Int32 idApi, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEInvoiceSessionApiResponseDAO.DeleteTblEInvoiceSessionApiResponse(idApi, conn, tran);
        }
        #endregion
    }
}
