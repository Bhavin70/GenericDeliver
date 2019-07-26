using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimMstDesignationBL
    {
        List<DimMstDesignationTO> SelectAllDimMstDesignationList();
        DimMstDesignationTO SelectDimMstDesignationTO(Int32 idDesignation);
        List<DropDownTO> SelectAllDesignationForDropDownList();
        int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO);
        int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO);
        int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimMstDesignation(Int32 idDesignation);
        int DeleteDimMstDesignation(Int32 idDesignation, SqlConnection conn, SqlTransaction tran);
    }
}
