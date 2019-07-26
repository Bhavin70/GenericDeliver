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
using ODLMWebAPI.DAL.Interfaces;
 
namespace ODLMWebAPI.BL
{ 
    public class TblBookingScheduleBL : ITblBookingScheduleBL
    {
        private readonly ITblBookingScheduleDAO _iTblBookingScheduleDAO;
        public TblBookingScheduleBL(ITblBookingScheduleDAO iTblBookingScheduleDAO)
        {
            _iTblBookingScheduleDAO = iTblBookingScheduleDAO;
        }
        #region Selection

        public List<TblBookingScheduleTO> SelectAllTblBookingScheduleList()
        {
            return _iTblBookingScheduleDAO.SelectAllTblBookingSchedule();
          
        }

        public TblBookingScheduleTO SelectTblBookingScheduleTO(Int32 idSchedule)
        {
           return _iTblBookingScheduleDAO.SelectTblBookingSchedule(idSchedule);
        }

        public List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId)
        {
            return _iTblBookingScheduleDAO.SelectAllTblBookingScheduleList(bookingId);
        }
        public List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblBookingScheduleDAO.SelectAllTblBookingScheduleList(bookingId, conn,tran);
        }


        //public List<TblBookingScheduleTO> SelectBookingScheduleByBookingId(Int32 bookingId)
        //{
        //    List<TblBookingScheduleTO> list = SelectAllTblBookingScheduleList(bookingId);

        //    if(list!=null && list.Count >0)
        //    {
        //        for (int k = 0; k < list.Count; k++)
        //        {
        //            TblBookingScheduleTO tblBookingScheduleTO = list[k];

        //            tblBookingScheduleTO.DeliveryAddressLst = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);

        //            tblBookingScheduleTO.OrderDetailsLst = BL.TblBookingExtBL.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);
        //        }
        //    }
          
        //    return list;
        //}

        #endregion

        #region Insertion
        public int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            return _iTblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO);
        }

        public int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            return _iTblBookingScheduleDAO.UpdateTblBookingSchedule(tblBookingScheduleTO);
        }

        public int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingScheduleDAO.UpdateTblBookingSchedule(tblBookingScheduleTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingSchedule(Int32 idSchedule)
        {
            return _iTblBookingScheduleDAO.DeleteTblBookingSchedule(idSchedule);
        }

        public int DeleteTblBookingSchedule(Int32 idSchedule, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingScheduleDAO.DeleteTblBookingSchedule(idSchedule, conn, tran);
        }

        #endregion
        
    }
}
