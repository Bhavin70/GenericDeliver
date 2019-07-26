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

namespace ODLMWebAPI.BL
{
    public class DimPageElementTypesBL : IDimPageElementTypesBL
    {
        #region Selection
        private readonly DimPageElementTypesDAO _iDimPageElementTypesDAO;
        public DimPageElementTypesBL(DimPageElementTypesDAO iDimPageElementTypesDAO)
        {
            _iDimPageElementTypesDAO = iDimPageElementTypesDAO;
        }
        public List<DimPageElementTypesTO> SelectAllDimPageElementTypesList()
        {
            return  _iDimPageElementTypesDAO.SelectAllDimPageElementTypes();
        }

        public DimPageElementTypesTO SelectDimPageElementTypesTO(Int32 idPageEleType)
        {
            return  _iDimPageElementTypesDAO.SelectDimPageElementTypes(idPageEleType);
        }

        #endregion
        #region Insertion
        public int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
            return _iDimPageElementTypesDAO.InsertDimPageElementTypes(dimPageElementTypesTO);
        }

        public int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPageElementTypesDAO.InsertDimPageElementTypes(dimPageElementTypesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
            return _iDimPageElementTypesDAO.UpdateDimPageElementTypes(dimPageElementTypesTO);
        }

        public int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPageElementTypesDAO.UpdateDimPageElementTypes(dimPageElementTypesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimPageElementTypes(Int32 idPageEleType)
        {
            return _iDimPageElementTypesDAO.DeleteDimPageElementTypes(idPageEleType);
        }

        public int DeleteDimPageElementTypes(Int32 idPageEleType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPageElementTypesDAO.DeleteDimPageElementTypes(idPageEleType, conn, tran);
        }

        #endregion
        
    }
}
