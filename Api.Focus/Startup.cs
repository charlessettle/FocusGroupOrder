using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Api.Focus.Models;
using Api.Focus.Interfaces;
using Api.Focus.Services;

namespace Api.Focus
{
    public class Startup
    {
        /// <summary>
        /// connection string to BudBargains SQL Azure Database
        /// </summary>
        public static readonly string connectionString = "Server=tcp:focusbrandscharles.database.windows.net,1433;Initial Catalog=FocusBrandsGroupOrder;Persist Security Info=False;User ID=codechallengeadmin;Password=Focused@1!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFocusGroupOrderRepository, FocusGroupOrderRepository>();

            services.AddDbContext<FocusDbContext>(opt =>
                opt.UseSqlServer(connectionString,
                providerOptions => providerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(3), null))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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

            //Configure the HTTP request pipeline.
#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI();
#endif

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
