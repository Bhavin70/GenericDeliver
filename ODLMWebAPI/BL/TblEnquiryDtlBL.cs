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
    public class TblEnquiryDtlBL : ITblEnquiryDtlBL
    {
        private readonly ITblEnquiryDtlDAO _iTblEnquiryDtlDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblEnquiryDtlBL(ICommon iCommon, ITblEnquiryDtlDAO iTblEnquiryDtlDAO, IConnectionString iConnectionString)
        {
            _iTblEnquiryDtlDAO = iTblEnquiryDtlDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblEnquiryDtlTO> SelectAllTblEnquiryDtlList()
        {
            return _iTblEnquiryDtlDAO.SelectAllTblEnquiryDtl();
        }

        /// <summary>
        ///  [2017-12-01]Vijaymala:Added to get enquiry detail List  of  organization
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public List<TblEnquiryDtlTO> SelectAllTblEnquiryDtl(String dealerIds)
        {
            return _iTblEnquiryDtlDAO.SelectAllTblEnquiryDtl(dealerIds);
        }


        /// <summary>
        ///  [2017-11-29]Vijaymala:Added to get enquiry detail of particular organization
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public List<TblEnquiryDtlTO> SelectEnquiryDtlList(Int32 dealerId)
        {
            return _iTblEnquiryDtlDAO.SelectEnquiryDtlList(dealerId);
        }

        public TblEnquiryDtlTO SelectTblEnquiryDtl(Int32 idEnquiryDtl)
        {
            return _iTblEnquiryDtlDAO.SelectTblEnquiryDtl(idEnquiryDtl);
        }



        #endregion

        #region Insertion
        public int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO)
        {
            return _iTblEnquiryDtlDAO.InsertTblEnquiryDtl(tblEnquiryDtlTO);
        }

        public int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEnquiryDtlDAO.InsertTblEnquiryDtl(tblEnquiryDtlTO, conn, tran);
        }

        /// <summary>
        /// [04-12-2017]Vijaymala :Added to save  enquiry detail of organization which exports from excel
        /// </summary>
        /// <param name="tblEnquiryDtlTO"></param>
        /// <returns></returns>

        public ResultMessage SaveOrgEnquiryDtl(List<TblEnquiryDtlTO> tblEnquiryDtlTOList,Int32 loginUserId)
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

                #region validations

                //for (int i = 0; i < tblEnquiryDtlTOList.Count; i++)
                //{
                //    TblEnquiryDtlTO tblEnquiryDtlTO = tblEnquiryDtlTOList[i];
                //    TblOrganizationTO tblOrganizationTO = BL.TblOrganizationBL.SelectTblOrganizationTOByEnqRefId(tblEnquiryDtlTO.EnqRefId);
                //    if(tblOrganizationTO !=null)
                //    {
                //        tblEnquiryDtlTO.IsMatch = 1;
                //    }
                //    else
                //    {
                //        tblEnquiryDtlTO.IsMatch = 0;
                //    }
                //    // tblEnquiryDtlTO.CreatedBy = Convert.ToInt32(loginUserId);
                //    // tblEnquiryDtlTO.CreatedOn = _iCommon.ServerDateTime;
                //    // tblEnquiryDtlTO.IsActive = 1;

                //    //#region 1. Deactivate All Previous Organization Details
                //    //TblEnquiryDtlTO activeEnquiryDtlTO = _iTblEnquiryDtlDAO.SelectOrganizationEnquiryDtl(tblEnquiryDtlTO.EnqRefId);
                //    //if (activeEnquiryDtlTO != null)
                //    //{
                //    //    tblEnquiryDtlTO.OrganizationId = activeEnquiryDtlTO.OrganizationId;
                //    //    result = DAL._iTblEnquiryDtlDAO.DeactivateOrgEnqDeatls(activeEnquiryDtlTO.IdEnquiryDtl, conn, tran);
                //    //    if (result < 0)
                //    //    {
                //    //        tran.Rollback();
                //    //        resultMessage.DefaultBehaviour();
                //    //        resultMessage.Text = "Error While Deactivating Organization Enquiry Details";
                //    //        return resultMessage;
                //    //    }
                //    //}

                //    //#endregion

                //    //#region 2. Save Organization Enquiry Details
                //    //result = InsertTblEnquiryDtl(tblEnquiryDtlTO, conn, tran);
                //    //if (result < 0)
                //    //{
                //    //    tran.Rollback();
                //    //    resultMessage.DefaultBehaviour();
                //    //    resultMessage.Text = "Error While Inserting Organization Enquiry Details";
                //    //    return resultMessage;
                //    //}
                //}

                ////List<TblEnquiryDtlTO> listTemp = tblEnquiryDtlTOList.Where(w => w.IsMatch == 0).ToList();
                ////if (listTemp != null && listTemp.Count > 0)
                ////{
                ////    //return tblEnquiryDtlTOList
                ////}


                #endregion

                #region Delete previous records

                result = DeleteTblEnquiryDtl(conn, tran);
                if(result == -1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Exception Error While Delete TblEnquiryDtl";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = -1;
                    return resultMessage;

                }

                #endregion


                #region Insert New Records

                DateTime createdOn = _iCommon.ServerDateTime;

                for (int i = 0; i < tblEnquiryDtlTOList.Count; i++)
                {
                    TblEnquiryDtlTO tblEnquiryDtlTO = tblEnquiryDtlTOList[i];
                    //TblOrganizationTO tblOrganizationTO = BL.TblOrganizationBL.SelectTblOrganizationTOByEnqRefId(tblEnquiryDtlTO.EnqRefId);
                    //if (tblOrganizationTO != null)
                    //    tblEnquiryDtlTO.OrganizationId = tblOrganizationTO.IdOrganization;
                    //else
                    //    tblEnquiryDtlTO.OrganizationId = 0;

                    tblEnquiryDtlTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblEnquiryDtlTO.CreatedOn = createdOn;

                    result = InsertTblEnquiryDtl(tblEnquiryDtlTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error While Inserting Organization Enquiry Details";
                        return resultMessage;
                    }

                }
                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Enquiry Details Of Organization Updated Successfully.";
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.Text = "Exception Error While Record Save in BL : SaveOrgEnquiryDtl";
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
        public int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO)
        {
            return _iTblEnquiryDtlDAO.UpdateTblEnquiryDtl(tblEnquiryDtlTO);
        }

        public int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEnquiryDtlDAO.UpdateTblEnquiryDtl(tblEnquiryDtlTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblEnquiryDtl(Int32 idEnquiryDtl)
        {
            return _iTblEnquiryDtlDAO.DeleteTblEnquiryDtl(idEnquiryDtl);
        }

        public int DeleteTblEnquiryDtl(Int32 idEnquiryDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEnquiryDtlDAO.DeleteTblEnquiryDtl(idEnquiryDtl, conn, tran);
        }

        public int DeleteTblEnquiryDtl(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEnquiryDtlDAO.DeleteTblEnquiryDtl(conn, tran);
        }


        #endregion
    }
}



