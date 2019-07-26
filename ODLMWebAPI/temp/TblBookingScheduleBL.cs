using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblBookingScheduleBL : ITblBookingScheduleBL
    {
        #region Selection

        public List<TblBookingScheduleTO> SelectAllTblBookingScheduleList()
        {
            return TblBookingScheduleDAO.SelectAllTblBookingSchedule();
          
        }

        public TblBookingScheduleTO SelectTblBookingScheduleTO(Int32 idSchedule)
        {
           return TblBookingScheduleDAO.SelectTblBookingSchedule(idSchedule);
        }

        public List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId)
        {
            return TblBookingScheduleDAO.SelectAllTblBookingScheduleList(bookingId);
        }
        public List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId,SqlConnection conn,SqlTransaction tran)
        {
            return TblBookingScheduleDAO.SelectAllTblBookingScheduleList(bookingId, conn,tran);
        }


        public List<TblBookingScheduleTO> SelectBookingScheduleByBookingId(Int32 bookingId)
        {
            List<TblBookingScheduleTO> list = TblBookingScheduleBL.SelectAllTblBookingScheduleList(bookingId);

            for (int k = 0; k < list.Count; k++)
            {
                TblBookingScheduleTO tblBookingScheduleTO = list[k];

                tblBookingScheduleTO.DeliveryAddressLst = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);

                tblBookingScheduleTO.OrderDetailsLst = BL.TblBookingExtBL.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);
            }
            return list;
        }

        #endregion

        #region Insertion
        public int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            return TblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO);
        }

        public int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            return TblBookingScheduleDAO.UpdateTblBookingSchedule(tblBookingScheduleTO);
        }

        public int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingScheduleDAO.UpdateTblBookingSchedule(tblBookingScheduleTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingSchedule(Int32 idSchedule)
        {
            return TblBookingScheduleDAO.DeleteTblBookingSchedule(idSchedule);
        }

        public int DeleteTblBookingSchedule(Int32 idSchedule, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingScheduleDAO.DeleteTblBookingSchedule(idSchedule, conn, tran);
        }

        #endregion
        
    }
}
