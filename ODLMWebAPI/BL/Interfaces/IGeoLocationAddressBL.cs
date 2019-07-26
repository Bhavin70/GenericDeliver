using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IGeoLocationAddressBL
    {
        GeoLocationAddressTo convertToProperAddress(GoogleGeoCodeResponse addressDetails);
        string createAddressDetils(List<results> googleGeoCodeResponse, string item);
        string myLocationAddress(string lat, string logn);
        string myLatLngByAddress(string address);
        List<newdata> SelectAlllatlngData();
        List<newdata> ConvertDTToList(SqlDataReader tblAddressTODT);
        Int32 insertAddress(GeoLocationAddressTo addressTo, int IdtblVisitDetails);
        Int32 insertlatlong(GeoLocationAddressTo addressTo, int idaddress);
        Int32 ExecuteInsertionCommand(GeoLocationAddressTo addressTo, int IdtblVisitDetails, SqlCommand cmdInsert);
        Int32 ExecuteUpdateCommand(GeoLocationAddressTo addressTo, int idaddress, SqlCommand cmdInsert);
        List<TblAddressTO> SelectAllAddress();
    }
}
