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
using ODLMWebAPI.BL;
using SkiaSharp;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using StackExchange.Redis;

namespace ODLMWebAPI.DAL
{
    public class DimProdSpecDescDAO : IDimProdSpecDescDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimProdSpecDescDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimProdSpec] where isActive=1";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimProdSpecDescTO> SelectAllDimProdSpecDesc()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " ORDER BY displaySequence ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimProdSpecDescTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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

        public DimProdSpecDescTO SelectDimProdSpecDesc(Int32 idProdSpec)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idProdSpec = " + idProdSpec + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimProdSpecDescTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
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

        public List<DimProdSpecDescTO> ConvertDTToList(SqlDataReader dimDimProdSpecTODT)
        {
            List<DimProdSpecDescTO> dimProdSpecTOList = new List<DimProdSpecDescTO>();
            if (dimDimProdSpecTODT != null)
            {
                while (dimDimProdSpecTODT.Read())
                {
                    DimProdSpecDescTO DimProdSpecTONew = new DimProdSpecDescTO();
                    if (dimDimProdSpecTODT["idProdSpec"] != DBNull.Value)
                        DimProdSpecTONew.IdProductSpecDesc = Convert.ToInt32(dimDimProdSpecTODT["idProdSpec"].ToString());
                    if (dimDimProdSpecTODT["isActive"] != DBNull.Value)
                        DimProdSpecTONew.IsActive = Convert.ToInt32(dimDimProdSpecTODT["isActive"].ToString());
                    if (dimDimProdSpecTODT["prodSpecDesc"] != DBNull.Value)
                        DimProdSpecTONew.ProdSpecDesc = Convert.ToString(dimDimProdSpecTODT["prodSpecDesc"].ToString());
                    if (dimDimProdSpecTODT["displaySequence"] != DBNull.Value)
                        DimProdSpecTONew.DisplaySequence = Convert.ToInt32(dimDimProdSpecTODT["displaySequence"].ToString());
                    dimProdSpecTOList.Add(DimProdSpecTONew);
                }
            }
            return dimProdSpecTOList;
        }

        public int SelectDimProdSpecDescription()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteSelectCommand(cmdInsert);
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

        public List<TblStripsTO> SelectAllTblStrips()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT ROW_NUMBER() OVER (ORDER BY tblStrips.idStrip ASC) AS RowNumber,tblStrips.*,PSC.size,PSC.thickness FROM tblStrips " +
                                        " LEFT JOIN tblPipesStripCommon AS PSC ON PSC.idPipesStripCommon = tblStrips.idPipesStripCommon";             
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStripsTO> list = ConvertDTToListStrips(rdr);
                rdr.Dispose();
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

        public List<TblStripsTO> ConvertDTToListStrips(SqlDataReader dimDimProdSpecTODT)
        {
            List<TblStripsTO> dimProdSpecTOList = new List<TblStripsTO>();
            if (dimDimProdSpecTODT != null)
            {
                while (dimDimProdSpecTODT.Read())
                {
                    TblStripsTO DimProdSpecTONew = new TblStripsTO();
                    if (dimDimProdSpecTODT["idStrips"] != DBNull.Value)
                        DimProdSpecTONew.IdStrips = Convert.ToInt32(dimDimProdSpecTODT["idStrips"].ToString());
                    if (dimDimProdSpecTODT["grade"] != DBNull.Value)
                        DimProdSpecTONew.Grade = Convert.ToInt32(dimDimProdSpecTODT["grade"].ToString());
                    if (dimDimProdSpecTODT["idSize"] != DBNull.Value)
                        DimProdSpecTONew.IdSize = Convert.ToInt32(dimDimProdSpecTODT["idSize"].ToString());
                    if (dimDimProdSpecTODT["size"] != DBNull.Value)
                        DimProdSpecTONew.Size = Convert.ToString(dimDimProdSpecTODT["size"].ToString());
                    if (dimDimProdSpecTODT["idThickness"] != DBNull.Value)
                        DimProdSpecTONew.IdThickness = Convert.ToInt32(dimDimProdSpecTODT["idThickness"].ToString());
                    if (dimDimProdSpecTODT["thickness"] != DBNull.Value)
                        DimProdSpecTONew.Thickness = Convert.ToDecimal(dimDimProdSpecTODT["thickness"].ToString());
                    if (dimDimProdSpecTODT["isActive"] != DBNull.Value)
                        DimProdSpecTONew.IsActive = Convert.ToInt32(dimDimProdSpecTODT["isActive"].ToString());
                    if (dimDimProdSpecTODT["createdOn"] != DBNull.Value)
                        DimProdSpecTONew.CreatedOn = Convert.ToDateTime(dimDimProdSpecTODT["createdOn"].ToString());
                    dimProdSpecTOList.Add(DimProdSpecTONew);
                }
            }
            return dimProdSpecTOList;
        }

        public List<TblPipesTO> SelectAllTblPipes()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT ROW_NUMBER() OVER (ORDER BY tblPipes.idPipes ASC) AS RowNumber,tblPipes.*,PSC.size,PSC.thickness FROM tblPipes " +
                                        " LEFT JOIN tblPipesStripCommon AS PSC ON tblPipes.idPipesStripCommon = PSC.idPipesStripCommon";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPipesTO> list = ConvertDTToListPipes(rdr);
                rdr.Dispose();
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

        public List<TblPipesTO> ConvertDTToListPipes(SqlDataReader dimDimProdSpecTODT)
        {
            List<TblPipesTO> dimProdSpecTOList = new List<TblPipesTO>();
            if (dimDimProdSpecTODT != null)
            {
                while (dimDimProdSpecTODT.Read())
                {
                    TblPipesTO DimProdSpecTONew = new TblPipesTO();
                    if (dimDimProdSpecTODT["idInch"] != DBNull.Value)
                        DimProdSpecTONew.IdInch = Convert.ToInt32(dimDimProdSpecTODT["idInch"].ToString());
                    if (dimDimProdSpecTODT["inch"] != DBNull.Value)
                        DimProdSpecTONew.Inch = Convert.ToDecimal(dimDimProdSpecTODT["inch"].ToString());
                    if (dimDimProdSpecTODT["idSize"] != DBNull.Value)
                        DimProdSpecTONew.IdSize = Convert.ToInt32(dimDimProdSpecTODT["idSize"].ToString());
                    if (dimDimProdSpecTODT["size"] != DBNull.Value)
                        DimProdSpecTONew.Size = Convert.ToString(dimDimProdSpecTODT["size"].ToString());
                    if (dimDimProdSpecTODT["idThickness"] != DBNull.Value)
                        DimProdSpecTONew.IdThickness = Convert.ToInt32(dimDimProdSpecTODT["idThickness"].ToString());
                    if (dimDimProdSpecTODT["thickness"] != DBNull.Value)
                        DimProdSpecTONew.Thickness = Convert.ToDecimal(dimDimProdSpecTODT["thickness"].ToString());
                    if (dimDimProdSpecTODT["isActive"] != DBNull.Value)
                        DimProdSpecTONew.IsActive = Convert.ToInt32(dimDimProdSpecTODT["isActive"].ToString());
                    if (dimDimProdSpecTODT["createdOn"] != DBNull.Value)
                        DimProdSpecTONew.CreatedOn = Convert.ToDateTime(dimDimProdSpecTODT["createdOn"].ToString());

                    dimProdSpecTOList.Add(DimProdSpecTONew);
                }
            }
            return dimProdSpecTOList;
        }

        //public List<TblPipesDropDownTo> SelectAllPipesInchForDropDown()
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = null;
        //    String sqlQuery = string.Empty;
        //    try
        //    {
        //        //DataTableExample();

        //        conn.Open();
        //        sqlQuery = "SELECT * FROM tblPipes WHERE isActive=1";

        //        cmdSelect = new SqlCommand(sqlQuery, conn);
        //        SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblPipesDropDownTo> dropDownTOList = new List<Models.TblPipesDropDownTo>();
        //        while (dateReader.Read())
        //        {
        //            TblPipesDropDownTo dropDownTONew = new TblPipesDropDownTo();
        //            if (dateReader["idPipes"] != DBNull.Value)
        //                dropDownTONew.IdPipes = Convert.ToInt32(dateReader["idPipes"].ToString());
        //            if (dateReader["inch"] != DBNull.Value)
        //                dropDownTONew.Inch = Convert.ToDecimal(dateReader["inch"].ToString());    
        //            dropDownTOList.Add(dropDownTONew);
        //        }

        //        if (dateReader != null)
        //            dateReader.Dispose();

        //        return dropDownTOList;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}
        public List<TblPipesStripCommonSizeTO> SelectAlltblPipesStripCommonSizeForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM tblPipesStripCommon WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPipesStripCommonSizeTO> dropDownTOList = new List<Models.TblPipesStripCommonSizeTO>();
                while (dateReader.Read())
                {
                    TblPipesStripCommonSizeTO dropDownTONew = new TblPipesStripCommonSizeTO();
                    if (dateReader["idPipesStripCommon"] != DBNull.Value)
                        dropDownTONew.IdPipesStripCommon = Convert.ToInt32(dateReader["idPipesStripCommon"].ToString());
                    if (dateReader["size"] != DBNull.Value)
                        dropDownTONew.Size = Convert.ToDecimal(dateReader["size"].ToString());
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
        public List<TblPipesStripCommonThicknessTO> SelectAlltblPipesStripCommonThicknessForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM tblPipesStripCommon WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPipesStripCommonThicknessTO> dropDownTOList = new List<Models.TblPipesStripCommonThicknessTO>();
                while (dateReader.Read())
                {
                    TblPipesStripCommonThicknessTO dropDownTONew = new TblPipesStripCommonThicknessTO();
                    if (dateReader["idPipesStripCommon"] != DBNull.Value)
                        dropDownTONew.IdPipesStripCommon = Convert.ToInt32(dateReader["idPipesStripCommon"].ToString());
                    if (dateReader["thickness"] != DBNull.Value)
                        dropDownTONew.Thickness = Convert.ToDecimal(dateReader["thickness"].ToString());
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
        public List<TblStripsGradeDropDownTo> SelectAlltblStripsGradeForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM tblStrips WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStripsGradeDropDownTo> dropDownTOList = new List<Models.TblStripsGradeDropDownTo>();
                while (dateReader.Read())
                {
                    TblStripsGradeDropDownTo dropDownTONew = new TblStripsGradeDropDownTo();
                    if (dateReader["idStrip"] != DBNull.Value)
                        dropDownTONew.IdStrip = Convert.ToInt32(dateReader["idStrip"].ToString());
                    if (dateReader["grade"] != DBNull.Value)
                        dropDownTONew.Grade = Convert.ToInt32(dateReader["grade"].ToString());
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
        public List<TblPipesStripCommonQuantityTO> SelectAlltblPipesStripCommonQuantityForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM tblPipesStripCommon WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPipesStripCommonQuantityTO> dropDownTOList = new List<Models.TblPipesStripCommonQuantityTO>();
                while (dateReader.Read())
                {
                    TblPipesStripCommonQuantityTO dropDownTONew = new TblPipesStripCommonQuantityTO();
                    if (dateReader["idPipesStripCommon"] != DBNull.Value)
                        dropDownTONew.IdPipesStripCommon = Convert.ToInt32(dateReader["idPipesStripCommon"].ToString());
                    if (dateReader["quantity"] != DBNull.Value)
                        dropDownTONew.Quantity = Convert.ToInt32(dateReader["quantity"].ToString());
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

        public List<TblInchDropDownTO> SelectAllTblInchForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM tblInch WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInchDropDownTO> dropDownTOList = new List<Models.TblInchDropDownTO>();
                while (dateReader.Read())
                {
                    TblInchDropDownTO dropDownTONew = new TblInchDropDownTO();
                    if (dateReader["idInch"] != DBNull.Value)
                        dropDownTONew.IdInch = Convert.ToInt32(dateReader["idInch"].ToString());
                    if (dateReader["inch"] != DBNull.Value)
                        dropDownTONew.Inch = Convert.ToDecimal(dateReader["inch"].ToString());
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
        public List<TblSizeTO> SelectAllTblSizeForDropDown(Int32 idInch = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT ROW_NUMBER() OVER (ORDER BY tblSize.idSize ASC) AS RowNumber, tblSize.*, inch.idInch FROM tblSize " +
                        "LEFT JOIN tblInch AS inch ON tblSize.idSize = inch.idInch";

                if (idInch > 0)
                {
                    cmdSelect.CommandText += " WHERE tblSize.idInch = @idInch";
                    cmdSelect.Parameters.AddWithValue("@idInch", idInch);
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSizeTO> list = ConvertDTToListSize(rdr);
                rdr.Dispose();
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

        public List<TblSizeTO> ConvertDTToListSize(SqlDataReader dimDimProdSpecTODT)
        {
            List<TblSizeTO> dimProdSpecTOList = new List<TblSizeTO>();
            if (dimDimProdSpecTODT != null)
            {
                while (dimDimProdSpecTODT.Read())
                {
                    TblSizeTO DimProdSpecTONew = new TblSizeTO();
                    if (dimDimProdSpecTODT["idSize"] != DBNull.Value)
                        DimProdSpecTONew.IdSize = Convert.ToInt32(dimDimProdSpecTODT["idSize"].ToString());
                    if (dimDimProdSpecTODT["size"] != DBNull.Value)
                        DimProdSpecTONew.Size = Convert.ToString(dimDimProdSpecTODT["size"].ToString());
                    if (dimDimProdSpecTODT["isActive"] != DBNull.Value)
                        DimProdSpecTONew.IsActive = Convert.ToInt32(dimDimProdSpecTODT["isActive"].ToString());
                    if (dimDimProdSpecTODT["createdOn"] != DBNull.Value)
                        DimProdSpecTONew.CreatedOn = Convert.ToDateTime(dimDimProdSpecTODT["createdOn"].ToString());
                    if (dimDimProdSpecTODT["idInch"] != DBNull.Value)
                        DimProdSpecTONew.IdInch = Convert.ToInt32(dimDimProdSpecTODT["idInch"].ToString());         
                    dimProdSpecTOList.Add(DimProdSpecTONew);
                }
            }
            return dimProdSpecTOList;
        }
        public List<TblThicknessDropDownTO> SelectAllTblThicknessForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM tblThickness WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblThicknessDropDownTO> dropDownTOList = new List<Models.TblThicknessDropDownTO>();
                while (dateReader.Read())
                {
                    TblThicknessDropDownTO dropDownTONew = new TblThicknessDropDownTO();
                    if (dateReader["idThickness"] != DBNull.Value)
                        dropDownTONew.IdThickness = Convert.ToInt32(dateReader["idThickness"].ToString());
                    if (dateReader["thickness"] != DBNull.Value)
                        dropDownTONew.Thickness = Convert.ToDecimal(dateReader["thickness"].ToString());
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

        public List<TblWidthDropDownTO> SelectAllTblWidthForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM tblWidth WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWidthDropDownTO> dropDownTOList = new List<Models.TblWidthDropDownTO>();
                while (dateReader.Read())
                {
                    TblWidthDropDownTO dropDownTONew = new TblWidthDropDownTO();
                    if (dateReader["idWidth"] != DBNull.Value)
                        dropDownTONew.IdWidth = Convert.ToInt32(dateReader["idWidth"].ToString());
                    if (dateReader["width"] != DBNull.Value)
                        dropDownTONew.Width = Convert.ToInt32(dateReader["width"].ToString());
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
        #endregion

        #region Insertion
        public int InsertDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimProdSpecDescTO, cmdInsert);
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

        public int InsertDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimProdSpecDescTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(DimProdSpecDescTO dimProdSpecDescTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @"INSERT INTO [dimProdSpec](" +
                           " [idProdSpec]" +
            " ,[isActive]" +
            " ,[prodSpecDesc]" +
            ", [displaySequence]" +
            " )" +
             " VALUES (" + "@idProdSpec"+
                            " ,@isActive " +
                            " ,@prodSpecDesc " +
                            " ,@displaySequence " +
                            " )";            
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@idProdSpec", System.Data.SqlDbType.Int).Value = dimProdSpecDescTO.IdProductSpecDesc;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 1;
            cmdInsert.Parameters.Add("@prodSpecDesc", System.Data.SqlDbType.NVarChar).Value = dimProdSpecDescTO.ProdSpecDesc;
            cmdInsert.Parameters.Add("@displaySequence", System.Data.SqlDbType.NVarChar).Value = dimProdSpecDescTO.DisplaySequence;
            return cmdInsert.ExecuteNonQuery();
            
        }


        /// <summary>
        /// Added by vinod for the selection of the max records on Dated :12/12/2017
        /// </summary>
        /// <param name="cmdInsert"></param>
        /// <returns></returns>
        public int ExecuteSelectCommand(SqlCommand cmdInsert)
        {
            String sqlQuery = @"SELECT MAX(idProdSpec+1) FROM [dimProdSpec]"; /// where isActive = 1
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            int resultSet = Convert.ToInt32(cmdInsert.ExecuteScalar());
            return resultSet;
        }

        #endregion

        #region Updation
        public int UpdateDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimProdSpecDescTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimProdSpecDescTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(DimProdSpecDescTO dimProdSpecDescTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimProdSpec] SET " +
            " [isActive] = @isActive" +
            " ,[prodSpecDesc] = @prodSpecDesc" +
            " ,[displaySequence] = @displaySequence" +
            " WHERE idProdSpec=" + dimProdSpecDescTO.IdProductSpecDesc;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 1;
            cmdUpdate.Parameters.Add("@prodSpecDesc", System.Data.SqlDbType.NVarChar).Value = dimProdSpecDescTO.ProdSpecDesc;
            cmdUpdate.Parameters.Add("@displaySequence", System.Data.SqlDbType.Int).Value = dimProdSpecDescTO.DisplaySequence;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion

        public int UpdateDimProdSpecDescription(DimProdSpecDescTO dimProdSpecDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdateCommand(dimProdSpecDescTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateDimProdSpecDescription(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdateCommand(dimProdSpecDescTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdateCommand(DimProdSpecDescTO dimProdSpecDescTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimProdSpec] SET " +
            " [isActive] = @isActive" +
            " ,[prodSpecDesc] = @prodSpecDesc" +
            " ,[displaySequence] = @displaySequence" +
            " WHERE idProdSpec=" + dimProdSpecDescTO.IdProductSpecDesc;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 0;
            cmdUpdate.Parameters.Add("@prodSpecDesc", System.Data.SqlDbType.NVarChar).Value = dimProdSpecDescTO.ProdSpecDesc;
            cmdUpdate.Parameters.Add("@displaySequence", System.Data.SqlDbType.Int).Value = dimProdSpecDescTO.DisplaySequence;
            return cmdUpdate.ExecuteNonQuery();
        }


        #endregion
    }
}
