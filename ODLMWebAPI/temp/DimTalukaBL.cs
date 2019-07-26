using System;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
	public class DimTalukaBL : IDimTalukaBL
    {
		#region Updation
		public int UpdateDimTaluka(StateMasterTO dimtalTO)
		{
			return DimTalukaDAO.UpdateDimTaluka(dimtalTO);

		}

		#endregion




		#region insertion
		public int InsertDimTaluka(StateMasterTO dimTalukaTO)
		{
			return DimTalukaDAO.InsertDimTaluka(dimTalukaTO);

		}
		#endregion



	}
	}
