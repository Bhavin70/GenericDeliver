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
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblUnloadingStandDescBL : ITblUnloadingStandDescBL
    {
        private readonly ITblUnloadingStandDescDAO _iTblUnloadingStandDescDAO;
        public TblUnloadingStandDescBL(ITblUnloadingStandDescDAO iTblUnloadingStandDescDAO)
        {
            _iTblUnloadingStandDescDAO = iTblUnloadingStandDescDAO;
        }
        #region Selection
        public List<TblUnloadingStandDescTO> SelectAllTblUnloadingStandDescList()
        {
            return _iTblUnloadingStandDescDAO.SelectAllTblUnloadingStandDesc();
        }

        public TblUnloadingStandDescTO SelectTblUnloadingStandDescTO(Int32 idUnloadingStandDesc)
        {
            return _iTblUnloadingStandDescDAO.SelectTblUnloadingStandDesc(idUnloadingStandDesc);
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] Added to select all Unloading Standard Descriptions for drop down
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectAllUnloadingStandDescForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> list = _iTblUnloadingStandDescDAO.SelectAllUnloadingStandDescForDropDown();

                if (list != null)               
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnloadingStandDescForDropDown");
                return null;
            }
        }



        #endregion

        #region Insertion
        public int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO UnloadingStandDescTO)
        {
            return _iTblUnloadingStandDescDAO.InsertTblUnloadingStandDesc(UnloadingStandDescTO);
        }

        public int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUnloadingStandDescDAO.InsertTblUnloadingStandDesc(tblUnloadingStandDescTO, conn, tran);
        }



        #endregion

        #region Updation
        public int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO)
        {
            return _iTblUnloadingStandDescDAO.UpdateTblUnloadingStandDesc(tblUnloadingStandDescTO);
        }

        public int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUnloadingStandDescDAO.UpdateTblUnloadingStandDesc(tblUnloadingStandDescTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc)
        {
            return _iTblUnloadingStandDescDAO.DeleteTblUnloadingStandDesc(idUnloadingStandDesc);
        }

        public int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUnloadingStandDescDAO.DeleteTblUnloadingStandDesc(idUnloadingStandDesc, conn, tran);
        }

        #endregion

    }
}
