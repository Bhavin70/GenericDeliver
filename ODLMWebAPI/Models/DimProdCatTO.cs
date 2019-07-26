using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimProdCatTO
    {
        #region Declarations
        Int32 idProdCat;
        Int32 isActive;
        String prodCateDesc;
        //Aniket [15-7-2019]
        String materialIdStr;
        String specificationIdStr;
        #endregion

        #region Constructor
        public DimProdCatTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProdCat
        {
            get { return idProdCat; }
            set { idProdCat = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String ProdCateDesc
        {
            get { return prodCateDesc; }
            set { prodCateDesc = value; }
        }

        public string MaterialIdStr { get => materialIdStr; set => materialIdStr = value; }
        public string SpecificationIdStr { get => specificationIdStr; set => specificationIdStr = value; }
        #endregion
    }
}
