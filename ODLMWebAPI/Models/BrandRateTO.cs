using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class BrandRateTO
    {
        #region Declaration

        int brandId;
        string brandName;
        double rate;



        #endregion

        #region GetSet

        public int BrandId { get => brandId; set => brandId = value; }
        public string BrandName { get => brandName; set => brandName = value; }
        public double Rate { get => rate; set => rate = value; }

        #endregion

    }

    public class BrandRateDtlTO
    {
        #region Declaration

        int brandId;
        string brandName;
        double rate;
        double rateBand;
        Int32 validUpto;
        private Double balanceQty;
        private Double lastAllocQty;
        private int quotaDeclarationId;

        #endregion

        #region GetSet

        public int BrandId { get => brandId; set => brandId = value; }
        public string BrandName { get => brandName; set => brandName = value; }
        public double Rate { get => rate; set => rate = value; }
        public double RateBand { get => rateBand; set => rateBand = value; }
        public double LastAllocQty { get => lastAllocQty; set => lastAllocQty = value; }
        public double BalanceQty { get => balanceQty; set => balanceQty = value; }
        public int ValidUpto { get => validUpto; set => validUpto = value; }
        public int QuotaDeclarationId { get => quotaDeclarationId; set => quotaDeclarationId = value; }

        #endregion

    }
}
