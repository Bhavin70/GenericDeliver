using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblModuleCommunicationBL
    {
        List<TblModuleCommunicationTO> SelectAllTblModuleCommunicationList();
        List<TblModuleCommunicationTO> SelectAllTblModuleCommunicationListById(Int32 srcModuleId, Int32 srcTxnId);
        TblModuleCommunicationTO SelectTblModuleCommunicationTO(Int32 idModuleCommunication);
        int InsertTblModuleCommunication(List<TblModuleCommunicationTO> tblModuleCommunicationList, string loginUserId);
        int InsertTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO);
        int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblModuleCommunication(Int32 idModuleCommunication);
        int DeleteTblModuleCommunication(Int32 idModuleCommunication, SqlConnection conn, SqlTransaction tran);
    }
}