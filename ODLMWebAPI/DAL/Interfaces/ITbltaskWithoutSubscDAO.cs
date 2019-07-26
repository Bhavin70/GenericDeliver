using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITbltaskWithoutSubscDAO
    {
        String SqlSelectQuery();
        List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubsc();
        TbltaskWithoutSubscTO SelectTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc);
        List<TbltaskWithoutSubscTO> SelectTbltaskWithoutSubscList(Int32 moduleId, Int32 entityId);
        List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubsc(SqlConnection conn, SqlTransaction tran);
        int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO);
        int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlCommand cmdInsert);
        int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO);
        int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlCommand cmdUpdate);
        int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc);
        int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTaskWithoutSubsc, SqlCommand cmdDelete);
        List<TbltaskWithoutSubscTO> ConvertDTToList(SqlDataReader tbltaskWithoutSubscTODT);

    }
}