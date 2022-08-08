using Microsoft.AspNetCore.Mvc;
using SmsService.Dtos.Sms;
using SmsService.Dtos.User;
using SmsService.Interfaces;
using SmsService.Percistance;

namespace SmsService.Controllers
{
  public class TestController : Controller
  {
    [HttpGet]
    [Route("/Test")]
    public async Task<IActionResult> Test([FromServices]ISmsService smsService)
    {

      SmsInputDto input = new SmsInputDto(BaseData.SmsTypes.OPT.Id, "123",
        BaseData.Providers.KaveNegar.Id,
        new UserDto(1, "nikoo", "asad", "09393701422"));

      var r = await smsService.CreateSmsAsync(input);

      return Ok();
    }
  }
}
