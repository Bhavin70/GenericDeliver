using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblVisitPurposeBL : ITblVisitPurposeBL
    { 
        private readonly ITblVisitPurposeDAO _iTblVisitPurposeDAO;
        public TblVisitPurposeBL(ITblVisitPurposeDAO iTblVisitPurposeDAO)
        {
            _iTblVisitPurposeDAO = iTblVisitPurposeDAO;
        }
        #region Selection
        public List<TblVisitPurposeTO> SelectAllTblVisitPurpose()
        {
            return _iTblVisitPurposeDAO.SelectAllTblVisitPurpose();
        }

        // Vaibhav [2-Oct-2017] added to select visit purpose list
        public List<DropDownTO> SelectVisitPurposeListForDropDown(int visitTypeId)
        {
            List<DropDownTO> reportingTypeList = _iTblVisitPurposeDAO.SelectVisitPurposeListForDropDown(visitTypeId);
            if (reportingTypeList != null)
                return reportingTypeList;
            else
                return null;
        }

        public TblVisitPurposeTO SelectTblVisitPurposeTO(Int32 idVisitPurpose)
        {
            DataTable tblVisitPurposeTODT = _iTblVisitPurposeDAO.SelectTblVisitPurpose(idVisitPurpose);
            List<TblVisitPurposeTO> tblVisitPurposeTOList = ConvertDTToList(tblVisitPurposeTODT);
            if (tblVisitPurposeTOList != null && tblVisitPurposeTOList.Count == 1)
                return tblVisitPurposeTOList[0];
            else
                return null;
        }



        public List<TblVisitPurposeTO> ConvertDTToList(DataTable tblVisitPurposeTODT)
        {
            List<TblVisitPurposeTO> tblVisitPurposeTOList = new List<TblVisitPurposeTO>();
            if (tblVisitPurposeTODT != null)
            {
                
            }
            return tblVisitPurposeTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO)
        {
            return _iTblVisitPurposeDAO.InsertTblVisitPurpose(tblVisitPurposeTO);
        }

        public int InsertTblVisitPurpose(ref TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                if(conn.State == ConnectionState.Closed)
                conn.Open();

                result = _iTblVisitPurposeDAO.InsertTblVisitPurpose(tblVisitPurposeTO, conn, tran);
                
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblVisitPurpose");
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitPurpose");
                return -1;
            }
            finally
            {
                //conn.Close();
            }
        }

        //// Vaibhav [3-Oct-2017] added to inser new visit purpose
        //public ResultMessage SaveNewVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO)
        //{
        //    SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction tran = null;
        //    ResultMessage resultMessage = new ResultMessage();
        //    int result = 0;
        //    try
        //    {
        //        conn.Open();

        //        result = InsertTblVisitPurpose(tblVisitPurposeTO, conn, tran);

        //        if (result != 1)
        //        {
        //            resultMessage.DefaultBehaviour("Error While InsertTblVisitPurpose");
        //            return resultMessage;
        //        }
        //        resultMessage.DefaultSuccessBehaviour();
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "SaveNewVisitPurpose");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        #endregion

        #region Updation
        public int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO)
        {
            return _iTblVisitPurposeDAO.UpdateTblVisitPurpose(tblVisitPurposeTO);
        }

        public int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitPurposeDAO.UpdateTblVisitPurpose(tblVisitPurposeTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblVisitPurpose(Int32 idVisitPurpose)
        {
            return _iTblVisitPurposeDAO.DeleteTblVisitPurpose(idVisitPurpose);
        }

        public int DeleteTblVisitPurpose(Int32 idVisitPurpose, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitPurposeDAO.DeleteTblVisitPurpose(idVisitPurpose, conn, tran);
        }

        #endregion

    }
}
