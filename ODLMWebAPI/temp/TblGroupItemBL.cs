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
namespace ODLMWebAPI.BL
{
    public class TblGroupItemBL : ITblGroupItemBL
    {
        #region Selection

        public List<TblGroupItemTO> SelectAllTblGroupItemList()
        {
            return TblGroupItemDAO.SelectAllTblGroupItem();
           
        }

        public TblGroupItemTO SelectTblGroupItemTO(Int32 idGroupItem)
        {
            return TblGroupItemDAO.SelectTblGroupItem(idGroupItem);
        }

        public TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId)
        {
            return TblGroupItemDAO.SelectTblGroupItemDetails(prodItemId);
        }


        public TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            return TblGroupItemDAO.SelectTblGroupItemDetails(prodItemId, conn, tran);
        }

        public List<TblGroupItemTO> SelectAllTblGroupItemDtlsList(Int32 groupId = 0)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblGroupItemDAO.SelectAllTblGroupItemDtlsList(groupId, conn, tran);
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
            return TblGroupItemDAO.InsertTblGroupItem(tblGroupItemTO);
        }

        public int InsertTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGroupItemDAO.InsertTblGroupItem(tblGroupItemTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO)
        {
            return TblGroupItemDAO.UpdateTblGroupItem(tblGroupItemTO);
        }

        public int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGroupItemDAO.UpdateTblGroupItem(tblGroupItemTO, conn, tran);
        }

        internal static ResultMessage UpdateProductGroupITem(List<TblGroupItemTO> tblGroupItemTOList, int loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                DateTime serverDate = Constants.ServerDateTime;
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
            return TblGroupItemDAO.DeleteTblGroupItem(idGroupItem);
        }

        public int DeleteTblGroupItem(Int32 idGroupItem, SqlConnection conn, SqlTransaction tran)
        {
            return TblGroupItemDAO.DeleteTblGroupItem(idGroupItem, conn, tran);
        }

        #endregion
        
    }
}
