using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblInvoiceAddressDAO
    {
        String SqlSelectQuery();
        List<TblInvoiceAddressTO> SelectAllTblInvoiceAddress();
        TblInvoiceAddressTO SelectTblInvoiceAddress(Int32 idInvoiceAddr);
        List<TblInvoiceAddressTO> SelectAllTblInvoiceAddress(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceAddressTO> ConvertDTToList(SqlDataReader tblInvoiceAddressTODT);
        int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO);
        int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblInvoiceAddressTO tblInvoiceAddressTO, SqlCommand cmdInsert);
        int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO);
        int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblInvoiceAddressTO tblInvoiceAddressTO, SqlCommand cmdUpdate);
        int DeleteTblInvoiceAddress(Int32 idInvoiceAddr);
        int DeleteTblInvoiceAddressByinvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceAddress(Int32 idInvoiceAddr, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idInvoiceAddr, SqlCommand cmdDelete);
        //Aniket [22-4-2019]
        List<TblInvoiceAddressTO> SelectTblInvoiceAddressByDealerId(Int32 dealerOrgId, String txnAddrTypeIdtemp,Int32 topRecordcnt);

        List<TblInvoiceAddressTO> SelectTblInvoice(Int32 invoiceId);
    }
}