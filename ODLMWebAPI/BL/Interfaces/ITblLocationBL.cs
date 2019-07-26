using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLocationBL
    {
        List<TblLocationTO> SelectAllTblLocationList();
        List<TblLocationTO> SelectAllCompartmentLocationList(Int32 parentLocationId);
        List<TblLocationTO> SelectAllParentLocation();
        TblLocationTO SelectTblLocationTO(Int32 idLocation);
        List<TblLocationTO> SelectStkNotTakenCompartmentList(DateTime stockDate);
        int InsertTblLocation(TblLocationTO tblLocationTO);
        int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLocation(TblLocationTO tblLocationTO);
        int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLocation(Int32 idLocation);
        int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran);
    }
}