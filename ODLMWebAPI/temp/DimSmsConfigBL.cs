using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class DimSmsConfigBL : IDimSmsConfigBL
    {
        #region Selection
        //public static DataTable SelectAllDimSmsConfig()
        //{
        //    return DimSmsConfigDAO.SelectAllDimSmsConfig();
        //}

        public DimSmsConfigTO SelectAllDimSmsConfigList()
        {
            DimSmsConfigTO dimSmsConfigTO = DimSmsConfigDAO.SelectAllDimSmsConfig();
            return dimSmsConfigTO;
        }

        //public static DimSmsConfigTO SelectDimSmsConfigTO()
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
        //public static int InsertDimSmsConfig(DimSmsConfigTO dimSmsConfigTO)
        //{
        //    return DimSmsConfigDAO.InsertDimSmsConfig(dimSmsConfigTO);
        //}

        //public static int InsertDimSmsConfig(DimSmsConfigTO dimSmsConfigTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    return DimSmsConfigDAO.InsertDimSmsConfig(dimSmsConfigTO, conn, tran);
        //}

        #endregion

        #region Updation
        //public static int UpdateDimSmsConfig(DimSmsConfigTO dimSmsConfigTO)
        //{
        //    return DimSmsConfigDAO.UpdateDimSmsConfig(dimSmsConfigTO);
        //}

        //public static int UpdateDimSmsConfig(DimSmsConfigTO dimSmsConfigTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    return DimSmsConfigDAO.UpdateDimSmsConfig(dimSmsConfigTO, conn, tran);
        //}

        #endregion

        #region Deletion
        //public static int DeleteDimSmsConfig()
        //{
        //    return DimSmsConfigDAO.DeleteDimSmsConfig();
        //}

        //public static int DeleteDimSmsConfig(, SqlConnection conn, SqlTransaction tran)
        //{
        //    return DimSmsConfigDAO.DeleteDimSmsConfig(, conn, tran);
        //}

        #endregion


    }
}
