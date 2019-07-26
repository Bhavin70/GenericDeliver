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
    public class TblSupervisorBL : ITblSupervisorBL
    {
        private readonly ITblSupervisorDAO _iTblSupervisorDAO;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly IConnectionString _iConnectionString;
        public TblSupervisorBL(IConnectionString iConnectionString, ITblSupervisorDAO iTblSupervisorDAO, ITblPersonBL iTblPersonBL)
        {
            _iTblSupervisorDAO = iTblSupervisorDAO;
            _iTblPersonBL = iTblPersonBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblSupervisorTO> SelectAllTblSupervisorList()
        {
           return  _iTblSupervisorDAO.SelectAllTblSupervisor();
        }

        public TblSupervisorTO SelectTblSupervisorTO(Int32 idSupervisor)
        {
            return _iTblSupervisorDAO.SelectTblSupervisor(idSupervisor);
        }



        #endregion

        #region Insertion

        public ResultMessage SaveNewSuperwisor(TblSupervisorTO supervisorTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                supervisorTO.PersonTO.CreatedBy = supervisorTO.CreatedBy;
                supervisorTO.PersonTO.CreatedOn = supervisorTO.CreatedOn;
                if (string.IsNullOrEmpty(supervisorTO.PersonTO.Comments))
                    supervisorTO.PersonTO.Comments = "Superwisor";

                if (supervisorTO.PersonTO.DobDay > 0 && supervisorTO.PersonTO.DobMonth > 0 && supervisorTO.PersonTO.DobYear > 0)
                {
                    supervisorTO.PersonTO.DateOfBirth = new DateTime(supervisorTO.PersonTO.DobYear, supervisorTO.PersonTO.DobMonth, supervisorTO.PersonTO.DobDay);
                }
                else
                {
                    supervisorTO.PersonTO.DateOfBirth = DateTime.MinValue;
                }

                int result = _iTblPersonBL.InsertTblPerson(supervisorTO.PersonTO, conn, tran);
                if(result!=1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In Method SaveNewSuperwisor";
                    resultMessage.DisplayMessage = "Error... Record could not be saved";
                    return resultMessage;
                }

                supervisorTO.PersonId = supervisorTO.PersonTO.IdPerson;
                result = InsertTblSupervisor(supervisorTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In Method InsertTblSupervisor";
                    resultMessage.DisplayMessage = "Error... Record could not be saved";
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Success... Record saved";
                resultMessage.DisplayMessage = "Success... Record saved";
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error In Method SaveNewSuperwisor";
                resultMessage.DisplayMessage = "Exception Error... Record could not be saved";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO)
        {
            return _iTblSupervisorDAO.InsertTblSupervisor(tblSupervisorTO);
        }

        public int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSupervisorDAO.InsertTblSupervisor(tblSupervisorTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO)
        {
            return _iTblSupervisorDAO.UpdateTblSupervisor(tblSupervisorTO);
        }

        public int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSupervisorDAO.UpdateTblSupervisor(tblSupervisorTO, conn, tran);
        }

        public ResultMessage UpdateSuperwisor(TblSupervisorTO supervisorTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (supervisorTO.PersonTO.DobDay > 0 && supervisorTO.PersonTO.DobMonth > 0 && supervisorTO.PersonTO.DobYear > 0)
                {
                    supervisorTO.PersonTO.DateOfBirth = new DateTime(supervisorTO.PersonTO.DobYear, supervisorTO.PersonTO.DobMonth, supervisorTO.PersonTO.DobDay);
                }
                else
                {
                    supervisorTO.PersonTO.DateOfBirth = DateTime.MinValue;
                }

                int result = _iTblPersonBL.UpdateTblPerson(supervisorTO.PersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In Method SaveNewSuperwisor";
                    resultMessage.DisplayMessage = "Error... Record could not be saved";
                    return resultMessage;
                }

                supervisorTO.SupervisorName = supervisorTO.PersonTO.FirstName + " " + supervisorTO.PersonTO.LastName;
                result = UpdateTblSupervisor(supervisorTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In Method UpdateTblSupervisor";
                    resultMessage.DisplayMessage = "Error... Record could not be saved";
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Success... Record saved";
                resultMessage.DisplayMessage = "Success... Record saved";
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error In Method UpdateSuperwisor";
                resultMessage.DisplayMessage = "Exception Error... Record could not be saved";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Deletion
        public int DeleteTblSupervisor(Int32 idSupervisor)
        {
            return _iTblSupervisorDAO.DeleteTblSupervisor(idSupervisor);
        }

        public int DeleteTblSupervisor(Int32 idSupervisor, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSupervisorDAO.DeleteTblSupervisor(idSupervisor, conn, tran);
        }

       

        #endregion

    }
}
