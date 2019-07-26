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
    public class DimProdSpecDescBL : IDimProdSpecDescBL
    {

        #region Selection

           public List<DimProdSpecDescTO> SelectAllDimProdSpecDescList()
            {                
                return DimProdSpecDescDAO.SelectAllDimProdSpecDesc();
            }

            public DimProdSpecDescTO SelectDimPRodSpecDescTO(Int32 idCodeType)
            {
            return DimProdSpecDescDAO.SelectDimProdSpecDesc(idCodeType);              
            }

        /// <summary>
        /// Added by vinod Dated:12/12/2017 for the select of max record from the product Specification 
        /// </summary>
        /// <returns></returns>
        /// 

        public int SelectAllDimProdSpecDescriptionList()
        {           
            return DimProdSpecDescDAO.SelectDimProdSpecDescription();           
        }

        #endregion

        #region Insertion
        public int InsertDimProdSpecDesc(DimProdSpecDescTO ProSpecDesc)
            {
                return DimProdSpecDescDAO.InsertDimProdSpecDesc(ProSpecDesc);              
            }

            public int InsertDimProdSpecDesc(DimProdSpecDescTO dimProSpecDescTO, SqlConnection conn, SqlTransaction tran)
            {
                return DimProdSpecDescDAO.InsertDimProdSpecDesc(dimProSpecDescTO, conn, tran);               
            }

            #endregion

            #region Updation
            public int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO)
            {
                return DimProdSpecDescDAO.UpdateDimProdSpecDesc(dimProdSpecDescTO);
            }
            public int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
            {
               return DimProdSpecDescDAO.UpdateDimProdSpecDesc(dimProdSpecDescTO, conn,tran);            
            }

            #endregion

            #region Deletion
            public int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO)
            {
            return DimProdSpecDescDAO.UpdateDimProdSpecDescription(DimProdSpecDescTO);            
            }

            public int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
            {
            return DimProdSpecDescDAO.UpdateDimProdSpecDescription(DimProdSpecDescTO, conn, tran);
            }

            #endregion

    }
}