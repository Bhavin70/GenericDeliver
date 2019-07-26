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
    public class TblSiteRequirementsBL : ITblSiteRequirementsBL
    {
        private readonly ITblSiteRequirementsDAO _iTblSiteRequirementsDAO;
        public TblSiteRequirementsBL(ITblSiteRequirementsDAO iTblSiteRequirementsDAO)
        {
            _iTblSiteRequirementsDAO = iTblSiteRequirementsDAO;
        }
        #region Selection
        public DataTable SelectAllTblSiteRequirements()
        {
            return _iTblSiteRequirementsDAO.SelectAllTblSiteRequirements();
        }

        public List<TblSiteRequirementsTO> SelectAllTblSiteRequirementsList()
        {
            DataTable tblSiteRequirementsTODT = _iTblSiteRequirementsDAO.SelectAllTblSiteRequirements();
            return ConvertDTToList(tblSiteRequirementsTODT);
        }

        public TblSiteRequirementsTO SelectTblSiteRequirementsTO(Int32 idSiteRequirement)
        {
            DataTable tblSiteRequirementsTODT = _iTblSiteRequirementsDAO.SelectTblSiteRequirements(idSiteRequirement);
            List<TblSiteRequirementsTO> tblSiteRequirementsTOList = ConvertDTToList(tblSiteRequirementsTODT);
            if (tblSiteRequirementsTOList != null && tblSiteRequirementsTOList.Count == 1)
                return tblSiteRequirementsTOList[0];
            else
                return null;
        }

        public List<TblSiteRequirementsTO> ConvertDTToList(DataTable tblSiteRequirementsTODT)
        {
            List<TblSiteRequirementsTO> tblSiteRequirementsTOList = new List<TblSiteRequirementsTO>();
            if (tblSiteRequirementsTODT != null)
            {

            }
            return tblSiteRequirementsTOList;
        }

        public TblSiteRequirementsTO SelectSiteRequirementsTO(Int32 visitId)
        {
            TblSiteRequirementsTO tblSiteRequirementsTO = _iTblSiteRequirementsDAO.SelectSiteRequirements(visitId);
            if (tblSiteRequirementsTO != null)
                return tblSiteRequirementsTO;
            else
                return null;
        }
        #endregion

        #region Insertion
        public int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO)
        {
            return _iTblSiteRequirementsDAO.InsertTblSiteRequirements(tblSiteRequirementsTO);
        }

        public int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSiteRequirementsDAO.InsertTblSiteRequirements(tblSiteRequirementsTO, conn, tran);
        }

        // Vaibhav [3-Oct-2017] added to insert new site requirements
        public ResultMessage SaveSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                result = InsertTblSiteRequirements(tblSiteRequirementsTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblSiteRequirements");
                    tran.Rollback();
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveSiteRequirements");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO)
        {
            return _iTblSiteRequirementsDAO.UpdateTblSiteRequirements(tblSiteRequirementsTO);
        }

        public int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSiteRequirementsDAO.UpdateTblSiteRequirements(tblSiteRequirementsTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] Added to update new site requirements
        public ResultMessage UpdateSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                
                result = UpdateTblSiteRequirements(tblSiteRequirementsTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblSiteRequirements");
                    tran.Rollback();
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateSiteRequirements");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Deletion
        public int DeleteTblSiteRequirements(Int32 idSiteRequirement)
        {
            return _iTblSiteRequirementsDAO.DeleteTblSiteRequirements(idSiteRequirement);
        }

        public int DeleteTblSiteRequirements(Int32 idSiteRequirement, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSiteRequirementsDAO.DeleteTblSiteRequirements(idSiteRequirement, conn, tran);
        }

        #endregion

    }
}
