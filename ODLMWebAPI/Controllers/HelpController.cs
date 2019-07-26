using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.Controllers
{
    
    [Produces("application/json")]
    [Route("api/Help")]
    public class HelpController : Controller
    {
        private readonly IConnectionString _iConnectionString;
        public HelpController(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        // GET: api/values
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

        [Route("GetAllDocuments")]
        [HttpGet]
        public List<string> GetAllDocuments()
        {

            try
           {
                List<string> fileNames = new List<string>();

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer sourceContainer = blobClient.GetContainerReference(StaticStuff.Constants.AzureDocumentContainerName);

                var blobList = sourceContainer.ListBlobsSegmentedAsync(string.Empty, false, BlobListingDetails.None, int.MaxValue, null, null, null).Result.Results;

                if (blobList != null)
                {
                    foreach (var file in blobList)
                    {
                        CloudBlockBlob sourceBlob = sourceContainer.GetBlockBlobReference(((Microsoft.WindowsAzure.Storage.Blob.CloudBlob)file).Name);
                        string fileName = ((Microsoft.WindowsAzure.Storage.Blob.CloudBlob)file).Name;
                        fileNames.Add(fileName);
                    }
                }
                return fileNames;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}