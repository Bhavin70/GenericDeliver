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

namespace ODLMWebAPI.BL
{
    public class DimAddonsFunBL : IDimAddonsFunBL
    {
        #region Selection
        private readonly IDimAddonsFunDAO _iDimAddonsFunDAO;
        public DimAddonsFunBL(IDimAddonsFunDAO iDimAddonsFunDAO)
        {
            _iDimAddonsFunDAO = iDimAddonsFunDAO;
        }

        public List<DimAddonsFunTO> SelectAllDimAddonsFunList()
        {
            return _iDimAddonsFunDAO.SelectAllDimAddonsFun();
            
        }

        public DimAddonsFunTO SelectDimAddonsFunTO(Int32 idAddonsFun)
        {
            return _iDimAddonsFunDAO.SelectDimAddonsFun(idAddonsFun);
         
        }

        
        #endregion
        
        #region Insertion
        public ResultMessage InsertDimAddonsFun(DimAddonsFunTO dimAddonsFunTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            result= _iDimAddonsFunDAO.InsertDimAddonsFun(dimAddonsFunTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFun");
                resultMessage.DisplayMessage = "Error While Inserting Data into TblAddonsFun";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public ResultMessage InsertDimAddonsFun(DimAddonsFunTO dimAddonsFunTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            _iDimAddonsFunDAO.InsertDimAddonsFun(dimAddonsFunTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFun");
                resultMessage.DisplayMessage = "Error While Inserting Data into TblAddonsFun";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion
        
        #region Updation
        public ResultMessage UpdateDimAddonsFun(DimAddonsFunTO dimAddonsFunTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            result = _iDimAddonsFunDAO.UpdateDimAddonsFun(dimAddonsFunTO);
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

        public ResultMessage UpdateDimAddonsFun(DimAddonsFunTO dimAddonsFunTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            result = _iDimAddonsFunDAO.UpdateDimAddonsFun(dimAddonsFunTO, conn, tran);
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

        #endregion
        
        #region Deletion
        public int DeleteDimAddonsFun(Int32 idAddonsFun)
        {
            return _iDimAddonsFunDAO.DeleteDimAddonsFun(idAddonsFun);
        }

        public int DeleteDimAddonsFun(Int32 idAddonsFun, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimAddonsFunDAO.DeleteDimAddonsFun(idAddonsFun, conn, tran);
        }

        #endregion
        
    }
}
