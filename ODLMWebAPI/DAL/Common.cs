using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using System.Dynamic;
using ODLMWebAPI.Models;

namespace ODLMWebAPI.DAL
{ 
    public class Common : ICommon
    {
        private readonly IConnectionString _iConnectionString;
        public Common(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        public System.DateTime SelectServerDateTime()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                /*To get Server Date Time for Local DB*/
                String sqlQuery = "SELECT CURRENT_TIMESTAMP AS ServerDate";

                //To get Server Date Time for Azure Server DB
                // string sqlQuery = " declare @dfecha as datetime " +
                //                   " declare @d as datetimeoffset " +
                //                   " set @dfecha= sysdatetime()   " +
                //                   " set @d = convert(datetimeoffset, @dfecha) at time zone 'india standard time'" +
                //                   " select convert(datetime, @d)";

                cmdSelect = new SqlCommand(sqlQuery, conn);

                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                while (dateReader.Read())
                {
                    if (TimeZoneInfo.Local.Id != "India Standard Time")
                        return Convert.ToDateTime(dateReader[0]).ToLocalTime();
                    else return Convert.ToDateTime(dateReader[0]);
                }

                return new DateTime();
            }
            catch (Exception ex)
            {
                return new DateTime();
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DateTime ServerDateTime
        {
            get
            {
                return SelectServerDateTime();
            }
        }

        public String getNotificationTimeHint(DateTime raisedDate)
        {

            DateTime curr = ServerDateTime;

            TimeSpan ts = curr - raisedDate;
            int differenceInDays = ts.Days;
            int differenceInHours = ts.Hours;
            int differenceInMinutes = ts.Minutes; // This is in int 
            if (differenceInDays != 0)
                return differenceInDays + " Days Ago";
                if (differenceInHours != 0)
                return differenceInHours + " hours Ago";
            if (differenceInMinutes != 0)
                return differenceInMinutes + " minutes Ago";
            return "Few seconds Ago";

        }
        public List<DynamicReportTO> SqlDataReaderToExpando(SqlDataReader reader)
        {
            List<DynamicReportTO> list = new List<DynamicReportTO>();

            while (reader.Read())
            {
                DynamicReportTO dynamicReportTO = new DynamicReportTO();
                List<DropDownTO> dropDownList = new List<DropDownTO>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = reader.GetName(i);
                    dropDownTO.Tag = reader[i];
                    dropDownList.Add(dropDownTO);
                }
                dynamicReportTO.DropDownList = dropDownList;
                list.Add(dynamicReportTO);
            }
            return list;
        }

        public IEnumerable<dynamic> GetDynamicSqlData(string connectionstring, string sql, params SqlParameter[] commandParmeter)
        {
            using (var conn = new SqlConnection(connectionstring))
            {
                using (var comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    if (commandParmeter != null)
                    {
                        foreach (SqlParameter parm in commandParmeter)
                        {
                            if (parm.Value == null)
                            {
                                parm.Value = DBNull.Value;
                            }
                            comm.Parameters.Add(parm);
                        }
                    }
                    using (var reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return SqlDataReaderToExpandoV1(reader);
                        }
                    }
                    conn.Close();
                }
            }
        }

        private dynamic SqlDataReaderToExpandoV1(SqlDataReader reader)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                name = name.Replace('_', ' ');
                expandoObject.Add(name, reader[i]);
            }
            return expandoObject;
        }


        
              public void serialPortListner(String RequestOriginString)
        {
            try
            {
                String requestUrl = "Support/PostTblWeighingTO";
                String url = Startup.DeliverUrl + requestUrl;
                String result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", RequestOriginString);
                request.Method = "GET";
                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public void CalculateBookingsOpeningBalance(String RequestOriginString)
        {
            try
            {
                String requestUrl = "booking/CalculateBookingsOpeningBalance";
                String url = Startup.DeliverUrl + requestUrl;
                String result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", RequestOriginString);
                request.Method = "GET";
                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public void PostSnoozeAndroid(String RequestOriginString)
        {
            
            try
            {
                String requestUrl = "Notify/postSnoozeAndroidNotification";
                String url = Startup.DeliverUrl + requestUrl;
                String result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", RequestOriginString);
                request.Method = "GET";
                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public void PostCancelNotConfirmLoadings(String RequestOriginString)
        {
            try
            {
                String requestUrl = "LoadSlip/PostCancelNotConfirmLoadings";
                String url = Startup.DeliverUrl + requestUrl;
                String result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", RequestOriginString);
                request.Method = "GET";
                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void PostAutoResetAndDeleteAlerts(String RequestOriginString)
        {
            try
            {
                String requestUrl = "Notify/PostAutoResetAndDeleteAlerts";
                String url = Startup.DeliverUrl + requestUrl;
                String result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", RequestOriginString);
                request.Method = "GET";
                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        //user Tracking checked login entry if logout time is not null then return true
        public  int CheckLogOutEntry(int loginId)
{
    int count=0; 
      String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
    try{

            String sqlQuery = "select count(*) from tbllogin where idLogin= "+loginId+"  and logoutDate IS NOT NULL";
            SqlCommand cmd=new SqlCommand(sqlQuery,conn);
             conn.Open();
         count= Convert.ToInt32(cmd.ExecuteScalar());
         return count;

    }
    catch(Exception ex)
            {
                throw ex;
                 return count;
            }
            finally
            {
                conn.Close();
                
            }
            return count;


}  

#region  isDeactivate User
 public  int IsUserDeactivate(int userId)
{
    int active=1;
      String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
    try{

            String sqlQuery = "select isActive from tblUser where idUser= "+userId;
            SqlCommand cmd=new SqlCommand(sqlQuery,conn);
             conn.Open();
         active= Convert.ToInt32(cmd.ExecuteScalar());
         return active;

    }
    catch(Exception ex)
            {
                throw ex;
                 return active;
            }
            finally
            {
                conn.Close();
                
            }
            return active;


}  
#endregion
         #region get Login id List which register on apk and then uninstall  user Tracking
public  string SelectApKLoginArray(int userId)
        {
            string LoginIds="";
            
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select idLogin from tbllogin where logoutdate is  null and deviceId is not Null and userId="+userId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader ApkLoginReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while(ApkLoginReader.Read())
                {
                    if(!string.IsNullOrEmpty(LoginIds))
                    {
                       LoginIds+=",";

                    }
                    LoginIds+=ApkLoginReader["idLogin"];
                }
                return LoginIds;
            }
            catch (Exception ex)
            {
                return LoginIds;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        #endregion
    }
}
