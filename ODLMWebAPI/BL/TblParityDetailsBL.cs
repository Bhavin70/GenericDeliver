using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{     
    public class TblParityDetailsBL : ITblParityDetailsBL
    {
        private readonly ITblParityDetailsDAO _iTblParityDetailsDAO;
        private readonly IDimStateDAO _iDimStateDAO;
        private readonly IConnectionString _iConnectionString;
        public TblParityDetailsBL(IConnectionString iConnectionString, ITblParityDetailsDAO iTblParityDetailsDAO, IDimStateDAO iDimStateDAO)
        {
            _iTblParityDetailsDAO = iTblParityDetailsDAO;
            _iDimStateDAO = iDimStateDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public List<TblParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, Int32 prodSpecId, Int32 stateId, Int32 brandId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblParityDetailsTO> list = null;
                if (parityId == 0)
                    list = _iTblParityDetailsDAO.SelectAllLatestParityDetails(stateId, prodSpecId, brandId, conn, tran);
                else
                {
                    list = _iTblParityDetailsDAO.SelectAllTblParityDetails(parityId, prodSpecId, conn, tran);
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblParityDetailsTO> SelectAllEmptyParityDetailsList(Int32 prodSpecId, Int32 stateId, Int32 brandId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblParityDetailsTO> list = null;
                list = _iTblParityDetailsDAO.SelectAllLatestParityDetails(stateId, prodSpecId, brandId, conn, tran);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, int prodSpecId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParityDetailsDAO.SelectAllTblParityDetails(parityId, prodSpecId, conn, tran);
        }

        public List<TblParityDetailsTO> SelectAllTblParityDetailsList(String parityIds, int prodSpecId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParityDetailsDAO.SelectAllTblParityDetails(parityIds, prodSpecId, conn, tran);
        }

        public TblParityDetailsTO SelectTblParityDetailsTO(Int32 idParityDtl)
        {
            return _iTblParityDetailsDAO.SelectTblParityDetails(idParityDtl);
        }

        //Sudhir[30-APR-2018] Added for Connection and Transaction for the Method.
        public TblParityDetailsTO SelectTblParityDetailsTO(Int32 idParityDtl,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblParityDetailsDAO.SelectTblParityDetails(idParityDtl,conn,tran);
        }

        public List<TblParityDetailsTO> SelectAllParityDetailsListByIds(String parityDtlIds, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParityDetailsDAO.SelectAllParityDetailsListByIds(parityDtlIds, conn, tran);
        }


        //Sudhir[20-MARCH-2018] Added for get List of All Parity Details List
        public List<TblParityDetailsTO> SelectAllParityDetailsList()
        {
            List<TblParityDetailsTO> ParityDetailsList = _iTblParityDetailsDAO.SelectAllParityDetails();
            if (ParityDetailsList != null && ParityDetailsList.Count > 0)
                return ParityDetailsList;
            else
                return null;

        }
        /// <summary>
        /// Sudhir[21-MARCH-2018] Added to Save Parity Details List For Other Item . 
        /// </summary>
        /// <param name="tblParitySummaryTO"></param>
        /// <returns></returns>
        public ResultMessage SaveParityDetailsOtherItem(TblParitySummaryTO tblParitySummaryTO, Int32 isForUpdate)
        {

            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                if (tblParitySummaryTO != null)
                {
                    List<TblParityDetailsTO> parityDetailsList = tblParitySummaryTO.ParityDetailList;
                    int result = -1;
                    if (parityDetailsList != null && parityDetailsList.Count > 0)
                    {

                        Int32 productItemId = parityDetailsList[0].ProdItemId;
                        Int32 brandId = parityDetailsList[0].BrandId;


                        if(isForUpdate==1)
                        {
                            //Select the TO Where parityDetailsTo isUpdate=1
                            TblParityDetailsTO parityDetailsTO = tblParitySummaryTO.ParityDetailList.Where(ele => ele.IsForUpdate == 1).FirstOrDefault();
                            if(parityDetailsTO != null)
                            {
                                result = DeactivateParityDetailsForUpdate(parityDetailsTO, conn, tran);
                                if(result ==-1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While Deactivating Updating Single Record");
                                    return resultMessage;
                                }

                                parityDetailsTO.CreatedBy = tblParitySummaryTO.CreatedBy;
                                parityDetailsTO.CreatedOn = tblParitySummaryTO.CreatedOn;
                                parityDetailsTO.IsActive = 1;
                                result = InsertTblParityDetails(parityDetailsTO, conn, tran);

                                if(result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While Inserting New parityDetails on Single Record");
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {
                            //1) Deactivate Record Against ProductItemId and BrandId Which are Aleready Active
                            for(int i=0; i<parityDetailsList.Count; i++)
                            {
                                result = DeactivateParityDetails(parityDetailsList[i], conn, tran);
                                if (result == -1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While Deactivating ParityDetails");
                                    return resultMessage;
                                }
                            }
                            

                            //2)Insert one by one into parityDetails
                            foreach (TblParityDetailsTO parityDetailsTo in parityDetailsList)
                            {
                                parityDetailsTo.CreatedBy = tblParitySummaryTO.CreatedBy;
                                parityDetailsTo.CreatedOn = tblParitySummaryTO.CreatedOn;
                                parityDetailsTo.IsActive = 1;
                               
                                result = InsertTblParityDetails(parityDetailsTo, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While Adding Record into TblParityDetails");
                                    return resultMessage;
                                }
                            }
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While Adding Record into TblParityDetails");
                                return resultMessage;
                            }
                        }

                        tran.Commit();
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Parity Details List Found Null");
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour("Parity Summary Found Null");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveParityDetailsOtherItem");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// Sudhir[21-March-2018] Added for Deactivate the ParityDetails List Based On ProductItemId And Brand Id
        /// </summary>
        /// <param name="productItemId"></param>
        /// <returns></returns>
        public Int32 DeactivateParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = -1;
            if (tblParityDetailsTO != null)
            {
                result = _iTblParityDetailsDAO.DeactivateAllParityDetails(tblParityDetailsTO,conn,tran);
            }
            return result;
        }


        /// <summary>
        /// Sudhir[21-March-2018] Added for Deactivate the ParityDetails List Based On ProductItemId And Brand Id
        /// </summary>
        /// <param name="productItemId"></param>
        /// <returns></returns>
        public Int32 DeactivateParityDetailsForUpdate(TblParityDetailsTO parityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = -1;
            if (parityDetailsTO != null)
            {
                    result = _iTblParityDetailsDAO.DeactivateAllParityDetailsForUpdate(parityDetailsTO, conn, tran);
            }
            return result;
        }

        /// <summary>
        /// Sudhir[20-March-2018] Added for Get Parity List Based on B
        /// </summary>
        /// <param name="productItemId"></param>
        /// <param name="brandId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="prodSpeecId"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        /// 
        //public List<TblParityDetailsTO> SelectAllParityDetailsToList(Int32 productItemId,Int32 brandId,Int32 prodCatId,Int32 prodSpecId,Int32 materialId)
        //{
        //    List<TblParityDetailsTO> parityDetailslist= _iTblParityDetailsDAO.SelectAllParityDetailsOnProductItemId(productItemId, brandId,prodCatId,prodSpecId,materialId);
        //    if (parityDetailslist != null && parityDetailslist.Count > 0)
        //        return parityDetailslist;
        //    else
        //        return null;
        //}

        public List<TblParityDetailsTO> SelectAllParityDetailsToList(Int32 brandId, Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo, Int32 productSpecForRegular, Int32 districtId, Int32 talukaId)
        {
            List<TblParityDetailsTO> parityDetailslist = _iTblParityDetailsDAO.SelectAllParityDetailsOnProductItemId(brandId, productItemId, prodCatId, stateId, currencyId, productSpecInfoListTo, productSpecForRegular, districtId, talukaId);
            if (parityDetailslist != null && parityDetailslist.Count > 0)
                return parityDetailslist;
            else
                return null;
        }

        //Sudhir[20-MARCH-2018] Added for get List of All Parity Details List on ProductItem Id
        //public List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 productItemId,Int32 brandId,Int32 prodCatId,Int32 prodSpecId,Int32 materialId)
        //{
        //    List<DimStateTO> allStateList = _iDimStateBL.SelectAllDimState().OrderBy(ele => ele.StateName).ToList(); ;
        //    //List<TblParityDetailsTO> allParityDetailsList = _iTblParityDetailsDAO.SelectAllParityDetails();
        //    List<TblParityDetailsTO> productItemIdparityDetailsList = SelectAllParityDetailsToList(productItemId,brandId,prodCatId,prodSpecId,materialId);
        //    List<TblParityDetailsTO> newParityDetailsList = new List<TblParityDetailsTO>();
        //    if (allStateList != null && allStateList.Count > 0)
        //    {
        //        foreach (DimStateTO dimStateTO in allStateList)
        //        {
        //            TblParityDetailsTO tblParityDetailsTO = new TblParityDetailsTO();
        //            if (productItemIdparityDetailsList != null && productItemIdparityDetailsList.Count > 0)
        //            {
        //                tblParityDetailsTO = productItemIdparityDetailsList.Where(ele => ele.ProdItemId == productItemId && ele.StateId == dimStateTO.IdState 
        //                                        && ele.IsActive == 1 && ele.BrandId==brandId && ele.ProdCatId==prodCatId 
        //                                        && ele.ProdSpecId==prodSpecId && ele.MaterialId==materialId).FirstOrDefault();

        //                if (tblParityDetailsTO == null)
        //                {
        //                    tblParityDetailsTO = new TblParityDetailsTO();
        //                    tblParityDetailsTO.StateName = dimStateTO.StateName;
        //                    tblParityDetailsTO.StateId = dimStateTO.IdState;
        //                    tblParityDetailsTO.ProdItemId = productItemId;
        //                    tblParityDetailsTO.BrandId = brandId;
        //                    tblParityDetailsTO.ProdCatId = prodCatId;
        //                    tblParityDetailsTO.ProdSpecId = prodSpecId;
        //                    tblParityDetailsTO.MaterialId = materialId;
        //                }
        //                else
        //                {
        //                    if(tblParityDetailsTO.StateId== dimStateTO.IdState)
        //                    {
        //                        tblParityDetailsTO.StateName = dimStateTO.StateName;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                tblParityDetailsTO = new TblParityDetailsTO();
        //                tblParityDetailsTO.StateName = dimStateTO.StateName;
        //                tblParityDetailsTO.StateId = dimStateTO.IdState;
        //                tblParityDetailsTO.BrandId = brandId;
        //                tblParityDetailsTO.ProdItemId = productItemId;
        //                tblParityDetailsTO.ProdCatId = prodCatId;
        //                tblParityDetailsTO.ProdSpecId = prodSpecId;
        //                tblParityDetailsTO.MaterialId = materialId;
        //            }
        //            newParityDetailsList.Add(tblParityDetailsTO);
        //        }
        //    }
        //    return newParityDetailsList;
        //}

        /// <summary>
        /// Priyanka [28-08-2018] 
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="productItemId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="stateId"></param>
        /// <param name="currencyId"></param>
        /// <param name="productSpecInfoListTo"></param>
        /// <returns></returns>
        public List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 brandId, Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo, Int32 productSpecForRegular, Int32 districtId, Int32 talukaId)
        {
            List<DimStateTO> allStateList = _iDimStateDAO.SelectAllDimState().OrderBy(ele => ele.StateName).ToList(); 
            //List<TblParityDetailsTO> allParityDetailsList = TblParityDetailsDAO.SelectAllParityDetails();
            List<TblParityDetailsTO> productItemIdparityDetailsList = SelectAllParityDetailsToList(brandId, productItemId, prodCatId, stateId, currencyId, productSpecInfoListTo, productSpecForRegular, districtId, talukaId);
            //List<TblParityDetailsTO> newParityDetailsList = new List<TblParityDetailsTO>();
            //if (allStateList != null && allStateList.Count > 0)
            //{
            //    foreach (DimStateTO dimStateTO in allStateList)
            //    {
            //        TblParityDetailsTO tblParityDetailsTO = new TblParityDetailsTO();
            //        if (productItemIdparityDetailsList != null && productItemIdparityDetailsList.Count > 0)
            //        {
            //            tblParityDetailsTO = productItemIdparityDetailsList.Where(ele => ele.BrandId == brandId && ele.ProdItemId == productItemId && ele.StateId == stateId
            //                                    && ele.IsActive == 1 && ele.BrandId == brandId && ele.ProdCatId == prodCatId ).FirstOrDefault();
            //                                    //&& ele.ProdSpecId == prodSpecId && ele.MaterialId == materialId).FirstOrDefault();

            //            if (tblParityDetailsTO == null)
            //            {
            //                tblParityDetailsTO = new TblParityDetailsTO();
            //                tblParityDetailsTO.StateName = dimStateTO.StateName;
            //                tblParityDetailsTO.StateId = dimStateTO.IdState;
            //                tblParityDetailsTO.ProdItemId = productItemId;
            //                tblParityDetailsTO.BrandId = brandId;
            //                tblParityDetailsTO.ProdCatId = prodCatId;
            //               // tblParityDetailsTO.ProdSpecId = prodSpecId;
            //                //tblParityDetailsTO.MaterialId = materialId;
            //            }
            //            else
            //            {
            //                if (tblParityDetailsTO.StateId == dimStateTO.IdState)
            //                {
            //                    tblParityDetailsTO.StateName = dimStateTO.StateName;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            //tblParityDetailsTO = new TblParityDetailsTO();
            //            //tblParityDetailsTO.StateName = dimStateTO.StateName;
            //            //tblParityDetailsTO.StateId = dimStateTO.IdState;
            //            //tblParityDetailsTO.BrandId = brandId;
            //            //tblParityDetailsTO.ProdItemId = productItemId;
            //            //tblParityDetailsTO.ProdCatId = prodCatId;
            //            //tblParityDetailsTO.ProdSpecId = prodSpecId;
            //            //tblParityDetailsTO.MaterialId = materialId;
            //        }
             //       newParityDetailsList.Add(tblParityDetailsTO);
              //  }
           // }
            return productItemIdparityDetailsList;
        }

        //Aniket [21-Jan-2019]
        public List<TblParityDetailsTO> SelectBrandForCopy(Int32 fromBrand,Int32 toBrand, Int32 currencyId, Int32 categoryId, Int32 stateId)
        {
            List<TblParityDetailsTO> list = _iTblParityDetailsDAO.SelectParityDetailsForBrand(fromBrand, toBrand, currencyId, categoryId, stateId);
            if (list != null)
                return list;
            else
            return null;
        }
        /// <summary>
        /// Sudhir[23-MARCH-2018] Added for Get ParityDetail List based on Booking DateTime and Other Combination
        /// </summary>
        /// <returns></returns>
        public TblParityDetailsTO SelectParityDetailToListOnBooking(Int32 materialId,Int32 prodCatId,Int32 prodSpecId,Int32 productItemId,Int32 brandId,Int32 stateId,DateTime boookingDate, Int32 districtId, Int32 talukaId) //02-12-2020 Dhananjay added districtId and talukaId
        {
            List<TblParityDetailsTO> parityDetailslist = _iTblParityDetailsDAO.SelectParityDetailToListOnBooking(materialId, prodCatId, prodSpecId, productItemId, brandId, stateId, boookingDate, districtId, talukaId);
            if(parityDetailslist != null && parityDetailslist.Count != 0)
            {
                TblParityDetailsTO tblParityDetailsTO = parityDetailslist.FirstOrDefault();
                return tblParityDetailsTO;
            }
            else
            {
                //Create Null To And Return That 
                TblParityDetailsTO tblParityDetailsTO =CreateEmptyParityDetailsTo(materialId, prodCatId, prodSpecId, productItemId, brandId, stateId, boookingDate);
                return tblParityDetailsTO;
            }
        }


        /// <summary>
        /// Sudhir[23-MARCH-2018] Added for Get ParityDetail List based on Booking DateTime and Other Combination
        /// </summary>
        /// <returns></returns>
        public TblParityDetailsTO CreateEmptyParityDetailsTo(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 brandId, Int32 stateId, DateTime boookingDate)
        {
            TblParityDetailsTO tblParityDetailsTO = new TblParityDetailsTO();
            tblParityDetailsTO.BrandId = brandId;
            tblParityDetailsTO.MaterialId = materialId;
            tblParityDetailsTO.ProdCatId = prodCatId;
            tblParityDetailsTO.ProdSpecId = prodSpecId;
            tblParityDetailsTO.ProdItemId = productItemId;
            tblParityDetailsTO.StateId = stateId;
            return tblParityDetailsTO;
        }

        //Aniket[29-01-2019] for copy from one brand to multiple brand, fetch parity details for selected brand
        public ResultMessage GetParityDetialsForCopyBrand(Int32 brandId, List<DropDownToForParity> selectedBrands, List<DropDownToForParity> selectedStates)
        {
            int flag = 0;
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            List<int> brandIds = new List<int>();
            List<TblParityDetailsTO> toTblParityDetailsTO = new List<TblParityDetailsTO>() ;
            List<TblParityDetailsTO> fromTblParityDetailsTO=new List<TblParityDetailsTO>();
            List<TblParityDetailsTO> tempToTblParityDetailsTO = new List<TblParityDetailsTO>();
            List<TblParityDetailsTO> tempFromTblParityDetailsTO = new List<TblParityDetailsTO>();
            int result1 = -1;
            int result2 = -1;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                foreach (var item in selectedBrands)
                {
                    brandIds.Add(item.Id);
                }
                fromTblParityDetailsTO = _iTblParityDetailsDAO.GetParityDetailsForBrand(brandId);
                //if state list is  present then work for multiple brand, and selected states
                if (selectedStates.Count > 0 && selectedStates != null)
                {

                    foreach (int id in brandIds)
                    {
                        toTblParityDetailsTO = _iTblParityDetailsDAO.GetParityDetailsForBrand(id);
                        foreach (var item in selectedStates)
                        {
                            //fromTblParityDetailsTO = fromTblParityDetailsTO.Where(x => x.StateId == item.Id).ToList();
                            //     var ax = fromTblParityDetailsTO.Where(x => x.StateId == item.Id).ToList();
                            //toTblParityDetailsTO = toTblParityDetailsTO.Where(x => x.StateId == item.Id).ToList();

                            for (int i = 0; i < fromTblParityDetailsTO.Count; i++)
                            {

                                if (item.Id == fromTblParityDetailsTO[i].StateId)
                                {
                                    tempFromTblParityDetailsTO.Add(fromTblParityDetailsTO[i]);
                                }
                            }
                            for (int i = 0; i < toTblParityDetailsTO.Count; i++)
                            {
                                if (item.Id == toTblParityDetailsTO[i].StateId)
                                {
                                    tempToTblParityDetailsTO.Add(toTblParityDetailsTO[i]);
                                }

                            }

                            foreach (TblParityDetailsTO tblParityDetailsTO in tempToTblParityDetailsTO)
                            {
                                result1 = DeactivateParityDetails(tblParityDetailsTO, conn, tran);
                            }

                            if (result1 ==1)
                            {
                                tempToTblParityDetailsTO = tempFromTblParityDetailsTO;
                                if (tempToTblParityDetailsTO != null && tempToTblParityDetailsTO.Count > 0)
                                {
                                    foreach (TblParityDetailsTO tblParityDetailsTO in tempToTblParityDetailsTO)
                                    {

                                        tempToTblParityDetailsTO.ForEach(e => e.BrandId = id);
                                        result2 = InsertTblParityDetails(tblParityDetailsTO, conn, tran);


                                    }
                                }
                             
                            }
                           

                        }
                        if (result2 == 1)
                        {
                            flag = 1;
                           
                        }

                       
                    }
                    if(flag==1)
                    {
                        tran.Commit();
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record could not be updated");
                        return resultMessage;
                    }
                }

               else  // if state list is not present then work for multiple brand, multiple state
                {
                    foreach (int id in brandIds)
                    {
                        toTblParityDetailsTO = _iTblParityDetailsDAO.GetParityDetailsForBrand(id);
                        foreach (TblParityDetailsTO tblParityDetailsTO in toTblParityDetailsTO)
                        {
                            result1 = DeactivateParityDetails(tblParityDetailsTO, conn, tran);
                        }
                    }
                    if (result1 != -1)
                    {
                        toTblParityDetailsTO = fromTblParityDetailsTO;
                        if (toTblParityDetailsTO != null && toTblParityDetailsTO.Count > 0)
                        {
                            foreach (TblParityDetailsTO tblParityDetailsTO in toTblParityDetailsTO)
                            {
                                foreach (int id in brandIds)
                                {
                                    toTblParityDetailsTO.ForEach(e => e.BrandId = id);
                                    result2 = InsertTblParityDetails(tblParityDetailsTO, conn, tran);
                                }

                            }
                        }

                        if (result2 == 1)
                        {
                            tran.Commit();
                            resultMessage.DefaultSuccessBehaviour();
                            return resultMessage;
                        }

                        else
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Record could not be updated");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record could not be updated");
                        return resultMessage;
                    }
                }
               
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetParityDetialsForCopyBrand");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }
            #endregion

            #region Insertion
            public int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        {
            return _iTblParityDetailsDAO.InsertTblParityDetails(tblParityDetailsTO);
        }

        public int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParityDetailsDAO.InsertTblParityDetails(tblParityDetailsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        {
            return _iTblParityDetailsDAO.UpdateTblParityDetails(tblParityDetailsTO);
        }

        public int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParityDetailsDAO.UpdateTblParityDetails(tblParityDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblParityDetails(Int32 idParityDtl)
        {
            return _iTblParityDetailsDAO.DeleteTblParityDetails(idParityDtl);
        }

        public int DeleteTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblParityDetailsDAO.DeleteTblParityDetails(idParityDtl, conn, tran);
        }

        #endregion

    }
}
