using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVersionBL
    {
        List<TblVersionTO> SelectAllTblVersionList();
        TblVersionTO SelectTblVersionTO(Int32 idVersion);
        TblVersionTO SelectLatestVersionTO();
        int InsertTblVersion(TblVersionTO tblVersionTO);
        int InsertTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVersion(TblVersionTO tblVersionTO);
        int UpdateTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVersion(Int32 idVersion);
        int DeleteTblVersion(Int32 idVersion, SqlConnection conn, SqlTransaction tran);
    }
}