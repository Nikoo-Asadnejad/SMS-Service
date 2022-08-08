using ErrorHandlingDll.Configurations;
using HttpService.Configuration;
using MongoRepository.Configurations;
using SmsService.Interfaces;
using SmsService.Services;

namespace SmsService.Configurations
{
  public static class Configurator
  {
    public static void InjectServices(IServiceCollection services, IConfiguration configuration)
    {

      services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();

      services.Configure<AppSetting>(configuration);

      ErrorHandlingDllConfigurator.InjectServices(services, configuration);
      HttpServiceConfigurator.InjectHttpService(services);
      MongoRepositoryDllConfigurator.InjectServices(services);

      services.AddScoped<ISmsService, SmsService.Services.SmsService>();
      services.AddScoped<ISendSmsService, KaveNegarService>();

    }

    public static void ConfigPipeLines(WebApplication app)
    {
      

      app.UseHttpsRedirection();
      //ErrorHandlingDllConfigurator.ConfigureAppPipeline(app);
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
