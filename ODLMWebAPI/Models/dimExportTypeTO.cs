using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimExportTypeTO
    {
        #region Declarations
        Int32 idExportType;
        String exportTypeName;
        Int32 isActive;
        #endregion

        #region Constructor
        public DimExportTypeTO()
        {
        }
        #endregion

        #region GetSet
        public int IdExportType { get => idExportType; set => idExportType = value; }
        public string ExportTypeName { get => exportTypeName; set => exportTypeName = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        #endregion
    }
}
