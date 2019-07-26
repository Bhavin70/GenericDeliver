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
    public class DimTaxTypeBL : IDimTaxTypeBL
    {
        private readonly IDimTaxTypeDAO _iDimTaxTypeDAO;
        public DimTaxTypeBL(IDimTaxTypeDAO iDimTaxTypeDAO)
        {
            _iDimTaxTypeDAO = iDimTaxTypeDAO;
        }
        #region Selection

        public List<DimTaxTypeTO> SelectAllDimTaxTypeList()
        {
            return _iDimTaxTypeDAO.SelectAllDimTaxType();
        }

        public DimTaxTypeTO SelectDimTaxTypeTO(Int32 idTaxType)
        {
            return  _iDimTaxTypeDAO.SelectDimTaxType(idTaxType);
        }

        #endregion
        
        #region Insertion
        public int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO)
        {
            return _iDimTaxTypeDAO.InsertDimTaxType(dimTaxTypeTO);
        }

        public int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimTaxTypeDAO.InsertDimTaxType(dimTaxTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO)
        {
            return _iDimTaxTypeDAO.UpdateDimTaxType(dimTaxTypeTO);
        }

        public int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimTaxTypeDAO.UpdateDimTaxType(dimTaxTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimTaxType(Int32 idTaxType)
        {
            return _iDimTaxTypeDAO.DeleteDimTaxType(idTaxType);
        }

        public int DeleteDimTaxType(Int32 idTaxType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimTaxTypeDAO.DeleteDimTaxType(idTaxType, conn, tran);
        }

        #endregion
        
    }
}
