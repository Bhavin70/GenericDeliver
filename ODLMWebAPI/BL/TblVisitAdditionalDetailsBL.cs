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
    public class TblVisitAdditionalDetailsBL : ITblVisitAdditionalDetailsBL
    {
        private readonly ITblVisitAdditionalDetailsDAO _iTblVisitAdditionalDetailsDAO;
        private readonly ITblVisitPersonDetailsBL _iTblVisitPersonDetailsBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ITblVisitDetailsBL _iTblVisitDetailsBL;
        public TblVisitAdditionalDetailsBL(ITblVisitDetailsBL iTblVisitDetailsBL,ITblPersonBL iTblPersonBL,ITblVisitPersonDetailsBL iTblVisitPersonDetailsBL,ITblVisitAdditionalDetailsDAO iTblVisitAdditionalDetailsDAO)
        {
            _iTblVisitAdditionalDetailsDAO = iTblVisitAdditionalDetailsDAO;
            _iTblVisitPersonDetailsBL = iTblVisitPersonDetailsBL;
            _iTblPersonBL = iTblPersonBL;
            _iTblVisitDetailsBL = iTblVisitDetailsBL;
        }
        #region Selection
        public DataTable SelectAllTblVisitAdditionalDetails()
        {
            return _iTblVisitAdditionalDetailsDAO.SelectAllTblVisitAdditionalDetails();
        }

        public List<TblVisitAdditionalDetailsTO> SelectAllTblVisitAdditionalDetailsList()
        {
            DataTable tblVisitAdditionalDetailsTODT = _iTblVisitAdditionalDetailsDAO.SelectAllTblVisitAdditionalDetails();
            return ConvertDTToList(tblVisitAdditionalDetailsTODT);
        }

        public TblVisitAdditionalDetailsTO SelectTblVisitAdditionalDetailsTO(Int32 idVisitDetails)
        {
            DataTable tblVisitAdditionalDetailsTODT = _iTblVisitAdditionalDetailsDAO.SelectTblVisitAdditionalDetails(idVisitDetails);
            List<TblVisitAdditionalDetailsTO> tblVisitAdditionalDetailsTOList = ConvertDTToList(tblVisitAdditionalDetailsTODT);
            if (tblVisitAdditionalDetailsTOList != null && tblVisitAdditionalDetailsTOList.Count == 1)
                return tblVisitAdditionalDetailsTOList[0];
            else
                return null;
        }

        public List<TblVisitAdditionalDetailsTO> ConvertDTToList(DataTable tblVisitAdditionalDetailsTODT)
        {
            List<TblVisitAdditionalDetailsTO> tblVisitAdditionalDetailsTOList = new List<TblVisitAdditionalDetailsTO>();
            if (tblVisitAdditionalDetailsTODT != null)
            {
            }
            return tblVisitAdditionalDetailsTOList;
        }

        public TblVisitAdditionalDetailsTO SelectVisitAdditionalDetailsTO(Int32 visitId)
        {
            TblVisitAdditionalDetailsTO VisitAdditionalDetailsTO = _iTblVisitAdditionalDetailsDAO.SelectVisitAdditionalDetails(visitId);
            
            if (VisitAdditionalDetailsTO != null )
                return VisitAdditionalDetailsTO;
            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO)
        {
            return _iTblVisitAdditionalDetailsDAO.InsertTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO);
        }

        public int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitAdditionalDetailsDAO.InsertTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);
        }

        public int InsertTblVisitAdditionalDetailsAbountCompany(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitAdditionalDetailsDAO.InsertTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);
        }

        // Vaibhav [3-Oct-2017] added to insert new visit additional information
        public ResultMessage SaveVisitAdditionalInfo(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
            
                // Visit person mapping
                if (tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList != null && tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList.Count > 0)
                {
                    foreach (var person in tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList)
                    {
                        //TblPersonTO personTO = new TblPersonTO();

                        if (person.IdPerson == 0)
                        {
                            person.SalutationId = Constants.DefaultSalutationId; //set default 1

                            person.CreatedBy = tblVisitAdditionalDetailsTO.CreatedBy;
                            person.CreatedOn = tblVisitAdditionalDetailsTO.CreatedOn;

                            result = _iTblPersonBL.InsertTblPerson(person, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // Visit person insertion
                            TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();


                            tblVisitPersonDetailsTO.VisitId = _iTblVisitDetailsBL.SelectLastVisitId() + 1;
                            tblVisitPersonDetailsTO.PersonId = person.IdPerson;
                            tblVisitPersonDetailsTO.PersonTypeId = person.PersonTypeId;

                            result = _iTblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour(" Error While InsertTblVisitPersonDetails");
                                tran.Rollback();
                                return resultMessage;
                            }

                            switch (person.PersonTypeId)
                            {
                                case (int)Constants.VisitPersonE.SITE_COMPLAINT_REFRRED_BY:
                                    tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.COMMUNICATION_WITH_AT_SITE:
                                    tblVisitAdditionalDetailsTO.CommunicationPersonId = person.IdPerson;
                                    break;
                            }
                        }
                        else
                        {
                            switch (person.PersonTypeId)
                            {

                                case (int)Constants.VisitPersonE.SITE_COMPLAINT_REFRRED_BY:
                                    tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.COMMUNICATION_WITH_AT_SITE:
                                    tblVisitAdditionalDetailsTO.CommunicationPersonId = person.IdPerson;
                                    break;
                            }

                            result = _iTblPersonBL.UpdateTblPerson(person, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPerson while updation");
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO visitPersonDetailsTO = new TblVisitPersonDetailsTO();

                            visitPersonDetailsTO.VisitId = tblVisitAdditionalDetailsTO.VisitId;
                            visitPersonDetailsTO.PersonId = person.IdPerson;
                            visitPersonDetailsTO.PersonTypeId = person.PersonTypeId;

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

                if (tblVisitAdditionalDetailsTO.DesignationTO != null)
                {
                    tblVisitAdditionalDetailsTO.DesignationId = tblVisitAdditionalDetailsTO.DesignationTO.Value;
                }

                result = InsertTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblVisitAdditionalDetails");
                    tran.Rollback();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveVisitAdditionalInfo");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO)
        {
            return _iTblVisitAdditionalDetailsDAO.UpdateTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO);
        }

        public int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitAdditionalDetailsDAO.UpdateTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);
        }


        // Vaibhav [1-Nov-2017] Added to update visit additional information
        public ResultMessage UpdateVisitAdditionalInfo(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                // Visit person mapping
                if (tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList != null && tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList.Count > 0)
                {
                    foreach (var person in tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList)
                    {
                        //TblPersonTO personTO = new TblPersonTO();

                        if (person.IdPerson == 0)
                        {
                            person.SalutationId = Constants.DefaultSalutationId; //set default 1

                            person.CreatedBy = tblVisitAdditionalDetailsTO.CreatedBy;
                            person.CreatedOn = tblVisitAdditionalDetailsTO.CreatedOn;

                            result = _iTblPersonBL.InsertTblPerson(person, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // Visit person insertion
                            TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();


                            tblVisitPersonDetailsTO.VisitId = _iTblVisitDetailsBL.SelectLastVisitId() + 1;
                            tblVisitPersonDetailsTO.PersonId = person.IdPerson;
                            tblVisitPersonDetailsTO.PersonTypeId = person.PersonTypeId;

                            result = _iTblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour(" Error While InsertTblPersonVisitDetails");
                                tran.Rollback();
                                return resultMessage;
                            }


                            switch (person.PersonTypeId)
                            {
                                case (int)Constants.VisitPersonE.SITE_COMPLAINT_REFRRED_BY:
                                    tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.COMMUNICATION_WITH_AT_SITE:
                                    tblVisitAdditionalDetailsTO.CommunicationPersonId = person.IdPerson;
                                    break;
                            }
                        }
                        else
                        {
                            switch (person.PersonTypeId)
                            {

                                case (int)Constants.VisitPersonE.SITE_COMPLAINT_REFRRED_BY:
                                    tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.COMMUNICATION_WITH_AT_SITE:
                                    tblVisitAdditionalDetailsTO.CommunicationPersonId = person.IdPerson;
                                    break;
                            }

                            result = _iTblPersonBL.UpdateTblPerson(person, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPerson while updation");
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO visitPersonDetailsTO = new TblVisitPersonDetailsTO();

                            visitPersonDetailsTO.VisitId = tblVisitAdditionalDetailsTO.VisitId;
                            visitPersonDetailsTO.PersonId = person.IdPerson;
                            visitPersonDetailsTO.PersonTypeId = person.PersonTypeId;

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

                if (tblVisitAdditionalDetailsTO.DesignationTO != null)
                {
                    tblVisitAdditionalDetailsTO.DesignationId = tblVisitAdditionalDetailsTO.DesignationTO.Value;
                }

                result = UpdateTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblVisitAdditionalDetails");
                    tran.Rollback();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitAdditionalInfo");
                tran.Rollback();
                return resultMessage;
            }
        }
        #endregion

        #region Deletion
        public int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails)
        {
            return _iTblVisitAdditionalDetailsDAO.DeleteTblVisitAdditionalDetails(idVisitDetails);
        }

        public int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitAdditionalDetailsDAO.DeleteTblVisitAdditionalDetails(idVisitDetails, conn, tran);
        }

        #endregion

    }
}
