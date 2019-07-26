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
    public class TblLoadingSlipRemovedItemsDAO : ITblLoadingSlipRemovedItemsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblLoadingSlipRemovedItemsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblLoadingSlipRemovedItems]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblLoadingSlipRemovedItemsTO> SelectAllTblLoadingSlipRemovedItems()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;



                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipRemovedItemsTO> list = ConvertDTToList(reader);
                return list;
                //cmdSelect.Parameters.Add("@idLoadingSlipRemovedItems", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.IdLoadingSlipRemovedItems;
               
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblLoadingSlipRemovedItemsTO SelectTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idLoadingSlipRemovedItems = " + idLoadingSlipRemovedItems +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipRemovedItemsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
                //cmdSelect.Parameters.Add("@idLoadingSlipRemovedItems", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.IdLoadingSlipRemovedItems;
                
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblLoadingSlipRemovedItemsTO> ConvertDTToList(SqlDataReader tblLoadingSlipRemovedItemsTODT)
        {
            List<TblLoadingSlipRemovedItemsTO> tblLoadingSlipRemovedItemsTOList = new List<TblLoadingSlipRemovedItemsTO>();
            if (tblLoadingSlipRemovedItemsTODT != null)
            {
                while (tblLoadingSlipRemovedItemsTODT.Read())
                {

                    TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTONew = new TblLoadingSlipRemovedItemsTO();
                    if (tblLoadingSlipRemovedItemsTODT["idLoadingSlipRemovedItems"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.IdLoadingSlipRemovedItems = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["idLoadingSlipRemovedItems"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["bookingId"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.BookingId = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["bookingId"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["loadingLayerid"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.LoadingLayerid = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["loadingLayerid"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["materialId"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.MaterialId = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["materialId"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["prodCatId"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.ProdCatId = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["prodCatId"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.ProdSpecId = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["prodSpecId"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["parityDtlId"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.ParityDtlId = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["parityDtlId"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["brandId"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.BrandId = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["brandId"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["updatedBy"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.UpdatedBy = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["updatedBy"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["updatedOn"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.UpdatedOn = Convert.ToDateTime(tblLoadingSlipRemovedItemsTODT["updatedOn"].ToString());
                    if (tblLoadingSlipRemovedItemsTODT["ratePerMT"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.RatePerMT = Convert.ToDouble(tblLoadingSlipRemovedItemsTODT["ratePerMT"].ToString());

                    if (tblLoadingSlipRemovedItemsTODT["loadingSlipExtId"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.LoadingSlipExtId = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["loadingSlipExtId"].ToString());

                    if (tblLoadingSlipRemovedItemsTODT["createdBy"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.CreatedBy = Convert.ToInt32(tblLoadingSlipRemovedItemsTODT["createdBy"].ToString());

                    if (tblLoadingSlipRemovedItemsTODT["loadingQty"] != DBNull.Value)
                        tblLoadingSlipRemovedItemsTONew.LoadingQty = Convert.ToDouble(tblLoadingSlipRemovedItemsTODT["loadingQty"].ToString());

                    tblLoadingSlipRemovedItemsTOList.Add(tblLoadingSlipRemovedItemsTONew);


                }
            }
            return tblLoadingSlipRemovedItemsTOList;
        }

        public List<TblLoadingSlipRemovedItemsTO> SelectAllTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipRemovedItemsTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        #endregion
        
        #region Insertion
        public int InsertTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingSlipRemovedItemsTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingSlipRemovedItemsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLoadingSlipRemovedItems]( " + 
           // "  [idLoadingSlipRemovedItems]" +
            " [bookingId]" +
            " ,[loadingLayerid]" +
            " ,[materialId]" +
            " ,[prodCatId]" +
            " ,[prodSpecId]" +
            " ,[parityDtlId]" +
            " ,[brandId]" +
            " ,[updatedBy]" +
            " ,[updatedOn]" +
            " ,[ratePerMT]" +
            " ,[loadingSlipExtId]"+
            " ,[createdBy] "+
            " ,[loadingQty]" +
            " ,[prodItemId]" +    //[05-09-2018]Vijaymala added to set other item id
            " )" +
" VALUES (" +
         //   "  @IdLoadingSlipRemovedItems " +
            " @BookingId " +
            " ,@LoadingLayerid " +
            " ,@MaterialId " +
            " ,@ProdCatId " +
            " ,@ProdSpecId " +
            " ,@ParityDtlId " +
            " ,@BrandId " +
            " ,@UpdatedBy " +
            " ,@UpdatedOn " +
            " ,@RatePerMT " +
            " ,@LoadingSlipExtId" +
            " ,@CreatedBy" +
            " ,@LoadingQty" +
            " ,@ProdItemId" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

          //cmdInsert.Parameters.Add("@IdLoadingSlipRemovedItems", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.IdLoadingSlipRemovedItems;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.BookingId;
            cmdInsert.Parameters.Add("@LoadingLayerid", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.LoadingLayerid;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipRemovedItemsTO.MaterialId);
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipRemovedItemsTO.ProdCatId);
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipRemovedItemsTO.ProdSpecId);
            cmdInsert.Parameters.Add("@ParityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipRemovedItemsTO.ParityDtlId);
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipRemovedItemsTO.BrandId);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipRemovedItemsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipRemovedItemsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@RatePerMT", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipRemovedItemsTO.RatePerMT;
            cmdInsert.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.LoadingSlipExtId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.CreatedBy;
            cmdInsert.Parameters.Add("@LoadingQty", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipRemovedItemsTO.LoadingQty;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipRemovedItemsTO.ProdItemId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipRemovedItemsTO.IdLoadingSlipRemovedItems = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingSlipRemovedItemsTO, cmdUpdate);
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

        public int UpdateTblLoadingSlipRemovedItems(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingSlipRemovedItemsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLoadingSlipRemovedItems] SET " + 
            "  [idLoadingSlipRemovedItems] = @IdLoadingSlipRemovedItems" +
            " ,[bookingId]= @BookingId" +
            " ,[loadingLayerid]= @LoadingLayerid" +
            " ,[materialId]= @MaterialId" +
            " ,[prodCatId]= @ProdCatId" +
            " ,[prodSpecId]= @ProdSpecId" +
            " ,[parityDtlId]= @ParityDtlId" +
            " ,[brandId]= @BrandId" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[ratePerMT] = @RatePerMT" +
            " ,[loadingSlipExtId]= @LoadingSlipExtId"+
            " ,[createdBy] = @CreatedBy" +
            " ,[loadingQty] = @LoadingQty" +
            " ,[prodItemId]= @ProdItemId" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingSlipRemovedItems", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.IdLoadingSlipRemovedItems;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.BookingId;
            cmdUpdate.Parameters.Add("@LoadingLayerid", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.LoadingLayerid;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.MaterialId;
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.ProdCatId;
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.ProdSpecId;
            cmdUpdate.Parameters.Add("@ParityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipRemovedItemsTO.ParityDtlId);
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.BrandId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipRemovedItemsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@RatePerMT", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipRemovedItemsTO.RatePerMT;
            cmdUpdate.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.LoadingSlipExtId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@LoadingQty", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.LoadingQty;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.ProdItemId;

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingSlipRemovedItems, cmdDelete);
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

        public int DeleteTblLoadingSlipRemovedItems(Int32 idLoadingSlipRemovedItems, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingSlipRemovedItems, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idLoadingSlipRemovedItems, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblLoadingSlipRemovedItems] " +
            " WHERE idLoadingSlipRemovedItems = " + idLoadingSlipRemovedItems +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingSlipRemovedItems", System.Data.SqlDbType.Int).Value = tblLoadingSlipRemovedItemsTO.IdLoadingSlipRemovedItems;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
