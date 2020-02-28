using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;
using PowerBIEmbedded_AppOwnsData.Models;
using PowerBIEmbedded_AppOwnsData.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PowerBIEmbedded_AppOwnsData.Controllers
{
    public class HomeController : Controller
    {
        //Note: QuickStat Reports Variable
        private readonly IEmbedService m_embedService;
        private readonly IEmbedService m_LFLembedService;

        //Note: Scorecard Reports variable
        private readonly IEmbedService m_SCSummary;
        private readonly IEmbedService m_SCAggView;
        private readonly IEmbedService m_SCMonthlyRunRate;

        //Note: Finance Reports variable
        private readonly IEmbedService m_FinanceFYview;
        private readonly IEmbedService m_FinanceHOview;
        private readonly IEmbedService m_FinanceMLview;

        //Note: Opertaional Reports variable
        private readonly IEmbedService m_OpInsights;
        private readonly IEmbedService m_OpDailyLabourAnalysis;
        private readonly IEmbedService m_OpActualProjectLabour;
        private readonly IEmbedService m_OpSalesByProductHierarchy;
        private readonly IEmbedService m_OpMysteryShopperQAR;

        //Note: Head Office Reports variable
        private readonly IEmbedService m_FinanceReport;
        private readonly IEmbedService m_MarketingReport;

        //Note: Head Office Finance Reports Variable
        private readonly IEmbedService m_HOFDCSSR;
        private readonly IEmbedService m_HOFSBDBMT;
        private readonly IEmbedService m_HOFSSFW;
        private readonly IEmbedService m_HOFGCS;
        private readonly IEmbedService m_HOFSSFMY;
        private readonly IEmbedService m_HOFLSSFY;
        private readonly IEmbedService m_HOFLSSF;
        private readonly IEmbedService m_HOFBST;

        //Note: Head Office Marketing Reports Variable


        //Note: Help Reports Variable
        private readonly IEmbedService m_HelpReport;

        //Note: Mobile Reports variable
        private readonly IEmbedService m_QuickStat_Mobile;
        private readonly IEmbedService m_QuickStatLFL_Mobile;
        private readonly IEmbedService m_SCSummary_Mobile;
        private readonly IEmbedService m_DailyLabour_Mobile;


        public HomeController()
        {
            //Note: QuickStat Reports Constrcutor call
            m_embedService = new EmbedService();
            m_LFLembedService = new QuickStatLFLEmbedService();
            //Note: Finance Reports Constructor call
            m_FinanceFYview = new FinanceFYView();
            m_FinanceHOview = new FinanceHOView();
            m_FinanceMLview = new FinanceMLView();
            //Note: Operational Reports Constructor call
            m_OpInsights = new OPInsight();
            m_OpDailyLabourAnalysis = new OPDailyLabourAnalysis();
            m_OpActualProjectLabour = new OPActualProjectLabour();
            m_OpSalesByProductHierarchy = new OPSalesProdHierarchy();
            m_OpMysteryShopperQAR = new OPMysteryShopperQAR();
            //Note: Scorecard Reports Constructor call
            m_SCSummary = new SCSummary();
            m_SCAggView = new SCAggView();
            m_SCMonthlyRunRate = new SCMonRunRate();
            //Note: Head Office Reports constructor call
            m_FinanceReport = new HOFinanceReportService();
            m_MarketingReport = new HOMarketingReportService();
            //Note: Head Office Finance Constructor Call
            m_HOFDCSSR = new HOFDCSSR();
            m_HOFSBDBMT = new HOFSBDBMT();
            m_HOFSSFW = new HOFSSFW();
            m_HOFGCS = new HOFGCS();
            m_HOFSSFMY = new HOFSSFMY();
            m_HOFLSSFY = new HOFLSSFY();
            m_HOFLSSF = new HOFLSSF();
            m_HOFBST = new HOFBST();
            //Note: Help Report constructor call
            m_HelpReport = new HelpReportService();
            //Note: Mobile Reports constructor call
            m_QuickStat_Mobile = new MobileQuickStatReportServie();
            m_QuickStatLFL_Mobile = new MobileQuickStatLFLReportService();
            m_SCSummary_Mobile = new MobileSCSummaryService();
            m_DailyLabour_Mobile = new MobileDailyLabourReportService();
        }

        public ActionResult Index()
        {
            var result = new IndexConfig();
            var assembly = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(n => n.Name.Equals("Microsoft.PowerBI.Api")).FirstOrDefault();
            if (assembly != null)
            {
                result.DotNETSDK = assembly.Version.ToString(3);
            }
            return View(result);
        }

        public ActionResult Landing()
        {
            return View();
        }
        public ActionResult HeadOffice()
        {
            return View();
        }

        public ActionResult HeadOfficeMarketing()
        {
            return View();
        }
        public async Task<ActionResult> EmbedReport(string username, string roles)
        {
            var embedResult = false;
            var reportName = Request.QueryString["name"].ToString();
            var deviceType = Request.QueryString["d"].ToString();
            if (deviceType == "m")
            {
                if (reportName.ToLower() == "quick stats")
                {
                    embedResult = await m_QuickStat_Mobile.EmbedReport(username, roles);
                }
                else if (reportName.ToLower() == "quick stats lfl")
                {
                    embedResult = await m_QuickStatLFL_Mobile.EmbedReport(username, roles);
                }
                else if (reportName.ToLower() == "daily labour analysis")
                {
                    embedResult = await m_DailyLabour_Mobile.EmbedReport(username, roles);
                }
                else
                {
                    embedResult = await m_SCSummary_Mobile.EmbedReport(username, roles);
                }
                if (embedResult)
                {
                    if (reportName.ToLower() == "quick stats")
                    {
                        return View(m_QuickStat_Mobile.EmbedConfig);
                    }
                    else if (reportName.ToLower() == "quick stats lfl")
                    {
                        return View(m_QuickStatLFL_Mobile.EmbedConfig);
                    }
                    else if (reportName.ToLower() == "daily labour analysis")
                    {
                        return View(m_DailyLabour_Mobile.EmbedConfig);
                    }
                    else
                    {
                        return View(m_SCSummary_Mobile.EmbedConfig);
                    }    
                } 
                else
                {
                    return View(m_embedService.EmbedConfig);
                }
            }
            //
            else { 
            //Note: QuickStats All Report
            if (reportName.ToLower() == "quick stats")
            {
                embedResult = await m_embedService.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "quick stats lfl")
            {
                embedResult = await m_LFLembedService.EmbedReport(username, roles);
            }
            //Note: Finance all the view
            else if (reportName.ToLower() == "pl fy view")
            {
                embedResult = await m_FinanceFYview.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "pl head office view")
            {
                embedResult = await m_FinanceHOview.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "pl multi location view")
            {
                embedResult = await m_FinanceMLview.EmbedReport(username, roles);
            }
            //Note: Operational All Report
            else if (reportName.ToLower() == "operational insights")
            {
                embedResult = await m_OpInsights.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "daily labour analysis")
            {
                embedResult = await m_OpDailyLabourAnalysis.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "actual projected labour")
            {
                embedResult = await m_OpActualProjectLabour.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "sales by product hierarchy")
            {
                embedResult = await m_OpSalesByProductHierarchy.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "mystery shopper  qar")
            {
                embedResult = await m_OpMysteryShopperQAR.EmbedReport(username, roles);
            }
            //Note: Head Office All Report
            else if (reportName.ToLower() == "finance")
            {
                embedResult = await m_FinanceReport.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "marketing")
            {
                embedResult = await m_MarketingReport.EmbedReport(username, roles);
            }

            //Note: Head Office Finance All Report  
            else if (reportName.ToLower() == "daily corp store stats report")
            {
                embedResult = await m_HOFDCSSR.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "sales by date by media type")
            {
                 embedResult = await m_HOFSBDBMT.EmbedReport(username, roles);
            }
                else if (reportName.ToLower() == "summary scorecard finance weekly")
                {
                    embedResult = await m_HOFSSFW.EmbedReport(username, roles);
                }
                else if (reportName.ToLower() == "gift card sales")
                {
                    embedResult = await m_HOFGCS.EmbedReport(username, roles);
                }
                else if (reportName.ToLower() == "summary scorecard finance month and year")
                {
                    embedResult = await m_HOFSSFMY.EmbedReport(username, roles);
                }
                else if (reportName.ToLower() == "lfl summary scorecard finance year")
                {
                    embedResult = await m_HOFLSSFY.EmbedReport(username, roles);
                }
                else if (reportName.ToLower() == "lfl summary scorecard finance")
                {
                    embedResult = await m_HOFLSSF.EmbedReport(username, roles);
                }
                else if (reportName.ToLower() == "brewery sales trend")
                {
                    embedResult = await m_HOFBST.EmbedReport(username, roles);
                }

                //Note: Help Report Service
                else if (reportName.ToLower() == "dashboard help")
            {
                embedResult = await m_HelpReport.EmbedReport(username, roles);
            }
            //Note: Scorecard All Report
            else if (reportName.ToLower() == "scorecard summary")
            {
                embedResult = await m_SCSummary.EmbedReport(username, roles);
            }
            else if (reportName.ToLower() == "aggregated view")
            {
                embedResult = await m_SCAggView.EmbedReport(username, roles);
            }
            else
            {
                //For monthly run rate
                embedResult = await m_SCMonthlyRunRate.EmbedReport(username, roles);
            }

            if (embedResult)
            {
                //Note: Quick Stat Report View
                if (reportName.ToLower() == "quick stats")
                {
                    return View(m_embedService.EmbedConfig);
                }
                else if (reportName.ToLower() == "quick stats lfl")
                {
                    return View(m_LFLembedService.EmbedConfig);
                }

                //Note: Finance Report View
                else if (reportName.ToLower() == "pl fy view")
                {
                    return View(m_FinanceFYview.EmbedConfig);
                }
                else if (reportName.ToLower() == "pl head office view")
                {
                    return View(m_FinanceHOview.EmbedConfig);
                }
                else if (reportName.ToLower() == "pl multi location view")
                {
                    return View(m_FinanceMLview.EmbedConfig);
                }
                //Note: Operational Report View
                else if (reportName.ToLower() == "operational insights")
                {
                    return View(m_OpInsights.EmbedConfig);
                }
                else if (reportName.ToLower() == "daily labour analysis")
                {
                    return View(m_OpDailyLabourAnalysis.EmbedConfig);
                }
                else if (reportName.ToLower() == "actual projected labour")
                {
                    return View(m_OpActualProjectLabour.EmbedConfig);
                }
                else if (reportName.ToLower() == "sales by product hierarchy")
                {
                    return View(m_OpSalesByProductHierarchy.EmbedConfig);
                }
                else if (reportName.ToLower() == "mystery shopper  qar")
                {
                    return View(m_OpMysteryShopperQAR.EmbedConfig);
                }
                //Note: Head Office All Report
                else if (reportName.ToLower() == "finance")
                {
                    return View(m_FinanceReport.EmbedConfig);
                }
                else if (reportName.ToLower() == "marketing")
                {
                    return View(m_MarketingReport.EmbedConfig);
                }
                    //Note: Head Office Finance Report View
                    //Note: Head Office Finance All Report  
                    else if (reportName.ToLower() == "daily corp store stats report")
                    {
                        return View(m_HOFDCSSR.EmbedConfig);
                    }
                    else if (reportName.ToLower() == "sales by date by media type")
                    {
                        //embedResult = await m_HOFSBDBMT.EmbedReport(username, roles);
                        return View(m_HOFSBDBMT.EmbedConfig);
                    }
                    else if (reportName.ToLower() == "summary scorecard finance weekly")
                    {
                        //embedResult = await m_HOFSSFW.EmbedReport(username, roles);
                        return View(m_HOFSSFW.EmbedConfig);
                    }
                    else if (reportName.ToLower() == "gift card sales")
                    {
                        //embedResult = await m_HOFGCS.EmbedReport(username, roles);
                        return View(m_HOFGCS.EmbedConfig);
                    }
                    else if (reportName.ToLower() == "summary scorecard finance month and year")
                    {
                        //embedResult = await m_HOFSSFMY.EmbedReport(username, roles);
                        return View(m_HOFSSFMY.EmbedConfig);
                    }
                    else if (reportName.ToLower() == "lfl summary scorecard finance year")
                    {
                        //embedResult = await m_HOFLSSFY.EmbedReport(username, roles);
                        return View(m_HOFLSSFY.EmbedConfig);
                    }
                    else if (reportName.ToLower() == "lfl summary scorecard finance")
                    {
                        //embedResult = await m_HOFLSSF.EmbedReport(username, roles);
                        return View(m_HOFLSSF.EmbedConfig);
                    }
                    else if (reportName.ToLower() == "brewery sales trend")
                    {
                        //embedResult = await m_HOFBST.EmbedReport(username, roles);
                        return View(m_HOFBST.EmbedConfig);
                    }
                    //Note: Help Report View
                    else if (reportName.ToLower() == "dashboard help")
                {
                    return View(m_HelpReport.EmbedConfig);
                }
                //Note: Scorecard Report View
                else if (reportName.ToLower() == "scorecard summary")
                {
                    return View(m_SCSummary.EmbedConfig);
                }
                else if (reportName.ToLower() == "aggregated view")
                {
                    return View(m_SCAggView.EmbedConfig);
                }
                else
                {
                    return View(m_SCMonthlyRunRate.EmbedConfig);
                }
            }
            else
            {
                return View(m_embedService.EmbedConfig);
            }
        }
        }

        public async Task<ActionResult> EmbedDashboard()
        {
            var embedResult = await m_embedService.EmbedDashboard();
            if (embedResult)
            {
                return View(m_embedService.EmbedConfig);
            }
            else
            {
                return View(m_embedService.EmbedConfig);
            }
        }

        public async Task<ActionResult> EmbedTile()
        {
            var embedResult = await m_embedService.EmbedTile();
            if (embedResult)
            {
                return View(m_embedService.TileEmbedConfig);
            }
            else
            {
                return View(m_embedService.TileEmbedConfig);
            }
        }
    }
}
