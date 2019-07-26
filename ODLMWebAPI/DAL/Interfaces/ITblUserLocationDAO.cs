using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblUserLocationDAO
    {
        String SqlSelectQuery();
        String SqlSelectQueryForNearByDealers();
        List<TblUserLocationTO> SelectAllTblUserLocation();
        TblUserLocationTO SelectTblUserLocation();
        List<TblUserLocationTO> SelectAllTblUserLocation(SqlConnection conn, SqlTransaction tran);
        List<TblUserLocationTO> UserLastLocationListOnUserId(string userIds);
        List<TblUserLocationTO> SelectActualPlanRoute(Int32 UserId);
        List<TblUserLocationTO> SelectSuggestedPlanRoute(Int32 UserId);
        List<nearBymeTo> getNearBycustomer(int distance, int siteType, string currentLat, string currentLng, DateTime? visitDate,string userIds);
        List<nearBymeTo> SelectNearByDealer(int distance, Int32 cnfId, TblUserRoleTO tblUserRoleTO);
        int InsertTblUserLocation(TblUserLocationTO tblUserLocationTO);
        int InsertTblUserLocation(TblUserLocationTO tblUserLocationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserLocationTO tblUserLocationTO, SqlCommand cmdInsert);
        int UpdateTblUserLocation(TblUserLocationTO tblUserLocationTO);
        int UpdateTblUserLocation(TblUserLocationTO tblUserLocationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUserLocationTO tblUserLocationTO, SqlCommand cmdUpdate);
        int DeleteTblUserLocation(Int32 idlocation, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idlocation, SqlCommand cmdDelete);
        int DeleteTblUserLocationPreviousDays(TblUserLocationTO tblUserLocationTO);
        int ExecuteInsertionCommandOnDays(TblUserLocationTO tblUserLocationTO, SqlCommand cmdDelete);
        List<TblUserLocationTO> ConvertDTToList(SqlDataReader tblUserLocationTODT);
        List<nearBymeTo> ConvertDTToListNearByMe(SqlDataReader NearByMeTODT);

    }
}