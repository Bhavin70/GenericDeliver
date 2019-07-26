using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblBookingQtyConsumptionDAO
    {
        String SqlSelectQuery();
        List<TblBookingQtyConsumptionTO> SelectAllTblBookingQtyConsumption();
        List<TblBookingQtyConsumptionTO> SelectAllTblBookingQtyConsumption(DateTime asOnDate);
        TblBookingQtyConsumptionTO SelectTblBookingQtyConsumption(Int32 idBookQtyConsuption);
        List<TblBookingQtyConsumptionTO> ConvertDTToList(SqlDataReader tblBookingQtyConsumptionTODT);
        int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO);
        int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlCommand cmdInsert);
        int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO);
        int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlCommand cmdUpdate);
        int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption);
        int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBookQtyConsuption, SqlCommand cmdDelete);

    }
}