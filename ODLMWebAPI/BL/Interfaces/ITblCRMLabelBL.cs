using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
   public interface ITblCRMLabelBL
    {
        


        List<TblCRMLabelTO> SelectAllTblCRMLabelList(int pageId, int langId);

         TblCRMLabelTO SelectTblCRMLabelTO(Int32 idLabel);

    }
}
