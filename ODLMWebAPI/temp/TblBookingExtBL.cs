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
    public class TblBookingExtBL : ITblBookingExtBL
    {
        #region Selection
       
        public List<TblBookingExtTO> SelectAllTblBookingExtList()
        {
           return TblBookingExtDAO.SelectAllTblBookingExt();
      
        }

        public List<TblBookingExtTO> SelectAllTblBookingExtList(int bookingId)
        {
            return TblBookingExtDAO.SelectAllTblBookingExt(bookingId);

        }

        public List<TblBookingExtTO> SelectAllTblBookingExtList(int bookingId, SqlConnection conn, SqlTransaction trans)
        {
            return TblBookingExtDAO.SelectAllTblBookingExt(bookingId, conn, trans);

        }

        /// <summary>
        /// Sanjay [2017-03-27] This list will return all material alongwith prod cat and spec
        /// Required while showing popup to take material wise ,spec wise qty while booking
        /// </summary>
        /// <param name="prodCatgId"></param>
        /// <returns></returns>
        public List<TblBookingExtTO> SelectEmptyBookingExtList(int prodCatgId,int prodSpecId)
        {

            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.LOADING_SLIP_DEFAULT_SPECIFICATION);
            if (tblConfigParamsTO != null)
            {
                Int32.TryParse(tblConfigParamsTO.ConfigParamVal, out prodSpecId);
            }
            tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.LOADING_SLIP_DEFAULT_CATEGORY);
            if (tblConfigParamsTO != null)
            {
                Int32.TryParse(tblConfigParamsTO.ConfigParamVal, out prodCatgId);
            }
            List<TblBookingExtTO> list= TblBookingExtDAO.SelectEmptyBookingExtList(prodCatgId,prodSpecId);

            tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.LOADING_SLIP_DEFAULT_SIZES);
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
            return TblBookingExtDAO.SelectBookingDetails(dealerId);

        }

        public TblBookingExtTO SelectTblBookingExtTO(Int32 idBookingExt)
        {
            return TblBookingExtDAO.SelectTblBookingExt(idBookingExt);

        }


        /// <summary>
        /// [15-12-2017]Vijaymala:Added to get booking extension list according to schedule
        /// </summary>
        /// <param name="tblBookingExtTO"></param>
        /// <returns></returns>
        public List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(Int32 scheduleId)
        {
            return TblBookingExtDAO.SelectAllTblBookingExtListBySchedule(scheduleId);

        }

        public List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(Int32 scheduleId,SqlConnection conn,SqlTransaction tran)
        {
            return TblBookingExtDAO.SelectAllTblBookingExtListBySchedule(scheduleId,conn,tran);

        }

        #endregion

        #region Insertion
        public int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            return TblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO);
        }

        public int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            return TblBookingExtDAO.UpdateTblBookingExt(tblBookingExtTO);
        }

        public int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingExtDAO.UpdateTblBookingExt(tblBookingExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingExt(Int32 idBookingExt)
        {
            return TblBookingExtDAO.DeleteTblBookingExt(idBookingExt);
        }

        public int DeleteTblBookingExt(Int32 idBookingExt, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingExtDAO.DeleteTblBookingExt(idBookingExt, conn, tran);
        }

        public int DeleteTblBookingExtBySchedule(Int32 scheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingExtDAO.DeleteTblBookingExtBySchedule(scheduleId, conn, tran);
        }
        #endregion

    }
}
