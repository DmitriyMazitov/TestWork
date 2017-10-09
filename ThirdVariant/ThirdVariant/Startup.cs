using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThirdVariant.DAL;

namespace ThirdVariant
{
    public static class Vars
    {
        public static object _lock = new object();
        public static bool IsRunOfGetContent { get; set; } = false;
        public static string hhApiUrl = "https://api.hh.ru/vacancies/";
        public static string uriString =
            "https://hh.ru/search/vacancy?text=&area=16&area=40&area=5&area=113&area=9&area=1001&salary=&currency_code=RUR&experience=doesNotMatter&order_by=relevance&search_period=&items_on_page=50&no_magic=true";

        public static string pgConnString = "PORT=5432;TIMEOUT=1024;POOLING=True;MINPOOLSIZE=1;MAXPOOLSIZE=100;COMMANDTIMEOUT=2400;USER ID=test_user;PASSWORD=test;HOST=10.250.2.35;DATABASE=test_db;ENLIST=True";

        public static void GetVacanciesFromHh()
        {
            if (!IsRunOfGetContent)
            {
                lock (_lock)
                {
                    IsRunOfGetContent = true;
                    GetAndStoreContent hhVd = new GetAndStoreContent(uriString, hhApiUrl, pgConnString);
                    bool hhVdResult = hhVd.StartProcess();
                    IsRunOfGetContent = false;
                }
            }
        }
            
    }
    public class Startup
    {        
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {     
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
