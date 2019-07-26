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
    public class DimTranActionTypeBL : IDimTranActionTypeBL
    {
        private readonly IDimTranActionTypeDAO _iDimTranActionTypeDAO;
        public DimTranActionTypeBL(IDimTranActionTypeDAO iDimTranActionTypeDAO)
        {
            _iDimTranActionTypeDAO = iDimTranActionTypeDAO;
        }
        #region Selection
        public List<DimTranActionTypeTO> SelectAllDimTranActionType()
        {
            return _iDimTranActionTypeDAO.SelectAllDimTranActionType();
        }

        //public List<DimTranActionTypeTO> SelectAllDimTranActionTypeList()
        //{
        //    return _iDimTranActionTypeDAO.SelectAllDimTranActionType();
        //}

        public DimTranActionTypeTO SelectDimTranActionTypeTO(Int32 idTranActionType)
        {
            return SelectDimTranActionTypeTO(idTranActionType);
        }

        #endregion
        
        #region Insertion
        public int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO)
        {
            return _iDimTranActionTypeDAO.InsertDimTranActionType(dimTranActionTypeTO);
        }

        public int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimTranActionTypeDAO.InsertDimTranActionType(dimTranActionTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO)
        {
            return _iDimTranActionTypeDAO.UpdateDimTranActionType(dimTranActionTypeTO);
        }

        public int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimTranActionTypeDAO.UpdateDimTranActionType(dimTranActionTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimTranActionType(Int32 idTranActionType)
        {
            return _iDimTranActionTypeDAO.DeleteDimTranActionType(idTranActionType);
        }

        public int DeleteDimTranActionType(Int32 idTranActionType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimTranActionTypeDAO.DeleteDimTranActionType(idTranActionType, conn, tran);
        }

        #endregion
        
    }
}
