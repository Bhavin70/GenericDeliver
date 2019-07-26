using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimDistrictDAO
    {
        int InsertDimDistrict(StateMasterTO dimDistrictTO);
        int ExecuteInsertCommand(StateMasterTO dimDistrictTO, SqlCommand cmdInsert);
        int UpdateDimDistrict(StateMasterTO dimDistrictTO);
        int ExecuteUpdateCommand(StateMasterTO dimDistrictTO, SqlCommand cmdUpdate);

    }
}