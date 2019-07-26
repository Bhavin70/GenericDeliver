using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimSmsConfigDAO
    {
          String SqlSelectQuery();
          DimSmsConfigTO SelectAllDimSmsConfig();
          List<DimSmsConfigTO> ConvertDTToList(SqlDataReader dimSmsConfigTODT);
    }
}