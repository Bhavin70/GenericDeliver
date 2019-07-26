using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Data.SqlClient;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimTalukaDAO : IDimTalukaDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimTalukaDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Insertion

        public int InsertDimTaluka(StateMasterTO dimTalukaTO)
		{
			String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
			SqlConnection conn = new SqlConnection(sqlConnStr);
			SqlCommand cmdUpdate = new SqlCommand();
			try
			{
				conn.Open();
				cmdUpdate.Connection = conn;
				return ExecuteInsertCommand(dimTalukaTO, cmdUpdate);
			}
			catch (Exception ex)
			{
				return -1;
			}
			finally
			{
				conn.Close();
				cmdUpdate.Dispose();
			}


		}
		public int ExecuteInsertCommand(StateMasterTO dimTalukaTO, SqlCommand cmdInsert)
		{

			String sqlQuery = @" INSERT INTO [dimTaluka]( " +

								"   [talukaName]" +
								" ,[talukaCode]" +
								" ,[districtId]" +
								" )" +
					" VALUES (" +

								"  @talukaName " +
								" ,@talukaCode " +
								" ,@districtId " +

								" )";
			cmdInsert.CommandText = sqlQuery;
			cmdInsert.CommandType = System.Data.CommandType.Text;

			//cmdInsert.Parameters.Add("@idDistrict", System.Data.SqlDbType.Int).Value = dimDistrictTO.Id;
			cmdInsert.Parameters.Add("@talukaName", System.Data.SqlDbType.NVarChar).Value = dimTalukaTO.Name;
			cmdInsert.Parameters.Add("@districtId", System.Data.SqlDbType.Int).Value = dimTalukaTO.ParentId;
			cmdInsert.Parameters.Add("@talukaCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimTalukaTO.Code);
			if (cmdInsert.ExecuteNonQuery() == 1)
			{
				cmdInsert.CommandText = Constants.IdentityColumnQuery;
				dimTalukaTO.Id = Convert.ToInt32(cmdInsert.ExecuteScalar());
				return 1;
			}
			else return 0;
		}


		#endregion


		#region Updation
		public int UpdateDimTaluka(StateMasterTO dimDistrictTO)
		{
			String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
			SqlConnection conn = new SqlConnection(sqlConnStr);
			SqlCommand cmdUpdate = new SqlCommand();
			try
			{
				conn.Open();
				cmdUpdate.Connection = conn;
				return ExecuteUpdateCommand(dimDistrictTO, cmdUpdate);
			}
			catch (Exception ex)
			{
				return -1;
			}
			finally
			{
				conn.Close();
				cmdUpdate.Dispose();
			}


		}
		public int ExecuteUpdateCommand(StateMasterTO dimTal, SqlCommand cmdUpdate)
		{
			String sqlQuery = @" UPDATE [dimTaluka] SET " +
			//"  [idState] = @IdState" +
			" [talukaName]= @talName" +
			",[talukaCode]=@talCode"+
			" WHERE [idTaluka] =  @Idtal ";

			cmdUpdate.CommandText = sqlQuery;
			cmdUpdate.CommandType = System.Data.CommandType.Text;

			cmdUpdate.Parameters.Add("@talName", System.Data.SqlDbType.NVarChar).Value = dimTal.Name;
			cmdUpdate.Parameters.Add("@Idtal", System.Data.SqlDbType.Int).Value = dimTal.Id;
			cmdUpdate.Parameters.Add("@talCode",System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimTal.Code);
			return cmdUpdate.ExecuteNonQuery();
		}

	}
	#endregion
}

