using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimUnitMeasuresDAO
    {
        String SqlSelectQuery();
        List<DropDownTO> SelectAllUnitMeasuresForDropDown();
        List<DropDownTO> SelectAllUnitMeasuresForDropDownByCatId(Int32 unitCatId);
        List<DimUnitMeasuresTO> SelectAllDimUnitMeasures();
        DimUnitMeasuresTO SelectDimUnitMeasures(Int32 idWeightMeasurUnit);
        List<DimUnitMeasuresTO> ConvertDTToList(SqlDataReader dimUnitMeasuresTODT);
        int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO);
        int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimUnitMeasuresTO dimUnitMeasuresTO, SqlCommand cmdInsert);
        int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO);
        int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimUnitMeasuresTO dimUnitMeasuresTO, SqlCommand cmdUpdate);
        int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit);
        int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idWeightMeasurUnit, SqlCommand cmdDelete);

    }
}