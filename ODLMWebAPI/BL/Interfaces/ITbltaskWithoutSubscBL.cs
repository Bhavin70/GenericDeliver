using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITbltaskWithoutSubscBL
    {
        List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubsc();
        List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubscList();
        List<TbltaskWithoutSubscTO> SelectTbltaskWithoutSubscList(Int32 moduleId, Int32 entityId);
        TbltaskWithoutSubscTO SelectTbltaskWithoutSubscTO(Int32 idTaskWithoutSubsc);
        int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO);
        int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO);
        int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc);
        int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc, SqlConnection conn, SqlTransaction tran);
    }
}