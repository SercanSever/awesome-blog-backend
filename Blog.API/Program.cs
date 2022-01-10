using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Blog.CORE.DependencyResolvers;
using Blog.CORE.Extensions;
using Blog.DAL.Context;
using Blog.DAL.Repository;
using Blog.Service.DependencyResolver.Autofac;
using Blog.Service.Infrastructure.AutoMapperProfiler;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init main...");
try
{
   var builder = WebApplication.CreateBuilder(args);


   //CORS
   builder.Services.AddCors(options =>
           {
              options.AddPolicy("BlogOrigin",
              builder =>
              {
                 builder.WithOrigins("http://localhost:4200")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
              });
           });

   //Context
   builder.Services.AddDbContext<BlogContext>(opt =>
           opt.UseSqlServer(builder.Configuration.GetConnectionString("BlogConStr"), b => b.MigrationsAssembly("Blog.DAL")));

   //AutoMapper
   var mappingConfig = new MapperConfiguration(mapper =>
   {
      mapper.AddProfile(new BusinessProfile());
   });
   IMapper autoMapper = mappingConfig.CreateMapper();

   //DI
   builder.Services.AddSingleton(autoMapper);

   //Autofac Ioc.
   builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
   builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new BlogModule()));
   builder.Services.ConfigureCaching();

   // NLog
   builder.Logging.ClearProviders();
   builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
   builder.Host.UseNLog();

   builder.Services.AddControllers().AddFluentValidation().AddNewtonsoftJson();
   builder.Services.AddEndpointsApiExplorer();

   //Swagger
   builder.Services.AddSwaggerGen(options =>
   {
      options.SwaggerDoc("v1", new OpenApiInfo
      {
         Version = "v1",
         Title = "Blog API",
         Description = "ASP.NET Core Web API for personal blog.",
         Contact = new OpenApiContact
         {
            Name = "My Github Profile",
            Url = new Uri("https://github.com/SercanSever")
         },
      });
   });

   var app = builder.Build();

   // Configure the HTTP request pipeline.
   if (app.Environment.IsDevelopment())
   {
      app.UseSwagger();
      app.UseSwaggerUI(options =>
      {
         options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
         options.RoutePrefix = string.Empty;
      });
   }
   app.ConfigureCustomExceptionMiddleware();

   app.UseCors("BlogOrigin");

   app.UseHttpsRedirection();
   app.UseAuthentication();
   app.UseAuthorization();

   app.MapControllers();

   app.Run();

}
catch (Exception exception)
{
   logger.Error(exception, "Stopped program..");
   throw;
}
finally
{
   NLog.LogManager.Shutdown();
}
