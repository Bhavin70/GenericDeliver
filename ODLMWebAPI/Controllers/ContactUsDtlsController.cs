using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using System.Threading.Tasks;
using System;
using ODLMWebAPI.BL;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class ContactUsDtlsController : Controller
    {
        private readonly ITblContactUsDtlsBL _iTblContactUsDtlsBL;
        public ContactUsDtlsController(ITblContactUsDtlsBL iTblContactUsDtlsBL)
        {
            _iTblContactUsDtlsBL = iTblContactUsDtlsBL;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // Select contacts on condition - Tejaswini
        [Route("GetContactUsDtls/IsActive/{IsActive}")]
        [HttpGet]
        public List<IGrouping<int, TblContactUsDtls>> GetContactUsDtlsList(int IsActive)
        {
            try
            {
                //TblContactUsDtlsBL tblContactUsDtlsBL = new TblContactUsDtlsBL();
                return _iTblContactUsDtlsBL.SelectContactUsDtls(IsActive);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

}