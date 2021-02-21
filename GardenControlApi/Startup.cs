using AutoMapper;
using GardenControlApi.MapProfiles;
using GardenControlRepositories;
using GardenControlRepositories.Interfaces;
using GardenControlServices;
using GardenControlServices.Interfaces;
using GardenControlServices.MapProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GardenControlApi
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
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppSettingProfile());
                mc.AddProfile(new ControlDeviceProfile());
                mc.AddProfile(new ControlDeviceDtoProfile());
                mc.AddProfile(new DS18B20DtoProfile());
                mc.AddProfile(new AppSettingDtoProfile());
                mc.AddProfile(new MeasurementDtoProfile());
                mc.AddProfile(new MeasurementUnitDtoProfile());
                mc.AddProfile(new TaskScheduleProfile());
                mc.AddProfile(new TaskScheduleProfileDto());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
#if DEBUG
            services.AddDbContext<GardenControlContext>(
               options => options.UseSqlite(Configuration.GetConnectionString("GardenControlConnection"), builder =>
                   builder.MigrationsAssembly(typeof(Startup).Assembly.FullName)
            ));
#else
            services.AddDbContext<GardenControlContext>(
               options => options.UseSqlite(Environment.GetEnvironmentVariable("DATABASE_CONNECTIONSTRING"), builder =>
                   builder.MigrationsAssembly(typeof(Startup).Assembly.FullName)
            ));
#endif

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "GardenControlApi", 
                    Version = "v1",
                    Description = "An API that allows the monitor and control of devices plugged in to the GPIO Pins on a Raspberry Pi"});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddTransient<IAppSettingsRepository, AppSettingsRepository>();
            services.AddTransient<IAppSettingsService, AppSettingsService>();
            services.AddTransient<IControlDeviceRepository, ControlDeviceRepository>();
            services.AddTransient<IControlDeviceService, ControlDeviceService>();
            services.AddTransient<IMeasurementRepository, MeasurementRepository>();
            services.AddTransient<IMeasurementService, MeasurementService>();
            services.AddTransient<ITaskScheduleRepository, TaskScheduleRepository>();
            services.AddTransient<ITaskScheduleService, TaskScheduleService>();
            services.AddTransient<DS18B20Service>();
            services.AddTransient<RelayService>();
            services.AddTransient<FloatSensorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GardenControlApi v1"));
            //}

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GardenControlApi v1"));

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
