using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL;
using System.Data.SqlClient;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class MarketingDetailsBL : IMarketingDetailsBL
    {
        private readonly ITblVisitDetailsBL _iTblVisitDetailsBL;
        private readonly ITblSiteRequirementsBL _iTblSiteRequirementsBL;
        private readonly ITblVisitAdditionalDetailsBL _iTblVisitAdditionalDetailsBL;
        private readonly ITblVisitFollowupInfoBL _iTblVisitFollowupInfoBL;
        private readonly ITblVisitIssueDetailsBL _iTblVisitIssueDetailsBL;
        private readonly ITblVisitProjectDetailsBL _iTblVisitProjectDetailsBL;
        private readonly IConnectionString _iConnectionString;
        public MarketingDetailsBL(IConnectionString iConnectionString, ITblVisitProjectDetailsBL iTblVisitProjectDetailsBL, ITblVisitIssueDetailsBL iTblVisitIssueDetailsBL, ITblVisitFollowupInfoBL iTblVisitFollowupInfoBL, ITblVisitAdditionalDetailsBL iTblVisitAdditionalDetailsBL, ITblSiteRequirementsBL iTblSiteRequirementsBL, ITblVisitDetailsBL iTblVisitDetailsBL)
        {
            _iTblVisitDetailsBL = iTblVisitDetailsBL;
            _iTblSiteRequirementsBL = iTblSiteRequirementsBL;
            _iTblVisitAdditionalDetailsBL = iTblVisitAdditionalDetailsBL;
            _iTblVisitFollowupInfoBL = iTblVisitFollowupInfoBL;
            _iTblVisitIssueDetailsBL = iTblVisitIssueDetailsBL;
            _iTblVisitProjectDetailsBL = iTblVisitProjectDetailsBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public MarketingDetailsTO SelectVisitDetailsList(int visitId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                MarketingDetailsTO marketingDetailsTO = new MarketingDetailsTO();
                // Select Visit Basic Data
                marketingDetailsTO.VisitDetailsTO = _iTblVisitDetailsBL.SelectTblVisitDetailsTO(visitId);

                // Select Site Requirement Data
                marketingDetailsTO.RequirementTO = _iTblSiteRequirementsBL.SelectSiteRequirementsTO(visitId);

                // Select Visit Additional Data
                marketingDetailsTO.AdditionalInfoTO = _iTblVisitAdditionalDetailsBL.SelectVisitAdditionalDetailsTO(visitId);

                // Select Visit Follow Up Information
                marketingDetailsTO.VisitFollowUpInfoTo = _iTblVisitFollowupInfoBL.SelectVisitFollowupInfoTO(visitId);

                // Select Visit Issue Details Data
                marketingDetailsTO.VisitIssueDetailsTOList = _iTblVisitIssueDetailsBL.SelectVisitIssueDetailsTOList(visitId);

                // Select Visit Project Data
                marketingDetailsTO.VisitProjectDetailsTOList = _iTblVisitProjectDetailsBL.SelectProjectDetailsList(visitId);

                return marketingDetailsTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectVisitDetailsList");
                return null;
            }
        }

      #endregion

        #region Insertion
        public ResultMessage SaveMarketingDetails(MarketingDetailsTO marketingDetailsTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int visitId;

            if (marketingDetailsTO.VisitDetailsTO.IdVisit <= 0)
                visitId = 0;
            else
                visitId = marketingDetailsTO.VisitDetailsTO.IdVisit;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                // Save Visit Basic Details
                if (marketingDetailsTO.VisitDetailsTO != null)
                {
                    // Insertion
                    if (marketingDetailsTO.VisitDetailsTO.IdVisit <= 0)
                    {
                        marketingDetailsTO.VisitDetailsTO.CreatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.VisitDetailsTO.CreatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = _iTblVisitDetailsBL.SaveNewVisitDetails(marketingDetailsTO.VisitDetailsTO, conn, tran);

                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            visitId = resultMessage.Result;
                        }
                        else
                        {
                            resultMessage.DefaultBehaviour("Error While SaveNewVisitDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Updation
                    if (marketingDetailsTO.VisitDetailsTO.IdVisit > 0)
                    {
                        marketingDetailsTO.VisitDetailsTO.UpdatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.VisitDetailsTO.UpdatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = _iTblVisitDetailsBL.UpdateVisitDetails(marketingDetailsTO.VisitDetailsTO, conn, tran);

                        if(resultMessage.MessageType==ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error In UpdateVisitDetails While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }

                // Save Vist Requirements
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.RequirementTO != null)
                {
                    // Insertion
                    if (marketingDetailsTO.RequirementTO != null && marketingDetailsTO.RequirementTO.IdSiteRequirement <= 0)
                    {
                            marketingDetailsTO.RequirementTO.VisitId = visitId;

                        marketingDetailsTO.RequirementTO.CreatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.RequirementTO.CreatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = _iTblSiteRequirementsBL.SaveSiteRequirements(marketingDetailsTO.RequirementTO, conn, tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error While SaveSiteRequirements");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                    // Updation
                    if (marketingDetailsTO.RequirementTO != null && marketingDetailsTO.RequirementTO.IdSiteRequirement > 0)
                    {
                        marketingDetailsTO.RequirementTO.VisitId = visitId;

                        marketingDetailsTO.RequirementTO.UpdatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.RequirementTO.UpdatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = _iTblSiteRequirementsBL.UpdateSiteRequirements(marketingDetailsTO.RequirementTO, conn, tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error In UpdateSiteRequirements While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }

                // Save Visit Additional Information
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.AdditionalInfoTO != null)
                {
                    // Insertion
                    if (marketingDetailsTO.AdditionalInfoTO != null && marketingDetailsTO.AdditionalInfoTO.IdVisitDetails <= 0)
                    {
                            marketingDetailsTO.AdditionalInfoTO.VisitId = visitId;

                        marketingDetailsTO.AdditionalInfoTO.CreatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.AdditionalInfoTO.CreatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = _iTblVisitAdditionalDetailsBL.SaveVisitAdditionalInfo(marketingDetailsTO.AdditionalInfoTO,conn,tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error While SaveVisitAdditionalInfo");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Updation
                    if (marketingDetailsTO.AdditionalInfoTO != null && marketingDetailsTO.AdditionalInfoTO.IdVisitDetails > 0)
                    {
                        marketingDetailsTO.AdditionalInfoTO.VisitId = visitId;

                        marketingDetailsTO.AdditionalInfoTO.UpdatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.AdditionalInfoTO.UpdatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = _iTblVisitAdditionalDetailsBL.UpdateVisitAdditionalInfo(marketingDetailsTO.AdditionalInfoTO, conn, tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error In SaveVisitAdditionalInfo While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }


                // Save Visit Follow Up Information
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.VisitFollowUpInfoTo != null)
                {
                    // Insertion
                    if (marketingDetailsTO.VisitFollowUpInfoTo != null && marketingDetailsTO.VisitFollowUpInfoTo.IdProjectFollowUpInfo <= 0)
                    {
                            marketingDetailsTO.VisitFollowUpInfoTo.VisitId = visitId;

                        marketingDetailsTO.VisitFollowUpInfoTo.CreatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.VisitFollowUpInfoTo.CreatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = _iTblVisitFollowupInfoBL.SaveVisitFollowUpInfo(marketingDetailsTO.VisitFollowUpInfoTo,conn,tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error While SaveVisitFollowUpInfo");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Updation
                    if (marketingDetailsTO.VisitFollowUpInfoTo != null && marketingDetailsTO.VisitFollowUpInfoTo.IdProjectFollowUpInfo > 0)
                    {
                        marketingDetailsTO.VisitFollowUpInfoTo.VisitId = visitId;

                        marketingDetailsTO.VisitFollowUpInfoTo.UpdatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.VisitFollowUpInfoTo.UpdatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = _iTblVisitFollowupInfoBL.UpdateVisitFollowUpInfo(marketingDetailsTO.VisitFollowUpInfoTo, conn, tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error In SaveVisitFollowUpInfo While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }


                // Save Visit Issue Details
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.VisitIssueDetailsTOList != null)
                {
                    // Insertion
                    if (marketingDetailsTO.VisitIssueDetailsTOList != null &&  marketingDetailsTO.VisitIssueDetailsTOList.Count > 0)
                    {

                        resultMessage = _iTblVisitIssueDetailsBL.SaveVisitIssueDetails(marketingDetailsTO.VisitIssueDetailsTOList, marketingDetailsTO.CreatedBy, visitId,conn,tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error While SaveVisitIssueDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }


                // Save Project Details
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.VisitProjectDetailsTOList != null) 
                {
                    if (marketingDetailsTO.VisitProjectDetailsTOList != null && marketingDetailsTO.VisitProjectDetailsTOList.Count > 0)
                    {
                        resultMessage = _iTblVisitProjectDetailsBL.SaveVisitProjectDetails(marketingDetailsTO.VisitProjectDetailsTOList, marketingDetailsTO.CreatedBy, visitId,conn,tran);
                    }
                    if (resultMessage.MessageType == ResultMessageE.Error)
                    {
                        resultMessage.DefaultBehaviour("Error While SaveVisitProjectDetails");
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Result = visitId;
                tran.Commit();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveMarketingDetails");
                tran.Rollback();
                return resultMessage;
            }
        }
        #endregion

        #region updation
        public ResultMessage UpdateMarketingDetails(MarketingDetailsTO marketingDetailsTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            ResultMessage resultMessage = new ResultMessage();
            int visitId;

            if (marketingDetailsTO.VisitDetailsTO.IdVisit <= 0)
                visitId = 0;
            else
                visitId = marketingDetailsTO.VisitDetailsTO.IdVisit;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                // Update Visit Basic Details
                if (marketingDetailsTO.VisitDetailsTO != null && marketingDetailsTO.VisitDetailsTO.IdVisit > 0)
                {
                    marketingDetailsTO.VisitDetailsTO.UpdatedBy = marketingDetailsTO.UpdatedBy;
                    marketingDetailsTO.VisitDetailsTO.UpdatedOn = marketingDetailsTO.UpdatedOn;

                    resultMessage = _iTblVisitDetailsBL.UpdateVisitDetails(marketingDetailsTO.VisitDetailsTO,conn,tran);

                    if (resultMessage.MessageType == ResultMessageE.Error)
                    {
                        resultMessage.DefaultBehaviour("Error While UpdateVisitDetails");
                        tran.Rollback();
                        return resultMessage;
                    }
                }

                // Update Vist Requirements
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.RequirementTO != null)
                {
                    if (marketingDetailsTO.RequirementTO != null && marketingDetailsTO.RequirementTO.IdSiteRequirement>0)
                    {
                            marketingDetailsTO.RequirementTO.VisitId = visitId;

                        marketingDetailsTO.RequirementTO.UpdatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.RequirementTO.UpdatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = _iTblSiteRequirementsBL.UpdateSiteRequirements(marketingDetailsTO.RequirementTO,conn,tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateSiteRequirements");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Insert requirements details while updation
                    if (marketingDetailsTO.RequirementTO != null && marketingDetailsTO.RequirementTO.IdSiteRequirement <= 0)
                    {
                            marketingDetailsTO.RequirementTO.VisitId = visitId;

                        marketingDetailsTO.RequirementTO.CreatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.RequirementTO.CreatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = _iTblSiteRequirementsBL.SaveSiteRequirements(marketingDetailsTO.RequirementTO, conn, tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error In SaveSiteRequirements While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }
                

                // Update Visit Additional Information
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.AdditionalInfoTO != null)
                {
                    if (marketingDetailsTO.AdditionalInfoTO != null && marketingDetailsTO.AdditionalInfoTO.IdVisitDetails > 0)
                    {
                            marketingDetailsTO.AdditionalInfoTO.VisitId = visitId;

                        marketingDetailsTO.AdditionalInfoTO.UpdatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.AdditionalInfoTO.UpdatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = _iTblVisitAdditionalDetailsBL.UpdateVisitAdditionalInfo(marketingDetailsTO.AdditionalInfoTO,conn,tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateVisitAdditionalInfo");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Insert visit additional info while updation
                    if (marketingDetailsTO.AdditionalInfoTO != null && marketingDetailsTO.AdditionalInfoTO.IdVisitDetails <= 0)
                    {
                            marketingDetailsTO.AdditionalInfoTO.VisitId = visitId;

                        marketingDetailsTO.AdditionalInfoTO.CreatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.AdditionalInfoTO.CreatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = _iTblVisitAdditionalDetailsBL.SaveVisitAdditionalInfo(marketingDetailsTO.AdditionalInfoTO, conn, tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error in SaveVisitAdditionalInfo while updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }
               
                // Update Visit Follow Up Information
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.VisitFollowUpInfoTo != null)
                {
                    if (marketingDetailsTO.VisitFollowUpInfoTo != null && marketingDetailsTO.VisitFollowUpInfoTo.IdProjectFollowUpInfo > 0)
                    {
                            marketingDetailsTO.VisitFollowUpInfoTo.VisitId = visitId;

                        marketingDetailsTO.VisitFollowUpInfoTo.UpdatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.VisitFollowUpInfoTo.UpdatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = _iTblVisitFollowupInfoBL.UpdateVisitFollowUpInfo(marketingDetailsTO.VisitFollowUpInfoTo,conn,tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateVisitFollowUpInfo");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    if (marketingDetailsTO.VisitFollowUpInfoTo != null && marketingDetailsTO.VisitFollowUpInfoTo.IdProjectFollowUpInfo <= 0)
                    {
                            marketingDetailsTO.VisitFollowUpInfoTo.VisitId = visitId;

                        marketingDetailsTO.VisitFollowUpInfoTo.CreatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.VisitFollowUpInfoTo.CreatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = _iTblVisitFollowupInfoBL.SaveVisitFollowUpInfo(marketingDetailsTO.VisitFollowUpInfoTo, conn, tran);

                        if (resultMessage.MessageType == ResultMessageE.Error)
                        {
                            resultMessage.DefaultBehaviour("Error in SaveVisitFollowUpInfo while updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }

                // Update Visit Issue Details
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.VisitIssueDetailsTOList != null)
                {
                    if (marketingDetailsTO.VisitIssueDetailsTOList != null && marketingDetailsTO.VisitIssueDetailsTOList.Count > 0)
                    {

                        //resultMessage = _iTblVisitIssueDetailsBL.UpdateVisitIssueDetails(marketingDetailsTO.VisitIssueDetailsTOList, marketingDetailsTO.UpdatedBy, visitId,conn,tran);
                        resultMessage = _iTblVisitIssueDetailsBL.SaveVisitIssueDetails(marketingDetailsTO.VisitIssueDetailsTOList, marketingDetailsTO.UpdatedBy, visitId, conn, tran);
                    }
                    if (resultMessage.MessageType == ResultMessageE.Error)
                    {
                        resultMessage.DefaultBehaviour("Error While SaveVisitIssueDetails");
                        tran.Rollback();
                        return resultMessage;
                    }
                }
                
                // Update Visit Project Details
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.VisitProjectDetailsTOList != null)
                {
                    if (marketingDetailsTO.VisitProjectDetailsTOList != null && marketingDetailsTO.VisitProjectDetailsTOList.Count > 0)
                    {
                        //resultMessage = _iTblVisitProjectDetailsBL.UpdateVisitProjectDetails(marketingDetailsTO.VisitProjectDetailsTOList, marketingDetailsTO.UpdatedBy, visitId,conn,tran);
                        resultMessage = _iTblVisitProjectDetailsBL.SaveVisitProjectDetails(marketingDetailsTO.VisitProjectDetailsTOList, marketingDetailsTO.UpdatedBy, visitId, conn, tran);
                    }
                    if (resultMessage.MessageType == ResultMessageE.Error)
                    {
                        resultMessage.DefaultBehaviour("Error While SaveVisitProjectDetails");
                        tran.Rollback();
                        return resultMessage;
                    }
                }
                
                resultMessage.DefaultSuccessBehaviour();
                tran.Commit();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateMarketingDetails");
                tran.Rollback();
                return resultMessage;
            }
        }
        #endregion
    }
}
