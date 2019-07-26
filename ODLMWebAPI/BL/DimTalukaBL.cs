using System;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
	public class DimTalukaBL : IDimTalukaBL
    {
        private readonly IDimTalukaDAO _iDimTalukaDAO;
        public DimTalukaBL(IDimTalukaDAO iDimTalukaDAO)
        {
            _iDimTalukaDAO = iDimTalukaDAO;
        }
        #region Updation
        public int UpdateDimTaluka(StateMasterTO dimtalTO)
		{
			return _iDimTalukaDAO.UpdateDimTaluka(dimtalTO);

		}

		#endregion




		#region insertion
		public int InsertDimTaluka(StateMasterTO dimTalukaTO)
		{
			return _iDimTalukaDAO.InsertDimTaluka(dimTalukaTO);

		}
		#endregion



	}
	}
