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
    public class TblVisitFollowupInfoBL : ITblVisitFollowupInfoBL
    {
        private readonly ITblVisitFollowupInfoDAO _iTblVisitFollowupInfoDAO;
        public TblVisitFollowupInfoBL(ITblVisitFollowupInfoDAO iTblVisitFollowupInfoDAO)
        {
            _iTblVisitFollowupInfoDAO = iTblVisitFollowupInfoDAO;
        }
        #region Selection
        public DataTable SelectAllTblVisitFollowupInfo()
        {
            return _iTblVisitFollowupInfoDAO.SelectAllTblVisitFollowupInfo();
        }

        public List<TblVisitFollowupInfoTO> SelectAllTblVisitFollowupInfoList()
        {
            DataTable tblVisitFollowupInfoTODT = _iTblVisitFollowupInfoDAO.SelectAllTblVisitFollowupInfo();
            return ConvertDTToList(tblVisitFollowupInfoTODT);
        }

        public TblVisitFollowupInfoTO SelectTblVisitFollowupInfoTO(Int32 idProjectFollowUpInfo)
        {
            DataTable tblVisitFollowupInfoTODT = _iTblVisitFollowupInfoDAO.SelectTblVisitFollowupInfo(idProjectFollowUpInfo);
            List<TblVisitFollowupInfoTO> tblVisitFollowupInfoTOList = ConvertDTToList(tblVisitFollowupInfoTODT);
            if (tblVisitFollowupInfoTOList != null && tblVisitFollowupInfoTOList.Count == 1)
                return tblVisitFollowupInfoTOList[0];
            else
                return null;
        }

        public List<TblVisitFollowupInfoTO> ConvertDTToList(DataTable tblVisitFollowupInfoTODT)
        {
            List<TblVisitFollowupInfoTO> tblVisitFollowupInfoTOList = new List<TblVisitFollowupInfoTO>();
            if (tblVisitFollowupInfoTODT != null)
            {

            }
            return tblVisitFollowupInfoTOList;
        }

        public TblVisitFollowupInfoTO SelectVisitFollowupInfoTO(Int32 visitid)
        {
            TblVisitFollowupInfoTO visitFollowupInfoTO = _iTblVisitFollowupInfoDAO.SelectVisitFollowupInfo(visitid);
            if (visitFollowupInfoTO != null )
                return visitFollowupInfoTO;
            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO)
        {
            return _iTblVisitFollowupInfoDAO.InsertTblVisitFollowupInfo(tblVisitFollowupInfoTO);
        }

        public int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitFollowupInfoDAO.InsertTblVisitFollowupInfo(tblVisitFollowupInfoTO, conn, tran);
        }

        // Vaibhav [9-Oct-2017] save visit follow up information
        public ResultMessage SaveVisitFollowUpInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                result = InsertTblVisitFollowupInfo(tblVisitFollowupInfoTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblVisitFollowupInfo");
                    tran.Rollback();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveVisistFollowUpInfo");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO)
        {
            return _iTblVisitFollowupInfoDAO.UpdateTblVisitFollowupInfo(tblVisitFollowupInfoTO);
        }

        public int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitFollowupInfoDAO.UpdateTblVisitFollowupInfo(tblVisitFollowupInfoTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] save visit follow up information
        public ResultMessage UpdateVisitFollowUpInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                result = UpdateTblVisitFollowupInfo(tblVisitFollowupInfoTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblVisitFollowupInfo");
                    tran.Rollback();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitFollowUpInfo");
                tran.Rollback();
                return resultMessage;
            }
        }


        #endregion

        #region Deletion
        public int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo)
        {
            return _iTblVisitFollowupInfoDAO.DeleteTblVisitFollowupInfo(idProjectFollowUpInfo);
        }

        public int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitFollowupInfoDAO.DeleteTblVisitFollowupInfo(idProjectFollowUpInfo, conn, tran);
        }

        #endregion

    }
}
