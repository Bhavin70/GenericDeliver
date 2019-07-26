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
using System.Linq;
 
namespace ODLMWebAPI.BL
{  
    public class TblBookingExtBL : ITblBookingExtBL
    {
        private readonly ITblBookingExtDAO _iTblBookingExtDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;

        private readonly ITblParityDetailsDAO _iTblParityDetailsDAO;
        private readonly ITblProductInfoBL _iTblProductInfoBL;
        private readonly ITblBookingsDAO _iTblBookingsDAO;
        public TblBookingExtBL(ITblBookingsDAO iTblBookingsDAO,ITblProductInfoBL iTblProductInfoBL,ITblConfigParamsDAO iTblConfigParamsDAO, ITblBookingExtDAO iTblBookingExtDAO,ITblParityDetailsDAO iTblParityDetailsDAO)
        {
            _iTblBookingExtDAO = iTblBookingExtDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblParityDetailsDAO = iTblParityDetailsDAO;
            _iTblProductInfoBL = iTblProductInfoBL;
            _iTblBookingsDAO = iTblBookingsDAO;
        }
        #region Selection

        public List<TblBookingExtTO> SelectAllTblBookingExtList()
        {
           return _iTblBookingExtDAO.SelectAllTblBookingExt();
      
        }

        public List<TblBookingExtTO> SelectAllTblBookingExtList(Int32 bookingId,Int32 brandId,Int32 prodCatId,Int32 stateId)
        {
            List<TblBookingExtTO>  tblBookingExtTOList =  _iTblBookingExtDAO.SelectAllTblBookingExt(bookingId);
            List<TblProductInfoTO> tblProductInfoTOList  =_iTblProductInfoBL.SelectAllTblProductInfoListLatest();
            TblBookingsTO tblBookingsTO = _iTblBookingsDAO.SelectTblBookings(bookingId);
            if(tblBookingsTO==null)
            {
                return null;
            }
            if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
            {
                // List<TblParityDetailsTO> tblParityDetailsTOList = _iTblParityDetailsDAO.SelectAllParityDetailsOnProductItemId(brandId,0,prodCatId,stateId,1,0,0);
             
                 //if (tblParityDetailsTOList != null && tblParityDetailsTOList.Count > 0)
                 //{
                   
                     for (int i = 0; i < tblBookingExtTOList.Count; i++)
                     {
                        double discount = 0;
                       
                         TblBookingExtTO tempTO = tblBookingExtTOList[i];
                        var parityList = _iTblParityDetailsDAO.SelectParityDetailToListOnBooking(tempTO.MaterialId, tempTO.ProdCatId, tempTO.ProdSpecId, tempTO.ProdItemId, tempTO.BrandId, stateId, tblBookingsTO.CreatedOn);
                        
                        var result= tblProductInfoTOList.Where(a => a.MaterialId == tempTO.MaterialId && a.ProdCatId == tempTO.ProdCatId && a.ProdSpecId == tempTO.ProdSpecId).FirstOrDefault();
                        //var res = tblParityDetailsTOList.Where(a => a.MaterialId == tempTO.MaterialId && a.ProdCatId == tempTO.ProdCatId && a.ProdSpecId == tempTO.ProdSpecId).FirstOrDefault();
                         if(parityList != null && parityList.Count>0)
                         {
                        if (tblBookingsTO.IsConfirmed == 1)
                            tempTO.ParityAmt = parityList[0].ParityAmt;
                        else
                            tempTO.ParityAmt = parityList[0].ParityAmt + parityList[0].NonConfParityAmt;
                               
                         }
                         if(result!=null)
                         {
                            tempTO.AvgBundleWt = result.AvgBundleWt;
                            double itemwiseRate = tempTO.BookedQty*(tempTO.Rate+tempTO.ParityAmt);
                             discount = tempTO.Discount * tempTO.BookedQty * 1000;
                            tempTO.Discount = discount;
                            itemwiseRate = itemwiseRate - discount;
                            tempTO.ItemWiseRate = itemwiseRate;
                          
                            
                         }
                     
                     }
                     
                     
                // }
            }
            
            return tblBookingExtTOList;
        }

        public List<TblBookingExtTO> SelectAllTblBookingExtList(int bookingId)
        {
            List<TblBookingExtTO>  tblBookingExtTOList =  _iTblBookingExtDAO.SelectAllTblBookingExt(bookingId);
            return tblBookingExtTOList;
        }

        public List<TblBookingExtTO> SelectAllTblBookingExtList(int bookingId, SqlConnection conn, SqlTransaction trans)
        {
            return _iTblBookingExtDAO.SelectAllTblBookingExt(bookingId, conn, trans);

        }

        /// <summary>
        /// Sanjay [2017-03-27] This list will return all material alongwith prod cat and spec
        /// Required while showing popup to take material wise ,spec wise qty while booking
        /// </summary>
        /// <param name="prodCatgId"></param>
        /// <returns></returns>
        public List<TblBookingExtTO> SelectEmptyBookingExtList(int prodCatgId,int prodSpecId)
        {

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.LOADING_SLIP_DEFAULT_SPECIFICATION);
            if (tblConfigParamsTO != null)
            {
                Int32.TryParse(tblConfigParamsTO.ConfigParamVal, out prodSpecId);
            }
            tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.LOADING_SLIP_DEFAULT_CATEGORY);
            if (tblConfigParamsTO != null)
            {
                Int32.TryParse(tblConfigParamsTO.ConfigParamVal, out prodCatgId);
            }
            List<TblBookingExtTO> list= _iTblBookingExtDAO.SelectEmptyBookingExtList(prodCatgId,prodSpecId);

            tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.LOADING_SLIP_DEFAULT_SIZES);
            String sizes = string.Empty;

            if (tblConfigParamsTO != null)
                sizes = Convert.ToString(tblConfigParamsTO.ConfigParamVal);

            string[] sizeList = sizes.Split(',');

            for (int l = 0; l < list.Count; l++)
            {
                int materialId = list[l].MaterialId;
                if (Constants.IsNeedToRemoveFromList(sizeList, materialId))
                {
                    list.RemoveAt(l);
                    l--;
                }
            }

            return list;
        }



        public List<TblBookingExtTO> SelectAllBookingDetailsWrtDealer(Int32 dealerId)
        {
            return _iTblBookingExtDAO.SelectBookingDetails(dealerId);

        }

        public TblBookingExtTO SelectTblBookingExtTO(Int32 idBookingExt)
        {
            return _iTblBookingExtDAO.SelectTblBookingExt(idBookingExt);

        }


        /// <summary>
        /// [15-12-2017]Vijaymala:Added to get booking extension list according to schedule
        /// </summary>
        /// <param name="tblBookingExtTO"></param>
        /// <returns></returns>
        public List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(Int32 scheduleId)
        {
            return _iTblBookingExtDAO.SelectAllTblBookingExtListBySchedule(scheduleId);

        }

        public List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(Int32 scheduleId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblBookingExtDAO.SelectAllTblBookingExtListBySchedule(scheduleId,conn,tran);

        }

        #endregion

        #region Insertion
        public int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            return _iTblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO);
        }

        public int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            return _iTblBookingExtDAO.UpdateTblBookingExt(tblBookingExtTO);
        }

        public int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingExtDAO.UpdateTblBookingExt(tblBookingExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingExt(Int32 idBookingExt)
        {
            return _iTblBookingExtDAO.DeleteTblBookingExt(idBookingExt);
        }

        public int DeleteTblBookingExt(Int32 idBookingExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingExtDAO.DeleteTblBookingExt(idBookingExt, conn, tran);
        }

        public int DeleteTblBookingExtBySchedule(Int32 scheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingExtDAO.DeleteTblBookingExtBySchedule(scheduleId, conn, tran);
        }
        #endregion

    }
}
