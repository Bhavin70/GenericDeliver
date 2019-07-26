using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimMstDesignationDAO
    {
        String SqlSelectQuery();
        List<DimMstDesignationTO> SelectAllDimMstDesignation();
        DimMstDesignationTO SelectDimMstDesignation(Int32 idDesignation);
        List<DropDownTO> SelectAllDesignationForDropDownList();
        List<DimMstDesignationTO> ConvertDTToList(SqlDataReader dimMstDesignationTODT);
        int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO);
        int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimMstDesignationTO dimMstDesignationTO, SqlCommand cmdInsert);
        int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO);
        int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimMstDesignationTO dimMstDesignationTO, SqlCommand cmdUpdate);
        int DeleteDimMstDesignation(Int32 idDesignation);
        int DeleteDimMstDesignation(Int32 idDesignation, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idDesignation, SqlCommand cmdDelete);

    }
}