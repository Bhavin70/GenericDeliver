using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblModuleDAO
    {
        String SqlSelectQuery();
        List<DropDownTO> SelectAllTblModule();
        List<TblModuleTO> SelectTblModuleList();
        TblModuleTO SelectTblModule(Int32 idModule);
        TblModuleTO SelectTblModule(Int32 idModule, SqlConnection conn, SqlTransaction transaction);
        List<TblModuleTO> ConvertDTToList(SqlDataReader tblModuleTODT);
        int InsertTblModule(TblModuleTO tblModuleTO);
        int InsertTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblModuleTO tblModuleTO, SqlCommand cmdInsert);
        int UpdateTblModule(TblModuleTO tblModuleTO);
        int UpdateTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblModuleTO tblModuleTO, SqlCommand cmdUpdate);
        int DeleteTblModule(Int32 idModule);
        int DeleteTblModule(Int32 idModule, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idModule, SqlCommand cmdDelete);
        int GetUserSubscriptionSetting();
          List<TblModuleTO> SelectAllActiveUserCount();
 int InserttblModuleCommunicationHistory(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran);
   int CheckIsImpPersonFromLoginId(TblModuleCommHisTO tblModuleCommHis,SqlConnection conn,SqlTransaction transaction);
 int GetPreviousLoginId(TblModuleCommHisTO tblModuleCommHis,SqlConnection conn,SqlTransaction transaction);
   int UpdateAlltblModuleCommunicationHistory(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran);
      int UpdatetblModuleCommunicationHistory(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran);
        int UpdatetblLogin(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran);
         int UpdateAlltblLogin(TblModuleCommHisTO tblModuleCommHisTO, SqlConnection conn, SqlTransaction tran);
           List<TblModuleCommHisTO> GetAllTblModuleCommHis(int userId);
             
                TblModuleTO GetAllActiveAllowedCnt(int moduleId,int userId,int loginId);
                 List<TblModuleCommHisTO> GetActiveUserDetail(int moduleId);
    }
}