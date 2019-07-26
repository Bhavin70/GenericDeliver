using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblTransportSlipBL
    {
        List<TblTransportSlipTO> SelectAllTblTransportSlip(DateTime tDate, int isLink);
        List<TblTransportSlipTO> SelectAllTblTransportSlipList(DateTime tDate, int isLink);
        TblTransportSlipTO SelectTblTransportSlipTO(Int32 idTransportSlip);
        int InsertTblTransportSlip(TblTransportSlipTO tblTransportSlipTO);
        int InsertTblTransportSlip(TblTransportSlipTO tblTransportSlipTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewtransportSlip(TblTransportSlipTO tblTransportSlipTO);
        int UpdateTblTransportSlip(TblTransportSlipTO tblTransportSlipTO);
        ResultMessage UpdateNewtransportSlip(TblTransportSlipTO tblTransportSlipTO);
        int UpdateTblTransportSlip(TblTransportSlipTO tblTransportSlipTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblTransportSlip(Int32 idTransportSlip);
        int DeleteTblTransportSlip(Int32 idTransportSlip, SqlConnection conn, SqlTransaction tran);
    }
}