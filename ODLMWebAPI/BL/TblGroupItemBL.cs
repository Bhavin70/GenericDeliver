using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TblGroupItemBL : ITblGroupItemBL
    {
        private readonly ITblGroupItemDAO _iTblGroupItemDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblGroupItemBL(ICommon iCommon, ITblGroupItemDAO iTblGroupItemDAO, IConnectionString iConnectionString)
        {
            _iTblGroupItemDAO = iTblGroupItemDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblGroupItemTO> SelectAllTblGroupItemList()
        {
            return _iTblGroupItemDAO.SelectAllTblGroupItem();
           
        }

        public TblGroupItemTO SelectTblGroupItemTO(Int32 idGroupItem)
        {
            return _iTblGroupItemDAO.SelectTblGroupItem(idGroupItem);
        }

        public TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId)
        {
            return _iTblGroupItemDAO.SelectTblGroupItemDetails(prodItemId);
        }


        public TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGroupItemDAO.SelectTblGroupItemDetails(prodItemId, conn, tran);
        }

        public List<TblGroupItemTO> SelectAllTblGroupItemDtlsList(Int32 groupId = 0)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblGroupItemDAO.SelectAllTblGroupItemDtlsList(groupId, conn, tran);
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


      
        #endregion

        #region Insertion
        public int InsertTblGroupItem(TblGroupItemTO tblGroupItemTO)
        {
            return _iTblGroupItemDAO.InsertTblGroupItem(tblGroupItemTO);
        }

        public int InsertTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGroupItemDAO.InsertTblGroupItem(tblGroupItemTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO)
        {
            return _iTblGroupItemDAO.UpdateTblGroupItem(tblGroupItemTO);
        }

        public int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGroupItemDAO.UpdateTblGroupItem(tblGroupItemTO, conn, tran);
        }

        public ResultMessage UpdateProductGroupITem(List<TblGroupItemTO> tblGroupItemTOList, int loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                DateTime serverDate = _iCommon.ServerDateTime;
                for (int i = 0; i < tblGroupItemTOList.Count; i++)
                {

                    TblGroupItemTO tblGroupItemTO = tblGroupItemTOList[i];
                    TblGroupItemTO existingtblGroupItemTO = SelectTblGroupItemDetails(tblGroupItemTO.ProdItemId, conn, tran);
                    if (existingtblGroupItemTO != null)
                    {
                        //Update and Deactivate the Linkage
                        existingtblGroupItemTO.IsActive = tblGroupItemTO.IsActive;
                        existingtblGroupItemTO.UpdatedBy = loginUserId;
                        existingtblGroupItemTO.UpdatedOn = serverDate;
                        result = UpdateTblGroupItem(existingtblGroupItemTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While UpdateProductGroupITem");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        tblGroupItemTO.CreatedBy = loginUserId;
                        tblGroupItemTO.CreatedOn = serverDate;
                        tblGroupItemTO.IsActive = tblGroupItemTO.IsActive;
                        result = InsertTblGroupItem(tblGroupItemTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblProdGstCodeDtls");
                            return resultMessage;
                        }
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProductGstCode");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Deletion
        public int DeleteTblGroupItem(Int32 idGroupItem)
        {
            return _iTblGroupItemDAO.DeleteTblGroupItem(idGroupItem);
        }

        public int DeleteTblGroupItem(Int32 idGroupItem, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGroupItemDAO.DeleteTblGroupItem(idGroupItem, conn, tran);
        }

        #endregion
        
    }
}
