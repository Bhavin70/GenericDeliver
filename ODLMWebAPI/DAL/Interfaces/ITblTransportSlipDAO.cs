using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblTransportSlipDAO
    {
        String SqlSelectQuery();
        List<TblTransportSlipTO> SelectAllTblTransportSlip(DateTime tDate, int isLink);
        TblTransportSlipTO SelectTblTransportSlip(Int32 idTransportSlip);
        List<TblTransportSlipTO> SelectAllTblTransportSlip(SqlConnection conn, SqlTransaction tran);
        List<TblTransportSlipTO> ConvertDTToList(SqlDataReader tblTransportSlipTODT);
        int InsertTblTransportSlip(TblTransportSlipTO tblTransportSlipTO);
        int InsertTblTransportSlip(TblTransportSlipTO tblTransportSlipTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblTransportSlipTO tblTransportSlipTO, SqlCommand cmdInsert);
        int UpdateTblTransportSlip(TblTransportSlipTO tblTransportSlipTO);
        int UpdateTblTransportSlip(TblTransportSlipTO tblTransportSlipTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblTransportSlipTO tblTransportSlipTO, SqlCommand cmdUpdate);
        int DeleteTblTransportSlip(Int32 idTransportSlip);
        int DeleteTblTransportSlip(Int32 idTransportSlip, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTransportSlip, SqlCommand cmdDelete);

    }
}