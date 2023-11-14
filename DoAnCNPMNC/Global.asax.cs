using DoAnCNPMNC.Controllers;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DoAnCNPMNC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        [Obsolete]
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var entityFrameworkConnectionString = ConfigurationManager.ConnectionStrings["DBRedNitEntities"].ConnectionString;
            var efBuilder = new EntityConnectionStringBuilder(entityFrameworkConnectionString);

            var sqlServerConnectionString = new SqlConnectionStringBuilder(efBuilder.ProviderConnectionString).ToString();

            // Cấu hình Hangfire để sử dụng SQL Server
            GlobalConfiguration.Configuration.UseSqlServerStorage(sqlServerConnectionString);

            // Khởi tạo Worker
            var options = new BackgroundJobServerOptions { WorkerCount = 1 };
            var server = new BackgroundJobServer(options);

            // Đăng ký công việc cập nhật trạng thái chuyến bay
            RecurringJob.AddOrUpdate(() => new ChuyenBaysController().UpdateChuyenBayStatusJob(), Cron.MinuteInterval(5));
        }
    }
}
