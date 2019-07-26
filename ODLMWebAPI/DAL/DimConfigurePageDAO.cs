using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL
{
    public class DimConfigurePageDAO : IDimConfigurePageDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimConfigurePageDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimConfigurePage]";
            return sqlSelectQry;
        }
        public List<DimConfigurePageTO> GetConfigurationByPageId(int pageId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                sqlQuery = SqlSelectQuery() + " WHERE  pageId = @PageId AND isActive=1";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Parameters.AddWithValue("@PageId", DbType.Int32).Value = pageId;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dimConfigurePageTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimConfigurePageTO> dimConfigurePageTOList = ConvertDTToList(dimConfigurePageTODT);

                return dimConfigurePageTOList;
            }

            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimMstDept");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
           
        }

        public List<DimConfigurePageTO> ConvertDTToList(SqlDataReader dimConfigurePageTODT)
        {
            List<DimConfigurePageTO> dimConfigurePageTOList = new List<DimConfigurePageTO>();
            if (dimConfigurePageTODT!=null)
            {
                while(dimConfigurePageTODT.Read())
                {
                    DimConfigurePageTO dimConfigurePageTO = new DimConfigurePageTO();
                    if (dimConfigurePageTODT["idConfiguration"] != null)
                        dimConfigurePageTO.IdConfiguration = Convert.ToInt32(dimConfigurePageTODT["idConfiguration"]);
                    if (dimConfigurePageTODT["columnName"] != null)
                        dimConfigurePageTO.ColumnName = Convert.ToString(dimConfigurePageTODT["columnName"]);
                    if (dimConfigurePageTODT["isShow"] != null)
                        dimConfigurePageTO.IsShow = Convert.ToInt32(dimConfigurePageTODT["isShow"]);
                    if (dimConfigurePageTODT["isDisabled"] != null)
                        dimConfigurePageTO.IsDisabled = Convert.ToInt32(dimConfigurePageTODT["isDisabled"]);
                    if (dimConfigurePageTODT["isMandatory"] != null)
                        dimConfigurePageTO.IsMandatory = Convert.ToInt32(dimConfigurePageTODT["isMandatory"]);
                    if (dimConfigurePageTODT["pageId"] != null)
                        dimConfigurePageTO.PageId = Convert.ToInt32(dimConfigurePageTODT["pageId"]);
                    if (dimConfigurePageTODT["isActive"] != null)
                        dimConfigurePageTO.IsActive = Convert.ToInt32(dimConfigurePageTODT["isActive"]);
                    //if (dimConfigurePageTODT["createdOn"] != null)
                    //    dimConfigurePageTO.CreatedOn = Convert.ToDateTime(dimConfigurePageTODT["createdOn"]);
                    //if (dimConfigurePageTODT["createdBy"] != null)
                    //    dimConfigurePageTO.CreatedBy = Convert.ToInt32(dimConfigurePageTODT["createdBy"]);
                    //if (dimConfigurePageTODT["updatedOn"] != null)
                    //    dimConfigurePageTO.UpdatedOn = Convert.ToDateTime(dimConfigurePageTODT["updatedOn"]);
                    //if (dimConfigurePageTODT["updatedBy"] != null)
                    //    dimConfigurePageTO.UpdatedBy = Convert.ToInt32(dimConfigurePageTODT["updatedBy"]);

                    dimConfigurePageTOList.Add(dimConfigurePageTO);
                }
            }
            return dimConfigurePageTOList;
        }
    }
}
