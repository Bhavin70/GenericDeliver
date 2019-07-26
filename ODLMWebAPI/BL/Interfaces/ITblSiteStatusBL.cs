using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblSiteStatusBL
    {
        DataTable SelectAllTblSiteStatus();
        List<TblSiteStatusTO> SelectAllTblSiteStatusList();
        TblSiteStatusTO SelectTblSiteStatusTO(Int32 idSiteStatus);
        List<DropDownTO> SelectAllSiteStatusForDropDown();
        List<TblSiteStatusTO> ConvertDTToList(DataTable tblSiteStatusTODT);
        int InsertTblSiteStatus(TblSiteStatusTO tblSiteStatusTO);
        int InsertTblSiteStatus(ref TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewSiteStatus(TblSiteStatusTO tblSiteStatusTO);
        int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO);
        int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSiteStatus(Int32 idSiteStatus);
        int DeleteTblSiteStatus(Int32 idSiteStatus, SqlConnection conn, SqlTransaction tran);
    }
}