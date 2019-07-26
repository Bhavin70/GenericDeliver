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
    public class TblVisitProjectDetailsBL : ITblVisitProjectDetailsBL
    {
        private readonly ITblVisitProjectDetailsDAO _iTblVisitProjectDetailsDAO;
        private readonly ITblPersonDAO _iTblPersonDAO;
        private readonly ICommon _iCommon;
        public TblVisitProjectDetailsBL(ICommon iCommon, ITblPersonDAO iTblPersonDAO, ITblVisitProjectDetailsDAO iTblVisitProjectDetailsDAO)
        {
            _iTblVisitProjectDetailsDAO = iTblVisitProjectDetailsDAO;
            _iTblPersonDAO = iTblPersonDAO;
            _iCommon = iCommon;

        }
        #region Selection
        public List<TblVisitProjectDetailsTO> SelectProjectDetailsList(Int32 visitId)
        {
            return _iTblVisitProjectDetailsDAO.SelectAllTblVisitProjectDetails(visitId);
        }

        public List<TblVisitProjectDetailsTO> SelectAllTblVisitProjectDetailsList()
        {
            List<TblVisitProjectDetailsTO> tblProjectDetailsTOList = _iTblVisitProjectDetailsDAO.SelectAllTblVisitProjectDetails(0);
            return tblProjectDetailsTOList;
        }

        public TblVisitProjectDetailsTO SelectTblProjectDetailsTO(Int32 idProject)
        {
            DataTable tblProjectDetailsTODT = _iTblVisitProjectDetailsDAO.SelectTblVisitProjectDetails(idProject);
            List<TblVisitProjectDetailsTO> tblProjectDetailsTOList = ConvertDTToList(tblProjectDetailsTODT);
            if (tblProjectDetailsTOList != null && tblProjectDetailsTOList.Count == 1)
                return tblProjectDetailsTOList[0];
            else
                return null;
        }

        public List<TblVisitProjectDetailsTO> ConvertDTToList(DataTable tblProjectDetailsTODT)
        {
            List<TblVisitProjectDetailsTO> tblProjectDetailsTOList = new List<TblVisitProjectDetailsTO>();
            if (tblProjectDetailsTODT != null)
            {

            }
            return tblProjectDetailsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO)
        {
            return _iTblVisitProjectDetailsDAO.InsertTblVisitProjectDetails(tblProjectDetailsTO);
        }

        public int InsertTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitProjectDetailsDAO.InsertTblVisitProjectDetails(tblProjectDetailsTO, conn, tran);
        }

        // Vaibhav [25-Oct-2017] added to insert visit project information
        public ResultMessage SaveVisitProjectDetails(List<TblVisitProjectDetailsTO> tblVisitProjectDetailsTOList, Int32 createdBy,Int32 visitId,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                foreach (var visitProjectDetailsTO in tblVisitProjectDetailsTOList)
                {
                    // Insertion
                    if (visitProjectDetailsTO.IdProject <= 0)
                    {
                        if (visitProjectDetailsTO.VisitId <= 0)
                            visitProjectDetailsTO.VisitId = visitId;

                        visitProjectDetailsTO.CreatedBy = createdBy;
                        visitProjectDetailsTO.CreatedOn = _iCommon.ServerDateTime;

                        if (visitProjectDetailsTO.ContactPersonId == 0)
                        {
                            TblPersonTO personTO = new TblPersonTO();

                            personTO.SalutationId = Constants.DefaultSalutationId; //set default 1

                            
                            String contactPersonName= visitProjectDetailsTO.ContactPersonName;

                            if (contactPersonName.Contains(' '))
                            {
                                String[] contactPersonNames = contactPersonName.Split(' ');

                                if (contactPersonNames.Length >= 0)
                                    personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                                if (contactPersonNames.Length > 1)
                                    personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                                if (contactPersonNames.Length > 2)
                                    personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                            }
                            else
                            {
                                personTO.FirstName = contactPersonName;
                                personTO.LastName = "-";
                            }
                            personTO.MobileNo = visitProjectDetailsTO.ContactNo;
                            personTO.PrimaryEmail = visitProjectDetailsTO.EmailId;
                            personTO.CreatedBy = visitProjectDetailsTO.CreatedBy;
                            personTO.CreatedOn = visitProjectDetailsTO.CreatedOn;

                            result = _iTblPersonDAO.InsertTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }

                        result = InsertTblProjectDetails(visitProjectDetailsTO, conn, tran);

                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblProjectDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Updation
                    else
                    { 
                        if (visitProjectDetailsTO.VisitId <= 0)
                            visitProjectDetailsTO.VisitId = visitId;

                        visitProjectDetailsTO.UpdatedBy = createdBy;
                        visitProjectDetailsTO.UpdatedOn = _iCommon.ServerDateTime;

                        TblPersonTO personTO = new TblPersonTO();

                        personTO.SalutationId = Constants.DefaultSalutationId; //set default 1

                        String contactPersonName = visitProjectDetailsTO.ContactPersonName;

                        if (contactPersonName.Contains(' '))
                        {
                            String[] contactPersonNames = visitProjectDetailsTO.ContactPersonName.Split(' ');

                            if (contactPersonNames.Length >= 0)
                                personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                            if (contactPersonNames.Length > 1)
                                personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                            if (contactPersonNames.Length > 2)
                                personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                        }
                        else
                        {
                            personTO.FirstName = contactPersonName;
                            personTO.LastName = "-";
                        }

                        personTO.IdPerson = visitProjectDetailsTO.ContactPersonId;
                        personTO.MobileNo = visitProjectDetailsTO.ContactNo;
                        personTO.PrimaryEmail = visitProjectDetailsTO.EmailId;
                        personTO.CreatedBy = createdBy;
                        personTO.CreatedOn = _iCommon.ServerDateTime;

                        if (visitProjectDetailsTO.ContactPersonId == 0)
                        {

                            result = _iTblPersonDAO.InsertTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }
                        else
                        {
                            result = _iTblPersonDAO.UpdateTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While UpdateTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }

                        result = UpdateTblProjectDetails(visitProjectDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateTblProjectDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveVisitProjectDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public int UpdateTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO)
        {
            return _iTblVisitProjectDetailsDAO.UpdateTblVisitProjectDetails(tblProjectDetailsTO);
        }

        public int UpdateTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitProjectDetailsDAO.UpdateTblVisitProjectDetails(tblProjectDetailsTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] added to update visit project information
        public ResultMessage UpdateVisitProjectDetails(List<TblVisitProjectDetailsTO> tblVisitProjectDetailsTOList, Int32 updatedBy, Int32 visitId,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                foreach (var visitProjectDetailsTO in tblVisitProjectDetailsTOList)
                {
                    if (visitProjectDetailsTO.IdProject > 0)
                    {
                        if (visitProjectDetailsTO.VisitId <= 0)
                            visitProjectDetailsTO.VisitId = visitId;

                        visitProjectDetailsTO.UpdatedBy = updatedBy;
                        visitProjectDetailsTO.UpdatedOn = _iCommon.ServerDateTime;

                        if (visitProjectDetailsTO.ContactPersonId == 0)
                        {
                            TblPersonTO personTO = new TblPersonTO();

                            personTO.SalutationId = Constants.DefaultSalutationId; //set default 1

                            String contactPersonName= visitProjectDetailsTO.ContactPersonName;

                            if (contactPersonName.Contains(' '))
                            {
                                String[] contactPersonNames = visitProjectDetailsTO.ContactPersonName.Split(' ');

                                if (contactPersonNames.Length >= 0)
                                    personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                                if (contactPersonNames.Length > 1)
                                    personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                                if (contactPersonNames.Length > 2)
                                    personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                            }
                            else
                            {
                                personTO.FirstName = contactPersonName;
                                personTO.LastName = "-";
                            }

                            personTO.MobileNo = visitProjectDetailsTO.ContactNo;
                            personTO.PrimaryEmail = visitProjectDetailsTO.EmailId;
                            personTO.CreatedBy = updatedBy;
                            personTO.CreatedOn = _iCommon.ServerDateTime;

                            result = _iTblPersonDAO.InsertTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }

                        result = UpdateTblProjectDetails(visitProjectDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateTblProjectDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Insert project details while updation
                    if (visitProjectDetailsTO.IdProject <= 0)
                    {
                        if (visitProjectDetailsTO.VisitId <= 0)
                            visitProjectDetailsTO.VisitId = visitId;

                        visitProjectDetailsTO.CreatedBy = updatedBy;
                        visitProjectDetailsTO.CreatedOn = _iCommon.ServerDateTime;

                        if (visitProjectDetailsTO.ContactPersonId == 0)
                        {
                            TblPersonTO personTO = new TblPersonTO();

                            personTO.SalutationId = Constants.DefaultSalutationId; //set default 1

                            String contactPersonName = visitProjectDetailsTO.ContactPersonName;

                            if (contactPersonName.Contains(' '))
                            {
                                String[] contactPersonNames = visitProjectDetailsTO.ContactPersonName.Split(' ');

                                if (contactPersonNames.Length >= 0)
                                    personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                                if (contactPersonNames.Length > 1)
                                    personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                                if (contactPersonNames.Length > 2)
                                    personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                            }
                            else
                            {
                                personTO.FirstName = contactPersonName;
                                personTO.LastName = "-";
                            }

                            personTO.MobileNo = visitProjectDetailsTO.ContactNo;
                            personTO.PrimaryEmail = visitProjectDetailsTO.EmailId;
                            personTO.CreatedBy = updatedBy;
                            personTO.CreatedOn = _iCommon.ServerDateTime;

                            result = _iTblPersonDAO.InsertTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }

                        result = InsertTblProjectDetails(visitProjectDetailsTO, conn, tran);

                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in InsertTblProjectDetails while updation");
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
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitProjectDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Deletion
        public int DeleteTblProjectDetails(Int32 idProject)
        {
            return _iTblVisitProjectDetailsDAO.DeleteTblVisitProjectDetails(idProject);
        }

        public int DeleteTblProjectDetails(Int32 idProject, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitProjectDetailsDAO.DeleteTblVisitProjectDetails(idProject, conn, tran);
        }

        #endregion

    }
}
