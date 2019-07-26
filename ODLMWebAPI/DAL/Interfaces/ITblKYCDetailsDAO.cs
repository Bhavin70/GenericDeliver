using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblKYCDetailsDAO
    {
        String SqlSelectQuery();
        List<TblKYCDetailsTO> SelectAllTblKYCDetails();
        TblKYCDetailsTO SelectTblKYCDetails(Int32 idKYCDetails);
        TblKYCDetailsTO SelectTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran);
        TblKYCDetailsTO SelectTblKYCDetailsTOByOrgId(Int32 organizationId, SqlConnection conn, SqlTransaction tran);
        List<TblKYCDetailsTO> SelectTblKYCDetailsTOByOrgId(Int32 organizationId);
        List<TblKYCDetailsTO> SelectAllTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran);
        List<TblKYCDetailsTO> ConvertDTToList(SqlDataReader tblKYCDetailsTODT);
        int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO);
        int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblKYCDetailsTO tblKYCDetailsTO, SqlCommand cmdInsert);
        int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO);
        int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblKYCDetailsTO tblKYCDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblKYCDetails(Int32 idKYCDetails);
        int DeleteTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idKYCDetails, SqlCommand cmdDelete);

    }
}