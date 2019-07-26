using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{  
    public class TblAlertSubscribersBL : ITblAlertSubscribersBL
    {
        private readonly ITblAlertSubscribersDAO _iTblAlertSubscribersDAO;
        private readonly ITblAlertSubscriptSettingsDAO _iTblAlertSubscriptSettingsDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;

        public TblAlertSubscribersBL(ICommon iCommon, IConnectionString iConnectionString, ITblAlertSubscribersDAO iTblAlertSubscribersDAO, ITblAlertSubscriptSettingsDAO iTblAlertSubscriptSettingsDAO)
        {
            _iTblAlertSubscribersDAO = iTblAlertSubscribersDAO;
            _iTblAlertSubscriptSettingsDAO = iTblAlertSubscriptSettingsDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection
        public List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList()
        {
            return  _iTblAlertSubscribersDAO.SelectAllTblAlertSubscribers();
        }

        public TblAlertSubscribersTO SelectTblAlertSubscribersTO(Int32 idSubscription)
        {
            return _iTblAlertSubscribersDAO.SelectTblAlertSubscribers(idSubscription);
        }

        //Priyanka [20-09-2018] 
        public List<TblAlertSubscribersTO> SelectTblAlertSubscribersByAlertDefId(Int32 alertDefId)
        {
            List< TblAlertSubscribersTO> list =  _iTblAlertSubscribersDAO.SelectTblAlertSubscribersByAlertDefId(alertDefId);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    List<TblAlertSubscriptSettingsTO> AlertSubscriptSettingsTOListWithNotify = _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(list[i].IdSubscription);

                    AlertSubscriptSettingsTOListWithNotify.ForEach(f => f.SubscriptionId = list[i].IdSubscription);

                    list[i].AlertSubscriptSettingsTOList = AlertSubscriptSettingsTOListWithNotify;
                }
            }

            //if (list == null)
            //{
            //    list = new List<TblAlertSubscribersTO>();
            //}

            TblAlertSubscribersTO defaultTblAlertSubscribersTO = new TblAlertSubscribersTO();
             List<TblAlertSubscriptSettingsTO> temp = _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettingsByAlertDefId(alertDefId);
            temp.ForEach(f => f.AlertDefId = alertDefId);
            defaultTblAlertSubscribersTO.AlertSubscriptSettingsTOList = temp;

            //list.Add(defaultTblAlertSubscribersTO);

            List<TblAlertSubscribersTO> mainReturnlist = new List<TblAlertSubscribersTO>();
            mainReturnlist.Add(defaultTblAlertSubscribersTO);

            if (list != null)
            {
                mainReturnlist.AddRange(list);
            }

            return mainReturnlist;
        }

        public List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList(Int32 alertDefId,SqlConnection conn,SqlTransaction tran)
        {
            List<TblAlertSubscribersTO> list= _iTblAlertSubscribersDAO.SelectAllTblAlertSubscribers(alertDefId,conn,tran);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].AlertSubscriptSettingsTOList = _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(list[i].IdSubscription, conn, tran);
                }
            }

            return list;
        }

        #endregion

        #region Insertion
        public int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            return _iTblAlertSubscribersDAO.InsertTblAlertSubscribers(tblAlertSubscribersTO);
        }

        public int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscribersDAO.InsertTblAlertSubscribers(tblAlertSubscribersTO, conn, tran);
        }

        public ResultMessage UpdateAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO )
        {
            ResultMessage resultMessage = new ResultMessage();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblAlertSubscriptSettingsTO> tblAlertSubscriptSettingsTOList = _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(tblAlertSubscribersTO.IdSubscription, conn, tran);
                if (tblAlertSubscriptSettingsTOList != null && tblAlertSubscriptSettingsTOList.Count > 0)
                {
                    for (int i = 0; i < tblAlertSubscriptSettingsTOList.Count; i++)
                    {
                        TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO = tblAlertSubscriptSettingsTOList[i];
                        tblAlertSubscriptSettingsTO.IsActive = 0;
                        tblAlertSubscriptSettingsTO.UpdatedOn = _iCommon.ServerDateTime;
                        tblAlertSubscriptSettingsTO.UpdatedBy = tblAlertSubscribersTO.UpdatedBy;

                        int result1 = _iTblAlertSubscriptSettingsDAO.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
                        if (result1 != 1)
                        {
                            resultMessage.DefaultBehaviour("Error... Record could not be saved");
                            return resultMessage;
                        }
                    }
                }

                int result = UpdateTblAlertSubscribers(tblAlertSubscribersTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateVehicleDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }


        #endregion

        #region Updation
        public int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            return _iTblAlertSubscribersDAO.UpdateTblAlertSubscribers(tblAlertSubscribersTO);
        }

        public int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscribersDAO.UpdateTblAlertSubscribers(tblAlertSubscribersTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAlertSubscribers(Int32 idSubscription)
        {
            return _iTblAlertSubscribersDAO.DeleteTblAlertSubscribers(idSubscription);
        }

        public int DeleteTblAlertSubscribers(Int32 idSubscription, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscribersDAO.DeleteTblAlertSubscribers(idSubscription, conn, tran);
        }

        #endregion
        
    }
}
