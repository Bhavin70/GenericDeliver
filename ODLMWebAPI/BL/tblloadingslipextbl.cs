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
using ODLMWebAPI.DAL.Interfaces;
 
namespace ODLMWebAPI.BL
{    
    public class TblLoadingSlipExtBL : ITblLoadingSlipExtBL
    {
        private readonly ITblLoadingSlipExtDAO _iTblLoadingSlipExtDAO;
        private readonly ITblBookingExtBL _iTblBookingExtBL;
        private readonly ITblBookingScheduleBL _iTblBookingScheduleBL;
        private readonly ITblBookingDelAddrBL _iTblBookingDelAddrBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblLoadingDAO _iTblLoadingDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblProductInfoDAO _iTblProductInfoDAO;
        private readonly ITblProductInfoBL _iTblProductInfoBL;


        public TblLoadingSlipExtBL(ITblProductInfoBL iTblProductInfoBL, IConnectionString iConnectionString, ITblLoadingDAO iTblLoadingDAO, ITblConfigParamsBL iTblConfigParamsBL, ITblBookingDelAddrBL iTblBookingDelAddrBL, ITblBookingScheduleBL iTblBookingScheduleBL, ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, ITblBookingExtBL iTblBookingExtBL)
        {
            _iTblLoadingSlipExtDAO = iTblLoadingSlipExtDAO;
            _iTblBookingExtBL = iTblBookingExtBL;
            _iTblBookingScheduleBL = iTblBookingScheduleBL;
            _iTblBookingDelAddrBL = iTblBookingDelAddrBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblLoadingDAO = iTblLoadingDAO;
            _iConnectionString = iConnectionString;
            _iTblProductInfoBL = iTblProductInfoBL;



        }
        #region Selection

        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtList()
        {
            return _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt();
            
        }

        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtList(int loadingSlipId)
        {
            return _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(loadingSlipId);

        }

        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtList(int loadingSlipId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(loadingSlipId,conn,tran);
        }

        /// <summary>
        /// Saket [2018-05-03] Added to reverse weighing
        /// </summary>
        /// <param name="WeighingMeasureId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtListByWeighingMeasureId(int WeighingMeasureId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExtByWeighingMeasureId(WeighingMeasureId, conn, tran);
        }

        public TblLoadingSlipExtTO SelectTblLoadingSlipExtTO(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
           return _iTblLoadingSlipExtDAO.SelectTblLoadingSlipExt(idLoadingSlipExt, conn, tran);
       
        }

        public List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExtList(Int32 prodCatId,  int bookingId)
        {
            List<TblLoadingSlipExtTO> list = _iTblLoadingSlipExtDAO.SelectEmptyLoadingSlipExt(prodCatId);
            if (list != null)
            {
                List<TblBookingExtTO> bookingList = _iTblBookingExtBL.SelectAllTblBookingExtList(bookingId);
                if (bookingList != null && bookingList.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var matchedTOList = bookingList.Where(b => b.MaterialId == list[i].MaterialId
                                                          && b.ProdCatId == list[i].ProdCatId
                                                          && b.ProdSpecId == list[i].ProdSpecId).ToList();
                        if (matchedTOList != null)
                        {
                            list[i].LoadingQty = matchedTOList.Sum(b => b.BookedQty);
                        }
                    }
                }
            }
            return list;

        }

        public List<TblBookingScheduleTO> SelectEmptyLoadingSlipExtListAgainstSch(Int32 prodCatId, Int32 prodSpecId, int bookingId, int brandId)
        {
            List<TblBookingScheduleTO> list = _iTblBookingScheduleBL.SelectAllTblBookingScheduleList(bookingId);
            List<TblProductInfoTO> tblProductInfoTOs = _iTblProductInfoBL.SelectAllTblProductInfoListLatest();
            if (list != null && list.Count > 0)
            {
                list = list.OrderBy(o => o.ScheduleDate).ToList();

                for (int k = 0; k < list.Count; k++)
                {
                    TblBookingScheduleTO tblBookingScheduleTO = list[k];
                    List<TblProductInfoTO> tblProductInfoTOList=new List<TblProductInfoTO>();
                    tblBookingScheduleTO.DeliveryAddressLst = _iTblBookingDelAddrBL.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);
                    
                    List<TblBookingExtTO> bookingList = _iTblBookingExtBL.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);

                    
                    if (bookingList != null && bookingList.Count > 0)
                    {
                        for (int i = 0; i < bookingList.Count; i++)
                        {
                            var matchedAvgWt = tblProductInfoTOs.Where(b => b.MaterialId == bookingList[i].MaterialId
                                                           && b.ProdCatId == bookingList[i].ProdCatId
                                                           && b.ProdSpecId == bookingList[i].ProdSpecId
                                                           && b.BrandId == bookingList[i].BrandId).ToList();

                            TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO(bookingList[i]);
                            tblLoadingSlipExtTO.LoadingQty = bookingList[i].BalanceQty;
                            tblLoadingSlipExtTO.ConversionFactor = bookingList[i].ConversionFactor;
                            tblLoadingSlipExtTO.AvgBundleWt= bookingList[i].AvgBundleWt;
                            tblLoadingSlipExtTO.SchedulelayerId = tblBookingScheduleTO.LoadingLayerId;
                            tblLoadingSlipExtTO.Bundles = bookingList[i].PendingUomQty;
                            if(matchedAvgWt.Count>0 && matchedAvgWt!=null)
                            tblLoadingSlipExtTO.AvgBundleWt = matchedAvgWt[0].AvgBundleWt;
                            list[k].LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);
                        }

                        tblBookingScheduleTO.BalanceQty = bookingList.Sum(s => s.BalanceQty);

                    }

                }

                list = list.Where(w => w.BalanceQty > 0).ToList();

            }
            return list;
        }

        public List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExtList(Int32 prodCatId, Int32 prodSpecId, int bookingId, int brandId)
        {

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.LOADING_SLIP_DEFAULT_SPECIFICATION);
            if (tblConfigParamsTO != null)
            {
               Int32.TryParse(tblConfigParamsTO.ConfigParamVal, out prodSpecId);
            }

            tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.LOADING_SLIP_DEFAULT_CATEGORY);
            if (tblConfigParamsTO != null)
            {
                Int32.TryParse(tblConfigParamsTO.ConfigParamVal, out prodCatId);
            }
            //[05-09-2018]
            List<TblLoadingSlipExtTO> list = new List<TblLoadingSlipExtTO>();
            List<TblBookingExtTO> bookingExtList = _iTblBookingExtBL.SelectAllTblBookingExtList(bookingId);
            //Aniket added code to add AvgBundleWt [5-7-2019]
            List<TblProductInfoTO> tblProductInfoTOList = _iTblProductInfoBL.SelectAllTblProductInfoListLatest();
            if (bookingExtList != null && bookingExtList.Count > 0)
            {
                prodCatId = 0;
                prodSpecId = 0;
            }
            list = _iTblLoadingSlipExtDAO.SelectEmptyLoadingSlipExt(prodCatId, prodSpecId);
            if (list != null)
            {
                List<TblLoadingSlipExtTO> finalList = new List<TblLoadingSlipExtTO>();
                if (bookingExtList != null && bookingExtList.Count > 0)
                {

                    //List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromBookingId(bookingId);

                    for (int i = 0; i < list.Count; i++)
                    {
                        var matchedAvgWt=tblProductInfoTOList.Where(b => b.MaterialId == list[i].MaterialId
                                                          && b.ProdCatId == list[i].ProdCatId
                                                          && b.ProdSpecId == list[i].ProdSpecId
                                                          && b.BrandId == list[i].BrandId).ToList();

                        var matchedTOList = bookingExtList.Where(b => b.MaterialId == list[i].MaterialId
                                                          && b.ProdCatId == list[i].ProdCatId
                                                          && b.ProdSpecId == list[i].ProdSpecId
                                                          && b.BrandId == list[i].BrandId).ToList();
                        if(matchedAvgWt!=null && matchedAvgWt.Count>0)
                        {
                            list[i].AvgBundleWt = matchedAvgWt[0].AvgBundleWt;
                        }
                        if (matchedTOList != null && matchedTOList.Count  > 0)
                        {

                            //if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0)
                            //{
                            //    var previous = tblLoadingSlipExtTOList.Where(b => b.MaterialId == list[i].MaterialId
                            //                                  && b.ProdCatId == list[i].ProdCatId
                            //                                  && b.ProdSpecId == list[i].ProdSpecId
                            //                                  && b.BrandId == list[i].BrandId).ToList();


                            //    list[i].LoadingQty = matchedTOList.Sum(b => b.BookedQty);

                            //    list[i].LoadingQty -= previous.Sum(b => b.LoadingQty);
                            //}
                            //else
                            //{
                            //    list[i].LoadingQty = matchedTOList.Sum(b => b.BookedQty);
                            //}

                            //list[i].LoadingQty = matchedTOList.Sum(b => b.BookedQty);
                            list[i].LoadingQty = matchedTOList.Sum(b => b.BalanceQty);
                            
                            finalList.Add(list[i]);
                        }
                    }

                    //Vijaymala[22-08-2018] Added for Return LoadingSlipExtList Based on Other Item 
                    List<TblLoadingSlipExtTO> otherItemList = new List<TblLoadingSlipExtTO>();
                    List<TblBookingExtTO> filterBookingExtList = bookingExtList.Where(ele => ele.ProdItemId > 0).ToList();


                    if (filterBookingExtList != null && filterBookingExtList.Count > 0)
                    {


                        var distinctBookingsExtList = filterBookingExtList.GroupBy(b => b.ProdItemId).Select(s => s.FirstOrDefault()).ToList();
                        for (int k = 0; k < distinctBookingsExtList.Count; k++)
                        {
                         
                            Double LoadingQty = bookingExtList.Where(b => b.ProdItemId == distinctBookingsExtList[k].ProdItemId).Sum(l => l.BalanceQty);
                            //foreach (TblBookingExtTO bookingExtTo in bookingList)
                            //{
                               TblBookingExtTO bookingExtTo = distinctBookingsExtList[k];
                               TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();

                                tblLoadingSlipExtTO.MaterialId = bookingExtTo.MaterialId;
                                tblLoadingSlipExtTO.ProdCatId = bookingExtTo.ProdCatId;
                                tblLoadingSlipExtTO.ProdSpecId = bookingExtTo.ProdSpecId;
                                tblLoadingSlipExtTO.ProdItemId = bookingExtTo.ProdItemId;
                                tblLoadingSlipExtTO.BookingId = bookingExtTo.BookingId;
                                tblLoadingSlipExtTO.BookingExtId = bookingExtTo.IdBookingExt;
                                tblLoadingSlipExtTO.DisplayName = bookingExtTo.DisplayName;
                                tblLoadingSlipExtTO.LoadingQty = LoadingQty;
                                tblLoadingSlipExtTO.MaterialDesc = bookingExtTo.MaterialSubType;
                                tblLoadingSlipExtTO.BrandId = bookingExtTo.BrandId;

                                tblLoadingSlipExtTO.BrandDesc = bookingExtTo.BrandDesc;
                                tblLoadingSlipExtTO.ConversionUnitOfMeasure = bookingExtTo.ConversionUnitOfMeasure;
                                tblLoadingSlipExtTO.WeightMeasureUnitDesc = bookingExtTo.WeightMeasureUnitDesc;
                                tblLoadingSlipExtTO.UomQty = bookingExtTo.UomQty;
                                tblLoadingSlipExtTO.BookedQty = bookingExtTo.BookedQty;
                                tblLoadingSlipExtTO.BookingRate = bookingExtTo.BookingRate;
                                tblLoadingSlipExtTO.Rate = bookingExtTo.Rate;
                                tblLoadingSlipExtTO.Discount = bookingExtTo.Discount;

                            otherItemList.Add(tblLoadingSlipExtTO);
                            //}

                        }
                          
                       
                           
                        finalList = otherItemList;
                    }

                    return finalList;
                }

                tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.LOADING_SLIP_DEFAULT_SIZES);
                String sizes = string.Empty;

                if (tblConfigParamsTO != null)
                    sizes = Convert.ToString(tblConfigParamsTO.ConfigParamVal);

                string[] sizeList = sizes.Split(',');

                for (int l = 0; l < list.Count; l++)
                {
                    int materialId = list[l].MaterialId;
                    if (list[l].BrandId != brandId || list[l].ProdCatId != prodCatId || prodSpecId != list[l].ProdSpecId || Constants.IsNeedToRemoveFromList(sizeList, materialId))
                    {
                       

                            list.RemoveAt(l);
                            l--;
                        
                    }
                }
            }
            return list;

        }

        public List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(Int32 loadingId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId(loadingId.ToString(), conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        public List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromBookingId(Int32 bookingId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromBookingId(bookingId.ToString(), conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(Int32 loadingId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId(loadingId.ToString(),conn,tran);

        }

        public Dictionary<Int32,Double> SelectLoadingQuotaWiseApprovedLoadingQtyDCT(String loadingQuotaIds,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingSlipExtDAO.SelectLoadingQuotaWiseApprovedLoadingQtyDCT(loadingQuotaIds, conn, tran);
        }

        public List<TblLoadingSlipExtTO> SelectCnfWiseLoadingMaterialToPostPoneList(SqlConnection conn, SqlTransaction tran)
        {
            List<TblLoadingSlipExtTO> postponeList = null;
            TblConfigParamsTO postponeConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID, conn, tran);
            //Sanjay [2017-07-18] The vehicles which are gate in ,loading completed or postponed
            //List<TblLoadingTO> loadingTOToPostponeList = _iTblLoadingDAO.SelectAllLoadingListByStatus((int)Constants.TranStatusE.LOADING_POSTPONED + "", conn, tran);
            List<TblLoadingTO> loadingTOToPostponeList = _iTblLoadingDAO.SelectAllLoadingListByStatus(postponeConfigParamsTO.ConfigParamVal, conn, tran);
            if (loadingTOToPostponeList != null)
            {
                postponeList = new List<TblLoadingSlipExtTO>();
                var loadingIds = string.Join(",", loadingTOToPostponeList.Where(x=>x.LoadingTypeE==Constants.LoadingTypeE.REGULAR).Select(p => p.IdLoading.ToString()));
                List<TblLoadingSlipExtTO> extList = _iTblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId(loadingIds, conn, tran);
                if (extList != null && extList.Count > 0)
                {
                    postponeList.AddRange(extList);
                }
            }

            return postponeList;
        }

        /// <summary>
        /// [13-12-2017] Vijaymala : Added To Get Loading slip extension list according to filter 
        /// </summary>
        /// <returns></returns>
        public List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtByDate(DateTime frmDt, DateTime toDt, String statusStr)
        {
          
            List<TblLoadingSlipExtTO>tblLoadingSlipExtTolist= _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExtByDate(frmDt, toDt, statusStr);
          //  tblLoadingSlipExtTolist = tblLoadingSlipExtTolist.OrderBy(asc=>asc.SequenceNo).ThenBy(a => a.DisplayName).ToList();
            return tblLoadingSlipExtTolist;
                //.ThenBy(a => a.AlertInstanceId).ToList();

        }

        /// <summary>
        ///[03-01-2017] Vijaymala Added :To get loading slip extension list
        /// </summary>
        /// <param name="idLoadingSlipExt"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        /// 
        public List<TblLoadingSlipExtTO> SelectTblLoadingSlipExtByIds(String idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtDAO.SelectTblLoadingSlipExtByIds(idLoadingSlipExt, conn, tran);

        }
        #endregion

        #region Insertion
        public int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            return _iTblLoadingSlipExtDAO.InsertTblLoadingSlipExt(tblLoadingSlipExtTO);
        }

        public int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtDAO.InsertTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            return _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO);
        }

        public int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt)
        {
            return _iTblLoadingSlipExtDAO.DeleteTblLoadingSlipExt(idLoadingSlipExt);
        }

        public int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtDAO.DeleteTblLoadingSlipExt(idLoadingSlipExt, conn, tran);
        }

        #endregion

    }
}
