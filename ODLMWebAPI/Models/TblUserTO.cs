using ODLMWebAPI.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblUserTO
    {
        #region Declarations
        Int32 idUser;
        Int32 isActive;
        String userLogin;
        String userPasswd;
        List<TblUserRoleTO> userRoleList;
        Int32 personId;
        Int32 addressId;
        Int32 organizationId;
        String organizationName;
        TblLoginTO loginTO;
        String userDisplayName;
        Dictionary<int, String> sysEleAccessDCT;
        List<TblMenuStructureTO> menuStructureTOList;
        String registeredDeviceId;
        Int32 isSpecialCnf;
        TblPersonTO userPersonTO;
        TblUserExtTO userExtTO;
        DateTime deactivatedOn;
        Int32 deactivatedBy;
        String imeiNumber;
        String deviceId;
        Int32 firmNameE;
 //Added By Vipul User Tracking [15/03/2019]
        TblModuleCommHisTO tblModuleCommHisTO;
        //End 
        List<TblModuleTO> moduleTOList;
        AuthenticationTO authorizationTO;
        Int32 orgStructId;
        Int32 levelId;
        Int32 reportingTo;
        Int32 userReportingId;
        Int32 doucmentId;
        String path;
Boolean isSetDeviceId;
        #endregion

        #region Constructor
        public TblUserTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String UserLogin
        {
            get { return userLogin; }
            set { userLogin = value; }
        }
        public String UserPasswd
        {
            get { return userPasswd; }
            set { userPasswd = value; }
        }

        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }
        public Int32 AddressId
        {
            get { return addressId; }
            set { addressId = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public String OrganizationName
        {
            get { return organizationName; }
            set { organizationName = value; }
        }
        public List<TblUserRoleTO> UserRoleList
        {
            get
            {
                return userRoleList;
            }

            set
            {
                userRoleList = value;
            }
        }

        public TblLoginTO LoginTO
        {
            get
            {
                return loginTO;
            }

            set
            {
                loginTO = value;
            }
        }

        public string UserDisplayName
        {
            get
            {
                return userDisplayName;
            }

            set
            {
                userDisplayName = value;
            }
        }

        public Dictionary<int, string> SysEleAccessDCT
        {
            get
            {
                return sysEleAccessDCT;
            }

            set
            {
                sysEleAccessDCT = value;
            }
        }

        public List<TblMenuStructureTO> MenuStructureTOList { get => menuStructureTOList; set => menuStructureTOList = value; }
        public string RegisteredDeviceId { get => registeredDeviceId; set => registeredDeviceId = value; }
        public int IsSpecialCnf { get => isSpecialCnf; set => isSpecialCnf = value; }
        public TblPersonTO UserPersonTO { get => userPersonTO; set => userPersonTO = value; }
        public TblUserExtTO UserExtTO { get => userExtTO; set => userExtTO = value; }
        public DateTime DeactivatedOn { get => deactivatedOn; set => deactivatedOn = value; }
        public int DeactivatedBy { get => deactivatedBy; set => deactivatedBy = value; }
        public string ImeiNumber { get => imeiNumber; set => imeiNumber = value; }
        public int FirmNameE { get => firmNameE; set => firmNameE = value; }
 public bool IsSetDeviceId { get => isSetDeviceId; set => isSetDeviceId = value; }
        public List<TblModuleTO> ModuleTOList
        {
            get{ return moduleTOList; }
            set { moduleTOList = value; }
        }

        public AuthenticationTO AuthorizationTO
        {
            get { return authorizationTO; }
            set { authorizationTO = value; }
        }
        public int OrgStructId { get => orgStructId; set => orgStructId = value; }
        public int LevelId { get => levelId; set => levelId = value; }
        public int ReportingTo { get => reportingTo; set => reportingTo = value; }
        public int UserReportingId { get => userReportingId; set => userReportingId = value; }
        public string Path { get => path; set => path = value; }
        public int DoucmentId { get => doucmentId; set => doucmentId = value; }
        public string DeviceId { get => deviceId; set => deviceId = value; }
        public TblModuleCommHisTO TblModuleCommHisTO { get => tblModuleCommHisTO; set => tblModuleCommHisTO = value; }

        #endregion
    }
}
