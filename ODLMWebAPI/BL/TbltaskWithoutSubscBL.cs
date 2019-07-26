using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TbltaskWithoutSubscBL : ITbltaskWithoutSubscBL
    {
        private readonly ITbltaskWithoutSubscDAO _iTbltaskWithoutSubscDAO;
        public TbltaskWithoutSubscBL(ITbltaskWithoutSubscDAO iTbltaskWithoutSubscDAO)
        {
            _iTbltaskWithoutSubscDAO = iTbltaskWithoutSubscDAO;
        }
        #region Selection
        public List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubsc()
        {
            return _iTbltaskWithoutSubscDAO.SelectAllTbltaskWithoutSubsc();
        }

        public List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubscList()
        {
            return _iTbltaskWithoutSubscDAO.SelectAllTbltaskWithoutSubsc();
        }
        /// <summary>
        /// Sudhir[30-AUG-2018]
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public List<TbltaskWithoutSubscTO> SelectTbltaskWithoutSubscList(Int32 moduleId, Int32 entityId)
        {
            return _iTbltaskWithoutSubscDAO.SelectTbltaskWithoutSubscList(moduleId, entityId);
        }

        public TbltaskWithoutSubscTO SelectTbltaskWithoutSubscTO(Int32 idTaskWithoutSubsc)
        {
            TbltaskWithoutSubscTO tbltaskWithoutSubscTODT = _iTbltaskWithoutSubscDAO.SelectTbltaskWithoutSubsc(idTaskWithoutSubsc);
            if (tbltaskWithoutSubscTODT != null)
                return tbltaskWithoutSubscTODT;
            else
                return null;
        }
        #endregion

        #region Insertion
        public int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO)
        {
            return _iTbltaskWithoutSubscDAO.InsertTbltaskWithoutSubsc(tbltaskWithoutSubscTO);
        }

        public int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTbltaskWithoutSubscDAO.InsertTbltaskWithoutSubsc(tbltaskWithoutSubscTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO)
        {
            return _iTbltaskWithoutSubscDAO.UpdateTbltaskWithoutSubsc(tbltaskWithoutSubscTO);
        }

        public int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTbltaskWithoutSubscDAO.UpdateTbltaskWithoutSubsc(tbltaskWithoutSubscTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc)
        {
            return _iTbltaskWithoutSubscDAO.DeleteTbltaskWithoutSubsc(idTaskWithoutSubsc);
        }

        public int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc, SqlConnection conn, SqlTransaction tran)
        {
            return _iTbltaskWithoutSubscDAO.DeleteTbltaskWithoutSubsc(idTaskWithoutSubsc, conn, tran);
        }

        #endregion
    }
}
