using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{  
    public class TblTransportSlipBL : ITblTransportSlipBL
    {
        private readonly ITblTransportSlipDAO _iTblTransportSlipDAO;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly IConnectionString _iConnectionString;
        public TblTransportSlipBL(IConnectionString iConnectionString, ITblTransportSlipDAO iTblTransportSlipDAO, ITblAlertInstanceBL iTblAlertInstanceBL)
        {
            _iTblTransportSlipDAO = iTblTransportSlipDAO;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblTransportSlipTO> SelectAllTblTransportSlip(DateTime tDate, int isLink)
        {
            return _iTblTransportSlipDAO.SelectAllTblTransportSlip(tDate, isLink);
        }

        public List<TblTransportSlipTO> SelectAllTblTransportSlipList(DateTime tDate,int isLink)
        {
             List<TblTransportSlipTO> tblTransportSlipTODT = _iTblTransportSlipDAO.SelectAllTblTransportSlip(tDate, isLink);
            if(tblTransportSlipTODT != null)
            {
                return tblTransportSlipTODT;
            }
            return null;
        }

        public TblTransportSlipTO SelectTblTransportSlipTO(Int32 idTransportSlip)
        {
            TblTransportSlipTO tblTransportSlipTODT = _iTblTransportSlipDAO.SelectTblTransportSlip(idTransportSlip);
            if (tblTransportSlipTODT != null)
                return tblTransportSlipTODT;
            else
                return null;
        }

        //public List<TblTransportSlipTO> ConvertDTToList(DataTable tblTransportSlipTODT)
        //{
        //    List<TblTransportSlipTO> tblTransportSlipTOList = new List<TblTransportSlipTO>();
        //    if (tblTransportSlipTODT != null)
        //    {
        //        for (int rowCount = 0; rowCount < tblTransportSlipTODT.Rows.Count; rowCount++)
        //        {
        //            TblTransportSlipTO tblTransportSlipTONew = new TblTransportSlipTO();
        //            if (tblTransportSlipTODT.Rows[rowCount]["idTransportSlip"] != DBNull.Value)
        //                tblTransportSlipTONew.IdTransportSlip = Convert.ToInt32(tblTransportSlipTODT.Rows[rowCount]["idTransportSlip"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["partyOrgId"] != DBNull.Value)
        //                tblTransportSlipTONew.PartyOrgId = Convert.ToInt32(tblTransportSlipTODT.Rows[rowCount]["partyOrgId"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["transporterOrgId"] != DBNull.Value)
        //                tblTransportSlipTONew.TransporterOrgId = Convert.ToInt32(tblTransportSlipTODT.Rows[rowCount]["transporterOrgId"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["vehicleTypeId"] != DBNull.Value)
        //                tblTransportSlipTONew.VehicleTypeId = Convert.ToInt32(tblTransportSlipTODT.Rows[rowCount]["vehicleTypeId"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["isFromDealer"] != DBNull.Value)
        //                tblTransportSlipTONew.IsFromDealer = Convert.ToInt32(tblTransportSlipTODT.Rows[rowCount]["isFromDealer"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
        //                tblTransportSlipTONew.CreatedBy = Convert.ToInt32(tblTransportSlipTODT.Rows[rowCount]["createdBy"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["updatedBy"] != DBNull.Value)
        //                tblTransportSlipTONew.UpdatedBy = Convert.ToInt32(tblTransportSlipTODT.Rows[rowCount]["updatedBy"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
        //                tblTransportSlipTONew.CreatedOn = Convert.ToDateTime(tblTransportSlipTODT.Rows[rowCount]["createdOn"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["updatedOn"] != DBNull.Value)
        //                tblTransportSlipTONew.UpdatedOn = Convert.ToDateTime(tblTransportSlipTODT.Rows[rowCount]["updatedOn"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["partyName"] != DBNull.Value)
        //                tblTransportSlipTONew.PartyName = Convert.ToString(tblTransportSlipTODT.Rows[rowCount]["partyName"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["transporterName"] != DBNull.Value)
        //                tblTransportSlipTONew.TransporterName = Convert.ToString(tblTransportSlipTODT.Rows[rowCount]["transporterName"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["destination"] != DBNull.Value)
        //                tblTransportSlipTONew.Destination = Convert.ToString(tblTransportSlipTODT.Rows[rowCount]["destination"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["vehicleNo"] != DBNull.Value)
        //                tblTransportSlipTONew.VehicleNo = Convert.ToString(tblTransportSlipTODT.Rows[rowCount]["vehicleNo"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["driverName"] != DBNull.Value)
        //                tblTransportSlipTONew.DriverName = Convert.ToString(tblTransportSlipTODT.Rows[rowCount]["driverName"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["contactNo"] != DBNull.Value)
        //                tblTransportSlipTONew.ContactNo = Convert.ToString(tblTransportSlipTODT.Rows[rowCount]["contactNo"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["comments"] != DBNull.Value)
        //                tblTransportSlipTONew.Comments = Convert.ToString(tblTransportSlipTODT.Rows[rowCount]["comments"].ToString());
        //            if (tblTransportSlipTODT.Rows[rowCount]["refNo"] != DBNull.Value)
        //                tblTransportSlipTONew.RefNo = Convert.ToString(tblTransportSlipTODT.Rows[rowCount]["refNo"].ToString());
        //            tblTransportSlipTOList.Add(tblTransportSlipTONew);
        //        }
        //    }
        //    return tblTransportSlipTOList;
        //}

        #endregion

        #region Insertion
        /// <summary>
        /// KiranM @ Insert Transport Slip 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int InsertTblTransportSlip(TblTransportSlipTO tblTransportSlipTO)
        {
            return _iTblTransportSlipDAO.InsertTblTransportSlip(tblTransportSlipTO);
        }

        public int InsertTblTransportSlip(TblTransportSlipTO tblTransportSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTransportSlipDAO.InsertTblTransportSlip(tblTransportSlipTO, conn, tran);
        }
        public ResultMessage SaveNewtransportSlip(TblTransportSlipTO tblTransportSlipTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                result = InsertTblTransportSlip(tblTransportSlipTO, conn,tran);
                if(result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error SaveNewtransportSlip");
                    return resultMessage;
                }
                else
                {
                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.NEW_TRANSPORT_SLIP_GENERATE;
                    tblAlertInstanceTO.AlertAction = "NEW_TRANSPORT_SLIP_GENERATE";
                    tblAlertInstanceTO.AlertComment =" New Transport slip of Vehicle No " + tblTransportSlipTO.VehicleNo;
                    tblAlertInstanceTO.EffectiveFromDate = tblTransportSlipTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(12);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "New Transport Slip";
                    tblAlertInstanceTO.SourceEntityId = tblTransportSlipTO.IdTransportSlip;
                    tblAlertInstanceTO.RaisedBy = tblTransportSlipTO.CreatedBy;
                    tblAlertInstanceTO.RaisedOn = tblTransportSlipTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 0;
                    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType == ResultMessageE.Information){
                        tran.Commit();
                        resultMessage.DefaultSuccessBehaviour();
                        resultMessage.Tag = tblTransportSlipTO.IdTransportSlip;

                        return resultMessage;
                    }
                    else{
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("SaveNewAlertInstance");
                        return resultMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex,"SaveNewtransportSlip");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public int UpdateTblTransportSlip(TblTransportSlipTO tblTransportSlipTO)
        {
            return _iTblTransportSlipDAO.UpdateTblTransportSlip(tblTransportSlipTO);
        }
        /// <summary>
        /// KiranM @ Update Transport Slip 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResultMessage UpdateNewtransportSlip(TblTransportSlipTO tblTransportSlipTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                result = UpdateTblTransportSlip(tblTransportSlipTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("UpdateTblTransportSlip");
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblTransportSlip");
                return resultMessage;
            }
        }
        public int UpdateTblTransportSlip(TblTransportSlipTO tblTransportSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTransportSlipDAO.UpdateTblTransportSlip(tblTransportSlipTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblTransportSlip(Int32 idTransportSlip)
        {
            return _iTblTransportSlipDAO.DeleteTblTransportSlip(idTransportSlip);
        }

        public int DeleteTblTransportSlip(Int32 idTransportSlip, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTransportSlipDAO.DeleteTblTransportSlip(idTransportSlip, conn, tran);
        }

        #endregion
    }
}
