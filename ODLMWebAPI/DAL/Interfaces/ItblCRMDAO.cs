using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblCRMLabelDAO
    {
        #region Methods
        String SqlSelectQuery();

        #endregion


        List<TblCRMLabelTO> SelectAllTblCRMLabelList(int pageId, int langId);


        TblCRMLabelTO SelectTblCRMLabel(Int32 idLabel);


        List<TblCRMLabelTO> ConvertDTToList(SqlDataReader tblCRMLabelTODT);

    }
}