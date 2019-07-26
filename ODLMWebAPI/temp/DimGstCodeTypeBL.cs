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
    public class DimGstCodeTypeBL : IDimGstCodeTypeBL
    {
        #region Selection
        public List<DimGstCodeTypeTO> SelectAllDimGstCodeTypeList()
        {
            return DimGstCodeTypeDAO.SelectAllDimGstCodeType();
        }

        public DimGstCodeTypeTO SelectDimGstCodeTypeTO(Int32 idCodeType)
        {
            return DimGstCodeTypeDAO.SelectDimGstCodeType(idCodeType);
        }

       

        #endregion
        
        #region Insertion
        public int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO)
        {
            return DimGstCodeTypeDAO.InsertDimGstCodeType(dimGstCodeTypeTO);
        }

        public int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimGstCodeTypeDAO.InsertDimGstCodeType(dimGstCodeTypeTO, conn, tran);
        }

        #endregion
                
        #region Deletion
        public int DeleteDimGstCodeType(Int32 idCodeType)
        {
            return DimGstCodeTypeDAO.DeleteDimGstCodeType(idCodeType);
        }

        public int DeleteDimGstCodeType(Int32 idCodeType, SqlConnection conn, SqlTransaction tran)
        {
            return DimGstCodeTypeDAO.DeleteDimGstCodeType(idCodeType, conn, tran);
        }

        #endregion
        
    }
}
