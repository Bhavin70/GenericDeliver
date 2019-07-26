using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class DimReportTemplateBL : IDimReportTemplateBL
    {
        #region Selection
        public List<DimReportTemplateTO> SelectAllDimReportTemplate()
        {
            return DimReportTemplateDAO.SelectAllDimReportTemplate();
        }


        public DimReportTemplateTO SelectDimReportTemplateTO(Int32 idReport)
        {
            return DimReportTemplateDAO.SelectDimReportTemplate(idReport);
        }

       
        public String SelectReportFullName(String reportName)
        {
            String reportFullName = null;

            //MstReportTemplateTO mstReportTemplateTO = MstReportTemplateDAO.SelectMstReportTemplateTO(reportName);
            DimReportTemplateTO dimReportTemplateTO = SelectDimReportTemplateTO(reportName);
            if (dimReportTemplateTO != null)
            {

                TblConfigParamsTO templatePath = BL.TblConfigParamsBL.SelectTblConfigParamsValByName("REPORT_TEMPLATE_FOLDER_PATH");
                //object templatePath = BL.MstCsParamBL.GetValue("TEMP_REPORT_PATH");//For Testing Pramod InputRemovalExciseReport

                if (templatePath != null)
                    return templatePath.ConfigParamVal+ dimReportTemplateTO.ReportFileName + "." + dimReportTemplateTO.ReportFileExtension;
            }
            return reportFullName;
        }

        public DimReportTemplateTO SelectDimReportTemplateTO(String reportName)
        {
            return DimReportTemplateDAO.SelectDimReportTemplate(reportName);


        } 

        /// <summary>
        /// KISHOR [2014-11-28] Add - with conn tran
        /// </summary>
        /// <param name="reportFileName"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public Boolean isVisibleAllowMultisheetReport(String reportFileName, SqlConnection conn, SqlTransaction tran)
        {
            
            List<DimReportTemplateTO> dimReportTemplateTOList = DimReportTemplateDAO.isVisibleAllowMultisheetReportList(reportFileName, conn, tran);
            if (dimReportTemplateTOList != null && dimReportTemplateTOList.Count == 1)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Amol[2011-09-16] Check is allow multisheet report for PDF file
        /// </summary>
        /// <param name="mstReportTemplateTO"></param>
        /// <returns></returns>
        /// 
        public Boolean isVisibleAllowMultisheetReport(String reportFileName)
        {
           
            List<DimReportTemplateTO>dimReportTemplateTOList = DimReportTemplateDAO.isVisibleAllowMultisheetReportList(reportFileName);
            if (dimReportTemplateTOList != null && dimReportTemplateTOList.Count == 1)
            {
                return true;
            }
            else
                return false;
        }
    
        #endregion

        #region Insertion
        public int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO)
        {
            return DimReportTemplateDAO.InsertDimReportTemplate(dimReportTemplateTO);
        }

        public int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimReportTemplateDAO.InsertDimReportTemplate(dimReportTemplateTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO)
        {
            return DimReportTemplateDAO.UpdateDimReportTemplate(dimReportTemplateTO);
        }

        public int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimReportTemplateDAO.UpdateDimReportTemplate(dimReportTemplateTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimReportTemplate(Int32 idReport)
        {
            return DimReportTemplateDAO.DeleteDimReportTemplate(idReport);
        }

        public int DeleteDimReportTemplate(Int32 idReport, SqlConnection conn, SqlTransaction tran)
        {
            return DimReportTemplateDAO.DeleteDimReportTemplate(idReport, conn, tran);
        }

        #endregion
        
    }
}
