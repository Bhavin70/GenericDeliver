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
    public interface ITblVisitDetailsBL
    {
        List<TblVisitDetailsTO> SelectAllTblVisitDetails();
        List<TblVisitDetailsTO> SelectAllTblVisitDetailsList();
        TblVisitDetailsTO SelectTblVisitDetailsTO(int idVisit);
        List<TblVisitDetailsTO> ConvertDTToList(DataTable tblVisitDetailsTODT);
        Int32 SelectLastVisitId();
        List<TblVisitDetailsTO> SelectAllVisitDetailsList();
        int InsertTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO);
        int InsertTblVisitDetails(ref TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVisitDetails(int idVisit);
        int DeleteTblVisitDetails(int idVisit, SqlConnection conn, SqlTransaction tran);
    }
}