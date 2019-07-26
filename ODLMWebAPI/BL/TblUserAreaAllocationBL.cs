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
    public class TblUserAreaAllocationBL : ITblUserAreaAllocationBL
    {
        #region Selection 

        private readonly ITblUserAreaAllocationDAO _iTblUserAreaAllocationDAO;
        private readonly IConnectionString _iConnectionString;

        public TblUserAreaAllocationBL(IConnectionString iConnectionString, ITblUserAreaAllocationDAO iTblUserAreaAllocationDAO)
        {
            _iTblUserAreaAllocationDAO = iTblUserAreaAllocationDAO;
            _iConnectionString = iConnectionString;
        }

        public TblUserAreaAllocationTO SelectTblUserAreaAllocationTO(Int32 idAreaAllocDtl)
        {
            return  _iTblUserAreaAllocationDAO.SelectTblUserAreaAllocation(idAreaAllocDtl);
        }

        public List<TblUserAreaAllocationTO> SelectAllTblUserAreaAllocationList(int userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblUserAreaAllocationList(userId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblUserAreaAllocationTO> SelectAllTblUserAreaAllocationList(int userId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserAreaAllocationDAO.SelectAllTblUserAreaAllocation(userId, conn, tran);
        }

        public List<UserAreaCnfDealerDtlTO> SelectAllUserAreaCnfDealerList(int userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblUserAreaAllocationDAO.SelectAllUserAreaCnfDealerList(userId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        public List<TblUserAreaAllocationTO> SelectAllBookingUserAreaAllocationList(Int32 cnfOrgId,Int32 dealerOrgId,Int32 brandId,  SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserAreaAllocationDAO.SelectAllBookingUserAreaAllocationList(cnfOrgId, dealerOrgId, brandId, conn, tran);
        }
        #endregion

        #region Insertion

        public ResultMessage SaveUserAreaAllocation(List<TblUserAreaAllocationTO> userAreaAllocationTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (userAreaAllocationTOList == null || userAreaAllocationTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,userAreaAllocationTOList Found NULL : Method SaveUserAreaAllocation";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #region 1. Mark Balance Stock = Total Stock i.e Make this stock available to C&F View
                int userId = userAreaAllocationTOList[0].UserId;
                List<TblUserAreaAllocationTO> list = SelectAllTblUserAreaAllocationList(userId, conn, tran);
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]
                        var existingTO = userAreaAllocationTOList.Where(e => e.UserId == list[i].UserId &&
                                                                        e.CnfOrgId == list[i].CnfOrgId &&
                                                                        e.DistrictId == list[i].DistrictId &&
                                                                        e.BrandId == list[i].BrandId).FirstOrDefault();

                        if (existingTO == null)
                        {
                            // Mark this as inactive
                            list[i].IsActive = 0;
                            result = UpdateTblUserAreaAllocation(list[i], conn, tran);
                            if(result!=1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Error while UpdateTblUserAreaAllocation : Method SaveUserAreaAllocation";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }
                        else
                        {
                            if (userAreaAllocationTOList != null && userAreaAllocationTOList.Count > 0)
                                userAreaAllocationTOList.Remove(existingTO);
                        }

                    }
                }
                #endregion

                for (int i = 0; i < userAreaAllocationTOList.Count; i++)
                {
                    result = InsertTblUserAreaAllocation(userAreaAllocationTOList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error while InsertTblUserAreaAllocation : Method SaveUserAreaAllocation";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }
               

                #region 3. Notification to Concern User that New area has been updated. Now required for the time being

                //TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                //tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.TODAYS_STOCK_CONFIRMED;
                //tblAlertInstanceTO.AlertAction = "TODAYS_STOCK_CONFIRMED";
                //tblAlertInstanceTO.AlertComment = "Today's Stock is Confirmed . Total Stock(In MT) is :" + stockSummaryTO.TotalStock;
                //tblAlertInstanceTO.EffectiveFromDate = stockSummaryTO.ConfirmedOn;
                //tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                //tblAlertInstanceTO.IsActive = 1;
                //tblAlertInstanceTO.SourceDisplayId = "TODAYS_STOCK_CONFIRMED";
                //tblAlertInstanceTO.SourceEntityId = stockSummaryTO.IdStockSummary;
                //tblAlertInstanceTO.RaisedBy = stockSummaryTO.ConfirmedBy;
                //tblAlertInstanceTO.RaisedOn = stockSummaryTO.ConfirmedOn;
                //tblAlertInstanceTO.IsAutoReset = 1;

                //ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                //if (rMessage.MessageType != ResultMessageE.Information)
                //{
                //    tran.Rollback();
                //    resultMessage.MessageType = ResultMessageE.Error;
                //    resultMessage.Text = "Error While SaveNewAlertInstance";
                //    resultMessage.Tag = tblAlertInstanceTO;
                //    return resultMessage;
                //}

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveUserAreaAllocation";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public int InsertTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO)
        {
            return _iTblUserAreaAllocationDAO.InsertTblUserAreaAllocation(tblUserAreaAllocationTO);
        }

        public int InsertTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserAreaAllocationDAO.InsertTblUserAreaAllocation(tblUserAreaAllocationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO)
        {
            return _iTblUserAreaAllocationDAO.UpdateTblUserAreaAllocation(tblUserAreaAllocationTO);
        }

        public int UpdateTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserAreaAllocationDAO.UpdateTblUserAreaAllocation(tblUserAreaAllocationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblUserAreaAllocation(Int32 idAreaAllocDtl)
        {
            return _iTblUserAreaAllocationDAO.DeleteTblUserAreaAllocation(idAreaAllocDtl);
        }

        public int DeleteTblUserAreaAllocation(Int32 idAreaAllocDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserAreaAllocationDAO.DeleteTblUserAreaAllocation(idAreaAllocDtl, conn, tran);
        }

        #endregion
        
    }
}
