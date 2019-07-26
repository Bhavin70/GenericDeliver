using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimReportTemplateDAO : IDimReportTemplateDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimReportTemplateDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimReportTemplate]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimReportTemplateTO> SelectAllDimReportTemplate()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DimReportTemplateTO SelectDimReportTemplate(Int32 idReport)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idReport = " + idReport +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                //String computerName = System.Windows.Forms.SystemInformation.ComputerName;
               // String userName = System.Windows.Forms.SystemInformation.UserName;
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimReportTemplateTO> ConvertDTToList(SqlDataReader dimReportTemplateTODT)
        {
            List<DimReportTemplateTO> dimReportTemplateTOList = new List<DimReportTemplateTO>();
            if (dimReportTemplateTODT != null)
            {
                while (dimReportTemplateTODT.Read())
                {
                    DimReportTemplateTO dimReportTemplateTONew = new DimReportTemplateTO();
                    if (dimReportTemplateTODT ["idReport"] != DBNull.Value)
                        dimReportTemplateTONew.IdReport = Convert.ToInt32(dimReportTemplateTODT ["idReport"].ToString());
                    if (dimReportTemplateTODT ["isDisplayMultisheetAllow"] != DBNull.Value)
                        dimReportTemplateTONew.IsDisplayMultisheetAllow = Convert.ToInt32(dimReportTemplateTODT ["isDisplayMultisheetAllow"].ToString());
                    if (dimReportTemplateTODT ["createdBy"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedBy = Convert.ToInt32(dimReportTemplateTODT ["createdBy"].ToString());
                    if (dimReportTemplateTODT ["createdOn"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedOn = Convert.ToDateTime(dimReportTemplateTODT ["createdOn"].ToString());
                    if (dimReportTemplateTODT ["reportName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportName = Convert.ToString(dimReportTemplateTODT ["reportName"].ToString());
                    if (dimReportTemplateTODT ["reportFileName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileName = Convert.ToString(dimReportTemplateTODT ["reportFileName"].ToString());
                    if (dimReportTemplateTODT ["reportFileExtension"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileExtension = Convert.ToString(dimReportTemplateTODT ["reportFileExtension"].ToString());
                    if (dimReportTemplateTODT ["reportPassword"] != DBNull.Value)
                        dimReportTemplateTONew.ReportPassword = Convert.ToString(dimReportTemplateTODT ["reportPassword"].ToString());
                    dimReportTemplateTOList.Add(dimReportTemplateTONew);
                }
            }
            return dimReportTemplateTOList;
        }


        public DimReportTemplateTO SelectDimReportTemplate(String reportName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate WHERE reportName = '" + reportName + "' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@report_name", System.Data.SqlDbType.NVarChar).Value = mstReportTemplateTO.ReportName;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                //String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                //String userName = System.Windows.Forms.SystemInformation.UserName;
                //MessageBox.Show("Computer Name:" + computerName + Environment.NewLine + "User Name:" + userName + Environment.NewLine + "Class Name: MstReportTemplateDAO" + Environment.NewLine + "Method Name:SelectMstReportTemplate(MstReportTemplateTO mstReportTemplateTO)" + Environment.NewLine + "Exception Message:" + ex.Message.ToString() + "");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// KISHOR [2014-11-28] Add with conn tran
        /// </summary>
        /// <param name="reportFileName"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<DimReportTemplateTO> isVisibleAllowMultisheetReportList(string reportFileName, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate " +
                                    " WHERE reportFileName = '" + reportFileName + "' AND isDisplayMultisheetAllow ='true'";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;

                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
            {
                //  VegaERPFrameWork.VErrorList.Add("Error in Class-MstReportTemplateDAO, Method-isVisibleAllowMultisheetReport(string reportFileName, SqlConnection conn, SqlTransaction tran)", VegaERPFrameWork.EMessageType.Error, ex, null);
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                if (rdr != null) rdr.Dispose();
            }
        }

        // <summary>
        /// isVisibleAllowMultisheetReport for display multisheet Report in PDF
        /// </summary>
        /// <param name="mstReportTemplateTO"></param>
        /// <returns></returns>

        public List<DimReportTemplateTO> isVisibleAllowMultisheetReportList(string reportFileName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate " +
                                    " WHERE reportFileName = '" + reportFileName + "' AND isDisplayMultisheetAllow =1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
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
                if (rdr != null) rdr.Dispose();
            }
        }


        #endregion

        #region Insertion
        public int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimReportTemplateTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimReportTemplateTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(DimReportTemplateTO dimReportTemplateTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimReportTemplate]( " + 
            "  [idReport]" +
            " ,[isDisplayMultisheetAllow]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[reportName]" +
            " ,[reportFileName]" +
            " ,[reportFileExtension]" +
            " ,[reportPassword]" +
            " )" +
" VALUES (" +
            "  @IdReport " +
            " ,@IsDisplayMultisheetAllow " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@ReportName " +
            " ,@ReportFileName " +
            " ,@ReportFileExtension " +
            " ,@ReportPassword " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
            cmdInsert.Parameters.Add("@IsDisplayMultisheetAllow", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IsDisplayMultisheetAllow;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimReportTemplateTO.CreatedOn;
            cmdInsert.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportName;
            cmdInsert.Parameters.Add("@ReportFileName", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportFileName;
            cmdInsert.Parameters.Add("@ReportFileExtension", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportFileExtension;
            cmdInsert.Parameters.Add("@ReportPassword", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportPassword;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimReportTemplateTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimReportTemplateTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(DimReportTemplateTO dimReportTemplateTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimReportTemplate] SET " + 
            "  [idReport] = @IdReport" +
            " ,[isDisplayMultisheetAllow]= @IsDisplayMultisheetAllow" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[reportName]= @ReportName" +
            " ,[reportFileName]= @ReportFileName" +
            " ,[reportFileExtension]= @ReportFileExtension" +
            " ,[reportPassword] = @ReportPassword" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
            cmdUpdate.Parameters.Add("@IsDisplayMultisheetAllow", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IsDisplayMultisheetAllow;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimReportTemplateTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportName;
            cmdUpdate.Parameters.Add("@ReportFileName", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportFileName;
            cmdUpdate.Parameters.Add("@ReportFileExtension", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportFileExtension;
            cmdUpdate.Parameters.Add("@ReportPassword", System.Data.SqlDbType.NVarChar).Value = dimReportTemplateTO.ReportPassword;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimReportTemplate(Int32 idReport)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idReport, cmdDelete);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteDimReportTemplate(Int32 idReport, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idReport, cmdDelete);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idReport, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimReportTemplate] " +
            " WHERE idReport = " + idReport +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idReport", System.Data.SqlDbType.Int).Value = dimReportTemplateTO.IdReport;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
