using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingVehDocExtBL
    {
        List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExt();
        List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExtList();
        List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExtListEmptyAgainstLLoading(Int32 loadingId, Int32 getEmptyList);
        List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExtList(Int32 loadingId, Int32 ActiveYnAll);
        TblLoadingVehDocExtTO SelectTblLoadingVehDocExtTO(Int32 idLoadingVehDocExt);
        int InsertTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO);
        int InsertTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingVehDocExt(List<TblLoadingVehDocExtTO> tblLoadingVehDocExtTOList, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO);
        int UpdateTblLoadingVehDocExtActiveYn(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingVehDocExt(Int32 idLoadingVehDocExt);
        int DeleteTblLoadingVehDocExt(Int32 idLoadingVehDocExt, SqlConnection conn, SqlTransaction tran);
    }
}