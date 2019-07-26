using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblMaterialBL
    {
        List<TblMaterialTO> SelectAllTblMaterialList();
        List<DropDownTO> SelectAllMaterialListForDropDown();
        dynamic GetDynamicObject(Dictionary<string, object> properties);
        TblMaterialTO SelectTblMaterialTO(Int32 idMaterial);
        List<DropDownTO> SelectMaterialTypeDropDownList();
        int InsertTblMaterial(TblMaterialTO tblMaterialTO);
        int InsertTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblMaterial(TblMaterialTO tblMaterialTO);
        int DeleteTblMaterial(Int32 idMaterial);
        int DeleteTblMaterial(Int32 idMaterial, SqlConnection conn, SqlTransaction tran);
    }
}