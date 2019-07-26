using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblPaymentTermOptionsBL
    {
        List<TblPaymentTermOptionsTO> SelectAllTblPaymentTermOptions();
        List<TblPaymentTermOptionsTO> SelectAllTblPaymentTermOptionsList();
        TblPaymentTermOptionsTO SelectTblPaymentTermOptionsTO(Int32 idPaymentTermOption);
        int InsertTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO);
        int InsertTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO);
        int UpdateTblPaymentTermOptions(TblPaymentTermOptionsTO tblPaymentTermOptionsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPaymentTermOptions(Int32 idPaymentTermOption);
        int DeleteTblPaymentTermOptions(Int32 idPaymentTermOption, SqlConnection conn, SqlTransaction tran);
    }
}
