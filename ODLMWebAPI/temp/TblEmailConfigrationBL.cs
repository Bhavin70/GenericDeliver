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
namespace ODLMWebAPI.BL
{
    public class TblEmailConfigrationBL : ITblEmailConfigrationBL
    {
        #region Selection
        public List<TblEmailConfigrationTO> SelectAllDimEmailConfigration()
        {
            return TblEmailConfigrationDAO.SelectAllDimEmailConfigration();
        }

        public List<TblEmailConfigrationTO> SelectAllDimEmailConfigrationList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblEmailConfigrationTO> list = TblEmailConfigrationDAO.SelectAllDimEmailConfigration();
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimEmailConfigrationList");
                return null;
            }
        }

        public TblEmailConfigrationTO SelectDimEmailConfigrationTO()
        {
            TblEmailConfigrationTO dimEmailConfigrationTODT = TblEmailConfigrationDAO.SelectDimEmailConfigrationIsActive();
            if(dimEmailConfigrationTODT !=null)
            {
                return dimEmailConfigrationTODT;
            }
            else
            {
                return null;
            }
        }

        #endregion
        
        #region Insertion
        public int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            return TblEmailConfigrationDAO.InsertDimEmailConfigration(dimEmailConfigrationTO);
        }

        public int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEmailConfigrationDAO.InsertDimEmailConfigration(dimEmailConfigrationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            return TblEmailConfigrationDAO.UpdateDimEmailConfigration(dimEmailConfigrationTO);
        }

        public int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEmailConfigrationDAO.UpdateDimEmailConfigration(dimEmailConfigrationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimEmailConfigration(Int32 idEmailConfig)
        {
            return TblEmailConfigrationDAO.DeleteDimEmailConfigration(idEmailConfig);
        }

        public int DeleteDimEmailConfigration(Int32 idEmailConfig, SqlConnection conn, SqlTransaction tran)
        {
            return TblEmailConfigrationDAO.DeleteDimEmailConfigration(idEmailConfig, conn, tran);
        }

        #endregion
        
    }
}
