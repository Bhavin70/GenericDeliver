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
        private readonly ITblBookingsBL _iTblBookingsBL;
        private readonly ITblBookingQtyConsumptionBL _iTblBookingQtyConsumptionBL;
        private readonly ITblLoadingBL _iTblLoadingBL;
        private readonly ITblInvoiceBL _iTblInvoiceBL;
        private readonly IRunReport _iRunReport;
        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        private readonly ITblInvoiceItemDetailsBL _iTblInvoiceItemDetailsBL;
        public TblReportsBL(ICommon iCommon, IConnectionString iConnectionString, ITblReportsDAO iTblReportsDAO,
                            ITblFilterReportBL iTblFilterReportBL, ITblOrgStructureBL iTblOrgStructureBL, 
                            ITblBookingsBL iTblBookingsBL, ITblBookingQtyConsumptionBL iTblBookingQtyConsumptionBL,
                            ITblLoadingBL iTblLoadingBL, ITblInvoiceBL iTblInvoiceBL, IRunReport iRunReport,
                            IDimReportTemplateBL iDimReportTemplateBL, ITblInvoiceItemDetailsBL iTblInvoiceItemDetailsBL)
        {
            _iTblReportsDAO = iTblReportsDAO;
            _iTblFilterReportBL = iTblFilterReportBL;
            _iTblOrgStructureBL = iTblOrgStructureBL;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
            _iTblBookingsBL = iTblBookingsBL;
            _iTblBookingQtyConsumptionBL = iTblBookingQtyConsumptionBL;
            _iTblLoadingBL = iTblLoadingBL;
            _iTblInvoiceBL = iTblInvoiceBL;
            _iRunReport = iRunReport;
            _iDimReportTemplateBL = iDimReportTemplateBL;
            _iTblInvoiceItemDetailsBL = iTblInvoiceItemDetailsBL;
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

        public ResultMessage PrintLoadingTODetailsReport(int bookingId)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DataSet reportDS = new DataSet();
            try
            {
                TblBookingsTO tblBookingsTO = _iTblBookingsBL.SelectTblBookingsTO(bookingId);
                if (tblBookingsTO != null)
                {
                    List<TblBookingQtyConsumptionTO> tblBookingQtyConsumptionTOList = _iTblBookingQtyConsumptionBL.SelectTblBookingQtyConsumptionTOByBookingId(bookingId);
                    List<TblLoadingTO> tblLoadingTOList = _iTblLoadingBL.SelectAllTblLoadingByBookingId(bookingId);
                    DataTable tblBookingsDT = new DataTable();
                    tblBookingsDT.Columns.Add("dealerName", typeof(string));
                    tblBookingsDT.Columns.Add("bookingQty", typeof(double));
                    tblBookingsDT.Columns.Add("pendingQty", typeof(double));
                    tblBookingsDT.Columns.Add("bookingDisplayNo", typeof(string));
                    tblBookingsDT.Columns.Add("createdOn", typeof(DateTime));
                    tblBookingsDT.Columns.Add("bookingRate", typeof(double));
                    tblBookingsDT.Columns.Add("cdStructure", typeof(double));
                    tblBookingsDT.Columns.Add("orcAmt", typeof(string));
                    tblBookingsDT.Columns.Add("overDueAmt", typeof(double));

                    tblBookingsDT.Rows.Add();
                    tblBookingsDT.Rows[0]["dealerName"] = tblBookingsTO.DealerName;
                    tblBookingsDT.Rows[0]["bookingQty"] = tblBookingsTO.BookingQty;
                    tblBookingsDT.Rows[0]["pendingQty"] = tblBookingsTO.PendingQty;
                    tblBookingsDT.Rows[0]["bookingDisplayNo"] = "#" + tblBookingsTO.BookingDisplayNo;
                    tblBookingsDT.Rows[0]["createdOn"] = tblBookingsTO.CreatedOnStr;
                    tblBookingsDT.Rows[0]["bookingRate"] = tblBookingsTO.BookingRate;
                    tblBookingsDT.Rows[0]["cdStructure"] = tblBookingsTO.CdStructure;
                    tblBookingsDT.Rows[0]["orcAmt"] = tblBookingsTO.OrcAmt + "/" + tblBookingsTO.OrcMeasure;

                    DataTable loadingDT = new DataTable();
                    loadingDT.Columns.Add("vehicalNo", typeof(string));
                    loadingDT.Columns.Add("statusName", typeof(string));
                    loadingDT.Columns.Add("loadingDate", typeof(string));
                    loadingDT.Columns.Add("idLoading", typeof(Int32));

                    DataTable loadingSlipDT = new DataTable();
                    loadingSlipDT.Columns.Add("idLoading", typeof(Int32));
                    loadingSlipDT.Columns.Add("idLoadingSlip", typeof(Int32));
                    loadingSlipDT.Columns.Add("salesEngineer", typeof(string));
                    loadingSlipDT.Columns.Add("loadingSlipStatus", typeof(string));
                    loadingSlipDT.Columns.Add("JointDelivery", typeof(string));
                    loadingSlipDT.Columns.Add("noOfDeliveries", typeof(int));
                    loadingSlipDT.Columns.Add("loadingSlipNo", typeof(string));
                    loadingSlipDT.Columns.Add("dealer", typeof(string));
                    loadingSlipDT.Columns.Add("rate", typeof(double));
                    loadingSlipDT.Columns.Add("CD", typeof(double));
                    loadingSlipDT.Columns.Add("Confirm", typeof(string));
                    loadingSlipDT.Columns.Add("invoiceAmt", typeof(Double));

                    DataTable loadingSlipExtDT = new DataTable();
                    loadingSlipExtDT.Columns.Add("idLoadingSlip", typeof(Int32));
                    loadingSlipExtDT.Columns.Add("Size",typeof(string));
                    loadingSlipExtDT.Columns.Add("prodItem", typeof(string));
                    loadingSlipExtDT.Columns.Add("Qty", typeof(Double));
                    loadingSlipExtDT.Columns.Add("Bundles", typeof(Double));
                    loadingSlipExtDT.Columns.Add("tareWt", typeof(Double));
                    loadingSlipExtDT.Columns.Add("grossWt", typeof(Double));
                    loadingSlipExtDT.Columns.Add("loadedWt", typeof(Double));
                    loadingSlipExtDT.Columns.Add("loadedBundles", typeof(Double));
                    loadingSlipExtDT.Columns.Add("rate", typeof(Double));
                    loadingSlipExtDT.Columns.Add("totalAmt", typeof(double));

                    loadingSlipExtDT.Columns.Add("layer", typeof(string));
                    loadingSlipExtDT.Columns.Add("invoiceAmt", typeof(Double));
                    string MTStr = "(MT)";
                    if (tblLoadingTOList != null && tblLoadingTOList.Count > 0)
                    {
                        for (int i = 0; i < tblLoadingTOList.Count; i++)
                        {
                            TblLoadingTO tblLoadingTO = tblLoadingTOList[i];
                            loadingDT.Rows.Add();
                            int loadingDTRowNo = loadingDT.Rows.Count-1;
                            double loadingQty = 0;
                            
                            loadingDT.Rows[loadingDTRowNo]["statusName"] = tblLoadingTO.StatusDesc;
                            loadingDT.Rows[loadingDTRowNo]["loadingDate"] = tblLoadingTO.CreatedOnStr;
                            loadingDT.Rows[loadingDTRowNo]["idLoading"] = tblLoadingTO.IdLoading;

                            List<TblLoadingSlipTO> tblLoadingSlipTOist = tblLoadingTO.LoadingSlipList;
                            if(tblLoadingSlipTOist!=null && tblLoadingSlipTOist.Count>0)
                            {
                                for (int j = 0; j < tblLoadingSlipTOist.Count; j++)
                                {
                                    TblLoadingSlipTO TblLoadingSlipTO = tblLoadingSlipTOist[j];
                                    loadingSlipDT.Rows.Add();
                                    int loadingSlipDTrowNo = loadingSlipDT.Rows.Count - 1;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["idLoading"] = tblLoadingTO.IdLoading;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["idLoadingSlip"] = TblLoadingSlipTO.IdLoadingSlip;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["salesEngineer"] = TblLoadingSlipTO.CnfOrgName;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["loadingSlipStatus"] = TblLoadingSlipTO.StatusName;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["JointDelivery"] = TblLoadingSlipTO.IsJointDelivery==1?"Yes":"No";
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["noOfDeliveries"] = TblLoadingSlipTO.NoOfDeliveries;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["loadingSlipNo"] = TblLoadingSlipTO.LoadingSlipNo;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["dealer"] = TblLoadingSlipTO.DealerOrgName;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["rate"] = TblLoadingSlipTO.TblLoadingSlipDtlTO.BookingRate;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["CD"] = TblLoadingSlipTO.CdStructure;
                                    loadingSlipDT.Rows[loadingSlipDTrowNo]["Confirm"] = TblLoadingSlipTO.IsConfirmed == 1 ? "Confirm" :"-";
                                    List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = TblLoadingSlipTO.LoadingSlipExtTOList;
                                    List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();
                                    double loadingSlipInvAmt = 0;
                                    List<TblInvoiceTO> tblInvoiceTOList = _iTblInvoiceBL.SelectInvoiceTOListFromLoadingSlipId(TblLoadingSlipTO.IdLoadingSlip);
                                    if(tblInvoiceTOList!=null && tblInvoiceTOList.Count>0)
                                    {
                                        for (int c = 0; c < tblInvoiceTOList.Count; c++)
                                        {
                                            loadingSlipInvAmt += tblInvoiceTOList[c].GrandTotal;
                                            tblInvoiceItemDetailsTOList.AddRange(_iTblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(tblInvoiceTOList[c].IdInvoice));
                                        }
                                    }
                                   
                                    for (int k = 0; k < tblLoadingSlipExtTOList.Count; k++)
                                    {
                                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipExtTOList[k];
                                        loadingSlipExtDT.Rows.Add();
                                        int loadingSlipDTrowNorowNo = loadingSlipExtDT.Rows.Count - 1;
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["idLoadingSlip"] = TblLoadingSlipTO.IdLoadingSlip;
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["Size"] = tblLoadingSlipExtTO.DisplayName;
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["prodItem"] = tblLoadingSlipExtTO.ProdItemDesc;

                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["Qty"] = tblLoadingSlipExtTO.LoadingQty;
                                        loadingQty += tblLoadingSlipExtTO.LoadingQty;
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["Bundles"] = tblLoadingSlipExtTO.Bundles;
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["tareWt"] = tblLoadingSlipExtTO.CalcTareWeight;
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["grossWt"] = tblLoadingSlipExtTO.LoadedWeight + tblLoadingSlipExtTO.CalcTareWeight;
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["loadedWt"] = tblLoadingSlipExtTO.LoadedWeight;
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["loadedBundles"] = tblLoadingSlipExtTO.LoadedBundles;
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["rate"] = Math.Round(tblLoadingSlipExtTO.RatePerMT);
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["layer"] = tblLoadingSlipExtTO.LoadingLayerDesc
                                                                                                == "Middle 1" ? TblLoadingSlipTO.NoOfDeliveries
                                                                                                > 3 ? tblLoadingSlipExtTO.LoadingLayerDesc
                                                                                                : "Middle" : tblLoadingSlipExtTO.LoadingLayerDesc;

                                        
                                        loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["totalAmt"] = Math.Round(tblLoadingSlipExtTO.LoadingQty * tblLoadingSlipExtTO.RatePerMT);

                                        if (tblInvoiceItemDetailsTOList!=null && tblInvoiceItemDetailsTOList.Count>0)
                                        {
                                            loadingSlipExtDT.Rows[loadingSlipDTrowNorowNo]["invoiceAmt"] = Math.Round((tblInvoiceItemDetailsTOList.Where(w => w.LoadingSlipExtId == tblLoadingSlipExtTO.IdLoadingSlipExt).Sum(s => s.GrandTotal)));
                                        }
                                    }
                                    //if (tblInvoiceItemDetailsTOList != null && tblInvoiceItemDetailsTOList.Count > 0)
                                    //{
                                    //    loadingSlipDT.Rows[loadingSlipDTrowNo]["invoiceAmt"] = Math.Round(tblInvoiceItemDetailsTOList.Sum(s=>s.GrandTotal));
                                    //}
                                }
                            }
                            loadingDT.Rows[loadingDTRowNo]["vehicalNo"] = tblLoadingTO.VehicleNo+"("+ loadingQty+ MTStr+")";



                        }
                    }
                    tblBookingsDT.TableName = "tblBookingsDT";
                    loadingDT.TableName = "loadingDT";
                    loadingSlipDT.TableName = "loadingSlipDT";
                    loadingSlipExtDT.TableName = "loadingSlipExtDT";
                    reportDS.Tables.Add(tblBookingsDT);
                    reportDS.Tables.Add(loadingDT);
                    reportDS.Tables.Add(loadingSlipDT);
                    reportDS.Tables.Add(loadingSlipExtDT);
                }


                string templateName = "LoadingAgainstBookingReport";
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                String fileName = "LoadingAgainstBookingReport-" + DateTime.Now.Ticks;

                //download location for rewrite  template file
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                // RunReport runReport = new RunReport();
                Boolean IsProduction = true;
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(reportDS, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                String filePath = String.Empty;

                if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                {
                    filePath = resultMessage.Tag.ToString();
                }

                //driveName + path;
                Byte[] bytes = _iTblInvoiceBL.DeleteFile(saveLocation, filePath);
                if (bytes != null && bytes.Length > 0)
                {
                    resultMessage.Tag = Convert.ToBase64String(bytes);
                }
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    resultMessage.DefaultSuccessBehaviour();
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DisplayMessage = ex.ToString();

            }
            return resultMessage;
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
