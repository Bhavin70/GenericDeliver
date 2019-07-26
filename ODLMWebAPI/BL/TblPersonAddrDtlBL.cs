using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TblPersonAddrDtlBL : ITblPersonAddrDtlBL
    {
        private readonly ITblPersonAddrDtlDAO _iTblPersonAddrDtlDAO;
        private readonly ITblAddressBL _iTblAddressBL;
        public TblPersonAddrDtlBL(ITblPersonAddrDtlDAO iTblPersonAddrDtlDAO, ITblAddressBL iTblAddressBL)
        {
            _iTblPersonAddrDtlDAO = iTblPersonAddrDtlDAO;
            _iTblAddressBL = iTblAddressBL;
        }
        #region Selection
        public List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtl()
        {
            return _iTblPersonAddrDtlDAO.SelectAllTblPersonAddrDtl();
        }

        public List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtlList()
        {
            List<TblPersonAddrDtlTO> tblPersonAddrDtlTODT = _iTblPersonAddrDtlDAO.SelectAllTblPersonAddrDtl();
            return tblPersonAddrDtlTODT;
        }

        public TblPersonAddrDtlTO SelectTblPersonAddrDtlTO(Int32 idPersonAddrDtl)
        {
            TblPersonAddrDtlTO tblPersonAddrDtlTO = _iTblPersonAddrDtlDAO.SelectTblPersonAddrDtl(idPersonAddrDtl);
            if (tblPersonAddrDtlTO != null)
                return tblPersonAddrDtlTO;
            else
                return null;
        }

        public TblPersonAddrDtlTO SelectTblPersonAddrDtlTO(Int32 personId,Int32 addressTypeId)
        {
            TblPersonAddrDtlTO tblPersonAddrDtlTO = _iTblPersonAddrDtlDAO.SelectTblPersonAddrDtl(personId,addressTypeId);
            if (tblPersonAddrDtlTO != null)
                return tblPersonAddrDtlTO;
            else
                return null;
        }


        public TblAddressTO SelectAddressTOonPersonAddrDtlId(Int32 personId, Int32 addressTypeId)
        {
            TblPersonAddrDtlTO tblPersonAddrDtlTO = SelectTblPersonAddrDtlTO(personId, addressTypeId);
            if (tblPersonAddrDtlTO != null)
            {
                TblAddressTO tblAddressTO = _iTblAddressBL.SelectTblAddressTO(tblPersonAddrDtlTO.AddressId);
                if (tblAddressTO != null)
                {
                    return tblAddressTO;
                }
                else
                    return null;

            }
            else
                return null;

        }


        #endregion

        #region Insertion
        public int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO)
        {
            return _iTblPersonAddrDtlDAO.InsertTblPersonAddrDtl(tblPersonAddrDtlTO);
        }

        public int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPersonAddrDtlDAO.InsertTblPersonAddrDtl(tblPersonAddrDtlTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO)
        {
            return _iTblPersonAddrDtlDAO.UpdateTblPersonAddrDtl(tblPersonAddrDtlTO);
        }

        public int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPersonAddrDtlDAO.UpdateTblPersonAddrDtl(tblPersonAddrDtlTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl)
        {
            return _iTblPersonAddrDtlDAO.DeleteTblPersonAddrDtl(idPersonAddrDtl);
        }

        public int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPersonAddrDtlDAO.DeleteTblPersonAddrDtl(idPersonAddrDtl, conn, tran);
        }

        #endregion

    }
}
