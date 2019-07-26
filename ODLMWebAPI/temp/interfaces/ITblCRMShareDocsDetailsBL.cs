using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblCRMShareDocsDetailsBL
    {
        List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetails();
        List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetailsList();
        TblCRMShareDocsDetailsTO SelectTblCRMShareDocsDetailsTO(Int32 idShareDoc);
        int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO);
        int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO);
        int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblCRMShareDocsDetails(Int32 idShareDoc);
        int DeleteTblCRMShareDocsDetails(Int32 idShareDoc, SqlConnection conn, SqlTransaction tran);
        ResultMessage ShareDetials(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO);
        ResultMessage SendEmailToSelectedPersons(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO);
    }
}
