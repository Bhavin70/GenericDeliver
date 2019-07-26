using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblFreightUpdateBL
    {
        List<TblFreightUpdateTO> SelectAllTblFreightUpdateList();
        TblFreightUpdateTO SelectTblFreightUpdateTO(Int32 idFreightUpdate);
        int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO);
        int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO);
        int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblFreightUpdate(Int32 idFreightUpdate);
        int DeleteTblFreightUpdate(Int32 idFreightUpdate, SqlConnection conn, SqlTransaction tran);
        List<TblFreightUpdateTO> SelectAllTblFreightUpdateList(DateTime frmDt, DateTime toDt, int districtId, int talukaId);
    }
}
