using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.Controllers
{
    
    [Produces("application/json")]
    [Route("api/Report")]
    public class ReportController : Controller
    {
        private readonly ITblReportsBL _iTblReportsBL;
        public ReportController(ITblReportsBL iTblReportsBL)
        {
            _iTblReportsBL = iTblReportsBL;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Sudhir[04-SEPT-2018] Added for Selection Of Report.
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [Route("GetReportObject")]
        [HttpGet]
        [ProducesResponseType(typeof(TblReportsTO), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetReportObject(Int32 reportId)
        {
            try
            {
                TblReportsTO tblReportsTO = _iTblReportsBL.SelectTblReportsTO(reportId);
                if (tblReportsTO != null)
                {
                    return Ok(tblReportsTO);
                }
                else
                {
                    return NotFound(tblReportsTO);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Sudhir[10-SEPT-2018] Added for Selection Of All Report.
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [Route("GetAllReportObject")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblReportsTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllReportObject(Int32 reportId)
        {
            try
            {
                List<TblReportsTO> tblReportsTOList = _iTblReportsBL.SelectAllTblReportsList();
                if (tblReportsTOList != null)
                {
                    return Ok(tblReportsTOList);
                }
                else
                {
                    return NotFound(tblReportsTOList);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [Route("GetDynamicData")]
        [HttpPost]
        [ProducesResponseType(typeof(List<DynamicReportTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetDynamicData([FromBody] JObject data)
        {

            TblReportsTO tblReportsTO = JsonConvert.DeserializeObject<TblReportsTO>(data.ToString());

            IEnumerable<dynamic> dataList = _iTblReportsBL.CreateDynamicQuery(tblReportsTO);
            if (data != null)
            {
                return Ok(dataList);
            }
            else
            {
                return NotFound("No Data  Found"); ;
            }
        }
    }
}