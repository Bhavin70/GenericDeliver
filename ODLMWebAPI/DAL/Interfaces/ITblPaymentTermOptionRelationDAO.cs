using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblPaymentTermOptionRelationDAO
    {
        String SqlSelectQuery();
        List<TblPaymentTermOptionRelationTO> SelectAllTblPaymentTermOptionRelation();
        TblPaymentTermOptionRelationTO SelectTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation);
        List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByBookingIdForUpdate(Int32 bookingId, Int32 paymentTermId);
        List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByInvoiceIdForUpdate(Int32 invoiceId, Int32 paymentTermId);
        List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByBookingId(Int32 bookingId);
        List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByInvoiceId(Int32 invoiceId);
        List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByLoadingId(Int32 loadingId);
        List<TblPaymentTermOptionRelationTO> SelectTblPaymentTermOptionRelationByLoadingId(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblPaymentTermOptionRelationTO> SelectAllTblPaymentTermOptionRelation(SqlConnection conn, SqlTransaction tran);
        List<TblPaymentTermOptionRelationTO> ConvertDTToList(SqlDataReader tblPaymentTermOptionRelationTODT);
        int InsertTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO);
        int InsertTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlCommand cmdInsert);
        int UpdateTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO);
        int UpdateTblPaymentTermOptionRelation(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO, SqlCommand cmdUpdate);
        int DeleteTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation);
        int DeleteTblPaymentTermOptionRelation(Int32 idPaymentTermOptionRelation, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPaymentTermOptionRelation, SqlCommand cmdDelete);
    }
}
