using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblPurchaseCompetitorExtBL
    {
        List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList();
        TblPurchaseCompetitorExtTO SelectTblPurchaseCompetitorExtTO(Int32 idPurCompetitorExt);
        int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO);
        int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList(Int32 organizationId);
        List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList(Int32 organizationId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO);
        int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt);
        int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt, SqlConnection conn, SqlTransaction tran);
    }
}