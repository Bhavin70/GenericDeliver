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
       
        public List<DimPageElementTypesTO> SelectAllDimPageElementTypesList()
        {
            return  DimPageElementTypesDAO.SelectAllDimPageElementTypes();
        }

        public DimPageElementTypesTO SelectDimPageElementTypesTO(Int32 idPageEleType)
        {
            return  DimPageElementTypesDAO.SelectDimPageElementTypes(idPageEleType);
        }

        #endregion
        
        #region Insertion
        public int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
            return DimPageElementTypesDAO.InsertDimPageElementTypes(dimPageElementTypesTO);
        }

        public int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimPageElementTypesDAO.InsertDimPageElementTypes(dimPageElementTypesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
            return DimPageElementTypesDAO.UpdateDimPageElementTypes(dimPageElementTypesTO);
        }

        public int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimPageElementTypesDAO.UpdateDimPageElementTypes(dimPageElementTypesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimPageElementTypes(Int32 idPageEleType)
        {
            return DimPageElementTypesDAO.DeleteDimPageElementTypes(idPageEleType);
        }

        public int DeleteDimPageElementTypes(Int32 idPageEleType, SqlConnection conn, SqlTransaction tran)
        {
            return DimPageElementTypesDAO.DeleteDimPageElementTypes(idPageEleType, conn, tran);
        }

        #endregion
        
    }
}
