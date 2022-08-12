using ErrorHandlingDll.Extensions;
using ErrorHandlingDll.ReturnTypes;
using Microsoft.AspNetCore.Mvc;
using SmsService.Dtos.Sms;
using SmsService.Interfaces;
using System.Net;

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
    [ProducesResponseType(typeof(ReturnModel<SendSmsReturnDto>), 200)]
    [ProducesResponseType(typeof(ReturnModel<SendSmsReturnDto>), 400)]
    [ProducesResponseType(typeof(ReturnModel<SendSmsReturnDto>), 500)]
    public async Task<IActionResult> SendSms([FromBody]SmsInputDto smsInputDto)
    {

      if(!ModelState.IsValid)
      {
        var errors = ModelState.GetModelErrors();
        return StatusCode(400, new ReturnModel<SendSmsReturnDto>(title : null ,data: null ,
          HttpStatusCode.BadRequest, message: ReturnMessage.InvalidInputDataErrorMessage ,
          fieldErrors: errors));
      }

      ReturnModel<SendSmsReturnDto> result = await _sendSmsService.SendSmsAsync(smsInputDto);
      return StatusCode((int)result.HttpStatusCode, result);
    }
  }
}
