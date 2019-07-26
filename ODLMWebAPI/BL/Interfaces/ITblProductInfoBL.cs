using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblProductInfoBL
    { 
        List<TblProductInfoTO> SelectAllTblProductInfoList();
        List<TblProductInfoTO> SelectAllTblProductInfoList(SqlConnection conn, SqlTransaction tran);
        List<TblProductInfoTO> SelectAllTblProductInfoListLatest();
        TblProductInfoTO SelectTblProductInfoTO(Int32 idProduct);
        List<TblProductInfoTO> SelectAllEmptyProductInfoList(int prodCatId);
        List<TblProductInfoTO> SelectProductInfoListByLoadingSlipExtIds(string strLoadingSlipExtIds);
        int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO);
        int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO);
        int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProductInfo(Int32 idProduct);
        int DeleteTblProductInfo(Int32 idProduct, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveProductInformation(List<TblProductInfoTO> productInfoTOList);
    }
}