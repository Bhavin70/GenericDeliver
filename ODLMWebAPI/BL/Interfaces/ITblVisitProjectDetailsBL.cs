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
    public interface ITblVisitProjectDetailsBL
    {
        List<TblVisitProjectDetailsTO> SelectProjectDetailsList(Int32 visitId);
        List<TblVisitProjectDetailsTO> SelectAllTblVisitProjectDetailsList();
        TblVisitProjectDetailsTO SelectTblProjectDetailsTO(Int32 idProject);
        List<TblVisitProjectDetailsTO> ConvertDTToList(DataTable tblProjectDetailsTODT);
        int InsertTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO);
        int InsertTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveVisitProjectDetails(List<TblVisitProjectDetailsTO> tblVisitProjectDetailsTOList, Int32 createdBy, Int32 visitId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO);
        int UpdateTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateVisitProjectDetails(List<TblVisitProjectDetailsTO> tblVisitProjectDetailsTOList, Int32 updatedBy, Int32 visitId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProjectDetails(Int32 idProject);
        int DeleteTblProjectDetails(Int32 idProject, SqlConnection conn, SqlTransaction tran);
    }
}