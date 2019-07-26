using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{ 
    public class DimAddonsFunDAO : IDimAddonsFunDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimAddonsFunDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimAddonsFun]"; 
            return sqlSelectQry;
        }

        

        #endregion

        #region Selection
        public List<DimAddonsFunTO> SelectAllDimAddonsFun()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            { 
                       conn.Open();
            cmdSelect.CommandText = SqlSelectQuery();
            cmdSelect.Connection = conn;
            cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
            List<DimAddonsFunTO> list = ConvertDTToList(sqlReader);
            return list;
            }
            catch(Exception ex)
            {
       
                return null;
            }
            finally
            {
                if (sqlReader != null) sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DimAddonsFunTO SelectDimAddonsFun(Int32 idAddonsFun)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idAddonsFun = " + idAddonsFun +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idAddonsFun", System.Data.SqlDbType.Int).Value = dimAddonsFunTO.IdAddonsFun;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
               List<DimAddonsFunTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }


            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null) rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimAddonsFunTO> ConvertDTToList(SqlDataReader dimAddonsFunTODT)
        {
            List<DimAddonsFunTO> dimAddonsFunTOList = new List<DimAddonsFunTO>();
            if (dimAddonsFunTODT != null)
            {
                while (dimAddonsFunTODT.Read())
                {

                    DimAddonsFunTO dimAddonsFunTONew = new DimAddonsFunTO();
                    if (dimAddonsFunTODT["idAddonsFun"] != DBNull.Value)
                        dimAddonsFunTONew.IdAddonsFun = Convert.ToInt32(dimAddonsFunTODT["idAddonsFun"].ToString());
                    if (dimAddonsFunTODT["funName"] != DBNull.Value)
                        dimAddonsFunTONew.FunName = Convert.ToString(dimAddonsFunTODT["funName"].ToString());
                    dimAddonsFunTOList.Add(dimAddonsFunTONew);
                }
            }
            return dimAddonsFunTOList;
        }


        public List<DimAddonsFunTO> SelectAllDimAddonsFun(SqlConnection conn, SqlTransaction tran)
        {

                SqlCommand cmdSelect = new SqlCommand();
        SqlDataReader reader = null;
        cmdSelect.Connection = conn;
            cmdSelect.Transaction = tran;
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE  isActive = 1";
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimAddonsFunTO> list = ConvertDTToList(reader);
             
                    return list;

                return null;

            }
            catch (Exception ex)
            {
          
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion
        
        #region Insertion
        public int InsertDimAddonsFun(DimAddonsFunTO dimAddonsFunTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimAddonsFunTO, cmdInsert);
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

        public int InsertDimAddonsFun(DimAddonsFunTO dimAddonsFunTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimAddonsFunTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimAddonsFunTO dimAddonsFunTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimAddonsFun]( " + 
        //    "  [idAddonsFun]," +
            " [funName]" +
            " )" +
" VALUES (" +
         //   "  @IdAddonsFun, " +
            " @FunName " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

          //  cmdInsert.Parameters.Add("@IdAddonsFun", System.Data.SqlDbType.Int).Value = dimAddonsFunTO.IdAddonsFun;
            cmdInsert.Parameters.Add("@FunName", System.Data.SqlDbType.NVarChar).Value = dimAddonsFunTO.FunName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateDimAddonsFun(DimAddonsFunTO dimAddonsFunTO)
        { 
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        SqlConnection conn = new SqlConnection(sqlConnStr);
        SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimAddonsFunTO, cmdUpdate);
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

        public int UpdateDimAddonsFun(DimAddonsFunTO dimAddonsFunTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimAddonsFunTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimAddonsFunTO dimAddonsFunTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimAddonsFun] SET " + 
         //   "  [idAddonsFun] = @IdAddonsFun," +
            " [funName] = @FunName" +
            " WHERE idAddonsFun= "+ dimAddonsFunTO.IdAddonsFun; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

         //   cmdUpdate.Parameters.Add("@IdAddonsFun", System.Data.SqlDbType.Int).Value = dimAddonsFunTO.IdAddonsFun;
            cmdUpdate.Parameters.Add("@FunName", System.Data.SqlDbType.NVarChar).Value = dimAddonsFunTO.FunName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimAddonsFun(Int32 idAddonsFun)
        {


            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAddonsFun, cmdDelete);
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

        public int DeleteDimAddonsFun(Int32 idAddonsFun, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAddonsFun, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idAddonsFun, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimAddonsFun] " +
            " WHERE idAddonsFun = " + idAddonsFun +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idAddonsFun", System.Data.SqlDbType.Int).Value = dimAddonsFunTO.IdAddonsFun;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
