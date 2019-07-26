using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimVehicleTypeBL
    {
        List<DimVehicleTypeTO> SelectAllDimVehicleTypeList();
        DimVehicleTypeTO SelectDimVehicleTypeTO(Int32 idVehicleType);
        int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO);
        int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO);
        int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimVehicleType(Int32 idVehicleType);
        int DeleteDimVehicleType(Int32 idVehicleType, SqlConnection conn, SqlTransaction tran);
    }
}
