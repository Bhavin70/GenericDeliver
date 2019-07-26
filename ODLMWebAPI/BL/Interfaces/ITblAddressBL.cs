using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblAddressBL
    {
        List<TblAddressTO> SelectAllTblAddressList();
        TblAddressTO SelectTblAddressTO(Int32 idAddr);
        TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE = StaticStuff.Constants.AddressTypeE.OFFICE_ADDRESS);
        TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE, SqlConnection conn, SqlTransaction tran);
        List<TblAddressTO> SelectOrgAddressList(Int32 orgId);
        List<TblAddressTO> SelectOrgAddressDetailOfRegion(string orgId, StaticStuff.Constants.AddressTypeE addressTypeE = StaticStuff.Constants.AddressTypeE.OFFICE_ADDRESS);
        int InsertTblAddress(TblAddressTO tblAddressTO);
        int InsertTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAddress(TblAddressTO tblAddressTO);
        int UpdateTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAddress(Int32 idAddr);
        int DeleteTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran);
    }
}
