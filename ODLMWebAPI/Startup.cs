using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Diagnostics;
using System.IO;
using Serilog;
using Serilog.Filters;
using ODLMWebAPI.Controllers;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.AccessTokenValidation;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.BL;
using ODLMWebAPI.DAL;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Authentication;
using ODLMWebAPI.IoT;
using ODLMWebAPI.IoT.Interfaces;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI
{
    public class Startup
    {
        //private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        //public ITblConfigParamsDAO _iTblConfigParamsDAO { get; }
        public SAPLoginDetails sapLogindtls;
        //public Startup(ITblConfigParamsDAO iTblConfigParamsDAO)
        //{
        //    _iTblConfigParamsDAO = iTblConfigParamsDAO;
        //}
        public static string ConnectionString { get; set; }
        public static string RequestOriginString { get; set; }
        public static string AzureConnectionStr { get; set; }
        public static string NewConnectionString { get; private set; }

        public static JObject ConnectionJsonFile { get; private set; }
        public static string DeliverUrl { get; private set; }

        //Aniket [30-7-2019] added for IOT
        public static Int32 WeighingSrcConfig { get; private set; }

        public static string IoTBackUpConnectionString { get; private set; }
        //public static List<int> AvailableModbusRefList { get; set; }

        public static string GateIotApiURL { get; set; }

        public static String SapConnectivityErrorCode { get; private set; }
        public static SAPbobsCOM.Company CompanyObject { get; private set; }
        public static string StockUrl { get; private set; }

        public static string SERVER_DATETIME_QUERY_STRING { get; private set; }
        public static Boolean IsLocalAPI { get; private set; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //_iTblConfigParamsDAO = iTblConfigParamsDAO;

            //Sanjay[2017-02-11] For Logging Configuration

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.RollingFile("../logs/error_log-{Date}.txt")
            .WriteTo.Logger(l => l
            .MinimumLevel.Warning()
            .WriteTo.RollingFile("../logs/warling_log-{Date}.log")).CreateLogger();

                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.RollingFile("../logs/log-{Date}.txt").CreateLogger();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder.Build());
            });
             
            // Sanjay [2017-02-08] All Properties Initials were got automatically converted in lowercase. Following code convert it into proper format
            services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented; });
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());


            //Sanjay [2018-02-12] To Maintain API Documentation with Versions
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;
                //option.ApiVersionReader = new HeaderApiVersionReader("api-version");
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(
       options =>
       {
           var provider = services.BuildServiceProvider()
                                  .GetRequiredService<IApiVersionDescriptionProvider>();

           foreach (var description in provider.ApiVersionDescriptions)
           {
               options.SwaggerDoc(
                   description.GroupName,
                   new Info()
                   {
                       Title = $"SimpliDeliverAPI {description.ApiVersion}",
                       Version = description.ApiVersion.ToString(),
                       Description = "SimpliDeliverAPI V 1.0 Web API Services",
                       Contact = new Contact()
                       { Name = "Vega Innovations & Technoconsultants Pvt Ltd", Email = "sanjay.gunjal@vegainnovations.co.in", Url = "www.vegainnovations.co.in" }

                   });
              // options.IncludeXmlComments(GetXmlCommentsPath());
               options.DescribeAllEnumsAsStrings();
           }
       });

                //Hrushikesh added for IOT Configuration
             services.AddScoped<IModbusRefConfig, ModbusRefConfig>();
            services.AddScoped<IDimBrandBL, DimBrandBL>();
            services.AddScoped<IDimDistrictBL, DimDistrictBL>();
            services.AddScoped<IDimensionBL, DimensionBL>();
            services.AddScoped<IDimGstCodeTypeBL, DimGstCodeTypeBL>();
            services.AddScoped<IDimMstDeptBL, DimMstDeptBL>();
            services.AddScoped<IDimMstDesignationBL, DimMstDesignationBL>();
            services.AddScoped<IDimOrgTypeBL, DimOrgTypeBL>();
            services.AddScoped<IDimPageElementTypesBL, DimPageElementTypesBL>();
            services.AddScoped<IDimProdCatBL, DimProdCatBL>();
            services.AddScoped<IDimProdSpecBL, DimProdSpecBL>();
            services.AddScoped<IDimProdSpecDescBL, DimProdSpecDescBL>();
            services.AddScoped<IDimReportTemplateBL, DimReportTemplateBL>();
            services.AddScoped<IDimRoleTypeBL, DimRoleTypeBL>();
            services.AddScoped<IDimSmsConfigBL, DimSmsConfigBL>();
            services.AddScoped<IDimStateBL, DimStateBL>();
            services.AddScoped<IDimStatusBL, DimStatusBL>();
            services.AddScoped<IDimTalukaBL, DimTalukaBL>();
            services.AddScoped<IDimTaxTypeBL, DimTaxTypeBL>();
            services.AddScoped<IDimTranActionTypeBL, DimTranActionTypeBL>();
            services.AddScoped<IDimUnitMeasuresBL, DimUnitMeasuresBL>();
            services.AddScoped<IDimVehDocTypeBL, DimVehDocTypeBL>();
            services.AddScoped<IDimVehicleTypeBL, DimVehicleTypeBL>();
            services.AddScoped<IDimVisitIssueReasonsBL, DimVisitIssueReasonsBL>();
            services.AddScoped<IFinalBookingData, FinalBookingData>();
            services.AddScoped<IFinalEnquiryData, FinalEnquiryData>();
            services.AddScoped<IGeoLocationAddressBL, GeoLocationAddressBL>();
            services.AddScoped<IMarketingDetailsBL, MarketingDetailsBL>();
            services.AddScoped<IRunReport, RunReport>();
            services.AddScoped<IRunVegaFlexCelReport, RunVegaFlexCelReport>();
            services.AddScoped<ISendMailBL, SendMailBL>();
            services.AddScoped<ITblAddressBL, TblAddressBL>();
            services.AddScoped<ITblAlertActionDtlBL, TblAlertActionDtlBL>();
            services.AddScoped<ITblAlertDefinitionBL, TblAlertDefinitionBL>();
            services.AddScoped<ITblAlertEscalationSettingsBL, TblAlertEscalationSettingsBL>();
            services.AddScoped<ITblAlertInstanceBL, TblAlertInstanceBL>();
            services.AddScoped<ITblAlertSubscribersBL, TblAlertSubscribersBL>();
            services.AddScoped<ITblAlertSubscriptSettingsBL, TblAlertSubscriptSettingsBL>();
            services.AddScoped<ITblAlertUsersBL, TblAlertUsersBL>();
            services.AddScoped<ITblBookingActionsBL, TblBookingActionsBL>();
            services.AddScoped<ITblBookingBeyondQuotaBL, TblBookingBeyondQuotaBL>();
            services.AddScoped<ITblBookingDelAddrBL, TblBookingDelAddrBL>();
            services.AddScoped<ITblBookingExtBL, TblBookingExtBL>();
            services.AddScoped<ITblBookingOpngBalBL, TblBookingOpngBalBL>();
            services.AddScoped<ITblBookingParitiesBL, TblBookingParitiesBL>();
            services.AddScoped<ITblBookingQtyConsumptionBL, TblBookingQtyConsumptionBL>();
            services.AddScoped<ITblBookingsBL, TblBookingsBL>();
            services.AddScoped<ITblBookingScheduleBL, TblBookingScheduleBL>();
            services.AddScoped<ITblCnfDealersBL, TblCnfDealersBL>();
            services.AddScoped<ITblCompetitorExtBL, TblCompetitorExtBL>();
            services.AddScoped<ITblCompetitorUpdatesBL, TblCompetitorUpdatesBL>();
            services.AddScoped<ITblConfigParamHistoryBL, TblConfigParamHistoryBL>();
            services.AddScoped<ITblConfigParamsBL, TblConfigParamsBL>();
            services.AddScoped<ITblContactUsDtlsBL, TblContactUsDtlsBL>();
            services.AddScoped<ITblCRMShareDocsDetailsBL, TblCRMShareDocsDetailsBL>();
            services.AddScoped<ITblDocumentDetailsBL, TblDocumentDetailsBL>();
            services.AddScoped<ITblEmailConfigrationBL, TblEmailConfigrationBL>();
            services.AddScoped<ITblEnquiryDtlBL, TblEnquiryDtlBL>();
            services.AddScoped<ITblEntityRangeBL, TblEntityRangeBL>();
            services.AddScoped<ITblFeedbackBL, TblFeedbackBL>();
            services.AddScoped<ITblFilterReportBL, TblFilterReportBL>();
            services.AddScoped<ITblFreightUpdateBL, TblFreightUpdateBL>();
            services.AddScoped<ITblGlobalRateBL, TblGlobalRateBL>();
            services.AddScoped<ITblGroupBL, TblGroupBL>();
            services.AddScoped<ITblGroupItemBL, TblGroupItemBL>();
            services.AddScoped<ITblGstCodeDtlsBL, TblGstCodeDtlsBL>();
            services.AddScoped<ITblInvoiceAddressBL, TblInvoiceAddressBL>();
            services.AddScoped<ITblInvoiceBankDetailsBL, TblInvoiceBankDetailsBL>();
            services.AddScoped<ITblInvoiceBL, TblInvoiceBL>();
            services.AddScoped<ITblInvoiceHistoryBL, TblInvoiceHistoryBL>();
            services.AddScoped<ITblInvoiceItemDetailsBL, TblInvoiceItemDetailsBL>();
            services.AddScoped<ITblInvoiceItemTaxDtlsBL, TblInvoiceItemTaxDtlsBL>();
            services.AddScoped<ITblInvoiceOtherDetailsBL, TblInvoiceOtherDetailsBL>();
            services.AddScoped<ITblIssueTypeBL, TblIssueTypeBL>();
            services.AddScoped<ITblItemBroadCategoriesBL, TblItemBroadCategoriesBL>();
            services.AddScoped<ITblItemTallyRefDtlsBL, TblItemTallyRefDtlsBL>();
            services.AddScoped<ITblKYCDetailsBL, TblKYCDetailsBL>();
            services.AddScoped<ITblLoadingAllowedTimeBL, TblLoadingAllowedTimeBL>();
            services.AddScoped<ITblLoadingBL, TblLoadingBL>();
            services.AddScoped<ITblLoadingQuotaConfigBL, TblLoadingQuotaConfigBL>();
            services.AddScoped<ITblLoadingQuotaConsumptionBL, TblLoadingQuotaConsumptionBL>();
            services.AddScoped<ITblLoadingQuotaDeclarationBL, TblLoadingQuotaDeclarationBL>();
            services.AddScoped<ITblLoadingQuotaTransferBL, TblLoadingQuotaTransferBL>();
            services.AddScoped<ITblLoadingSlipAddressBL, TblLoadingSlipAddressBL>();
            services.AddScoped<ITblLoadingSlipBL, TblLoadingSlipBL>();
            services.AddScoped<ITblLoadingSlipDtlBL, TblLoadingSlipDtlBL>();
            services.AddScoped<ITblLoadingSlipExtBL, TblLoadingSlipExtBL>();
            services.AddScoped<ITblLoadingSlipExtHistoryBL, TblLoadingSlipExtHistoryBL>();
            services.AddScoped<ITblLoadingSlipRemovedItemsBL, TblLoadingSlipRemovedItemsBL>();
            services.AddScoped<ITblLoadingStatusHistoryBL, TblLoadingStatusHistoryBL>();
            services.AddScoped<ITblLoadingVehDocExtBL, TblLoadingVehDocExtBL>();
            services.AddScoped<ITblLocationBL, TblLocationBL>();
            services.AddScoped<ITblLoginBL, TblLoginBL>();
            services.AddScoped<ITblMachineCalibrationBL, TblMachineCalibrationBL>();
            services.AddScoped<ITblMaterialBL, TblMaterialBL>();
            services.AddScoped<ITblMenuStructureBL, TblMenuStructureBL>();
            services.AddScoped<ITblModuleBL, TblModuleBL>();
            services.AddScoped<ITblModuleDAO, TblModuleDAO>();
            services.AddScoped<ITblModuleCommunicationBL, TblModuleCommunicationBL>();
            services.AddScoped<ITblModuleCommunicationDAO, TblModuleCommunicationDAO>();
            services.AddScoped<ITblOrgAddressBL, TblOrgAddressBL>();
            services.AddScoped<ITblOrganizationBL, TblOrganizationBL>();
            services.AddScoped<ITblOrgLicenseDtlBL, TblOrgLicenseDtlBL>();
            services.AddScoped<ITblOrgOverdueHistoryBL, TblOrgOverdueHistoryBL>();
            services.AddScoped<ITblOrgPersonDtlsBL, TblOrgPersonDtlsBL>();
            services.AddScoped<ITblOrgStructureBL, TblOrgStructureBL>();
            services.AddScoped<ITblOtherDesignationsBL, TblOtherDesignationsBL>();
            services.AddScoped<ITblOtherSourceBL, TblOtherSourceBL>();
            services.AddScoped<ITblOtherTaxesBL, TblOtherTaxesBL>();
            services.AddScoped<ITblOverdueDtlBL, TblOverdueDtlBL>();
            services.AddScoped<ITblPageElementsBL, TblPageElementsBL>();
            services.AddScoped<ITblPagesBL, TblPagesBL>();
            services.AddScoped<ITblParityDetailsBL, TblParityDetailsBL>();
            services.AddScoped<ITblParitySummaryBL, TblParitySummaryBL>();
            services.AddScoped<ITblPaymentTermBL, TblPaymentTermBL>();
            services.AddScoped<ITblPersonAddrDtlBL, TblPersonAddrDtlBL>();
            services.AddScoped<ITblPersonBL, TblPersonBL>();
            services.AddScoped<ITblProdClassificationBL, TblProdClassificationBL>();
            services.AddScoped<ITblProdGstCodeDtlsBL, TblProdGstCodeDtlsBL>();
            services.AddScoped<ITblProductInfoBL, TblProductInfoBL>();
            services.AddScoped<ITblProductItemBL, TblProductItemBL>();
            services.AddScoped<ITblPurchaseCompetitorExtBL, TblPurchaseCompetitorExtBL>();
            services.AddScoped<ITblQuotaConsumHistoryBL, TblQuotaConsumHistoryBL>();
            services.AddScoped<ITblQuotaDeclarationBL, TblQuotaDeclarationBL>();
            services.AddScoped<ITblRateDeclareReasonsBL, TblRateDeclareReasonsBL>();
            services.AddScoped<ITblReportsBL, TblReportsBL>();
            services.AddScoped<ITblRoleBL, TblRoleBL>();
            services.AddScoped<ITblRoleOrgSettingBL, TblRoleOrgSettingBL>();
            services.AddScoped<ITblRunningSizesBL, TblRunningSizesBL>();
            services.AddScoped<ITblSessionBL, TblSessionBL>();
            services.AddScoped<ITblSessionHistoryBL, TblSessionHistoryBL>();
            services.AddScoped<ITblSiteRequirementsBL, TblSiteRequirementsBL>();
            services.AddScoped<ITblSiteStatusBL, TblSiteStatusBL>();
            services.AddScoped<ITblSiteTypeBL, TblSiteTypeBL>();
            services.AddScoped<ITblSmsBL, TblSmsBL>();
            services.AddScoped<ITblStatusReasonBL, TblStatusReasonBL>();
            services.AddScoped<ITblStockAsPerBooksBL, TblStockAsPerBooksBL>();
            services.AddScoped<ITblStockConfigBL, TblStockConfigBL>();
            services.AddScoped<ITblStockConsumptionBL, TblStockConsumptionBL>();
            services.AddScoped<ITblStockDetailsBL, TblStockDetailsBL>();
            services.AddScoped<ITblStockSummaryBL, TblStockSummaryBL>();
            services.AddScoped<ITblStockTransferNoteBL, TblStockTransferNoteBL>();
            services.AddScoped<ITblStockYardBL, TblStockYardBL>();
            services.AddScoped<ITblSupervisorBL, TblSupervisorBL>();
            services.AddScoped<ITblSupportDetailsBL, TblSupportDetailsBL>();
            services.AddScoped<ITblSysElementsBL, TblSysElementsBL>();
            services.AddScoped<ITblSysEleRoleEntitlementsBL, TblSysEleRoleEntitlementsBL>();
            services.AddScoped<ITblSysEleUserEntitlementsBL, TblSysEleUserEntitlementsBL>();
            services.AddScoped<ITblTaskModuleExtBL, TblTaskModuleExtBL>();
            services.AddScoped<ITbltaskWithoutSubscBL, TbltaskWithoutSubscBL>();
            services.AddScoped<ITblTaxRatesBL, TblTaxRatesBL>();
            services.AddScoped<ITblTranActionsBL, TblTranActionsBL>();
            services.AddScoped<ITblTransportSlipBL, TblTransportSlipBL>();
            services.AddScoped<ITblUnLoadingBL, TblUnLoadingBL>();
            services.AddScoped<ITblUnLoadingItemDetBL, TblUnLoadingItemDetBL>();
            services.AddScoped<ITblUnloadingStandDescBL, TblUnloadingStandDescBL>();
            services.AddScoped<ITblUserAreaAllocationBL, TblUserAreaAllocationBL>();
            services.AddScoped<ITblUserBL, TblUserBL>();
            services.AddScoped<ITblUserBrandBL, TblUserBrandBL>();
            services.AddScoped<ITblUserExtBL, TblUserExtBL>();
            services.AddScoped<ITblUserLocationBL, TblUserLocationBL>();
            services.AddScoped<ITblUserPwdHistoryBL, TblUserPwdHistoryBL>();
            services.AddScoped<ITblUserRoleBL, TblUserRoleBL>();
            services.AddScoped<ITblUserVerBL, TblUserVerBL>();
            services.AddScoped<ITblVerReleaseNotesBL, TblVerReleaseNotesBL>();
            services.AddScoped<ITblVersionBL, TblVersionBL>();
            services.AddScoped<ITblVisitAdditionalDetailsBL, TblVisitAdditionalDetailsBL>();
            services.AddScoped<ITblVisitDetailsBL, TblVisitDetailsBL>();
            services.AddScoped<ITblVisitFeedbackBL, TblVisitFeedbackBL>();
            services.AddScoped<ITblVisitFollowupInfoBL, TblVisitFollowupInfoBL>();
            services.AddScoped<ITblVisitFollowUpRolesBL, TblVisitFollowUpRolesBL>();
            services.AddScoped<ITblVisitIssueDetailsBL, TblVisitIssueDetailsBL>();
            services.AddScoped<ITblVisitIssueReasonsBL, TblVisitIssueReasonsBL>();
            services.AddScoped<ITblVisitPersonDetailsBL, TblVisitPersonDetailsBL>();
            services.AddScoped<ITblVisitPersonTypeBL, TblVisitPersonTypeBL>();
            services.AddScoped<ITblVisitProjectDetailsBL, TblVisitProjectDetailsBL>();
            services.AddScoped<ITblVisitPurposeBL, TblVisitPurposeBL>();
            services.AddScoped<ITblWeighingBL, TblWeighingBL>();
            services.AddScoped<ITblWeighingMachineBL, TblWeighingMachineBL>();
            services.AddScoped<ITblWeighingMeasuresBL, TblWeighingMeasuresBL>();
            services.AddScoped<ITempInvoiceDocumentDetailsBL, TempInvoiceDocumentDetailsBL>();
            services.AddScoped<ITempLoadingSlipInvoiceBL, TempLoadingSlipInvoiceBL>();
            services.AddScoped<IVitplNotify, VitplNotify>();
            services.AddScoped<IVitplSMS, VitplSMS>();
            services.AddScoped<IDimBrandDAO, DimBrandDAO>();
            services.AddScoped<IDimDistrictDAO, DimDistrictDAO>();
            services.AddScoped<IDimensionDAO, DimensionDAO>();
            services.AddScoped<IDimGstCodeTypeDAO, DimGstCodeTypeDAO>();
            services.AddScoped<IDimMstDeptDAO, DimMstDeptDAO>();
            services.AddScoped<IDimMstDesignationDAO, DimMstDesignationDAO>();
            services.AddScoped<IDimOrgTypeDAO, DimOrgTypeDAO>();
            services.AddScoped<IDimPageElementTypesDAO, DimPageElementTypesDAO>();
            services.AddScoped<IDimProdCatDAO, DimProdCatDAO>();
            services.AddScoped<IDimProdSpecDAO, DimProdSpecDAO>();
            services.AddScoped<IDimProdSpecDescDAO, DimProdSpecDescDAO>();
            services.AddScoped<IDimReportTemplateDAO, DimReportTemplateDAO>();
            services.AddScoped<IDimRoleTypeDAO, DimRoleTypeDAO>();
            services.AddScoped<IDimSmsConfigDAO, DimSmsConfigDAO>();
            services.AddScoped<IDimStateDAO, DimStateDAO>();
            services.AddScoped<IDimStatusDAO, DimStatusDAO>();
            services.AddScoped<IDimTalukaDAO, DimTalukaDAO>();
            services.AddScoped<IDimTaxTypeDAO, DimTaxTypeDAO>();
            services.AddScoped<IDimTranActionTypeDAO, DimTranActionTypeDAO>();
            services.AddScoped<IDimUnitMeasuresDAO, DimUnitMeasuresDAO>();
            services.AddScoped<IDimVehDocTypeDAO, DimVehDocTypeDAO>();
            services.AddScoped<IDimVehicleTypeDAO, DimVehicleTypeDAO>();
            services.AddScoped<IDimVisitIssueReasonsDAO, DimVisitIssueReasonsDAO>();
            services.AddScoped<ISendMailDAO, SendMailDAO>();
            services.AddScoped<ISQLHelper, SQLHelper>();
            services.AddScoped<ITblAddressDAO, TblAddressDAO>();
            services.AddScoped<ITblAlertActionDtlDAO, TblAlertActionDtlDAO>();
            services.AddScoped<ITblAlertDefinitionDAO, TblAlertDefinitionDAO>();
            services.AddScoped<ITblAlertEscalationSettingsDAO, TblAlertEscalationSettingsDAO>();
            services.AddScoped<ITblAlertInstanceDAO, TblAlertInstanceDAO>();
            services.AddScoped<ITblAlertSubscribersDAO, TblAlertSubscribersDAO>();
            services.AddScoped<ITblAlertSubscriptSettingsDAO, TblAlertSubscriptSettingsDAO>();
            services.AddScoped<ITblAlertUsersDAO, TblAlertUsersDAO>();
            services.AddScoped<ITblBookingActionsDAO, TblBookingActionsDAO>();
            services.AddScoped<ITblBookingBeyondQuotaDAO, TblBookingBeyondQuotaDAO>();
            services.AddScoped<ITblBookingDelAddrDAO, TblBookingDelAddrDAO>();
            services.AddScoped<ITblBookingExtDAO, TblBookingExtDAO>();
            services.AddScoped<ITblBookingOpngBalDAO, TblBookingOpngBalDAO>();
            services.AddScoped<ITblBookingParitiesDAO, TblBookingParitiesDAO>();
            services.AddScoped<ITblBookingQtyConsumptionDAO, TblBookingQtyConsumptionDAO>();
            services.AddScoped<ITblBookingScheduleDAO, TblBookingScheduleDAO>();
            services.AddScoped<ITblBookingsDAO, TblBookingsDAO>();
            services.AddScoped<ITblCnfDealersDAO, TblCnfDealersDAO>();
            services.AddScoped<ITblCompetitorExtDAO, TblCompetitorExtDAO>();
            services.AddScoped<ITblCompetitorUpdatesDAO, TblCompetitorUpdatesDAO>();
            services.AddScoped<ITblConfigParamHistoryDAO, TblConfigParamHistoryDAO>();
            services.AddScoped<ITblConfigParamsDAO, TblConfigParamsDAO>();
            services.AddScoped<ITblContactUsDtlsDAO, TblContactUsDtlsDAO>();
            services.AddScoped<ITblCRMShareDocsDetailsDAO, TblCRMShareDocsDetailsDAO>();
            services.AddScoped<ITblDocumentDetailsDAO, TblDocumentDetailsDAO>();
            services.AddScoped<ITblEmailConfigrationDAO, TblEmailConfigrationDAO>();
            services.AddScoped<ITblEmailHistoryDAO, TblEmailHistoryDAO>();
            services.AddScoped<ITblEnquiryDtlDAO, TblEnquiryDtlDAO>();
            services.AddScoped<ITblEntityRangeDAO, TblEntityRangeDAO>();
            services.AddScoped<ITblEntityRangeDAO, TblEntityRangeDAO>();
            services.AddScoped<ITblFeedbackDAO, TblFeedbackDAO>();
            services.AddScoped<ITblFilterReportDAO, TblFilterReportDAO>();
            services.AddScoped<ITblFreightUpdateDAO, TblFreightUpdateDAO>();
            services.AddScoped<ITblGlobalRateDAO, TblGlobalRateDAO>();
            services.AddScoped<ITblGroupDAO, TblGroupDAO>();
            services.AddScoped<ITblGroupItemDAO, TblGroupItemDAO>();
            services.AddScoped<ITblGstCodeDtlsDAO, TblGstCodeDtlsDAO>();
            services.AddScoped<ITblInvoiceAddressDAO, TblInvoiceAddressDAO>();
            services.AddScoped<ITblInvoiceBankDetailsDAO, TblInvoiceBankDetailsDAO>();
            services.AddScoped<ITblInvoiceDAO, TblInvoiceDAO>();
            services.AddScoped<ITblInvoiceHistoryDAO, TblInvoiceHistoryDAO>();
            services.AddScoped<ITblInvoiceItemDetailsDAO, TblInvoiceItemDetailsDAO>();
            services.AddScoped<ITblInvoiceItemTaxDtlsDAO, TblInvoiceItemTaxDtlsDAO>();
            services.AddScoped<ITblInvoiceOtherDetailsDAO, TblInvoiceOtherDetailsDAO>();
            services.AddScoped<ITblIssueTypeDAO, TblIssueTypeDAO>();
            services.AddScoped<ITblItemBroadCategoriesDAO, TblItemBroadCategoriesDAO>();
            services.AddScoped<ITblItemTallyRefDtlsDAO, TblItemTallyRefDtlsDAO>();
            services.AddScoped<ITblKYCDetailsDAO, TblKYCDetailsDAO>();
            services.AddScoped<ITblLoadingAllowedTimeDAO, TblLoadingAllowedTimeDAO>();
            services.AddScoped<ITblLoadingDAO, TblLoadingDAO>();
            services.AddScoped<ITblLoadingQuotaConfigDAO, TblLoadingQuotaConfigDAO>();
            services.AddScoped<ITblLoadingQuotaConsumptionDAO, TblLoadingQuotaConsumptionDAO>();
            services.AddScoped<ITblLoadingQuotaDeclarationDAO, TblLoadingQuotaDeclarationDAO>();
            services.AddScoped<ITblLoadingQuotaTransferDAO, TblLoadingQuotaTransferDAO>();
            services.AddScoped<ITblLoadingSlipAddressDAO, TblLoadingSlipAddressDAO>();
            services.AddScoped<ITblLoadingSlipDAO, TblLoadingSlipDAO>();
            services.AddScoped<ITblLoadingSlipDtlDAO, TblLoadingSlipDtlDAO>();
            services.AddScoped<ITblLoadingSlipExtDAO, TblLoadingSlipExtDAO>();
            services.AddScoped<ITblLoadingSlipExtHistoryDAO, TblLoadingSlipExtHistoryDAO>();
            services.AddScoped<ITblLoadingSlipRemovedItemsDAO, TblLoadingSlipRemovedItemsDAO>();
            services.AddScoped<ITblLoadingStatusHistoryDAO, TblLoadingStatusHistoryDAO>();
            services.AddScoped<ITblLoadingVehDocExtDAO, TblLoadingVehDocExtDAO>();
            services.AddScoped<ITblLocationDAO, TblLocationDAO>();
            services.AddScoped<ITblLoginDAO, TblLoginDAO>();
            services.AddScoped<ITblMachineCalibrationDAO, TblMachineCalibrationDAO>();
            services.AddScoped<ITblMaterialDAO, TblMaterialDAO>();
            services.AddScoped<ITblMenuStructureDAO, TblMenuStructureDAO>();
            services.AddScoped<ITblOrgAddressDAO, TblOrgAddressDAO>();
            services.AddScoped<ITblOrganizationDAO, TblOrganizationDAO>();
            services.AddScoped<ITblOrgLicenseDtlDAO, TblOrgLicenseDtlDAO>();
            services.AddScoped<ITblOrgOverdueHistoryDAO, TblOrgOverdueHistoryDAO>();
            services.AddScoped<ITblOrgPersonDtlsDAO, TblOrgPersonDtlsDAO>();
            services.AddScoped<ITblOrgStructureDAO, TblOrgStructureDAO>();
            services.AddScoped<ITblOtherDesignationsDAO, TblOtherDesignationsDAO>();
            services.AddScoped<ITblOtherSourceDAO, TblOtherSourceDAO>();
            services.AddScoped<ITblOtherTaxesDAO, TblOtherTaxesDAO>();
            services.AddScoped<ITblOverdueDtlDAO, TblOverdueDtlDAO>();
            services.AddScoped<ITblPageElementsDAO, TblPageElementsDAO>();
            services.AddScoped<ITblPagesDAO, TblPagesDAO>();
            services.AddScoped<ITblParityDetailsDAO, TblParityDetailsDAO>();
            services.AddScoped<ITblParitySummaryDAO, TblParitySummaryDAO>();
            services.AddScoped<ITblPaymentTermDAO, TblPaymentTermDAO>();
            services.AddScoped<ITblPersonAddrDtlDAO, TblPersonAddrDtlDAO>();
            services.AddScoped<ITblPersonDAO, TblPersonDAO>();
            services.AddScoped<ITblProdClassificationDAO, TblProdClassificationDAO>();
            services.AddScoped<ITblProdGstCodeDtlsDAO, TblProdGstCodeDtlsDAO>();
            services.AddScoped<ITblProductInfoDAO, TblProductInfoDAO>();
            services.AddScoped<ITblProductItemDAO, TblProductItemDAO>();
            services.AddScoped<ITblPurchaseCompetitorExtDAO, TblPurchaseCompetitorExtDAO>();
            services.AddScoped<ITblQuotaDeclarationDAO, TblQuotaDeclarationDAO>();
            services.AddScoped<ITblQuotaConsumHistoryDAO, TblQuotaConsumHistoryDAO>();
            services.AddScoped<ITblRateDeclareReasonsDAO, TblRateDeclareReasonsDAO>();
            services.AddScoped<ITblReportsDAO, TblReportsDAO>();
            services.AddScoped<ITblRoleDAO, TblRoleDAO>();
            services.AddScoped<ITblRoleOrgSettingDAO, TblRoleOrgSettingDAO>();
            services.AddScoped<ITblRunningSizesDAO, TblRunningSizesDAO>();
            services.AddScoped<ITblSessionDAO, TblSessionDAO>();
            services.AddScoped<ITblSessionHistoryDAO, TblSessionHistoryDAO>();
            services.AddScoped<ITblSiteRequirementsDAO, TblSiteRequirementsDAO>();
            services.AddScoped<ITblSiteStatusDAO, TblSiteStatusDAO>();
            services.AddScoped<ITblSiteTypeDAO, TblSiteTypeDAO>();
            services.AddScoped<ITblSmsDAO, TblSmsDAO>();
            services.AddScoped<ITblStatusReasonDAO, TblStatusReasonDAO>();
            services.AddScoped<ITblStockAsPerBooksDAO, TblStockAsPerBooksDAO>();
            services.AddScoped<ITblStockConfigDAO, TblStockConfigDAO>();
            services.AddScoped<ITblStockConfigDAO, TblStockConfigDAO>();
            services.AddScoped<ITblStockConsumptionDAO, TblStockConsumptionDAO>();
            services.AddScoped<ITblStockDetailsDAO, TblStockDetailsDAO>();
            services.AddScoped<ITblStockSummaryDAO, TblStockSummaryDAO>();
            services.AddScoped<ITblStockTransferNoteDAO, TblStockTransferNoteDAO>();
            services.AddScoped<ITblStockYardDAO, TblStockYardDAO>();
            services.AddScoped<ITblSupervisorDAO, TblSupervisorDAO>();
            services.AddScoped<ITblSupportDetailsDAO, TblSupportDetailsDAO>();
            services.AddScoped<ITblSysElementsDAO, TblSysElementsDAO>();
            services.AddScoped<ITblSysEleRoleEntitlementsDAO, TblSysEleRoleEntitlementsDAO>();
            services.AddScoped<ITblSysEleUserEntitlementsDAO, TblSysEleUserEntitlementsDAO>();
            services.AddScoped<ITblTaskModuleExtDAO, TblTaskModuleExtDAO>();
            services.AddScoped<ITbltaskWithoutSubscDAO, TbltaskWithoutSubscDAO>();
            services.AddScoped<ITblTaxRatesDAO, TblTaxRatesDAO>();
            services.AddScoped<ITblTranActionsDAO, TblTranActionsDAO>();
            services.AddScoped<ITblTransportSlipDAO, TblTransportSlipDAO>();
            services.AddScoped<ITblUnLoadingDAO, TblUnLoadingDAO>();
            services.AddScoped<ITblUnLoadingItemDetDAO, TblUnLoadingItemDetDAO>();
            services.AddScoped<ITblUnloadingStandDescDAO, TblUnloadingStandDescDAO>();
            services.AddScoped<ITblUserAreaAllocationDAO, TblUserAreaAllocationDAO>();
            services.AddScoped<ITblUserBrandDAO, TblUserBrandDAO>();
            services.AddScoped<ITblUserDAO, TblUserDAO>();
            services.AddScoped<ITblUserExtDAO, TblUserExtDAO>();
            services.AddScoped<ITblUserLocationDAO, TblUserLocationDAO>();
            services.AddScoped<ITblUserPwdHistoryDAO, TblUserPwdHistoryDAO>();
            services.AddScoped<ITblUserRoleDAO, TblUserRoleDAO>();
            services.AddScoped<ITblUserVerDAO, TblUserVerDAO>();
            services.AddScoped<ITblVerReleaseNotesDAO, TblVerReleaseNotesDAO>();
            services.AddScoped<ITblVersionDAO, TblVersionDAO>();
            services.AddScoped<ITblVisitAdditionalDetailsDAO, TblVisitAdditionalDetailsDAO>();
            services.AddScoped<ITblVisitDetailsDAO, TblVisitDetailsDAO>();
            services.AddScoped<ITblVisitFeedbackDAO, TblVisitFeedbackDAO>();
            services.AddScoped<ITblVisitFollowupInfoDAO, TblVisitFollowupInfoDAO>();
            services.AddScoped<ITblVisitFollowUpRolesDAO, TblVisitFollowUpRolesDAO>();
            services.AddScoped<ITblVisitIssueDetailsDAO, TblVisitIssueDetailsDAO>();
            services.AddScoped<ITblVisitIssueReasonsDAO, TblVisitIssueReasonsDAO>();
            services.AddScoped<ITblVisitPersonDetailsDAO, TblVisitPersonDetailsDAO>();
            services.AddScoped<ITblVisitPersonTypeDAO, TblVisitPersonTypeDAO>();
            services.AddScoped<ITblVisitProjectDetailsDAO, TblVisitProjectDetailsDAO>();
            services.AddScoped<ITblVisitPurposeDAO, TblVisitPurposeDAO>();
            services.AddScoped<ITblWeighingDAO, TblWeighingDAO>();
            services.AddScoped<ITblWeighingMachineDAO, TblWeighingMachineDAO>();
            services.AddScoped<ITblWeighingMeasuresDAO, TblWeighingMeasuresDAO>();
            services.AddScoped<ITempInvoiceDocumentDetailsDAO, TempInvoiceDocumentDetailsDAO>();
            services.AddScoped<ITempLoadingSlipInvoiceDAO, TempLoadingSlipInvoiceDAO>();
            services.AddScoped<IConnectionString, ConnectionString>();
            services.AddScoped<IAuthenticationDAO, AuthenticationDAO>();
            services.AddScoped<IAuthentication, Authentications>();
            services.AddScoped<ICommon, Common>();
            services.AddScoped<ICircularDependencyBL, CircularDependencyBL>();
            services.AddScoped<IDimAddonsFunBL, DimAddonsFunBL>();
            services.AddScoped<IDimAddonsFunDAO, DimAddonsFunDAO>();
            services.AddScoped<ITblPaymentTermOptionRelationBL, TblPaymentTermOptionRelationBL>();
            services.AddScoped<ITblPaymentTermOptionRelationDAO, TblPaymentTermOptionRelationDAO>();
            services.AddScoped<ITblAddonsFunDtlsBL, TblAddonsFunDtlsBL>();
            services.AddScoped<ITblAddonsFunDtlsDAO, TblAddonsFunDtlsDAO>();
            services.AddScoped<ITblPaymentTermsForBookingBL, TblPaymentTermsForBookingBL>();
            services.AddScoped<ITblPaymentTermsForBookingDAO, TblPaymentTermsForBookingDAO>();
            services.AddScoped<ITblPaymentTermOptionsBL, TblPaymentTermOptionsBL>();
            services.AddScoped<ITblPaymentTermOptionsDAO, TblPaymentTermOptionsDAO>();
             services.AddScoped<IDynamicApprovalCYcleBL, DynamicApprovalCycleBL>();
              services.AddScoped<IDynamicApprovalCycleDAO, DynamicApprovalCycleDAO>();
            services.AddScoped<ITblCRMLabelBL, TblCRMLabelBL>();
            services.AddScoped<ITblCRMLabelDAO, TblCRMLabelDAO>();
            services.AddScoped<IDimConfigurePageBL, DimConfigurePageBL>();
            services.AddScoped<IDimConfigurePageDAO, DimConfigurePageDAO>();
            services.AddScoped<InotificationDAO, notificationDAO>();
            services.AddScoped<IGateCommunication, GateCommunication>();
            services.AddScoped<IIotCommunication, IotCommunication>();
            services.AddScoped<IWeighingCommunication, WeighingCommunication>();
            services.AddScoped<ITblGateBL, TblGateBL>();
            services.AddScoped<ITblGateDAO, TblGateDAO>();
            services.AddScoped<ITblEInvoiceApiBL, TblEInvoiceApiBL>();
            services.AddScoped<ITblEInvoiceApiDAO, TblEInvoiceApiDAO>();
            services.AddScoped<ITblEInvoiceApiResponseBL, TblEInvoiceApiResponseBL>();
            services.AddScoped<ITblEInvoiceApiResponseDAO, TblEInvoiceApiResponseDAO>();
            services.AddScoped<ITblEInvoiceSessionApiResponseBL, TblEInvoiceSessionApiResponseBL>();
            services.AddScoped<ITblEInvoiceSessionApiResponseDAO, TblEInvoiceSessionApiResponseDAO>();


            services.AddScoped<ITblInvoiceChangeOrgHistoryDAO, TblInvoiceChangeOrgHistoryDAO>();

            services.AddScoped<ITblConfigParamsDAO, TblConfigParamsDAO>();
            services.AddScoped<ITblConfigParamsBL, TblConfigParamsBL>();



            DOSapLogin();
            services.AddMvc();
            ConnectionString = Configuration.GetSection("Data:DefaultConnection").Value.ToString();
            RequestOriginString = Configuration.GetSection("Data:RequestOriginString").Value.ToString();
            NewConnectionString = Configuration.GetSection("Data:NewDefaultConnection").Value.ToString();
            DeliverUrl = Configuration.GetSection("Data:DeliverUrl").Value.ToString();
            StockUrl = Configuration.GetSection("Data:StockUrl").Value.ToString();
            AzureConnectionStr = Configuration.GetSection("Data:AzureConnectionStr").Value.ToString();

            ConnectionJsonFile = JObject.Parse(System.IO.File.ReadAllText(@".\connection.json"));

            GetDateTimeQueryString();
            IsLocalApi();
            SAPLoginDetails();
            DOSapLogin();
            

            //TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(StaticStuff.Constants.CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION);
            //if (tblConfigParamsTO != null)
            //{
            //    AzureConnectionStr = tblConfigParamsTO.ConfigParamVal;
            //}

            //if (isLive)
            //{
            //    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(StaticStuff.Constants.CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION);
            //    if (tblConfigParamsTO != null)
            //    {
            //        AzureConnectionStr = tblConfigParamsTO.ConfigParamVal;
            //    }
            //}
            //else
            //{
            //    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(StaticStuff.Constants.CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION_TESTING);
            //    if (tblConfigParamsTO != null)
            //    {
            //        AzureConnectionStr = tblConfigParamsTO.ConfigParamVal;
            //    }
            //}

            // Vaibhav [15-Mar-2018] Configure authentication server.
            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //        .AddIdentityServerAuthentication(options =>
            //        {
            //            options.Authority = "http://localhost:5000"; // Auth Server
            //            options.RequireHttpsMetadata = false;
            //        });
        }


        public void GetDateTimeQueryString()
        {
            string sqlQuery = "SELECT CURRENT_TIMESTAMP AS ServerDate";

            TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamValByName(Constants.SERVER_DATETIME_QUERY_STRING);
            if (tblConfigParamsTO.ConfigParamVal != null)
            {
                sqlQuery = tblConfigParamsTO.ConfigParamVal;
            }
            SERVER_DATETIME_QUERY_STRING = sqlQuery;

        }

        public void IsLocalApi()
        {
            IsLocalAPI = false;
            TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamValByName(Constants.IS_LOCAL_API);
            if (tblConfigParamsTO != null)
            {
                Int32 isLocalAPI = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                if (isLocalAPI == 1)
                    IsLocalAPI = true;
                else
                    IsLocalAPI = false;
            }
        }


        public void SAPLoginDetails()
        {
            TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamValByName(Constants.SAP_LOGIN_DETAILS);
            if (tblConfigParamsTO != null && tblConfigParamsTO.ConfigParamVal != null)
            {
                sapLogindtls = Newtonsoft.Json.JsonConvert.DeserializeObject<SAPLoginDetails>(tblConfigParamsTO.ConfigParamVal);
            }
        }

        public void DOSapLogin()
        {
            try
            {
                SAPbobsCOM.Company companyObject;
                //SAPbouiCOM.SboGuiApi sboGuiApi;


                companyObject = new SAPbobsCOM.Company();
                //sboGuiApi = new SAPbouiCOM.SboGuiApi();


                companyObject.CompanyDB = sapLogindtls.CompanyDB;


                companyObject.UserName = sapLogindtls.UserName;


                //companyObject.Password = "Sap@1234";
                //companyObject.Password = "Vega@123";
                companyObject.Password = sapLogindtls.Password;
                companyObject.language = SAPbobsCOM.BoSuppLangs.ln_English;
                companyObject.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
                //companyObject.Server = "10.10.110.102";
                companyObject.Server = sapLogindtls.Server;


                companyObject.LicenseServer = sapLogindtls.LicenseServer;


                //companyObject.SLDServer = "52.172.136.203\\SQLEXPRESS,1430";
                companyObject.SLDServer = sapLogindtls.SLDServer;
                companyObject.DbUserName = sapLogindtls.DbUserName;


                companyObject.DbPassword = sapLogindtls.DbPassword;


                //companyObject.LicenseServer = "10.10.110.102:40000";


                //companyObject.UseTrusted = false;
                //string var = companyObject.GetLastErrorDescription();


                //string Error = companyObject.GetLastErrorDescription();


                int checkConnection = -1;


                checkConnection = companyObject.Connect();


                if (checkConnection == 0)
                    CompanyObject = companyObject;
                else
                    SapConnectivityErrorCode = "Connectivity Error Code : " + companyObject.GetLastErrorDescription() + " " + checkConnection.ToString();
            }
            catch (Exception ex)
            {
                SapConnectivityErrorCode = "SAP connectivity Exception " + ex.ToString();
            }
        }


        private string GetXmlCommentsPath()
        {
            var app = PlatformServices.Default.Application;
            return System.IO.Path.Combine(app.ApplicationBasePath, "ODLMWebAPI.xml");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors("AllowAll");
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();


            //Sanjay [2017-02-11] For Logging
            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug(LogLevel.Information).AddSerilog();
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error).AddSerilog();
            }

            app.UseAuthentication();
            app.UseMvc();

            //Sanjay [2018-02-12] To Implement Swagger Documentation for APIs
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"../swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });

                //setting up multiTenant modbusRefLists
                //Hrushikesh
                //ModbusRefConfig.setModbusRef();

        }
    }


    public class SAPLoginDetails
    {


        string companyDB;
        string userName;
        string password;
        string server;
        string licenseServer;
        string sLDServer;
        string dbUserName;
        string dbPassword;


        public string CompanyDB
        {
            get { return companyDB; }
            set { companyDB = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        public string LicenseServer
        {
            get { return licenseServer; }
            set { licenseServer = value; }
        }

        public string SLDServer
        {
            get { return sLDServer; }
            set { sLDServer = value; }
        }

        public string DbUserName
        {
            get { return dbUserName; }
            set { dbUserName = value; }
        }


        public string DbPassword
        {
            get { return dbPassword; }
            set { dbPassword = value; }
        }


    }

}
