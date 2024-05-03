using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimProdSpecDescBL
    {
        List<DimProdSpecDescTO> SelectAllDimProdSpecDescList();
        DimProdSpecDescTO SelectDimPRodSpecDescTO(Int32 idCodeType);
        int SelectAllDimProdSpecDescriptionList();
        List<TblPipesTO> SelectAllTblPipesList(); 
        List<TblStripsTO> SelectAllTblStripsList(); 
        List<DropDownTO> SelectAllPipesInchForDropDown();
        int InsertDimProdSpecDesc(DimProdSpecDescTO ProSpecDesc);
        int InsertDimProdSpecDesc(DimProdSpecDescTO dimProSpecDescTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO);
        int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO);
        int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO, SqlConnection conn, SqlTransaction tran);
        //List<TblPipesDropDownTo> GetAllPipesInchForDropDown();
        List<TblPipesStripCommonSizeTO> GetAlltblPipesStripCommonSizeForDropDown();
        List<TblPipesStripCommonThicknessTO> GetAlltblPipesStripCommonThicknessForDropDown();
        List<TblStripsGradeDropDownTo> GetAlltblStripsGradeForDropDown();
        List<TblPipesStripCommonQuantityTO> GetAlltblPipesStripCommonQuantityForDropDown();
        List<TblInchDropDownTO> GetTblInchForDropDown();
        List<TblSizeTO> GetTblSizeForDropDown(Int32 idInch = 0);
        List<TblThicknessDropDownTO> GetTblThicknessForDropDown();
        List<TblWidthDropDownTO> GetTblWidthForDropDown();
    }
}
