using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{  
    public class TblVisitPersonDetailsBL : ITblVisitPersonDetailsBL
    {
        private readonly ITblVisitPersonDetailsDAO _iTblVisitPersonDetailsDAO;
        private readonly ITblPersonDAO _iTblPersonDAO;
        private readonly IConnectionString _iConnectionString;
        public TblVisitPersonDetailsBL(IConnectionString iConnectionString , ITblPersonDAO iTblPersonDAO, ITblVisitPersonDetailsDAO iTblVisitPersonDetailsDAO)
        {
            _iTblVisitPersonDetailsDAO = iTblVisitPersonDetailsDAO;
            _iTblPersonDAO = iTblPersonDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        // Vaibhav [2-oct-2017] added to select all person visit details list
        public List<TblVisitPersonDetailsTO> SelectAllTblVisitPersonDetailsList(int visitTypeId)
        {
            List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOList = _iTblVisitPersonDetailsDAO.SelectAllTblVisitPersonDetails(visitTypeId);
            if (tblVisitPersonDetailsTOList != null)
                return tblVisitPersonDetailsTOList;
            else
                return null;
        }

        public TblVisitPersonDetailsTO SelectTblPersonVisitDetailsTO(Int32 personId)
        {
            DataTable tblPersonVisitDetailsTODT = _iTblVisitPersonDetailsDAO.SelectTblPersonVisitDetails(personId);
            List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOList = ConvertDTToList(tblPersonVisitDetailsTODT);
            if (tblVisitPersonDetailsTOList != null && tblVisitPersonDetailsTOList.Count == 1)
                return tblVisitPersonDetailsTOList[0];
            else
                return null;
        }

        public List<TblVisitPersonDetailsTO> ConvertDTToList(DataTable tblVisitPersonDetailsTODT)
        {
            List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOList = new List<TblVisitPersonDetailsTO>();
            if (tblVisitPersonDetailsTODT != null)
            {

            }
            return tblVisitPersonDetailsTOList;
        }

        // Vaibhav [8-Nov-2017] added to select all person visit details list
        public int SelectVisitPersonCount(int visitId,int personId,int personTypeId)
        {
            int personCount = _iTblVisitPersonDetailsDAO.SelectVisitPersonCount(visitId,personId,personTypeId);
            return personCount;
        }

        //Sudhir- Added for Get All Visit Person Type List.
        public List<DropDownTO> SelectAllVisitPersonTypeList()
        {
            return _iTblVisitPersonDetailsDAO.SelectAllVisitPersonTypeList();
        }

        //GetVisitPersonDropDownListOnPersonType
        public List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType)
        {
            return _iTblVisitPersonDetailsDAO.SelectVisitPersonDropDownListOnPersonType(personType);
        }
        //Sudhir[17-July-2018] Added For More Filtering Data --GetVisitPersonDropDownListOnPersonType
        public List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType, int? organizationId)
        {
            return _iTblVisitPersonDetailsDAO.SelectVisitPersonDropDownListOnPersonType(personType, organizationId);
        }
        //Hrushikesh[11/01/2019]
        public List<TblVisitPersonDetailsTO> SelectPersonDetailsForOffline(String personTypes)
        {
            return _iTblVisitPersonDetailsDAO.SelectPersonDetailsForOffline(personTypes);
        }

        #endregion

        #region Insertion
        public int InsertTblPersonVisitDetails(TblVisitPersonDetailsTO tblPersonVisitDetailsTO)
        {
            return _iTblVisitPersonDetailsDAO.InsertTblPersonVisitDetails(tblPersonVisitDetailsTO);
        }

        public int InsertTblVisitPersonDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitPersonDetailsDAO.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);
        }

        // Vaibhav [3-Oct-2017] added to insert new visit person details
        public ResultMessage SaveNewVisitPersonDetail(TblVisitPersonDetailsTO tblVisitPersonDetailsTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                result = _iTblPersonDAO.InsertTblPerson(tblVisitPersonDetailsTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error while InsertTblPerson");
                    return resultMessage;
                }
                result = InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error while InsertTblPersonVisitDetails");
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewVisitPersonDetail");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblPersonVisitDetailsTO)
        {
            return _iTblVisitPersonDetailsDAO.UpdateTblPersonVisitDetails(tblPersonVisitDetailsTO);
        }

        public int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblPersonVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitPersonDetailsDAO.UpdateTblPersonVisitDetails(tblPersonVisitDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPersonVisitDetails(Int32 personId)
        {
            return _iTblVisitPersonDetailsDAO.DeleteTblPersonVisitDetails(personId);
        }

        public int DeleteTblPersonVisitDetails(Int32 personId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVisitPersonDetailsDAO.DeleteTblPersonVisitDetails(personId, conn, tran);
        }

        #endregion

    }
}
