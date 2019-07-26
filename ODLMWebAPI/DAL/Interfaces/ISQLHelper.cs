using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface ISQLHelper
    {
        IConfiguration Configuration { get; }
          int ExecuteNonQuery(CommandType cmdType, string cmdText,string connectionString, params SqlParameter[] commandParmeter);
          int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters);
          object ExecuteScalar(CommandType cmdType, string cmdText,string connectionString, params SqlParameter[] commandParameters);
          SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, string connectionString,CommandBehavior behavior, params SqlParameter[] commandParameters);
          List<T> ExecuteReader<T>(CommandType cmdType, string cmdText, string connectionString,params SqlParameter[] commandParameters);
  DataTable ExecuteReaderDataTable<T>(CommandType cmdType, string cmdText,string connectionString, params SqlParameter[] commandParameters);

    }
}