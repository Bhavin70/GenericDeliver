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
    public class TblAddressBL : ITblAddressBL
    {
        #region Selection
     
        public List<TblAddressTO> SelectAllTblAddressList()
        {
            return  TblAddressDAO.SelectAllTblAddress();
        }

        public TblAddressTO SelectTblAddressTO(Int32 idAddr)
        {
            return  TblAddressDAO.SelectTblAddress(idAddr);
        
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Get Specific Address Details of the Given Organization.
        /// It can be dealer,C&F agent Or Competitor
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="addressTypeE"></param>
        /// <returns></returns>
        public TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblAddressTO tblAddressTO = TblAddressDAO.SelectOrgAddressWrtAddrType(orgId, addressTypeE, conn, tran);
                tblAddressTO.AddressTypeE = addressTypeE;
                return tblAddressTO;

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

        public TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE ,SqlConnection conn,SqlTransaction tran)
        {
            return TblAddressDAO.SelectOrgAddressWrtAddrType(orgId, addressTypeE, conn, tran);
        }

        public List<TblAddressTO> SelectOrgAddressList(Int32 orgId)
        {
            return TblAddressDAO.SelectOrgAddressList(orgId);
        }

        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization address list of particular type;
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="addressTypeE"></param>
        /// <returns></returns>
        public List <TblAddressTO> SelectOrgAddressDetailOfRegion(string orgId, StaticStuff.Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblAddressTO> tblAddressTOList = TblAddressDAO.SelectOrgAddressDetailOfRegion(orgId, addressTypeE, conn, tran);
                return tblAddressTOList;

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

        #endregion

        #region Insertion
        public int InsertTblAddress(TblAddressTO tblAddressTO)
        {
            return TblAddressDAO.InsertTblAddress(tblAddressTO);
        }

        public int InsertTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAddressDAO.InsertTblAddress(tblAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAddress(TblAddressTO tblAddressTO)
        {
            return TblAddressDAO.UpdateTblAddress(tblAddressTO);
        }

        public int UpdateTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAddressDAO.UpdateTblAddress(tblAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAddress(Int32 idAddr)
        {
            return TblAddressDAO.DeleteTblAddress(idAddr);
        }

        public int DeleteTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran)
        {
            return TblAddressDAO.DeleteTblAddress(idAddr, conn, tran);
        }

        #endregion
        
    }
}
