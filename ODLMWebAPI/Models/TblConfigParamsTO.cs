using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblConfigParamsTO
    {
        #region Declarations
        Int32 idConfigParam;
        Int32 configParamValType;
        DateTime createdOn;
        String configParamName;
        String configParamVal;
         Int32 moduleId;

		////Hrishikesh[27 - 03 - 2018] Added 
		Int32 isActive;
		String configParamDisplayVal;
		String booleanFlag;
		Boolean slider;
        Int32 categoryType;
        #endregion

        #region Constructor
        public TblConfigParamsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdConfigParam
        {
            get { return idConfigParam; }
            set { idConfigParam = value; }
        }
        public Int32 ConfigParamValType
        {
            get { return configParamValType; }
            set { configParamValType = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String ConfigParamName
        {
            get { return configParamName; }
            set { configParamName = value; }
        }
        public String ConfigParamVal
        {
            get { return configParamVal; }
            set { configParamVal = value; }
        }

		public int IsActive
		{
			get { return isActive; }
			set { isActive = value; }
		}

		public String ConfigParamDisplayVal
		{
			get { return configParamDisplayVal; }
			set { configParamDisplayVal = value; }
		}
		public String BooleanFlag
		{
			get { return booleanFlag; }
			set {booleanFlag = value; }
		}
		public Boolean Slider
		{
			get { return slider; }
			set { slider = value; }
		}

        public int ModuleId { get => moduleId; set => moduleId = value; }

        public Int32 CategoryType
        {
            get { return categoryType; }
            set { categoryType = value; }
        }
        #endregion
    }

    public class realTimeDatabaseTO
    {
        String apiKey;
        String authDomain;
        String databaseURL;
        String projectId;
        String storageBucket;
        String messagingSenderId;

        public string ApiKey { get => apiKey; set => apiKey = value; }
        public string AuthDomain { get => authDomain; set => authDomain = value; }
        public string DatabaseURL { get => databaseURL; set => databaseURL = value; }
        public string ProjectId { get => projectId; set => projectId = value; }
        public string StorageBucket { get => storageBucket; set => storageBucket = value; }
        public string MessagingSenderId { get => messagingSenderId; set => messagingSenderId = value; }
    }
}
