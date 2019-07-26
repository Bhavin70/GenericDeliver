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
    public class DynamicApprovalCycleDAO : IDynamicApprovalCycleDAO
    {
       private readonly IConnectionString _iConnectionString;
        private readonly ISQLHelper _sqlHelper;
         private readonly ICommon _icommon;
        public DynamicApprovalCycleDAO(ICommon _icommon,IConnectionString iConnectionString,ISQLHelper _sqlHelper )
        {
            _iConnectionString = iConnectionString;
             this._sqlHelper=_sqlHelper;
             this._icommon=_icommon;
        }
         public List<DimDynamicApprovalTO> SelectAllApprovalList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText ="select * from dimApproval where isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allApproval = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimDynamicApprovalTO> ApprovalList = ConvertDTToList(allApproval);
                if (ApprovalList != null)
                    return ApprovalList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllApprovalList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        
         public DataTable SelectAllList(int seqNo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText ="select * from dimApproval where isActive=1 and sequenceNo="+seqNo;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allApprovalList = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DataTable dt=new DataTable();
                dt.Load(allApprovalList);
               
                if (dt.Rows.Count>0)
                    {
                      string SelectQuery= Convert.ToString(dt.Rows[0]["selectQuery"]);
                      cmdSelect.CommandText=SelectQuery;
                      
                      return   _sqlHelper.ExecuteReaderDataTable<DataTable>(System.Data.CommandType.Text,SelectQuery,sqlConnStr,null);
                     
                  
                    }
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public int UpdateStatus(Dictionary<string, string> tableData,int status,int userId,int seqNo)
        {
          String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
              SqlTransaction tran = null;
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
             try
            {
              DateTime currentdate=_icommon.ServerDateTime;
              string areaId;
              string moduleId;
                conn.Open();
                cmdSelect.CommandText ="select * from dimApproval where isActive=1 and sequenceNo="+seqNo;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allApprovalList = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DataTable dt=new DataTable();
                dt.Load(allApprovalList);
               conn.Close();
                if (dt.Rows.Count>0)
                    {
                      string SelectQuery= Convert.ToString(dt.Rows[0]["updateQuery"]);
                      string authorisedStatusId=Convert.ToString(dt.Rows[0]["authorisedStatusId"]);
                      string rejectStatusId=Convert.ToString(dt.Rows[0]["rejectStatusId"]);
                      areaId=Convert.ToString(dt.Rows[0]["approvalId"]);
                        moduleId=Convert.ToString(dt.Rows[0]["moduleId"]);
                          conn.Open();
                        tran = conn.BeginTransaction ();
                      SqlCommand cmdUpdate =new SqlCommand();
                      cmdUpdate.CommandText=SelectQuery;
                      cmdUpdate.CommandType = System.Data.CommandType.Text;
                      cmdUpdate.Connection = conn;
                      cmdUpdate.Transaction = tran;
                        
                      if(status==1)
                      {
                    cmdUpdate.Parameters.AddWithValue("@status",authorisedStatusId);
                      } else{
                        cmdUpdate.Parameters.AddWithValue("@status",rejectStatusId);
                      }
                     
                      cmdUpdate.Parameters.AddWithValue("updatedBy",userId);
                      cmdUpdate.Parameters.AddWithValue("updatedOn",currentdate);
                      cmdUpdate.Parameters.AddWithValue("id",tableData["Id"]);
                     
                    
              
                int result= cmdUpdate.ExecuteNonQuery();
                if(result<=0)
                {
                    tran.Rollback();
                        return 0;
                }
   SqlCommand cmdInsert= new SqlCommand();
               cmdInsert.CommandText="insert into dynamicApprovalHistory values (@transactionId,@statusId,@areaId,@moduleId,@createdOn,@createdBy)";
             
               cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                cmdInsert.Parameters.AddWithValue("@transactionId",tableData["Id"]);
                if(status==1)
                      {
                    cmdInsert.Parameters.AddWithValue("@statusId",authorisedStatusId);
                      } else{
                        cmdInsert.Parameters.AddWithValue("@statusId",rejectStatusId);
                      }
                cmdInsert.Parameters.AddWithValue("@areaId",areaId);
                  cmdInsert.Parameters.AddWithValue("@moduleId",moduleId);
                cmdInsert.Parameters.AddWithValue("@createdOn",currentdate);
                cmdInsert.Parameters.AddWithValue("@createdBy",userId);
               result= cmdInsert.ExecuteNonQuery();
               if(result!=1)
               {
                 tran.Rollback();
                 return 0;

               }
               tran.Commit();
                      return 1;
                     
                     
                  
                    }
                    return 0;
            
            }
            catch (Exception ex)
            { tran.Rollback();
                
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateStatus");
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }
         public List<DimDynamicApprovalTO> ConvertDTToList(SqlDataReader dimVehicleTypeTODT)
        {
            List<DimDynamicApprovalTO> dimVehicleTypeTOList = new List<DimDynamicApprovalTO>();
            if (dimVehicleTypeTODT != null)
            {
                while (dimVehicleTypeTODT.Read())
                {
                    DimDynamicApprovalTO dimDynamciAoproval = new DimDynamicApprovalTO();
                    if (dimVehicleTypeTODT["approvalName"] != DBNull.Value)
                        dimDynamciAoproval.ApprovalName =(dimVehicleTypeTODT["approvalName"].ToString());
                    if (dimVehicleTypeTODT["sequenceNo"] != DBNull.Value)
                        dimDynamciAoproval.SequenceNo = Convert.ToInt32(dimVehicleTypeTODT["sequenceNo"].ToString());

                          if (dimVehicleTypeTODT["approvalTypeId"] != DBNull.Value)
                        dimDynamciAoproval.ApprovalTypeId = Convert.ToInt32(dimVehicleTypeTODT["approvalTypeId"].ToString());
                       
                          if (dimVehicleTypeTODT["currentStatusId"] != DBNull.Value)
                        dimDynamciAoproval.CurrentStatusId = Convert.ToInt32(dimVehicleTypeTODT["currentStatusId"].ToString());
                          if (dimVehicleTypeTODT["authorisedStatusId"] != DBNull.Value)
                        dimDynamciAoproval.AuthorisedStatusId = Convert.ToInt32(dimVehicleTypeTODT["authorisedStatusId"].ToString());
                        
                          if (dimVehicleTypeTODT["rejectStatusId"] != DBNull.Value)
                        dimDynamciAoproval.RejectStatusId = Convert.ToInt32(dimVehicleTypeTODT["rejectStatusId"].ToString());
                     if (dimVehicleTypeTODT["isActive"] != DBNull.Value)
                        dimDynamciAoproval.IsActive = Convert.ToInt32(dimVehicleTypeTODT["isActive"].ToString());
                         if (dimVehicleTypeTODT["bootstrapIconName"] != DBNull.Value)
                        dimDynamciAoproval.BootstrapIconName = (dimVehicleTypeTODT["bootstrapIconName"].ToString()); 
                          if (dimVehicleTypeTODT["sysElementId"] != DBNull.Value)
                        dimDynamciAoproval.SysElementId = Convert.ToInt32(dimVehicleTypeTODT["sysElementId"].ToString());
                         if (dimVehicleTypeTODT["moduleId"] != DBNull.Value)
                        dimDynamciAoproval.ModuleId = Convert.ToInt32(dimVehicleTypeTODT["moduleId"].ToString());
                          if (dimVehicleTypeTODT["selectQuery"] != DBNull.Value)
                        dimDynamciAoproval.SelectQuery = (dimVehicleTypeTODT["selectQuery"].ToString()); 
                          if (dimVehicleTypeTODT["updateQuery"] != DBNull.Value)
                        dimDynamciAoproval.UpdateQuery = (dimVehicleTypeTODT["updateQuery"].ToString()); 
                          if (dimVehicleTypeTODT["navigateTo"] != DBNull.Value)
                        dimDynamciAoproval.NavigateTo = (dimVehicleTypeTODT["navigateTo"].ToString()); 
                    dimVehicleTypeTOList.Add(dimDynamciAoproval);
                }
            }
            return dimVehicleTypeTOList;
        }
    }

}