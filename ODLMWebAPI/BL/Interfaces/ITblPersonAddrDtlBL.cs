using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblPersonAddrDtlBL
    {
        List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtl();
        List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtlList();
        TblPersonAddrDtlTO SelectTblPersonAddrDtlTO(Int32 idPersonAddrDtl);
        TblPersonAddrDtlTO SelectTblPersonAddrDtlTO(Int32 personId, Int32 addressTypeId);
        TblAddressTO SelectAddressTOonPersonAddrDtlId(Int32 personId, Int32 addressTypeId);
        int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO);
        int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO);
        int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl);
        int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl, SqlConnection conn, SqlTransaction tran);
    }
}