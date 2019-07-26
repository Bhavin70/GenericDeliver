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
    public class DimUnitMeasuresBL : IDimUnitMeasuresBL
    {
        private readonly IDimUnitMeasuresDAO _iDimUnitMeasuresDAO;
        public DimUnitMeasuresBL(IDimUnitMeasuresDAO iDimUnitMeasuresDAO)
        {
            _iDimUnitMeasuresDAO = iDimUnitMeasuresDAO;
        }
        #region Selection
        public List<DimUnitMeasuresTO> SelectAllDimUnitMeasuresList()
        {
            return _iDimUnitMeasuresDAO.SelectAllDimUnitMeasures();
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] Added to select all measurement units for drop down
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectAllUnitMeasuresListForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> list = _iDimUnitMeasuresDAO.SelectAllUnitMeasuresForDropDown();

                if (list != null)
                {
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresListForDropDown");
                return null;
            }
        }
        /// <summary>
        /// Kiran [08-Sep-2018] Added to select all measurement units for drop down Using CatId
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectAllUnitMeasuresForDropDownByCatId(Int32 unitCatId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> list = _iDimUnitMeasuresDAO.SelectAllUnitMeasuresForDropDownByCatId(unitCatId);

                if (list != null)
                {
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresForDropDownByCatId");
                return null;
            }
        }
        

        public DimUnitMeasuresTO SelectDimUnitMeasuresTO(Int32 idWeightMeasurUnit)
        {
            return _iDimUnitMeasuresDAO.SelectDimUnitMeasures(idWeightMeasurUnit);
        }

        #endregion

        #region Insertion
        public int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO)
        {
            return _iDimUnitMeasuresDAO.InsertDimUnitMeasures(dimUnitMeasuresTO);
        }

        public int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUnitMeasuresDAO.InsertDimUnitMeasures(dimUnitMeasuresTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO)
        {
            return _iDimUnitMeasuresDAO.UpdateDimUnitMeasures(dimUnitMeasuresTO);
        }

        public int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUnitMeasuresDAO.UpdateDimUnitMeasures(dimUnitMeasuresTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit)
        {
            return _iDimUnitMeasuresDAO.DeleteDimUnitMeasures(idWeightMeasurUnit);
        }

        public int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUnitMeasuresDAO.DeleteDimUnitMeasures(idWeightMeasurUnit, conn, tran);
        }

        #endregion

    }
}
