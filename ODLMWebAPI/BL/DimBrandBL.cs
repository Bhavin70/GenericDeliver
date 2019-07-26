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
    public class DimBrandBL : IDimBrandBL
    { 
        private readonly IDimBrandDAO _iDimBrandDAO;
        public DimBrandBL(IDimBrandDAO iDimBrandDAO)
        {
            _iDimBrandDAO = iDimBrandDAO;
        }
        #region Selection
        public List<DimBrandTO> SelectAllDimBrand()
        {
            return _iDimBrandDAO.SelectAllDimBrand();
        }

        public List<DimBrandTO> SelectAllDimBrandList()
        {
           return _iDimBrandDAO.SelectAllDimBrand();
        }

        public DimBrandTO SelectDimBrandTO(Int32 idBrand)
        {
           return _iDimBrandDAO.SelectDimBrand(idBrand);
        }

        public DimBrandTO SelectDimBrandTO(Int32 idBrand,SqlConnection conn,SqlTransaction tran)
        {
            return _iDimBrandDAO.SelectDimBrand(idBrand,conn,tran);
        }

        public List<DimBrandTO> SelectAllDimBrandList(DimBrandTO dimBrandTO)
        {
            return _iDimBrandDAO.SelectAllDimBrand(dimBrandTO);
        }

        #endregion

        #region Insertion
        public int InsertDimBrand(DimBrandTO dimBrandTO)
        {
            return _iDimBrandDAO.InsertDimBrand(dimBrandTO);
        }

        public int InsertDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimBrandDAO.InsertDimBrand(dimBrandTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateDimBrand(DimBrandTO dimBrandTO)
        {
            return _iDimBrandDAO.UpdateDimBrand(dimBrandTO);
        }

        public int UpdateDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimBrandDAO.UpdateDimBrand(dimBrandTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteDimBrand(Int32 idBrand)
        {
            return _iDimBrandDAO.DeleteDimBrand(idBrand);
        }

        public int DeleteDimBrand(Int32 idBrand, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimBrandDAO.DeleteDimBrand(idBrand, conn, tran);
        }

        #endregion

    }
}
