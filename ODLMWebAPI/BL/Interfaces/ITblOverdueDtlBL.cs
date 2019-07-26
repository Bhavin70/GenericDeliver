using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOverdueDtlBL
    {
        List<TblOverdueDtlTO> SelectAllTblOverdueDtlList();
        List<TblOverdueDtlTO> SelectAllTblOverdueDtlList(String dealerIds);
        TblOverdueDtlTO SelectTblOverdueDtlTO(Int32 idOverdueDtl);
        List<TblOverdueDtlTO> SelectTblOverdueDtlList(Int32 dealerId);
        int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO);
        int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveOrgOverDueDtl(List<TblOverdueDtlTO> tblOverdueDtlTOList, Int32 loginUserId);
        int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO);
        int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOverdueDtl(Int32 idOverdueDtl);
        int DeleteTblOverdueDtl(Int32 idOverdueDtl, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOverdueDtl(SqlConnection conn, SqlTransaction tran);
    }
}