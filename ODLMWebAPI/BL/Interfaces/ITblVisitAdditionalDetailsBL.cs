using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVisitAdditionalDetailsBL
    {
        DataTable SelectAllTblVisitAdditionalDetails();
        List<TblVisitAdditionalDetailsTO> SelectAllTblVisitAdditionalDetailsList();
        TblVisitAdditionalDetailsTO SelectTblVisitAdditionalDetailsTO(Int32 idVisitDetails);
        List<TblVisitAdditionalDetailsTO> ConvertDTToList(DataTable tblVisitAdditionalDetailsTODT);
        TblVisitAdditionalDetailsTO SelectVisitAdditionalDetailsTO(Int32 visitId);
        int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO);
        int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblVisitAdditionalDetailsAbountCompany(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveVisitAdditionalInfo(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO);
        int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateVisitAdditionalInfo(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails);
        int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails, SqlConnection conn, SqlTransaction tran);
    }
}