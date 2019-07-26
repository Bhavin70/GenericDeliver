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
    public class DimVehicleTypeBL : IDimVehicleTypeBL
    {
        #region Selection

        public List<DimVehicleTypeTO> SelectAllDimVehicleTypeList()
        {
            return DimVehicleTypeDAO.SelectAllDimVehicleType();
        }

        public DimVehicleTypeTO SelectDimVehicleTypeTO(Int32 idVehicleType)
        {
            return DimVehicleTypeDAO.SelectDimVehicleType(idVehicleType);
        }

        

        #endregion
        
        #region Insertion
        public int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
            return DimVehicleTypeDAO.InsertDimVehicleType(dimVehicleTypeTO);
        }

        public int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVehicleTypeDAO.InsertDimVehicleType(dimVehicleTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
            return DimVehicleTypeDAO.UpdateDimVehicleType(dimVehicleTypeTO);
        }

        public int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVehicleTypeDAO.UpdateDimVehicleType(dimVehicleTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimVehicleType(Int32 idVehicleType)
        {
            return DimVehicleTypeDAO.DeleteDimVehicleType(idVehicleType);
        }

        public int DeleteDimVehicleType(Int32 idVehicleType, SqlConnection conn, SqlTransaction tran)
        {
            return DimVehicleTypeDAO.DeleteDimVehicleType(idVehicleType, conn, tran);
        }

        #endregion
        
    }
}
