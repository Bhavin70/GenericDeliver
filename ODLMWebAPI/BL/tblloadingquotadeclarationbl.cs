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
    public class TblLoadingQuotaDeclarationBL : ITblLoadingQuotaDeclarationBL
    {
        private readonly ITblLoadingQuotaDeclarationDAO _iTblLoadingQuotaDeclarationDAO;
        private readonly IConnectionString _iConnectionString;
        public TblLoadingQuotaDeclarationBL(IConnectionString iConnectionString, ITblLoadingQuotaDeclarationDAO iTblLoadingQuotaDeclarationDAO)
        {
            _iTblLoadingQuotaDeclarationDAO = iTblLoadingQuotaDeclarationDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclarationList()
        {
            return  _iTblLoadingQuotaDeclarationDAO.SelectAllTblLoadingQuotaDeclaration();
        }

        public List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclarationList(DateTime declarationDate)
        {
            return _iTblLoadingQuotaDeclarationDAO.SelectAllTblLoadingQuotaDeclaration(declarationDate);
        }

        public Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate, Int32 prodCatId, Int32 prodSpecId)
        {
            return _iTblLoadingQuotaDeclarationDAO.IsLoadingQuotaDeclaredForTheDate(declarationDate,prodCatId,prodSpecId);
        }

        public Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblLoadingQuotaDeclarationDAO.IsLoadingQuotaDeclaredForTheDate(declarationDate, conn, tran);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingQuotaDeclarationDAO.IsLoadingQuotaDeclaredForTheDate(declarationDate,conn,tran);
        }

        public List<TblLoadingQuotaDeclarationTO> SelectAvailableLoadingQuotaForCnf(int cnfId, DateTime declarationDate)
        {
            return _iTblLoadingQuotaDeclarationDAO.SelectAllTblLoadingQuotaDeclaration(cnfId,declarationDate);
        }

        public List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 prodCatId, Int32 prodSpecId)
        {
            return _iTblLoadingQuotaDeclarationDAO.SelectLatestCalculatedLoadingQuotaDeclarationList(stockDate, prodCatId, prodSpecId);
        }

        public List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate,Int32 cnfOrgId, SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingQuotaDeclarationDAO.SelectLatestCalculatedLoadingQuotaDeclarationList(stockDate,cnfOrgId, conn, tran);
        }

        public TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclarationTO(Int32 idLoadingQuota)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectTblLoadingQuotaDeclarationTO(idLoadingQuota, conn, tran);
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

        public TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclarationTO(Int32 idLoadingQuota,SqlConnection conn,SqlTransaction tran)
        {
           return  _iTblLoadingQuotaDeclarationDAO.SelectTblLoadingQuotaDeclaration(idLoadingQuota,conn,tran);
        }

        public TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclarationTO(Int32 cnfId,Int32 prodCatId,Int32 prodSpecId,Int32 materialId,DateTime quotaDate, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaDeclarationDAO.SelectLoadingQuotaDeclarationTO(cnfId, prodCatId, prodSpecId, materialId, quotaDate, conn, tran);
        }

        public List<TblLoadingQuotaDeclarationTO> SelectLoadingQuotaListForCnfAndDate(Int32 cnfOrgId,DateTime quotaDate, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaDeclarationDAO.SelectLoadingQuotaDeclaredForCnfAndDate(cnfOrgId,quotaDate, conn, tran);
        }

        /// <summary>
        /// Sanjay [2017-04-05] To Get All Declared Loading Quota List against given Loading Slip Ext Ids
        /// These are the Ids of material against a loading slip. Required while confirming the loading slip
        /// </summary>
        /// <param name="loadingSlipExtId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblLoadingQuotaDeclarationTO> SelectAllLoadingQuotaDeclListFromLoadingExt(String loadingSlipExtId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingQuotaDeclarationDAO.SelectAllLoadingQuotaDeclListFromLoadingExt(loadingSlipExtId,conn,tran);
        }
        #endregion

        #region Insertion
        public int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO)
        {
            return _iTblLoadingQuotaDeclarationDAO.InsertTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO);
        }

        public int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaDeclarationDAO.InsertTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
        }

        public ResultMessage SaveLoadingQuotaDeclaration(List<TblLoadingQuotaDeclarationTO> loadingQuotaDeclarationTOList)
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

                if (loadingQuotaDeclarationTOList == null || loadingQuotaDeclarationTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "competitorUpdatesTOList Found Null : SaveLoadingQuotaDeclaration";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #region 1. Mark All Previous Loading Quota As Inactive

                Int32 prodCatId = loadingQuotaDeclarationTOList[0].ProdCatId;
                Int32 prodSpecId = loadingQuotaDeclarationTOList[0].ProdSpecId;
                result = _iTblLoadingQuotaDeclarationDAO.DeactivateAllPrevLoadingQuota(loadingQuotaDeclarationTOList[0].CreatedBy,prodCatId,prodSpecId, conn, tran);
                if(result < 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error While DeactivateAllPrevLoadingQuota : SaveLoadingQuotaDeclaration";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                #region 2. Assign New Quota 

                for (int i = 0; i < loadingQuotaDeclarationTOList.Count; i++)
                {
                    result = InsertTblLoadingQuotaDeclaration(loadingQuotaDeclarationTOList[i], conn, tran);
                    if(result!=1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While InsertTblLoadingQuotaDeclaration : SaveLoadingQuotaDeclaration";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }

                #endregion


                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveLoadingQuotaDeclaration";
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

        #endregion

        #region Updation
        public int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO)
        {
            return _iTblLoadingQuotaDeclarationDAO.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO);
        }

        public int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaDeclarationDAO.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota)
        {
            return _iTblLoadingQuotaDeclarationDAO.DeleteTblLoadingQuotaDeclaration(idLoadingQuota);
        }

        public int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaDeclarationDAO.DeleteTblLoadingQuotaDeclaration(idLoadingQuota, conn, tran);
        }

       

        #endregion

    }
}
