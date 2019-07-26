using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL
{
    public class DimConfigurePageBL : IDimConfigurePageBL
    {
        private readonly IDimConfigurePageDAO _iDimConfigurePageDAO;
        public DimConfigurePageBL(IDimConfigurePageDAO iDimConfigurePageDAO)
        {
            _iDimConfigurePageDAO = iDimConfigurePageDAO;
        }
        public List<DimConfigurePageTO> GetConfigurationByPageId(int pageId)
        {
            return _iDimConfigurePageDAO.GetConfigurationByPageId(pageId);
        }
    }
}
