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
    public class DimensionBL : IDimensionBL
    {
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblLocationDAO _iTblLocationDAO;
        private readonly ITblEntityRangeDAO _iTblEntityRangeDAO;
        private readonly ITblRoleOrgSettingDAO _iTblRoleOrgSettingDAO;
        private readonly IConnectionString _iConnectionString;
        public DimensionBL(ITblRoleOrgSettingDAO iTblRoleOrgSettingDAO, ITblEntityRangeDAO iTblEntityRangeDAO, ITblLocationDAO iTblLocationDAO, ITblConfigParamsDAO iTblConfigParamsDAO, IConnectionString iConnectionString, IDimensionDAO iDimensionDAO)
        {
            _iDimensionDAO = iDimensionDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblLocationDAO = iTblLocationDAO;
            _iTblEntityRangeDAO = iTblEntityRangeDAO;
            _iTblRoleOrgSettingDAO = iTblRoleOrgSettingDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        //Sudhir[24-APR-2018] Added for Get All Organization Type.
        public List<DropDownTO> GetAllOrganizationType()
        {
            return _iDimensionDAO.SelectAllOrganizationType();
        }

        public List<DropDownTO> SelectDeliPeriodForDropDown()
        {
            return _iDimensionDAO.SelectDeliPeriodForDropDown();
        }
        //Priyanka [23-04-2019]
        public List<DropDownTO> GetSAPMasterDropDown(Int32 dimensionId)
        {
            return _iDimensionDAO.GetSAPMasterDropDown(dimensionId);
        }
        
        /// <summary>
        /// Hrishikesh[27-03-2018]Added to get district by state
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<StateMasterTO> GetDistrictsForStateMaster(int stateId)
        {
            return _iDimensionDAO.SelectDistrictForStateMaster(stateId);
        }

        public List<DropDownTO> SelectCDStructureForDropDown(Int32 moduleId=0)
        {
            //Vijaymala added[22-06-2018]
            Int32 isRsOrPerncent = 0;
            Int32 isRs = 0, isPercent = 0;

            TblConfigParamsTO tblConfigParamsTORs = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_CD_STRUCTURE_IN_RS);
            if (tblConfigParamsTORs != null)
            {
                if (Convert.ToInt32(tblConfigParamsTORs.ConfigParamVal) == 1)
                {
                    isRs = 1;
                }
            }
            TblConfigParamsTO tblConfigParamsTOPer = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_CD_STRUCTURE_IN_PERCENTAGE);
            if (tblConfigParamsTOPer != null)
            {
                if (Convert.ToInt32(tblConfigParamsTOPer.ConfigParamVal) == 1)
                {
                    isPercent = 1;
                }
            }
            if ((isRs == 0 && isPercent == 0) || (isRs == 1 && isPercent == 1))
            {
                isRsOrPerncent = 0;
            }
            else if (isRs == 1 && isPercent == 0)
            {
                isRsOrPerncent = 1;
            }
            else if (isRs == 0 && isPercent == 1)
            {
                isRsOrPerncent = 2;
            }

            return _iDimensionDAO.SelectCDStructureForDropDown(isRsOrPerncent,moduleId);
        }

        //Vijaymala added[22-06-2018]
        public DropDownTO SelectCDDropDown(Int32 cdStructureId)
        {
            return _iDimensionDAO.SelectCDDropDown(cdStructureId);
        }

        public List<Dictionary<string, string>> GetColumnName(string tableName,Int32 tableValue)
        {
            return _iDimensionDAO.GetColumnName(tableName, tableValue);
        }

        public List<DropDownTO> SelectCountriesForDropDown()
        {
            return _iDimensionDAO.SelectCountriesForDropDown();
        }

        public List<DropDownTO> SelectStatesForDropDown(int countryId)
        {
            return _iDimensionDAO.SelectStatesForDropDown(countryId);
        }
        public List<DropDownTO> SelectDistrictForDropDown(int stateId)
        {
            return _iDimensionDAO.SelectDistrictForDropDown(stateId);
        }

        public List<DropDownTO> SelectTalukaForDropDown(int districtId)
        {
            return _iDimensionDAO.SelectTalukaForDropDown(districtId);
        }

        /// <summary>
        ///Hrishikesh[27 - 03 - 2018] Added to get taluka by district
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public List<StateMasterTO> GetTalukasForStateMaster(int districtId)
        {
            return _iDimensionDAO.SelectTalukaForStateMaster(districtId);
        }

        public List<DropDownTO> SelectOrgLicensesForDropDown()
        {
            return _iDimensionDAO.SelectOrgLicensesForDropDown();
        }

        public List<DropDownTO> SelectSalutationsForDropDown()
        {
            return _iDimensionDAO.SelectSalutationsForDropDown();
        }

        public List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown()
        {
            return _iDimensionDAO.SelectRoleListWrtAreaAllocationForDropDown();

        }

        public List<DropDownTO> SelectAllSystemRoleListForDropDown()
        {
            return _iDimensionDAO.SelectAllSystemRoleListForDropDown();

        }
        public List<DropDownTO> SelectAllSystemRoleListForDropDownByUserId(Int32 userId)
        {
            return _iDimensionDAO.SelectAllSystemRoleListForDropDownByUserId(userId);

        }
        public List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId)
        {
            return _iDimensionDAO.SelectCnfDistrictForDropDown(cnfOrgId);

        }

        public List<DropDownTO> SelectAllTransportModeForDropDown()
        {
            return _iDimensionDAO.SelectAllTransportModeForDropDown();

        }

        public List<DropDownTO> SelectInvoiceTypeForDropDown()
        {
            return _iDimensionDAO.SelectInvoiceTypeForDropDown();

        }

        public List<DropDownTO> SelectInvoiceModeForDropDown()
        {
            return _iDimensionDAO.SelectInvoiceModeForDropDown();

        }
        public List<DropDownTO> SelectCurrencyForDropDown()
        {
            return _iDimensionDAO.SelectCurrencyForDropDown();

        }

        public List<DropDownTO> GetInvoiceStatusForDropDown()
        {
            return _iDimensionDAO.GetInvoiceStatusForDropDown();

        }
        //Kiran[15-MAR-2018] Added for Select All Dimension Tables from tblMasterDimension
        public List<DimensionTO> SelectAllMasterDimensionList()
        {
            return _iDimensionDAO.SelectAllMasterDimensionList();

        }

        public List<DropDownTO> SelectDefaultRoleListForDropDown()
        {
            return _iDimensionDAO.SelectDefaultRoleListForDropDown();
        }

        //Kiran[15-MAR-2018] Added for New Dimension in  Selected Dim Tables 
        public Int32 saveNewDimensional(Dictionary<string, string> tableData, string tableName)
        {
            var result = 0;
            int cnt = 0;
            string query = "INSERT INTO " + tableName + "( ";
            string values = "VALUES(";
            foreach (KeyValuePair<string, string> item in tableData)
            {
                if (cnt == 0)
                {
                    result = getIdentityOfTable(item.Key, tableName);
                    if (result > 0)
                    {
                        query += item.Key + ",";
                        values += "'" + result + "',";
                    }
                }
                else
                {
                    query += item.Key + ",";
                    values += "'" + item.Value + "',";
                }
                cnt++;
            }
            string executeQuery = query.TrimEnd(',') + ")" + values.TrimEnd(',') + ")";
            return _iDimensionDAO.InsertdimentionalData(executeQuery);
        }
        //Kiran[15-MAR-2018] Added for Update Selected Dim Tables 
        public Int32 UpdateDimensionalData(Dictionary<string, string> tableData, string tableName)
        {
            Int32 result = 0;
            string query = "UPDATE " + tableName + " SET ";
            string value = "";
            Int32 cnt = 0;
            foreach (KeyValuePair<string, string> item in tableData)
            {
                if (cnt == 0)
                {
                    value += item.Key + " = " + item.Value;
                }
                else
                {
                    string stree = string.Empty;
                    if (item.Value != null)
                        if (item.Value.Contains("PM") || item.Value.Contains("AM"))
                        {
                            DateTime dt = Convert.ToDateTime(item.Value);
                            stree = dt.ToString("yyyy-MM-dd HH:MM");
                        }
                    if (stree == string.Empty)
                        query += item.Key + " = '" + item.Value + "',";
                    else
                    {
                        query += item.Key + " = '" + stree + "',";
                        stree = string.Empty;
                    }
                }
                cnt++;
            }
            string executeQuery = query.TrimEnd(',') + " WHERE " + value;
            return _iDimensionDAO.InsertdimentionalData(executeQuery);
        }
        //Kiran[15-MAR-2018] Added for Validate Identity of Selected Tables Coloumn.
        public Int32 getIdentityOfTable(string columnName, string tableName)
        {

            var query = "SELECT name FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = " + "'" + tableName + "'";
            Int32 result = _iDimensionDAO.getidentityOfTable(query);
            if (result == 0)
            {
                return _iDimensionDAO.getMaxCountOfTable(columnName, tableName) + 1;
            }
            return 0;
        }
        public DimFinYearTO GetCurrentFinancialYear(DateTime curDate, SqlConnection conn, SqlTransaction tran)
        {
            List<DimFinYearTO> mstFinYearTOList = _iDimensionDAO.SelectAllMstFinYearList(conn, tran);
            for (int i = 0; i < mstFinYearTOList.Count; i++)
            {
                DimFinYearTO mstFinYearTO = mstFinYearTOList[i];
                if (curDate >= mstFinYearTO.FinYearStartDate &&
                    curDate <= mstFinYearTO.FinYearEndDate)
                    return mstFinYearTO;
            }

            //Means Current Financial year not found so insert it
            DateTime startDate = Constants.GetStartDateTimeOfYear(curDate);
            DateTime endDate = Constants.GetEndDateTimeOfYear(curDate);
            int finYear = startDate.Year;
            DimFinYearTO newMstFinYearTO = new DimFinYearTO();
            newMstFinYearTO.FinYearDisplayName = finYear + "-" + (finYear + 1);
            newMstFinYearTO.FinYearEndDate = endDate;
            newMstFinYearTO.IdFinYear = finYear;
            newMstFinYearTO.FinYearStartDate = startDate;
            int result = _iDimensionDAO.InsertMstFinYear(newMstFinYearTO, conn, tran);
            if (result == 1)
            {
                return newMstFinYearTO;
            }

            return null;
        }

        public DimFinYearTO GetCurrentFinancialYear(DateTime curDate)
        {
            List<DimFinYearTO> mstFinYearTOList = _iDimensionDAO.SelectAllMstFinYearList();
            for (int i = 0; i < mstFinYearTOList.Count; i++)
            {
                DimFinYearTO mstFinYearTO = mstFinYearTOList[i];
                if (curDate >= mstFinYearTO.FinYearStartDate &&
                    curDate <= mstFinYearTO.FinYearEndDate)
                    return mstFinYearTO;
                }

            //Means Current Financial year not found so insert it
            DateTime startDate = Constants.GetStartDateTimeOfYear(curDate);
            DateTime endDate = Constants.GetEndDateTimeOfYear(curDate);
            int finYear = startDate.Year;
            DimFinYearTO newMstFinYearTO = new DimFinYearTO();
            newMstFinYearTO.FinYearDisplayName = finYear + "-" + (finYear + 1);
            newMstFinYearTO.FinYearEndDate = endDate;
            newMstFinYearTO.IdFinYear = finYear;
            newMstFinYearTO.FinYearStartDate = startDate;
            int result = _iDimensionDAO.InsertMstFinYear(newMstFinYearTO);
            if (result == 1)
            {
                return newMstFinYearTO;
            }

            return null;
        }

        // Vaibhav [27-Sep-2017] added to select all reporting type list
        public List<DropDownTO> GetReportingType()
        {
            List<DropDownTO> reportingTypeList = _iDimensionDAO.SelectReportingType();
            if (reportingTypeList != null)
                return reportingTypeList;
            else
                return null;
        }

        // Vaibhav [3-Oct-2017] added to select visit issue reason list
        public List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList()
        {
            List<DimVisitIssueReasonsTO> visitIssueReasonList = _iDimensionDAO.SelectVisitIssueReasonsList();
            if (visitIssueReasonList != null)
                return visitIssueReasonList;
            else
                return null;
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details 
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectBrandList(int categoryType)
        {
            return _iDimensionDAO.SelectBrandList(categoryType);
        }
        public List<DimBrandTO> SelectBrandListV2()
        {
            return _iDimensionDAO.SelectBrandListV2();
        }

        /// <summary>
        /// [2018-01-02]Vijaymala:Added to get loading layer list  
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectLoadingLayerList()
        {
            return _iDimensionDAO.SelectLoadingLayerList();
        }
        public List<DropDownTO> GetBookingTaxCategoryList()
        {
            return _iDimensionDAO.GetBookingTaxCategoryList();
        }

        public List<DropDownTO> GetBookingCommentCategoryList()
        {
            return _iDimensionDAO.GetBookingCommentCategoryList();
        }
        // Vijaymala [09-11-2017] added to get state Code
        public DropDownTO SelectStateCode(Int32 stateId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DropDownTO dropDownTO = _iDimensionDAO.SelectStateCode(stateId);
                if (dropDownTO != null)
                    return dropDownTO;
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
        }

        public List<DropDownTO> GetItemProductCategoryListForDropDown()
        {
            return _iDimensionDAO.GetItemProductCategoryListForDropDown();
        }

        //Sudhir[22-01-2018] Added  for GetInvoiceStatusList.
        public List<DropDownTO> GetInvoiceStatusDropDown()
        {
            return _iDimensionDAO.GetInvoiceStatusDropDown();
        }

        //Sudhir[07-MAR-2018] Added for Get All Firm List.
        public List<DropDownTO> GetAllFirmTypesForDropDown()
        {
            return _iDimensionDAO.SelectAllFirmTypesForDropDown();
        }

        //Sudhir[07-MAR-2018] Added for Get All Firm List.
        public List<DropDownTO> GetAllInfluencerTypesForDropDown()
        {
            return _iDimensionDAO.SelectAllInfluencerTypesForDropDown();
        }

        //Sudhir[15-MAR-2018] Added for Select All Enquiry Channels  dimEnqChannel
        public List<DropDownTO> SelectAllEnquiryChannels()
        {
            return _iDimensionDAO.SelectAllEnquiryChannels();
        }

        //Sudhir[15-MAR-2018] Added for Select All Industry Sector.
        public List<DropDownTO> SelectAllIndustrySector()
        {
            return _iDimensionDAO.SelectAllIndustrySector();
        }

        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetCallBySelfForDropDown()
        {
            return _iDimensionDAO.GetCallBySelfForDropDown();
        }

        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetArrangeForDropDown()
        {
            return _iDimensionDAO.GetArrangeForDropDown();
        }


        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetArrangeVisitToDropDown()
        {
            return _iDimensionDAO.GetArrangeVisitToDropDown();
        }

        public List<DropDownTO> GetLocationWiseCompartmentList()
        {
            try
            {
                List<TblLocationTO> tblLocationTOList = _iTblLocationDAO.SelectAllTblLocation().FindAll(ele => ele.ParentLocId > 0);
                if (tblLocationTOList != null && tblLocationTOList.Count > 0)
                {
                    List<DropDownTO> compartmentList = new List<Models.DropDownTO>();
                    for (int i = 0; i < tblLocationTOList.Count; i++)
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        dropDownTO.Text = tblLocationTOList[i].LocationDesc;
                        dropDownTO.Value = tblLocationTOList[i].IdLocation;
                        dropDownTO.Tag = tblLocationTOList[i].ParentLocationDesc;
                        compartmentList.Add(dropDownTO);
                    }
                    return compartmentList;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Vijaymala[28-03-2018] Added to get Address Type
        public List<DropDownTO> SelectAddressTypeListForDropDown()
        {
            return _iDimensionDAO.SelectAddressTypeListForDropDown();

        }

        //Dipali[26-07-2018] For RoleOrgTo List Mapping With Role
        public List<RoleOrgTO> SelectAllSystemRoleListForTbl(int visitTypeId, int personTypeId)
        {
            List<RoleOrgTO> roleOrgTOList = new List<RoleOrgTO>();

            List<DropDownTO> list = new List<DropDownTO>();
            List<DropDownTO> listSaved = new List<DropDownTO>();
            listSaved = _iTblRoleOrgSettingDAO.SelectSavedRoles(visitTypeId, personTypeId);
            list = _iDimensionDAO.SelectAllSystemRoleListForDropDown();


            for (int i = 0; i < list.Count; i++)
            {
                RoleOrgTO roleorgTO = new RoleOrgTO();
                roleorgTO.Role = list[i].Text;
                roleorgTO.RoleId = list[i].Value;
                if (listSaved != null)
                {
                    for (int j = 0; j < listSaved.Count; j++)
                    {
                        if (listSaved[j].Value == list[i].Value)
                        {
                            if (listSaved[j].Tag.ToString() == "1")
                                roleorgTO.Status = true;

                            else
                                roleorgTO.Status = false;
                        }
                    }
                }
                roleOrgTOList.Add(roleorgTO);
            }

            return roleOrgTOList;
        }
        public List<RoleOrgTO> SelectAllSystemOrgListForTbl(int visitTypeId, int personTypeId)
        {
            List<RoleOrgTO> roleOrgTOList = new List<RoleOrgTO>();

            List<DropDownTO> list = new List<DropDownTO>();
            list = _iDimensionDAO.SelectAllOrganizationType();
            List<DropDownTO> listSaved = new List<DropDownTO>();
            listSaved = _iTblRoleOrgSettingDAO.SelectSavedOrg(visitTypeId, personTypeId);



            for (int i = 0; i < list.Count; i++)
            {
                RoleOrgTO roleorgTO = new RoleOrgTO();
                roleorgTO.Org = list[i].Text;
                roleorgTO.OrgId = list[i].Value;
                if (listSaved != null)
                {
                    for (int j = 0; j < listSaved.Count; j++)
                    {
                        if (listSaved[j].Value == list[i].Value)
                        {
                            if (listSaved[j].Tag.ToString() == "1")
                                roleorgTO.Status = true;

                            else
                                roleorgTO.Status = false;
                        }
                    }
                }

                roleOrgTOList.Add(roleorgTO);
            }
            return roleOrgTOList;
        }
        public List<DropDownTO> SelectAllVisitTypeListForDropDown()
        {
            return _iDimensionDAO.SelectAllVisitTypeListForDropDown();
        }

        public List<DropDownTO> GetFixedDropDownValues()
        {
            return _iDimensionDAO.GetFixedDropDownList();
        }

        public List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId)
        {
            return _iDimensionDAO.SelectMasterSiteTypes(parentSiteTypeId);
        }
        #endregion

        #region Insertion

        public int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimensionDAO.InsertTaluka(commonDimensionsTO, conn, tran);
        }

        public int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimensionDAO.InsertDistrict(commonDimensionsTO, conn, tran);
        }
        #endregion

        #region Execute Command

        public int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimensionDAO.ExecuteGivenCommand(cmdStr, conn, tran);
        }


        #endregion

        public TblEntityRangeTO SelectEntityRangeTOFromVisitType(string entityName, DateTime createdOn)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                DimFinYearTO curFinYearTO = GetCurrentFinancialYear(createdOn, conn, tran);
                if (curFinYearTO == null)
                {
                    tran.Rollback();
                    return null;
                }
                TblEntityRangeTO EntityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityName, curFinYearTO.IdFinYear, conn, tran);
                EntityRangeTO.EntityPrevValue = EntityRangeTO.EntityPrevValue + EntityRangeTO.IncrementBy;
                result = _iTblEntityRangeDAO.UpdateTblEntityRange(EntityRangeTO);
                if (result == 0)
                {
                    tran.Rollback();
                    return null;
                }
                tran.Commit();
                return EntityRangeTO;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// Deepali[19-10-2018]added :to get Department wise Users
        ///
        public List<DropDownTO> GetUserListDepartmentWise(string deptId)
        {
            return _iDimensionDAO.GetUserListDepartmentWise(deptId);

        }

        public List<DimExportTypeTO> GetExportTypeList()
        {
            return _iDimensionDAO.GetExportTypeList();
        }
        public List<DimIndustrySegmentTO> GetIndustryTypeList()
        {
            return _iDimensionDAO.GetIndustryTypeList();
        }

        public List<DimIndustrySegmentTypeTO> GetIndustrySegmentTypeList(Int32 industrySegmentId)
        {
            return _iDimensionDAO.GetIndustrySegmentTypeList(industrySegmentId);
        }
        /// <summary>
        /// Vijaymala[08-09-2018]added :to get state from booking
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>

        public List<DropDownTO> SelectStatesForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate)
        {
            return _iDimensionDAO.SelectStatesForDropDownAccToBooking(countryId, fromDate, toDate);
        }

        /// <summary>
        /// Vijaymala[08-09-2018]added : to get district from booking
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDistrictForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate)
        {
            return _iDimensionDAO.SelectDistrictForDropDownAccToBooking(countryId, fromDate, toDate);
        }

        //Aniket [01-02-2019] to fetch multiple copy details for invoice
        public  List<DropDownTO> SelectInvoiceCopyList()
        {
            return _iDimensionDAO.SelectAllInvoiceCopyList();
        }
    }
}
