using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblGlobalRateBL : ITblGlobalRateBL
    {
        #region Selection
       

        public TblGlobalRateTO SelectTblGlobalRateTO(Int32 idGlobalRate)
        {
            return  TblGlobalRateDAO.SelectTblGlobalRate(idGlobalRate);
           
        }

        public TblGlobalRateTO SelectTblGlobalRateTO(Int32 idGlobalRate,SqlConnection conn,SqlTransaction tran)
        {
            return TblGlobalRateDAO.SelectTblGlobalRate(idGlobalRate,conn,tran);

        }

        public TblGlobalRateTO SelectLatestTblGlobalRateTO()
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.BeginTransaction();
                tran = conn.BeginTransaction();
                return TblGlobalRateDAO.SelectLatestTblGlobalRateTO(conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public TblGlobalRateTO SelectLatestTblGlobalRateTO(SqlConnection conn,SqlTransaction tran)
        {
            return TblGlobalRateDAO.SelectLatestTblGlobalRateTO(conn,tran);
        }

        public List<TblGlobalRateTO> SelectTblGlobalRateTOList(DateTime fromDate,DateTime toDate)
        {
            return TblGlobalRateDAO.SelectLatestTblGlobalRateTOList(fromDate,toDate);

        }

        public Dictionary<Int32, Int32> SelectLatestBrandAndRateDCT()
        {
            return TblGlobalRateDAO.SelectLatestBrandAndRateDCT();
        }

        public Dictionary<Int32, Int32> SelectLatestGroupAndRateDCT()
        {
            return TblGlobalRateDAO.SelectLatestGroupAndRateDCT();
        }

        public Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            return TblGlobalRateDAO.IsRateAlreadyDeclaredForTheDate(date, conn,tran);

        }

        /// <summary>
        /// [19/01/2018] Vijaymala : Added To Get Rate Of Group Against Product Item
        /// </summary>
        /// <param name="tblGlobalRateTO"></param>
        /// <returns></returns>

        public TblGlobalRateTO SelectProductGroupItemGlobalRate(Int32 prodItemId)
        {
            TblGlobalRateTO rateTO = new TblGlobalRateTO();
            TblGroupItemTO tblGroupItemTO = BL.TblGroupItemBL.SelectTblGroupItemDetails(prodItemId);
            if (tblGroupItemTO != null)
            {
                Dictionary<Int32, Int32> groupRateDCT = BL.TblGlobalRateBL.SelectLatestGroupAndRateDCT();
                if (groupRateDCT != null)
                {
                    if (groupRateDCT.ContainsKey(tblGroupItemTO.GroupId))
                    {
                        Int32 rateID = groupRateDCT[tblGroupItemTO.GroupId];
                        rateTO = BL.TblGlobalRateBL.SelectTblGlobalRateTO(rateID);
                    }
                }
            }
            return rateTO;

        }
        #endregion

        #region Insertion
        public int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO)
        {
            return TblGlobalRateDAO.InsertTblGlobalRate(tblGlobalRateTO);
        }

        public int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGlobalRateDAO.InsertTblGlobalRate(tblGlobalRateTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO)
        {
            return TblGlobalRateDAO.UpdateTblGlobalRate(tblGlobalRateTO);
        }

        public int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGlobalRateDAO.UpdateTblGlobalRate(tblGlobalRateTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblGlobalRate(Int32 idGlobalRate)
        {
            return TblGlobalRateDAO.DeleteTblGlobalRate(idGlobalRate);
        }

        public int DeleteTblGlobalRate(Int32 idGlobalRate, SqlConnection conn, SqlTransaction tran)
        {
            return TblGlobalRateDAO.DeleteTblGlobalRate(idGlobalRate, conn, tran);
        }

        #endregion
        
    }
}
