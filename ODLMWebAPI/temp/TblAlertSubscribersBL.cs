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
namespace ODLMWebAPI.BL
{
    public class TblAlertSubscribersBL : ITblAlertSubscribersBL
    {
        #region Selection
        public List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList()
        {
            return  TblAlertSubscribersDAO.SelectAllTblAlertSubscribers();
        }

        public TblAlertSubscribersTO SelectTblAlertSubscribersTO(Int32 idSubscription)
        {
            return TblAlertSubscribersDAO.SelectTblAlertSubscribers(idSubscription);
        }

        //Priyanka [20-09-2018] 
        public List<TblAlertSubscribersTO> SelectTblAlertSubscribersByAlertDefId(Int32 alertDefId)
        {
            List< TblAlertSubscribersTO> list =  TblAlertSubscribersDAO.SelectTblAlertSubscribersByAlertDefId(alertDefId);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    List<TblAlertSubscriptSettingsTO> AlertSubscriptSettingsTOListWithNotify = BL.TblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsList(list[i].IdSubscription);

                    AlertSubscriptSettingsTOListWithNotify.ForEach(f => f.SubscriptionId = list[i].IdSubscription);

                    list[i].AlertSubscriptSettingsTOList = AlertSubscriptSettingsTOListWithNotify;
                }
            }

            //if (list == null)
            //{
            //    list = new List<TblAlertSubscribersTO>();
            //}

            TblAlertSubscribersTO defaultTblAlertSubscribersTO = new TblAlertSubscribersTO();
             List<TblAlertSubscriptSettingsTO> temp= BL.TblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsListByAlertDefId(alertDefId);
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
            List<TblAlertSubscribersTO> list= TblAlertSubscribersDAO.SelectAllTblAlertSubscribers(alertDefId,conn,tran);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].AlertSubscriptSettingsTOList = BL.TblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsList(list[i].IdSubscription, conn, tran);
                }
            }

            return list;
        }

        #endregion

        #region Insertion
        public int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            return TblAlertSubscribersDAO.InsertTblAlertSubscribers(tblAlertSubscribersTO);
        }

        public int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscribersDAO.InsertTblAlertSubscribers(tblAlertSubscribersTO, conn, tran);
        }

        public ResultMessage UpdateAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO )
        {
            ResultMessage resultMessage = new ResultMessage();
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblAlertSubscriptSettingsTO> tblAlertSubscriptSettingsTOList = BL.TblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsList(tblAlertSubscribersTO.IdSubscription, conn, tran);
                if (tblAlertSubscriptSettingsTOList != null && tblAlertSubscriptSettingsTOList.Count > 0)
                {
                    for (int i = 0; i < tblAlertSubscriptSettingsTOList.Count; i++)
                    {
                        TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO = tblAlertSubscriptSettingsTOList[i];
                        tblAlertSubscriptSettingsTO.IsActive = 0;
                        tblAlertSubscriptSettingsTO.UpdatedOn = Constants.ServerDateTime;
                        tblAlertSubscriptSettingsTO.UpdatedBy = tblAlertSubscribersTO.UpdatedBy;

                        int result1 = BL.TblAlertSubscriptSettingsBL.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
                        if (result1 != 1)
                        {
                            resultMessage.DefaultBehaviour("Error... Record could not be saved");
                            return resultMessage;
                        }
                    }
                }

                int result = BL.TblAlertSubscribersBL.UpdateTblAlertSubscribers(tblAlertSubscribersTO);
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
            return TblAlertSubscribersDAO.UpdateTblAlertSubscribers(tblAlertSubscribersTO);
        }

        public int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscribersDAO.UpdateTblAlertSubscribers(tblAlertSubscribersTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAlertSubscribers(Int32 idSubscription)
        {
            return TblAlertSubscribersDAO.DeleteTblAlertSubscribers(idSubscription);
        }

        public int DeleteTblAlertSubscribers(Int32 idSubscription, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscribersDAO.DeleteTblAlertSubscribers(idSubscription, conn, tran);
        }

        #endregion
        
    }
}
