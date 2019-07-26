using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimTalukaDAO
    {
        int InsertDimTaluka(StateMasterTO dimTalukaTO);
        int ExecuteInsertCommand(StateMasterTO dimTalukaTO, SqlCommand cmdInsert);
        int UpdateDimTaluka(StateMasterTO dimDistrictTO);
        int ExecuteUpdateCommand(StateMasterTO dimTal, SqlCommand cmdUpdate);

    }
}