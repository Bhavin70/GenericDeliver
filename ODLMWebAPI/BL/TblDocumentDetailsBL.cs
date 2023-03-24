using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{      
    public class TblDocumentDetailsBL : ITblDocumentDetailsBL
    {
        private readonly ITblDocumentDetailsDAO _iTblDocumentDetailsDAO;
        private readonly ITblModuleDAO _iTblModuleDAO;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblDocumentDetailsBL(ITblModuleDAO iTblModuleDAO, ITblDocumentDetailsDAO iTblDocumentDetailsDAO, IConnectionString iConnectionString, ICommon iCommon, ITblUserDAO iTblUserDAO)
        {
            _iTblDocumentDetailsDAO = iTblDocumentDetailsDAO;
            _iTblModuleDAO = iTblModuleDAO;
            _iTblUserDAO = iTblUserDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Selection
        public List<TblDocumentDetailsTO> SelectAllTblDocumentDetails()
        {
            return _iTblDocumentDetailsDAO.SelectAllTblDocumentDetails();
        }

        public List<TblDocumentDetailsTO> SelectAllTblDocumentDetailsList()
        {
            List<TblDocumentDetailsTO> tblDocumentDetailsTODT = _iTblDocumentDetailsDAO.SelectAllTblDocumentDetails();
            return tblDocumentDetailsTODT;
        }

        public TblDocumentDetailsTO SelectTblDocumentDetailsTO(Int32 idDocument)
        {
            TblDocumentDetailsTO tblDocumentDetailsTODT = _iTblDocumentDetailsDAO.SelectTblDocumentDetails(idDocument);
            if (tblDocumentDetailsTODT != null)
                return tblDocumentDetailsTODT;
            else
                return null;
        }

        public List<TblDocumentDetailsTO> GetUploadedFileBasedOnFileType(Int32 fileTypeId, Int32 createdBy)
        {
            return _iTblDocumentDetailsDAO.SelectDocumentDetailsBasedOnFileType(fileTypeId, createdBy);
        }

        public List<TblDocumentDetailsTO> GetUploadedFileBasedOnDocumentId(string DocumentIds)
        {
            return _iTblDocumentDetailsDAO.GetUploadedFileBasedOnDocumentId(DocumentIds);
        }


        #endregion

        #region Insertion
        public int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO)
        {
            return _iTblDocumentDetailsDAO.InsertTblDocumentDetails(tblDocumentDetailsTO);
        }

        public int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDocumentDetailsDAO.InsertTblDocumentDetails(tblDocumentDetailsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO)
        {
            return _iTblDocumentDetailsDAO.UpdateTblDocumentDetails(tblDocumentDetailsTO);
        }

        public int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDocumentDetailsDAO.UpdateTblDocumentDetails(tblDocumentDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblDocumentDetails(Int32 idDocument)
        {
            return _iTblDocumentDetailsDAO.DeleteTblDocumentDetails(idDocument);
        }

        public int DeleteTblDocumentDetails(Int32 idDocument, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDocumentDetailsDAO.DeleteTblDocumentDetails(idDocument, conn, tran);
        }


        //public void DeleteFromBlob(string filename)
        //{
        //    // Retrieve storage account from connection string.
        //    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));



        //    // Create the blob client.
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    // Retrieve reference to a target container.
        //    CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForTestingDocument);


        //    // Retrieve reference to a blob named "myblob.csv".
        //    CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

        //    // Delete the blob.
        //      blockBlob.DeleteAsync();
        //}

        #endregion

        //public void TestUploadFile()
        //{
        //    ResultMessage resultMessage = new ResultMessage();
        //    List<TblDocumentDetailsTO> list = new List<TblDocumentDetailsTO>();

        //    TblDocumentDetailsTO tblDocumentDetailsTO = new TblDocumentDetailsTO();
        //    tblDocumentDetailsTO.CreatedBy = 1;
        //    tblDocumentDetailsTO.ModuleId = 1;
        //    tblDocumentDetailsTO.Extension = "png";
        //    tblDocumentDetailsTO.IsActive = 1;
        //    tblDocumentDetailsTO.DocumentDesc = "ABC";
        //    tblDocumentDetailsTO.CreatedOn = _iCommon.ServerDateTime;
        //    var webClient = new WebClient();
        //    byte[] imageBytes = webClientData("http://www.google.com/images/logos/ps_logo2.png");
        //    tblDocumentDetailsTO.FileData = imageBytes;
        //    list.Add(tblDocumentDetailsTO);
        //    resultMessage=UploadDocument(list);
        //}



        public async Task<ResultMessage> UploadFileAsync(TblDocumentDetailsTO tblDocumentDetailsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            Boolean uploadFile = false;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();

                // Create azure storage  account connection.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = null;

                TblModuleTO tblModuleTO = _iTblModuleDAO.SelectTblModule(tblDocumentDetailsTO.ModuleId, conn, tran);
                if (tblModuleTO != null)
                {
                    // Retrieve reference to a target container.
                    if (tblModuleTO.ContainerName != null && tblModuleTO.ContainerName != String.Empty)
                    {
                        uploadFile = true;
                        container = blobClient.GetContainerReference(tblModuleTO.ContainerName);
                    }
                    else
                    {
                        uploadFile = false;
                    }

                }

                //For Unique Id.
                String UUID = Guid.NewGuid().ToString();

                //String fileName = tblDocumentDetailsTO.DocumentDesc + UUID + "." + tblDocumentDetailsTO.Extension;
                CloudBlockBlob blockBlob = null;

                String fileName = tblDocumentDetailsTO.DocumentDesc + UUID + "." + tblDocumentDetailsTO.Extension;
                if (container != null)
                {
                    blockBlob = container.GetBlockBlobReference(fileName);

                    long size = tblDocumentDetailsTO.FileData.Length;

                    //// full path to file in temp location
                    var filePath = Path.GetTempFileName();

                    if (tblDocumentDetailsTO.FileData.Length > 0 && uploadFile)
                    {
                        var fileStream = tblDocumentDetailsTO.FileData;

                        await blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                        tblDocumentDetailsTO.DocumentDesc = fileName;
                        tblDocumentDetailsTO.Path = blockBlob.SnapshotQualifiedUri.AbsoluteUri;
                        tblDocumentDetailsTO.CreatedOn = _iCommon.ServerDateTime;
                        result = InsertTblDocumentDetails(tblDocumentDetailsTO, conn, tran);
                        if (result == 1)
                        {
                            tblDocumentDetailsTO.FileData = null;
                            resultMessage.Tag = tblDocumentDetailsTO;
                            TblUserTO tblUserTO = _iTblUserDAO.SelectTblUser(tblDocumentDetailsTO.CreatedBy, conn, tran);
                            if (tblUserTO != null)
                            {
                                if (resultMessage.Tag != null)
                                {
                                    TblDocumentDetailsTO tblDocumentDetailsTOUpload = tblDocumentDetailsTO;
                                    tblDocumentDetailsTO = tblDocumentDetailsTOUpload;
                                    tblDocumentDetailsTO.FileData = null;
                                    tblUserTO.DoucmentId = tblDocumentDetailsTO.IdDocument;
                                    result = _iTblUserDAO.UpdateTblUser(tblUserTO, conn, tran);
                                    if (result != -1)
                                    {
                                        tran.Commit();
                                        return resultMessage;
                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Result = -1;
                                        resultMessage.Tag = null;
                                        resultMessage.DefaultBehaviour("While Updating User");
                                        return resultMessage;
                                    }
                                }
                                else
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Result = -1;
                                    resultMessage.Tag = null;
                                    resultMessage.DefaultBehaviour("WHile Uploading User");
                                    return resultMessage;
                                }

                            }
                        }
                        else
                        {
                            resultMessage.DefaultBehaviour("Error While UPloading File");
                        }
                    }
                }
                return resultMessage;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #region Upload Image 

        public ResultMessage UploadUserProfilePicture(TblDocumentDetailsTO tblDocumentDetailsTO)
        {
            List<TblDocumentDetailsTO> newDocumentDetailsList = new List<TblDocumentDetailsTO>();
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                newDocumentDetailsList.Add(tblDocumentDetailsTO);
                resultMessage = UploadDocumentWithConnTran(newDocumentDetailsList, conn, tran);
                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                {
                    TblUserTO tblUserTO = _iTblUserDAO.SelectTblUser(tblDocumentDetailsTO.CreatedBy, conn, tran);
                    if (tblUserTO != null)
                    {
                        if(resultMessage.Tag != null && typeof(List<TblDocumentDetailsTO>) == resultMessage.Tag.GetType())
                        {
                            List<TblDocumentDetailsTO> list = (List<TblDocumentDetailsTO>)resultMessage.Tag;
                            tblDocumentDetailsTO = list[0];
                            tblDocumentDetailsTO.FileData = null;
                            tblUserTO.DoucmentId = tblDocumentDetailsTO.IdDocument;
                            int result = _iTblUserDAO.UpdateTblUser(tblUserTO, conn, tran);
                            if (result != -1)
                            {
                                tran.Commit();
                                return resultMessage;
                            }
                            else
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = -1;
                                resultMessage.Tag = null;
                                resultMessage.DefaultBehaviour("WHile Uploading User");
                                return resultMessage;
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = -1;
                            resultMessage.Tag = null;
                            resultMessage.DefaultBehaviour("WHile Uploading User");
                            return resultMessage;
                        }

                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = -1;
                        resultMessage.Tag = null;
                        resultMessage.DefaultBehaviour("WHile Uploading User");
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = -1;
                    resultMessage.Tag = null;
                    resultMessage.DefaultBehaviour("Contailner Name Not Found for This Module");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "mergeInvoices");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        //Sudhir[24-APR-2018] Added for Uploading Image 
        public ResultMessage UploadDocument(List<TblDocumentDetailsTO> tblDocumentDetailsTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage = UploadDocumentWithConnTran(tblDocumentDetailsTOList, conn, tran);
                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Success..Invoice decomposed";
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "mergeInvoices");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Vijaymala added[22-05-2018]added:to upload  document with connection and transaction
        /// </summary>
        /// <param name="tblDocumentDetailsTOList"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>

        public ResultMessage UploadDocumentWithConnTran(List<TblDocumentDetailsTO> tblDocumentDetailsTOList, SqlConnection conn, SqlTransaction tran)
        {
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();
            String ErrorMessage = "";
            Boolean error = false;
            try
            {
                if (tblDocumentDetailsTOList != null)
                {

                    foreach (TblDocumentDetailsTO tblDocumentDetailsTO in tblDocumentDetailsTOList)
                    {
                        if (tblDocumentDetailsTO.FileData != null)
                        {
                            // Create azure storage  account connection.
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));

                            // Create the blob client.
                            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                            CloudBlobContainer container = null;

                            TblModuleTO tblModuleTO = _iTblModuleDAO.SelectTblModule(tblDocumentDetailsTO.ModuleId, conn, tran);
                            if (tblModuleTO != null)
                            {
                                if (tblModuleTO.ContainerName != null && tblModuleTO.ContainerName != String.Empty)
                                {
                                    container = blobClient.GetContainerReference(tblModuleTO.ContainerName);
                                }
                                else
                                {
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Result = -1;
                                    resultMessage.Tag = null;
                                    resultMessage.DefaultBehaviour("Contailner Name Not Found for This Module");
                                    return resultMessage;
                                }
                            }
                            //if (isLive == true)
                            //{
                            //    // Retrieve reference to a target container.
                            //    container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForDocument);
                            //}
                            //else
                            //{
                            //    // Retrieve reference to a target container.
                            //    container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForTestingDocument);
                            //}


                            // Retrieve reference to a target container.
                            //CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForDocument);

                            //For Unique Id.
                            String UUID = Guid.NewGuid().ToString();

                            String fileName = tblDocumentDetailsTO.DocumentDesc + UUID + "." + tblDocumentDetailsTO.Extension;

                            if (container != null)
                            {
                                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                                blockBlob.Properties.ContentType = "application/pdf";
                                var fileStream = tblDocumentDetailsTO.FileData;

                                Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);


                                tblDocumentDetailsTO.IsActive = 1;
                                tblDocumentDetailsTO.Path = blockBlob.SnapshotQualifiedUri.AbsoluteUri;
                                tblDocumentDetailsTO.CreatedOn = _iCommon.ServerDateTime;
                                result = InsertTblDocumentDetails(tblDocumentDetailsTO, conn, tran);
                                if (result == 1)
                                {
                                    resultMessage.Tag = tblDocumentDetailsTOList;
                                }
                                else
                                {
                                    ErrorMessage += "File Uploading Failed For" + tblDocumentDetailsTO.DocumentDesc + "|";
                                    error = true;
                                }
                            }
                        }
                    }
                    if (error)
                    {
                        resultMessage.DefaultBehaviour(ErrorMessage);
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = -1;
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        resultMessage.DisplayMessage = "File Uploaded Succesfully";
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour("List is Empty");
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = -1;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //Post Multi Part Files
        public async Task<List<TblDocumentDetailsTO>> UploadMultipleTypesFile(List<IFormFile> files, Int32 createdBy, Int32 FileTypeId, Int32 moduleId)
        {

            int result = 0;
            Boolean uploadFile = false;
            List<TblDocumentDetailsTO> tblDocumentDetailsList = new List<TblDocumentDetailsTO>();
            try
            {
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        // Create azure storage  account connection.
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));

                        // Create the blob client.
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = null;



                        TblModuleTO tblModuleTO = _iTblModuleDAO.SelectTblModule(moduleId);
                        if (tblModuleTO != null)
                        {
                            // Retrieve reference to a target container.
                            if (tblModuleTO.ContainerName != null && tblModuleTO.ContainerName != String.Empty)
                            {
                                uploadFile = true;
                                container = blobClient.GetContainerReference(tblModuleTO.ContainerName);
                            }
                            else
                            {
                                uploadFile = false;
                            }

                        }


                        //For Unique Id.
                        String UUID = Guid.NewGuid().ToString();

                        //String fileName = tblDocumentDetailsTO.DocumentDesc + UUID + "." + tblDocumentDetailsTO.Extension;
                        CloudBlockBlob blockBlob = null;
                        if (container != null)
                        {
                            blockBlob = container.GetBlockBlobReference(file.FileName);

                            long size = file.Length;

                            //// full path to file in temp location
                            var filePath = Path.GetTempFileName();

                            if (file.Length > 0 && uploadFile)
                            {
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                using (var stream = new FileStream(filePath, FileMode.Open))
                                {
                                    await blockBlob.UploadFromStreamAsync(stream);
                                }

                                //Insertion into tblDocument Details for Maintaining Record.
                                TblDocumentDetailsTO tblDocumentDetailsTO = new TblDocumentDetailsTO();
                                tblDocumentDetailsTO.IsActive = 1;
                                tblDocumentDetailsTO.ModuleId = moduleId;
                                tblDocumentDetailsTO.DocumentDesc = file.FileName;
                                tblDocumentDetailsTO.Path = blockBlob.SnapshotQualifiedUri.AbsoluteUri;
                                tblDocumentDetailsTO.CreatedOn = _iCommon.ServerDateTime;
                                tblDocumentDetailsTO.CreatedBy = createdBy;
                                tblDocumentDetailsTO.FileTypeId = FileTypeId;
                                result = InsertTblDocumentDetails(tblDocumentDetailsTO);
                                if (result == 1)
                                {
                                    tblDocumentDetailsList.Add(tblDocumentDetailsTO);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblDocumentDetailsList;
        }


        #endregion

    }
}
