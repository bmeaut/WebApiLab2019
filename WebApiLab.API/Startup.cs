using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebApiLab.BLL;
using WebApiLab.BLL.DTO;
using WebApiLab.DAL;

namespace WebApiLab.API
{
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
            services.AddDbContext<NorthwindContext>(o =>
                    o.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddControllers()
                /*.AddJsonOptions(o=> { o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; })*/;
            services.AddTransient<IProductService, ProductService>();
            services.AddAutoMapper(typeof(WebApiProfile));
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => false;
                options.Map<EntityNotFoundException>(
                    (ctx, ex) =>
                    {
                        var pd = StatusCodeProblemDetails.Create(StatusCodes.Status404NotFound);
                        pd.Title = ex.Message;
                        return pd;
                    }
                );
            });
            services.AddOpenApiDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseProblemDetails();
            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenApi(); // ez az MW szolgálja ki az OpenAPI JSON-t
            app.UseSwaggerUi3(); //ez az MW adja az OpenAPI felületet

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
