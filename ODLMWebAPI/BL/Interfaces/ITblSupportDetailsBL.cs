using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblSupportDetailsBL
    {
        List<TblSupportDetailsTO> SelectAllTblSupportDetails();
        List<TblSupportDetailsTO> SelectAllTblSupportDetailsList();
        TblSupportDetailsTO SelectTblSupportDetailsTO(Int32 idsupport);
        ResultMessage StopService(TblUserTO tblUserTO);
        int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO);
        int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage PostDeleteWeighingMeasureForSupport(TblWeighingMeasuresTO tblWeighingMeasuresTO, Int32 fromUser);
        int InsertTblStopServiceHistory(TblLoginTO tblLoginTO);
        int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO);
        ResultMessage UpdateInvoiceForSupport(TblInvoiceTO tblInvoiceTO, Int32 fromUser, String Comments);
        int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSupportDetails(Int32 idsupport);
        int DeleteTblSupportDetails(Int32 idsupport, SqlConnection conn, SqlTransaction tran);
    }
}