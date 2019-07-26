using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOtherTaxesBL
    {
        List<TblOtherTaxesTO> SelectAllTblOtherTaxesList();
        TblOtherTaxesTO SelectTblOtherTaxesTO(Int32 idOtherTax);
        TblOtherTaxesTO SelectTblOtherTaxesTO(Int32 idOtherTax, SqlConnection conn, SqlTransaction tran);
        int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO);
        int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO);
        int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOtherTaxes(Int32 idOtherTax);
        int DeleteTblOtherTaxes(Int32 idOtherTax, SqlConnection conn, SqlTransaction tran);
    }
}