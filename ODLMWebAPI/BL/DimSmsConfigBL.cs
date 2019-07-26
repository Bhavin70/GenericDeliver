using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class DimSmsConfigBL : IDimSmsConfigBL
    {
        private readonly IDimSmsConfigDAO _iDimSmsConfigDAO;
        public DimSmsConfigBL(IDimSmsConfigDAO iDimSmsConfigDAO)
        {
            _iDimSmsConfigDAO = iDimSmsConfigDAO;
        }
        #region Selection
        //public DataTable SelectAllDimSmsConfig()
        //{
        //    return DimSmsConfigDAO.SelectAllDimSmsConfig();
        //}

        public DimSmsConfigTO SelectAllDimSmsConfigList()
        {
            DimSmsConfigTO dimSmsConfigTO = _iDimSmsConfigDAO.SelectAllDimSmsConfig();
            return dimSmsConfigTO;
        }

        //public DimSmsConfigTO SelectDimSmsConfigTO()
        //{
        //    DataTable dimSmsConfigTODT = DimSmsConfigDAO.SelectDimSmsConfig();
        //    List<DimSmsConfigTO> dimSmsConfigTOList = ConvertDTToList(dimSmsConfigTODT);
        //    if (dimSmsConfigTOList != null && dimSmsConfigTOList.Count == 1)
        //        return dimSmsConfigTOList[0];
        //    else
        //        return null;
        //}

       

        #endregion

        #region Insertion
        //public int InsertDimSmsConfig(DimSmsConfigTO dimSmsConfigTO)
        //{
        //    return DimSmsConfigDAO.InsertDimSmsConfig(dimSmsConfigTO);
        //}

        //public int InsertDimSmsConfig(DimSmsConfigTO dimSmsConfigTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    return DimSmsConfigDAO.InsertDimSmsConfig(dimSmsConfigTO, conn, tran);
        //}

        #endregion

        #region Updation
        //public int UpdateDimSmsConfig(DimSmsConfigTO dimSmsConfigTO)
        //{
        //    return DimSmsConfigDAO.UpdateDimSmsConfig(dimSmsConfigTO);
        //}

        //public int UpdateDimSmsConfig(DimSmsConfigTO dimSmsConfigTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    return DimSmsConfigDAO.UpdateDimSmsConfig(dimSmsConfigTO, conn, tran);
        //}

        #endregion

        #region Deletion
        //public int DeleteDimSmsConfig()
        //{
        //    return DimSmsConfigDAO.DeleteDimSmsConfig();
        //}

        //public int DeleteDimSmsConfig(, SqlConnection conn, SqlTransaction tran)
        //{
        //    return DimSmsConfigDAO.DeleteDimSmsConfig(, conn, tran);
        //}

        #endregion


    }
}
