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
    public class DimVehicleTypeBL : IDimVehicleTypeBL
    {
        private readonly IDimVehicleTypeDAO _iDimVehicleTypeDAO;
        public DimVehicleTypeBL(IDimVehicleTypeDAO iDimVehicleTypeDAO)
        {
            _iDimVehicleTypeDAO = iDimVehicleTypeDAO;
        }
        #region Selection

        public List<DimVehicleTypeTO> SelectAllDimVehicleTypeList()
        {
            return _iDimVehicleTypeDAO.SelectAllDimVehicleType();
        }

        public DimVehicleTypeTO SelectDimVehicleTypeTO(Int32 idVehicleType)
        {
            return _iDimVehicleTypeDAO.SelectDimVehicleType(idVehicleType);
        }

        

        #endregion
        
        #region Insertion
        public int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
            return _iDimVehicleTypeDAO.InsertDimVehicleType(dimVehicleTypeTO);
        }

        public int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehicleTypeDAO.InsertDimVehicleType(dimVehicleTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
            return _iDimVehicleTypeDAO.UpdateDimVehicleType(dimVehicleTypeTO);
        }

        public int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehicleTypeDAO.UpdateDimVehicleType(dimVehicleTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimVehicleType(Int32 idVehicleType)
        {
            return _iDimVehicleTypeDAO.DeleteDimVehicleType(idVehicleType);
        }

        public int DeleteDimVehicleType(Int32 idVehicleType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehicleTypeDAO.DeleteDimVehicleType(idVehicleType, conn, tran);
        }

        #endregion
        
    }
}
