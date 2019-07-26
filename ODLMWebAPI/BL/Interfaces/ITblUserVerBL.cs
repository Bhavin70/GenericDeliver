using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUserVerBL
    {
        List<TblUserVerTO> SelectAllTblUserVer();
        List<TblUserVerTO> SelectAllTblUserVerList();
        TblUserVerTO SelectTblUserVerTO(Int32 idUserVer);
        int InsertTblUserVer(TblUserVerTO tblUserVerTO);
        int InsertTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserVer(TblUserVerTO tblUserVerTO);
        int UpdateTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserVer(Int32 idUserVer);
        int DeleteTblUserVer(Int32 idUserVer, SqlConnection conn, SqlTransaction tran);
    }
}