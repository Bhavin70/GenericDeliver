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
    public class TblVisitDetailsBL : ITblVisitDetailsBL
    {
        private readonly ITblVisitDetailsDAO _iTblVisitDetailsDAO;
        private readonly ITblVisitPurposeBL _iTblVisitPurposeBL;
        private readonly ITblPaymentTermBL _iTblPaymentTermBL;
        private readonly ITblSiteStatusBL _iTblSiteStatusBL;
        private readonly ITblSiteTypeBL _iTblSiteTypeBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ITblVisitPersonDetailsBL _iTblVisitPersonDetailsBL;
        public TblVisitDetailsBL(ITblVisitPersonDetailsBL iTblVisitPersonDetailsBL,ITblPersonBL iTblPersonBL,ITblSiteTypeBL iTblSiteTypeBL,ITblSiteStatusBL iTblSiteStatusBL,ITblPaymentTermBL iTblPaymentTermBL,ITblVisitPurposeBL iTblVisitPurposeBL,ITblVisitDetailsDAO iTblVisitDetailsDAO)
        {
            _iTblVisitDetailsDAO = iTblVisitDetailsDAO;
            _iTblVisitPurposeBL = iTblVisitPurposeBL;
            _iTblPaymentTermBL = iTblPaymentTermBL;
            _iTblSiteStatusBL = iTblSiteStatusBL;
            _iTblSiteTypeBL = iTblSiteTypeBL;
            _iTblPersonBL = iTblPersonBL;
            _iTblVisitPersonDetailsBL = iTblVisitPersonDetailsBL;
        }
        #region Selection
        public List<TblVisitDetailsTO> SelectAllTblVisitDetails()
        {
            return _iTblVisitDetailsDAO.SelectAllTblVisitDetails();
        }

        public List<TblVisitDetailsTO> SelectAllTblVisitDetailsList()
        {
            List<TblVisitDetailsTO> tblVisitDetailsTODT = _iTblVisitDetailsDAO.SelectAllTblVisitDetails();
            //return ConvertDTToList(tblVisitDetailsTODT);
            return null;
        }

        public TblVisitDetailsTO SelectTblVisitDetailsTO(int idVisit)
        {
            TblVisitDetailsTO tblVisitDetailsTO = _iTblVisitDetailsDAO.SelectTblVisitDetails(idVisit);
            if (tblVisitDetailsTO != null)
                return tblVisitDetailsTO;
            else
                return null;
        }

        public List<TblVisitDetailsTO> ConvertDTToList(DataTable tblVisitDetailsTODT)
        {
            List<TblVisitDetailsTO> tblVisitDetailsTOList = new List<TblVisitDetailsTO>();
            if (tblVisitDetailsTODT != null)
            {

            }
            return tblVisitDetailsTOList;
        }


        // Vaibhav [28-Oct-2017] added to select max visit id 
        public Int32 SelectLastVisitId()
        {
            Int32 result = _iTblVisitDetailsDAO.SelectLastVisitId();
            if (result > 0)
                return result;
            else
                return -1;
        }

        // Vaibhav [28-Oct-2017] added to select all visit details list
        public List<TblVisitDetailsTO> SelectAllVisitDetailsList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblVisitDetailsTO> visitDetailsTOList = _iTblVisitDetailsDAO.SelectAllTblVisitDetails();
                if (visitDetailsTOList != null)
                    return visitDetailsTOList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllVisitDetails");
                return null;
            }
        }

        #endregion

        #region Insertion
        public int InsertTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO)
        {
            return _iTblVisitDetailsDAO.InsertTblVisitDetails(tblVisitDetailsTO);
        }

        public int InsertTblVisitDetails(ref TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitDetailsDAO.InsertTblVisitDetails(ref tblVisitDetailsTO, conn, tran);
        }

        public ResultMessage SaveNewVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {

                // Insert new visit purpose
                if (tblVisitDetailsTO.VisitPurposeTO != null)
                {
                    TblVisitPurposeTO tblVisitPurposeTO = new TblVisitPurposeTO();

                    if (tblVisitDetailsTO.VisitPurposeTO.Value == 0)
                    {
                        tblVisitPurposeTO.VisitPurposeDesc = tblVisitDetailsTO.VisitPurposeTO.Text;
                        tblVisitPurposeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblVisitPurposeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblVisitPurposeTO.VisitTypeId = tblVisitDetailsTO.VisitTypeId;
                        tblVisitPurposeTO.IsActive = 1;

                        result = _iTblVisitPurposeBL.InsertTblVisitPurpose(ref tblVisitPurposeTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblVisitPurpose");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.VisitPurposeId = tblVisitPurposeTO.IdVisitPurpose;
                    }
                    else
                    {
                        tblVisitDetailsTO.VisitPurposeId = tblVisitDetailsTO.VisitPurposeTO.Value;
                    }
                }

                // Insert new payment term
                if (tblVisitDetailsTO.PaymentTermTO != null)
                {
                    TblPaymentTermTO tblPaymentTermTO = new TblPaymentTermTO();

                    if (tblVisitDetailsTO.PaymentTermTO.Value == 0)
                    {
                        tblPaymentTermTO.PaymentTermDisplayName = tblVisitDetailsTO.PaymentTermTO.Text;
                        tblPaymentTermTO.PaymentTermDesc = tblVisitDetailsTO.PaymentTermTO.Text;
                        tblPaymentTermTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblPaymentTermTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblPaymentTermTO.IsActive = 1;

                        result = _iTblPaymentTermBL.InsertTblPaymentTerm(ref tblPaymentTermTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblPaymentTerm");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.PaymentTermId = tblPaymentTermTO.IdPaymentTerm;
                    }
                    else
                    {
                        tblVisitDetailsTO.PaymentTermId = tblVisitDetailsTO.PaymentTermTO.Value;
                    }
                }

                // Insert new site status

                if (tblVisitDetailsTO.SiteStatusTO != null)
                {
                    if (tblVisitDetailsTO.SiteStatusTO.Value == 0)
                    {
                        TblSiteStatusTO tblSiteStatusTO = new TblSiteStatusTO();
                        tblSiteStatusTO.SiteStatusDisplayName = tblVisitDetailsTO.SiteStatusTO.Text;
                        tblSiteStatusTO.SiteStatusDesc = tblVisitDetailsTO.SiteStatusTO.Text;
                        tblSiteStatusTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblSiteStatusTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblSiteStatusTO.IsActive = 1;

                        result = _iTblSiteStatusBL.InsertTblSiteStatus(ref tblSiteStatusTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.SiteStatusId = tblSiteStatusTO.IdSiteStatus;
                    }
                    else
                    {
                        tblVisitDetailsTO.SiteStatusId = tblVisitDetailsTO.SiteStatusTO.Value;
                    }
                }

                // Insert visit site type
                if (tblVisitDetailsTO.SiteTypeTOList != null && tblVisitDetailsTO.SiteTypeTOList.Count > 0)
                {
                    List<TblSiteTypeTO> siteTypeList = tblVisitDetailsTO.SiteTypeTOList.OrderBy(ele => ele.DimSiteTypeId).ToList();

                    foreach (var sitetype in siteTypeList)
                    {
                        if (sitetype.DimSiteTypeId == (int)Constants.VisitSiteTypeE.SITE_CATEGORY)
                        {
                            if (sitetype.IdSiteType == 0)
                            {
                                TblSiteTypeTO tblSiteTypeTO = new TblSiteTypeTO();
                                tblSiteTypeTO.SiteTypeDisplayName = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.SiteTypeDesc = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.ParentSiteTypeId = sitetype.ParentSiteTypeId;
                                tblSiteTypeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                                tblSiteTypeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                                tblSiteTypeTO.IsActive = 1;

                                result = _iTblSiteTypeBL.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error While InsertTblSiteType");
                                    tran.Rollback();
                                    return resultMessage;
                                }
                                else
                                    tblVisitDetailsTO.SiteTypeId = tblSiteTypeTO.IdSiteType;
                            }
                            else
                            {
                                tblVisitDetailsTO.SiteTypeId = sitetype.IdSiteType;
                            }
                        }
                        if (sitetype.DimSiteTypeId == (int)Constants.VisitSiteTypeE.SITE_SUBCATEGORY)
                        {
                            if (sitetype.IdSiteType == 0)
                            {
                                TblSiteTypeTO tblSiteTypeTO = new TblSiteTypeTO();
                                tblSiteTypeTO.SiteTypeDisplayName = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.SiteTypeDesc = sitetype.SiteTypeDisplayName;

                                if (sitetype.ParentSiteTypeId <= 0)
                                    tblSiteTypeTO.ParentSiteTypeId = tblVisitDetailsTO.SiteTypeId;
                                else
                                    tblSiteTypeTO.ParentSiteTypeId = sitetype.ParentSiteTypeId;

                                tblSiteTypeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                                tblSiteTypeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                                tblSiteTypeTO.DimSiteTypeId = sitetype.DimSiteTypeId;
                                tblSiteTypeTO.IsActive = 1;

                                result = _iTblSiteTypeBL.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error While InsertTblSiteType");
                                    tran.Rollback();
                                    return resultMessage;
                                }
                                else
                                    tblVisitDetailsTO.SiteTypeId = tblSiteTypeTO.IdSiteType;
                            }
                            else
                            {
                                tblVisitDetailsTO.SiteTypeId = sitetype.IdSiteType;
                            }
                        }                     
                    }
                }

                // Visit person mapping
                if (tblVisitDetailsTO.VisitPersonDetailsTOList != null && tblVisitDetailsTO.VisitPersonDetailsTOList.Count > 0)
                {
                    foreach (var person in tblVisitDetailsTO.VisitPersonDetailsTOList)
                    {
                        //TblPersonTO personTO = new TblPersonTO();

                        int userId = tblVisitDetailsTO.CreatedBy;

                        if (person.IdPerson == 0)
                        {
                            person.SalutationId = Constants.DefaultCompanyId; //set default 1

                            person.CreatedBy = tblVisitDetailsTO.CreatedBy;
                            person.CreatedOn = tblVisitDetailsTO.CreatedOn;

                            result = _iTblPersonBL.InsertTblPerson(person, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();

                            tblVisitPersonDetailsTO.VisitId = SelectLastVisitId() + 1;
                            tblVisitPersonDetailsTO.PersonId = person.IdPerson;
                            tblVisitPersonDetailsTO.PersonTypeId = person.PersonTypeId;

                            result = _iTblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour(" Error While InsertTblPersonVisitDetails");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // For site owner 
                            if (person.IsSiteOwner == 1)
                            {
                                tblVisitDetailsTO.SiteOwnerId = person.IdPerson;
                            }

                            switch (person.PersonTypeId)
                            {

                                case (int)Constants.VisitPersonE.SITE_ARCHITECT:
                                    tblVisitDetailsTO.SiteArchitectId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_STRUCTURAL_ENGG:
                                    tblVisitDetailsTO.SiteStructuralEnggId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_CONTRACTOR:
                                    if (person.IsSiteOwner == 0)
                                        tblVisitDetailsTO.ContractorId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_STEEL_PURCHASE_AUTHORITY:
                                    tblVisitDetailsTO.PurchaseAuthorityPersonId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_MEETING_WITH:
                                    tblVisitDetailsTO.DealerMeetingWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_VISIT_ALONG_WITH:
                                    tblVisitDetailsTO.DealerVisitAlongWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_VISITED_BY:
                                    tblVisitDetailsTO.InfluencerVisitedBy = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_RECOMMANDEDED_BY:
                                    tblVisitDetailsTO.InfluencerRecommandedBy = person.IdPerson;
                                    break;
                            }
                        }
                        else
                        {
                            switch (person.PersonTypeId)
                            {
                                case (int)Constants.VisitPersonE.SITE_ARCHITECT:
                                    tblVisitDetailsTO.SiteArchitectId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_STRUCTURAL_ENGG:
                                    tblVisitDetailsTO.SiteStructuralEnggId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_CONTRACTOR:
                                    tblVisitDetailsTO.ContractorId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_STEEL_PURCHASE_AUTHORITY:
                                    tblVisitDetailsTO.PurchaseAuthorityPersonId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER:
                                    tblVisitDetailsTO.DealerId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_MEETING_WITH:
                                    tblVisitDetailsTO.DealerMeetingWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_VISIT_ALONG_WITH:
                                    tblVisitDetailsTO.DealerVisitAlongWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_VISITED_BY:
                                    tblVisitDetailsTO.InfluencerVisitedBy = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_RECOMMANDEDED_BY:
                                    tblVisitDetailsTO.InfluencerRecommandedBy = person.IdPerson;
                                    break;
                            }

                            result = _iTblPersonBL.UpdateTblPerson(person, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPerson while updation");
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO visitPersonDetailsTO = new TblVisitPersonDetailsTO();

                            visitPersonDetailsTO.VisitId = SelectLastVisitId() + 1;
                            visitPersonDetailsTO.PersonId = person.IdPerson;
                            visitPersonDetailsTO.PersonTypeId = person.PersonTypeId;

                            int personCount = _iTblVisitPersonDetailsBL.SelectVisitPersonCount(visitPersonDetailsTO.VisitId, visitPersonDetailsTO.PersonId, visitPersonDetailsTO.PersonTypeId);
                            if (personCount <= 0)
                                result = _iTblVisitPersonDetailsBL.InsertTblVisitPersonDetails(visitPersonDetailsTO, conn, tran);
                            else
                                result = _iTblVisitPersonDetailsBL.UpdateTblPersonVisitDetails(visitPersonDetailsTO, conn, tran);

                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPersonVisitDetails while updation");
                                tran.Rollback();
                                return resultMessage;
                            }
                        }
                    }
                }

                result = InsertTblVisitDetails(ref tblVisitDetailsTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblVisitDetails");
                    tran.Rollback();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Result = tblVisitDetailsTO.IdVisit;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewVisitBasicDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        //public int UpdateTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO)
        //{
        //    return _iTblVisitDetailsDAO.UpdateTblVisitDetails(tblVisitDetailsTO);
        //}

        public int UpdateTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitDetailsDAO.UpdateTblVisitDetails(tblVisitDetailsTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] Added to update visit basic details
        public ResultMessage UpdateVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                // Insert new visit purpose
                if (tblVisitDetailsTO.VisitPurposeTO != null)
                {
                    TblVisitPurposeTO tblVisitPurposeTO = new TblVisitPurposeTO();

                    if (tblVisitDetailsTO.VisitPurposeTO.Value == 0)
                    {
                        tblVisitPurposeTO.VisitPurposeDesc = tblVisitDetailsTO.VisitPurposeTO.Text;
                        tblVisitPurposeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblVisitPurposeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblVisitPurposeTO.VisitTypeId = tblVisitDetailsTO.VisitTypeId;
                        tblVisitPurposeTO.IsActive = 1;

                        result = _iTblVisitPurposeBL.InsertTblVisitPurpose(ref tblVisitPurposeTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblVisitPurpose");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.VisitPurposeId = tblVisitPurposeTO.IdVisitPurpose;
                    }
                    else
                    {
                        tblVisitDetailsTO.VisitPurposeId = tblVisitDetailsTO.VisitPurposeTO.Value;
                    }
                }

                // Insert new payment term
                if (tblVisitDetailsTO.PaymentTermTO != null)
                {
                    TblPaymentTermTO tblPaymentTermTO = new TblPaymentTermTO();

                    if (tblVisitDetailsTO.PaymentTermTO.Value == 0)
                    {
                        tblPaymentTermTO.PaymentTermDisplayName = tblVisitDetailsTO.PaymentTermTO.Text;
                        tblPaymentTermTO.PaymentTermDesc = tblVisitDetailsTO.PaymentTermTO.Text;
                        tblPaymentTermTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblPaymentTermTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblPaymentTermTO.IsActive = 1;

                        result = _iTblPaymentTermBL.InsertTblPaymentTerm(ref tblPaymentTermTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblPaymentTerm");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.PaymentTermId = tblPaymentTermTO.IdPaymentTerm;
                    }
                    else
                    {
                        tblVisitDetailsTO.PaymentTermId = tblVisitDetailsTO.PaymentTermTO.Value;
                    }
                }

                // Insert new site status

                if (tblVisitDetailsTO.SiteStatusTO != null)
                {
                    if (tblVisitDetailsTO.SiteStatusTO.Value == 0)
                    {
                        TblSiteStatusTO tblSiteStatusTO = new TblSiteStatusTO();
                        tblSiteStatusTO.SiteStatusDisplayName = tblVisitDetailsTO.SiteStatusTO.Text;
                        tblSiteStatusTO.SiteStatusDesc = tblVisitDetailsTO.SiteStatusTO.Text;
                        tblSiteStatusTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblSiteStatusTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblSiteStatusTO.IsActive = 1;

                        result = _iTblSiteStatusBL.InsertTblSiteStatus(ref tblSiteStatusTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.SiteStatusId = tblSiteStatusTO.IdSiteStatus;
                    }
                    else
                    {
                        tblVisitDetailsTO.SiteStatusId = tblVisitDetailsTO.SiteStatusTO.Value;
                    }
                }

                // Insert visit site type
                if (tblVisitDetailsTO.SiteTypeTOList != null && tblVisitDetailsTO.SiteTypeTOList.Count > 0)
                {
                    List<TblSiteTypeTO> siteTypeList = tblVisitDetailsTO.SiteTypeTOList.OrderBy(ele => ele.DimSiteTypeId).ToList();

                    foreach (var sitetype in siteTypeList)
                    {
                        if (sitetype.DimSiteTypeId == (int)Constants.VisitSiteTypeE.SITE_CATEGORY)
                        {
                            if (sitetype.IdSiteType == 0)
                            {
                                TblSiteTypeTO tblSiteTypeTO = new TblSiteTypeTO();
                                tblSiteTypeTO.SiteTypeDisplayName = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.SiteTypeDesc = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.ParentSiteTypeId = sitetype.ParentSiteTypeId;
                                tblSiteTypeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                                tblSiteTypeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                                tblSiteTypeTO.IsActive = 1;

                                result = _iTblSiteTypeBL.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error While InsertTblSiteType");
                                    tran.Rollback();
                                    return resultMessage;
                                }
                                else
                                    tblVisitDetailsTO.SiteTypeId = tblSiteTypeTO.IdSiteType;
                            }
                            else
                            {
                                tblVisitDetailsTO.SiteTypeId = sitetype.IdSiteType;
                            }
                        }
                        if (sitetype.DimSiteTypeId == (int)Constants.VisitSiteTypeE.SITE_SUBCATEGORY)
                        {
                            if (sitetype.IdSiteType == 0)
                            {
                                TblSiteTypeTO tblSiteTypeTO = new TblSiteTypeTO();
                                tblSiteTypeTO.SiteTypeDisplayName = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.SiteTypeDesc = sitetype.SiteTypeDisplayName;

                                if (sitetype.ParentSiteTypeId <= 0)
                                    tblSiteTypeTO.ParentSiteTypeId = tblVisitDetailsTO.SiteTypeId;
                                else
                                    tblSiteTypeTO.ParentSiteTypeId = sitetype.ParentSiteTypeId;

                                tblSiteTypeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                                tblSiteTypeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                                tblSiteTypeTO.DimSiteTypeId = sitetype.DimSiteTypeId;
                                tblSiteTypeTO.IsActive = 1;

                                result = _iTblSiteTypeBL.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error While InsertTblSiteType");
                                    tran.Rollback();
                                    return resultMessage;
                                }
                                else
                                    tblVisitDetailsTO.SiteTypeId = tblSiteTypeTO.IdSiteType;
                            }
                            else
                            {
                                tblVisitDetailsTO.SiteTypeId = sitetype.IdSiteType;
                            }
                        }
                    }
                }

                // Visit person mapping
                if (tblVisitDetailsTO.VisitPersonDetailsTOList != null && tblVisitDetailsTO.VisitPersonDetailsTOList.Count > 0)
                {
                    foreach (var person in tblVisitDetailsTO.VisitPersonDetailsTOList)
                    {
                        //TblPersonTO personTO = new TblPersonTO();

                        int userId = tblVisitDetailsTO.CreatedBy;

                        if (person.IdPerson == 0)
                        {
                            person.SalutationId = Constants.DefaultSalutationId; //set default 1

                            person.CreatedBy = tblVisitDetailsTO.CreatedBy;
                            person.CreatedOn = tblVisitDetailsTO.CreatedOn;

                            result = _iTblPersonBL.InsertTblPerson(person, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();


                            tblVisitPersonDetailsTO.VisitId = SelectLastVisitId() + 1;
                            tblVisitPersonDetailsTO.PersonId = person.IdPerson;
                            tblVisitPersonDetailsTO.PersonTypeId = person.PersonTypeId;

                            result = _iTblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour(" Error While InsertTblPersonVisitDetails");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // For site owner 
                            if (person.IsSiteOwner == 1)
                            {
                                tblVisitDetailsTO.SiteOwnerId = person.IdPerson;
                            }

                            switch (person.PersonTypeId)
                            {

                                case (int)Constants.VisitPersonE.SITE_ARCHITECT:
                                    tblVisitDetailsTO.SiteArchitectId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_STRUCTURAL_ENGG:
                                    tblVisitDetailsTO.SiteStructuralEnggId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_CONTRACTOR:
                                    if (person.IsSiteOwner == 0)
                                        tblVisitDetailsTO.ContractorId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_STEEL_PURCHASE_AUTHORITY:
                                    tblVisitDetailsTO.PurchaseAuthorityPersonId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_MEETING_WITH:
                                    tblVisitDetailsTO.DealerMeetingWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_VISIT_ALONG_WITH:
                                    tblVisitDetailsTO.DealerVisitAlongWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_VISITED_BY:
                                    tblVisitDetailsTO.InfluencerVisitedBy = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_RECOMMANDEDED_BY:
                                    tblVisitDetailsTO.InfluencerRecommandedBy = person.IdPerson;
                                    break;
                            }
                        }
                        else
                        {
                            switch (person.PersonTypeId)
                            {
                                case (int)Constants.VisitPersonE.SITE_ARCHITECT:
                                    tblVisitDetailsTO.SiteArchitectId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_STRUCTURAL_ENGG:
                                    tblVisitDetailsTO.SiteStructuralEnggId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_CONTRACTOR:
                                    tblVisitDetailsTO.ContractorId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_STEEL_PURCHASE_AUTHORITY:
                                    tblVisitDetailsTO.PurchaseAuthorityPersonId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER:
                                    tblVisitDetailsTO.DealerId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_MEETING_WITH:
                                    tblVisitDetailsTO.DealerMeetingWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_VISIT_ALONG_WITH:
                                    tblVisitDetailsTO.DealerVisitAlongWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_VISITED_BY:
                                    tblVisitDetailsTO.InfluencerVisitedBy = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_RECOMMANDEDED_BY:
                                    tblVisitDetailsTO.InfluencerRecommandedBy = person.IdPerson;
                                    break;
                            }

                            result = _iTblPersonBL.UpdateTblPerson(person, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPerson while updation");
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO visitPersonDetailsTO = new TblVisitPersonDetailsTO();

                            visitPersonDetailsTO.VisitId = tblVisitDetailsTO.IdVisit;
                            visitPersonDetailsTO.PersonId = person.IdPerson;
                            visitPersonDetailsTO.PersonTypeId = person.PersonTypeId;

                            int personCount = _iTblVisitPersonDetailsBL.SelectVisitPersonCount(visitPersonDetailsTO.VisitId, visitPersonDetailsTO.PersonId, visitPersonDetailsTO.PersonTypeId);

                            if(personCount<=0)
                                result = _iTblVisitPersonDetailsBL.InsertTblVisitPersonDetails(visitPersonDetailsTO, conn, tran);
                            else
                            result = _iTblVisitPersonDetailsBL.UpdateTblPersonVisitDetails(visitPersonDetailsTO, conn, tran);

                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPersonVisitDetails while updation");
                                tran.Rollback();
                                return resultMessage;
                            }
                        }
                    }
                }

                result = UpdateTblVisitDetails(tblVisitDetailsTO, conn, tran);
                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblVisitDetails");
                    tran.Rollback();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Result = tblVisitDetailsTO.IdVisit;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Deletion
        public int DeleteTblVisitDetails(int idVisit)
        {
            return _iTblVisitDetailsDAO.DeleteTblVisitDetails(idVisit);
        }

        public int DeleteTblVisitDetails(int idVisit, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitDetailsDAO.DeleteTblVisitDetails(idVisit, conn, tran);
        }

        #endregion

    }
}
