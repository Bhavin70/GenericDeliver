using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimIndustrySegmentTypeTO
    {
        #region Declarations
        Int32 idIndustrySegType;
        String typeName;
        Int32 industrySegmentId;
        Int32 isActive;
        #endregion

        #region Constructor
        public DimIndustrySegmentTypeTO()
        {
        }
        #endregion

        #region GetSet

        public int IsActive { get => isActive; set => isActive = value; }
        public string TypeName { get => typeName; set => typeName = value; }
        public int IndustrySegmentId { get => industrySegmentId; set => industrySegmentId = value; }
        public int IdIndustrySegType { get => idIndustrySegType; set => idIndustrySegType = value; }

        #endregion
    }
}
