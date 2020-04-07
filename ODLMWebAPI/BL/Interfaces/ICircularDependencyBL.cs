using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ICircularDependencyBL
    {
        List<TblBookingScheduleTO> SelectBookingScheduleByBookingId(Int32 bookingId);
        List<TblLoadingSlipTO> SelectAllLoadingSlipListWithDetails(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId);
        ResultMessage CheckInvoiceNoGeneratedByVehicleNo(string vehicleNo, SqlConnection conn, SqlTransaction tran, Boolean isForOutOnly = false);
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId, SqlConnection conn, SqlTransaction tran);
        TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking);  //Rupali and Sameeksha
        //ResultMessage UploadDocumentWithConnTran(List<TblDocumentDetailsTO> tblDocumentDetailsTOList, SqlConnection conn, SqlTransaction tran);
    }
}
