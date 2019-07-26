using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimDistrictDAO : IDimDistrictDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimDistrictDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Insertion

        public int InsertDimDistrict(StateMasterTO dimDistrictTO)
		{
			String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
			SqlConnection conn = new SqlConnection(sqlConnStr);
			SqlCommand cmdUpdate = new SqlCommand();
			try
			{
				conn.Open();
				cmdUpdate.Connection = conn;
				return ExecuteInsertCommand(dimDistrictTO, cmdUpdate);
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
		public int ExecuteInsertCommand(StateMasterTO dimDistrictTO, SqlCommand cmdInsert)
		{

			String sqlQuery = @" INSERT INTO [dimDistrict]( " +
									
								"   [districtName]" +
								" ,[districtCode]" +
								" ,[stateId]" +
								" )" +
					" VALUES (" +
					            
								"  @districtName " +
								" ,@districtCode " +
								" ,@StateId " +
								
								" )";
			cmdInsert.CommandText = sqlQuery;
			cmdInsert.CommandType = System.Data.CommandType.Text;

			//cmdInsert.Parameters.Add("@idDistrict", System.Data.SqlDbType.Int).Value = dimDistrictTO.Id;
			cmdInsert.Parameters.Add("@districtName", System.Data.SqlDbType.NVarChar).Value = dimDistrictTO.Name;
			cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = dimDistrictTO.ParentId;
			cmdInsert.Parameters.Add("@districtCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimDistrictTO.Code);
			if (cmdInsert.ExecuteNonQuery() == 1)
			{
				cmdInsert.CommandText = Constants.IdentityColumnQuery;
				dimDistrictTO.Id = Convert.ToInt32(cmdInsert.ExecuteScalar());
				return 1;
			}
			else return 0;
		}


		#endregion

		#region Updation
		public int UpdateDimDistrict(StateMasterTO dimDistrictTO)
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
		public int ExecuteUpdateCommand(StateMasterTO dimDistrictTO, SqlCommand cmdUpdate)
		{
			String sqlQuery = @" UPDATE [dimDistrict] SET " +
			//"  [idState] = @IdState" +
			" [districtName]= @districtName" +
			", [districtCode]= @districtCode" +
			" WHERE [idDistrict] =  @IdDistrict ";

			cmdUpdate.CommandText = sqlQuery;
			cmdUpdate.CommandType = System.Data.CommandType.Text;

			cmdUpdate.Parameters.Add("@districtName", System.Data.SqlDbType.NVarChar).Value = dimDistrictTO.Name;
			cmdUpdate.Parameters.Add("@IdDistrict", System.Data.SqlDbType.Int).Value = dimDistrictTO.Id;
			cmdUpdate.Parameters.Add("@districtCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimDistrictTO.Code);
			return cmdUpdate.ExecuteNonQuery();
		}

	}
	#endregion


}


