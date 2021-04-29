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
using ODLMWebAPI.IoT;
using System.Threading;
using System.Text;
using QRCoder;
using System.Drawing;
using System.IO.Compression;

namespace ODLMWebAPI.DAL
{ 
    public class Common : ICommon
    {
        private readonly IConnectionString _iConnectionString;
        private readonly IModbusRefConfig _iModbusRefConfig;

        static readonly object uniqueModBusRefIdLock = new object();

        public Common(IConnectionString iConnectionString, IModbusRefConfig iModbusRefConfig)
        {
            _iConnectionString = iConnectionString;
         _iModbusRefConfig =iModbusRefConfig;
        }

        public List<int> GeModRefMaxData()
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT TOP 255 modbusRefId FROM tempLoading WHERE modbusRefId IS NOT NULL ORDER BY modbusRefId DESC";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<int> list = new List<int>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        int modRefId = 0;
                        if (sqlReader["modbusRefId"] != DBNull.Value)
                            modRefId = Convert.ToInt32(sqlReader["modbusRefId"].ToString());
                        if (modRefId > 0)
                            list.Add(modRefId);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #region GetNextAvailableModRefIdNew
        //Aniket [30-7-2019]  added for IOT
        public int GetNextAvailableModRefIdNew()
        {

            lock (uniqueModBusRefIdLock)
            {
                int modRefNumber = 0;
                List<int> list = GeModRefMaxData();
                if (list != null && list.Count > 0)
                {
                    int maxNumber = 1;
                    modRefNumber = GetAvailNumber(list, maxNumber);
                }
                else
                {
                    modRefNumber = 1;
                }
                bool isInList = list.Contains(modRefNumber);
                if (isInList)
                    return 0;
                else
                    list.Add(modRefNumber);
                //     Random num = new Random();
                //    modRefNumber= num.Next(1, 255);
                return modRefNumber;
            }
        }


        public int GetAvailNumber(List<int> list, int maxNumber)
        {
            if (list.Contains(maxNumber))
            {
                if (maxNumber > 255)
                {
                    return 0;
                }
                maxNumber++;
                return GetAvailNumber(list, maxNumber);
            }
            else
            {
                return maxNumber;
            }
        }
        #endregion

        public System.DateTime SelectServerDateTime()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                /*To get Server Date Time for Local DB*/
                String sqlQuery = Startup.SERVER_DATETIME_QUERY_STRING;

                ////To get Server Date Time for Azure Server DB
                //string sqlQuery = " declare @dfecha as datetime " +
                //                  " declare @d as datetimeoffset " +
                //                  " set @dfecha= sysdatetime()   " +
                //                  " set @d = convert(datetimeoffset, @dfecha) at time zone 'india standard time'" +
                //                  " select convert(datetime, @d)";

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
        public dynamic PostSalesInvoiceToSAP(TblInvoiceTO tblInvoiceTO)
        {
            var tRequest = WebRequest.Create(Startup.StockUrl + "Commercial/PostSalesInvoiceToSAP") as HttpWebRequest;
            try
            {
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    data = tblInvoiceTO,
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(tblInvoiceTO);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

       

        public byte[] convertQRStringToByteArray(String signedQRCode)
        {
            try
            {
                QRCoder.QRCodeGenerator qrGen = new QRCodeGenerator();
                var qrData = qrGen.CreateQrCode(signedQRCode, QRCoder.QRCodeGenerator.ECCLevel.L);
                var qrCode = new QRCoder.QRCode(qrData);
                System.Drawing.Image image = qrCode.GetGraphic(50);

                byte[] PhotoCodeInBytes = null;

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    PhotoCodeInBytes = ms.ToArray();
                }
                //byte[] compressedByte = Compress(PhotoCodeInBytes);

                return PhotoCodeInBytes;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static Byte[] Compress(Byte[] buffer)
        {
            byte[] imageBytes;

            //Of course image bytes is set to the bytearray of your image      

            using (MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length))
            {
                using (Image img = Image.FromStream(ms))
                {
                    int h = 100;
                    int w = 100;

                    using (Bitmap b = new Bitmap(img, new Size(w, h)))
                    {
                        using (MemoryStream ms2 = new MemoryStream())
                        {
                            b.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);
                            imageBytes = ms2.ToArray();
                        }
                    }
                }
            }
            return imageBytes;
        }
    }
}
