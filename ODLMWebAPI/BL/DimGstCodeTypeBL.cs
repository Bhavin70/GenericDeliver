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
    public class DimGstCodeTypeBL : IDimGstCodeTypeBL
    {
        private readonly IDimGstCodeTypeDAO _iDimGstCodeTypeDAO;
        public DimGstCodeTypeBL(IDimGstCodeTypeDAO iDimGstCodeTypeDAO)
        {
            _iDimGstCodeTypeDAO = iDimGstCodeTypeDAO;
        }
        #region Selection
        public List<DimGstCodeTypeTO> SelectAllDimGstCodeTypeList()
        {
            return _iDimGstCodeTypeDAO.SelectAllDimGstCodeType();
        }

        public DimGstCodeTypeTO SelectDimGstCodeTypeTO(Int32 idCodeType)
        {
            return _iDimGstCodeTypeDAO.SelectDimGstCodeType(idCodeType);
        }

       

        #endregion
        
        #region Insertion
        public int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO)
        {
            return _iDimGstCodeTypeDAO.InsertDimGstCodeType(dimGstCodeTypeTO);
        }

        public int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimGstCodeTypeDAO.InsertDimGstCodeType(dimGstCodeTypeTO, conn, tran);
        }

        #endregion
                
        #region Deletion
        public int DeleteDimGstCodeType(Int32 idCodeType)
        {
            return _iDimGstCodeTypeDAO.DeleteDimGstCodeType(idCodeType);
        }

        public int DeleteDimGstCodeType(Int32 idCodeType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimGstCodeTypeDAO.DeleteDimGstCodeType(idCodeType, conn, tran);
        }

        #endregion
        
    }
}
