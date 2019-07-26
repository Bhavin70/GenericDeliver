using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.BL {
    public class TblModuleBL : ITblModuleBL {
        private readonly ITblModuleDAO _iTblModuleDAO;
        private readonly ICommon _iCommon;
           private readonly IConnectionString _iConnectionString;
        public TblModuleBL (IConnectionString iConnectionString,ICommon _iCommon,ITblModuleDAO iTblModuleDAO) {
            _iTblModuleDAO = iTblModuleDAO;
           this._iCommon=_iCommon;
this._iConnectionString=iConnectionString;
        }
        #region Selection
        public TblModuleTO SelectTblModuleTO (Int32 idModule) {
            return _iTblModuleDAO.SelectTblModule (idModule);
        }
        public TblModuleTO SelectTblModuleTO (Int32 idModule, SqlConnection conn, SqlTransaction tran) {
            return _iTblModuleDAO.SelectTblModule (idModule, conn, tran);
        }
        public List<DropDownTO> SelectAllTblModuleList () {
            return _iTblModuleDAO.SelectAllTblModule ();
        }

        /// <summary>
        /// Sanjay [25-Feb-2019] Method is Old only comment is added on 25 feb.
        /// It will return only active module list
        /// </summary>
        /// <returns></returns>
        public List<TblModuleTO> SelectTblModuleList () {
            List<TblModuleTO> list = _iTblModuleDAO.SelectTblModuleList ();
            List<TblModuleTO> ActiveLicenseCnt = _iTblModuleDAO.SelectAllActiveUserCount ();
            if (ActiveLicenseCnt != null) {
                for (int i = 0; i < list.Count; i++) {
                    for (int j = 0; j < ActiveLicenseCnt.Count; j++) {
                        if (list[i].IdModule == ActiveLicenseCnt[j].IdModule) {
                            list[i].NoOfActiveLicenseCnt = ActiveLicenseCnt[j].NoOfActiveLicenseCnt;
                        }
                    }

                }
            }
            return list;

        }

        #endregion

        #region Insertion
        public int InsertTblModule (TblModuleTO tblModuleTO) {
            return _iTblModuleDAO.InsertTblModule (tblModuleTO);
        }

        public int InsertTblModule (TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran) {
            return _iTblModuleDAO.InsertTblModule (tblModuleTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblModule (TblModuleTO tblModuleTO) {
            return _iTblModuleDAO.UpdateTblModule (tblModuleTO);
        }

        public int UpdateTblModule (TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran) {
            return _iTblModuleDAO.UpdateTblModule (tblModuleTO, conn, tran);
        }

        #endregion
 // Added By vipul [14/03/2019]
#region UserTracking
public  int InsertTblModuleCommHis(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran)
        {
            if(conn==null)
            {
                
                conn=new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            }
            if(tblModuleCommhisTO.InTime==DateTime.MinValue)
            {
               tblModuleCommhisTO.InTime=_iCommon.ServerDateTime;
               
            }
           
            return _iTblModuleDAO.InserttblModuleCommunicationHistory(tblModuleCommhisTO,conn,tran);
        }
        public  int UpdateTblModuleCommHis(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran)
        {
            if(conn==null)
            {
                
                conn=new SqlConnection( _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            }
             if(tblModuleCommhisTO.OutTime==DateTime.MinValue)
            {
               tblModuleCommhisTO.OutTime=_iCommon.ServerDateTime;
             }
             //  tblModuleCommhisTO.OutTime=DateTime.MinValue;
                
          
            return _iTblModuleDAO.UpdatetblModuleCommunicationHistory(tblModuleCommhisTO,conn,tran);
        }

          public  int UpdateTblModuleCommHisBeforeLogin(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran)
        {
            if(conn==null)
            {
                conn=new SqlConnection( _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            }
          
             tran = null; 
                conn.Open();
                tran = conn.BeginTransaction ();  
            if(tblModuleCommhisTO.OutTime==DateTime.MinValue)
            {
            tblModuleCommhisTO.OutTime=_iCommon.ServerDateTime;
            }
           int result =_iTblModuleDAO.UpdatetblModuleCommunicationHistory(tblModuleCommhisTO,conn,tran);
               if(result<0) 
               {
                   tran.Rollback();
                   return 0;
               }
          result=_iTblModuleDAO.UpdatetblLogin(tblModuleCommhisTO,conn,tran);
          if(result!=1) 
               {
                   tran.Rollback();
                   return 0;
               }
               tran.Commit();
            return 1;
        }
           public  int UpdateTblModuleCommHisBeforeLoginForAPK(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran)
        {
                 
              
            if(tblModuleCommhisTO.OutTime==DateTime.MinValue)
            {
            tblModuleCommhisTO.OutTime=_iCommon.ServerDateTime;
            }
           int result =_iTblModuleDAO.UpdatetblModuleCommunicationHistory(tblModuleCommhisTO,conn,tran);
               if(result<0) 
               {
                   tran.Rollback();
                   return 0;
               }
          result=_iTblModuleDAO.UpdatetblLogin(tblModuleCommhisTO,conn,tran);
          if(result!=1) 
               {
                   tran.Rollback();
                   return 0;
               }
              
            return 1;
        }
          public  int FindLatestLoginIdForLogout(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran)
          {
            int isImpLogin=1;
            tblModuleCommhisTO.UserLogin=tblModuleCommhisTO.LoginId.ToString();
            while(isImpLogin==1)
            {
            
             tblModuleCommhisTO.LoginId=_iTblModuleDAO.GetPreviousLoginId(tblModuleCommhisTO,conn,tran);
    isImpLogin= _iTblModuleDAO.CheckIsImpPersonFromLoginId(tblModuleCommhisTO,conn,tran);
     // if(!String.IsNullOrEmpty(tblModuleCommhisTO.UserLogin)) 
      if(isImpLogin==1)
              {
                    tblModuleCommhisTO.UserLogin+=",";
                    tblModuleCommhisTO.UserLogin+= tblModuleCommhisTO.LoginId.ToString();
              }
               // tblModuleCommhisTO.UserLogin+= tblModuleCommhisTO.LoginId.ToString();
               
            }
          
           if(isImpLogin==0)
           {
                
                  int result=  UpdateTblModuleCommHisBeforeLogin(tblModuleCommhisTO,conn,tran);
                  return result;
           }
 return 0;
          }
         public  int UpdateAllTblModuleCommHis(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran)
        {
            try{

            
            if(conn==null)
            {
                conn=new SqlConnection( _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            }
            tran=null;
            conn.Open();
              tran = conn.BeginTransaction();  
                tblModuleCommhisTO.OutTime=_iCommon.ServerDateTime;
            int  result =_iTblModuleDAO.UpdateAlltblModuleCommunicationHistory(tblModuleCommhisTO,conn,tran);
                
             if(result<0)
            {
                tran.Rollback();
                return result;
            }
                result=_iTblModuleDAO.UpdateAlltblLogin(tblModuleCommhisTO,conn,tran);
         if(result==0)
            {
                tran.Rollback();
                return result;
            }
            tran.Commit();
            return 1;
            }
            catch(Exception ex)
            {
                    return 0;
            }
            finally {
                conn.Close();
            }
            return 0;
        }
          public  List<TblModuleCommHisTO> GetAlltblModuleCommHis(int userId)
        {
                  
            return _iTblModuleDAO.GetAllTblModuleCommHis(userId);
        }
           public  TblModuleTO GetAllActiveAllowedCnt(int moduleId,int userId,int loginId)
        {
                  
            return _iTblModuleDAO.GetAllActiveAllowedCnt(moduleId,userId,loginId);
        }

          public  List<TblModuleCommHisTO> GetActiveCntDetails(int moduleId)
        {
                  
            return _iTblModuleDAO.GetActiveUserDetail(moduleId);
        }
          public  int UpdateInsertTblModuleCommHis(TblModuleCommHisTO tblModuleCommhisTO,SqlConnection conn,SqlTransaction tran)
        {
            try{

            
            if(conn==null)
            {
                conn=new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            }
               tran = null; 
                conn.Open ();
                tran = conn.BeginTransaction ();  
                tblModuleCommhisTO.OutTime=_iCommon.ServerDateTime;
          
            int result= _iTblModuleDAO.UpdatetblModuleCommunicationHistory(tblModuleCommhisTO,conn,tran);
            if(result<0)
            {
                tran.Rollback();
                return result;
            }
            if(tblModuleCommhisTO.InTime==DateTime.MinValue)
            {
               tblModuleCommhisTO.InTime=_iCommon.ServerDateTime;
               tblModuleCommhisTO.OutTime=DateTime.MinValue;
            }
           result=_iTblModuleDAO.InserttblModuleCommunicationHistory(tblModuleCommhisTO,conn,tran);
           if(result!=1)
           {
              tran.Rollback();
                return result; 
           }
           tran.Commit();
           return result;
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }
                return 0;
        }
        
#endregion
        #region Deletion
        public int DeleteTblModule (Int32 idModule) {
            return _iTblModuleDAO.DeleteTblModule (idModule);
        }

        public int DeleteTblModule (Int32 idModule, SqlConnection conn, SqlTransaction tran) {
            return _iTblModuleDAO.DeleteTblModule (idModule, conn, tran);
        }

        #endregion

    }
}