using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblSessionBL : ITblSessionBL
    {
        private readonly ITblSessionDAO _iTblSessionDAO;
        private readonly ITblSessionHistoryBL _iTblSessionHistoryBL;
        private readonly ICommon _iCommon;
        public TblSessionBL(ICommon iCommon, ITblSessionDAO iTblSessionDAO, ITblSessionHistoryBL iTblSessionHistoryBL)
        {
            _iTblSessionDAO = iTblSessionDAO;
            _iTblSessionHistoryBL = iTblSessionHistoryBL;
            _iCommon = iCommon;
        }
        #region Selection
        public TblSessionTO SelectAllTblSession()
        {
            return _iTblSessionDAO.SelectAllTblSession();
        }

        public List<TblSessionTO> SelectAllTblSessionList()
        {
            return _iTblSessionDAO.SelectAllTblSessionData();
        }

        public TblSessionTO SelectTblSessionTO(int idsession)
        {
            return _iTblSessionDAO.SelectTblSession(idsession);
        }

        public TblSessionTO getSessionAllreadyExist(Int32 idUser, Int32 ConversionUserId)
        {
            return _iTblSessionDAO.getSessionAllreadyExist(idUser, ConversionUserId);
        }

        #endregion

        #region Insertion
        public int InsertTblSession(TblSessionTO tblSessionTO)
        {
            return _iTblSessionDAO.InsertTblSession(tblSessionTO);
        }

        public int InsertTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSessionDAO.InsertTblSession(tblSessionTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblSession(int idsession)
        {
            TblSessionTO SessionTO = new TblSessionTO();
            SessionTO = _iTblSessionDAO.SelectTblSession(idsession);
            if (SessionTO != null)
            {
                SessionTO.EndTime = _iCommon.ServerDateTime;
                SessionTO.IsEndSession = 1;
                return _iTblSessionDAO.UpdateTblSession(SessionTO);
            }
            else
            {
                return 0;
            }
        }

        public int UpdateTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSessionDAO.UpdateTblSession(tblSessionTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblSession(int idsession)
        {
            return _iTblSessionDAO.DeleteTblSession(idsession);
        }
        public int deleteAllMsgData()
        {
            _iTblSessionHistoryBL.DeleteTblSessionHistory();
            return _iTblSessionDAO.DeleteTblSession();

        }

        public int DeleteTblSession(int idsession, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSessionDAO.DeleteTblSession(idsession, conn, tran);
        }

        #endregion

    }
}
