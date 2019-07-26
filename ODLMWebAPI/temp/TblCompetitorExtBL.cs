using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblCompetitorExtBL : ITblCompetitorExtBL
    {
        #region Selection

        public List<TblCompetitorExtTO> SelectAllTblCompetitorExtList()
        {
            return TblCompetitorExtDAO.SelectAllTblCompetitorExt();

        }

        public TblCompetitorExtTO SelectTblCompetitorExtTO(Int32 idCompetitorExt)
        {
            return TblCompetitorExtDAO.SelectTblCompetitorExt(idCompetitorExt);
        }

        public List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId)
        {
            return TblCompetitorExtDAO.SelectCompetitorBrandNamesDropDownList(competitorOrgId);
        }

        public List<TblCompetitorExtTO> SelectAllTblCompetitorExtList(Int32 orgId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblCompetitorExtList(orgId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblCompetitorExtTO> SelectAllTblCompetitorExtList(Int32 orgId, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorExtDAO.SelectAllTblCompetitorExt(orgId, conn, tran);

        }


        //Sudhir[09-APR-2018] Added fot GetAllCompetitorList
        public List<DropDownTO> SelectAllCompetitorDropDownList()
        {
            try
            {

                List<TblOrganizationTO> competitorOrganizationList = BL.TblOrganizationBL.SelectAllTblOrganizationList(Constants.OrgTypeE.COMPETITOR);
                List<DropDownTO> emptyCompetitorList = new List<DropDownTO>();
                if (competitorOrganizationList != null && competitorOrganizationList.Count > 0)
                {
                    for (int i = 0; i < competitorOrganizationList.Count; i++)
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        dropDownTO.Text = competitorOrganizationList[i].FirmName;
                        dropDownTO.Value = competitorOrganizationList[i].IdOrganization;
                        List<TblCompetitorExtTO> competitorList = SelectAllTblCompetitorExtList(competitorOrganizationList[i].IdOrganization);
                        List<DropDownTO> emptyBrandList = new List<DropDownTO>();
                        foreach (TblCompetitorExtTO item in competitorList)
                        {
                            DropDownTO brandTo = new DropDownTO();
                            brandTo.Text = item.BrandName;
                            brandTo.Value = item.IdCompetitorExt;
                            brandTo.Tag = competitorOrganizationList[i].IdOrganization;
                            emptyBrandList.Add(brandTo);
                        }
                        dropDownTO.Tag = emptyBrandList;
                        emptyCompetitorList.Add(dropDownTO);
                    }
                    return emptyCompetitorList.OrderBy(x => x.Value).ToList(); ;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        #endregion

        #region Insertion
        public int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO)
        {
            return TblCompetitorExtDAO.InsertTblCompetitorExt(tblCompetitorExtTO);
        }

        public int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorExtDAO.InsertTblCompetitorExt(tblCompetitorExtTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO)
        {
            return TblCompetitorExtDAO.UpdateTblCompetitorExt(tblCompetitorExtTO);
        }

        public int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorExtDAO.UpdateTblCompetitorExt(tblCompetitorExtTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblCompetitorExt(Int32 idCompetitorExt)
        {
            return TblCompetitorExtDAO.DeleteTblCompetitorExt(idCompetitorExt);
        }

        public int DeleteTblCompetitorExt(Int32 idCompetitorExt, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorExtDAO.DeleteTblCompetitorExt(idCompetitorExt, conn, tran);
        }

        #endregion

    }
}
