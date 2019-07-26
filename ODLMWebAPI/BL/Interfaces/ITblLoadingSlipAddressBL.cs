using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingSlipAddressBL
    {
        List<TblLoadingSlipAddressTO> SelectAllTblLoadingSlipAddressList(Int32 loadingSlipId);
        List<TblLoadingSlipAddressTO> SelectAllTblLoadingSlipAddressList(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipAddressTO SelectTblLoadingSlipAddressTO(Int32 idLoadSlipAddr);
        int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO);
        int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO);
        int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr);
        int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr, SqlConnection conn, SqlTransaction tran);
    }
}