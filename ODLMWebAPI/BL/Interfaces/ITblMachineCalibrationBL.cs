using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblMachineCalibrationBL
    {
        List<TblMachineCalibrationTO> SelectAllTblMachineCalibration();
        List<TblMachineCalibrationTO> SelectAllTblMachineCalibrationList();
        TblMachineCalibrationTO SelectTblMachineCalibrationTO(Int32 idMachineCalibration);
        TblMachineCalibrationTO SelectTblMachineCalibrationTOByWeighingMachineId(Int32 weighingMachineId);
        ResultMessage InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO);
        int InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO);
        int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblMachineCalibration(Int32 idMachineCalibration);
        int DeleteTblMachineCalibration(Int32 idMachineCalibration, SqlConnection conn, SqlTransaction tran);
    }
}