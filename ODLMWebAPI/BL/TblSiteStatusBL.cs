using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblSiteStatusBL : ITblSiteStatusBL
    {
        private readonly ITblSiteStatusDAO _iTblSiteStatusDAO;
        private readonly IConnectionString _iConnectionString;
        public TblSiteStatusBL(ITblSiteStatusDAO iTblSiteStatusDAO, IConnectionString iConnectionString)
        {
            _iTblSiteStatusDAO = iTblSiteStatusDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public DataTable SelectAllTblSiteStatus()
        {
            return _iTblSiteStatusDAO.SelectAllTblSiteStatus();
        }

        public List<TblSiteStatusTO> SelectAllTblSiteStatusList()
        {
            DataTable tblSiteStatusTODT = _iTblSiteStatusDAO.SelectAllTblSiteStatus();
            return ConvertDTToList(tblSiteStatusTODT);
        }

        public TblSiteStatusTO SelectTblSiteStatusTO(Int32 idSiteStatus)
        {
            DataTable tblSiteStatusTODT = _iTblSiteStatusDAO.SelectTblSiteStatus(idSiteStatus);
            List<TblSiteStatusTO> tblSiteStatusTOList = ConvertDTToList(tblSiteStatusTODT);
            if (tblSiteStatusTOList != null && tblSiteStatusTOList.Count == 1)
                return tblSiteStatusTOList[0];
            else
                return null;
        }

        // Vaibhav [3-Oct-2017] added to select site status fro drop down
        public List<DropDownTO> SelectAllSiteStatusForDropDown()
        {
            List<DropDownTO> siteStatusTOList = _iTblSiteStatusDAO.SelectSiteStatusForDropDown();
            if (siteStatusTOList != null)
                return siteStatusTOList;
            else
                return null;
        }

        public List<TblSiteStatusTO> ConvertDTToList(DataTable tblSiteStatusTODT)
        {
            List<TblSiteStatusTO> tblSiteStatusTOList = new List<TblSiteStatusTO>();
            if (tblSiteStatusTODT != null)
            {
            }
            return tblSiteStatusTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblSiteStatus(TblSiteStatusTO tblSiteStatusTO)
        {
            return _iTblSiteStatusDAO.InsertTblSiteStatus(tblSiteStatusTO);
        }

        public int InsertTblSiteStatus(ref TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                result = _iTblSiteStatusDAO.InsertTblSiteStatus(ref tblSiteStatusTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblSiteStatus");
                return -1;
            }
            finally
            {
                //conn.Close();
            }
        }

        // Vaibhav [3-Oct-2017] added to save new site status
        public ResultMessage SaveNewSiteStatus(TblSiteStatusTO tblSiteStatusTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();

                result = InsertTblSiteStatus(ref tblSiteStatusTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewSiteStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO)
        {
            return _iTblSiteStatusDAO.UpdateTblSiteStatus(tblSiteStatusTO);
        }

        public int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSiteStatusDAO.UpdateTblSiteStatus(tblSiteStatusTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblSiteStatus(Int32 idSiteStatus)
        {
            return _iTblSiteStatusDAO.DeleteTblSiteStatus(idSiteStatus);
        }

        public int DeleteTblSiteStatus(Int32 idSiteStatus, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSiteStatusDAO.DeleteTblSiteStatus(idSiteStatus, conn, tran);
        }

        #endregion

    }
}
