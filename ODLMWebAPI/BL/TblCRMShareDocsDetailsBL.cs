using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblCRMShareDocsDetailsBL : ITblCRMShareDocsDetailsBL
    {
        private readonly ITblCRMShareDocsDetailsDAO _iTblCRMShareDocsDetailsDAO;
        private readonly ISendMailBL _iSendMailBL;
        private readonly ICommon _iCommon;
        public TblCRMShareDocsDetailsBL(ICommon iCommon, ISendMailBL iSendMailBL, ITblCRMShareDocsDetailsDAO iTblCRMShareDocsDetailsDAO)
        {
            _iTblCRMShareDocsDetailsDAO = iTblCRMShareDocsDetailsDAO;
            _iSendMailBL = iSendMailBL;
            _iCommon = iCommon;
        }
        #region Selection
        public List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetails()
        {
            return _iTblCRMShareDocsDetailsDAO.SelectAllTblCRMShareDocsDetails();
        }

        public List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetailsList()
        {
            return _iTblCRMShareDocsDetailsDAO.SelectAllTblCRMShareDocsDetails();
            //ConvertDTToList(tblCRMShareDocsDetailsTODT);
        }

        public TblCRMShareDocsDetailsTO SelectTblCRMShareDocsDetailsTO(Int32 idShareDoc)
        {
            TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTODT = _iTblCRMShareDocsDetailsDAO.SelectTblCRMShareDocsDetails(idShareDoc);
            if (tblCRMShareDocsDetailsTODT != null)
                return tblCRMShareDocsDetailsTODT;
            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            return _iTblCRMShareDocsDetailsDAO.InsertTblCRMShareDocsDetails(tblCRMShareDocsDetailsTO);
        }

        public int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblCRMShareDocsDetailsDAO.InsertTblCRMShareDocsDetails(tblCRMShareDocsDetailsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            return _iTblCRMShareDocsDetailsDAO.UpdateTblCRMShareDocsDetails(tblCRMShareDocsDetailsTO);
        }

        public int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblCRMShareDocsDetailsDAO.UpdateTblCRMShareDocsDetails(tblCRMShareDocsDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblCRMShareDocsDetails(Int32 idShareDoc)
        {
            return _iTblCRMShareDocsDetailsDAO.DeleteTblCRMShareDocsDetails(idShareDoc);
        }

        public int DeleteTblCRMShareDocsDetails(Int32 idShareDoc, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblCRMShareDocsDetailsDAO.DeleteTblCRMShareDocsDetails(idShareDoc, conn, tran);
        }

        #endregion

        public ResultMessage ShareDetials(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                resultMessage = SendEmailToSelectedPersons(tblCRMShareDocsDetailsTO);
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ShareDetials");
                resultMessage.DisplayMessage = ex.Message;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
        }

        //Sudhir[10-MAY-2018] Added for Sending Mail. And Saving Record to ShareDetails
        public ResultMessage SendEmailToSelectedPersons(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (tblCRMShareDocsDetailsTO != null)
                {
                    List<PersonShareData> personShareDataList = tblCRMShareDocsDetailsTO.PersonShareData;
                    if (personShareDataList != null && personShareDataList.Count > 0)
                    {
                        foreach (PersonShareData personShareData in personShareDataList)
                        {
                            if (personShareData.EmailId != null)
                            {
                                int result = 0;
                                SendMail sendMail = new SendMail();
                                sendMail.To = personShareData.EmailId;
                                sendMail.BodyContent = personShareData.DocBase64;
                                sendMail.Message = personShareData.DocBase64;
                                string[] mailSubject = personShareData.FileName.Split('.'); ;
                                if (mailSubject.Length > 0)
                                {
                                    sendMail.Subject = mailSubject[0];
                                }
                                else
                                {
                                    sendMail.Subject = "Regards New Visit Details ";
                                }
                                if (personShareData.FileName == null)
                                {
                                    personShareData.FileName = "VisitDetails.pdf";
                                }
                                resultMessage = _iSendMailBL.SendEmail(sendMail, personShareData.FileName);
                                if (resultMessage.MessageType != ResultMessageE.Error)
                                {
                                    //Insert Record to tblCRMShareDoc Details.
                                    TblCRMShareDocsDetailsTO tblCRMShareDocsDetails = tblCRMShareDocsDetailsTO;
                                    tblCRMShareDocsDetails.VisitId = tblCRMShareDocsDetailsTO.VisitId;
                                    tblCRMShareDocsDetails.DocumentId = tblCRMShareDocsDetailsTO.DocumentId;
                                    if (personShareData.RoleId == -1)
                                    {
                                        tblCRMShareDocsDetailsTO.EntityTypeId = 2;
                                        tblCRMShareDocsDetailsTO.UserId = personShareData.PersonId;
                                    }
                                    else
                                    {
                                        tblCRMShareDocsDetailsTO.EntityTypeId = 1;
                                        tblCRMShareDocsDetailsTO.UserId = personShareData.PersonId;
                                    }

                                    if (personShareData.RoleId == -1)
                                    {
                                        tblCRMShareDocsDetails.RoleId = 0;
                                        personShareData.RoleId = 0;
                                    }
                                    tblCRMShareDocsDetails.RoleId = personShareData.RoleId;
                                    tblCRMShareDocsDetails.CreatedOn = _iCommon.ServerDateTime;
                                    tblCRMShareDocsDetails.CreatedBy = tblCRMShareDocsDetailsTO.CreatedBy;
                                    result = InsertTblCRMShareDocsDetails(tblCRMShareDocsDetails);
                                    if (result != 1)
                                    {
                                        resultMessage.DefaultBehaviour("Error While Inserting Data into TblCRMShareDocsDetails");
                                        resultMessage.DisplayMessage = "Error While Sending Email";
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        return resultMessage;
                                    }
                                }
                                else
                                {
                                    return resultMessage;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        //personShareDataList    return -1;
                        resultMessage.DefaultBehaviour("personShareDataList found Null");
                        resultMessage.DisplayMessage = "Error While Sending Email";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }
                else
                {
                    //tblCRMShareDocsDetailsTO 
                    resultMessage.DefaultBehaviour("tblCRMShareDocsDetailsTO found Null");
                    resultMessage.DisplayMessage = "Error While Sending Email";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }
                //return 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendEmailToSelectedPersons");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
        }
    }
}