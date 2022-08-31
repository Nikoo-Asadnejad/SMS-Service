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
    private readonly ISmsService _smsService;
    public SmsController(ISendSmsService sendSmsService , ISmsService smsService)
    {
      _sendSmsService = sendSmsService;
      _smsService = smsService;
    }


    /// <summary>
    /// Sends SMS by provider choosen by user 
    /// </summary>
    /// <param name="smsInputDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/v1/smses/send")]
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


    /// <summary>
    /// Sends OPT SMS by provider choosen by user 
    /// </summary>
    /// <param name="smsInputDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/v1/smses/send-opt")]
    [ProducesResponseType(typeof(ReturnModel<SendSmsReturnDto>), 200)]
    [ProducesResponseType(typeof(ReturnModel<SendSmsReturnDto>), 400)]
    [ProducesResponseType(typeof(ReturnModel<SendSmsReturnDto>), 500)]
    public async Task<IActionResult> SendOPTSms([FromBody] LoginSmsInputDto smsInputDto)
    {

      if (!ModelState.IsValid)
      {
        var errors = ModelState.GetModelErrors();
        return StatusCode(400, new ReturnModel<SendSmsReturnDto>(title: null, data: null,
          HttpStatusCode.BadRequest, message: ReturnMessage.InvalidInputDataErrorMessage,
          fieldErrors: errors));
      }

      ReturnModel<SendSmsReturnDto> result = await _sendSmsService.SendLoginCode(smsInputDto);
      return StatusCode((int)result.HttpStatusCode, result);
    }

    /// <summary>
    /// Gets SMS with given id
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("api/v1/smses/{id}")]
    [ProducesResponseType(typeof(ReturnModel<SmsReturnDto>), 200)]
    [ProducesResponseType(typeof(ReturnModel<SmsReturnDto>), 400)]
    [ProducesResponseType(typeof(ReturnModel<SmsReturnDto>), 500)]
    public async Task<IActionResult> GetSms([FromRoute] string id)
    {
      ReturnModel<SmsReturnDto> result = await _smsService.GetSmsAsync(id);
      return StatusCode((int)result.HttpStatusCode, result);
    }
  }
}
