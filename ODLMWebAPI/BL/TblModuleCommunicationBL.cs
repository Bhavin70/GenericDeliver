using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblModuleCommunicationBL : ITblModuleCommunicationBL
    {
        private readonly ITblModuleCommunicationDAO _iTblModuleCommunicationDAO;
        private readonly ICommon _iCommon;
        public TblModuleCommunicationBL(ICommon iCommon, ITblModuleCommunicationDAO iTblModuleCommunicationDAO)
        {
            _iTblModuleCommunicationDAO = iTblModuleCommunicationDAO;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblModuleCommunicationTO> SelectAllTblModuleCommunicationList()
        {
            return _iTblModuleCommunicationDAO.SelectAllTblModuleCommunication();
        }

        public List<TblModuleCommunicationTO> SelectAllTblModuleCommunicationListById(Int32 srcModuleId, Int32 srcTxnId)
        {
            return _iTblModuleCommunicationDAO.SelectAllTblModuleCommunicationById(srcModuleId, srcTxnId);
        }

        public TblModuleCommunicationTO SelectTblModuleCommunicationTO(Int32 idModuleCommunication)
        {
            return _iTblModuleCommunicationDAO.SelectTblModuleCommunication(idModuleCommunication);
           
        }

        #endregion
        
        #region Insertion
        public int InsertTblModuleCommunication(List<TblModuleCommunicationTO> tblModuleCommunicationList, string loginUserId)
        {
            foreach (var item in tblModuleCommunicationList)
            {
                item.CreatedBy = Convert.ToInt32(loginUserId);
                item.CreatedOn = _iCommon.ServerDateTime;
                int result = _iTblModuleCommunicationDAO.InsertTblModuleCommunication(item);
                if (result != 1)
                {
                    return -1;
                }
            }
            return 1;
        }

        public int InsertTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModuleCommunicationDAO.InsertTblModuleCommunication(tblModuleCommunicationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO)
        {
            return _iTblModuleCommunicationDAO.UpdateTblModuleCommunication(tblModuleCommunicationTO);
        }

        public int UpdateTblModuleCommunication(TblModuleCommunicationTO tblModuleCommunicationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModuleCommunicationDAO.UpdateTblModuleCommunication(tblModuleCommunicationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblModuleCommunication(Int32 idModuleCommunication)
        {
            return _iTblModuleCommunicationDAO.DeleteTblModuleCommunication(idModuleCommunication);
        }

        public int DeleteTblModuleCommunication(Int32 idModuleCommunication, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModuleCommunicationDAO.DeleteTblModuleCommunication(idModuleCommunication, conn, tran);
        }

        #endregion
        
    }
}
