using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblEmailConfigrationBL
    {
        List<TblEmailConfigrationTO> SelectAllDimEmailConfigration();
        List<TblEmailConfigrationTO> SelectAllDimEmailConfigrationList();
        TblEmailConfigrationTO SelectDimEmailConfigrationTO();
        int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO);
        int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO);
        int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimEmailConfigration(Int32 idEmailConfig);
        int DeleteDimEmailConfigration(Int32 idEmailConfig, SqlConnection conn, SqlTransaction tran);
    }
}
