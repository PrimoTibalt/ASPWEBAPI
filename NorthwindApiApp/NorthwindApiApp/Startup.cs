using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Northwind.Services.Products;
using Northwind.Services.Context;
using System.Data.SqlClient;
using Northwind.Services.DataAccess.Services;

namespace NorthwindApiApp
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
            services.AddTransient<IProductManagementService, ProductManagementDataAccessService>();
            services.AddTransient<IProductCategoryManagementService, ProductCategoriesManagementDataAccessService>();
            services.AddTransient<IProductCategoryPicturesService, ProductCategoryPictureService>();
            services.AddTransient<Northwind.DataAccess.NorthwindDataAccessFactory, Northwind.DataAccess.SqlServerDataAccessFactory>();

            services.AddControllers();
            services.AddDbContext<NorthwindContext>(cont => cont.UseInMemoryDatabase("Northwind"));
            services.AddScoped((service) =>
            {
                var sqlConnection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                sqlConnection.Open();
                return sqlConnection;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
