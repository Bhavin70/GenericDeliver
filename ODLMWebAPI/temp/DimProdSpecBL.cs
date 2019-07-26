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
    public class DimProdSpecBL : IDimProdSpecBL
    {
        #region Selection
        public List<DimProdSpecTO> SelectAllDimProdSpecList()
        {
            return  DimProdSpecDAO.SelectAllDimProdSpec();
        }

        public DimProdSpecTO SelectDimProdSpecTO(Int32 idProdSpec)
        {
            return  DimProdSpecDAO.SelectDimProdSpec(idProdSpec);
        }

       

        #endregion
        
        #region Insertion
        public int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO)
        {
            return DimProdSpecDAO.InsertDimProdSpec(dimProdSpecTO);
        }

        public int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdSpecDAO.InsertDimProdSpec(dimProdSpecTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO)
        {
            return DimProdSpecDAO.UpdateDimProdSpec(dimProdSpecTO);
        }

        public int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdSpecDAO.UpdateDimProdSpec(dimProdSpecTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimProdSpec(Int32 idProdSpec)
        {
            return DimProdSpecDAO.DeleteDimProdSpec(idProdSpec);
        }

        public int DeleteDimProdSpec(Int32 idProdSpec, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdSpecDAO.DeleteDimProdSpec(idProdSpec, conn, tran);
        }

        #endregion
        
    }
}
