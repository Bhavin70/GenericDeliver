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
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.BL
{
    public class TblPurchaseCompetitorExtBL : ITblPurchaseCompetitorExtBL
    {
        private readonly ITblPurchaseCompetitorExtDAO _iTblPurchaseCompetitorExtDAO;
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseCompetitorExtBL(IConnectionString iConnectionString, ITblPurchaseCompetitorExtDAO iTblPurchaseCompetitorExtDAO)
        {
            _iTblPurchaseCompetitorExtDAO = iTblPurchaseCompetitorExtDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList()
        {
            return  _iTblPurchaseCompetitorExtDAO.SelectAllTblPurchaseCompetitorExt();
        }

        public TblPurchaseCompetitorExtTO SelectTblPurchaseCompetitorExtTO(Int32 idPurCompetitorExt)
        {
            return  _iTblPurchaseCompetitorExtDAO.SelectTblPurchaseCompetitorExt(idPurCompetitorExt);
        }

        #endregion
        
        #region Insertion
        public int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO)
        {
            return _iTblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO);
        }

        public int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO, conn, tran);
        }

        /// <summary>
        ///  Priyanka [16-02-18]: Added to get Purchase Competitor Details
        /// </summary>
        /// <returns></returns>
        public List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList(Int32 organizationId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblPurchaseCompetitorExtList(organizationId, conn, tran);
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

        public List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList(Int32 organizationId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.SelectAllTblPurchaseCompetitorExt(organizationId, conn, tran);

        }

        #endregion

        #region Updation
        public int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO)
        {
            return _iTblPurchaseCompetitorExtDAO.UpdateTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO);
        }

        public int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.UpdateTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt)
        {
            return _iTblPurchaseCompetitorExtDAO.DeleteTblPurchaseCompetitorExt(idPurCompetitorExt);
        }

        public int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.DeleteTblPurchaseCompetitorExt(idPurCompetitorExt, conn, tran);
        }

        #endregion
        
    }
}
