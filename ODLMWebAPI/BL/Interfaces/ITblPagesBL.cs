using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblPagesBL
    {
        List<TblPagesTO> SelectAllTblPagesList();
        TblPagesTO SelectTblPagesTO(Int32 idPage);
        int InsertTblPages(TblPagesTO tblPagesTO);
        int InsertTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPages(TblPagesTO tblPagesTO);
        int UpdateTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPages(Int32 idPage);
        int DeleteTblPages(Int32 idPage, SqlConnection conn, SqlTransaction tran);
        List<TblPagesTO> SelectAllTblPagesList(int moduleId);
    }
}