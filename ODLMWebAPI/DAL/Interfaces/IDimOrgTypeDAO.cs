using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimOrgTypeDAO
    {
             String SqlSelectQuery();
          List<DimOrgTypeTO> SelectAllDimOrgType();
          DimOrgTypeTO SelectDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran);
          DataTable SelectAllDimOrgType(SqlConnection conn, SqlTransaction tran);
          List<DimOrgTypeTO> ConvertDTToList(SqlDataReader dimOrgTypeTODT);
          int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO);
          int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteInsertionCommand(DimOrgTypeTO dimOrgTypeTO, SqlCommand cmdInsert);
          int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO);
          int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteUpdationCommand(DimOrgTypeTO dimOrgTypeTO, SqlCommand cmdUpdate);
          int DeleteDimOrgType(Int32 idOrgType);
          int DeleteDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran);
          int ExecuteDeletionCommand(Int32 idOrgType, SqlCommand cmdDelete);

    }
}