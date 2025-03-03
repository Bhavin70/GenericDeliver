using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.IoT.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblWeighingBL : ITblWeighingBL
    {
        private readonly ITblWeighingDAO _iTblWeighingDAO;
        private readonly ITblWeighingMachineDAO _iTblWeighingMachineDAO;
        private readonly IGateCommunication _iGateCommunication;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ICommon _iCommon;
        public TblWeighingBL(ICommon iCommon, ITblWeighingDAO iTblWeighingDAO, ITblConfigParamsDAO iTblConfigParamsDAO, ITblWeighingMachineDAO iTblWeighingMachineDAO, IGateCommunication iGateCommunication)
        {
            _iTblWeighingDAO = iTblWeighingDAO;
            _iCommon = iCommon;
            _iTblWeighingMachineDAO = iTblWeighingMachineDAO;
            _iGateCommunication = iGateCommunication;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }

        #region Selection
        public List<TblWeighingTO> SelectAllTblWeighing()
        {
            return _iTblWeighingDAO.SelectAllTblWeighing();
        }

        

        public TblWeighingTO SelectTblWeighingTO(Int32 idWeighing)
        {
            return _iTblWeighingDAO.SelectTblWeighing(idWeighing);
        }

        public TblWeighingTO SelectTblWeighingByMachineIp(string ipAddr, int machineId)
        {
            //TblWeighingTO tblWeighingTO = new TblWeighingTO();
            //DateTime serverDateTime = _iCommon.ServerDateTime;
            //DateTime defaultTime1 = serverDateTime.AddHours(15);
            //tblWeighingTO = _iTblWeighingDAO.SelectTblWeighingByMachineIp(ipAddr, defaultTime1);
            //if (tblWeighingTO == null)
            //{
            //    return null;
            //}
            ////DateTime dt = DateTime.Now.AddMinutes(-10);
            //TimeSpan CurrentdateTime = serverDateTime.TimeOfDay;
            //TimeSpan weighingTime = tblWeighingTO.TimeStamp.TimeOfDay;
            ////TimeSpan diffTime = CurrentdateTime - toDateTime;
            //TimeSpan defaultTime = CurrentdateTime.Add(new TimeSpan(-2, -30, -30));
            //if (weighingTime == TimeSpan.Zero || weighingTime < defaultTime)
            //{
            //    return null;
            //}
            //else
            //{
            //    DeleteTblWeighingByByMachineIp(ipAddr);
            //}

            //return tblWeighingTO;

            TblWeighingTO tblWeighingTO = new TblWeighingTO();
            int confiqId = _iTblConfigParamsDAO.IoTSetting();
            if (machineId > 0 && confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                TblWeighingMachineTO tblWeighingMachineTO = _iTblWeighingMachineDAO.SelectTblWeighingMachine(machineId);
                if (tblWeighingMachineTO == null)
                {
                    return null;
                }
                string weight = _iGateCommunication.ReadWeightFromWeightIoT(tblWeighingMachineTO);
                tblWeighingTO.Measurement = weight;
                tblWeighingTO.MachineIp = tblWeighingMachineTO.MachineIP;
                tblWeighingTO.TimeStamp = _iCommon.ServerDateTime;
                return tblWeighingTO;
            }
            DateTime serverDateTime = _iCommon.ServerDateTime;
            DateTime defaultTime1 = serverDateTime.AddHours(15);
            tblWeighingTO = _iTblWeighingDAO.SelectTblWeighingByMachineIp(ipAddr, defaultTime1);
            if (tblWeighingTO == null)
            {
                return null;
            }
            TimeSpan CurrentdateTime = serverDateTime.TimeOfDay;
            TimeSpan weighingTime = tblWeighingTO.TimeStamp.TimeOfDay;
            TimeSpan defaultTime = CurrentdateTime.Add(new TimeSpan(-2, -30, -30));
            if (weighingTime == TimeSpan.Zero || weighingTime < defaultTime)
            {
                return null;
            }
            else
            {
                DeleteTblWeighingByByMachineIp(ipAddr);
            }

            return tblWeighingTO;

        }


        #endregion

        #region Insertion
        public int InsertTblWeighing(TblWeighingTO tblWeighingTO)
        {
            return _iTblWeighingDAO.InsertTblWeighing(tblWeighingTO);
        }

        public int InsertTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingDAO.InsertTblWeighing(tblWeighingTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblWeighing(TblWeighingTO tblWeighingTO)
        {
            return _iTblWeighingDAO.UpdateTblWeighing(tblWeighingTO);
        }

        public int UpdateTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingDAO.UpdateTblWeighing(tblWeighingTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblWeighing(Int32 idWeighing)
        {
            return _iTblWeighingDAO.DeleteTblWeighing(idWeighing);
        }
        /// <summary>
        /// GJ@20170830 : Remove the all previous weighing measured records from tables againest IpAddr
        /// </summary>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        public int DeleteTblWeighingByByMachineIp(string ipAddr)
        {
            try
            {              
                return _iTblWeighingDAO.DeleteTblWeighingByByMachineIp(ipAddr);                
            }
            catch (Exception)
            {
                return 0;
            }
            
        }

        public int DeleteTblWeighing(Int32 idWeighing, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingDAO.DeleteTblWeighing(idWeighing, conn, tran);
        }

        #endregion
        
    }
}
