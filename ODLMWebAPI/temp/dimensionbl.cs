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

namespace ODLMWebAPI.BL
{
    public class DimensionBL : IDimensionBL
    {

        #region Selection

        //Sudhir[24-APR-2018] Added for Get All Organization Type.
        public List<DropDownTO> GetAllOrganizationType()
        {
            return DimensionDAO.SelectAllOrganizationType();
        }

        public List<DropDownTO> SelectDeliPeriodForDropDown()
        {
            return DimensionDAO.SelectDeliPeriodForDropDown();
        }
        /// <summary>
        /// Hrishikesh[27-03-2018]Added to get district by state
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<StateMasterTO> GetDistrictsForStateMaster(int stateId)
        {
            return DimensionDAO.SelectDistrictForStateMaster(stateId);
        }

        public List<DropDownTO> SelectCDStructureForDropDown()
        {
            //Vijaymala added[22-06-2018]
            Int32 isRsOrPerncent = 0;
            Int32 isRs = 0, isPercent = 0;

            TblConfigParamsTO tblConfigParamsTORs = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_CD_STRUCTURE_IN_RS);
            if (tblConfigParamsTORs != null)
            {
                if (Convert.ToInt32(tblConfigParamsTORs.ConfigParamVal) == 1)
                {
                    isRs = 1;
                }
            }
            TblConfigParamsTO tblConfigParamsTOPer = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_CD_STRUCTURE_IN_PERCENTAGE);
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

            return DimensionDAO.SelectCDStructureForDropDown(isRsOrPerncent);
        }

        //Vijaymala added[22-06-2018]
        public DropDownTO SelectCDDropDown(Int32 cdStructureId)
        {
            return DimensionDAO.SelectCDDropDown(cdStructureId);
        }

        public List<Dictionary<string, string>> GetColumnName(string tableName)
        {
            return DimensionDAO.GetColumnName(tableName);
        }

        public List<DropDownTO> SelectCountriesForDropDown()
        {
            return DimensionDAO.SelectCountriesForDropDown();
        }

        public List<DropDownTO> SelectStatesForDropDown(int countryId)
        {
            return DimensionDAO.SelectStatesForDropDown(countryId);
        }
        public List<DropDownTO> SelectDistrictForDropDown(int stateId)
        {
            return DimensionDAO.SelectDistrictForDropDown(stateId);
        }

        public List<DropDownTO> SelectTalukaForDropDown(int districtId)
        {
            return DimensionDAO.SelectTalukaForDropDown(districtId);
        }

        /// <summary>
        ///Hrishikesh[27 - 03 - 2018] Added to get taluka by district
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public List<StateMasterTO> GetTalukasForStateMaster(int districtId)
        {
            return DimensionDAO.SelectTalukaForStateMaster(districtId);
        }

        public List<DropDownTO> SelectOrgLicensesForDropDown()
        {
            return DimensionDAO.SelectOrgLicensesForDropDown();
        }

        public List<DropDownTO> SelectSalutationsForDropDown()
        {
            return DimensionDAO.SelectSalutationsForDropDown();
        }

        public List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown()
        {
            return DimensionDAO.SelectRoleListWrtAreaAllocationForDropDown();

        }

        public List<DropDownTO> SelectAllSystemRoleListForDropDown()
        {
            return DimensionDAO.SelectAllSystemRoleListForDropDown();

        }

        public List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId)
        {
            return DimensionDAO.SelectCnfDistrictForDropDown(cnfOrgId);

        }

        public List<DropDownTO> SelectAllTransportModeForDropDown()
        {
            return DimensionDAO.SelectAllTransportModeForDropDown();

        }

        public List<DropDownTO> SelectInvoiceTypeForDropDown()
        {
            return DimensionDAO.SelectInvoiceTypeForDropDown();

        }

        public List<DropDownTO> SelectInvoiceModeForDropDown()
        {
            return DimensionDAO.SelectInvoiceModeForDropDown();

        }
        public List<DropDownTO> SelectCurrencyForDropDown()
        {
            return DimensionDAO.SelectCurrencyForDropDown();

        }

        public List<DropDownTO> GetInvoiceStatusForDropDown()
        {
            return DimensionDAO.GetInvoiceStatusForDropDown();

        }
        //Kiran[15-MAR-2018] Added for Select All Dimension Tables from tblMasterDimension
        public List<DropDownTO> SelectAllMasterDimensionList()
        {
            return DimensionDAO.SelectAllMasterDimensionList();

        }

        public List<DropDownTO> SelectDefaultRoleListForDropDown()
        {
            return DimensionDAO.SelectDefaultRoleListForDropDown();
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
            return DimensionDAO.InsertdimentionalData(executeQuery);
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
            return DimensionDAO.InsertdimentionalData(executeQuery);
        }
        //Kiran[15-MAR-2018] Added for Validate Identity of Selected Tables Coloumn.
        public Int32 getIdentityOfTable(string columnName, string tableName)
        {

            var query = "SELECT name FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = " + "'" + tableName + "'";
            Int32 result = DimensionDAO.getidentityOfTable(query);
            if (result == 0)
            {
                return DimensionDAO.getMaxCountOfTable(columnName, tableName) + 1;
            }
            return 0;
        }
        public DimFinYearTO GetCurrentFinancialYear(DateTime curDate, SqlConnection conn, SqlTransaction tran)
        {
            List<DimFinYearTO> mstFinYearTOList = DimensionDAO.SelectAllMstFinYearList(conn, tran);
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
            int result = DimensionDAO.InsertMstFinYear(newMstFinYearTO, conn, tran);
            if (result == 1)
            {
                return newMstFinYearTO;
            }

            return null;
        }



        // Vaibhav [27-Sep-2017] added to select all reporting type list
        public List<DropDownTO> GetReportingType()
        {
            List<DropDownTO> reportingTypeList = DAL.DimensionDAO.SelectReportingType();
            if (reportingTypeList != null)
                return reportingTypeList;
            else
                return null;
        }

        // Vaibhav [3-Oct-2017] added to select visit issue reason list
        public List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList()
        {
            List<DimVisitIssueReasonsTO> visitIssueReasonList = DimensionDAO.SelectVisitIssueReasonsList();
            if (visitIssueReasonList != null)
                return visitIssueReasonList;
            else
                return null;
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details 
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectBrandList()
        {
            return DimensionDAO.SelectBrandList();
        }


        /// <summary>
        /// [2018-01-02]Vijaymala:Added to get loading layer list  
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectLoadingLayerList()
        {
            return DimensionDAO.SelectLoadingLayerList();
        }

        // Vijaymala [09-11-2017] added to get state Code
        public DropDownTO SelectStateCode(Int32 stateId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DropDownTO dropDownTO = DimensionDAO.SelectStateCode(stateId);
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
            return DimensionDAO.GetItemProductCategoryListForDropDown();
        }

        //Sudhir[22-01-2018] Added  for GetInvoiceStatusList.
        public List<DropDownTO> GetInvoiceStatusDropDown()
        {
            return DimensionDAO.GetInvoiceStatusDropDown();
        }

        //Sudhir[07-MAR-2018] Added for Get All Firm List.
        public List<DropDownTO> GetAllFirmTypesForDropDown()
        {
            return DimensionDAO.SelectAllFirmTypesForDropDown();
        }

        //Sudhir[07-MAR-2018] Added for Get All Firm List.
        public List<DropDownTO> GetAllInfluencerTypesForDropDown()
        {
            return DimensionDAO.SelectAllInfluencerTypesForDropDown();
        }

        //Sudhir[15-MAR-2018] Added for Select All Enquiry Channels  dimEnqChannel
        public List<DropDownTO> SelectAllEnquiryChannels()
        {
            return DimensionDAO.SelectAllEnquiryChannels();
        }

        //Sudhir[15-MAR-2018] Added for Select All Industry Sector.
        public List<DropDownTO> SelectAllIndustrySector()
        {
            return DimensionDAO.SelectAllIndustrySector();
        }

        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetCallBySelfForDropDown()
        {
            return DimensionDAO.GetCallBySelfForDropDown();
        }

        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetArrangeForDropDown()
        {
            return DimensionDAO.GetArrangeForDropDown();
        }


        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetArrangeVisitToDropDown()
        {
            return DimensionDAO.GetArrangeVisitToDropDown();
        }

        public List<DropDownTO> GetLocationWiseCompartmentList()
        {
            try
            {
                List<TblLocationTO> tblLocationTOList = BL.TblLocationBL.SelectAllTblLocationList().FindAll(ele => ele.ParentLocId > 0);
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
            return DimensionDAO.SelectAddressTypeListForDropDown();

        }

        //Dipali[26-07-2018] For RoleOrgTo List Mapping With Role
        public List<RoleOrgTO> SelectAllSystemRoleListForTbl(int visitTypeId, int personTypeId)
        {
            List<RoleOrgTO> roleOrgTOList = new List<RoleOrgTO>();

            List<DropDownTO> list = new List<DropDownTO>();
            List<DropDownTO> listSaved = new List<DropDownTO>();
            listSaved = TblRoleOrgSettingBL.SelectSavedRoles(visitTypeId, personTypeId);
            list = DimensionDAO.SelectAllSystemRoleListForDropDown();


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
            list = DimensionDAO.SelectAllOrganizationType();
            List<DropDownTO> listSaved = new List<DropDownTO>();
            listSaved = TblRoleOrgSettingBL.SelectSavedOrg(visitTypeId, personTypeId);



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
            return DimensionDAO.SelectAllVisitTypeListForDropDown();
        }

        public List<DropDownTO> GetFixedDropDownValues()
        {
            return DimensionDAO.GetFixedDropDownList();
        }

        public List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId)
        {
            return DimensionDAO.SelectMasterSiteTypes(parentSiteTypeId);
        }
        #endregion

        #region Insertion

        public int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimensionDAO.InsertTaluka(commonDimensionsTO, conn, tran);
        }

        public int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimensionDAO.InsertDistrict(commonDimensionsTO, conn, tran);
        }
        #endregion

        #region Execute Command

        public int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran)
        {
            return DimensionDAO.ExecuteGivenCommand(cmdStr, conn, tran);
        }


        #endregion

        public TblEntityRangeTO SelectEntityRangeTOFromVisitType(string entityName, DateTime createdOn)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
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
                TblEntityRangeTO EntityRangeTO = BL.TblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(entityName, curFinYearTO.IdFinYear, conn, tran);
                EntityRangeTO.EntityPrevValue = EntityRangeTO.EntityPrevValue + EntityRangeTO.IncrementBy;
                result = BL.TblEntityRangeBL.UpdateTblEntityRange(EntityRangeTO);
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
        internal List<DropDownTO> GetUserListDepartmentWise(string deptId)
        {
            return DimensionDAO.GetUserListDepartmentWise(deptId);

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
            return DimensionDAO.SelectStatesForDropDownAccToBooking(countryId, fromDate, toDate);
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
            return DimensionDAO.SelectDistrictForDropDownAccToBooking(countryId, fromDate, toDate);
        }

    }
}
