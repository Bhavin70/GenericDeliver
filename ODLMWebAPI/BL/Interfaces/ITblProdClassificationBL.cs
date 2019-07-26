using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblProdClassificationBL
    {
        List<TblProdClassificationTO> SelectAllTblProdClassificationList(string prodClassType = "");
        List<TblProdClassificationTO> SelectAllTblProdClassificationList(SqlConnection conn, SqlTransaction tran, string prodClassType = "");
        List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId);
        TblProdClassificationTO SelectTblProdClassificationTO(Int32 idProdClass);
        List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(StaticStuff.Constants.ItemProdCategoryE itemProdCategoryE);
        void SetProductClassificationDisplayName(TblProdClassificationTO tblProdClassificationTO, List<TblProdClassificationTO> allProdClassificationList);
        void GetDisplayName(List<TblProdClassificationTO> allProdClassificationList, int parentId, List<TblProdClassificationTO> DisplayNameList);
        string SelectProdtClassificationListOnType(Int32 idProdClass);
        void GetIdsofProductClassification(List<TblProdClassificationTO> allList, int parentId, ref String ids);
        List<TblProdClassificationTO> SelectProductClassificationListByProductItemId(Int32 prodItemId);
        List<TblProdClassificationTO> SelectProductChildList(List<TblProdClassificationTO> tblProdClassificationTOlist, Int32 parentId);
        int InsertProdClassification(TblProdClassificationTO tblProdClassificationTO);
        List<Int32> GetDefaultProductClassification();
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDisplayName(List<TblProdClassificationTO> allProdClassificationList, TblProdClassificationTO ProdClassificationTO, ref String idClassStr, SqlConnection conn, SqlTransaction tran);
        int UpdateProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProdClassification(Int32 idProdClass);
        int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran);
    }
}