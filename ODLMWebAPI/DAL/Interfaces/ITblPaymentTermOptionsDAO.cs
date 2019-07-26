using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblPaymentTermOptionsDAO
    {
        String SqlSelectQuery();
        List<TblPaymentTermOptionsTO> SelectAllTblPaymentTermOptions();
        TblPaymentTermOptionsTO SelectTblPaymentTermOptions(Int32 idPaymentTermOption);
        List<TblPaymentTermOptionsTO> SelectAllTblPaymentTermOptions(SqlConnection conn, SqlTransaction tran);
        List<TblPaymentTermOptionsTO> SelectTblPaymentTermOptionRelationBypaymentTermId(Int32 paymentTermId);
        List<TblPaymentTermOptionsTO> ConvertDTToList(SqlDataReader tblPaymentTermOptionsTODT);
        int InsertTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO);
        int InsertTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlCommand cmdInsert);
        int UpdateTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO);
        int UpdateTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlCommand cmdUpdate);
        int DeleteTblPaymentTermOptions(Int32 idPaymentTermOption);
        int DeleteTblPaymentTermOptions(Int32 idPaymentTermOption, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPaymentTermOption, SqlCommand cmdDelete);
    }
}
