using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblOtherDesignationsDAO
    {
        String SqlSelectQuery();
        List<TblOtherDesignationsTO> SelectAllTblOtherDesignations();
        TblOtherDesignationsTO SelectTblOtherDesignations(Int32 idOtherDesignation);
        List<TblOtherDesignationsTO> SelectAllTblOtherDesignations(SqlConnection conn, SqlTransaction tran);
        int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO);
        int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOtherDesignationsTO tblOtherDesignationsTO, SqlCommand cmdInsert);
        int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO);
        int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOtherDesignationsTO tblOtherDesignationsTO, SqlCommand cmdUpdate);
        int DeleteTblOtherDesignations(Int32 idOtherDesignation);
        int DeleteTblOtherDesignations(Int32 idOtherDesignation, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOtherDesignation, SqlCommand cmdDelete);
        List<TblOtherDesignationsTO> ConvertDTToList(SqlDataReader tblOtherDesignationsTODT);

    }
}