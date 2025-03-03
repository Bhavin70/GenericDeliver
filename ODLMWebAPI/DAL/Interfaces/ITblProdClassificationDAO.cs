using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblProdClassificationDAO
    {
        String SqlSelectQuery();
        List<TblProdClassificationTO> SelectAllTblProdClassification(string prodClassType = "");
        List<TblProdClassificationTO> SelectAllTblProdClassification(SqlConnection conn, SqlTransaction tran, string prodClassType = "");
        List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId);
        TblProdClassificationTO SelectTblProdClassification(Int32 idProdClass);
        List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(StaticStuff.Constants.ItemProdCategoryE itemProdCategoryE);
        List<TblProdClassificationTO> ConvertDTToList(SqlDataReader tblProdClassificationTODT);
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdInsert);
        int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int SetIsDefaultByClassificationType(TblProdClassificationTO tblProdClassificationTO);
        int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationSetDefaultCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdUpdate);
        int ExecuteUpdationCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdUpdate);
        int DeleteTblProdClassification(Int32 idProdClass);
        int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProdClass, SqlCommand cmdDelete);

    }
}