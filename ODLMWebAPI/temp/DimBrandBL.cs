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
    public class DimBrandBL : IDimBrandBL
    {
        #region Selection
        public List<DimBrandTO> SelectAllDimBrand()
        {
            return DimBrandDAO.SelectAllDimBrand();
        }

        public List<DimBrandTO> SelectAllDimBrandList()
        {
           return DimBrandDAO.SelectAllDimBrand();
        }

        public DimBrandTO SelectDimBrandTO(Int32 idBrand)
        {
           return DimBrandDAO.SelectDimBrand(idBrand);
        }

        public DimBrandTO SelectDimBrandTO(Int32 idBrand,SqlConnection conn,SqlTransaction tran)
        {
            return DimBrandDAO.SelectDimBrand(idBrand,conn,tran);
        }

        public List<DimBrandTO> SelectAllDimBrandList(DimBrandTO dimBrandTO)
        {
            return DimBrandDAO.SelectAllDimBrand(dimBrandTO);
        }

        #endregion

        #region Insertion
        public int InsertDimBrand(DimBrandTO dimBrandTO)
        {
            return DimBrandDAO.InsertDimBrand(dimBrandTO);
        }

        public int InsertDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimBrandDAO.InsertDimBrand(dimBrandTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateDimBrand(DimBrandTO dimBrandTO)
        {
            return DimBrandDAO.UpdateDimBrand(dimBrandTO);
        }

        public int UpdateDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimBrandDAO.UpdateDimBrand(dimBrandTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteDimBrand(Int32 idBrand)
        {
            return DimBrandDAO.DeleteDimBrand(idBrand);
        }

        public int DeleteDimBrand(Int32 idBrand, SqlConnection conn, SqlTransaction tran)
        {
            return DimBrandDAO.DeleteDimBrand(idBrand, conn, tran);
        }

        #endregion

    }
}
