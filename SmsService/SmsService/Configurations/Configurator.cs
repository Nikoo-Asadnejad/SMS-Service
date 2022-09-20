using DocumentFormat.OpenXml.EMMA;
using ErrorHandlingDll.Configurations;
using HttpService.Configuration;
using Microsoft.OpenApi.Models;
using MongoRepository.Configurations;
using SmsService.DataAccess.Repository;
using SmsService.Interfaces;
using SmsService.Services;

namespace SmsService.Configurations
{
  public static class Configurator
  {
    public static void InjectServices(IServiceCollection services, IConfiguration configuration)
    {

      services.AddControllers();
      services.AddEndpointsApiExplorer();

      services.AddSwaggerGen(c =>
      {
        var filePath = Path.Combine(AppContext.BaseDirectory, "SMSService.xml");
        c.IncludeXmlComments(filePath);
      });

      services.Configure<AppSetting>(configuration);

      ErrorHandlingDllConfigurator.InjectServices(services, configuration);
      HttpServiceConfigurator.InjectHttpService(services);
      MongoRepositoryDllConfigurator.InjectServices(services);

      services.AddScoped<ISmsService, SmsService.Services.SmsService>();
      services.AddScoped<ISendSmsService, KaveNegarService>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();

    }

    public static void ConfigPipeLines(WebApplication app)
    {

    //  ErrorHandlingDllConfigurator.ConfigureAppPipeline(app);
      app.UseHttpsRedirection();
      app.UseRouting();   
      app.UseAuthorization();  
      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
      app.MapControllers();
      


      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI(c => {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sms-Service API's");
        });
      }

      app.Run();
    }


  }

}
