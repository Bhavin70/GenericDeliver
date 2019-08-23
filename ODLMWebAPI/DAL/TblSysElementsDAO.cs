using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.DAL
{
    public class TblSysElementsDAO : ITblSysElementsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblSysElementsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT sysElement.* , " +
                                  "CASE WHEN sysElement.menuId IS NULL THEN  CASE WHEN pgElement.pageEleTypeId is null THEN module.moduleName " +
                                  "ELSE pgElement.elementDisplayName END ELSE menus.menuName END AS elementName," +
                                  " CASE WHEN sysElement.menuId IS NULL THEN CASE WHEN pgElement.pageEleTypeId is null THEN module.moduleDesc " +
                                  "ELSE pgElement.elementDesc END ELSE menus.menuDesc END AS elementDesc " +
                                  " FROM tblSysElements sysElement " +
                                  " LEFT JOIN tblPageElements pgElement ON sysElement.pageElementId = pgElement.idPageElement " +
                                  " LEFT JOIN tblMenuStructure menus ON sysElement.menuId = menus.idMenu " +
                                  " LEFT JOIN tblModule module ON sysElement.moduleId = module.idModule ";

            return sqlSelectQry;
        }
          public String SqlSelectModuleQuery()
        {
            String sqlModuleqry="select module.moduleDesc,permission.permission,module.idModule" + 
                                " from tblSysElements as main "+
                                " left join tblModule as module on main.moduleId=module.idModule"+
                                " left join tblSysEleRoleEntitlements as permission on main.idSysElement=permission.sysEleId";
       return sqlModuleqry;
        }
         public String SqlSelectModulewrtUserQuery()
        {
             String sqlModuleUserqry="select module.moduleDesc,users.permission,module.idModule" + 
                                " from tblSysElements as main "+
                                " left join tblModule as module on main.moduleId=module.idModule"+
                                " left join tblSysEleUserEntitlements as users on main.idSysElement=users.sysEleId";
       return sqlModuleUserqry;
        }

         public static String SqlSelectMenuQuery()
        {
            String sqlMenuqry=" select menu.menuDesc,permission.permission,menu.idMenu " +
                                " from tblSysElements as main "+
                                " left join tblMenuStructure as menu on main.menuId=menu.idMenu "+
                                " left join tblSysEleRoleEntitlements as permission on main.idSysElement=permission.sysEleId ";
       return sqlMenuqry;
      
    
        }


        public static String SqlSelectElementQuery()
        {
            String sqlElementQuery= " select pageelement.elementName as menuDesc,roleelment.permission as permission,pages.idPage as idMenu " +
                                    " from tblPages pages " +
                                    " left join tblPageElements as pageelement on pageelement.pageId = pages.idPage " +
                                    " left join tblSysElements as syswlmwnt on syswlmwnt.pageElementId = pageelement.idPageElement " +
                                    " left join tblSysEleRoleEntitlements as roleelment on roleelment.sysEleId = syswlmwnt.idSysElement ";
                                    return sqlElementQuery;
        }

         public static String SqlSelectMenuUserQuery()
        {
            String sqlMenuUserqry=" select DISTINCT menu.menuDesc,users.permission,menu.idMenu " +
                                " from tblSysElements as main "+
                                " left join tblMenuStructure as menu on main.menuId=menu.idMenu "+
                                " left join tblSysEleUserEntitlements as users on main.idSysElement=users.sysEleId ";
       return sqlMenuUserqry; 
        }

        public static String SqlSelectElementUserQuery()
        {
            String sqlElementQuery= " select DISTINCT pageelement.elementName as menuDesc,users.permission as permission,pages.idPage as idMenu " +
                                    " from tblPages pages " +
                                    " left join tblPageElements as pageelement on pageelement.pageId = pages.idPage " +
                                    " left join tblSysElements as syswlmwnt on syswlmwnt.pageElementId = pageelement.idPageElement " +
                                   
                                    " left join tblSysEleUserEntitlements as users on users.sysEleId = syswlmwnt.idSysElement ";
                                    return sqlElementQuery;
        }

public static String SqlSelectAllPermissionQuery()
        {
            String sqlQuery= " select main.idsyselement,  module.moduleDesc + ' - ' + menu.menuDesc as menuDesc, page.pageDesc + ' - ' + elements.elementDesc as elementDesc from tblSysElements as main " +
                             " left join tblMenuStructure as menu on main.menuId=menu.idMenu " +
                              "left join tblModule as module on module.idModule=menu.moduleId" +
                             " left join tblPageElements as elements on main.pageElementId=elements.idPageElement " +
                             "  left join tblPages as page on elements.pageId=page.idPage " +
                             "  where (type='mi') or (type='e') ";
                                return sqlQuery;
        }                 

public static String SqlSelectRolewrtPermissionQuery()
{
    String sqlQuery= " select tblrole.roleDesc as name,roles.permission, main.idSysElement as sysEleId,tblrole.idRole as id from tblSysElements as main " +
                     " left join tblSysEleRoleEntitlements as roles on roles.sysEleId=main.idSysElement " +
                     " left join tblRole as tblrole on tblrole.idRole = roles.roleId " +
                     "  where roles.permission='rw' ";
                            return sqlQuery;

}                 
public static String SqlSelectUserwrtPermissionQuery()
{
    String sqlQuery= " select tbluser.userDisplayName as name,users.permission, main.idSysElement as sysEleId,tbluser.idUser as id from tblSysElements as main " +
                     " left join tblSysEleUserEntitlements as users on users.sysEleId=main.idSysElement " +
                     " left join tblUser as tbluser on tbluser.idUser = users.userId " +
                     "  where users.permission='rw' ";
                            return sqlQuery;

}  
        #endregion
#region user subscription
// user Tracking
 public  int SelectIsImportantPerson(int userId,int sysEleID)
        {
            int impPerson=0;
            String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
               
               cmdSelect.CommandText = "select isImpPerson from tblSysEleUserEntitlements where sysEleId="+sysEleID+" and userId="+userId;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while(rdr.Read())
                {
                    impPerson=Convert.ToInt32(rdr["isImpPerson"].ToString());
                }
                
              
                return impPerson;
            }
            catch(Exception ex)
            {
                return impPerson;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
//END
#endregion
        #region Selection
        public List<TblSysElementsTO> SelectAllTblSysElements(int menuPageId,string type,int moduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if (menuPageId == 0)
                {
                    if(menuPageId == 0 && moduleId != 0)
                        cmdSelect.CommandText = SqlSelectQuery() + " WHERE sysElement.type=" + "'" + type + "' AND menus.moduleId = " + moduleId;
                    else
                        cmdSelect.CommandText = SqlSelectQuery() + " WHERE sysElement.type=" + "'" + type + "'";
                }  
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE pgElement.pageId=" + menuPageId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysElementsTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblSysElementsTO SelectTblSysElements(Int32 idSysElement)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSysElement = " + idSysElement +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysElementsTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblSysElementsTO> ConvertDTToList(SqlDataReader tblSysElementsTODT)
        {
            List<TblSysElementsTO> tblSysElementsTOList = new List<TblSysElementsTO>();
            if (tblSysElementsTODT != null)
            {
                while (tblSysElementsTODT.Read())
                {
                    TblSysElementsTO tblSysElementsTONew = new TblSysElementsTO();
                    if (tblSysElementsTODT["idSysElement"] != DBNull.Value)
                        tblSysElementsTONew.IdSysElement = Convert.ToInt32(tblSysElementsTODT["idSysElement"].ToString());
                    if (tblSysElementsTODT["pageElementId"] != DBNull.Value)
                        tblSysElementsTONew.PageElementId = Convert.ToInt32(tblSysElementsTODT["pageElementId"].ToString());
                    if (tblSysElementsTODT["menuId"] != DBNull.Value)
                        tblSysElementsTONew.MenuId = Convert.ToInt32(tblSysElementsTODT["menuId"].ToString());
                    if (tblSysElementsTODT["type"] != DBNull.Value)
                        tblSysElementsTONew.Type = Convert.ToString(tblSysElementsTODT["type"].ToString());
                    if (tblSysElementsTODT["elementName"] != DBNull.Value)
                        tblSysElementsTONew.ElementName = Convert.ToString(tblSysElementsTODT["elementName"].ToString());
                    if (tblSysElementsTODT["elementDesc"] != DBNull.Value)
                        tblSysElementsTONew.ElementDesc = Convert.ToString(tblSysElementsTODT["elementDesc"].ToString());
                    if (tblSysElementsTODT["moduleId"] != DBNull.Value)
                        tblSysElementsTONew.ModuleId = Convert.ToInt32(tblSysElementsTODT["moduleId"].ToString());

                    tblSysElementsTOList.Add(tblSysElementsTONew);
                }
            }
            return tblSysElementsTOList;
        }

        public static List<tblViewPermissionTO> ConvertDTTOList(SqlDataReader tblViewPermissionTODT)
        {
            List<tblViewPermissionTO> tblViewPermissionTOList = new List<tblViewPermissionTO>();
            if (tblViewPermissionTODT != null)
            {
                while (tblViewPermissionTODT.Read())
                {
                    tblViewPermissionTO tblViewPermissionTONew = new tblViewPermissionTO();
                    if (tblViewPermissionTODT["moduleDesc"] != DBNull.Value)
                        tblViewPermissionTONew.ModuleDesc = Convert.ToString(tblViewPermissionTODT["moduleDesc"].ToString());
                    if (tblViewPermissionTODT["permission"] != DBNull.Value)
                        tblViewPermissionTONew.Permission = Convert.ToString(tblViewPermissionTODT["permission"].ToString());
                    if (tblViewPermissionTODT["idModule"] != DBNull.Value)
                        tblViewPermissionTONew.IdModule = Convert.ToInt32(tblViewPermissionTODT["idModule"].ToString());
                         
                   
                    tblViewPermissionTOList.Add(tblViewPermissionTONew);
                }
            }
            return tblViewPermissionTOList;
        }

        public static List<tblViewMenuTO> ConvertDTTOListMenu(SqlDataReader tblViewMenuTODT)
        {
            List<tblViewMenuTO> tblViewMenuTOList = new List<tblViewMenuTO>();
            if (tblViewMenuTODT != null)
            {
                while (tblViewMenuTODT.Read())
                {
                    tblViewMenuTO tblViewMenuTONew = new tblViewMenuTO();
                    if (tblViewMenuTODT["menuDesc"] != DBNull.Value)
                       tblViewMenuTONew.MenuDesc = Convert.ToString(tblViewMenuTODT["menuDesc"].ToString());
                    if (tblViewMenuTODT["permission"] != DBNull.Value)
                        tblViewMenuTONew.Permission = Convert.ToString(tblViewMenuTODT["permission"].ToString());
                    if (tblViewMenuTODT["idMenu"] != DBNull.Value)
                        tblViewMenuTONew.IdMenu = Convert.ToInt32(tblViewMenuTODT["idMenu"].ToString());

                    tblViewMenuTOList.Add(tblViewMenuTONew);
                }
            }
            return tblViewMenuTOList;
        }

         public static List<tblRoleUserTO> ConvertDTTOListRoleUser(SqlDataReader tblRoleUserTODT)
        {
            List<tblRoleUserTO> tblRoleUserTOList = new List<tblRoleUserTO>();
            if (tblRoleUserTODT != null)
            {
                while (tblRoleUserTODT.Read())
                {
                    tblRoleUserTO tblRoleUserTONew = new tblRoleUserTO();
                    if (tblRoleUserTODT["name"] != DBNull.Value)
                       tblRoleUserTONew.Name = Convert.ToString(tblRoleUserTODT["name"].ToString());
                    if (tblRoleUserTODT["permission"] != DBNull.Value)
                        tblRoleUserTONew.Permission = Convert.ToString(tblRoleUserTODT["permission"].ToString());
                    if (tblRoleUserTODT["sysEleId"] != DBNull.Value)
                        tblRoleUserTONew.SysEleId = Convert.ToInt32(tblRoleUserTODT["sysEleId"].ToString());
                    if (tblRoleUserTODT["id"] != DBNull.Value)
                        tblRoleUserTONew.Id = Convert.ToInt32(tblRoleUserTODT["id"].ToString());

                    tblRoleUserTOList.Add(tblRoleUserTONew);
                }
            }
            return tblRoleUserTOList;
        }



        #endregion

        #region Insertion
        public int InsertTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSysElementsTO, cmdInsert);
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

        public int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSysElementsTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSysElements]( " + 
                            "  [idSysElement]" +
                            " ,[pageElementId]" +
                            " ,[menuId]" +
                            " ,[type]" +
                            " )" +
                " VALUES (" +
                            "  @IdSysElement " +
                            " ,@PageElementId " +
                            " ,@MenuId " +
                            " ,@Type " + 
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdSysElement", System.Data.SqlDbType.Int).Value = tblSysElementsTO.IdSysElement;
            cmdInsert.Parameters.Add("@PageElementId", System.Data.SqlDbType.Int).Value = tblSysElementsTO.PageElementId;
            cmdInsert.Parameters.Add("@MenuId", System.Data.SqlDbType.Int).Value = tblSysElementsTO.MenuId;
            cmdInsert.Parameters.Add("@Type", System.Data.SqlDbType.Char).Value = tblSysElementsTO.Type;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSysElementsTO, cmdUpdate);
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

        public int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSysElementsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSysElements] SET " + 
            "  [idSysElement] = @IdSysElement" +
            " ,[pageElementId]= @PageElementId" +
            " ,[menuId]= @MenuId" +
            " ,[type] = @Type" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSysElement", System.Data.SqlDbType.Int).Value = tblSysElementsTO.IdSysElement;
            cmdUpdate.Parameters.Add("@PageElementId", System.Data.SqlDbType.Int).Value = tblSysElementsTO.PageElementId;
            cmdUpdate.Parameters.Add("@MenuId", System.Data.SqlDbType.Int).Value = tblSysElementsTO.MenuId;
            cmdUpdate.Parameters.Add("@Type", System.Data.SqlDbType.Char).Value = tblSysElementsTO.Type;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblSysElements(Int32 idSysElement)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSysElement, cmdDelete);
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

        public int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSysElement, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idSysElement, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSysElements] " +
            " WHERE idSysElement = " + idSysElement +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSysElement", System.Data.SqlDbType.Int).Value = tblSysElementsTO.IdSysElement;
            return cmdDelete.ExecuteNonQuery();
        }

        //Harshala
         public  List<TblSysElementsTO> SelectgiveAllTblSysElements()
        {
            
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysElementsTO> list = ConvertDTToList(rdr);
                return list;

            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
            
        }

        //harshala
         public List<tblViewPermissionTO> selectPermissionswrtRole(int roleId,int userId)
        {
             String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if (roleId!= 0 && userId==0)
                {
               
                        cmdSelect.CommandText = SqlSelectModuleQuery()+ " WHERE roleId=" + roleId +" and type='m' ";
                     

                }  
                else
                {
                    cmdSelect.CommandText=SqlSelectModulewrtUserQuery() + " WHERE  type='m' and userId=" + userId + " ";
                }
                

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<tblViewPermissionTO> list = ConvertDTTOList(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
       
        //harshala

        public List<tblViewMenuTO> SelectMenuPermission(int roleId,int userId, int moduleId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if(roleId!=0 && moduleId!=0 && userId==0)
                 {
                 cmdSelect.CommandText = SqlSelectMenuQuery() + " WHERE roleId = " + roleId + " and menu.moduleId = " + moduleId + " and type='MI' ";
                }
                else
                {
                    cmdSelect.CommandText=SqlSelectMenuUserQuery() + " WHERE  menu.moduleId = " + moduleId + " and userId = " + userId + " and type='MI' ";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<tblViewMenuTO> list = ConvertDTTOListMenu(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

         public List<tblViewMenuTO> SelectElementPermission(int roleId,int userId, int moduleId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if(roleId!=0 && moduleId!=0 && userId==0)
                 {
                 cmdSelect.CommandText = SqlSelectElementQuery() + " WHERE  roleelment.roleId = " + roleId + " and pages.moduleId = " + moduleId + " ";
                }
                else
                {
                    cmdSelect.CommandText=SqlSelectElementUserQuery() +  " WHERE  pages.moduleId = " + moduleId + " and userId= " + userId + " ";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<tblViewMenuTO> list = ConvertDTTOListMenu(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectAllPermissionList()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectAllPermissionQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSysElement"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSysElement"].ToString());
                    if (dateReader["menuDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["menuDesc"].ToString());
                    if(dateReader["menuDesc"]==DBNull.Value)
                        dropDownTONew.Text=Convert.ToString(dateReader["elementDesc"].ToString());   
                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

          public List<tblRoleUserTO> SelectAllRolewrtPermission(int idSysElement)
        {
             String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if(idSysElement!=0)
                 {
                 cmdSelect.CommandText = SqlSelectRolewrtPermissionQuery() + " and  idSysElement = " + idSysElement + " ";
                }
                

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<tblRoleUserTO> list = ConvertDTTOListRoleUser(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        
        }

        public List<tblRoleUserTO> SelectAllUserwrtPermission(int idSysElement)
        {
             String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if(idSysElement!=0)
                 {
                 cmdSelect.CommandText = SqlSelectUserwrtPermissionQuery() + " and  idSysElement = " + idSysElement + " ";
                }
                

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<tblRoleUserTO> list = ConvertDTTOListRoleUser(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        
        }


        #endregion
        
    }
}
