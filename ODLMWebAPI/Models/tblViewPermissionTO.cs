using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static ODLMWebAPI.StaticStuff.Constants;
namespace ODLMWebAPI.Models
{
    public  class tblViewPermissionTO
    {
          #region Declarations
          String moduleDesc;
          String permission;
          Int32 idModule;
         List<tblViewMenuTO> menuTOs;
         List<tblViewMenuTO> elementList;
        List<tblRoleUserTO> roleList;
         List<tblRoleUserTO> userList;
         
         
          
           #endregion

           #region Constructor

           public tblViewPermissionTO()
           {
           }
            #endregion

        #region GetSet

        public String ModuleDesc
        {
            get { return moduleDesc; }
            set { moduleDesc = value; }
        }
        public String Permission
        {
            get { return permission; }
            set { permission = value; }
        }

        public Int32 IdModule
        {
            get { return idModule; }
            set { idModule = value; }
        }

        public List<tblViewMenuTO> MenuTOs { get => menuTOs; set => menuTOs = value; }
        public List<tblViewMenuTO> ElementList { get => elementList; set => elementList = value; }
         public List<tblRoleUserTO> RoleList { get => roleList; set => roleList = value; }
        public List<tblRoleUserTO> UserList { get => userList; set => userList = value; }
      
    }
        #endregion
    

    public class tblViewMenuTO
    {
          #region Declarations
          String menuDesc;
          String permission;
          Int32 idMenu;
         

           #endregion

           #region Constructor

           public tblViewMenuTO()
           {
           }
            #endregion

        #region GetSet

        public String MenuDesc
        {
            get { return menuDesc; }
            set { menuDesc = value; }
        }
        public String Permission
        {
            get { return permission; }
            set { permission = value; }
        }

      
        public Int32 IdMenu
        {
            get { return idMenu; }
            set { idMenu = value; }
        }

      


        #endregion
    }

    public class tblRoleUserTO
    {
        #region Declarations
        String name;
          String permission;
          Int32 sysEleId;
           Int32 id;

            #endregion

           #region Constructor

           public tblRoleUserTO()
           {
           }
            #endregion

        public string Name { get => name; set => name = value; }
        public string Permission { get => permission; set => permission = value; }
        public int SysEleId { get => sysEleId; set => sysEleId = value; }
     
       public int Id { get => id; set => id = value; }
    }
}