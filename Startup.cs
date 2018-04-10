using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using rider_view_syntaxhighlightissues.Base;

namespace rider_view_syntaxhighlightissues
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddRazorOptions(razorViewEngineOptions =>
                {
                    razorViewEngineOptions.FileProviders.Clear();
                    razorViewEngineOptions.FileProviders.Add(new TestFileProvider(_hostingEnvironment));
                    razorViewEngineOptions.AreaViewLocationFormats.Clear();
                    razorViewEngineOptions.PageViewLocationFormats.Clear();
                    razorViewEngineOptions.ViewLocationFormats.Clear();
                    razorViewEngineOptions.ViewLocationExpanders.Add(new TestViewLocationExpander());

//                    razorViewEngineOptions.AreaViewLocationFormats.Add("/Views/{2}/Partials/{0}.cshtml");
                    razorViewEngineOptions.AreaViewLocationFormats.Add("/Views/{2}/{0}.cshtml");
//                    razorViewEngineOptions.ViewLocationFormats.Add("/Views/Partials/{0}.cshtml");
//                    razorViewEngineOptions.ViewLocationFormats.Add("/Views/{0}.cshtml");
                    razorViewEngineOptions.ViewLocationFormats.Add("/Views/{0}.cshtml");
                });
        }

        private IHostingEnvironment _hostingEnvironment;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}