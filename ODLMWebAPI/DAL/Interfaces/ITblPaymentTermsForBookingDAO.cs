using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblPaymentTermsForBookingDAO
    {
        String SqlSelectQuery();
        List<TblPaymentTermsForBookingTO> SelectAllTblPaymentTermsForBooking();
        TblPaymentTermsForBookingTO SelectTblPaymentTermsForBooking(Int32 idPaymentTerm);
        List<TblPaymentTermsForBookingTO> SelectAllTblPaymentTermsForBooking(SqlConnection conn, SqlTransaction tran);
        List<TblPaymentTermsForBookingTO> ConvertDTToList(SqlDataReader tblPaymentTermsForBookingTODT);
        int InsertTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO);
        int InsertTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlCommand cmdInsert);
        int UpdateTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO);
        int UpdateTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlCommand cmdUpdate);
        int DeleteTblPaymentTermsForBooking(Int32 idPaymentTerm);
        int DeleteTblPaymentTermsForBooking(Int32 idPaymentTerm, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPaymentTerm, SqlCommand cmdDelete);
    }
}
