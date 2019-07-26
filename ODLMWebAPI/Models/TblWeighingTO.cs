using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblWeighingTO
    {
        #region Declarations
        Int32 idWeighing;
        DateTime timeStamp;
        String measurement;
        String machineIp;
        #endregion

        #region Constructor
        public TblWeighingTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdWeighing
        {
            get { return idWeighing; }
            set { idWeighing = value; }
        }
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
        public String Measurement
        {
            get { return measurement; }
            set { measurement = value; }
        }
        public String MachineIp
        {
            get { return machineIp; }
            set { machineIp = value; }
        }
        #endregion
    }
}
