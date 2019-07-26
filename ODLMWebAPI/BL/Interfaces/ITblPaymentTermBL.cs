using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblPaymentTermBL
    {
        DataTable SelectAllTblPaymentTerm();
        List<TblPaymentTermTO> SelectAllTblPaymentTermList();
        TblPaymentTermTO SelectTblPaymentTermTO(Int32 idPaymentTerm);
        List<DropDownTO> SelectPaymentTermListForDopDown();
        List<TblPaymentTermTO> ConvertDTToList(DataTable tblPaymentTermTODT);
        int InsertTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO);
        int InsertTblPaymentTerm(ref TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO);
        int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPaymentTerm(Int32 idPaymentTerm);
        int DeleteTblPaymentTerm(Int32 idPaymentTerm, SqlConnection conn, SqlTransaction tran);
    }
}