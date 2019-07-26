using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL;
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
		#region insertion
		public int InsertDimDistrict(StateMasterTO dimDistrictTO)
		{
			return DimDistrictDAO.InsertDimDistrict(dimDistrictTO);

		}
		#endregion


		#region Updation
		public int UpdateDimDistrict(StateMasterTO dimDistrictTO)
		{
			return DimDistrictDAO.UpdateDimDistrict(dimDistrictTO);

		}
#endregion
	}
}
