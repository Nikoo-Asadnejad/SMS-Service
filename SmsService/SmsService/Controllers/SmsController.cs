using Microsoft.AspNetCore.Mvc;
using SmsService.Dtos.Sms;
using SmsService.Interfaces;

namespace SmsService.Controllers
{
  public class SmsController : Controller
  {
    private readonly ISendSmsService _sendSmsService;
    public SmsController(ISendSmsService sendSmsService)
    {
      _sendSmsService = sendSmsService;
    }

    [HttpPost]
    [Route("api/v1/sms/send")]
    public async Task<IActionResult> SendSms([FromBody]SmsInputDto smsInputDto)
    {
      var result = await _sendSmsService.SendSmsAsync(smsInputDto);
      return StatusCode((int)result.HttpStatusCode, result);
    }
  }
}
