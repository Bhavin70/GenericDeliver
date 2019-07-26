using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOtherDesignationsBL
    {
        List<TblOtherDesignationsTO> SelectAllTblOtherDesignations();
        List<TblOtherDesignationsTO> SelectAllTblOtherDesignationsList();
        TblOtherDesignationsTO SelectTblOtherDesignationsTO(Int32 idOtherDesignation);
        int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO);
        int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO);
        int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOtherDesignations(Int32 idOtherDesignation);
        int DeleteTblOtherDesignations(Int32 idOtherDesignation, SqlConnection conn, SqlTransaction tran);
    }
}