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
    public class TblOverdueDtlBL : ITblOverdueDtlBL
    {
        private readonly ITblOverdueDtlDAO _iTblOverdueDtlDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblOverdueDtlBL(ICommon iCommon, IConnectionString iConnectionString, ITblOverdueDtlDAO iTblOverdueDtlDAO)
        {
            _iTblOverdueDtlDAO = iTblOverdueDtlDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }

        #region Selection

        public List<TblOverdueDtlTO> SelectAllTblOverdueDtlList()
        {
            return  _iTblOverdueDtlDAO.SelectAllTblOverdueDtl();
        }
        
        /// <summary>
        /// [2017-12-01]Vijaymala:Added to get overdue detail List of organization
        /// </summary>
        /// <param name="dealerIds"></param>
        /// <returns></returns>
        public List<TblOverdueDtlTO> SelectAllTblOverdueDtlList(String dealerIds)
        {
            return _iTblOverdueDtlDAO.SelectAllTblOverdueDtl(dealerIds);
        }

        public TblOverdueDtlTO SelectTblOverdueDtlTO(Int32 idOverdueDtl)
        {
            return _iTblOverdueDtlDAO.SelectTblOverdueDtl(idOverdueDtl);
        }

        /// <summary>
        /// [2017-12-01]Vijaymala:Added to get overdue detail List of particular organization
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public List<TblOverdueDtlTO> SelectTblOverdueDtlList(Int32 dealerId)
        {
            return _iTblOverdueDtlDAO.SelectTblOverdueDtlList(dealerId);
        }



        #endregion

        #region Insertion
        public int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO)
        {
            return _iTblOverdueDtlDAO.InsertTblOverdueDtl(tblOverdueDtlTO);
        }

        public int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOverdueDtlDAO.InsertTblOverdueDtl(tblOverdueDtlTO, conn, tran);
        }


        /// <summary>
        /// [11-12-2017]Vijaymala :Added to save  overDue detail of organization which exports from excel
        /// </summary>
        /// <param name="tblOverdueDtlTOList"></param>
        /// <returns></returns>

        public ResultMessage SaveOrgOverDueDtl(List<TblOverdueDtlTO> tblOverdueDtlTOList, Int32 loginUserId)
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

                #region Delete previous records

                result = DeleteTblOverdueDtl(conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Exception Error While Delete TblOverdueDtl";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = -1;
                    return resultMessage;

                }

                #endregion


                #region Insert New Records

                DateTime createdOn = _iCommon.ServerDateTime;

                for (int i = 0; i < tblOverdueDtlTOList.Count; i++)
                {
                    TblOverdueDtlTO tblOverdueDtlTO = tblOverdueDtlTOList[i];
                    tblOverdueDtlTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblOverdueDtlTO.CreatedOn = createdOn;

                    result = InsertTblOverdueDtl(tblOverdueDtlTO, conn, tran);
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
                resultMessage.Text = "OverDue  Details Of Organization Updated Successfully.";
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.Text = "Exception Error While Record Save in BL : SaveOrgOverDueDtl";
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
        public int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO)
        {
            return _iTblOverdueDtlDAO.UpdateTblOverdueDtl(tblOverdueDtlTO);
        }

        public int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOverdueDtlDAO.UpdateTblOverdueDtl(tblOverdueDtlTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblOverdueDtl(Int32 idOverdueDtl)
        {
            return _iTblOverdueDtlDAO.DeleteTblOverdueDtl(idOverdueDtl);
        }

        public int DeleteTblOverdueDtl(Int32 idOverdueDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOverdueDtlDAO.DeleteTblOverdueDtl(idOverdueDtl, conn, tran);
        }

        /// <summary>
        /// [11-12-2017]Vijaymala: Added to delete previous overdue details
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteTblOverdueDtl(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOverdueDtlDAO.DeleteTblOverdueDtl(conn, tran);
        }

        #endregion

    }
}
