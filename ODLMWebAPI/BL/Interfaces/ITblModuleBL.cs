using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblModuleBL
    {
        TblModuleTO SelectTblModuleTO(Int32 idModule);
        TblModuleTO SelectTblModuleTO(Int32 idModule, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectAllTblModuleList();
        List<TblModuleTO> SelectTblModuleList();
        int InsertTblModule(TblModuleTO tblModuleTO);
        int InsertTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblModule(TblModuleTO tblModuleTO);
        int UpdateTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblModule(Int32 idModule);
        int DeleteTblModule(Int32 idModule, SqlConnection conn, SqlTransaction tran);

         int InsertTblModuleCommHis(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran);
           int UpdateTblModuleCommHis(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran);
             int UpdateTblModuleCommHisBeforeLogin(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran);
               int UpdateTblModuleCommHisBeforeLoginForAPK(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran);
                 int FindLatestLoginIdForLogout(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran);
                   int UpdateAllTblModuleCommHis(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran);
                    List<TblModuleCommHisTO> GetAlltblModuleCommHis(int userId);
                  
                        TblModuleTO GetAllActiveAllowedCnt(int moduleId,int userId,int loginId);
                     List<TblModuleCommHisTO> GetActiveCntDetails(int moduleId);
                      int UpdateInsertTblModuleCommHis(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran);
    }
}