namespace SmsService.Configurations.AppSettings
{
  public class AppSetting
  {

      public Logging Logging { get; set; }
      public Mongodb MongoDb { get; set; }
      public Kavenegar KaveNegar { get; set; }
      public Sentry Sentry { get; set; }
      public string AllowedHosts { get; set; }

  
  }

  public class Logging
  {
    public Loglevel LogLevel { get; set; }
  }

  public class Sentry
  {
    public string Dsn { get; set; }

  }
  public class Loglevel
  {
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
  }

  public class Mongodb
  {
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }

  public class Kavenegar
  {
    public string ApiKey { get; set; }
    public string Sender { get; set; }
  }
}
