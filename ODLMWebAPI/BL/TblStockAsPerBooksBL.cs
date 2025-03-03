using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblStockAsPerBooksBL : ITblStockAsPerBooksBL
    {
        private readonly ITblStockAsPerBooksDAO _iTblStockAsPerBooksDAO;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly IConnectionString _iConnectionString;
        public TblStockAsPerBooksBL(IConnectionString iConnectionString, ITblStockAsPerBooksDAO iTblStockAsPerBooksDAO, ITblAlertInstanceBL iTblAlertInstanceBL)
        {
            _iTblStockAsPerBooksDAO = iTblStockAsPerBooksDAO;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public List<TblStockAsPerBooksTO> SelectAllTblStockAsPerBooksList()
        {
            return  _iTblStockAsPerBooksDAO.SelectAllTblStockAsPerBooks();
        }

        public TblStockAsPerBooksTO SelectTblStockAsPerBooksTO(Int32 idStockAsPerBooks)
        {
            return _iTblStockAsPerBooksDAO.SelectTblStockAsPerBooks(idStockAsPerBooks);
        }

        public TblStockAsPerBooksTO SelectTblStockAsPerBooksTO(DateTime stockDate)
        {
            return _iTblStockAsPerBooksDAO.SelectTblStockAsPerBooks(stockDate);
        }

        #endregion

        #region Insertion
        public int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO)
        {
            return _iTblStockAsPerBooksDAO.InsertTblStockAsPerBooks(tblStockAsPerBooksTO);
        }

        public ResultMessage SaveStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save the Stock as per books

                int result = _iTblStockAsPerBooksDAO.InsertTblStockAsPerBooks(tblStockAsPerBooksTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblStockAsPerBooks";
                    resultMessage.Tag = tblStockAsPerBooksTO;
                    return resultMessage;
                }

                #endregion

                #region 2. Notification to Directors and account person on stock confirmation

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.TODAYS_STOCK_AS_PER_ACCOUNTANT;
                tblAlertInstanceTO.AlertAction = "TODAYS_STOCK_AS_PER_ACCOUNTANT";
                tblAlertInstanceTO.AlertComment = "Stock Ready for quota declaration . Stock(In MT) as per books is :" + tblStockAsPerBooksTO.StockInMT;
                tblAlertInstanceTO.EffectiveFromDate = tblStockAsPerBooksTO.CreatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "TODAYS_STOCK_AS_PER_ACCOUNTANT";
                tblAlertInstanceTO.SourceEntityId = tblStockAsPerBooksTO.IdStockAsPerBooks;
                tblAlertInstanceTO.RaisedBy = tblStockAsPerBooksTO.CreatedBy;
                tblAlertInstanceTO.RaisedOn = tblStockAsPerBooksTO.CreatedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Stock Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception While SaveStockAsPerBooks";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockAsPerBooksDAO.InsertTblStockAsPerBooks(tblStockAsPerBooksTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO)
        {
            return _iTblStockAsPerBooksDAO.UpdateTblStockAsPerBooks(tblStockAsPerBooksTO);
        }

        public int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockAsPerBooksDAO.UpdateTblStockAsPerBooks(tblStockAsPerBooksTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks)
        {
            return _iTblStockAsPerBooksDAO.DeleteTblStockAsPerBooks(idStockAsPerBooks);
        }

        public int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockAsPerBooksDAO.DeleteTblStockAsPerBooks(idStockAsPerBooks, conn, tran);
        }

        #endregion
        
    }
}
