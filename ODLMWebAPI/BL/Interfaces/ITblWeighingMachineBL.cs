using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblWeighingMachineBL
    {
        List<TblWeighingMachineTO> SelectAllTblWeighingMachineList();
        List<DropDownTO> SelectTblWeighingMachineDropDownList();
        TblWeighingMachineTO SelectTblWeighingMachineTO(Int32 idWeighingMachine);
        int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO);
        int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO);
        int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblWeighingMachine(Int32 idWeighingMachine);
        int DeleteTblWeighingMachine(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran);
    }
}