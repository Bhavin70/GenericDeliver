using System;
using System.Collections.Generic;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblContactUsDtlsBL : ITblContactUsDtlsBL
    {
        TblContactUsDtlsDAO TblContactUsDtlsDAO;
        #region selection
        public TblContactUsDtlsBL()
        {
            this.TblContactUsDtlsDAO = new TblContactUsDtlsDAO();
        }

          // Select contacts on condition - Tejaswini
        public List<IGrouping<int,TblContactUsDtls>> SelectContactUsDtls (int IsActive)
        {
            List<TblContactUsDtls> ContactUsDtlsList = new List<TblContactUsDtls>();

            if (IsActive == 0 || IsActive == 1)
            {
                ContactUsDtlsList = this.TblContactUsDtlsDAO.SelectContactUsDtls(IsActive);   
            }
            else
            {
                ContactUsDtlsList = this.TblContactUsDtlsDAO.SelectAllContactUsDtls();
            }
            List<TblContactUsDtls> ContactUsDtlsListTemp = new List<TblContactUsDtls>();
            var tempData = ContactUsDtlsList.GroupBy(g => g.SupportTypeId).Select(ele=>ele).ToList();
            return tempData;
        }

        #endregion
    }
}