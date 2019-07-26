using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblStatusReasonBL
    {
        List<TblStatusReasonTO> SelectAllTblStatusReasonList();
        TblStatusReasonTO SelectTblStatusReasonTO(Int32 idStatusReason);
        List<TblStatusReasonTO> SelectAllTblStatusReasonList(Int32 statusId);
        int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO);
        int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO);
        int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblStatusReason(Int32 idStatusReason);
        int DeleteTblStatusReason(Int32 idStatusReason, SqlConnection conn, SqlTransaction tran);
    }
}