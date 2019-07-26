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
    public class DimTaxTypeBL : IDimTaxTypeBL
    {
        #region Selection

        public List<DimTaxTypeTO> SelectAllDimTaxTypeList()
        {
            return DimTaxTypeDAO.SelectAllDimTaxType();
        }

        public DimTaxTypeTO SelectDimTaxTypeTO(Int32 idTaxType)
        {
            return  DimTaxTypeDAO.SelectDimTaxType(idTaxType);
        }

        #endregion
        
        #region Insertion
        public int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO)
        {
            return DimTaxTypeDAO.InsertDimTaxType(dimTaxTypeTO);
        }

        public int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimTaxTypeDAO.InsertDimTaxType(dimTaxTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO)
        {
            return DimTaxTypeDAO.UpdateDimTaxType(dimTaxTypeTO);
        }

        public int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimTaxTypeDAO.UpdateDimTaxType(dimTaxTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimTaxType(Int32 idTaxType)
        {
            return DimTaxTypeDAO.DeleteDimTaxType(idTaxType);
        }

        public int DeleteDimTaxType(Int32 idTaxType, SqlConnection conn, SqlTransaction tran)
        {
            return DimTaxTypeDAO.DeleteDimTaxType(idTaxType, conn, tran);
        }

        #endregion
        
    }
}
