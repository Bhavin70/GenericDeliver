using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface ITblWeighingMeasuresBL
    {
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresList();
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByTareWeight(DateTime fromDate, DateTime toDate);
        //List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId);
        //List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(string loadingId, Boolean isUnloading);
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByVehicleNo(string vehicleNo);
        TblWeighingMeasuresTO SelectTblWeighingMeasuresTO(Int32 idWeightMeasure);
        int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO);
        int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewWeighinMachineMeasurement(TblWeighingMeasuresTO tblWeighingMeasuresTO, List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList, List<TblUnLoadingItemDetTO> tblUnLoadingItemDetTOList = null);
        ResultMessage SendNotificationOfVehicaleIn(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran);
        //ResultMessage CheckInvoiceNoGeneratedByVehicleNo(string vehicleNo, SqlConnection conn, SqlTransaction tran, Boolean isForOutOnly = false);
        ResultMessage UpdateLoadingSlipExtTo(TblLoadingSlipExtTO tblLoadingSlipExtTO);
        int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO);
        int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblWeighingMeasures(Int32 idWeightMeasure);
        int DeleteTblWeighingMeasures(Int32 idWeightMeasure, SqlConnection conn, SqlTransaction tran);
    }
}