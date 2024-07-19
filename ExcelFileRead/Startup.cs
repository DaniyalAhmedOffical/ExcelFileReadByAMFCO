using Microsoft.AspNetCore.Builder;

namespace ExcelFileRead
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC services
            services.AddMvc();

            // Add distributed memory cache for session
            //services.AddDistributedMemoryCache();

            // Configure session options
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1); // Set session timeout
            });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();


        }

       
    }

    
}
