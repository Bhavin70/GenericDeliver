using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblKYCDetailsBL
    {
        List<TblKYCDetailsTO> SelectAllTblKYCDetails();
        List<TblKYCDetailsTO> SelectTblKYCDetailsTOByOrgId(Int32 organizationId);
        TblKYCDetailsTO SelectTblKYCDetailsTO(Int32 idKYCDetails);
        TblKYCDetailsTO SelectTblKYCDetailsTOByOrg(Int32 organizationId);
        int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO);
        int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO);
        int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblKYCDetails(Int32 idKYCDetails);
        int DeleteTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran);
    }
}
