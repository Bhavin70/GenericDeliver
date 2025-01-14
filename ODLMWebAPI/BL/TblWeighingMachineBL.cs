using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblWeighingMachineBL : ITblWeighingMachineBL
    {
        private readonly ITblWeighingMachineDAO _iTblWeighingMachineDAO;
        public TblWeighingMachineBL(ITblWeighingMachineDAO iTblWeighingMachineDAO)
        {
            _iTblWeighingMachineDAO = iTblWeighingMachineDAO;
        }
        #region Selection
        //Aniket [30-7-2019] added for IOT
        public List<TblWeighingMachineTO> SelectAllTblWeighingMachineOfWeighingList(int loadingId)
        {
            return _iTblWeighingMachineDAO.SelectAllTblWeighingMachineOfWeighingList(loadingId);
        }
        public List<TblWeighingMachineTO> SelectAllTblWeighingMachineList()
        {
            return _iTblWeighingMachineDAO.SelectAllTblWeighingMachine();
        }
        public List<DropDownTO> SelectTblWeighingMachineDropDownList()
        {
            return _iTblWeighingMachineDAO.SelectTblWeighingMachineDropDownList();
        }
        public TblWeighingMachineTO SelectTblWeighingMachineTO(Int32 idWeighingMachine)
        {
            return _iTblWeighingMachineDAO.SelectTblWeighingMachine(idWeighingMachine);
        }

        #endregion
        
        #region Insertion
        public int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO)
        {
            return _iTblWeighingMachineDAO.InsertTblWeighingMachine(tblWeighingMachineTO);
        }

        public int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingMachineDAO.InsertTblWeighingMachine(tblWeighingMachineTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO)
        {
            return _iTblWeighingMachineDAO.UpdateTblWeighingMachine(tblWeighingMachineTO);
        }

        public int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingMachineDAO.UpdateTblWeighingMachine(tblWeighingMachineTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblWeighingMachine(Int32 idWeighingMachine)
        {
            return _iTblWeighingMachineDAO.DeleteTblWeighingMachine(idWeighingMachine);
        }

        public int DeleteTblWeighingMachine(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingMachineDAO.DeleteTblWeighingMachine(idWeighingMachine, conn, tran);
        }

        #endregion
        
    }
}
