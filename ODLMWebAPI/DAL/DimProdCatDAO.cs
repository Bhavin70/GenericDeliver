using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimProdCatDAO : IDimProdCatDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimProdCatDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimProdCat]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimProdCatTO> SelectAllDimProdCat()
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

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimProdCatTO> list = ConvertDTToList(rdr);
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

        public DimProdCatTO SelectDimProdCat(Int32 idProdCat)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idProdCat = " + idProdCat + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimProdCatTO> list = ConvertDTToList(rdr);
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

        public List<DimProdCatTO> ConvertDTToList(SqlDataReader dimProdCatTODT)
        {
            List<DimProdCatTO> dimProdCatTOList = new List<DimProdCatTO>();
            if (dimProdCatTODT != null)
            {
                while (dimProdCatTODT.Read())
                {
                    DimProdCatTO dimProdCatTONew = new DimProdCatTO();
                    if (dimProdCatTODT["idProdCat"] != DBNull.Value)
                        dimProdCatTONew.IdProdCat = Convert.ToInt32(dimProdCatTODT["idProdCat"].ToString());
                    if (dimProdCatTODT["isActive"] != DBNull.Value)
                        dimProdCatTONew.IsActive = Convert.ToInt32(dimProdCatTODT["isActive"].ToString());
                    if (dimProdCatTODT["prodCateDesc"] != DBNull.Value)
                        dimProdCatTONew.ProdCateDesc = Convert.ToString(dimProdCatTODT["prodCateDesc"].ToString());
                    if (dimProdCatTODT["materialIdStr"] != DBNull.Value)
                        dimProdCatTONew.MaterialIdStr = Convert.ToString(dimProdCatTODT["materialIdStr"].ToString());
                    if (dimProdCatTODT["specificationIdStr"] != DBNull.Value)
                        dimProdCatTONew.SpecificationIdStr = Convert.ToString(dimProdCatTODT["specificationIdStr"].ToString());
                    dimProdCatTOList.Add(dimProdCatTONew);
                }
            }
            return dimProdCatTOList;
        }

        #endregion

        #region Insertion
        public int InsertDimProdCat(DimProdCatTO dimProdCatTO)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimProdCatTO, cmdInsert);
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

        public int InsertDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimProdCatTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimProdCatTO dimProdCatTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimProdCat]( " + 
            "  [idProdCat]" +
            " ,[isActive]" +
            " ,[prodCateDesc]" +
            " )" +
" VALUES (" +
            "  @IdProdCat " +
            " ,@IsActive " +
            " ,@ProdCateDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdProdCat", System.Data.SqlDbType.Int).Value = dimProdCatTO.IdProdCat;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimProdCatTO.IsActive;
            cmdInsert.Parameters.Add("@ProdCateDesc", System.Data.SqlDbType.NVarChar).Value = dimProdCatTO.ProdCateDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateDimProdCat(DimProdCatTO dimProdCatTO)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimProdCatTO, cmdUpdate);
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

        public int UpdateDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimProdCatTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimProdCatTO dimProdCatTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimProdCat] SET " + 
            "  [idProdCat] = @IdProdCat" +
            " ,[isActive]= @IsActive" +
            " ,[prodCateDesc] = @ProdCateDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdCat", System.Data.SqlDbType.Int).Value = dimProdCatTO.IdProdCat;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimProdCatTO.IsActive;
            cmdUpdate.Parameters.Add("@ProdCateDesc", System.Data.SqlDbType.NVarChar).Value = dimProdCatTO.ProdCateDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimProdCat(Int32 idProdCat)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdCat, cmdDelete);
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

        public int DeleteDimProdCat(Int32 idProdCat, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdCat, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idProdCat, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimProdCat] " +
            " WHERE idProdCat = " + idProdCat +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdCat", System.Data.SqlDbType.Int).Value = dimProdCatTO.IdProdCat;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
