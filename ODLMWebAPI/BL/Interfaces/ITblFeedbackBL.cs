using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblFeedbackBL
    { 
        List<TblFeedbackTO> SelectAllTblFeedbackList();
        TblFeedbackTO SelectTblFeedbackTO(Int32 idFeedback);
        int InsertTblFeedback(TblFeedbackTO tblFeedbackTO);
        int InsertTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO);
        int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblFeedback(Int32 idFeedback);
        int DeleteTblFeedback(Int32 idFeedback, SqlConnection conn, SqlTransaction tran);
        List<TblFeedbackTO> SelectAllTblFeedbackList(int userId, DateTime frmDt, DateTime toDt);
    }
}
