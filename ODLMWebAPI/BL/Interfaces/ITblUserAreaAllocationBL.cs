using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUserAreaAllocationBL
    { 
        TblUserAreaAllocationTO SelectTblUserAreaAllocationTO(Int32 idAreaAllocDtl);
        List<TblUserAreaAllocationTO> SelectAllTblUserAreaAllocationList(int userId);
        List<TblUserAreaAllocationTO> SelectAllTblUserAreaAllocationList(int userId, SqlConnection conn, SqlTransaction tran);
        List<UserAreaCnfDealerDtlTO> SelectAllUserAreaCnfDealerList(int userId);
        List<TblUserAreaAllocationTO> SelectAllBookingUserAreaAllocationList(Int32 cnfOrgId, Int32 dealerOrgId, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        int InsertTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO);
        int InsertTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO);
        int UpdateTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserAreaAllocation(Int32 idAreaAllocDtl);
        int DeleteTblUserAreaAllocation(Int32 idAreaAllocDtl, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveUserAreaAllocation(List<TblUserAreaAllocationTO> userAreaAllocationTOList);
    }
}