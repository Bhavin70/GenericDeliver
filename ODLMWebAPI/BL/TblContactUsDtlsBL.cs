using System;
using System.Collections.Generic;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TblContactUsDtlsBL : ITblContactUsDtlsBL
    {
        private readonly ITblContactUsDtlsDAO _iTblContactUsDtlsDAO;
        public TblContactUsDtlsBL(ITblContactUsDtlsDAO iTblContactUsDtlsDAO)
        {
            _iTblContactUsDtlsDAO = iTblContactUsDtlsDAO;
        }

        #region selection
        // Select contacts on condition - Tejaswini
        public List<IGrouping<int,TblContactUsDtls>> SelectContactUsDtls (int IsActive)
        {
            List<TblContactUsDtls> ContactUsDtlsList = new List<TblContactUsDtls>();

            if (IsActive == 0 || IsActive == 1)
            {
                ContactUsDtlsList = _iTblContactUsDtlsDAO.SelectContactUsDtls(IsActive);   
            }
            else
            {
                ContactUsDtlsList = _iTblContactUsDtlsDAO.SelectAllContactUsDtls();
            }
            List<TblContactUsDtls> ContactUsDtlsListTemp = new List<TblContactUsDtls>();
            var tempData = ContactUsDtlsList.GroupBy(g => g.SupportTypeId).Select(ele=>ele).ToList();
            return tempData;
        }

        #endregion
    }
}