using System;

namespace ODLMWebAPI.Models
{
    public class DimPageElementTypesTO
    {
        #region Declarations
        Int32 idPageEleType;
        String pageEleTypeName;
        String pageEleTypeDesc;
        #endregion

        #region Constructor
        public DimPageElementTypesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPageEleType
        {
            get { return idPageEleType; }
            set { idPageEleType = value; }
        }
        public String PageEleTypeName
        {
            get { return pageEleTypeName; }
            set { pageEleTypeName = value; }
        }
        public String PageEleTypeDesc
        {
            get { return pageEleTypeDesc; }
            set { pageEleTypeDesc = value; }
        }
        #endregion
    }
}
