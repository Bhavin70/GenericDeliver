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
    public class DimProdSpecDescBL : IDimProdSpecDescBL
    {
        private readonly IDimProdSpecDescDAO _iDimProdSpecDescDAO;
        public DimProdSpecDescBL(IDimProdSpecDescDAO iDimProdSpecDescDAO)
        {
            _iDimProdSpecDescDAO = iDimProdSpecDescDAO;
        }
        #region Selection

        public List<DimProdSpecDescTO> SelectAllDimProdSpecDescList()
            {                
                return _iDimProdSpecDescDAO.SelectAllDimProdSpecDesc();
            }

            public DimProdSpecDescTO SelectDimPRodSpecDescTO(Int32 idCodeType)
            {
            return _iDimProdSpecDescDAO.SelectDimProdSpecDesc(idCodeType);              
            }

        /// <summary>
        /// Added by vinod Dated:12/12/2017 for the select of max record from the product Specification 
        /// </summary>
        /// <returns></returns>
        /// 

        public int SelectAllDimProdSpecDescriptionList()
        {           
            return _iDimProdSpecDescDAO.SelectDimProdSpecDescription();           
        }
        public List<TblPipesTO> SelectAllTblPipesList(DateTime createdOn) 
        {
            return _iDimProdSpecDescDAO.SelectAllTblPipes(createdOn);
        }
        public List<TblStripsTO> SelectAllTblStripsList(DateTime createdOn)
        {
            return _iDimProdSpecDescDAO.SelectAllTblStrips(createdOn);
        }

        //public List<TblPipesDropDownTo> GetAllPipesInchForDropDown()
        //{
        //    return _iDimProdSpecDescDAO.SelectAllPipesInchForDropDown();
        //}
        public List<TblPipesStripCommonSizeTO> GetAlltblPipesStripCommonSizeForDropDown()
        {
            return _iDimProdSpecDescDAO.SelectAlltblPipesStripCommonSizeForDropDown();
        }
        public List<TblPipesStripCommonThicknessTO> GetAlltblPipesStripCommonThicknessForDropDown()
        {
            return _iDimProdSpecDescDAO.SelectAlltblPipesStripCommonThicknessForDropDown();
        }
        public List<TblStripsGradeDropDownTo> GetAlltblStripsGradeForDropDown()
        {
            return _iDimProdSpecDescDAO.SelectAlltblStripsGradeForDropDown();
        }
        public List<TblPipesStripCommonQuantityTO> GetAlltblPipesStripCommonQuantityForDropDown()
        {
            return _iDimProdSpecDescDAO.SelectAlltblPipesStripCommonQuantityForDropDown();
        }
        public List<TblInchDropDownTO> GetTblInchForDropDown()
        {
            return _iDimProdSpecDescDAO.SelectAllTblInchForDropDown();
        }
        public List<TblSizeTO> GetTblSizeForDropDown(Int32 idInch = 0)
        {
            return _iDimProdSpecDescDAO.SelectAllTblSizeForDropDown(idInch);
        }
        public List<TblThicknessDropDownTO> GetTblThicknessForDropDown()
        {
            return _iDimProdSpecDescDAO.SelectAllTblThicknessForDropDown();
        }

        public List<TblWidthDropDownTO> GetTblWidthForDropDown()
        {
            return _iDimProdSpecDescDAO.SelectAllTblWidthForDropDown();
        }
        #endregion

        #region Insertion
        public int InsertDimProdSpecDesc(DimProdSpecDescTO ProSpecDesc)
            {
                return _iDimProdSpecDescDAO.InsertDimProdSpecDesc(ProSpecDesc);              
            }

            public int InsertDimProdSpecDesc(DimProdSpecDescTO dimProSpecDescTO, SqlConnection conn, SqlTransaction tran)
            {
                return _iDimProdSpecDescDAO.InsertDimProdSpecDesc(dimProSpecDescTO, conn, tran);               
            }

            #endregion

            #region Updation
            public int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO)
            {
                return _iDimProdSpecDescDAO.UpdateDimProdSpecDesc(dimProdSpecDescTO);
            }
            public int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
            {
               return _iDimProdSpecDescDAO.UpdateDimProdSpecDesc(dimProdSpecDescTO, conn,tran);            
            }

            #endregion

            #region Deletion
            public int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO)
            {
            return _iDimProdSpecDescDAO.UpdateDimProdSpecDescription(DimProdSpecDescTO);            
            }

            public int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
            {
            return _iDimProdSpecDescDAO.UpdateDimProdSpecDescription(DimProdSpecDescTO, conn, tran);
            }

        public List<DropDownTO> SelectAllPipesInchForDropDown()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}