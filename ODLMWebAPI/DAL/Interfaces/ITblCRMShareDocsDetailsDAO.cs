using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblCRMShareDocsDetailsDAO
    {
        String SqlSelectQuery();
        List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetails();
        TblCRMShareDocsDetailsTO SelectTblCRMShareDocsDetails(Int32 idShareDoc);
        List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetails(SqlConnection conn, SqlTransaction tran);
        int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO);
        int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlCommand cmdInsert);
        int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO);
        int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblCRMShareDocsDetails(Int32 idShareDoc);
        int DeleteTblCRMShareDocsDetails(Int32 idShareDoc, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idShareDoc, SqlCommand cmdDelete);
        List<TblCRMShareDocsDetailsTO> ConvertDTToList(SqlDataReader tblCRMShareDocsDetailsTODT);

    }
}