using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class DimVehDocTypeBL : IDimVehDocTypeBL
    {
        #region Selection
        public List<DimVehDocTypeTO> SelectAllDimVehDocType()
        {
            return DimVehDocTypeDAO.SelectAllDimVehDocType();
        }

        public List<DimVehDocTypeTO> SelectAllDimVehDocTypeList()
        {
           return DimVehDocTypeDAO.SelectAllDimVehDocType();
        }

        public DimVehDocTypeTO SelectDimVehDocTypeTO(Int32 idVehDocType)
        {
            return DimVehDocTypeDAO.SelectDimVehDocType(idVehDocType);
        }

        #endregion
        
        #region Insertion
        public int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO)
        {
            return DimVehDocTypeDAO.InsertDimVehDocType(dimVehDocTypeTO);
        }

        public int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVehDocTypeDAO.InsertDimVehDocType(dimVehDocTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO)
        {
            return DimVehDocTypeDAO.UpdateDimVehDocType(dimVehDocTypeTO);
        }

        public int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVehDocTypeDAO.UpdateDimVehDocType(dimVehDocTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimVehDocType(Int32 idVehDocType)
        {
            return DimVehDocTypeDAO.DeleteDimVehDocType(idVehDocType);
        }

        public int DeleteDimVehDocType(Int32 idVehDocType, SqlConnection conn, SqlTransaction tran)
        {
            return DimVehDocTypeDAO.DeleteDimVehDocType(idVehDocType, conn, tran);
        }

        #endregion
        
    }
}
