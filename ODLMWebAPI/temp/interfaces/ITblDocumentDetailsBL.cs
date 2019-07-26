using Microsoft.AspNetCore.Http;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblDocumentDetailsBL
    {
        List<TblDocumentDetailsTO> SelectAllTblDocumentDetails();
        List<TblDocumentDetailsTO> SelectAllTblDocumentDetailsList();
        TblDocumentDetailsTO SelectTblDocumentDetailsTO(Int32 idDocument);
        List<TblDocumentDetailsTO> GetUploadedFileBasedOnFileType(Int32 fileTypeId, Int32 createdBy);
        List<TblDocumentDetailsTO> GetUploadedFileBasedOnDocumentId(string DocumentIds);
        int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO);
        int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO);
        int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblDocumentDetails(Int32 idDocument);
        int DeleteTblDocumentDetails(Int32 idDocument, SqlConnection conn, SqlTransaction tran);
        Task<ResultMessage> UploadFileAsync(TblDocumentDetailsTO tblDocumentDetailsTO);
        ResultMessage UploadUserProfilePicture(TblDocumentDetailsTO tblDocumentDetailsTO);
        ResultMessage UploadDocument(List<TblDocumentDetailsTO> tblDocumentDetailsTOList);
        ResultMessage UploadDocumentWithConnTran(List<TblDocumentDetailsTO> tblDocumentDetailsTOList, SqlConnection conn, SqlTransaction tran);
        Task<List<TblDocumentDetailsTO>> UploadMultipleTypesFile(List<IFormFile> files, Int32 createdBy, Int32 FileTypeId, Int32 moduleId);
    }
}
