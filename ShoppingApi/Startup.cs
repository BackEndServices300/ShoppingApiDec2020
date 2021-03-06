using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ShoppingApi.Data;
using ShoppingApi.Profiles;
using ShoppingApi.Services;

namespace ShoppingApi
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
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(Configuration.GetSection("seq"));
            });
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddDbContext<ShoppingDataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("shopping"))
            );
            services.AddScoped<ILookupProducts, EfSqlShopping>();
            services.AddScoped<IProductCommands, EfSqlShopping>();


            var pricingConfig = new PricingConfiguration();
            Configuration.GetSection(pricingConfig.SectionName).Bind(pricingConfig);
            // Makes this injectable into services using IOptions<T>
            services.Configure<PricingConfiguration>(Configuration.GetSection(pricingConfig.SectionName));


            var pickupConfig = new PickupEstimatorConfiguration();
            Configuration.GetSection(pickupConfig.SectionName).Bind(pickupConfig);
            services.Configure<PickupEstimatorConfiguration>(Configuration.GetSection(pickupConfig.SectionName));

            var mapperConfig = new MapperConfiguration(opt =>
            {
                opt.AddProfile(new ProductProfile(pricingConfig));
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
            services.AddSingleton<MapperConfiguration>(mapperConfig);

            
            services.AddScoped<ICurbsideCommands, EfSqlAsyncCurbside>();
            services.AddScoped<ICurbsideLookups, EfSqlAsyncCurbside>();
            services.AddSingleton<CurbsideChannel>();

            services.AddScoped<IGenerateCurbsidePickupTimes, GrpcPickupEstimator>(); // TODO: Should this be a singleton?

            services.AddHostedService<CurbsideOrderProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
