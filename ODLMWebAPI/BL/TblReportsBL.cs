using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblReportsBL : ITblReportsBL
    {
        private readonly ITblReportsDAO _iTblReportsDAO;
        private readonly ITblFilterReportBL _iTblFilterReportBL;
        private readonly ITblOrgStructureBL _iTblOrgStructureBL;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblReportsBL(ICommon iCommon, IConnectionString iConnectionString, ITblReportsDAO iTblReportsDAO, ITblFilterReportBL iTblFilterReportBL, ITblOrgStructureBL iTblOrgStructureBL)
        {
            _iTblReportsDAO = iTblReportsDAO;
            _iTblFilterReportBL = iTblFilterReportBL;
            _iTblOrgStructureBL = iTblOrgStructureBL;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }

        #region Selection
        //public List<TblReportsTO> SelectAllTblReports()
        //{
        //    return _iTblReportsDAO.SelectAllTblReports();
        //}

        public List<TblReportsTO> SelectAllTblReportsList()
        {
            List<TblReportsTO> tblReportsTODT = _iTblReportsDAO.SelectAllTblReports();
            Parallel.ForEach(tblReportsTODT, element =>
            {
                element.SqlQuery = null;
            }
            );
            return tblReportsTODT;
        }

        public TblReportsTO SelectTblReportsTO(Int32 idReports)
        {
            TblReportsTO tblReportsTODT = _iTblReportsDAO.SelectTblReports(idReports);
            if (tblReportsTODT != null)
            {
                List<TblFilterReportTO> tblFilterReportList = _iTblFilterReportBL.SelectTblFilterReportList(idReports);
                if (tblFilterReportList != null && tblFilterReportList.Count > 0)
                {
                    tblReportsTODT.TblFilterReportTOList1 = tblFilterReportList;

                }
                return tblReportsTODT;
            }

            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblReports(TblReportsTO tblReportsTO)
        {
            return _iTblReportsDAO.InsertTblReports(tblReportsTO);
        }

        public int InsertTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReportsDAO.InsertTblReports(tblReportsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblReports(TblReportsTO tblReportsTO)
        {
            return _iTblReportsDAO.UpdateTblReports(tblReportsTO);
        }

        public int UpdateTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReportsDAO.UpdateTblReports(tblReportsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblReports(Int32 idReports)
        {
            return _iTblReportsDAO.DeleteTblReports(idReports);
        }

        public int DeleteTblReports(Int32 idReports, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReportsDAO.DeleteTblReports(idReports, conn, tran);
        }

        #endregion

        //public List<DynamicReportTO> GetDynamicData(string cmdText, params SqlParameter[] commandParameters)
        //{
        //    try
        //    {
        //        List<DynamicReportTO> data = _iTblReportsDAO.GetDynamicSqlData(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING), "SELECT * FROM dimOrgType");
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        public IEnumerable<dynamic> GetDynamicData(string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                IEnumerable<dynamic> dynamicList = _iCommon.GetDynamicSqlData(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING), cmdText, commandParameters);
                if (dynamicList != null)
                {
                    return dynamicList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public IEnumerable<dynamic> CreateDynamicQuery(TblReportsTO tblReportsTO)
        {
            try
            {
                if (tblReportsTO != null)
                {
                    TblReportsTO temptblReportsTO = SelectTblReportsTO(tblReportsTO.IdReports);
                    List<TblFilterReportTO> filterReportlist = new List<TblFilterReportTO>();
                    if (tblReportsTO.TblFilterReportTOList1 != null)
                    {
                        filterReportlist = tblReportsTO.TblFilterReportTOList1.OrderBy(element => element.OrderArguments).ToList();
                    }
                    String sqlQuery = temptblReportsTO.SqlQuery;
                    int count = filterReportlist.Count;
                    SqlParameter[] commandParameters = new SqlParameter[count];
                    for (int i = 0; i < filterReportlist.Count; i++)
                    {
                        TblFilterReportTO tblFilterReportTO = filterReportlist[i];
                        if (tblFilterReportTO.OutputValue != null && tblFilterReportTO.OutputValue != string.Empty && tblFilterReportTO.IsOptional == 1)
                        {
                            sqlQuery += tblFilterReportTO.WhereClause;
                        }
                        if (tblFilterReportTO.IsRequired == 0 && temptblReportsTO.WhereClause != String.Empty)
                        {
                            object listofUsers = _iTblOrgStructureBL.ChildUserListOnUserId(tblReportsTO.CreatedBy, 1,(int)Constants.ReportingTypeE.ADMINISTRATIVE);  //this method is call for get Child User Id's From Organzization Structure.
                            List<int> userIdList = new List<int>();
                            if (listofUsers != null)
                            {
                                userIdList = (List<int>)listofUsers;
                                userIdList.Add(tblReportsTO.CreatedBy);
                            }
                            else
                            {
                                userIdList.Add(tblReportsTO.CreatedBy);
                            }
                            string createdArr = string.Join<int>(",", userIdList);
                            temptblReportsTO.WhereClause = temptblReportsTO.WhereClause.Replace(tblFilterReportTO.SqlParameterName, createdArr);
                            sqlQuery += temptblReportsTO.WhereClause;
                        }
                        SqlDbType sqlDbType = (SqlDbType)tblFilterReportTO.SqlDbTypeValue;
                        commandParameters[i] = new SqlParameter("@" + tblFilterReportTO.SqlParameterName, sqlDbType);
                        commandParameters[i].Value = tblFilterReportTO.OutputValue;
                    }
                    IEnumerable<dynamic> dynamicList = GetDynamicData(sqlQuery, commandParameters);
                    return dynamicList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }
    }
}
