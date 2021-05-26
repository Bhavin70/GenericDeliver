using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ODLMWebAPI.StaticStuff
{
    public class Constants
    {

        #region Common Methods
        /// <summary>
        /// if it is integer and zero then will return DBNull.Value
        /// if it is double and zero then will return DBNull.Value
        /// if it is datetime and is 1/1/1 then will return DBNull.Value
        /// 
        /// </summary>
        /// <param name="cSharpDataValue"></param>
        /// <returns></returns>
        public static object GetSqlDataValueNullForBaseValue(object cSharpDataValue)
        {
            if (cSharpDataValue == null)
            {
                return DBNull.Value;
            }
            else
            {
                if (cSharpDataValue.GetType() == typeof(DateTime))
                {
                    DateTime dt = (DateTime)cSharpDataValue;
                    if (dt.Year == 1 && dt.Month == 1 && dt.Day == 1)
                    {
                        return DBNull.Value;
                    }
                }
                else if (cSharpDataValue.GetType() == typeof(int))
                {
                    int intValue = (int)cSharpDataValue;
                    if (intValue == 0)
                    {
                        return DBNull.Value;
                    }
                }
                else if (cSharpDataValue.GetType() == typeof(Double))
                {
                    Double douValue = (Double)cSharpDataValue;
                    if (douValue == 0)
                    {
                        return DBNull.Value;
                    }
                }
                else if (cSharpDataValue.GetType() == typeof(Int64))
                {
                    Int64 bigIntValue = (Int64)cSharpDataValue;
                    if (bigIntValue == 0)
                    {
                        return DBNull.Value;
                    }
                }
                return cSharpDataValue;
            }
        }
        #endregion
        //vipul[06/05/2019] user subscription active count setting
        public enum UsersSubscriptionActiveCntCalSetting
        {
            ByTab = 1,
            WithOutTab = 2,
        }
        #region Enumerations


        public enum OrgTypeE
        {
            C_AND_F_AGENT = 1,
            DEALER = 2,
            COMPETITOR = 3,
            TRANSPOTER = 4,
            INTERNAL = 5,
            OTHER = 0,
            //Vaibhav 
            //INFLUENCER = 1006,
            PURCHASE_COMPETITOR = 7,
            INFLUENCER = 8,
            PROJECT_ORGANIZATIONS = 9,
            USERS = 15
        }

        public enum UsersConfigration
        {
            USER_CONFIG = 1
        }
        public enum AddressTypeE
        {
            OFFICE_ADDRESS = 1,
            FACTORY_ADDRESS = 2,
            WORKS_ADDRESS = 3,
            GODOWN_ADDRESS = 4,
            //Vijaymla[01-11-2017] added to save organization new address of type office supply address
            OFFICE_SUPPLY_ADDRESS = 5
        }

        public enum TransactionTypeE
        {
            BOOKING = 1,
            LOADING = 2,
            DELIVERY = 3,
            SPECIAL_REQUIREMENT = 4,
            UNLOADING = 5
        }

        //public static DateTime ServerDateTime
        //{
        //    get
        //    {
        //        return DAL.CommonDAO.SelectServerDateTime();
        //    }
        //}

        private static ILogger loggerObj;

        public static ILogger LoggerObj
        {
            get
            {
                return loggerObj;
            }

            set
            {
                loggerObj = value;
            }
        }



        public enum TranStatusE
        {
            BOOKING_NEW = 1,
            BOOKING_APPROVED = 2,
            //BOOKING_APPROVED_DIRECTORS = 3, //Saket [2017-11-10] Commented For SRJ.
            BOOKING_APPROVED_FINANCE = 3,
            LOADING_NEW = 4,
            LOADING_NOT_CONFIRM = 5,
            LOADING_WAITING = 6,
            LOADING_CONFIRM = 7,
            BOOKING_REJECTED_BY_FINANCE = 8,
            BOOKING_APPROVED_BY_MARKETING = 9,
            BOOKING_REJECTED_BY_MARKETING = 10,
            BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR = 11,
            BOOKING_REJECTED_BY_ADMIN_OR_DIRECTOR = 12,
            BOOKING_DELETE = 13,
            LOADING_REPORTED_FOR_LOADING = 14,
            LOADING_GATE_IN = 15,
            LOADING_COMPLETED = 16,
            LOADING_DELIVERED = 17,
            LOADING_CANCEL = 18,
            LOADING_POSTPONED = 19,
            LOADING_VEHICLE_CLERANCE_TO_SEND_IN = 20, // It will be given by Loading Incharge to Security Officer
            //GJ@20170915 : Added the Unloading Status
            UNLOADING_NEW = 21,
            UNLOADING_COMPLETED = 22,
            UNLOADING_CANCELED = 23,
            BOOKING_PENDING_FOR_DIRECTOR_APPROVAL = 24,  //Sanjay [2017-12-19] New Status when Finance Forward Booking to Director Approval.
            BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR = 25,                          //Priyanka [2018-30-07] Added for adding new status in booking.
            INVOICE_GENERATED_AND_READY_FOR_DISPACH = 26,
        }

        public enum LoadingLayerE
        {
            BOTTOM = 1,
            MIDDLE1 = 2,
            MIDDLE2 = 3,
            MIDDLE3 = 4,
            TOP = 5
        }

        /// <summary>
        /// Sanjay [2017-03-06] To Maintain the historical record for any transactional records
        /// </summary>
        public enum TxnOperationTypeE
        {
            NONE = 0,
            OPENING = 1,
            IN = 2,
            OUT = 3,
            UPDATE = 4
        }


        /// <summary>
        /// Sanjay [29 Nov 2018] Commented the Enum as it should not be used for any check.
        /// It should be derived from RoleType Enum with roleids configured in DB. From Org Structure implementations this is
        /// changed as roles become dynamic
        /// </summary>
        //public enum SystemRolesE
        //{
        //    SYSTEM_ADMIN = 1,
        //    DIRECTOR = 2,
        //    C_AND_F_AGENT = 3,
        //    LOADING_PERSON = 4,
        //    MARKETING_FRONTIER = 5,
        //    MARKETING_BACK_OFFICE = 6,
        //    FIELD_OFFICER = 7,
        //    REGIONAL_MANAGER = 8,
        //    VICE_PRESIDENT_MARKETING = 9,
        //    ACCOUNTANT = 10,
        //    SECURITY_OFFICER = 11,
        //    SUPERWISOR = 12,

        //    //Priyanka [07-03-2018]
        //    WEIGHING_OFFICER = 13,
        //    BILLING_OFFICER = 14,
        //    TRANSPORTER = 15
        //}

        public enum SystemRoleTypeE
        {
            SYSTEM_ADMIN = 1,
            DIRECTOR = 2,
            C_AND_F_AGENT = 3,
            LOADING_PERSON = 4,
            MARKETING_FRONTIER = 5,
            MARKETING_BACK_OFFICE = 6,
            FIELD_OFFICER = 7,
            REGIONAL_MANAGER = 8,
            VICE_PRESIDENT_MARKETING = 9,
            ACCOUNTANT = 10,
            SECURITY_OFFICER = 11,
            SUPERWISOR = 12,

            //Priyanka [07-03-2018]
            WEIGHING_OFFICER = 13,
            BILLING_OFFICER = 14,
            TRANSPORTER = 15,
            Dealer = 16
        }

        public enum ProductCategoryE
        {
            NONE = 0,
            TMT = 1,
            PLAIN = 2
        }

        public enum ProductSpecE
        {
            NONE = 0,
            STRAIGHT = 1,
            BEND = 2,
            RK_SHORT = 3,
            RK_LONG = 4,
            TUKADA = 5,
            COIL = 6,
        }

        public enum BookingActionE
        {
            OPEN,
            CLOSE
        }

        public enum CommercialLicenseE
        {
            PAN_NO = 1,
            VAT_NO = 2,
            TIN_NO = 3,
            CST_NO = 4,
            EXCISE_REG_NO = 5,
            SGST_NO = 6,  //Prov GSTIN No
            IGST_NO = 7,  //Permenent GSTIN No
            CGST_NO = 8,
            CIN_NO = 9
        }

        public enum TxnDeliveryAddressTypeE
        {
            BILLING_ADDRESS = 1,
            CONSIGNEE_ADDRESS = 2,
            SHIPPING_ADDRESS = 3
        }

        public enum AddressSourceTypeE
        {
            FROM_BOOKINGS = 1,
            FROM_DEALER = 2,
            FROM_CNF = 3,
            NEW_ADDRESS = 4,
            SELECT_FROM_EXISTING_BOOKINGS = 5       //Priyanka [14-12-2018]
        }

        public enum InvoiceTypeE
        {
            REGULAR_TAX_INVOICE = 1,
            EXPORT_INVOICE = 2,
            DEEMED_EXPORT_INVOICE = 3,
            SEZ_WITH_DUTY = 4,
            SEZ_WITHOUT_DUTY = 5
        }


        public enum InvoiceStatusE
        {
            NEW = 1,
            PENDING_FOR_AUTHORIZATION = 2,
            AUTHORIZED_BY_DIRECTOR = 3,
            REJECTED_BY_DIRECTOR = 4,
            PENDING_FOR_ACCEPTANCE = 5,
            ACCEPTED_BY_DISTRIBUTOR = 6,
            REJECTED_BY_DISTRIBUTOR = 7,
            CANCELLED = 8,
            AUTHORIZED = 9,
        }

        /*GJ@20170913 : Added Enum for Loading Slip Type*/
        public enum LoadingTypeE
        {
            REGULAR = 1,
            OTHER = 2,
        }
        /*GJ@20170913 : Added Enum for Tax Type*/
        public enum TaxTypeE
        {
            IGST = 1,
            CGST = 2,
            SGST = 3,
        }

        /*GJ@20170913 : Added Enum for Invoice Mod Type*/
        public enum InvoiceModeE
        {
            AUTO_INVOICE = 1,
            AUTO_INVOICE_EDIT = 2,
            MANUAL_INVOICE = 3,
        }

        /*GJ@20171007 : Weighing Measure Type*/
        public enum TransMeasureTypeE
        {
            TARE_WEIGHT = 1,
            INTERMEDIATE_WEIGHT = 2,
            GROSS_WEIGHT = 3,
            NET_WEIGHT = 4
        }
        // Vaibhav [18-Sep-2017] Added to department master

        public enum DepartmentTypeE
        {
            DIVISION = 1,
            DEPARTMENT = 2,
            SUB_DEPARTMENT = 3,
            BOM = 4,
        }

        // Vaibhav [7-Oct-2017] Added to visit persons
        public enum VisitPersonE
        {
            SITE_OWNER = 1,
            SITE_ARCHITECT = 2,
            SITE_STRUCTURAL_ENGG = 3,
            SITE_CONTRACTOR = 4,
            SITE_STEEL_PURCHASE_AUTHORITY = 5,
            DEALER = 6,
            DEALER_MEETING_WITH = 7,
            DEALER_VISIT_ALONG_WITH = 8,
            SITE_COMPLAINT_REFRRED_BY = 9,
            COMMUNICATION_WITH_AT_SITE = 10,
            INFLUENCER_VISITED_BY = 11,
            INFLUENCER_RECOMMANDEDED_BY = 12,
            SITE_EXECUTOR = 13,
        }

        // Vaibhav [7-Oct-2017] Added to visit follow up roles
        public enum VisitFollowUpActionE
        {
            SHARE_INFO_TO = 1,
            CALL_BY_SELF_TO = 2,
            ARRANGE_VISIT_OF = 3,
            ARRANGE_VISIT_TO = 4,
            ARRANGE_FOR = 5,
            POSSIBILITY_OF = 6
        }

        // Vaibhav [10-Oct-2017] added to visit issues 
        public enum VisitIssueTypeE
        {
            DELIVERY_ISSUE = 1,
            Quality_ISSUE = 2,
            PRICE_ISSUE = 3,
            ACCOUNT_ISSUE = 4,
            INFLUENCER_ISSUE = 5
        }

        // Vaibhav [23-Oct-2017] added to visit site type
        public enum VisitSiteTypeE
        {
            SITE_TYPE = 1,
            SITE_CATEGORY = 2,
            SITE_SUBCATEGORY = 3
        }

        // Vaibhav [24-Oct-2017] added to visit project type
        public enum VisitProjectTypeE
        {
            KEY_PROJECT = 1,
            CURRENT_PROJECT = 2
        }

        // Vaibhav [27-Oct-2017] added to follow up roles
        public enum VisitFollowUpRoleE
        {
            SHARE_INFO_TO = 1,
            CALL_BY_SELF_TO_WHOM = 2,
            ARRANGE_VISIT_OF = 3,
            ARRANGE_VISIT_TO = 4,
            ARRANGE_VISIT_FOR = 5,
            POSSIBILITY_OF = 6
        }

        /// <summary>
        /// Sanjay [2018-02-19] To Define Item Product Categories
        /// Was Required to distiguish between Finished Good & Scrap
        /// </summary>
        /// <remarks> Enum for Item Product Categories Of The System</remarks>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ItemProdCategoryE
        {
            REGULAR_RM = 1,
            FINISHED_GOODS = 2,
            SEMI_FINISHED_GOODS = 3,
            CAPITAL_GOODS = 4,
            SERVICE_CATG_ITEMS = 5,
            SCRAP_OR_WASTE = 6,

        }

        public enum FirmTypeE
        {
            Proprieter = 1,
            Partnership = 2,
            Company = 3,
        }

        //Priyanka [10-08-2018] : Added for transaction action types.
        public enum TranActionTypeE
        {
            READ = 1,
            DELETE = 2
        }
        //Aniket [30-7-2019] : Added enum for modBusTCP.
        public enum WeighingDataSourceE
        {
            DB = 1,
            IoT = 2,
            BOTH = 3
        }
        public enum ActiveSelectionTypeE
        {
            Both = 1,
            Active = 2,
            NonActive = 3
        }
        #endregion

        #region Constants Or Static Strings
        public static String Local_URL = "http://localhost:4200";
        //Added by Kiran for set current module id as per tblmodule sequence
        public static Int32 DefaultModuleID = 1;
        public static Boolean Local_API = Startup.IsLocalAPI;
        public static String CONNECTION_STRING = "ConnectionString";
        public static String AZURE_CONNECTION_STRING = "AzureConnectionStr";
        public static String REQUEST_ORIGIN_STRING = "RequestOriginString";
        public static String IdentityColumnQuery = "Select @@Identity";
        public static String DefaultCountry = "India";
        public static Int32 DefaultCountryID = 101;
        public static String DefaultDateFormat = "dd-MM-yyyy HH:mm tt";
        public static String AzureDateFormat = "yyyy-MM-dd HH:mm tt";
        public static String DefaultPassword = "123";
        public static String DefaultErrorMsg = "Error - 00 : Record Could Not Be Saved";
        public static String DefaultSuccessMsg = "Success - Record Saved Successfully";

        //Default Currency Id and Rate is Indain
        public static int DefaultCurrencyID = 1;
        public static int DefaultCurrencyRate = 1;

        // Vaibhav [26-Sep-2017] added to set default company id to Bhagyalaxmi Rolling Mills
        public static int DefaultCompanyId = 19;
        public static int DefaultSalutationId = 1;

        // Vaibhav [17-Dec-2017] Added to file encrypt descrypt and upload to azure
        //public static string AzureConnectionStr = "DefaultEndpointsProtocol=https;AccountName=apkupdates;AccountKey=IvC+sc8RLDl3DeH8uZ97A4jX978v78bVFHRQk/qxg2C/J8w/DRslJlLsK7JTF+KhOM0MNUZg443GCVXe3jIanA==";
        //public static string EncryptDescryptKey = "MAKV2SPBNI99212";
        public static string AzureSourceContainerName = "srjdocuments";
        public static string AzureTargetContainerName = "srjnewdocuments";
        public static string ExcelSheetName = "TranData";
        public static string ExcelFileName = "Tran";
        public static int LoadingCountForDataExtraction = 50;
        public static String ENTITY_RANGE_NC_LOADINGSLIP_COUNT = "NC_LOADINGSLIP_COUNT";
        public static int FinYear = 2017;
        public static String ENTITY_RANGE_C_LOADINGSLIP_COUNT = "C_LOADINGSLIP_COUNT";
        public static String ENTITY_RANGE_LOADING_COUNT = "LOADING_COUNT";
        //Pandurang [2018-02-21] Added to take files from Azure 
        public static string AzureDocumentContainerName = "documentation";
        public static string HELP_DOCUMENT_CONFIG = "HELP_DOCUMENT_CONFIG";

        //Sudhir[24-04-2018] Added for CRM Documents.
        //public static string AzureSourceContainerNameForDocument = "kalikadocuments";

        //Sudhir[19-07-2018] Added for CRM Testing Documents.
        //public static string AzureSourceContainerNameForTestingDocument = "testingdocuments";
        public static int PreviousRecordDeletionDays = -2; //Use for Delete Previous 2 Days Records.

        //Hrushikesh added 
        //Permissions 
        public static String ReadWrite = "RW";
        public static String NotApplicable = "NA";

        public static string AzureTemplateContainerName = "invoicetemplates";

        public static string DEFAULT_FETCH_SUCCESS_MSG = "Record Fetch Succesfully";

        public static string DEFAULT_NOTFOUND_MSG = " Record Could not be found";
        public static string CP_POST_SALES_INVOICE_TO_SAP_DIRECTLY_AFTER_INVOICE_GENERATION = "CP_POST_SALES_INVOICE_TO_SAP_DIRECTLY_AFTER_INVOICE_GENERATION";
        public static String SAPB1_SERVICES_ENABLE = "SAPB1_SERVICES_ENABLE";
        
        //Dhananjay [2020-12-02] Parity level
        public static string CP_PARITY_LEVEL = "PARITY_LEVEL";

        public static string SERVER_DATETIME_QUERY_STRING = "SERVER_DATETIME_QUERY_STRING";
        public static string IS_LOCAL_API = "IS_LOCAL_API";
        public static string SAP_LOGIN_DETAILS = "SAP_LOGIN_DETAILS";

        //public static string IS_MAP_MY_INDIA = "IS_MAP_MY_INDIA";
        public static string MAP_MY_INDIA_URL_FOR_myLocationAddress = "MAP_MY_INDIA_URL_FOR_myLocationAddress";
        public static string GOOGLE_MAP_API_URL_FOR_LAT_LONG = "GOOGLE_MAP_API_URL_FOR_LAT_LONG";

        #endregion

        #region Configuration Sections

        public static String IS_MAP_MY_INDIA = "IS_MAP_MY_INDIA";
        public static String MAP_API_URL = "MAP_API_URL";
        public static string CP_MAX_ALLOWED_DEL_PERIOD = "MAX_ALLOWED_DEL_PERIOD";
        public static string LOADING_SLIP_DEFAULT_SIZES = "LOADING_SLIP_DEFAULT_SIZES";
        public static string LOADING_SLIP_DEFAULT_SPECIFICATION = "LOADING_SLIP_DEFAULT_SPECIFICATION";
        public static string LOADING_SLIP_DEFAULT_CATEGORY = "LOADING_SLIP_DEFAULT_CATEGORY";
        public static string SMS_SUBSCRIPTION_ACTIVATION = "SMS_SUBSCRIPTION_ACTIVATION";
        public static string CP_AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION = "AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION";
        public static string CP_SYTEM_ADMIN_USER_ID = "SYTEM_ADMIN_USER_ID";
        public static string CP_COMPETITOR_TO_SHOW_IN_HISTORY = "COMPETITOR_TO_SHOW_IN_HISTORY";
        public static string CP_DELETE_ALERT_BEFORE_DAYS = "DELETE_ALERT_BEFORE_DAYS";
        public static string CP_MIN_AND_MAX_RATE_DEFAULT_VALUES = "MIN_AND_MAX_RATE_DEFAULT_VALUES";
        public static string CP_WEIGHT_TOLERANCE_IN_KGS = "WEIGHT_TOLERANCE_IN_KGS";
        public static string CP_WEIGHING_WEIGHT_TOLERANCE_IN_PERC = "WEIGHING_WEIGHT_TOLERANCE_IN_PERC";
        public static string CP_BOOKING_RATE_MIN_AND_MAX_BAND = "BOOKING_RATE_MIN_AND_MAX_BAND";
        public static string CP_MAX_ALLOWED_CD_STRUCTURE = "MAX_ALLOWED_CD_STRUCTURE";
        public static string CP_LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS = "LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS";
        public static string CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID = "LOADING_SLIPS_AUTO_POSTPONED_STATUS_IDS";
        public static string CP_LOADING_DEFAULT_ALLOWED_UPTO_TIME = "LOADING_DEFAULT_ALLOWED_UPTO_TIME";
        public static string CP_LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS = "LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS";
        public static string CP_DEFAULT_MATE_COMP_ORGID = "DEFAULT_MATE_COMP_ORGID";
        public static string MULTIPLE_TEMPLATE_FOR_PRINTED_INVOICE = "MULTIPLE_TEMPLATE_FOR_PRINTED_INVOICE";
        public static string MULTIPLE_TEMPLATE_FOR_PLAIN_INVOICE = "MULTIPLE_TEMPLATE_FOR_PLAIN_INVOICE";
        public static string CP_DEFAULT_MATE_SUB_COMP_ORGID = "DEFAULT_MATE_SUB_COMP_ORGID";
        public static string CP_APP_CONFIGURATION_AUTHENTICATION = "APP_CONFIGURATION_AUTHENTICATION";
        public static string CP_FRIEGHT_OTHER_TAX_ID = "FRIEGHT_OTHER_TAX_ID";
        public static string CP_REVERSE_CHARGE_MECHANISM = "REVERSE_CHARGE_MECHANISM";
        public static string CP_DEFAULT_WEIGHING_SCALE = "DEFAULT_WEIGHING_SCALE";
        public static string CP_BILLING_NOT_CONFIRM_AUTHENTICATION = "BILLING_NOT_CONFIRM_AUTHENTICATION";
        public static string CONSOLIDATE_STOCK = "CONSOLIDATE_STOCK";
        public static String ENTITY_RANGE_REGULAR_TAX_INVOICE_BMM = "REGULAR_TAX_INVOICE_BMM";
        public static String ENTITY_RANGE_REGULAR_TAX_INTERNALORG = "REGULAR_TAX_INVOICE_ORG_";
        public static String INTERNAL_DEFAULT_ITEM = "INTERNAL_DEFAULT_ITEM";
        public static String INTERNAL_DEFAULT_ITEM_BASE_RATE_DIFF_AMT = "INTERNAL_DEFAULT_ITEM_BASE_RATE_DIFF_AMT";

        public static string CP_BRAND_WISE_INVOICE = "BRAND_WISE_INVOICE";
        public static string CP_SKIP_LOADING_APPROVAL = "SKIP_LOADING_APPROVAL";
        public static string CP_SKIP_WEIGHING = "SKIP_WEIGHING";
        public static string CP_SKIP_INVOICE_APPROVAL = "SKIP_INVOICE_APPROVAL";
        public static string CP_AUTO_MERGE_INVOICE = "AUTO_MERGE_INVOICE";

        public static string CP_INTERNALTXFER_INVOICE_ORG_ID = "INTERNALTXFER_INVOICE_ORG_ID";
        public static string CP_ADD_CNF_AGENT_IN_INVOICE = "ADD_CNF_AGENT_IN_INVOICE";

        public static string CP_EVERY_AUTO_INVOICE_WITH_EDIT = "EVERY_AUTO_INVOICE_WITH_EDIT";

        //Priyanka [2018-04-16] Added
        public static string CP_ALLOW_TO_CANCEL_LOADING_IF_TARE_WT_TAKEN = "ALLOW_TO_CANCEL_LOADING_IF_TARE_WT_TAKEN";

        // Vaibhav [29-Dec-2017] Added to config days to delete previous stock and quotadeclaration
        public static string CP_DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS = "DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS";
        public static string CP_MIGRATE_ENQUIRY_DATA = "MIGRATE_ENQUIRY_DATA";
        public static string CP_MIGRATE_BEFORE_DAYS = "MIGRATE_BEFORE_DAYS";
        //Pandurang[2018-10-03]Added for Delete Dispatch data
        public static string CP_DELETE_BEFORE_DAYS = "MIGRATE_BEFORE_DAYS";
        public static string CP_DATA_EXTRACTION_TYPE = "DATA_EXTRACTION_TYPE";

        // Vijaymala[14-02-2018] Added to Set Current Company
        public static string CP_CURRENT_COMPANY = "CURRENT_COMPANY";
        public static string CP_FIRBASE_ANDROID_NOTIFICATION_SETTINGS = "FIRBASE_ANDROID_NOTIFICATION_SETTINGS";

        // Vijaymala[14-02-2018] Added to Set Display Brand On Invoice
        public static string CP_DISPLAY_BRAND_ON_INVOICE = "DISPLAY_BRAND_ON_INVOICE";
        //chetan[2020-06-08] added
        public static string ITEM_GRAND_TOTAL_ROUNDUP_VALUE = "ITEM_GRAND_TOTAL_ROUNDUP_VALUE";

        public static string CP_SIZEWISE_LOADING_REPORT_STATUS_IDS = "SIZEWISE_LOADING_REPORT_STATUS_IDS";


        // Vaibhav [21-Mar-2018] Added to get authentication server config params
        public static string CP_AUTHENTICATION_URL = "AUTHENTICATION_URL";
        public static string CP_CLIENT_ID = "CLIENT_ID";
        public static string CP_CLIENT_SECRET = "CLIENT_SECRET";
        public static string CP_SCOPE = "SCOPE";

        //Vijaymala [22-March-2018] Added to set Sales Ledger name in report 
        public static string CP_SALES_LEDGER_NAME = "SALES_LEDGER_NAME";

        // Vaibhav [11-April-2018] Added to set invoice date
        public static string CP_TARE_WEIGHT_DATE_AS_INV_DATE = "TARE_WEIGHT_DATE_AS_INV_DATE";
        public static string CP_CREATE_NC_DATA_FILE = "CREATE_NC_DATA_FILE";
        //Saket [2018-06-04] Added if true then invoice auth date as invoice date
        public static string CP_AUTHORIZATION_DATE_AS_INV_DATE = "AUTHORIZATION_DATE_AS_INV_DATE";

        //Vijaymala [02-05-2018] Added to set Dealer name in notification 
        public static string CP_ADD_DEALER_IN_NOTIFICATION = "ADD_DEALER_IN_NOTIFICATION";
        //Vijaymala[15 - 05 - 2018] Added to display discount on sales invoice export
        public static string CP_SHOW_DISCOUNT_ON_SALES_INVOICE_EXPORT = "SHOW_DISCOUNT_ON_SALES_INVOICE_EXPORT";

        //Vijaymala[15 - 05 - 2018] Added to export or print sales  export report
        public static string CP_PRINT_SALES_EXPORT_DIRECT_TO_PRINTER = "PRINT_SALES_EXPORT_DIRECT_TO_PRINTER";

        //Priyanka added [29-05-2018]
        public static string CP_MASTER_SUB_MASTER_BUNDLE_FACILITY = "MASTER_SUB_MASTER_BUNDLE_FACILITY";

        //Priyanka [07-06-2018] : Added for SHIVANGI
        public static string CP_PARTY_WISE_BLOCKING = "PARTY_WISE_BLOCKING";
        public static string CP_ENQUIRY_WISE_BLOCKING = "ENQUIRY_WISE_BLOCKING";
        public static string CP_DISPLAY_MATERIAL_DETAILS_ON_PAGE = "DISPLAY_MATERIAL_DETAILS_ON_PAGE";

        //Vijaymala[19 - 06 - 2018] Added to HIDE RATE BAND DECLARATION
        public static string CP_DISPLAY_RATE_BAND_DECLARATION = "DISPLAY_RATE_BAND_DECLARATION";
        public static string CP_RATE_DECLARATION_FOR_ENQUIRY = "DAILY_RATE_DECLARATION_FOR_ENQUIRY";
        public static string CP_SEND_RATE_SMS_TO_DEALER_REGISTER_MOBILE_NO = "SEND_RATE_SMS_TO_DEALER_REGISTER_MOBILE_NO";
        public static string CP_CD_STRUCTURE_IN_PERCENTAGE = "CD_STRUCTURE_IN_PERCENTAGE";
        public static string CP_CD_STRUCTURE_IN_RS = "CD_STRUCTURE_IN_RS";
        public static string CP_FROM_LOCATION = "FROM_LOCATION";
        public static string CP_EDIT_BOOKING_DETAILS = "EDIT_BOOKING_DETAILS";
        public static string CP_AUTO_GATE_IN_VEHICLE = "AUTO_GATE_IN_VEHICLE";
        public static string CP_DEFAULT_FOR_VALUE = "DEFAULT_FOR_VALUE";
        //Harshkunj[26 - 06 - 2018] Added to set booking end time
        public static string CP_BOOKING_END_TIME = "BOOKING_END_TIME";

        //Priyanka [27-06-2018] Added to set view loading days.
        public static string CP_VIEW_LOADING_SLIP_DAYS = "VIEW_LOADING_SLIP_DAYS";
        public static string CP_DASHBOARD_ENQ_QTY_STATUSES = "DASHBOARD_ENQ_QTY_STATUSES";

        public static string CP_AUTO_FINANCE_APPROVAL_FOR_ENQUIRY = "AUTO_FINANCE_APPROVAL_FOR_ENQUIRY";

        //Sanjay [2017-07-04] Tax Calculations Inclusive Of Taxes Or Exclusive Of Taxes. Reported From Customer Shivangi Rolling Mills
        public static string CP_RATE_CALCULATIONS_TAX_INCLUSIVE = "RATE_CALCULATIONS_TAX_INCLUSIVE";
        public static string CP_RATE_CALCULATIONS_TAX_INCLUSIVE_WITH_ALL_PARAMS_LESS = "RATE_CALCULATIONS_TAX_INCLUSIVE_WITH_ALL_PARAMS_LESS";

        //Priyanka [16-07-2018] Added for SHIVANGI (additional discount, displaying freight, convert freight into required format)  
        public static string CP_IS_ADDITIONAL_DISCOUNT_APPLICABLE = "IS_ADDITIONAL_DISCOUNT_APPLICABLE";
        public static string CP_DISPLAY_FREIGHT_ON_INVOICE = "DISPLAY_FREIGHT_ON_INVOICE";
        public static string CP_MINUS_FREIGHT_FROM_CALCULATION = "MINUS_FREIGHT_FROM_CALCULATION";

        //Priyanka [02-08-2018] Added for show the todays stock on dashboard.
        public static string CP_TODAYS_BOOKING_OPENING_BALANCE = "TODAYS_BOOKING_OPENING_BALANCE";

        //Priyanka [02-08-2018] Added for set the status for view enquiry statistics graphs
        public static string CP_ENQUIRY_STATISTICS_REPORT_STATUS = "ENQUIRY_STATISTICS_REPORT_STATUS";

        //Priyanka [06-09-2018] Added for set the view pending enquiries.(Show all or on date filter)
        public static string CP_VIEW_ALL_PENDING_ENQUIRIES = "VIEW_ALL_PENDING_ENQUIRIES";


        //Saket [2020-14-02] Added.
        public static string CP_GENERATE_INVOICE_NO_FOR_NC = "GENERATE_INVOICE_NO_FOR_NC";
        public static string CP_GENERATE_INVOICE_NO_FOR_NC_DAILY = "GENERATE_INVOICE_NO_FOR_NC_DAILY";


        //Pandurang[2018-09-10] added for Stop web services
        public static string STOP_WEB_API_SERVICE_KEYS = "STOP_WEB_API_SERVICE_KEYS";
        public static string STOP_WEB_GUI_SERVICE_KEYS = "STOP_WEB_GUI_SERVICE_KEYS";

        public static string CP_STATUS_TO_CALCULATE_ENQUIRY_OPENING_BALANCE = "STATUS_TO_CALCULATE_ENQUIRY_OPENING_BALANCE";

        public static string CP_DISPLAY_DEALER_NAME_ON_WEIGHMENT_SLIP = "DISPLAY_DEALER_NAME_ON_WEIGHMENT_SLIP";
        public static string CP_EWAY_NUMBER_COMPULSARY_BEFORE_INVOICE_PRINT = "EWAY_NUMBER_COMPULSARY_BEFORE_INVOICE_PRINT";
        public static string CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION = "AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION";

        public static string CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION_TESTING = "AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION_TESTING";

        public static string CP_TRANSPORTER_MANDATORY_FOR_LOADING = "TRANSPORTER_MANDATORY_FOR_LOADING";
        public static string CP_DEFAULT_TRANSPORTER_SCOPE_FOR_BOOKING = "DEFAULT_TRANSPORTER_SCOPE_FOR_BOOKING";

        //Priyanka [29-10-2018] Added to allow the loading without stock.
        public static string CP_ALLOW_LOADING_WITHOUT_STOCK = "ALLOW_LOADING_WITHOUT_STOCK";

        //Priyanka [14-11-2018] Added to enable or disble the not-confirm option.
        public static string CP_HIDE_NOT_CONFIRM_OPTION = "HIDE_NOT_CONFIRM_OPTION";

        public static string CP_DISPLAY_SHIPPING_ADDRESS = "DISPLAY_SHIPPING_ADDRESS";


        //Priyanka [14-11-2018] Added to enable or disble the number if bundles on Invoice and loading slip.
        public static string CP_HIDE_BUNDLES_ON_INVOICE = "HIDE_BUNDLES_ON_INVOICE";
        public static string CP_HIDE_BUNDLES_ON_LOADING_SLIP = "HIDE_BUNDLES_ON_LOADING_SLIP";

        //Priyanka [19-11-2018] Added to set the default weight tolerance.
        public static string CP_DEFAULT_WEIGHING_TOLERANCE = "DEFAULT_WEIGHING_TOLERANCE";
        public static string CP_STATUS_AFTER_SAVE_BOOKING = "STATUS_AFTER_SAVE_BOOKING";
        public static string CP_VALIDATE_RATEBAND_FOR_BOOKING = "VALIDATE_RATEBAND_FOR_BOOKING";
        public static string CP_STATUS_FOR_SAVE_BOOKING_VALIDATION = "STATUS_FOR_BOOKING_VALIDATION";


        public static string CP_IS_SHIPPING_ADDRESS_VALIDATION_MANDATORY = "IS_SHIPPING_ADDRESS_VALIDATION_MANDATORY";

        public static string CP_DISPLAY_SEZ_FOR_BOOKING = "DISPLAY_SEZ_FOR_BOOKING";

        public static string CP_ROLES_TO_SEND_SMS_ABOUT_RATE_AND_QUOTA = "ROLES_TO_SEND_SMS_ABOUT_RATE_AND_QUOTA";


        public static string CP_IS_INVOICE_RATE_ROUNDED = "IS_INVOICE_RATE_ROUNDED";

        public static string CP_DO_NOT_SHOW_CD_ON_INOVICE = "DO_NOT_SHOW_CD_ON_INOVICE";  //Added for A1

        //Priyanka[17-12-2018]
        public static string CP_HIDE_SOLD_UNSOLD_STOCK = "HIDE_SOLD_UNSOLD_STOCK";

        public static string CP_EXISTING_ADDRESS_COUNT_FOR_BOOKING = "EXISTING_ADDRESS_COUNT_FOR_BOOKING";

        //Priyanka [20-12-2018] : Added to check the setting of sms template including sizes or not
        public static string CP_SMS_TEMPLATE_INCLUDING_SIZE = "SMS_TEMPLATE_INCLUDING_SIZE";

        //Priyanka [25-12-2018] : Added to display or hide the company name & address on weighment receipt
        public static string CP_SHOW_ADDRESS_ON_WEIGHMENT_RECEIPT = "SHOW_ADDRESS_ON_WEIGHMENT_RECEIPT";

        //Priyanka [26-12-2018] : Added to display or hide the final dispatch section on print loading slip.
        public static string CP_SHOW_FINAL_DISPATCH_ON_PRINT_LOADING_SLIP = "SHOW_FINAL_DISPATCH_ON_PRINT_LOADING_SLIP";

        // Aniket K[3-Jan-2019] : Added to set rate declaration history in days.
        public static string QUOTA_RATE_DECLARATION_HISTORY_IN_DAYS = "QUOTA_RATE_DECLARATION_HISTORY_IN_DAYS";

        public static string CP_IS_PRODUCTION_ENVIRONMENT_ACTIVE = "IS_PRODUCTION_ENVIRONMENT_ACTIVE";

        //Vijaymala [09-01-2019] Added 
        public static string CP_DISPLAY_CONSIGNEE_ADDRESS_ON_PRINTABLE_INVOICE = "DISPLAY_CONSIGNEE_ADDRESS_ON_PRINTABLE_INVOICE";

        // Aniket [05-02-2019] added to check brandwise invoice number generate or not
        public static string GENERATE_MANUALLY_BRANDWISE_INVOICENO = "GENERATE_MANUALLY_BRANDWISE_INVOICENO";
        public static string CP_VIEW_COMMERCIAL_DETAILS = "VIEW_COMMERCIAL_DETAILS";
        // Aniket [18-02-2019] added to check Other material Qty display on dashoboard or not
        public static string IS_OTHER_MATERIAL_QTY_HIDE_ON_DASHBOARD = "IS_OTHER_MATERIAL_QTY_HIDE_ON_DASHBOARD";
        //Aniket [27-02-2019] added to check whether allow or restrict CNF beyond booking quota 
        public static string ANNOUNCE_RATE_WITH_RATEBAND_CURRENT_QUOTA = "ANNOUNCE_RATE_WITH_RATEBAND_CURRENT_QUOTA";
        public static string RESTRICT_CNF_BEYOND_BOOKING_QUOTA = "RESTRICT_CNF_BEYOND_BOOKING_QUOTA";

        public static string BOOKING_DUE_DAYS_WITH_START_NOTIFICATON_DAYS = "BOOKING_DUE_DAYS_WITH_START_NOTIFICATON_DAYS";

        public static string AVERAGE_BOOKING_QTY_DAYS_AND_DEV_PERCENT = "AVERAGE_BOOKING_QTY_DAYS_AND_DEV_PERCENT";

        //Aniket [06-03-2019] added to check Math.Round() function to be used in tax calculations or not
        public static string IS_ROUND_OFF_TAX_ON_PRINT_INVOICE = "IS_ROUND_OFF_TAX_ON_PRINT_INVOICE";

        //Aniket [25-3-2019] added to check which statusId booking details exclude from CNC report
        public static string CNF_BOOKING_REPORT_EXCLUDE_STATUSID = "CNF_BOOKING_REPORT_EXCLUDE_STATUSID";

        //Aniket [22-4-2019] added to check whether vehicle suggestion is should show or hide
        public static string IS_HIDE_VEHICLE_LIST_SUGGESTION = "IS_HIDE_VEHICLE_LIST_SUGGESTION";

        public static string SHOW_GST_CODE_UPTO_DIGITS = "SHOW_GST_CODE_UPTO_DIGITS";

        //Aniket [10-6-2019]
        public static string IS_BALAJI_CLIENT = "IS_BALAJI_CLIENT";
        //Aniket [9-9-2019]
        public static string HIDE_BRAND_NAME_ON_NC_INVOICE = "HIDE_BRAND_NAME_ON_NC_INVOICE";
        //Aniket [16-9-2019]
        public static string ROUND_OFF_TAX_INVOICE_VALUES = "ROUND_OFF_TAX_INVOICE_VALUES";
        //Aniket [18-9-2019]
        public static string SHOW_DELIVERY_LOCATION_ON_INVOICE = "SHOW_DELIVERY_LOCATION_ON_INVOICE";
        //@KKM [30-7-2019] added for IOT
        public static string CP_WEIGHING_MEASURE_SOURCE_ID = "WEIGHING_MEASURE_SOURCE_ID";
        public static String REGULAR_BOOKING = "REGULAR_BOOKING";

        public static string MULTIPLE_TEMPLATE_FOR_PRINTED_INVOICE_BY_CONFIRM = "MULTIPLE_TEMPLATE_FOR_PRINTED_INVOICE_BY_CONFIRM";
        public static string MULTIPLE_TEMPLATE_FOR_PLAIN_INVOICE_BY_CONFIRM = "MULTIPLE_TEMPLATE_FOR_PLAIN_INVOICE_BY_CONFIRM";

        public static string IS_SIZE_CHANGE_ALERT_GENERATE = "IS_SIZE_CHANGE_ALERT_GENERATE";

        //Harshala added
        public static string CP_TCS_OTHER_TAX_ID = "CP_TCS_OTHER_TAX_ID";

        public static string DEFAULT_TCS_PERCENT_IF_PAN_PRESENT = "DEFAULT_TCS_PERCENT_IF_PAN_PRESENT";

        public static string DEFAULT_TCS_PERCENT_IF_PAN_NOT_PRESENT = "DEFAULT_TCS_PERCENT_IF_PAN_NOT_PRESENT";

        public static string CP_IS_INCLUDE_TCS_TO_AUTO_INVOICE = "CP_IS_INCLUDE_TCS_TO_AUTO_INVOICE";

        public static string CP_SPLIT_BOOKING_AGAINST_INVOICE = "CP_SPLIT_BOOKING_AGAINST_INVOICE";

        //[2021-02-26] Dhananjay added
        public static string CP_EINVOICE_SHIPPING_ADDRESS = "CP_EINVOICE_SHIPPING_ADDRESS";

        public static string COMMA_SEPARATED_CNF_SHOULD_HAVE_ALL_DEALER = "COMMA_SEPARATED_CNF_SHOULD_HAVE_ALL_DEALER";

        public static string ADD_ITEMWISE_RATE_WHILE_BOOKING = "ADD_ITEMWISE_RATE_WHILE_BOOKING";

        #endregion

        //Harshala Added
        public static string WeighmentSlip = "WeighmentSlip";
        public static string GatePassSlip = "GatePassSlip";

        #region Common functions

        public static Boolean IsNeedToRemoveFromList(string[] sizeList, Int32 materialId)
        {
            for (int i = 0; i < sizeList.Length; i++)
            {
                int sizeId = Convert.ToInt32(sizeList[i]);
                if (sizeId == materialId)
                {
                    return false;
                }
            }

            return true;
        }

        public static Boolean IsDateTime(String value)
        {
            try
            {
                Convert.ToDateTime(value);
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }

        }

        public static Boolean IsInteger(String value)
        {
            try
            {
                Convert.ToInt32(value);
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }

        }

        public static void SetNullValuesToEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        pi.SetValue(myObject, string.Empty);
                    }
                }
            }
        }

        public static DateTime GetStartDateTimeOfYear(DateTime dateTime)
        {
            if (dateTime.Month < 4)
                return GetStartDateTime(new DateTime(dateTime.Year - 1, 4, 1)); //1 Apr
            else
                return GetStartDateTime(new DateTime(dateTime.Year, 4, 1)); //1 Apr
        }

        public static DateTime GetEndDateTimeOfYear(DateTime dateTime)
        {
            if (dateTime.Month > 3)
                return GetEndDateTime(new DateTime(dateTime.Year + 1, 3, 31)); //31 March
            else
                return GetEndDateTime(new DateTime(dateTime.Year, 3, 31)); //31 March

        }

        public static DateTime GetStartDateTime(DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            return new DateTime(year, month, day, 0, 0, 0);
        }

        public static DateTime GetEndDateTime(DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            return new DateTime(year, month, day, 23, 59, 59);
        }

        public static List<string> GetChangedProperties(Object A, Object B)
        {
            if (A.GetType() != B.GetType())
            {
                throw new System.InvalidOperationException("Objects of different Type");
            }
            List<string> changedProperties = ElaborateChangedProperties(A.GetType().GetProperties(), B.GetType().GetProperties(), A, B);
            return changedProperties;
        }


        public static List<string> ElaborateChangedProperties(PropertyInfo[] pA, PropertyInfo[] pB, Object A, Object B)
        {
            List<string> changedProperties = new List<string>();
            foreach (PropertyInfo info in pA)
            {
                object propValueA = info.GetValue(A, null);
                object propValueB = info.GetValue(B, null);
                if (propValueA != null && propValueB != null)
                {
                    if (propValueA.ToString() != propValueB.ToString())
                    {
                        changedProperties.Add(info.Name);
                    }
                }
                else
                {
                    if (propValueA == null && propValueB != null)
                    {
                        changedProperties.Add(info.Name);
                    }
                    else if (propValueA != null && propValueB == null)
                    {
                        changedProperties.Add(info.Name);
                    }
                }
            }
            return changedProperties;
        }


        //Added By Gokul - To Seperate string by comma to find Target string or number
        public static Boolean IsStringContainInfoSeperatedByComma(string availableStr,string target)
        {
            if (!String.IsNullOrEmpty(availableStr))
            {
                string[] strToArray = availableStr.Split(",");
                foreach (string arr in strToArray)
                {
                    if (arr == target)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Vijaymala[31-10-2017]Added To Set Details Type for invoice other details
        /// </summary>

        public enum invoiceOtherDetailsTypeE
        {
            DESCRIPTION = 1,
            TERMSANDCONDITION = 2
        }

        public enum LogoutValueE
        {
            LogoutWithTimer=1,
            DirectLogout=2
        }
        public enum bookingFilterTypeE
        {
            ALL = 0,
            CONFIRMED = 1,
            NOTCONFIRMED = 2

        }

        public enum RouteTypeE
        {
            ACTUAL = 1,
            SUGGESTED = 2
        }
        public enum InvoiceGenerateModeE
        {
            REGULAR = 0,
            DUPLICATE = 1,
            CHANGEFROM = 2
        }



        /// <summary>
        /// Vijaymala[06-02-2018]Added To Set Firm Name
        /// </summary>
        public enum FirmNameE
        {
            BHAGYALAXMI = 1,
            SRJ = 2,
            KALIKA = 3,


        }
        //Sudhir[23-01-2017] Added for PageId for Support Details Entry.
        public enum SupportPageTypE
        {
            BILLING = 16,
            LOADING_SLIP = 5
        }

        public enum ReportingTypeE
        {
            ADMINISTRATIVE = 1,
            TECHNICAL = 2
        }

    //Priyanka [12-03-2018] : Added for Select Type of list in view booking summary list
    public enum SelectTypeE
        {
            DISTRICT = 1,
            STATE = 2,
            CNF = 3,
            DEALER = 4
        }


        public enum OtherTaxTypeE
        {
            PF = 1,
            FREIGHT = 2,
            CESS = 3,
            AFTERCESS = 4
        }

        // Vaibhva [25-April-2018] Added to seperate transaction table
        public enum TranTableType
        {
            TEMP = 1,
            FINAL = 2
        }

        // Vijaymala [22-06-2018] Added
        public enum CdType
        {
            IsPercent = 1,
            IsRs = 0
        }

        // Vijaymala [17-08-2018] Added 
        public enum BookingType
        {
            IsRegular = 1,
            IsOther = 2
        }
        public enum DataExtractionTypeE
        {
            IsRegular = 1,
            IsDelete = 2
        }


        public enum CurrencyE
        {
            INR = 1,
            USD = 2

        }

        public enum ReportE
        {
            NONE = 1,
            EXCEL = 2,
            PDF = 3,
            BOTH = 4,
            PDF_DONT_OPEN = 5,
            EXCEL_DONT_OPEN = 6
        }

        public enum pageElements
        {
            SKIP_BOOKING_APPROVAL = 265
        }

        /// <summary>
        /// Dhananjay [2020-11-19] For EInvoice API
        /// </summary>
        public enum EInvoiceAPIE
        {
            OAUTH_TOKEN = 1,
            EINVOICE_AUTHENTICATE = 2,
            GENERATE_EINVOICE = 3,
            CANCEL_EINVOICE = 4,
            GET_EINVOICE = 5,
            GENERATE_EWAYBILL = 6,
            CANCEL_EWAYBILL = 7
        }

        /// <summary>
        /// Dhananjay [2021-01-02] For EInvoice API
        /// </summary>
        public static string EINVOICE_AUTHENTICATE = "einvoice/Authenticate";
        
        /// <summary>
        /// Dhananjay [2020-11-19] For EInvoice API
        /// </summary>
        public static Int32 secsToBeDeductededFromTokenExpTime = 120;
        ///<summary>
        ///Aditee [04-01-2021] for generate invoice flag for Address Update
        ///</summary>
        public enum EGenerateEInvoiceCreationType
        {
            UPDATE_ONLY_ADDRESS = 1,
            GENERATE_INVOICE_ONLY = 2,
            INVOICE_WITH_EWAY_BILL = 3,
        }

    }
}
