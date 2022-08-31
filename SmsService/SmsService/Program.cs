global using SmsService.Configurations.AppSettings;
using SmsService.Configurations;
using SmsService.Dtos.Sms;
using SmsService.Dtos.User;
using SmsService.Interfaces;
using SmsService.Percistance;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Configurator.InjectServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
Configurator.ConfigPipeLines(app);

var smsServce = app.Services.GetService<ISmsService>();

//SmsInputDto input = new SmsInputDto(BaseData.SmsTypes.OPT.Id , "123", 
//  BaseData.Providers.KaveNegar.Id,
//  new UserDto(1, "nikoo", "asad", "09393701422"));

//smsServce.CreateSmsAsync(input);





