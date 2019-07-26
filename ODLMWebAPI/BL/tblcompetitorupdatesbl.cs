using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblCompetitorUpdatesBL : ITblCompetitorUpdatesBL
    {
        private readonly ITblCompetitorUpdatesDAO _iTblCompetitorUpdatesDAO;
        private readonly ITblOtherSourceDAO _iTblOtherSourceDAO;
        private readonly IConnectionString _iConnectionString;
        public TblCompetitorUpdatesBL(IConnectionString iConnectionString, ITblOtherSourceDAO iTblOtherSourceDAO, ITblCompetitorUpdatesDAO iTblCompetitorUpdatesDAO)
        {
            _iTblCompetitorUpdatesDAO = iTblCompetitorUpdatesDAO;
            _iTblOtherSourceDAO = iTblOtherSourceDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList()
        {
            return _iTblCompetitorUpdatesDAO.SelectAllTblCompetitorUpdates();
           
        }

        public List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList(Int32 competitorId , Int32 enteredBy ,DateTime fromDate, DateTime toDate)
        {
            return _iTblCompetitorUpdatesDAO.SelectAllTblCompetitorUpdates(competitorId, enteredBy,fromDate, toDate);

        }

        public TblCompetitorUpdatesTO SelectTblCompetitorUpdatesTO(Int32 idCompeUpdate)
        {
           return  _iTblCompetitorUpdatesDAO.SelectTblCompetitorUpdates(idCompeUpdate);
            
        }

        public List<DropDownTO> SelectCompeUpdateUserDropDown()
        {
            return _iTblCompetitorUpdatesDAO.SelectCompeUpdateUserDropDown();

        }

        public TblCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId)
        {
            return _iTblCompetitorUpdatesDAO.SelectLastPriceForCompetitorAndBrand(brandId);
        }


            #endregion

            #region Insertion
            public int InsertTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO)
            {
                tblCompetitorUpdatesTO.isActive = 1;
                return _iTblCompetitorUpdatesDAO.InsertTblCompetitorUpdates(tblCompetitorUpdatesTO);
            }

        public int InsertTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran)
        {
            tblCompetitorUpdatesTO.isActive = 1;
            return _iTblCompetitorUpdatesDAO.InsertTblCompetitorUpdates(tblCompetitorUpdatesTO, conn, tran);
        }

        public ResultMessage SaveMarketUpdate(List<TblCompetitorUpdatesTO> competitorUpdatesTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if(competitorUpdatesTOList==null || competitorUpdatesTOList.Count==0)
                {
                    tran.Rollback();
                    resultMessage.Text = "competitorUpdatesTOList Found Null : SaveMarketUpdate";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                for (int i = 0; i < competitorUpdatesTOList.Count; i++)
                {

                    if (competitorUpdatesTOList[i].OtherSourceId == 0 && competitorUpdatesTOList[i].DealerId==0)
                    {
                        TblOtherSourceTO otherSourceTO = new TblOtherSourceTO();
                        otherSourceTO.OtherDesc = competitorUpdatesTOList[i].OtherSourceOtherDesc;
                        otherSourceTO.CreatedBy = competitorUpdatesTOList[i].CreatedBy;
                        otherSourceTO.CreatedOn = competitorUpdatesTOList[i].CreatedOn;

                        result = _iTblOtherSourceDAO.InsertTblOtherSource(otherSourceTO, conn, tran);

                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error While InsertTblOtherSource : SaveMarketUpdate";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }

                        competitorUpdatesTOList[i].OtherSourceId = otherSourceTO.IdOtherSource;
                    }

                    result = InsertTblCompetitorUpdates(competitorUpdatesTOList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While InsertTblCompetitorUpdates : SaveMarketUpdate";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveMarketUpdate";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Updation
        public int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO)
        {
            return _iTblCompetitorUpdatesDAO.UpdateTblCompetitorUpdates(tblCompetitorUpdatesTO);
        }

        public int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblCompetitorUpdatesDAO.UpdateTblCompetitorUpdates(tblCompetitorUpdatesTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblCompetitorUpdates(Int32 idCompeUpdate)
        {
            return _iTblCompetitorUpdatesDAO.DeleteTblCompetitorUpdates(idCompeUpdate);
        }
       

        public int DeleteTblCompetitorUpdates(Int32 idCompeUpdate, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblCompetitorUpdatesDAO.DeleteTblCompetitorUpdates(idCompeUpdate, conn, tran);
        }

       

        #endregion

    }
}
