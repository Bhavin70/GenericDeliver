using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{  
    public interface ITblLoginBL
    {
        List<TblLoginTO> SelectAllTblLoginList();
        TblLoginTO SelectTblLoginTO(Int32 idLogin);
        TblUserTO getPermissionsOnModule(int userId, int moduleId);
        int InsertTblLogin(TblLoginTO tblLoginTO);
        int InsertTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage LogIn(TblUserTO tblUserTO);
        ResultMessage LogOut(TblUserTO tblUserTO);
        int UpdateTblLogin(TblLoginTO tblLoginTO);
        int UpdateTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLogin(Int32 idLogin);
        int DeleteTblLogin(Int32 idLogin, SqlConnection conn, SqlTransaction tran);
        //Boolean GetUsersQuata(SqlConnection conn, SqlTransaction tran);
        List<TblLoginTO> GetCurrentActiveUsers();
        dimUserConfigrationTO GetUsersConfigration(int ConfigDesc);
        dimUserConfigrationTO GetUsersConfigration(int ConfigDesc, SqlConnection conn, SqlTransaction tran);
    }
}