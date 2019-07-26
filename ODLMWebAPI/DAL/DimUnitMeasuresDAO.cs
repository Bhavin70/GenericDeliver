using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimUnitMeasuresDAO : IDimUnitMeasuresDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimUnitMeasuresDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimUnitMeasures]";
            return sqlSelectQry;
        }



        #endregion

        #region Selection

        /// <summary>
        /// Vaibhav [13-Sep-2017] Added to select all measurement units for drop down
        /// </summary> 
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectAllUnitMeasuresForDropDown()
        { 
            ResultMessage resultMessage = new ResultMessage();

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 AND unitMeasureTypeId=" + unitMeasureTypeId;
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblUnitMeasuresTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblUnitMeasuresTODT != null)
                {
                    while (tblUnitMeasuresTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblUnitMeasuresTODT["idWeightMeasurUnit"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblUnitMeasuresTODT["idWeightMeasurUnit"].ToString());
                        if (tblUnitMeasuresTODT["weightMeasurUnitDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblUnitMeasuresTODT["weightMeasurUnitDesc"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DropDownTO> SelectAllUnitMeasuresForDropDownByCatId(Int32 unitCatId)
        {
            ResultMessage resultMessage = new ResultMessage();

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 AND unitMeasureTypeId=" + unitMeasureTypeId;
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 AND unitCatId=" + unitCatId ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblUnitMeasuresTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblUnitMeasuresTODT != null)
                {
                    while (tblUnitMeasuresTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblUnitMeasuresTODT["idWeightMeasurUnit"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblUnitMeasuresTODT["idWeightMeasurUnit"].ToString());
                        if (tblUnitMeasuresTODT["weightMeasurUnitDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblUnitMeasuresTODT["weightMeasurUnitDesc"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimUnitMeasuresTO> SelectAllDimUnitMeasures()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUnitMeasuresTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        public DimUnitMeasuresTO SelectDimUnitMeasures(Int32 idWeightMeasurUnit)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idWeightMeasurUnit = " + idWeightMeasurUnit + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUnitMeasuresTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();

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

        public List<DimUnitMeasuresTO> ConvertDTToList(SqlDataReader dimUnitMeasuresTODT)
        {
            List<DimUnitMeasuresTO> tblMaterialTOList = new List<DimUnitMeasuresTO>();
            if (dimUnitMeasuresTODT != null)
            {
                while (dimUnitMeasuresTODT.Read())
                {
                    DimUnitMeasuresTO dimUnitMeasuresTONew = new DimUnitMeasuresTO();
                    if (dimUnitMeasuresTODT["idWeightMeasurUnit"] != DBNull.Value)
                        dimUnitMeasuresTONew.IdWeightMeasurUnit = Convert.ToInt32(dimUnitMeasuresTODT["idWeightMeasurUnit"].ToString());
                    if (dimUnitMeasuresTODT["weightMeasurUnitDesc"] != DBNull.Value)
                        dimUnitMeasuresTONew.WeightMeasurUnitDesc = dimUnitMeasuresTODT["weightMeasurUnitDesc"].ToString();
                    if (dimUnitMeasuresTODT["isActive"] != DBNull.Value)
                        dimUnitMeasuresTONew.IsActive = Convert.ToInt32(dimUnitMeasuresTODT["isActive"].ToString());

                    tblMaterialTOList.Add(dimUnitMeasuresTONew);
                }
            }
            return tblMaterialTOList;
        }

        #endregion

        #region Insertion
        public int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimUnitMeasuresTO, cmdInsert);
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

        public int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimUnitMeasuresTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimUnitMeasuresTO dimUnitMeasuresTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimUnitMeasures]( " +
            "  [idWeightMeasurUnit]" +
            " ,[isActive]" +
            " ,[weightMeasurUnitDesc]" +
            " )" +
            " VALUES (" +
            "  @IdWeightMeasurUnit " +
            " ,@IsActive " +
            " ,@WeightMeasurUnitDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdWeightMeasurUnit", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IdWeightMeasurUnit;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IsActive;
            cmdInsert.Parameters.Add("@WeightMeasurUnitDesc", System.Data.SqlDbType.NVarChar).Value = dimUnitMeasuresTO.WeightMeasurUnitDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimUnitMeasuresTO, cmdUpdate);
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

        public int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimUnitMeasuresTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimUnitMeasuresTO dimUnitMeasuresTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimUnitMeasures] SET " +
            "  [idWeightMeasurUnit] = @IdWeightMeasurUnit" +
            " ,[isActive]= @IsActive" +
            " ,[weightMeasurUnitDesc] = @WeightMeasurUnitDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdWeightMeasurUnit", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IdWeightMeasurUnit;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IsActive;
            cmdUpdate.Parameters.Add("@WeightMeasurUnitDesc", System.Data.SqlDbType.NVarChar).Value = dimUnitMeasuresTO.WeightMeasurUnitDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idWeightMeasurUnit, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idWeightMeasurUnit, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idWeightMeasurUnit, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimUnitMeasures] " +
            " WHERE idWeightMeasurUnit = " + idWeightMeasurUnit + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idWeightMeasurUnit", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IdWeightMeasurUnit;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
