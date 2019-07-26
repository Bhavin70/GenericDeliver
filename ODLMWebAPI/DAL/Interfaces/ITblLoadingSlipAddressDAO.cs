using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingSlipAddressDAO
    {
        String SqlSelectQuery();
        List<TblLoadingSlipAddressTO> SelectAllTblLoadingSlipAddress(int loadingSlipId, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipAddressTO SelectTblLoadingSlipAddress(Int32 idLoadSlipAddr);
        List<TblLoadingSlipAddressTO> ConvertDTToList(SqlDataReader tblLoadingSlipAddressTODT);
        int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO);
        int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlCommand cmdInsert);
        int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO);
        int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr);
        int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadSlipAddr, SqlCommand cmdDelete);

    }
}