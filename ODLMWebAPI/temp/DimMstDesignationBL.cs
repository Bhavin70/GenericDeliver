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
namespace ODLMWebAPI.BL
{
    public class DimMstDesignationBL : IDimMstDesignationBL
    {
        #region Selection
       

        public List<DimMstDesignationTO> SelectAllDimMstDesignationList()
        {
            return DimMstDesignationDAO.SelectAllDimMstDesignation();
        }

        public DimMstDesignationTO SelectDimMstDesignationTO(Int32 idDesignation)
        {
            return DimMstDesignationDAO.SelectDimMstDesignation(idDesignation);
        
        }

        public List<DropDownTO> SelectAllDesignationForDropDownList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> dropDownTO = DAL.DimMstDesignationDAO.SelectAllDesignationForDropDownList();
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
            return DimMstDesignationDAO.InsertDimMstDesignation(dimMstDesignationTO);
        }

        public int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDesignationDAO.InsertDimMstDesignation(dimMstDesignationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO)
        {
            return DimMstDesignationDAO.UpdateDimMstDesignation(dimMstDesignationTO);
        }

        public int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDesignationDAO.UpdateDimMstDesignation(dimMstDesignationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimMstDesignation(Int32 idDesignation)
        {
            return DimMstDesignationDAO.DeleteDimMstDesignation(idDesignation);
        }

        public int DeleteDimMstDesignation(Int32 idDesignation, SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDesignationDAO.DeleteDimMstDesignation(idDesignation, conn, tran);
        }

        #endregion
        
    }
}
