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
    public class TblBookingOpngBalBL : ITblBookingOpngBalBL
    {
        #region Selection
        private readonly ITblBookingOpngBalDAO _iTblBookingOpngBalDAO;
        private readonly ITblBookingsDAO _iTblBookingsDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblBookingOpngBalBL(ICommon iCommon, IConnectionString iConnectionString, ITblBookingOpngBalDAO iTblBookingOpngBalDAO, ITblBookingsDAO iTblBookingsDAO)
        {
            _iTblBookingOpngBalDAO = iTblBookingOpngBalDAO;
            _iTblBookingsDAO = iTblBookingsDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        public List<TblBookingOpngBalTO> SelectAllTblBookingOpngBalList(DateTime asOnDate)
        {
           return _iTblBookingOpngBalDAO.SelectAllTblBookingOpngBal(asOnDate);
        }

        public TblBookingOpngBalTO SelectTblBookingOpngBalTO(Int32 idOpeningBal)
        {
            return _iTblBookingOpngBalDAO.SelectTblBookingOpngBal(idOpeningBal);
        }

        #endregion
        
        #region Insertion
        public int InsertTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO)
        {
            return _iTblBookingOpngBalDAO.InsertTblBookingOpngBal(tblBookingOpngBalTO);
        }

        public int InsertTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingOpngBalDAO.InsertTblBookingOpngBal(tblBookingOpngBalTO, conn, tran);
        }

        public ResultMessage CalculateBookingOpeningBalance()
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Select All Bookings whose PendingQty > 0
                Dictionary<Int32, Double> bookingDCT = _iTblBookingsDAO.SelectBookingsPendingQtyDCT(conn, tran);
                if (bookingDCT != null && bookingDCT.Count > 0)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    foreach (var bookingId in bookingDCT.Keys)
                    {
                        TblBookingOpngBalTO tblBookingOpngBalTO = new TblBookingOpngBalTO();
                        tblBookingOpngBalTO.BookingId = bookingId;
                        tblBookingOpngBalTO.OpeningBalQty = bookingDCT[bookingId];
                        tblBookingOpngBalTO.BalAsOnDate = serverDate;

                        int result = InsertTblBookingOpngBal(tblBookingOpngBalTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblBookingOpngBal For BookingID : " + bookingId;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Opening Balance Calculated Successfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method CalculateBookingOpeningBalance";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO)
        {
            return _iTblBookingOpngBalDAO.UpdateTblBookingOpngBal(tblBookingOpngBalTO);
        }

        public int UpdateTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingOpngBalDAO.UpdateTblBookingOpngBal(tblBookingOpngBalTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingOpngBal(Int32 idOpeningBal)
        {
            return _iTblBookingOpngBalDAO.DeleteTblBookingOpngBal(idOpeningBal);
        }

        public int DeleteTblBookingOpngBal(Int32 idOpeningBal, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingOpngBalDAO.DeleteTblBookingOpngBal(idOpeningBal, conn, tran);
        }

        #endregion
        
    }
}
