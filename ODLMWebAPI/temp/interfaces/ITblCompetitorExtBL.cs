using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblCompetitorExtBL
    {
        List<TblCompetitorExtTO> SelectAllTblCompetitorExtList();
        TblCompetitorExtTO SelectTblCompetitorExtTO(Int32 idCompetitorExt);
        List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId);
        List<TblCompetitorExtTO> SelectAllTblCompetitorExtList(Int32 orgId);
        List<TblCompetitorExtTO> SelectAllTblCompetitorExtList(Int32 orgId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectAllCompetitorDropDownList();
        int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO);
        int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO);
        int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblCompetitorExt(Int32 idCompetitorExt);
        int DeleteTblCompetitorExt(Int32 idCompetitorExt, SqlConnection conn, SqlTransaction tran);
    }
}
