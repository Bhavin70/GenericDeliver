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
    public class TblStockConfigBL : ITblStockConfigBL
    { 
        private readonly ITblStockConfigDAO _iTblStockConfigDAO;
        private readonly ITblStockDetailsBL _iTblStockDetailsBL;
        public TblStockConfigBL(ITblStockConfigDAO iTblStockConfigDAO, ITblStockDetailsBL iTblStockDetailsBL)
        {
            _iTblStockConfigDAO = iTblStockConfigDAO;
            _iTblStockDetailsBL = iTblStockDetailsBL;
        }
        #region Selection

        public List<TblStockConfigTO> SelectAllTblStockConfigTOList()
        {
            return _iTblStockConfigDAO.SelectAllTblStockConfigTOList();
            
        }

        public TblStockConfigTO SelectTblStockConfigTO(Int32 idStockConfig)
        {
            return _iTblStockConfigDAO.SelectTblStockConfigTO(idStockConfig);
            
        }

        public List<TblStockConfigTO> SelectAllTblStockConfigTOList(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockConfigDAO.SelectAllTblStockConfigTOList(conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTblStockConfig(TblStockConfigTO tblStockConfigTO)
        {
            return _iTblStockConfigDAO.InsertTblStockConfig(tblStockConfigTO);
        }

        public int InsertTblStockConfig(TblStockConfigTO tblStockConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockConfigDAO.InsertTblStockConfig(tblStockConfigTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblStockConfig(TblStockConfigTO tblStockConfigTO)
        {
            return _iTblStockConfigDAO.UpdateTblStockConfig(tblStockConfigTO);
        }

        public int UpdateTblStockConfig(TblStockConfigTO tblStockConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockConfigDAO.UpdateTblStockConfig(tblStockConfigTO, conn, tran);
        }

        public ResultMessage DeactivateTblStockConfig(TblStockConfigTO tblStockConfigTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
               
                resultMessage.MessageType = ResultMessageE.None;
                Double totalBalanceStock = 0;
                List<TblStockDetailsTO> tblStockDetailsTOList = _iTblStockDetailsBL.SelectTblStockDetailsList(tblStockConfigTO.MaterialId, tblStockConfigTO.ProdCatId, tblStockConfigTO.ProdSpecId, tblStockConfigTO.BrandId);
                if (tblStockDetailsTOList != null && tblStockDetailsTOList.Count > 0)
                {
                    totalBalanceStock = tblStockDetailsTOList.Sum(s => s.BalanceStock);
                }   
                    if (totalBalanceStock > 0)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Sorry..Record Could not be updated. Balance stock is remaining for that  item";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be updated. Balance stock is remaining for that  item";


                    }
                    
                    else
                    {
                        tblStockConfigTO.IsItemizedStock = 0;
                        int result = _iTblStockConfigDAO.DeleteTblStockConfig(tblStockConfigTO.IdStockConfig);
                        if (result != 0)
                        {
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.Result = 1;
                            resultMessage.Text = "Record deleted successfully";
                            resultMessage.DisplayMessage = "Record deleted successfully";
                        }

                    }

                
               
            }
            catch(Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveNewBooking";
                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;

            }
            return resultMessage;
        }

        #endregion

        #region Deletion
        public int DeleteTblStockConfig(Int32 idStockConfig)
        {
            return _iTblStockConfigDAO.DeleteTblStockConfig(idStockConfig);
        }

        public int DeleteTblStockConfig(Int32 idStockConfig, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockConfigDAO.DeleteTblStockConfig(idStockConfig, conn, tran);
        }

        #endregion
        
    }
}
