using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimUnitMeasuresBL
    {
        List<DimUnitMeasuresTO> SelectAllDimUnitMeasuresList();
        List<DropDownTO> SelectAllUnitMeasuresListForDropDown();
        List<DropDownTO> SelectAllUnitMeasuresForDropDownByCatId(Int32 unitCatId);
        DimUnitMeasuresTO SelectDimUnitMeasuresTO(Int32 idWeightMeasurUnit);
        int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO);
        int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO);
        int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit);
        int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit, SqlConnection conn, SqlTransaction tran);
    }
}
