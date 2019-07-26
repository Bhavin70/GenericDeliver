using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOrgAddressBL
    {
        List<TblOrgAddressTO> SelectAllTblOrgAddressList();
        TblOrgAddressTO SelectTblOrgAddressTO(Int32 idOrgAddr);
        int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO);
        int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO);
        int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOrgAddress(Int32 idOrgAddr);
        int DeleteTblOrgAddress(Int32 idOrgAddr, SqlConnection conn, SqlTransaction tran);
    }
}