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
    public class DimTranActionTypeBL : IDimTranActionTypeBL
    {
        #region Selection
        public List<DimTranActionTypeTO> SelectAllDimTranActionType()
        {
            return DimTranActionTypeDAO.SelectAllDimTranActionType();
        }

        //public static List<DimTranActionTypeTO> SelectAllDimTranActionTypeList()
        //{
        //    return DimTranActionTypeDAO.SelectAllDimTranActionType();
        //}

        public DimTranActionTypeTO SelectDimTranActionTypeTO(Int32 idTranActionType)
        {
            return DimTranActionTypeBL.SelectDimTranActionTypeTO(idTranActionType);
        }

        

        #endregion
        
        #region Insertion
        public int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO)
        {
            return DimTranActionTypeDAO.InsertDimTranActionType(dimTranActionTypeTO);
        }

        public int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimTranActionTypeDAO.InsertDimTranActionType(dimTranActionTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO)
        {
            return DimTranActionTypeDAO.UpdateDimTranActionType(dimTranActionTypeTO);
        }

        public int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimTranActionTypeDAO.UpdateDimTranActionType(dimTranActionTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimTranActionType(Int32 idTranActionType)
        {
            return DimTranActionTypeDAO.DeleteDimTranActionType(idTranActionType);
        }

        public int DeleteDimTranActionType(Int32 idTranActionType, SqlConnection conn, SqlTransaction tran)
        {
            return DimTranActionTypeDAO.DeleteDimTranActionType(idTranActionType, conn, tran);
        }

        #endregion
        
    }
}
