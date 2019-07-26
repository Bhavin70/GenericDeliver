using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL
{
    public class DimDistrictBL : IDimDistrictBL
    {
        private readonly IDimDistrictDAO _iDimDistrictDAO;
        public DimDistrictBL(IDimDistrictDAO iDimDistrictDAO)
        {
            _iDimDistrictDAO = iDimDistrictDAO;
        }
        #region insertion
        public int InsertDimDistrict(StateMasterTO dimDistrictTO)
		{
			return _iDimDistrictDAO.InsertDimDistrict(dimDistrictTO);

		}
		#endregion


		#region Updation
		public int UpdateDimDistrict(StateMasterTO dimDistrictTO)
		{
			return _iDimDistrictDAO.UpdateDimDistrict(dimDistrictTO);

		}
#endregion
	}
}
