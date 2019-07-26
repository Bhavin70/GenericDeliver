using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblGroupDAO
    {
        String SqlSelectQuery();
        List<TblGroupTO> SelectAllTblGroup();
        TblGroupTO SelectTblGroup(Int32 idGroup);
        List<TblGroupTO> SelectAllTblGroup(SqlConnection conn, SqlTransaction tran);
        List<TblGroupTO> ConvertDTToList(SqlDataReader tblGroupTODT);
        List<TblGroupTO> SelectAllGroupList(TblGroupTO tblGroupTO);
        List<TblGroupTO> SelectAllActiveGroupList();
        int InsertTblGroup(TblGroupTO tblGroupTO);
        int InsertTblGroup(TblGroupTO tblGroupTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblGroupTO tblGroupTO, SqlCommand cmdInsert);
        int UpdateTblGroup(TblGroupTO tblGroupTO);
        int UpdateTblGroup(TblGroupTO tblGroupTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblGroupTO tblGroupTO, SqlCommand cmdUpdate);
        int DeleteTblGroup(Int32 idGroup);
        int DeleteTblGroup(Int32 idGroup, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idGroup, SqlCommand cmdDelete);

    }
}