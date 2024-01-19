using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
 
namespace ODLMWebAPI.DAL
{
    public class DimensionDAO : IDimensionDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimensionDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public List<DropDownTO> SelectDeliPeriodForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimDelPeriod WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idDelPeriod"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idDelPeriod"].ToString());
                    if (dateReader["deliveryPeriod"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["deliveryPeriod"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }
        //Priyanka [23-04-2019] : Added for get drop down list for SAP
        public List<DropDownTO> GetSAPMasterDropDown(Int32 dimensionId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String SqlQuery = "SELECT * FROM dimGenericMaster WHERE dimensionId =" + dimensionId +" AND isActive=1";

                cmdSelect = new SqlCommand(SqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {

                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSAPMaster"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSAPMaster"].ToString());
                    if (dateReader["value"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["value"].ToString());
                    if (dateReader["dimensionId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["dimensionId"].ToString());
                  
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<TblProdGstCodeDtlsTO> GetSAPTaxCodeByIdProdGstCode(int idProdGstCode)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                string Query = "select tblTaxRates.sapTaxCode, tblTaxRates.taxTypeId, tblProdGstCodeDtls.idProdGstCode from tblProdGstCodeDtls tblProdGstCodeDtls " +
                " JOIN tblTaxRates tblTaxRates on tblTaxRates.gstCodeId = tblProdGstCodeDtls.gstCodeId " +
                " where tblProdGstCodeDtls.idProdGstCode = " + idProdGstCode + "";

                cmdSelect = new SqlCommand(Query, conn);
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader tblProdGstCodeDtlsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdGstCodeDtlsTO> list = new List<TblProdGstCodeDtlsTO>();
                while (tblProdGstCodeDtlsTODT.Read())
                {
                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTONew = new TblProdGstCodeDtlsTO();
                    if (tblProdGstCodeDtlsTODT["idProdGstCode"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.IdProdGstCode = Convert.ToInt32(tblProdGstCodeDtlsTODT["idProdGstCode"].ToString());
                    if (tblProdGstCodeDtlsTODT["taxTypeId"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.TaxTypeId = Convert.ToInt32(tblProdGstCodeDtlsTODT["taxTypeId"].ToString());
                    if (tblProdGstCodeDtlsTODT["sapTaxCode"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.SapTaxCode = Convert.ToString(tblProdGstCodeDtlsTODT["sapTaxCode"].ToString());
                    list.Add(tblProdGstCodeDtlsTONew);
                }

                if (tblProdGstCodeDtlsTODT != null)
                    tblProdGstCodeDtlsTODT.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public Int64 GetProductItemIdFromGivenRMDetails(int prodCatId, int prodSpecId, int materialId, int brandId, int rmProdItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            SqlDataReader dateReader = null;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM  tblProductItemRmToFGConfig WHERE  isFgToFgMapping=1 AND isActive=1 AND ISNULL(prodCatId,0)=" + prodCatId + " AND ISNULL(prodSpecId,0)=" + prodSpecId +
                          " AND ISNULL(materialId,0)=" + materialId + " AND ISNULL(brandId,0)=" + brandId + " AND ISNULL(rmProductItemId,0)=" + rmProdItemId;

                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();  // Return first ever record as above condition should produce single record in the table
                    if (dateReader["fgProductItemId"] != DBNull.Value)
                        return Convert.ToInt64(dateReader["fgProductItemId"].ToString());
                }

                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<Dictionary<string, string>> GetColumnName(string tablename, Int32 tableValue)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlCommand cmdtblSelect = null;
            String aqlQuery = null;
            try
            {
                conn.Open();
                if (tableValue > 0)
                {
                    aqlQuery = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + "'" + tablename + "'" + " ORDER BY ORDINAL_POSITION;"+
                               "SELECT sapMaster.* from "+ tablename + " sapMaster " +
                               "LEFT JOIN tblMasterDimension mstDimension ON mstDimension.idDimension = sapMaster.dimensionId " +
                               "WHERE sapMaster.dimensionId = " + tableValue;
                }
                else
                {
                    aqlQuery = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + "'" + tablename + "'" + " ORDER BY ORDINAL_POSITION; SELECT * from " + tablename + "";

                }
                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<string> columnName = new List<string>();
                List<Dictionary<string, string>> main = new List<Dictionary<string, string>>();
                while (dateReader.Read())
                {
                    if (dateReader["COLUMN_NAME"] != DBNull.Value)
                        columnName.Add(Convert.ToString(dateReader["COLUMN_NAME"]));
                }
                if (dateReader.NextResult())
                {
                    if (dateReader.HasRows)
                    {
                        while (dateReader.Read())
                        {
                            Dictionary<string, string> hh = new Dictionary<string, string>();
                            for (int i = 0; i < columnName.Count; i++)
                            {
                                hh.Add(columnName[i], Convert.ToString(dateReader[columnName[i]]));
                            }
                            main.Add(hh);
                        }
                    }
                    else
                    {
                        Dictionary<string, string> hh = new Dictionary<string, string>();
                        for (int i = 0; i < columnName.Count; i++)
                        {
                            hh.Add(columnName[i], null);
                        }
                        main.Add(hh);
                    }

                }

                if (dateReader != null)
                    dateReader.Dispose();

                return main;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }


        }

        public Int32 InsertdimentionalData(string tableQuery)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                cmdInsert.CommandText = tableQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                return cmdInsert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public List<DimensionTO> SelectAllMasterDimensionList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblMasterDimension";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimensionTO> dropDownTOList = new List<Models.DimensionTO>();
                while (dateReader.Read())
                {
                    DimensionTO dimensionTONew = new DimensionTO();
                    if (dateReader["idDimension"] != DBNull.Value)
                        dimensionTONew.IdDimension = Convert.ToInt32(dateReader["idDimension"].ToString());
                    if (dateReader["displayName"] != DBNull.Value)
                        dimensionTONew.DisplayName = Convert.ToString(dateReader["displayName"].ToString());
                    if (dateReader["dimensionValue"] != DBNull.Value)
                        dimensionTONew.DimensionValue = Convert.ToString(dateReader["dimensionValue"].ToString());
                    if (dateReader["isActive"] != DBNull.Value)
                        dimensionTONew.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());
                    if (dateReader["isGeneric"] != DBNull.Value)
                        dimensionTONew.IsGeneric = Convert.ToInt32(dateReader["isGeneric"].ToString());
                    dropDownTOList.Add(dimensionTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public Int32 getidentityOfTable(string Query)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                cmdSelect = new SqlCommand(Query, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (dateReader.HasRows)
                {
                    return 1;
                }
                dateReader.Dispose();
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public Int32 getMaxCountOfTable(string CoulumName, string tableName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                string Query = " select max(" + CoulumName + ") as cnt from " + tableName;
                cmdSelect = new SqlCommand(Query, conn);
                object dateReader = cmdSelect.ExecuteScalar();
                return Convert.ToInt32(dateReader);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectCDStructureForDropDown(Int32 isRsOrPerncent, Int32 moduleId=0)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimCdStructure WHERE isActive=1";

                if (isRsOrPerncent == 1)
                {
                    sqlQuery += " AND isPercent=0";
                }
                else if (isRsOrPerncent == 2)
                {
                    sqlQuery += " AND isPercent=1";
                }
                if(moduleId>0)
                {
                    sqlQuery += " And moduleId= " + moduleId;
                }

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCdStructure"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCdStructure"].ToString());
                    if (dateReader["cdValue"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["cdValue"].ToString());
                    if (dateReader["isPercent"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["isPercent"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }


        public List<DropDownTO> SelectCountriesForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCountry";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCountry"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCountry"].ToString());
                    if (dateReader["countryName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["countryName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectOrgLicensesForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCommerLicenceInfo WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idLicense"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idLicense"].ToString());
                    if (dateReader["licenseName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["licenseName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectSalutationsForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimSalutation";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSalutation"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSalutation"].ToString());
                    if (dateReader["salutationDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["salutationDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }
        /// <summary>
        /// Hrishikesh[27 - 03 - 2018] Added to get district by state
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>

        public List<StateMasterTO> SelectDistrictForStateMaster(int stateId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (stateId > 0)
                    sqlQuery = "SELECT * FROM dimDistrict WHERE stateId=" + stateId;
                else
                    sqlQuery = "SELECT * FROM dimDistrict ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<StateMasterTO> dropDownTOList = new List<Models.StateMasterTO>();
                while (dateReader.Read())
                {
                    StateMasterTO dropDownTONew = new StateMasterTO();
                    if (dateReader["idDistrict"] != DBNull.Value)
                        dropDownTONew.Id = Convert.ToInt32(dateReader["idDistrict"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Name = Convert.ToString(dateReader["districtName"].ToString());
                    if (dateReader["stateId"] != DBNull.Value)
                        dropDownTONew.ParentId = Convert.ToInt32(dateReader["stateId"].ToString());
                    if (dateReader["districtCode"] != DBNull.Value)
                        dropDownTONew.Code = Convert.ToString(dateReader["districtCode"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DropDownTO> SelectDistrictForDropDown(int stateId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (stateId > 0)
                    sqlQuery = "SELECT * FROM dimDistrict WHERE isActive = 1 and stateId=" + stateId;
                else
                    sqlQuery = "SELECT * FROM dimDistrict WHERE isActive = 1 ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idDistrict"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idDistrict"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["districtName"].ToString());
                    if (dateReader["stateId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["stateId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectStatesForDropDown(int countryId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (countryId > 0)
                    sqlQuery = "SELECT * FROM dimState ";  //No where condition. As we dont have country column in states
                else
                    sqlQuery = "SELECT * FROM dimState ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idState"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idState"].ToString());
                    if (dateReader["stateName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["stateName"].ToString());
                    if (dateReader["stateOrUTCode"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["stateOrUTCode"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectTalukaForDropDown(int districtId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (districtId > 0)
                    sqlQuery = "SELECT * FROM dimTaluka WHERE districtId=" + districtId;
                else
                    sqlQuery = "SELECT * FROM dimTaluka ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idTaluka"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idTaluka"].ToString());
                    if (dateReader["talukaName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["talukaName"].ToString());

                    if (dateReader["districtId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["districtId"].ToString());
                    dropDownTOList.Add(dropDownTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }
        /// <summary>
        /// Deepali[19-10-2018]added :to get Department wise Users

        public List<DropDownTO> GetUserListDepartmentWise(string deptId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "select URD.*,U.idUser,U.userDisplayName from tblUserReportingDetails URD left join tblOrgStructure OS"
                                   + " on OS.idOrgStructure = URD.orgStructureId "
                                   + "left join tblUser U on u.idUser = URD.userId "
                                   + "where U.isActive = 1 and OS.isActive = 1 and URD.isActive = 1 ";

                 aqlQuery =aqlQuery + "and OS.deptId IN (" + deptId +")" ;                  

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idUser"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }


        }

        /// <summary>
        /// Hrishikesh[27 - 03 - 2018] Added to get taluka by district
        /// 
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public List<StateMasterTO> SelectTalukaForStateMaster(int districtId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (districtId > 0)
                    sqlQuery = "SELECT * FROM dimTaluka WHERE districtId=" + districtId;
                else
                    sqlQuery = "SELECT * FROM dimTaluka ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<StateMasterTO> dropDownTOList = new List<Models.StateMasterTO>();
                while (dateReader.Read())
                {
                    StateMasterTO dropDownTONew = new StateMasterTO();
                    if (dateReader["idTaluka"] != DBNull.Value)
                        dropDownTONew.Id = Convert.ToInt32(dateReader["idTaluka"].ToString());
                    if (dateReader["talukaName"] != DBNull.Value)
                        dropDownTONew.Name = Convert.ToString(dateReader["talukaName"].ToString());

                    if (dateReader["districtId"] != DBNull.Value)
                        dropDownTONew.ParentId = Convert.ToInt32(dateReader["districtId"].ToString());

                    if (dateReader["talukaCode"] != DBNull.Value)
                        dropDownTONew.Code = Convert.ToString(dateReader["talukaCode"].ToString());
                    dropDownTOList.Add(dropDownTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblRole WHERE enableAreaAlloc=1 AND isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }


        public List<DropDownTO> SelectAllSystemRoleListForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblRole WHERE  isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }
        //Aniket[1-7-2019]
        public List<DropDownTO> SelectAllSystemRoleListForDropDownByUserId(Int32 userId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "select idRole,roleDesc from tblRole r "+
                                   " where r.isActive = 1 and r.idRole IN "+
                    " (select roleId from tblUserRole where userId = @UserId and isActive = 1)";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                cmdSelect.Parameters.AddWithValue("@UserId", DbType.Int32).Value = userId;
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }
        public List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = " SELECT distinct districtId,dimDistrict.districtName FROM tblOrganization  " +
                           " LEFT JOIN tblOrgAddress ON idOrganization = organizationId " +
                           " LEFT JOIN tblAddress ON idAddr = addressId " +
                           " LEFT JOIN dimDistrict ON idDistrict = districtId " +
                           " WHERE tblOrganization.isActive=1 AND tblOrganization.idOrganization IN(SELECT dealerOrgId FROM tblCnfDealers WHERE cnfOrgId=" + cnfOrgId + " and isActive=1) " +
                           " ORDER BY dimDistrict.districtName ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["districtId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["districtId"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["districtName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectAllTransportModeForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimTransportMode WHERE  isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idTransMode"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idTransMode"].ToString());
                    if (dateReader["transportMode"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["transportMode"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectInvoiceTypeForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceTypes WHERE  isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvoiceType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvoiceType"].ToString());
                    if (dateReader["invoiceTypeDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["invoiceTypeDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public String SelectInvoiceEntityNameByInvoiceTypeId(Int32 idInvoiceType)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT entityName FROM dimInvoiceTypes WHERE  isActive=1 AND idInvoiceType = " + idInvoiceType;

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                String entityName = String.Empty;
                while (dateReader.Read())
                {
                    if (dateReader["entityName"] != DBNull.Value)
                        entityName = Convert.ToString(dateReader["entityName"].ToString());
                }
                return entityName;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }


        public List<DropDownTO> SelectInvoiceModeForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceMode WHERE  1=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvoiceMode"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvoiceMode"].ToString());
                    if (dateReader["invoiceMode"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["invoiceMode"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectCurrencyForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCurrency";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCurrency"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCurrency"].ToString());
                    if (dateReader["currencyName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["currencyName"].ToString());
                    if (dateReader["currencySymbol"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["currencySymbol"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> GetInvoiceStatusForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCurrency";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCurrency"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCurrency"].ToString());
                    if (dateReader["currencyName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["currencyName"].ToString());
                    if (dateReader["currencySymbol"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["currencySymbol"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DimFinYearTO> SelectAllMstFinYearList(SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                String aqlQuery = "SELECT * FROM dimFinYear ";

                cmdSelect = new SqlCommand(aqlQuery, conn, tran);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimFinYearTO> finYearTOList = new List<DimFinYearTO>();
                while (dateReader.Read())
                {
                    DimFinYearTO finYearTO = new DimFinYearTO();
                    if (dateReader["idFinYear"] != DBNull.Value)
                        finYearTO.IdFinYear = Convert.ToInt32(dateReader["idFinYear"].ToString());
                    if (dateReader["finYearDisplayName"] != DBNull.Value)
                        finYearTO.FinYearDisplayName = Convert.ToString(dateReader["finYearDisplayName"].ToString());
                    if (dateReader["finYearStartDate"] != DBNull.Value)
                        finYearTO.FinYearStartDate = Convert.ToDateTime(dateReader["finYearStartDate"].ToString());
                    if (dateReader["finYearEndDate"] != DBNull.Value)
                        finYearTO.FinYearEndDate = Convert.ToDateTime(dateReader["finYearEndDate"].ToString());

                    finYearTOList.Add(finYearTO);
                }

                return finYearTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                cmdSelect.Dispose();
            }

        }


        public List<DimFinYearTO> SelectAllMstFinYearList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimFinYear ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimFinYearTO> finYearTOList = new List<DimFinYearTO>();
                while (dateReader.Read())
                {
                    DimFinYearTO finYearTO = new DimFinYearTO();
                    if (dateReader["idFinYear"] != DBNull.Value)
                        finYearTO.IdFinYear = Convert.ToInt32(dateReader["idFinYear"].ToString());
                    if (dateReader["finYearDisplayName"] != DBNull.Value)
                        finYearTO.FinYearDisplayName = Convert.ToString(dateReader["finYearDisplayName"].ToString());
                    if (dateReader["finYearStartDate"] != DBNull.Value)
                        finYearTO.FinYearStartDate = Convert.ToDateTime(dateReader["finYearStartDate"].ToString());
                    if (dateReader["finYearEndDate"] != DBNull.Value)
                        finYearTO.FinYearEndDate = Convert.ToDateTime(dateReader["finYearEndDate"].ToString());

                    finYearTOList.Add(finYearTO);
                }

                return finYearTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        // Vaibhav [27-Sep-2017] added to select reporting type list
        public List<DropDownTO> SelectReportingType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM dimReportingType WHERE isActive= 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reportingTypeTO = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (reportingTypeTO.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (reportingTypeTO["idReportingType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(reportingTypeTO["idReportingType"].ToString());
                    if (reportingTypeTO["reportingTypeName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(reportingTypeTO["reportingTypeName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectReportingType");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Vaibhav [3-Oct-2017] added to select visit issue reason list
        public List<DimVisitIssueReasonsTO> SelectVisitIssueReasonsList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimVisitIssueReasons WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DimVisitIssueReasonsTO> visitIssueReasonTOList = new List<DimVisitIssueReasonsTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DimVisitIssueReasonsTO dimVisitIssueReasonsTONew = new DimVisitIssueReasonsTO();
                    if (visitIssueReasonTODT["idVisitIssueReasons"] != DBNull.Value)
                        dimVisitIssueReasonsTONew.IdVisitIssueReasons = Convert.ToInt32(visitIssueReasonTODT["idVisitIssueReasons"].ToString());
                    if (visitIssueReasonTODT["issueTypeId"] != DBNull.Value)
                        dimVisitIssueReasonsTONew.IssueTypeId = Convert.ToInt32(visitIssueReasonTODT["issueTypeId"].ToString());
                    if (visitIssueReasonTODT["visitIssueReasonName"] != DBNull.Value)
                        dimVisitIssueReasonsTONew.VisitIssueReasonName = Convert.ToString(visitIssueReasonTODT["visitIssueReasonName"].ToString());

                    visitIssueReasonTOList.Add(dimVisitIssueReasonsTONew);
                }
                return visitIssueReasonTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectReportingType");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectBrandList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimBrand WHERE isActive=1 ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idBrand"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idBrand"].ToString());
                    if (dateReader["brandName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["brandName"].ToString());
                    //[05-09-2018]Vijaymala added to get default brand data for other item
                    if (dateReader["isDefault"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["isDefault"].ToString());
                   
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimBrandTO> SelectBrandListV2()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimBrand WHERE isActive=1 ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimBrandTO> dropDownTOList = new List<Models.DimBrandTO>();
                while (dateReader.Read())
                {
                    DimBrandTO dropDownTONew = new DimBrandTO();
                    if (dateReader["idBrand"] != DBNull.Value)
                        dropDownTONew.IdBrand = Convert.ToInt32(dateReader["idBrand"].ToString());
                    if (dateReader["brandName"] != DBNull.Value)
                        dropDownTONew.BrandName = Convert.ToString(dateReader["brandName"].ToString());
                    //[05-09-2018]Vijaymala added to get default brand data for other item
                    if (dateReader["isDefault"] != DBNull.Value)
                        dropDownTONew.IsDefault = Convert.ToInt32(dateReader["isDefault"].ToString());
                    if (dateReader["isBothTaxType"] != DBNull.Value)
                        dropDownTONew.IsBothTaxType = Convert.ToInt32(dateReader["isBothTaxType"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        /// <summary>
        /// [2017-01-02]Vijaymala:Added to get loading layer list 
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectLoadingLayerList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimLoadingLayers WHERE isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idLoadingLayer"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idLoadingLayer"].ToString());
                    if (dateReader["layerDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["layerDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        
        public List<DropDownTO> GetBookingTaxCategoryList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimBookingTaxCategory WHERE isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idTaxCat"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idTaxCat"].ToString());
                    if (dateReader["TaxCateName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["TaxCateName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetBookingCommentCategoryList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimBookingCommentCategory WHERE isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCat"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCat"].ToString());
                    if (dateReader["CommentCategoryName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["CommentCategoryName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DimExportTypeTO> GetExportTypeList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimExportType WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimExportTypeTO> DimExportTypeTOList = new List<Models.DimExportTypeTO>();
                while (dateReader.Read())
                {
                    DimExportTypeTO DimExportTypeTO = new DimExportTypeTO();
                    if (dateReader["idExportType"] != DBNull.Value)
                        DimExportTypeTO.IdExportType = Convert.ToInt32(dateReader["idExportType"].ToString());
                    if (dateReader["exportTypeName"] != DBNull.Value)
                        DimExportTypeTO.ExportTypeName = Convert.ToString(dateReader["exportTypeName"].ToString());
                    if (dateReader["isActive"] != DBNull.Value)
                        DimExportTypeTO.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());

                    DimExportTypeTOList.Add(DimExportTypeTO);
                }
                if (dateReader != null)
                    dateReader.Dispose();

                return DimExportTypeTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DimIndustrySegmentTO> GetIndustryTypeList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimIndustrySegment WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimIndustrySegmentTO> DimIndustrySegmentTOList = new List<Models.DimIndustrySegmentTO>();
                while (dateReader.Read())
                {
                    DimIndustrySegmentTO DimIndustrySegmentTO = new DimIndustrySegmentTO();
                    if (dateReader["idIndustrySegment"] != DBNull.Value)
                        DimIndustrySegmentTO.IdIndustrySegment = Convert.ToInt32(dateReader["idIndustrySegment"].ToString());
                    if (dateReader["industrySegName"] != DBNull.Value)
                        DimIndustrySegmentTO.IndustrySegName = Convert.ToString(dateReader["industrySegName"].ToString());
                    if (dateReader["isActive"] != DBNull.Value)
                        DimIndustrySegmentTO.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());

                    DimIndustrySegmentTOList.Add(DimIndustrySegmentTO);
                }
                if (dateReader != null)
                    dateReader.Dispose();

                return DimIndustrySegmentTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }


        public List<DimIndustrySegmentTypeTO> GetIndustrySegmentTypeList(Int32 industrySegmentId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = "select * from dimIndustrySegmentType  WHERE  industrySegmentId = " + industrySegmentId;

                conn.Open();
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimIndustrySegmentTypeTO> DimIndustrySegmentTypeTOList = new List<Models.DimIndustrySegmentTypeTO>();
                while (dateReader.Read())
                {
                    DimIndustrySegmentTypeTO DimIndustrySegmentTypeTO = new DimIndustrySegmentTypeTO();
                    if (dateReader["idIndustrySegType"] != DBNull.Value)
                        DimIndustrySegmentTypeTO.IdIndustrySegType = Convert.ToInt32(dateReader["idIndustrySegType"].ToString());
                    if (dateReader["typeName"] != DBNull.Value)
                        DimIndustrySegmentTypeTO.TypeName = Convert.ToString(dateReader["typeName"].ToString());
                    if (dateReader["industrySegmentId"] != DBNull.Value)
                        DimIndustrySegmentTypeTO.IndustrySegmentId = Convert.ToInt32(dateReader["industrySegmentId"].ToString());
                    if (dateReader["isActive"] != DBNull.Value)
                        DimIndustrySegmentTypeTO.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());

                    DimIndustrySegmentTypeTOList.Add(DimIndustrySegmentTypeTO);
                }
                if (dateReader != null)
                    dateReader.Dispose();

                return DimIndustrySegmentTypeTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "industrySegmentTypeList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        // Vijaymala [09-11-2017] added to get state code
        public DropDownTO SelectStateCode(Int32 stateId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = "select idState,stateOrUTCode from dimState  WHERE  idState = " + stateId;

                conn.Open();
                SqlDataReader departmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO dropDownTO = new DropDownTO();
                if (departmentTODT != null)
                {
                    while (departmentTODT.Read())
                    {
                        if (departmentTODT["idState"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(departmentTODT["idState"].ToString());
                        if (departmentTODT["stateOrUTCode"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(departmentTODT["stateOrUTCode"].ToString());
                    }
                }
                return dropDownTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Sanjay[2018-02-19] To Get dropdown list of Item Product Categories in the system
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetItemProductCategoryListForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimItemProdCateg WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idItemProdCat"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idItemProdCat"].ToString());
                    if (visitIssueReasonTODT["itemProdCategory"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["itemProdCategory"].ToString());
                    if (visitIssueReasonTODT["itemProdCategoryDesc"] != DBNull.Value)
                        dropDownTO.Tag = Convert.ToString(visitIssueReasonTODT["itemProdCategoryDesc"].ToString());

                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetItemProductCategoryListForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Sudhir[22-01-2018] Added for getStatusofInvoice.
        public List<DropDownTO> GetInvoiceStatusDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceStatus";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvStatus"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvStatus"].ToString());
                    if (dateReader["statusName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["statusName"].ToString());
                    if (dateReader["statusDesc"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["statusDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        //Sudhir[07-MAR-2018] Added for All Firm Types List.
        public List<DropDownTO> SelectAllFirmTypesForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                conn.Open();
                sqlQuery = "SELECT * FROM dimFirmType WHERE isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idFirmType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idFirmType"].ToString());
                    if (dateReader["firmName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["firmName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //Sudhir[07-MAR-2018] Added for All Firm Types List.
        public List<DropDownTO> SelectAllInfluencerTypesForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM dimInfluencerType WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInfluencerType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInfluencerType"].ToString());
                    if (dateReader["influencerTypeName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["influencerTypeName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //Sudhir[15-MAR-2018] Added for Select All Enquiry Channels  
        public List<DropDownTO> SelectAllEnquiryChannels()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM dimEnqChannel WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idEnqChanel"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idEnqChanel"].ToString());
                    if (dateReader["enqChannelDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["enqChannelDesc"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Sudhir[15-MAR-2018] Added for Select All Industry Sector.
        public List<DropDownTO> SelectAllIndustrySector()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                conn.Open();
                sqlQuery = "SELECT * FROM dimIndustrySector WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idIndustrySector"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idIndustrySector"].ToString());
                    if (dateReader["industrySectorDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["industrySectorDesc"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DropDownTO> GetCallBySelfForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimCallBySelfTo WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idCallBySelf"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idCallBySelf"].ToString());
                    if (visitIssueReasonTODT["callBySelfDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["callBySelfDesc"].ToString());

                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetCallBySelfForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetArrangeForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimArrangeFor WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idArrangeFor"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idArrangeFor"].ToString());
                    if (visitIssueReasonTODT["arrangeForDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["arrangeForDesc"].ToString());

                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetArrangeForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetArrangeVisitToDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimArrangeVisitTo WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idArrangeVisitTo"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idArrangeVisitTo"].ToString());
                    if (visitIssueReasonTODT["arrangeVisitToDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["arrangeVisitToDesc"].ToString());
                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetArrangeVisitToDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectAllOrganizationType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = "SELECT * FROM dimOrgType WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idOrgType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idOrgType"].ToString());
                    if (dateReader["OrgType"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["OrgType"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DropDownTO> SelectAddressTypeListForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimAddressType  where isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idAddressType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idAddressType"].ToString());
                    if (dateReader["addressType"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["addressType"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

            //vijaymala added[21-06-2018]to get cd dropdown


        }
        //vijaymala added[21-06-2018]to get cd dropdown
        public DropDownTO SelectCDDropDown(Int32 cdStructureId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = "SELECT * FROM dimCdStructure WHERE isActive=1 and idCdStructure = " + cdStructureId;

                conn.Open();
                SqlDataReader cdTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO dropDownTO = new DropDownTO();
                if (cdTODT != null)
                {
                    while (cdTODT.Read())
                    {
                        if (cdTODT["idCdStructure"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(cdTODT["idCdStructure"].ToString());
                        if (cdTODT["isPercent"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(cdTODT["isPercent"].ToString());
                    }
                }
                return dropDownTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }



        }
        public List<DropDownTO> SelectAllVisitTypeListForDropDown()
        {
            {

                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                SqlCommand cmdSelect = null;
                SqlDataReader dateReader = null;
                try
                {
                    conn.Open();
                    String aqlQuery = "SELECT * FROM dimVisitType WHERE  isActive=1";

                    cmdSelect = new SqlCommand(aqlQuery, conn);
                    dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                    while (dateReader.Read())
                    {
                        DropDownTO dropDownTONew = new DropDownTO();
                        if (dateReader["idVisit"] != DBNull.Value)
                            dropDownTONew.Value = Convert.ToInt32(dateReader["idVisit"].ToString());
                        if (dateReader["visitType"] != DBNull.Value)
                            dropDownTONew.Text = Convert.ToString(dateReader["visitType"].ToString());

                        dropDownTOList.Add(dropDownTONew);
                    }

                    return dropDownTOList;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    dateReader.Dispose();
                    conn.Close();
                    cmdSelect.Dispose();
                }

            }

        }
        public List<DropDownTO> SelectDefaultRoleListForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT Role.* FROM tblRole Role INNER JOIN  dimOrgType OrgType  ON Role.idROle = OrgType.defaultRoleId AND Role.isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        /// <summary>
        /// Vijaymala[08-09-2018]added:to get state from booking
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectStatesForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = "select distinct addr.stateId as idState,stateD.stateName ,stateD.stateOrUTCode from tblAddress addr" +
                    "  left join dimState stateD on stateD.idState = addr.stateId " +
                    "  left join tblOrgAddress orgAddr on orgAddr.addressId = addr.idAddr " +
                    "  left join tblOrganization org on orgAddr.organizationId = org.idOrganization " +
                    "  left join tblBookings booking on booking.dealerOrgId = org.idOrganization ";


                String sqlAllQuery = String.Empty;
                if (countryId > 0)
                    sqlAllQuery = sqlQuery + "where CAST(booking.createdOn AS DATE) BETWEEN @fromDate AND @toDate"; //No where condition. As we dont have country column in states
                else
                    sqlAllQuery = sqlQuery + "where CAST(booking.createdOn AS DATE) BETWEEN @fromDate AND @toDate";



                cmdSelect = new SqlCommand(sqlAllQuery, conn);
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idState"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idState"].ToString());
                    if (dateReader["stateName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["stateName"].ToString());
                    if (dateReader["stateOrUTCode"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["stateOrUTCode"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        /// <summary>
        /// Vijaymala[08-09-2018]:added to get district from booking
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDistrictForDropDownAccToBooking(int stateId, DateTime fromDate, DateTime toDate)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "select distinct addr.districtId as idDistrict,dist.districtName,dist.stateId from tblAddress addr" +
                    "  left join dimDistrict dist on dist.idDistrict = addr.districtId " +
                    "  left join tblOrgAddress orgAddr on orgAddr.addressId = addr.idAddr " +
                    "  left join tblOrganization org on orgAddr.organizationId = org.idOrganization " +
                    "  left join tblBookings booking on booking.dealerOrgId = org.idOrganization ";


                String sqlAllQuery = string.Empty;
                if (stateId > 0)
                    sqlAllQuery = sqlQuery + "where CAST(booking.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND stateId=" + stateId;
                else
                    sqlAllQuery = sqlQuery + "where CAST(booking.createdOn AS DATE) BETWEEN @fromDate AND @toDate  ";



                cmdSelect = new SqlCommand(sqlAllQuery, conn);
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idDistrict"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idDistrict"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["districtName"].ToString());
                    if (dateReader["stateId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["stateId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> GetFixedDropDownList()
        {
            List<DropDownTO> dropDownToList = new List<DropDownTO>()
            {
                new DropDownTO {Text=" USER ",Value=1,Tag=1},
                new DropDownTO {Text=" STATE ",Value=2,Tag=0},
                new DropDownTO {Text=" DISTRICT",Value=3,Tag=0}
            };
            return dropDownToList;
        }

        public List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM  [dbo].[tblCRMSiteType] WHERE parentSiteTypeId="+ parentSiteTypeId;

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSiteType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSiteType"].ToString());
                    if (dateReader["siteTypeDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["siteTypeDisplayName"].ToString());
                    if (dateReader["parentSiteTypeId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["parentSiteTypeId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectAllInvoiceCopyList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM  [dbo].[dimInvoiceCopy] WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvoiceCopy"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvoiceCopy"].ToString());
                    if (dateReader["invoiceCopyName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["invoiceCopyName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        #region Insertion

        public int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                String sqlQuery = @" INSERT INTO [dimTaluka]( " +
                            "  [districtId]" +
                            " ,[talukaCode]" +
                            " ,[talukaName]" +
                            " )" +
                " VALUES (" +
                            "  @districtId " +
                            " ,@talukaCode " +
                            " ,@talukaName " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                String sqlSelectIdentityQry = "Select @@Identity";

                cmdInsert.Parameters.Add("@districtId", System.Data.SqlDbType.Int).Value = commonDimensionsTO.ParentId;
                cmdInsert.Parameters.Add("@talukaCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionCode);
                cmdInsert.Parameters.Add("@talukaName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionName);
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    cmdInsert.CommandText = sqlSelectIdentityQry;
                    commonDimensionsTO.IdDimension = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return 1;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                String sqlQuery = @" INSERT INTO [dimDistrict]( " +
                            "  [stateId]" +
                            " ,[districtCode]" +
                            " ,[districtName]" +
                            " )" +
                " VALUES (" +
                            "  @stateId " +
                            " ,@districtCode " +
                            " ,@districtName " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                String sqlSelectIdentityQry = "Select @@Identity";

                cmdInsert.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = commonDimensionsTO.ParentId;
                cmdInsert.Parameters.Add("@districtCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionCode);
                cmdInsert.Parameters.Add("@districtName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionName);
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    cmdInsert.CommandText = sqlSelectIdentityQry;
                    commonDimensionsTO.IdDimension = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return 1;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int InsertMstFinYear(DimFinYearTO newMstFinYearTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                String sqlQuery = @" INSERT INTO [dimFinYear]( " +
                            "  [idFinYear]" +
                            " ,[finYearDisplayName]" +
                            " ,[finYearStartDate]" +
                            " ,[finYearEndDate]" +
                            " )" +
                " VALUES (" +
                            "  @idFinYear " +
                            " ,@finYearDisplayName " +
                            " ,@finYearStartDate " +
                            " ,@finYearEndDate " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@idFinYear", System.Data.SqlDbType.Int).Value = newMstFinYearTO.IdFinYear;
                cmdInsert.Parameters.Add("@finYearDisplayName", System.Data.SqlDbType.NVarChar).Value = newMstFinYearTO.FinYearDisplayName;
                cmdInsert.Parameters.Add("@finYearStartDate", System.Data.SqlDbType.DateTime).Value = newMstFinYearTO.FinYearStartDate;
                cmdInsert.Parameters.Add("@finYearEndDate", System.Data.SqlDbType.DateTime).Value = newMstFinYearTO.FinYearEndDate;
                return cmdInsert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int InsertMstFinYear(DimFinYearTO newMstFinYearTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {

                conn.Open();
                String sqlQuery = @" INSERT INTO [dimFinYear]( " +
                            "  [idFinYear]" +
                            " ,[finYearDisplayName]" +
                            " ,[finYearStartDate]" +
                            " ,[finYearEndDate]" +
                            " )" +
                " VALUES (" +
                            "  @idFinYear " +
                            " ,@finYearDisplayName " +
                            " ,@finYearStartDate " +
                            " ,@finYearEndDate " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@idFinYear", System.Data.SqlDbType.Int).Value = newMstFinYearTO.IdFinYear;
                cmdInsert.Parameters.Add("@finYearDisplayName", System.Data.SqlDbType.NVarChar).Value = newMstFinYearTO.FinYearDisplayName;
                cmdInsert.Parameters.Add("@finYearStartDate", System.Data.SqlDbType.DateTime).Value = newMstFinYearTO.FinYearStartDate;
                cmdInsert.Parameters.Add("@finYearEndDate", System.Data.SqlDbType.DateTime).Value = newMstFinYearTO.FinYearEndDate;
                return cmdInsert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        #endregion

        #region Execute Command

        public int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandText = cmdStr;

                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -2;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        #endregion

    }
}
