using System;
using System.Collections;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Data;
using System.Collections.Generic;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblContactUsDtlsDAO : ITblContactUsDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblContactUsDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            this.conn = new SqlConnection(sqlConnStr);
            this.cmdSelect = new SqlCommand();
            this.sqlReader = null;
            this.sqlSelectQry = " SELECT * FROM [tblContactUsDtls]";
        }
        SqlConnection conn;
        SqlCommand cmdSelect;
        SqlDataReader sqlReader;
        String sqlSelectQry;
        //public TblContactUsDtlsDAO()
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    this.conn = new SqlConnection(sqlConnStr);
        //    this.cmdSelect = new SqlCommand();
        //    this.sqlReader = null;
        //    this.sqlSelectQry = " SELECT * FROM [tblContactUsDtls]";
        //}
     
        #region Selection
        // Select contacts on condition - Tejaswini
        public List<TblContactUsDtls> SelectContactUsDtls(int IsActive)
        {
            try
            {
                this.conn.Open();
                this.cmdSelect.CommandText = this.sqlSelectQry + " WHERE isActive=" + IsActive;
                this.cmdSelect.Connection = this.conn;
                this.cmdSelect.CommandType = System.Data.CommandType.Text;

                this.sqlReader = this.cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblContactUsDtls> Contactlist = ConvertDTToList(this.sqlReader);
                return Contactlist;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                if (this.sqlReader != null)
                    this.sqlReader.Dispose();
                this.conn.Close();
                this.cmdSelect.Dispose();
            }
        }


        // Select all contacts  - Tejaswini
        public List<TblContactUsDtls> SelectAllContactUsDtls()
        {
            try
            {
                this.conn.Open();
                this.cmdSelect.CommandText = this.sqlSelectQry;
                this.cmdSelect.Connection = this.conn;
                this.cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = this.cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblContactUsDtls> Contactlist = ConvertDTToList(sqlReader);
                return Contactlist;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                if (this.sqlReader != null)
                    this.sqlReader.Dispose();
                this.conn.Close();
                this.cmdSelect.Dispose();
            }
        }


        public List<TblContactUsDtls> ConvertDTToList(SqlDataReader tblContactUsDtlsTODT)
        {
            List<TblContactUsDtls> tblContactUsDtlsTOList = new List<TblContactUsDtls>();
            if (tblContactUsDtlsTODT != null)
            {
                while (tblContactUsDtlsTODT.Read())
                {
                    TblContactUsDtls tblContactUsDtlsTONew = new TblContactUsDtls();
                    if (tblContactUsDtlsTODT["idContactUsDtls"] != DBNull.Value)
                        tblContactUsDtlsTONew.IdContactUsDtls = Convert.ToInt32(tblContactUsDtlsTODT["idContactUsDtls"].ToString());
                    if (tblContactUsDtlsTODT["departmentName"] != DBNull.Value)
                        tblContactUsDtlsTONew.DepartmentName = tblContactUsDtlsTODT["departmentName"].ToString();
                    if (tblContactUsDtlsTODT["designation"] != DBNull.Value)
                        tblContactUsDtlsTONew.Designation = tblContactUsDtlsTODT["designation"].ToString();
                    if (tblContactUsDtlsTODT["personName"] != DBNull.Value)
                        tblContactUsDtlsTONew.PersonName = tblContactUsDtlsTODT["personName"].ToString();
                    if (tblContactUsDtlsTODT["contactNo"] != DBNull.Value)
                        tblContactUsDtlsTONew.ContactNo = tblContactUsDtlsTODT["contactNo"].ToString();
                    if (tblContactUsDtlsTODT["emailId"] != DBNull.Value)
                        tblContactUsDtlsTONew.EmailId = tblContactUsDtlsTODT["emailId"].ToString();
                    if(tblContactUsDtlsTODT["supportTypeId"] != DBNull.Value)
                        tblContactUsDtlsTONew.SupportTypeId = Convert.ToInt32(tblContactUsDtlsTODT["supportTypeId"].ToString());
                    if (tblContactUsDtlsTODT["isActive"] != DBNull.Value)
                        tblContactUsDtlsTONew.IsActive = Convert.ToInt32(tblContactUsDtlsTODT["isActive"].ToString());

                    tblContactUsDtlsTOList.Add(tblContactUsDtlsTONew);
                }
            }
            return tblContactUsDtlsTOList;
        }
        #endregion
    }

}