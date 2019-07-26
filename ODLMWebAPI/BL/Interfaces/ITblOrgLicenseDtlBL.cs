using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOrgLicenseDtlBL
    {
        List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtlList(Int32 orgId);
        List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtlList(Int32 orgId, SqlConnection conn, SqlTransaction tran);
        TblOrgLicenseDtlTO SelectTblOrgLicenseDtlTO(Int32 idOrgLicense);
        List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtlList(Int32 orgId, Int32 licenseId, String licenseVal);
        int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO);
        int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO);
        int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOrgLicenseDtl(Int32 idOrgLicense);
        int DeleteTblOrgLicenseDtl(Int32 idOrgLicense, SqlConnection conn, SqlTransaction tran);
    }
}