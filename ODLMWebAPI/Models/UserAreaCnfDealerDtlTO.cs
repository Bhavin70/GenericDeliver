using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class UserAreaCnfDealerDtlTO
    {
        #region Declaration
        Int32 userId;
        Int32 cnfOrgId;
        Int32 districtId;
        Int32 dealerOrgId;
        Int32 isActive;
        String userName;
        String districtName;
        String cnfOrgName;
        String dealerOrgName;
        Int32 brandId;
        String brandName;
        #endregion

        #region Get Set

        public int UserId { get => userId; set => userId = value; }
        public int CnfOrgId { get => cnfOrgId; set => cnfOrgId = value; }
        public int DistrictId { get => districtId; set => districtId = value; }
        public int DealerOrgId { get => dealerOrgId; set => dealerOrgId = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public string UserName { get => userName; set => userName = value; }
        public string DistrictName { get => districtName; set => districtName = value; }
        public string CnfOrgName { get => cnfOrgName; set => cnfOrgName = value; }
        public string DealerOrgName { get => dealerOrgName; set => dealerOrgName = value; }
        public int BrandId { get => brandId; set => brandId = value; }
        public string BrandName { get => brandName; set => brandName = value; }

        #endregion
    }
}
