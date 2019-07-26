using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblCompetitorUpdatesDAO
    {
        String SqlSelectQuery();
        List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdates();
        List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdates(Int32 competitorId, Int32 enteredBy, DateTime fromDate, DateTime toDate);
        TblCompetitorUpdatesTO SelectTblCompetitorUpdates(Int32 idCompeUpdate);
        List<TblCompetitorUpdatesTO> ConvertDTToList(SqlDataReader tblCompetitorUpdatesTODT);
        List<DropDownTO> SelectCompeUpdateUserDropDown();
        TblCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId);
        int InsertTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO);
        int InsertTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlCommand cmdInsert);
        int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO);
        int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlCommand cmdUpdate);
        int DeleteTblCompetitorUpdates(Int32 idCompeUpdate);
        int DeleteTblCompetitorUpdates(Int32 idCompeUpdate, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idCompeUpdate, SqlCommand cmdDelete);

    }
}