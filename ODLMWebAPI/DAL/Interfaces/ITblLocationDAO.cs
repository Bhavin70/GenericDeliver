using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLocationDAO
    {
        String SqlSelectQuery();
        List<TblLocationTO> SelectAllTblLocation();
        List<TblLocationTO> SelectAllTblLocation(Int32 parentLocationId);
        List<TblLocationTO> SelectAllParentLocation();
        List<TblLocationTO> SelectStkNotTakenCompartmentList(DateTime stockDate);
        TblLocationTO SelectTblLocation(Int32 idLocation);
        List<TblLocationTO> ConvertDTToList(SqlDataReader tblLocationTODT);
        int InsertTblLocation(TblLocationTO tblLocationTO);
        int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLocationTO tblLocationTO, SqlCommand cmdInsert);
        int UpdateTblLocation(TblLocationTO tblLocationTO);
        int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLocationTO tblLocationTO, SqlCommand cmdUpdate);
        int DeleteTblLocation(Int32 idLocation);
        int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLocation, SqlCommand cmdDelete);

    }
}