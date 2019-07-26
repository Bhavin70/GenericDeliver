using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblBookingExtBL
    {
        List<TblBookingExtTO> SelectAllTblBookingExtList();
        List<TblBookingExtTO> SelectAllTblBookingExtList(int bookingId);
        List<TblBookingExtTO> SelectAllTblBookingExtList(Int32 bookingId,Int32 brandId,Int32 prodCatId,Int32 stateId);
        List<TblBookingExtTO> SelectAllTblBookingExtList(int bookingId, SqlConnection conn, SqlTransaction trans);
        List<TblBookingExtTO> SelectEmptyBookingExtList(int prodCatgId, int prodSpecId);
        List<TblBookingExtTO> SelectAllBookingDetailsWrtDealer(Int32 dealerId);
        TblBookingExtTO SelectTblBookingExtTO(Int32 idBookingExt);
        List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(Int32 scheduleId);
        List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(Int32 scheduleId, SqlConnection conn, SqlTransaction tran);
        int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO);
        int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO);
        int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBookingExt(Int32 idBookingExt);
        int DeleteTblBookingExt(Int32 idBookingExt, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBookingExtBySchedule(Int32 scheduleId, SqlConnection conn, SqlTransaction tran);
    }
}
