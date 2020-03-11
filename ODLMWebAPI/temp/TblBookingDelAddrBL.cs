using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblBookingDelAddrBL : ITblBookingDelAddrBL
    {
        #region Selection
       
        public List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrList()
        {
            return TblBookingDelAddrDAO.SelectAllTblBookingDelAddr();
        }

        public List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrList(int bookingId)
        {
            return TblBookingDelAddrDAO.SelectAllTblBookingDelAddr(bookingId);
        }

        public TblBookingDelAddrTO SelectTblBookingDelAddrTO(Int32 idBookingDelAddr)
        {
            return  TblBookingDelAddrDAO.SelectTblBookingDelAddr(idBookingDelAddr);
           
        }

        public List<TblBookingDelAddrTO> SelectDeliveryAddrListFromDealer(Int32 addrSourceTypeId, Int32 entityId)
        {
            List<TblBookingDelAddrTO> list = new List<TblBookingDelAddrTO>();
            List<TblBookingDelAddrTO> templist = new List<TblBookingDelAddrTO>();
            if (addrSourceTypeId == (int)Constants.AddressSourceTypeE.FROM_BOOKINGS)
            {
                List<TblBookingScheduleTO> listTemp = TblBookingScheduleBL.SelectAllTblBookingScheduleList(entityId);

                if (listTemp != null && listTemp.Count > 0)
                {
                    listTemp = listTemp.OrderBy(o => o.ScheduleDate).ToList();

                    for (int k = 0; k < listTemp.Count; k++)
                    {
                        TblBookingScheduleTO tblBookingScheduleTO = listTemp[k];

                         templist = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);
                         list.AddRange(templist);
                    }
                }
                //list = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrList(entityId);
               

            }
            else
            {
                list = new List<TblBookingDelAddrTO>();
                Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS;
                TblAddressTO tblAddressTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(entityId, addressTypeE);
                if (tblAddressTO == null)
                    return null;

                TblBookingDelAddrTO bookingDelAddrTO = new TblBookingDelAddrTO();
                String address = string.Empty;
                if (!string.IsNullOrEmpty(tblAddressTO.PlotNo))
                    address += tblAddressTO.PlotNo + ",";
                if (!string.IsNullOrEmpty(tblAddressTO.StreetName))
                    address += tblAddressTO.StreetName + ",";
                if (!string.IsNullOrEmpty(tblAddressTO.AreaName))
                    address += tblAddressTO.AreaName + ",";
                if (!string.IsNullOrEmpty(tblAddressTO.VillageName))
                    address += tblAddressTO.VillageName + ",";

                bookingDelAddrTO.Address = address;
                bookingDelAddrTO.BillingName = DAL.TblOrganizationDAO.SelectFirmNameOfOrganiationById(entityId);
                bookingDelAddrTO.DistrictName = tblAddressTO.DistrictName;
                bookingDelAddrTO.TalukaName = tblAddressTO.TalukaName;
                bookingDelAddrTO.VillageName = tblAddressTO.VillageName;
                bookingDelAddrTO.StateId = tblAddressTO.StateId;
                bookingDelAddrTO.State = tblAddressTO.StateName;

                List<TblOrgLicenseDtlTO> licList = BL.TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(entityId);
                if (licList != null)
                {
                    var gstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                    if (gstNoTO == null || string.IsNullOrEmpty(gstNoTO.LicenseValue))
                    {
                        var tempGstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.SGST_NO).FirstOrDefault();
                        if (tempGstNoTO != null && !string.IsNullOrEmpty(tempGstNoTO.LicenseValue))
                        {
                            if (tempGstNoTO.LicenseValue != "0")
                                bookingDelAddrTO.GstNo = tempGstNoTO.LicenseValue;
                        }
                    }
                    else if (gstNoTO.LicenseValue == "0")
                    {
                        var tempGstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.SGST_NO).FirstOrDefault();
                        if (tempGstNoTO != null && !string.IsNullOrEmpty(tempGstNoTO.LicenseValue))
                        {
                            if (tempGstNoTO.LicenseValue != "0")
                                bookingDelAddrTO.GstNo = tempGstNoTO.LicenseValue;
                        }
                    }
                    else
                        bookingDelAddrTO.GstNo = gstNoTO.LicenseValue;

                    var panTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                    if (panTO != null && !string.IsNullOrEmpty(panTO.LicenseValue))
                    {
                        if (panTO.LicenseValue != "0")
                            bookingDelAddrTO.PanNo = panTO.LicenseValue;
                    }
                }

                list.Add(bookingDelAddrTO);
            }

            return list;
        }

        /// <summary>
        /// [15-12-2017]Vijaymala:Added to get booking delivery address list according to schedule
        /// 
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId)
        {
            return TblBookingDelAddrDAO.SelectAllTblBookingDelAddrListBySchedule(scheduleId);
        }

        public List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId,SqlConnection conn,SqlTransaction tran)
        {
            return TblBookingDelAddrDAO.SelectAllTblBookingDelAddrListBySchedule(scheduleId,conn,tran);
        }
       
        
        /// <summary>
        ///  Priyanka [14-12-2018] : Added to get the existing booking address from existing bookings.
        /// </summary>
        /// <param name="dealerOrgId"></param>
        /// <returns></returns>
        public List<TblBookingDelAddrTO> SelectExistingBookingAddrListByDealerId(Int32 dealerOrgId,String txnAddrTypeId)
        {
            Int32 cnt = 0;
            String txnAddrTypeIdtemp = String.Empty;

            if (!String.IsNullOrEmpty(txnAddrTypeId))
            {
                txnAddrTypeIdtemp = txnAddrTypeId;
            }
          
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_EXISTING_ADDRESS_COUNT_FOR_BOOKING);
            if (tblConfigParamsTO != null)
            {
                cnt = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            }

            List<TblBookingDelAddrTO> tblBookingDelAddrTOList = new List<TblBookingDelAddrTO>();
            try
            {
                List<TblBookingDelAddrTO> tblBookingDelAddrTOListtemp = new List<TblBookingDelAddrTO>();
                tblBookingDelAddrTOList = DAL.TblBookingDelAddrDAO.SelectTblBookingsByDealerOrgId(dealerOrgId, txnAddrTypeIdtemp);
                tblBookingDelAddrTOList = tblBookingDelAddrTOList.Where(ele => ele.BillingName != null || ele.Address != null).ToList();
                tblBookingDelAddrTOList = tblBookingDelAddrTOList.GroupBy(c => new { c.BillingName , c.Address }).Select(s => s.FirstOrDefault()).ToList();
                if (cnt > tblBookingDelAddrTOList.Count)
                {
                    tblBookingDelAddrTOListtemp= tblBookingDelAddrTOList;
                }
                else
                {
                    for (int i = 0; i < cnt; i++)
                    {
                            TblBookingDelAddrTO tblBookingDelAddrTO = tblBookingDelAddrTOList[i];
                            tblBookingDelAddrTOListtemp.Add(tblBookingDelAddrTO);
                    }
                }
                return tblBookingDelAddrTOListtemp;
            }
            //List<TblBookingDelAddrTO> tblBookingDelAddrTOList = new List<TblBookingDelAddrTO>();
            //try
            //{
            //    List<TblBookingsTO> bookingList = DAL.TblBookingsDAO.SelectTblBookingsByDealerOrgId(dealerOrgId);
            //    if (bookingList != null && bookingList.Count > 0)
            //    {
            //        for (int i = 0; i < bookingList.Count; i++)
            //        {
            //            TblBookingsTO tblBookingsTO = bookingList[i];
            //            List<TblBookingScheduleTO> tblBookingScheduleTOList = BL.TblBookingScheduleBL.SelectBookingScheduleByBookingId(tblBookingsTO.IdBooking);
            //            if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0)
            //            {
            //                for (int j = 0; j < tblBookingScheduleTOList.Count; j++)
            //                {
            //                    TblBookingScheduleTO tblBookingScheduleTO = tblBookingScheduleTOList[j];
            //                    if (tblBookingScheduleTO.DeliveryAddressLst != null && tblBookingScheduleTO.DeliveryAddressLst.Count > 0)
            //                    {
            //                        tblBookingDelAddrTOList.AddRange(tblBookingScheduleTO.DeliveryAddressLst);
            //                        tblBookingDelAddrTOList = tblBookingDelAddrTOList.GroupBy(c => c.BillingOrgId).Select(s => s.FirstOrDefault()).ToList();
            //                        tblBookingDelAddrTOList = tblBookingDelAddrTOList.Where(ele => ele.BillingName != null || ele.Address != null).ToList();
            //                    }

            //                }
            //            }
            //        }
            //    }
            //    return tblBookingDelAddrTOList;
            //}
            catch (Exception ex)
            {
                return null;
            }

        }


        #endregion

        #region Insertion
        public int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO)
        {
            return TblBookingDelAddrDAO.InsertTblBookingDelAddr(tblBookingDelAddrTO);
        }

        public int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingDelAddrDAO.InsertTblBookingDelAddr(tblBookingDelAddrTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO)
        {
            return TblBookingDelAddrDAO.UpdateTblBookingDelAddr(tblBookingDelAddrTO);
        }

        public int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingDelAddrDAO.UpdateTblBookingDelAddr(tblBookingDelAddrTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingDelAddr(Int32 idBookingDelAddr)
        {
            return TblBookingDelAddrDAO.DeleteTblBookingDelAddr(idBookingDelAddr);
        }

        public int DeleteTblBookingDelAddr(Int32 idBookingDelAddr, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingDelAddrDAO.DeleteTblBookingDelAddr(idBookingDelAddr, conn, tran);
        }

        public int DeleteTblBookingDelAddrByScheduleId(Int32 scheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingDelAddrDAO.DeleteTblBookingDelAddrByScheduleId(scheduleId, conn, tran);
        }
        #endregion

    }
}