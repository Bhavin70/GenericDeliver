using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblModuleCommunicationDAO
    {
        String SqlSelectQuery();
        List<TblModuleCommunicationTO> SelectAllTblModuleCommunication();
        List<TblModuleCommunicationTO> SelectAllTblModuleCommunicationById(Int32 srcModuleId, Int32 srcTxnId);
        TblModuleCommunicationTO SelectTblModuleCommunication(Int32 idModuleCommunication);
        List<TblModuleCommunicationTO> SelectAllTblModuleCommunication(SqlConnection conn, SqlTransaction tran);
        int InsertTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO);
        int InsertTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblModuleCommunicationTO tblModuleCommunicationTO, SqlCommand cmdInsert);
        int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO);
        int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblModuleCommunicationTO tblModuleCommunicationTO, SqlCommand cmdUpdate);
        int DeleteTblModuleCommunication(Int32 idModuleCommunication);
        int DeleteTblModuleCommunication(Int32 idModuleCommunication, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idModuleCommunication, SqlCommand cmdDelete);
        List<TblModuleCommunicationTO> ConvertDTToList(SqlDataReader tblModuleCommunicationTODT);

    }
}