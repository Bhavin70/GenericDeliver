using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblGroupBL : ITblGroupBL
    {
        private readonly ITblGroupDAO _iTblGroupDAO;
        private readonly ITblGroupItemBL _iTblGroupItemBL;
        private readonly ITblGlobalRateBL _iTblGlobalRateBL;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblGroupBL(ICommon iCommon,  IConnectionString iConnectionString, ITblGroupDAO iTblGroupDAO, ITblGroupItemBL iTblGroupItemBL, ITblGlobalRateBL iTblGlobalRateBL)
        {
            _iTblGroupDAO = iTblGroupDAO;
            _iTblGroupItemBL = iTblGroupItemBL;
            _iTblGlobalRateBL = iTblGlobalRateBL;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblGroupTO> SelectAllTblGroupList()
        {
            return _iTblGroupDAO.SelectAllTblGroup();
        }

        public TblGroupTO SelectTblGroupTO(Int32 idGroup)
        {
            return _iTblGroupDAO.SelectTblGroup(idGroup);
        }

        public List<TblGroupTO> SelectAllGroupList(TblGroupTO tblGroupTO)
        {
            return _iTblGroupDAO.SelectAllGroupList(tblGroupTO);
        }

        public List<TblGroupTO> SelectAllActiveGroupList()
        {
            return _iTblGroupDAO.SelectAllActiveGroupList();
        }

        public List<TblGroupTO> SelectTblGroupTOWithRate()
        {
            List<TblGroupTO> tblGroupTOList = SelectAllActiveGroupList();

            Dictionary<Int32, Int32> groupRateDCT = _iTblGlobalRateBL.SelectLatestGroupAndRateDCT();

            for (int i = 0; i < tblGroupTOList.Count; i++)
            {
                if (groupRateDCT != null)
                {
                    if (groupRateDCT.ContainsKey(tblGroupTOList[i].IdGroup))
                    {
                        Int32 rateID = groupRateDCT[tblGroupTOList[i].IdGroup];
                        TblGlobalRateTO rateTO = _iTblGlobalRateBL.SelectTblGlobalRateTO(rateID);
                        if (rateTO != null)
                        {
                            tblGroupTOList[i].Rate = rateTO.Rate;
                        }
                    }
                }
            }

            return tblGroupTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblGroup(TblGroupTO tblGroupTO)
        {
            return _iTblGroupDAO.InsertTblGroup(tblGroupTO);
        }

        public int InsertTblGroup(TblGroupTO tblGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGroupDAO.InsertTblGroup(tblGroupTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public ResultMessage UpdateTblGroup(TblGroupTO tblGroupTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                #region to  update group 
                result = _iTblGroupDAO.UpdateTblGroup(tblGroupTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblGroup";
                    resultMessage.DisplayMessage = "Record Cound Not Update";
                    return resultMessage;
                }
                #endregion
                #region to update group item linking
                if(tblGroupTO.IsActive == 0)
                {
                    List<TblGroupItemTO> tblGroupItemTOList = _iTblGroupItemBL.SelectAllTblGroupItemDtlsList(tblGroupTO.IdGroup);
                    if (tblGroupItemTOList != null && tblGroupItemTOList.Count > 0)
                    {
                        DateTime serverDate = _iCommon.ServerDateTime;
                        for (int i = 0; i < tblGroupItemTOList.Count; i++)
                        {

                            TblGroupItemTO tblGroupItemTO = tblGroupItemTOList[i];
                            tblGroupItemTO.IsActive = tblGroupTO.IsActive;
                            tblGroupItemTO.UpdatedBy = tblGroupTO.UpdatedBy;
                            tblGroupItemTO.UpdatedOn = tblGroupTO.UpdatedOn;
                            result = _iTblGroupItemBL.UpdateTblGroupItem(tblGroupItemTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While UpdateTblGroupItem";
                                resultMessage.Result = -1;
                                return resultMessage;
                            }

                        }
                    }
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Success... Group Updated";
                resultMessage.DisplayMessage = "Success... Group Updated";
                return resultMessage;


            }
            catch (Exception ex)
            {

                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblGroup");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

            #endregion


        }

        public int UpdateTblGroup(TblGroupTO tblGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGroupDAO.UpdateTblGroup(tblGroupTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblGroup(Int32 idGroup)
        {
            return _iTblGroupDAO.DeleteTblGroup(idGroup);
        }

        public int DeleteTblGroup(Int32 idGroup, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGroupDAO.DeleteTblGroup(idGroup, conn, tran);
        }

        #endregion
        
    }
}
