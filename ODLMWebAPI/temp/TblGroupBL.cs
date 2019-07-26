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
namespace ODLMWebAPI.BL
{
    public class TblGroupBL : ITblGroupBL
    {
        #region Selection

        public List<TblGroupTO> SelectAllTblGroupList()
        {
            return TblGroupDAO.SelectAllTblGroup();
        }

        public TblGroupTO SelectTblGroupTO(Int32 idGroup)
        {
            return TblGroupDAO.SelectTblGroup(idGroup);
        }

        public List<TblGroupTO> SelectAllGroupList(TblGroupTO tblGroupTO)
        {
            return TblGroupDAO.SelectAllGroupList(tblGroupTO);
        }

        public List<TblGroupTO> SelectAllActiveGroupList()
        {
            return TblGroupDAO.SelectAllActiveGroupList();
        }

        public List<TblGroupTO> SelectTblGroupTOWithRate()
        {
            List<TblGroupTO> tblGroupTOList = SelectAllActiveGroupList();

            Dictionary<Int32, Int32> groupRateDCT = BL.TblGlobalRateBL.SelectLatestGroupAndRateDCT();

            for (int i = 0; i < tblGroupTOList.Count; i++)
            {
                if (groupRateDCT != null)
                {
                    if (groupRateDCT.ContainsKey(tblGroupTOList[i].IdGroup))
                    {
                        Int32 rateID = groupRateDCT[tblGroupTOList[i].IdGroup];
                        TblGlobalRateTO rateTO = BL.TblGlobalRateBL.SelectTblGlobalRateTO(rateID);
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
            return TblGroupDAO.InsertTblGroup(tblGroupTO);
        }

        public int InsertTblGroup(TblGroupTO tblGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGroupDAO.InsertTblGroup(tblGroupTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public ResultMessage UpdateTblGroup(TblGroupTO tblGroupTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                #region to  update group 
                result = TblGroupDAO.UpdateTblGroup(tblGroupTO, conn, tran);
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
                    List<TblGroupItemTO> tblGroupItemTOList = BL.TblGroupItemBL.SelectAllTblGroupItemDtlsList(tblGroupTO.IdGroup);
                    if (tblGroupItemTOList != null && tblGroupItemTOList.Count > 0)
                    {
                        DateTime serverDate = Constants.ServerDateTime;
                        for (int i = 0; i < tblGroupItemTOList.Count; i++)
                        {

                            TblGroupItemTO tblGroupItemTO = tblGroupItemTOList[i];
                            tblGroupItemTO.IsActive = tblGroupTO.IsActive;
                            tblGroupItemTO.UpdatedBy = tblGroupTO.UpdatedBy;
                            tblGroupItemTO.UpdatedOn = tblGroupTO.UpdatedOn;
                            result = BL.TblGroupItemBL.UpdateTblGroupItem(tblGroupItemTO, conn, tran);
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
            return TblGroupDAO.UpdateTblGroup(tblGroupTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblGroup(Int32 idGroup)
        {
            return TblGroupDAO.DeleteTblGroup(idGroup);
        }

        public int DeleteTblGroup(Int32 idGroup, SqlConnection conn, SqlTransaction tran)
        {
            return TblGroupDAO.DeleteTblGroup(idGroup, conn, tran);
        }

        #endregion
        
    }
}
