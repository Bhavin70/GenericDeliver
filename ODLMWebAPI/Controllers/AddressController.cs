using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Net;
using System.Net.Http;
using ODLMWebAPI.BL.Interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private readonly ITblAddressBL _iTblAddressBL;
        public AddressController(ITblAddressBL iTblAddressBL)
        {
            _iTblAddressBL = iTblAddressBL;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetOrgAddressDetails")]
        [HttpGet]
        public ActionResult GetOrgAddressDetails(Int32 orgId, Int32 addrTypeId=1)
        {
            Constants.AddressTypeE addressTypeE = (Constants.AddressTypeE)Enum.Parse(typeof(Constants.AddressTypeE), addrTypeId.ToString());
            TblAddressTO tblAddressTO =_iTblAddressBL.SelectOrgAddressWrtAddrType(orgId, addressTypeE);
            if(tblAddressTO==null)
            {
                return NoContent();
            }
            else 
            return new ObjectResult(tblAddressTO);
        }

        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization address list of particular type;
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="addrTypeId"></param>
        /// <returns></returns>
        [Route("GetOrgAddressDetailsOfRegion")]
        [HttpGet]
        public List<TblAddressTO> GetOrgAddressDetailsOfRegion(string orgId, Int32 addrTypeId = 1)
        {
            Constants.AddressTypeE addressTypeE = (Constants.AddressTypeE)Enum.Parse(typeof(Constants.AddressTypeE), addrTypeId.ToString());
            return _iTblAddressBL.SelectOrgAddressDetailOfRegion(orgId, addressTypeE);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
