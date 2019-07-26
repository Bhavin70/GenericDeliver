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
    public class DimProdCatBL : IDimProdCatBL
    {
        private readonly IDimProdCatDAO _iDimProdCatDAO;
        public DimProdCatBL(IDimProdCatDAO iDimProdCatDAO)
        {
            _iDimProdCatDAO = iDimProdCatDAO;
        }
        #region Selection
        public List<DimProdCatTO> SelectAllDimProdCatList()
        {
            return  _iDimProdCatDAO.SelectAllDimProdCat();
        }

        public DimProdCatTO SelectDimProdCatTO(Int32 idProdCat)
        {
            return  _iDimProdCatDAO.SelectDimProdCat(idProdCat);
        }

       

        #endregion
        
        #region Insertion
        public int InsertDimProdCat(DimProdCatTO dimProdCatTO)
        {
            return _iDimProdCatDAO.InsertDimProdCat(dimProdCatTO);
        }

        public int InsertDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimProdCatDAO.InsertDimProdCat(dimProdCatTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimProdCat(DimProdCatTO dimProdCatTO)
        {
            return _iDimProdCatDAO.UpdateDimProdCat(dimProdCatTO);
        }

        public int UpdateDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimProdCatDAO.UpdateDimProdCat(dimProdCatTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimProdCat(Int32 idProdCat)
        {
            return _iDimProdCatDAO.DeleteDimProdCat(idProdCat);
        }

        public int DeleteDimProdCat(Int32 idProdCat, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimProdCatDAO.DeleteDimProdCat(idProdCat, conn, tran);
        }

        #endregion
        
    }
}
