using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblPaymentTermsForBookingBL
    {
        List<TblPaymentTermsForBookingTO> SelectAllTblPaymentTermsForBooking();
        TblPaymentTermsForBookingTO SelectTblPaymentTermsForBookingTO(Int32 idPaymentTerm);
        List<TblPaymentTermsForBookingTO> SelectAllTblPaymentTermsForBookingFromBookingId(Int32 bookingId, Int32 invoiceId);
        int InsertTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO);
        int InsertTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO);
        int UpdateTblPaymentTermsForBooking(TblPaymentTermsForBookingTO tblPaymentTermsForBookingTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPaymentTermsForBooking(Int32 idPaymentTerm);
        int DeleteTblPaymentTermsForBooking(Int32 idPaymentTerm, SqlConnection conn, SqlTransaction tran);
    }
}
