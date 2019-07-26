using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblCRMLabelBL : ITblCRMLabelBL
    {
        private readonly ITblCRMLabelDAO _iTblCRMLabelDAO;
        public TblCRMLabelBL(ITblCRMLabelDAO iTblCRMLabelDAO) {
            _iTblCRMLabelDAO = iTblCRMLabelDAO;
        }

        #region Selection


        public  List<TblCRMLabelTO> SelectAllTblCRMLabelList(int pageId,int langId)
        {
            return _iTblCRMLabelDAO.SelectAllTblCRMLabelList(pageId,langId);
            
        }

        public  TblCRMLabelTO SelectTblCRMLabelTO(Int32 idLabel)
        {
           return _iTblCRMLabelDAO.SelectTblCRMLabel(idLabel);
            
        }

        
        #endregion
        
        //#region Insertion
        //public static int InsertTblCRMLabel(TblCRMLabelTO tblCRMLabelTO)
        //{
        //    return TblCRMLabelDAO.InsertTblCRMLabel(tblCRMLabelTO);
        //}

        //public static int InsertTblCRMLabel(TblCRMLabelTO tblCRMLabelTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblCRMLabelDAO.InsertTblCRMLabel(tblCRMLabelTO, conn, tran);
        //}

        //#endregion
        
        //#region Updation
        //public static int UpdateTblCRMLabel(TblCRMLabelTO tblCRMLabelTO)
        //{
        //    return TblCRMLabelDAO.UpdateTblCRMLabel(tblCRMLabelTO);
        //}

        //public static int UpdateTblCRMLabel(TblCRMLabelTO tblCRMLabelTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblCRMLabelDAO.UpdateTblCRMLabel(tblCRMLabelTO, conn, tran);
        //}

        //#endregion
        
        //#region Deletion
        //public static int DeleteTblCRMLabel(Int32 idLabel)
        //{
        //    return TblCRMLabelDAO.DeleteTblCRMLabel(idLabel);
        //}

        //public static int DeleteTblCRMLabel(Int32 idLabel, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblCRMLabelDAO.DeleteTblCRMLabel(idLabel, conn, tran);
        //}

        //#endregion
        
    }
}
