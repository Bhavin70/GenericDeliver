using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class DimVehDocTypeBL : IDimVehDocTypeBL
    {
        private readonly IDimVehDocTypeDAO _iDimVehDocTypeDAO;
        public DimVehDocTypeBL(IDimVehDocTypeDAO iDimVehDocTypeDAO)
        {
            _iDimVehDocTypeDAO = iDimVehDocTypeDAO;
        }
        #region Selection
        public List<DimVehDocTypeTO> SelectAllDimVehDocType()
        {
            return _iDimVehDocTypeDAO.SelectAllDimVehDocType();
        }

        public List<DimVehDocTypeTO> SelectAllDimVehDocTypeList()
        {
           return _iDimVehDocTypeDAO.SelectAllDimVehDocType();
        }

        public DimVehDocTypeTO SelectDimVehDocTypeTO(Int32 idVehDocType)
        {
            return _iDimVehDocTypeDAO.SelectDimVehDocType(idVehDocType);
        }

        #endregion
        
        #region Insertion
        public int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO)
        {
            return _iDimVehDocTypeDAO.InsertDimVehDocType(dimVehDocTypeTO);
        }

        public int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehDocTypeDAO.InsertDimVehDocType(dimVehDocTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO)
        {
            return _iDimVehDocTypeDAO.UpdateDimVehDocType(dimVehDocTypeTO);
        }

        public int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehDocTypeDAO.UpdateDimVehDocType(dimVehDocTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimVehDocType(Int32 idVehDocType)
        {
            return _iDimVehDocTypeDAO.DeleteDimVehDocType(idVehDocType);
        }

        public int DeleteDimVehDocType(Int32 idVehDocType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehDocTypeDAO.DeleteDimVehDocType(idVehDocType, conn, tran);
        }

        #endregion
        
    }
}
