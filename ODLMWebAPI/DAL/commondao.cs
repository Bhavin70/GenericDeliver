using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL
{
    public class CommonDAO
    {
        //public System.DateTime SelectServerDateTime()
        //{

        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = null;
        //    try
        //    {
        //        conn.Open();
        //        /*To get Server Date Time for Local DB*/
        //        String sqlQuery = "SELECT CURRENT_TIMESTAMP AS ServerDate";

        //        //To get Server Date Time for Azure Server DB
        //        //string sqlQuery = " declare @dfecha as datetime " +
        //        //                  " declare @d as datetimeoffset " +
        //        //                  " set @dfecha= sysdatetime()   " +
        //        //                  " set @d = convert(datetimeoffset, @dfecha) at time zone 'india standard time'" +
        //        //                  " select convert(datetime, @d)";

        //        cmdSelect = new SqlCommand(sqlQuery, conn);

        //        SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

        //        while (dateReader.Read())
        //        {
        //            if (TimeZoneInfo.Local.Id != "India Standard Time")
        //                return Convert.ToDateTime(dateReader[0]).ToLocalTime();
        //            else return Convert.ToDateTime(dateReader[0]);
        //        }

        //        return new DateTime();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new DateTime();
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }

        //}

        public void SetDateStandards(Object classTO, Double timeOffsetMins)
        {

            Type type = classTO.GetType();
            PropertyInfo[] propertyInfoArray = type.GetProperties();

            for (int j = 0; j < propertyInfoArray.Length; j++)
            {
                PropertyInfo propInfo = propertyInfoArray[j];
                if (propInfo.PropertyType == typeof(DateTime))
                {
                    DateTime columnValue = Convert.ToDateTime(propInfo.GetValue(classTO, null));
                    columnValue = columnValue.AddMinutes(timeOffsetMins);
                    propInfo.SetValue(classTO, columnValue);
                }
            }

        }
    }
}
