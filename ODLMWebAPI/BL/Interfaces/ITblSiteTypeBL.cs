using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblSiteTypeBL
    {
        List<TblSiteTypeTO> SelectAllTblSiteTypeList();
        TblSiteTypeTO SelectTblSiteTypeTO(Int32 idSiteType);
        List<TblSiteTypeTO> ConvertDTToList(DataTable tblSiteTypeTODT);
        int InsertTblSiteType(TblSiteTypeTO tblSiteTypeTO);
        int InsertTblSiteType(ref TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO);
        int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSiteType(Int32 idSiteType);
        int DeleteTblSiteType(Int32 idSiteType, SqlConnection conn, SqlTransaction tran);
    }
}