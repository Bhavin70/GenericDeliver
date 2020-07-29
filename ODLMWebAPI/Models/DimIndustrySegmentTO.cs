using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimIndustrySegmentTO
    {

        #region Declarations
        Int32 idIndustrySegment;
        String industrySegName;
        Int32 isActive;
        #endregion

        #region Constructor
        public DimIndustrySegmentTO()
        {
        }
        #endregion

        #region GetSet
       
        public int IsActive { get => isActive; set => isActive = value; }
        public int IdIndustrySegment { get => idIndustrySegment; set => idIndustrySegment = value; }
        public string IndustrySegName { get => industrySegName; set => industrySegName = value; }
        #endregion
    }
}

