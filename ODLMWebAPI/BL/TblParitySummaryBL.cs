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
using ODLMWebAPI.Controllers;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{  
    public class TblParitySummaryBL : ITblParitySummaryBL
    {
        private readonly ITblParitySummaryDAO _iTblParitySummaryDAO;
        private readonly ITblParityDetailsDAO _iTblParityDetailsDAO;
        private readonly IDimProdSpecDAO _iDimProdSpecDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblParitySummaryBL(ICommon iCommon, IConnectionString iConnectionString, ITblConfigParamsDAO iTblConfigParamsDAO, ITblParitySummaryDAO iTblParitySummaryDAO, ITblParityDetailsDAO iTblParityDetailsDAO, IDimProdSpecDAO iDimProdSpecDAO)
        {
            _iTblParitySummaryDAO = iTblParitySummaryDAO;
            _iTblParityDetailsDAO = iTblParityDetailsDAO;
            _iDimProdSpecDAO = iDimProdSpecDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblParitySummaryTO> SelectAllTblParitySummaryList()
        {
            return  _iTblParitySummaryDAO.SelectAllTblParitySummary();
        }

        public TblParitySummaryTO SelectTblParitySummaryTO(Int32 idParity,SqlConnection conn,SqlTransaction tran)
        {
           return  _iTblParitySummaryDAO.SelectTblParitySummary(idParity,conn,tran);
        }

        public TblParitySummaryTO SelectParitySummaryTOFromParityDtlId(Int32 parityDtlId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParitySummaryDAO.SelectParitySummaryFromParityDtlId(parityDtlId, conn, tran);
        }

        public TblParitySummaryTO SelectStatesActiveParitySummaryTO(Int32 stateId,Int32 brandId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblParitySummaryDAO.SelectStatesActiveParitySummary(stateId,brandId, conn, tran);
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

        public TblParitySummaryTO SelectStatesActiveParitySummaryTO(Int32 stateId,Int32 brandId ,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblParitySummaryDAO.SelectStatesActiveParitySummary(stateId,brandId,conn,tran);
        }

        /// <summary>
        /// Sanjay [2017-04-21] To Get Active Parity Id
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblParitySummaryTO> SelectActiveParitySummaryTOList(int dealerId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParitySummaryDAO.SelectActiveParitySummaryTOList(dealerId, conn, tran);
        }


        #endregion

        #region Insertion

        public ResultMessage SaveParitySettings(TblParitySummaryTO tblParitySummaryTO)
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

                #region 1. Deactivate All Prev Parity Summary Details
                result = _iTblParitySummaryDAO.DeactivateAllParitySummary(tblParitySummaryTO.StateId, tblParitySummaryTO.BrandId, conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While Deactivating Prev Parity Summary Details";
                    return resultMessage;
                }
                #endregion

                #region 2. Save New Parity Summary
                result = InsertTblParitySummary(tblParitySummaryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While Inserting New Parity Summary Details";
                    return resultMessage;
                }
                #endregion

                #region 3. Save Parity Details
                if (tblParitySummaryTO.ParityDetailList == null || tblParitySummaryTO.ParityDetailList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "ParityDetailList Found NULL";
                    return resultMessage;
                }

                for (int i = 0; i < tblParitySummaryTO.ParityDetailList.Count; i++)
                {
                    tblParitySummaryTO.ParityDetailList[i].ParityId = tblParitySummaryTO.IdParity;
                    tblParitySummaryTO.ParityDetailList[i].CreatedBy = tblParitySummaryTO.CreatedBy;
                    tblParitySummaryTO.ParityDetailList[i].CreatedOn = tblParitySummaryTO.CreatedOn;
                    result = _iTblParityDetailsDAO.InsertTblParityDetails(tblParitySummaryTO.ParityDetailList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error While Inserting New Parity Sizewise Details";
                        return resultMessage;
                    }
                }
                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Parity Settings Updated Successfully.";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save in BL : SaveParitySettings";
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

        public int InsertTblParitySummary(TblParitySummaryTO tblParitySummaryTO)
        {
            return _iTblParitySummaryDAO.InsertTblParitySummary(tblParitySummaryTO);
        }

        public int InsertTblParitySummary(TblParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParitySummaryDAO.InsertTblParitySummary(tblParitySummaryTO, conn, tran);
        }


        /// <summary>
        /// Sanjay [2017-08-03] This is one time function to execute manually for parity migration
        /// Prev In Parity Prod Spec was not there. Included for release 1.2.1 and hence old parity data needs to be
        /// set in prod spec format to loading of old bookings having old parity attached to them.
        /// </summary>
        /// <returns></returns>
        public Int32 MigrateOldParityWithAllSpecifications()
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {

                List<TblParitySummaryTO> parityList = SelectAllTblParitySummaryList();

                var stateList = parityList.GroupBy(st => st.StateId).ToList();

                List<DimProdSpecTO> prodSpecList = _iDimProdSpecDAO.SelectAllDimProdSpec();
                DateTime serverDatetime = _iCommon.ServerDateTime;
                Int32 systemUserId = Convert.ToInt32(_iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_SYTEM_ADMIN_USER_ID).ConfigParamVal);

                prodSpecList = prodSpecList.Where(sp => sp.IdProdSpec != (int)Constants.ProductSpecE.BEND).ToList();

                conn.Open();
                tran = conn.BeginTransaction();

                for (int st = 0; st < stateList.Count; st++)
                {

                    Int32 stateID = stateList[st].Key;
                    TblParitySummaryTO latestParityTO = SelectStatesActiveParitySummaryTO(stateID,0, conn, tran);
                    List<TblParityDetailsTO> parityDtlList = _iTblParityDetailsDAO.SelectAllTblParityDetails(latestParityTO.IdParity, 0, conn, tran);

                    if (parityDtlList.Count != 108)
                        continue;

                    var stateParityList = parityList.Where(s => s.IdParity != latestParityTO.IdParity && s.StateId==stateID).ToList();

                    for (int i = 0; i < stateParityList.Count; i++)
                    {
                        if (stateParityList[i].CreatedOn.Date == serverDatetime.Date)
                            continue;

                        Int32 parityid = stateParityList[i].IdParity;
                        if (parityDtlList != null)
                        {

                            for (int p = 0; p < parityDtlList.Count; p++)
                            {
                                if (parityDtlList[p].ProdSpecId == (int)Constants.ProductSpecE.BEND)
                                    continue;

                                parityDtlList[p].ParityId = parityid;
                                parityDtlList[p].CreatedOn = serverDatetime;
                                parityDtlList[p].CreatedBy = systemUserId;
                                parityDtlList[p].Remark = "Parity Migration To Product Specifications";
                                int result = _iTblParityDetailsDAO.InsertTblParityDetails(parityDtlList[p], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    return 0;
                                }
                            }
                        }
                    }
                }

                tran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public int UpdateTblParitySummary(TblParitySummaryTO tblParitySummaryTO)
        {
            return _iTblParitySummaryDAO.UpdateTblParitySummary(tblParitySummaryTO);
        }

        public int UpdateTblParitySummary(TblParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParitySummaryDAO.UpdateTblParitySummary(tblParitySummaryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblParitySummary(Int32 idParity)
        {
            return _iTblParitySummaryDAO.DeleteTblParitySummary(idParity);
        }

        public int DeleteTblParitySummary(Int32 idParity, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParitySummaryDAO.DeleteTblParitySummary(idParity, conn, tran);
        }

        #endregion
        
    }
}
