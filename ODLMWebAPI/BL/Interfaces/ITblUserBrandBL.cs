using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUserBrandBL
    {
        List<TblUserBrandTO> SelectAllTblUserBrand();
        List<TblUserBrandTO> SelectAllTblUserBrand(int isActive);
        List<TblUserBrandTO> SelectAllTblUserBrandByCnfId(Int32 cnfId);
        TblUserBrandTO SelectTblUserBrandTO(Int32 idUserBrand);
        ResultMessage SaveUserWithBrand(TblUserBrandTO tblUserBrandTO, Int32 userId);
        int InsertTblUserBrand(TblUserBrandTO tblUserBrandTO);
        int InsertTblUserBrand(TblUserBrandTO tblUserBrandTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserBrand(TblUserBrandTO tblUserBrandTO);
        int UpdateTblUserBrand(TblUserBrandTO tblUserBrandTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateTblUserBrandDetails(TblUserBrandTO tblUserBrandTO, Int32 userId);
        int DeleteTblUserBrand(Int32 idUserBrand);
        int DeleteTblUserBrand(Int32 idUserBrand, SqlConnection conn, SqlTransaction tran);
    }
}