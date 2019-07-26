using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblCompetitorUpdatesBL
    { 
        List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList();
        List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList(Int32 competitorId, Int32 enteredBy, DateTime fromDate, DateTime toDate);
        TblCompetitorUpdatesTO SelectTblCompetitorUpdatesTO(Int32 idCompeUpdate);
        List<DropDownTO> SelectCompeUpdateUserDropDown();
        TblCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId);
        int InsertTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO);
        int InsertTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO);
        int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblCompetitorUpdates(Int32 idCompeUpdate);
        int DeleteTblCompetitorUpdates(Int32 idCompeUpdate, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveMarketUpdate(List<TblCompetitorUpdatesTO> competitorUpdatesTOList);

    }
}
