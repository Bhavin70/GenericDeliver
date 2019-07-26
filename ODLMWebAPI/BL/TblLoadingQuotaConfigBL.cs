using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
 
namespace ODLMWebAPI.BL
{
    public class TblLoadingQuotaConfigBL : ITblLoadingQuotaConfigBL
    {
        private readonly ITblLoadingQuotaConfigDAO _iTblLoadingQuotaConfigDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblLoadingQuotaConfigBL(ICommon iCommon, IConnectionString iConnectionString, ITblLoadingQuotaConfigDAO iTblLoadingQuotaConfigDAO)
        {
            _iTblLoadingQuotaConfigDAO = iTblLoadingQuotaConfigDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblLoadingQuotaConfigTO> SelectAllTblLoadingQuotaConfigList()
        {
           return  _iTblLoadingQuotaConfigDAO.SelectAllTblLoadingQuotaConfig();
        }

        public TblLoadingQuotaConfigTO SelectTblLoadingQuotaConfigTO(Int32 idLoadQuotaConfig)
        {
            return _iTblLoadingQuotaConfigDAO.SelectTblLoadingQuotaConfig(idLoadQuotaConfig);
        }

        public List<TblLoadingQuotaConfigTO> SelectLatestLoadingQuotaConfigList(Int32 prodCatId, Int32 prodSpecId)
        {
            return _iTblLoadingQuotaConfigDAO.SelectLatestLoadingQuotaConfig(prodCatId,prodSpecId);

        }

        public List<TblLoadingQuotaConfigTO> SelectEmptyLoadingQuotaConfig(SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingQuotaConfigDAO.SelectEmptyLoadingQuotaConfig(conn, tran);
        }

       
        #endregion

        #region Insertion
        public int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO)
        {
            return _iTblLoadingQuotaConfigDAO.InsertTblLoadingQuotaConfig(tblLoadingQuotaConfigTO);
        }

        public int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaConfigDAO.InsertTblLoadingQuotaConfig(tblLoadingQuotaConfigTO, conn, tran);
        }

        public ResultMessage SaveNewLoadingQuotaConfiguration(List<TblLoadingQuotaConfigTO> loadingQuotaConfigTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage = SaveNewLoadingQuotaConfiguration(loadingQuotaConfigTOList, conn, tran);
                if (resultMessage.MessageType == ResultMessageE.Error)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Record Could Not Be Saved";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveNewLoadingQuotaConfiguration";
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

        public ResultMessage SaveNewLoadingQuotaConfiguration(List<TblLoadingQuotaConfigTO> loadingQuotaConfigTOList, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {

                if (loadingQuotaConfigTOList != null && loadingQuotaConfigTOList.Count > 0)
                {

                    DateTime deactivatedDate = _iCommon.ServerDateTime;
                    for (int i = 0; i < loadingQuotaConfigTOList.Count; i++)
                    {
                        //If Already Exist Then Deactivate it

                        TblLoadingQuotaConfigTO tblLoadingQuotaConfigTOOld = new TblLoadingQuotaConfigTO();
                        tblLoadingQuotaConfigTOOld.IdLoadQuotaConfig = loadingQuotaConfigTOList[i].IdLoadQuotaConfig;
                        tblLoadingQuotaConfigTOOld.IsActive = 0;
                        tblLoadingQuotaConfigTOOld.DeactivatedBy = loadingQuotaConfigTOList[i].CreatedBy;
                        tblLoadingQuotaConfigTOOld.DeactivatedOn = deactivatedDate;
                        tblLoadingQuotaConfigTOOld.CnfOrgId = loadingQuotaConfigTOList[i].CnfOrgId;
                        tblLoadingQuotaConfigTOOld.MaterialId = loadingQuotaConfigTOList[i].MaterialId;
                        tblLoadingQuotaConfigTOOld.ProdCatId = loadingQuotaConfigTOList[i].ProdCatId;
                        tblLoadingQuotaConfigTOOld.ProdSpecId = loadingQuotaConfigTOList[i].ProdSpecId;

                        result = _iTblLoadingQuotaConfigDAO.DeactivateLoadingQuotaConfig(tblLoadingQuotaConfigTOOld, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While DeactivateLoadingQuotaConfig";
                            resultMessage.Result = 0;
                            return resultMessage;
                        }

                        //Insert New Configuration
                        loadingQuotaConfigTOList[i].IsActive = 1;
                        result = InsertTblLoadingQuotaConfig(loadingQuotaConfigTOList[i], conn, tran);
                        if (result != 1)
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblLoadingQuotaConfig";
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loadingQuotaConfigTOList Found Null";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveNewLoadingQuotaConfiguration";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {

            }
        }

        #endregion

        #region Updation
        public int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO)
        {
            return _iTblLoadingQuotaConfigDAO.UpdateTblLoadingQuotaConfig(tblLoadingQuotaConfigTO);
        }

        public int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaConfigDAO.UpdateTblLoadingQuotaConfig(tblLoadingQuotaConfigTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig)
        {
            return _iTblLoadingQuotaConfigDAO.DeleteTblLoadingQuotaConfig(idLoadQuotaConfig);
        }

        public int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaConfigDAO.DeleteTblLoadingQuotaConfig(idLoadQuotaConfig, conn, tran);
        }

        #endregion
        
    }
}
