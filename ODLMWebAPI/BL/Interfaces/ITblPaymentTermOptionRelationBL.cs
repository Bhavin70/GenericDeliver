using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface ITblPaymentTermOptionRelationBL
    {
        List<TblPaymentTermOptionRelationTO> SelectAllTblPaymentTermOptionRelation();
        List<TblPaymentTermOptionRelationTO> SelectAllTblPaymentTermOptionRelationList();
        TblPaymentTermOptionRelationTO SelectTblPaymentTermOptionRelationTO(Int32 idPaymentTermOptionRelation);
        TblPaymentTermOptionRelationTO SelectTblPaymentTermOptionRelationTOByBookingId(Int32 bookingId, Int32 paymentTermId);
        TblPaymentTermOptionRelationTO SelectTblPaymentTermOptionRelationTOByInvoiceId(Int32 invoiceId, Int32 paymentTermId);
        int InsertTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO);
        int InsertTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO);
        int UpdateTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation);
        int DeleteTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation, SqlConnection conn, SqlTransaction tran);
    }
}
