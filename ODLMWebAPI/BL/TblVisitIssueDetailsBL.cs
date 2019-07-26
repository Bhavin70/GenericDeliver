using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblVisitIssueDetailsBL : ITblVisitIssueDetailsBL
    {
        private readonly ITblVisitIssueDetailsDAO _iTblVisitIssueDetailsDAO;
        private readonly ITblVisitIssueReasonsBL _iTblVisitIssueReasonsBL;
        private readonly ICommon _iCommon;
        public TblVisitIssueDetailsBL(ICommon iCommon, ITblVisitIssueReasonsBL iTblVisitIssueReasonsBL,ITblVisitIssueDetailsDAO iTblVisitIssueDetailsDAO)
        {
            _iTblVisitIssueDetailsDAO = iTblVisitIssueDetailsDAO;
            _iTblVisitIssueReasonsBL = iTblVisitIssueReasonsBL;
            _iCommon = iCommon;
        }
        #region Selection
        public DataTable SelectAllTblVisitIssueDetails()
        {
            return _iTblVisitIssueDetailsDAO.SelectAllTblVisitIssueDetails();
        }

        public List<TblVisitIssueDetailsTO> SelectAllTblVisitIssueDetailsList()
        {
            DataTable tblVisitIssueDetailsTODT = _iTblVisitIssueDetailsDAO.SelectAllTblVisitIssueDetails();
            return ConvertDTToList(tblVisitIssueDetailsTODT);
        }

        public TblVisitIssueDetailsTO SelectTblVisitIssueDetailsTO(Int32 idIssue)
        {
            DataTable tblVisitIssueDetailsTODT = _iTblVisitIssueDetailsDAO.SelectTblVisitIssueDetails(idIssue);
            List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTOList = ConvertDTToList(tblVisitIssueDetailsTODT);
            if (tblVisitIssueDetailsTOList != null && tblVisitIssueDetailsTOList.Count == 1)
                return tblVisitIssueDetailsTOList[0];
            else
                return null;
        }

        public List<TblVisitIssueDetailsTO> ConvertDTToList(DataTable tblVisitIssueDetailsTODT)
        {
            List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTOList = new List<TblVisitIssueDetailsTO>();
            if (tblVisitIssueDetailsTODT != null)
            {
                
            }
            return tblVisitIssueDetailsTOList;
        }


        public List<TblVisitIssueDetailsTO> SelectVisitIssueDetailsTOList(Int32 visitId)
        {
            List<TblVisitIssueDetailsTO> visitIssueDetailsTOList = _iTblVisitIssueDetailsDAO.SelectVisitIssueDetailsList(visitId);
            if (visitIssueDetailsTOList != null )
                return visitIssueDetailsTOList;
            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO)
        {
            return _iTblVisitIssueDetailsDAO.InsertTblVisitIssueDetails(tblVisitIssueDetailsTO);
        }

        public int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitIssueDetailsDAO.InsertTblVisitIssueDetails(tblVisitIssueDetailsTO, conn, tran);
        }


        // Vaibhav [9-Oct-2017] added to insert visit issue details
        public ResultMessage SaveVisitIssueDetails(List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTO, Int32 createdBy,Int32 visitId,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                foreach (var visitIssueDetailsTO in tblVisitIssueDetailsTO)
                {
                    // Insertion
                    if (visitIssueDetailsTO.IdIssue <= 0)
                    {
                        if (visitIssueDetailsTO.VisitId <= 0)
                            visitIssueDetailsTO.VisitId = visitId;

                        if (visitIssueDetailsTO.IssueReasonId <= 0)
                        {
                            TblVisitIssueReasonsTO visitIssueReasonsTO = new TblVisitIssueReasonsTO();

                            visitIssueReasonsTO.IssueTypeId = visitIssueDetailsTO.IssueTypeId;
                            visitIssueReasonsTO.VisitIssueReasonName = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.VisitIssueReasonDesc = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.IsActive = 1;

                            result = _iTblVisitIssueReasonsBL.InsertTblVisitIssueReasons(ref visitIssueReasonsTO,conn,tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueReasons");
                                tran.Rollback();
                                return resultMessage;
                            }
                            visitIssueDetailsTO.IssueReasonId = visitIssueReasonsTO.IdVisitIssueReasons;
                        }

                            visitIssueDetailsTO.CreatedBy = createdBy;
                        visitIssueDetailsTO.CreatedOn = _iCommon.ServerDateTime;

                        result = InsertTblVisitIssueDetails(visitIssueDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Updation
                    else
                    { 
                        if (visitIssueDetailsTO.VisitId <= 0)
                            visitIssueDetailsTO.VisitId = visitId;

                        if (visitIssueDetailsTO.IssueReasonId <= 0)
                        {
                            TblVisitIssueReasonsTO visitIssueReasonsTO = new TblVisitIssueReasonsTO();

                            visitIssueReasonsTO.IssueTypeId = visitIssueDetailsTO.IssueTypeId;
                            visitIssueReasonsTO.VisitIssueReasonName = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.VisitIssueReasonDesc = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.IsActive = 1;

                            result = _iTblVisitIssueReasonsBL.InsertTblVisitIssueReasons(ref visitIssueReasonsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueReasons");
                                tran.Rollback();
                                return resultMessage;
                            }
                            visitIssueDetailsTO.IssueReasonId = visitIssueReasonsTO.IdVisitIssueReasons;
                        }

                        visitIssueDetailsTO.UpdatedBy = createdBy;
                        visitIssueDetailsTO.UpdatedOn = _iCommon.ServerDateTime;

                        result = UpdateTblVisitIssueDetails(visitIssueDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateTblVisitIssueDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }
               
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveVisitIssueDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO)
        {
            return _iTblVisitIssueDetailsDAO.UpdateTblVisitIssueDetails(tblVisitIssueDetailsTO);
        }

        public int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitIssueDetailsDAO.UpdateTblVisitIssueDetails(tblVisitIssueDetailsTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] added to update visit issue details
        public ResultMessage UpdateVisitIssueDetails(List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTO, Int32 updatedBy, Int32 visitId,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {

                foreach (var visitIssueDetailsTO in tblVisitIssueDetailsTO)
                {
                    if (visitIssueDetailsTO.IdIssue > 0)
                    {
                        if (visitIssueDetailsTO.VisitId <= 0)
                            visitIssueDetailsTO.VisitId = visitId;

                        if (visitIssueDetailsTO.IssueReasonId <= 0)
                        {
                            TblVisitIssueReasonsTO visitIssueReasonsTO = new TblVisitIssueReasonsTO();

                            visitIssueReasonsTO.IssueTypeId = visitIssueDetailsTO.IssueTypeId;
                            visitIssueReasonsTO.VisitIssueReasonName = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.VisitIssueReasonDesc = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.IsActive = 1;

                            result = _iTblVisitIssueReasonsBL.InsertTblVisitIssueReasons(ref visitIssueReasonsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueReasons");
                                tran.Rollback();
                                return resultMessage;
                            }
                            visitIssueDetailsTO.IssueReasonId = visitIssueReasonsTO.IdVisitIssueReasons;
                        }

                        visitIssueDetailsTO.UpdatedBy = updatedBy;
                        visitIssueDetailsTO.UpdatedOn = _iCommon.ServerDateTime;

                        result = UpdateTblVisitIssueDetails(visitIssueDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateTblVisitIssueDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Insert visit issue details while updation
                    if (visitIssueDetailsTO.IdIssue <= 0)
                    {
                        if (visitIssueDetailsTO.VisitId <= 0)
                            visitIssueDetailsTO.VisitId = visitId;

                        if (visitIssueDetailsTO.IssueReasonId <= 0)
                        {
                            TblVisitIssueReasonsTO visitIssueReasonsTO = new TblVisitIssueReasonsTO();

                            visitIssueReasonsTO.IssueTypeId = visitIssueDetailsTO.IssueTypeId;
                            visitIssueReasonsTO.VisitIssueReasonName = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.VisitIssueReasonDesc = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.IsActive = 1;

                            result = _iTblVisitIssueReasonsBL.InsertTblVisitIssueReasons(ref visitIssueReasonsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueReasons");
                                tran.Rollback();
                                return resultMessage;
                            }
                            visitIssueDetailsTO.IssueReasonId = visitIssueReasonsTO.IdVisitIssueReasons;
                        }

                        visitIssueDetailsTO.CreatedBy = updatedBy;
                        visitIssueDetailsTO.CreatedOn = _iCommon.ServerDateTime;

                        result = InsertTblVisitIssueDetails(visitIssueDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in InsertTblVisitIssueDetails while updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitIssueDetails");
                tran.Rollback();
                return resultMessage;
            }
        }


        #endregion

        #region Deletion
        public int DeleteTblVisitIssueDetails(Int32 idIssue)
        {
            return _iTblVisitIssueDetailsDAO.DeleteTblVisitIssueDetails(idIssue);
        }

        public int DeleteTblVisitIssueDetails(Int32 idIssue, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitIssueDetailsDAO.DeleteTblVisitIssueDetails(idIssue, conn, tran);
        }

        #endregion

    }
}
