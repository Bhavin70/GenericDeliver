using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblWeighingMachineTO
    {
        #region Declarations
        Int32 idWeighingMachine;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double weighingCapMT;
        String machineName;
        String codeNumber;
        String machineDesc;
        String location;
        String deviceId;
        String machineIP;
        //Aniket [30-7-2019] added for IOT
        string portNumber;
        string ioTUrl;
        int isActive;
        int layerId;
        int weightMeasurTypeId;
        int idWeightMeasure;
        String userIds;
        String altPortNo;
        String altMachineIP;
        Int32 moduleId;

        #endregion

        #region Constructor
        public TblWeighingMachineTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdWeighingMachine
        {
            get { return idWeighingMachine; }
            set { idWeighingMachine = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Double WeighingCapMT
        {
            get { return weighingCapMT; }
            set { weighingCapMT = value; }
        }
        public String MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }
        public String CodeNumber
        {
            get { return codeNumber; }
            set { codeNumber = value; }
        }
        public String MachineDesc
        {
            get { return machineDesc; }
            set { machineDesc = value; }
        }
        public String Location
        {
            get { return location; }
            set { location = value; }
        }
        public String DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }
        public String MachineIP
        {
            get { return machineIP; }
            set { machineIP = value; }
        }

        public int ModuleId { get => moduleId; set => moduleId = value; }
        public string PortNumber { get => portNumber; set => portNumber = value; }
        public string IoTUrl { get => ioTUrl; set => ioTUrl = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public int LayerId { get => layerId; set => layerId = value; }
        public int WeightMeasurTypeId { get => weightMeasurTypeId; set => weightMeasurTypeId = value; }
        public int IdWeightMeasure { get => idWeightMeasure; set => idWeightMeasure = value; }
        public string UserIds { get => userIds; set => userIds = value; }
        public String AltPortNo
        {
            get { return altPortNo; }
            set { altPortNo = value; }
        }
        public String AltMachineIP
        {
            get { return altMachineIP; }
            set { altMachineIP = value; }
        }

        #endregion
    }
}
