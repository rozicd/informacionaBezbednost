using IB_projekat.Certificates.DTOS;
using IB_projekat.PaginatedResponseModel;
using IB_projekat.Requests.Model;
using IB_projekat.Requests.Service;
using IB_projekat.tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace IB_projekat.Requests.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly RecaptchaVerifier _recaptchaVerifier;
        private readonly Serilog.ILogger _logger;

        public RequestController(IRequestService requestService, Serilog.ILogger logger)
        {
            _requestService = requestService;
            _recaptchaVerifier = new RecaptchaVerifier(Environment.GetEnvironmentVariable("BACK_RECAPTCHA"));
            _logger = logger;
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public async Task<IActionResult> AddRequest(RequestDTO requestDTO)
        {
            if (!await _recaptchaVerifier.VerifyRecaptcha(requestDTO.RecaptchaToken))
            {
                _logger.Warning("Recaptcha is not valid!");
                return BadRequest("Recaptcha is not valid!");
            }

            if (requestDTO.SignitureSerialNumber == "" && requestDTO.CertificateType != Certificates.Model.CertificateType.Root)
            {
                _logger.Warning("Certificate needs to be signed by a serial number!!");
                return BadRequest("Certificate needs to be signed by a serial number!!");
            }

            if (!ModelState.IsValid)
            {
                _logger.Warning("Not all fields are satisfied!");
                return BadRequest("Not all fields are satisfied!");
            }

            try
            {
                await _requestService.Create(requestDTO);
                _logger.Information("Request created successfully.");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to create request.");
                return BadRequest(e.Message);
            }
        }

        [HttpPut("accept/{certId}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public async Task<IActionResult> AcceptRequest(int certId)
        {
            try
            {
                await _requestService.Accept(certId);
                _logger.Information("Request accepted successfully - Certificate ID: {CertificateId}", certId);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to accept request - Certificate ID: {CertificateId}", certId);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("decline/{certId}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public async Task<IActionResult> DeclineRequest(int certId)
        {
            try
            {
                await _requestService.Decline(certId);
                _logger.Information("Request declined successfully - Certificate ID: {CertificateId}", certId);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to decline request - Certificate ID: {CertificateId}", certId);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{userId}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public async Task<ActionResult<PaginationResponse<Request>>> GetByUserId(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var request = await _requestService.GetRequestsByCertificateSerialNumber(userId, page, pageSize);
                _logger.Information("Requests retrieved successfully for User ID: {UserId}", userId);
                return Ok(request);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to retrieve requests for User ID: {UserId}", userId);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("all")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public async Task<ActionResult<PaginationResponse<Request>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var request = await _requestService.GetAll(page, pageSize);
                _logger.Information("All requests retrieved successfully");
                return Ok(request);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to retrieve all requests");
                return BadRequest(e.Message);
            }
        }
    }
}
