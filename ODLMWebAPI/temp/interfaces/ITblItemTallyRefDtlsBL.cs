using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblItemTallyRefDtlsBL
    {
        List<TblItemTallyRefDtlsTO> SelectAllTblItemTallyRefDtlsList();
        TblItemTallyRefDtlsTO SelectTblItemTallyRefDtlsTO(Int32 idItemTallyRef);
        List<TblItemTallyRefDtlsTO> SelectExistingAllTblOrganizationByRefIds(String overdueRefId, String enqRefId);
        List<TblItemTallyRefDtlsTO> SelectEmptyTblItemTallyRefDtlsTOTemplate(Int32 brandId);
        List<TblItemTallyRefDtlsTO> SelectAllTallyRefDtlTOList(int brandId);
        TblItemTallyRefDtlsTO SelectExistingAllTblItemRef(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO);
        int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO);
        int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewItemTallyRef(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO);
        int UpdateTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO);
        int UpdateTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateItemTallyRef(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO);
        int DeleteTblItemTallyRefDtls(Int32 idItemTallyRef);
        int DeleteTblItemTallyRefDtls(Int32 idItemTallyRef, SqlConnection conn, SqlTransaction tran);
    }
}
