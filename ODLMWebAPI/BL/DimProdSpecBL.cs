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
    public class DimProdSpecBL : IDimProdSpecBL
    {
        private readonly IDimProdSpecDAO _iDimProdSpecDAO;
        public DimProdSpecBL(IDimProdSpecDAO iDimProdSpecDAO)
        {
            _iDimProdSpecDAO = iDimProdSpecDAO;
        }
        #region Selection
        public List<DimProdSpecTO> SelectAllDimProdSpecList()
        {
            return  _iDimProdSpecDAO.SelectAllDimProdSpec();
        }

        public DimProdSpecTO SelectDimProdSpecTO(Int32 idProdSpec)
        {
            return  _iDimProdSpecDAO.SelectDimProdSpec(idProdSpec);
        }

       

        #endregion
        
        #region Insertion
        public int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO)
        {
            return _iDimProdSpecDAO.InsertDimProdSpec(dimProdSpecTO);
        }

        public int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimProdSpecDAO.InsertDimProdSpec(dimProdSpecTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO)
        {
            return _iDimProdSpecDAO.UpdateDimProdSpec(dimProdSpecTO);
        }

        public int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimProdSpecDAO.UpdateDimProdSpec(dimProdSpecTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimProdSpec(Int32 idProdSpec)
        {
            return _iDimProdSpecDAO.DeleteDimProdSpec(idProdSpec);
        }

        public int DeleteDimProdSpec(Int32 idProdSpec, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimProdSpecDAO.DeleteDimProdSpec(idProdSpec, conn, tran);
        }

        #endregion
        
    }
}
