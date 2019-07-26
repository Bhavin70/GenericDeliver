using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitFeedbackDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblVisitFeedback();
        DataTable SelectTblVisitFeedback();
        DataTable SelectAllTblVisitFeedback(SqlConnection conn, SqlTransaction tran);
        int InsertTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO);
        int InsertTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVisitFeedbackTO tblVisitFeedbackTO, SqlCommand cmdInsert);
        int UpdateTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO);
        int UpdateTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitFeedbackTO tblVisitFeedbackTO, SqlCommand cmdUpdate);
        int DeleteTblVisitFeedback();
        int DeleteTblVisitFeedback(SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(SqlCommand cmdDelete);

    }
}