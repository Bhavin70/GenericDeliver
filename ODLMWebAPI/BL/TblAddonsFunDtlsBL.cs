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
    public class TblAddonsFunDtlsBL : ITblAddonsFunDtlsBL
    {
        private readonly ITblAddonsFunDtlsDAO _iTblAddonsFunDtlsDAO;
        private readonly ICommon _iCommon;
        public TblAddonsFunDtlsBL(ICommon iCommon, ITblAddonsFunDtlsDAO iTblAddonsFunDtlsDAO)
        {
            _iTblAddonsFunDtlsDAO = iTblAddonsFunDtlsDAO;
            _iCommon = iCommon;
        }
        #region Selection
        public List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtlsList()
        {
            return _iTblAddonsFunDtlsDAO.SelectAllTblAddonsFunDtls();
        }
        
        public  TblAddonsFunDtlsTO SelectTblAddonsFunDtlsTO(int idAddonsfunDtls)
        {
           return _iTblAddonsFunDtlsDAO.SelectTblAddonsFunDtls(idAddonsfunDtls);
               
        }

        public  List<TblAddonsFunDtlsTO> SelectAddonDetails(int transId, int ModuleId, String TransactionType, String PageElementId, String transIds)
        {
            return _iTblAddonsFunDtlsDAO.SelectAddonDetailsList(transId,ModuleId,TransactionType,PageElementId, transIds);
        }

        #endregion

        #region Insertion
        public ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            tblAddonsFunDtlsTO.CreatedOn = _iCommon.ServerDateTime;  
            tblAddonsFunDtlsTO.IsActive = 1;
            result = _iTblAddonsFunDtlsDAO.InsertTblAddonsFunDtls(tblAddonsFunDtlsTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Inserting Data into TblAddonsFunDtls";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            result= _iTblAddonsFunDtlsDAO.InsertTblAddonsFunDtls(tblAddonsFunDtlsTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion
        
        #region Updation
        public ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            tblAddonsFunDtlsTO.UpdatedOn = _iCommon.ServerDateTime;
            tblAddonsFunDtlsTO.UpdatedBy = tblAddonsFunDtlsTO.CreatedBy;
            result = _iTblAddonsFunDtlsDAO.UpdateTblAddonsFunDtls(tblAddonsFunDtlsTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Updating Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Updating Data";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }


        public  ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
           result=  _iTblAddonsFunDtlsDAO.UpdateTblAddonsFunDtls(tblAddonsFunDtlsTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion

        #region Deletion
        public int DeleteTblAddonsFunDtls(int idAddonsfunDtls)
        {
            return _iTblAddonsFunDtlsDAO.DeleteTblAddonsFunDtls(idAddonsfunDtls);
        }

        public int DeleteTblAddonsFunDtls(int idAddonsfunDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddonsFunDtlsDAO.DeleteTblAddonsFunDtls(idAddonsfunDtls, conn, tran);
        }

        #endregion
        
    }
}
