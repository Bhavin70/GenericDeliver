using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVisitFeedbackBL
    {
        DataTable SelectAllTblVisitFeedback();
        List<TblVisitFeedbackTO> SelectAllTblVisitFeedbackList();
        TblVisitFeedbackTO SelectTblVisitFeedbackTO();
        List<TblVisitFeedbackTO> ConvertDTToList(DataTable tblVisitFeedbackTODT);
        int InsertTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO);
        int InsertTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO);
        int UpdateTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVisitFeedback();
        int DeleteTblVisitFeedback(SqlConnection conn, SqlTransaction tran);
    }
}