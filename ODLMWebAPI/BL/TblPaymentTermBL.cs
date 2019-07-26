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
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblPaymentTermBL : ITblPaymentTermBL
    {
        private readonly ITblPaymentTermDAO _iTblPaymentTermDAO;
        public TblPaymentTermBL(ITblPaymentTermDAO iTblPaymentTermDAO)
        {
            _iTblPaymentTermDAO = iTblPaymentTermDAO;
        }
        #region Selection
        public DataTable SelectAllTblPaymentTerm()
        {
            return _iTblPaymentTermDAO.SelectAllTblPaymentTerm();
        }

        public List<TblPaymentTermTO> SelectAllTblPaymentTermList()
        {
            DataTable tblPaymentTermTODT = _iTblPaymentTermDAO.SelectAllTblPaymentTerm();
            return ConvertDTToList(tblPaymentTermTODT);
        }

        public TblPaymentTermTO SelectTblPaymentTermTO(Int32 idPaymentTerm)
        {
            DataTable tblPaymentTermTODT = _iTblPaymentTermDAO.SelectTblPaymentTerm(idPaymentTerm);
            List<TblPaymentTermTO> tblPaymentTermTOList = ConvertDTToList(tblPaymentTermTODT);
            if (tblPaymentTermTOList != null && tblPaymentTermTOList.Count == 1)
                return tblPaymentTermTOList[0];
            else
                return null;
        }

        public List<DropDownTO> SelectPaymentTermListForDopDown()
        {
            List<DropDownTO> paymentTermTOList = _iTblPaymentTermDAO.SelecPaymentTermForDropDown();
            if (paymentTermTOList != null)
                return paymentTermTOList;
            else
                return null;
        }

        public List<TblPaymentTermTO> ConvertDTToList(DataTable tblPaymentTermTODT)
        {
            List<TblPaymentTermTO> tblPaymentTermTOList = new List<TblPaymentTermTO>();
            if (tblPaymentTermTODT != null)
            {
            }
            return tblPaymentTermTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO)
        {
            return _iTblPaymentTermDAO.InsertTblPaymentTerm(tblPaymentTermTO);
        }

        public int InsertTblPaymentTerm(ref TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                if(conn.State == ConnectionState.Closed)
                conn.Open();

                result = _iTblPaymentTermDAO.InsertTblPaymentTerm(ref tblPaymentTermTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblPaymentTerm");
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblPaymentTerm");
                return -1;
            }
            finally
            {
                //conn.Close();
            }
        }

        #endregion

        #region Updation
        public int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO)
        {
            return _iTblPaymentTermDAO.UpdateTblPaymentTerm(tblPaymentTermTO);
        }

        public int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermDAO.UpdateTblPaymentTerm(tblPaymentTermTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPaymentTerm(Int32 idPaymentTerm)
        {
            return _iTblPaymentTermDAO.DeleteTblPaymentTerm(idPaymentTerm);
        }

        public int DeleteTblPaymentTerm(Int32 idPaymentTerm, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentTermDAO.DeleteTblPaymentTerm(idPaymentTerm, conn, tran);
        }

        #endregion

    }
}
