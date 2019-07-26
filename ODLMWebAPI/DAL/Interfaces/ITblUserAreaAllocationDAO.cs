using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblUserAreaAllocationDAO
    {
        String SqlSelectQuery();
        List<TblUserAreaAllocationTO> SelectAllTblUserAreaAllocation(Int32 userId, SqlConnection conn, SqlTransaction tran);
        List<UserAreaCnfDealerDtlTO> SelectAllUserAreaCnfDealerList(Int32 userId, SqlConnection conn, SqlTransaction tran);
        TblUserAreaAllocationTO SelectTblUserAreaAllocation(Int32 idAreaAllocDtl);
        List<TblUserAreaAllocationTO> ConvertDTToList(SqlDataReader tblUserAreaAllocationTODT);
        int InsertTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO);
        int InsertTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlCommand cmdInsert);
        int UpdateTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO);
        int UpdateTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlCommand cmdUpdate);
        List<TblUserAreaAllocationTO> SelectAllBookingUserAreaAllocationList(Int32 cnfOrgId, Int32 dealerOrgId, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserAreaAllocation(Int32 idAreaAllocDtl);
        int DeleteTblUserAreaAllocation(Int32 idAreaAllocDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idAreaAllocDtl, SqlCommand cmdDelete);

    }
}