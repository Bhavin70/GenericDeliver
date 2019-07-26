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
    public class TblUserBrandBL : ITblUserBrandBL
    {
        private readonly ITblUserBrandDAO _iTblUserBrandDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblUserBrandBL(ICommon iCommon, IConnectionString iConnectionString, ITblUserBrandDAO iTblUserBrandDAO)
        {
            _iTblUserBrandDAO = iTblUserBrandDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection
        public List<TblUserBrandTO> SelectAllTblUserBrand()
        {
            return _iTblUserBrandDAO.SelectAllTblUserBrand();
        }

        public List<TblUserBrandTO> SelectAllTblUserBrand(int isActive)
        {
            return _iTblUserBrandDAO.SelectAllTblUserBrand(isActive);
        }

        public List<TblUserBrandTO> SelectAllTblUserBrandByCnfId(Int32 cnfId)
        {
            return _iTblUserBrandDAO.SelectAllTblUserBrandByCnfId(cnfId);
        }

        public TblUserBrandTO SelectTblUserBrandTO(Int32 idUserBrand)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblUserBrandDAO.SelectTblUserBrand(idUserBrand);
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

        public ResultMessage SaveUserWithBrand(TblUserBrandTO tblUserBrandTO, Int32 userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                tblUserBrandTO.CreatedBy = userId;
                tblUserBrandTO.CreatedOn = serverDateTime;
                int result = _iTblUserBrandDAO.InsertTblUserBrand(tblUserBrandTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While Insert TblUserBrand for Users in Method SaveUserWithBrand ";
                    return rMessage;
                }

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                rMessage.Text = "Error While InsertTblUserBrand in Method SaveUserWithBrand ";
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Insertion
        public int InsertTblUserBrand(TblUserBrandTO tblUserBrandTO)
        {
            return _iTblUserBrandDAO.InsertTblUserBrand(tblUserBrandTO);
        }

        public int InsertTblUserBrand(TblUserBrandTO tblUserBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserBrandDAO.InsertTblUserBrand(tblUserBrandTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblUserBrand(TblUserBrandTO tblUserBrandTO)
        {
            return _iTblUserBrandDAO.UpdateTblUserBrand(tblUserBrandTO);
        }

        public int UpdateTblUserBrand(TblUserBrandTO tblUserBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserBrandDAO.UpdateTblUserBrand(tblUserBrandTO, conn, tran);
        }

        public ResultMessage UpdateTblUserBrandDetails(TblUserBrandTO tblUserBrandTO, Int32 userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                tblUserBrandTO.UpdatedBy = userId;
                tblUserBrandTO.UpdatedOn = serverDateTime;
                tblUserBrandTO.IsActive = 0;
                int result = _iTblUserBrandDAO.UpdateTblUserBrand(tblUserBrandTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While Update TblUserBrand for Users in Method UpdateTblUserBrandDetails ";
                    return rMessage;
                }

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                rMessage.Text = "Error While InsertTblUserBrand in Method SaveUserWithBrand ";
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Deletion
        public int DeleteTblUserBrand(Int32 idUserBrand)
        {
            return _iTblUserBrandDAO.DeleteTblUserBrand(idUserBrand);
        }

        public int DeleteTblUserBrand(Int32 idUserBrand, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserBrandDAO.DeleteTblUserBrand(idUserBrand, conn, tran);
        }

        #endregion
        
    }
}
