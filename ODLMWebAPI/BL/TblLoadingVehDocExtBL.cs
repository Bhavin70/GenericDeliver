using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{  
    public class TblLoadingVehDocExtBL : ITblLoadingVehDocExtBL
    {
        private readonly ITblLoadingVehDocExtDAO _iTblLoadingVehDocExtDAO;
        private readonly IDimVehDocTypeDAO _iDimVehDocTypeDAO;
        public TblLoadingVehDocExtBL(ITblLoadingVehDocExtDAO iTblLoadingVehDocExtDAO, IDimVehDocTypeDAO iDimVehDocTypeDAO)
        {
            _iTblLoadingVehDocExtDAO = iTblLoadingVehDocExtDAO;
            _iDimVehDocTypeDAO = iDimVehDocTypeDAO;
        }
        #region Selection
        public List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExt()
        {
            return _iTblLoadingVehDocExtDAO.SelectAllTblLoadingVehDocExt();
        }

        public List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExtList()
        {
            return _iTblLoadingVehDocExtDAO.SelectAllTblLoadingVehDocExt();
        }

        public List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExtListEmptyAgainstLLoading(Int32 loadingId, Int32 getEmptyList)
        {
            List<TblLoadingVehDocExtTO> tblLoadingVehDocExtTOList = SelectAllTblLoadingVehDocExtList(loadingId, 1);
            if (tblLoadingVehDocExtTOList != null && tblLoadingVehDocExtTOList.Count > 0)
            {
                tblLoadingVehDocExtTOList = tblLoadingVehDocExtTOList.OrderBy(w => w.SequenceNo).ToList();
            }
            if (getEmptyList == 1)
            {
                List<DimVehDocTypeTO> dimVehDocTypeTOList = _iDimVehDocTypeDAO.SelectAllDimVehDocType();

                if (dimVehDocTypeTOList != null && dimVehDocTypeTOList.Count > 0)
                {
                    dimVehDocTypeTOList = dimVehDocTypeTOList.Where(w => w.IsActive == 1).ToList();
                    dimVehDocTypeTOList = dimVehDocTypeTOList.OrderBy(o => o.SequenceNo).ToList();

                    for (int i = 0; i < dimVehDocTypeTOList.Count; i++)
                    {
                        DimVehDocTypeTO dimVehDocTypeTO = dimVehDocTypeTOList[i];
                        TblLoadingVehDocExtTO tblLoadingVehDocExtTO = tblLoadingVehDocExtTOList.Where(w => w.VehDocTypeId == dimVehDocTypeTO.IdVehDocType).FirstOrDefault();
                        if (tblLoadingVehDocExtTO == null)
                        {
                            tblLoadingVehDocExtTO = new TblLoadingVehDocExtTO();
                            tblLoadingVehDocExtTO.VehDocTypeName = dimVehDocTypeTO.VehDocTypeName;
                            tblLoadingVehDocExtTO.LoadingId = loadingId;
                            tblLoadingVehDocExtTO.VehDocTypeId = dimVehDocTypeTO.IdVehDocType;

                            tblLoadingVehDocExtTOList.Add(tblLoadingVehDocExtTO);
                        }
                    }
                }
            }
            return tblLoadingVehDocExtTOList;
        }

        /// <summary>
        /// Saket [2018-02-21] Added.
        /// </summary>
        /// <param name="loadingId"></param>
        /// <param name="ActiveYnAll"></param>
        /// <returns></returns>
        public List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExtList(Int32 loadingId, Int32 ActiveYnAll)
        {
            return _iTblLoadingVehDocExtDAO.SelectAllTblLoadingVehDocExtList(loadingId, ActiveYnAll);
        }

        public TblLoadingVehDocExtTO SelectTblLoadingVehDocExtTO(Int32 idLoadingVehDocExt)
        {
            return _iTblLoadingVehDocExtDAO.SelectTblLoadingVehDocExt(idLoadingVehDocExt);
        }

        #endregion
        
        #region Insertion
        public int InsertTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO)
        {
            return _iTblLoadingVehDocExtDAO.InsertTblLoadingVehDocExt(tblLoadingVehDocExtTO);
        }

        public int InsertTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingVehDocExtDAO.InsertTblLoadingVehDocExt(tblLoadingVehDocExtTO, conn, tran);
        }


        public int InsertTblLoadingVehDocExt(List<TblLoadingVehDocExtTO> tblLoadingVehDocExtTOList, SqlConnection conn, SqlTransaction tran)
        {
            Int32 result = 0;

            if (tblLoadingVehDocExtTOList != null && tblLoadingVehDocExtTOList.Count > 0)
            {
                for (int i = 0; i < tblLoadingVehDocExtTOList.Count; i++)
                {
                    TblLoadingVehDocExtTO tblLoadingVehDocExtTO = tblLoadingVehDocExtTOList[i];
                    result = InsertTblLoadingVehDocExt(tblLoadingVehDocExtTO, conn, tran);
                    if (result != 1)
                    {
                        return result;
                    }
                }
            }

            return 1;
        }

        #endregion

        #region Updation
        public int UpdateTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO)
        {
            return _iTblLoadingVehDocExtDAO.UpdateTblLoadingVehDocExt(tblLoadingVehDocExtTO);
        }

        public int UpdateTblLoadingVehDocExtActiveYn(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingVehDocExtDAO.UpdateTblLoadingVehDocExtActiveYn(tblLoadingVehDocExtTO, conn, tran);
        }

        public int UpdateTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingVehDocExtDAO.UpdateTblLoadingVehDocExt(tblLoadingVehDocExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingVehDocExt(Int32 idLoadingVehDocExt)
        {
            return _iTblLoadingVehDocExtDAO.DeleteTblLoadingVehDocExt(idLoadingVehDocExt);
        }

        public int DeleteTblLoadingVehDocExt(Int32 idLoadingVehDocExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingVehDocExtDAO.DeleteTblLoadingVehDocExt(idLoadingVehDocExt, conn, tran);
        }

        #endregion
        
    }
}
