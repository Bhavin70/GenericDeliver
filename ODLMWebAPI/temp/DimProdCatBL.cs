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
    public class DimProdCatBL : IDimProdCatBL
    {
        #region Selection
        public List<DimProdCatTO> SelectAllDimProdCatList()
        {
            return  DimProdCatDAO.SelectAllDimProdCat();
        }

        public DimProdCatTO SelectDimProdCatTO(Int32 idProdCat)
        {
            return  DimProdCatDAO.SelectDimProdCat(idProdCat);
        }

       

        #endregion
        
        #region Insertion
        public int InsertDimProdCat(DimProdCatTO dimProdCatTO)
        {
            return DimProdCatDAO.InsertDimProdCat(dimProdCatTO);
        }

        public int InsertDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdCatDAO.InsertDimProdCat(dimProdCatTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimProdCat(DimProdCatTO dimProdCatTO)
        {
            return DimProdCatDAO.UpdateDimProdCat(dimProdCatTO);
        }

        public int UpdateDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdCatDAO.UpdateDimProdCat(dimProdCatTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimProdCat(Int32 idProdCat)
        {
            return DimProdCatDAO.DeleteDimProdCat(idProdCat);
        }

        public int DeleteDimProdCat(Int32 idProdCat, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdCatDAO.DeleteDimProdCat(idProdCat, conn, tran);
        }

        #endregion
        
    }
}
