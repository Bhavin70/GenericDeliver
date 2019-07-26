using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ODLMWebAPI.Authentication;

namespace ODLMWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    { 
        private readonly IAuthentication _iAuthentication;
        public ValuesController(IAuthentication iAuthentication)
        {
            _iAuthentication = iAuthentication;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

        // Vaibhav 
        [Route("getEncryptData")]
        [HttpGet]
        public string getEncryptData(string data)
        {

            _iAuthentication.getAccessToken("","");

            return "";
        }
    }
}
