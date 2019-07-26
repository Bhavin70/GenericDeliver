using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class DimMstDesignationBL : IDimMstDesignationBL
    {
        #region Selection
        private readonly IDimMstDesignationDAO _iDimMstDesignationDAO;
        public DimMstDesignationBL(IDimMstDesignationDAO iDimMstDesignationDAO)
        {
            _iDimMstDesignationDAO = iDimMstDesignationDAO;
        }

        public List<DimMstDesignationTO> SelectAllDimMstDesignationList()
        {
            return _iDimMstDesignationDAO.SelectAllDimMstDesignation();
        }

        public DimMstDesignationTO SelectDimMstDesignationTO(Int32 idDesignation)
        {
            return _iDimMstDesignationDAO.SelectDimMstDesignation(idDesignation);
        
        }

        public List<DropDownTO> SelectAllDesignationForDropDownList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> dropDownTO = _iDimMstDesignationDAO.SelectAllDesignationForDropDownList();
                if (dropDownTO != null)
                    return dropDownTO;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDesignationForDropDown");
                return null;
            }
        }



        #endregion

        #region Insertion
        public int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO)
        {
            return _iDimMstDesignationDAO.InsertDimMstDesignation(dimMstDesignationTO);
        }

        public int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimMstDesignationDAO.InsertDimMstDesignation(dimMstDesignationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO)
        {
            return _iDimMstDesignationDAO.UpdateDimMstDesignation(dimMstDesignationTO);
        }

        public int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimMstDesignationDAO.UpdateDimMstDesignation(dimMstDesignationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimMstDesignation(Int32 idDesignation)
        {
            return _iDimMstDesignationDAO.DeleteDimMstDesignation(idDesignation);
        }

        public int DeleteDimMstDesignation(Int32 idDesignation, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimMstDesignationDAO.DeleteDimMstDesignation(idDesignation, conn, tran);
        }

        #endregion
        
    }
}
