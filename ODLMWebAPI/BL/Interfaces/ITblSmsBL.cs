using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblSmsBL
    {
        List<TblSmsTO> SelectAllTblSmsList();
        TblSmsTO SelectTblSmsTO(Int32 idSms);
        int InsertTblSms(TblSmsTO tblSmsTO);
        int InsertTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSms(TblSmsTO tblSmsTO);
        int UpdateTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSms(Int32 idSms);
        int DeleteTblSms(Int32 idSms, SqlConnection conn, SqlTransaction tran);
    }
}