using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUserLocationBL
    {
        TblUserLocationTO SelectAllTblUserLocation();
        List<TblUserLocationTO> SelectAllTblUserLocationList();
        TblUserLocationTO SelectTblUserLocationTO();
        List<TblUserLocationTO> getUserLastLocationListOnUserId(string userIds);
        List<TblUserLocationTO> SelectPlanRoute(Int32 UserId, StaticStuff.Constants.RouteTypeE RouteTypeE);
        List<nearBymeTo> getNearBycustomer(int distance, int siteType, string currentLat, string currentLng,DateTime? visitDate,Int32 userId);
        List<nearBymeTo> SelectNearByDealer(int distance, Int32 cnfId, TblUserRoleTO tblUserRoleTO);
        int InsertTblUserLocation(TblUserLocationTO tblUserLocationTO);
        int InsertTblUserLocation(TblUserLocationTO tblUserLocationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserLocation(TblUserLocationTO tblUserLocationTO);
        int UpdateTblUserLocation(TblUserLocationTO tblUserLocationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserLocation(Int32 idlocation, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserLocationPreviousDays(TblUserLocationTO tblUserLocationTO);
         dynamic getMatrixAPI(List<TblUserLocationTO> userLocList);
        dynamic getMatrixAPIUsingMapMyIndia(List<TblUserLocationTO> userLocList);
        dynamic autoSerachMapMyindia(String query);

    }
}