using IB_projekat.Certificates.DTOS;
using IB_projekat.Requests.Model;
using IB_projekat.Requests.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace IB_projekat.Requests.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRequest(RequestDTO requestDTO)
        {
            if(requestDTO.SignitureSerialNumber == null && requestDTO.CertificateType != Certificates.Model.CertificateType.Root)
            {
                return BadRequest("Certificate needs to be signed by a serial number!!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _requestService.Create(requestDTO);
            return Ok();
        }

        [HttpPut("accept/{certId}")]
        public async Task<IActionResult> AcceptRequest(int certId)
        {
            await _requestService.Accept(certId);
            return Ok();
        }

        [HttpPut("decline/{certId}")]
        public async Task<IActionResult> DeclineRequest(int certId)
        {
            await _requestService.Decline(certId);
            return Ok();
        }
        [HttpGet("{userId}")]
        public async Task<IEnumerable<Request>> GetByUserId(int userId)
        {
            return await _requestService.GetByUserId(userId);
        }

    }
}
