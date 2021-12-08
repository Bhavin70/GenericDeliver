using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ICommon
    {
        System.DateTime SelectServerDateTime();
        DateTime ServerDateTime { get; }


        String getNotificationTimeHint(DateTime raisedDate);
        List<DynamicReportTO> SqlDataReaderToExpando(SqlDataReader reader);
        IEnumerable<dynamic> GetDynamicSqlData(string connectionstring, string sql, params SqlParameter[] commandParmeter);
        void CalculateBookingsOpeningBalance(String RequestOriginString);
        void PostCancelNotConfirmLoadings(String RequestOriginString);
        void serialPortListner(String RequestOriginString);


        void PostSnoozeAndroid(String RequestOriginString);
        void PostAutoResetAndDeleteAlerts(String RequestOriginString);
        int CheckLogOutEntry(int loginId);
        int IsUserDeactivate(int userId);
        string SelectApKLoginArray(int userId);

        //Aniket [30-7-2019] added for IOT
      
        int GetNextAvailableModRefIdNew();
        int GetAvailNumber(List<int> list, int maxNumber);
        dynamic PostSalesInvoiceToSAP(TblInvoiceTO tblInvoiceTO);
        byte[] convertQRStringToByteArray(String signedQRCode);
        List<DropDownTO> GetConsumerCategoryList(String idConsumerStr = "");

    }
}
