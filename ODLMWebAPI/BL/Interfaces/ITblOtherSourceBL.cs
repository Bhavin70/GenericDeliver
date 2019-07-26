using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOtherSourceBL
    {
        List<TblOtherSourceTO> SelectAllTblOtherSourceList();
        TblOtherSourceTO SelectTblOtherSourceTO(Int32 idOtherSource);
        List<TblOtherSourceTO> SelectTblOtherSourceListFromDesc(string OtherSourceDesc);
        List<DropDownTO> SelectOtherSourceOfMarketTrendForDropDown();
        int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO);
        int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO);
        int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOtherSource(Int32 idOtherSource);
        int DeleteTblOtherSource(Int32 idOtherSource, SqlConnection conn, SqlTransaction tran);
    }
}