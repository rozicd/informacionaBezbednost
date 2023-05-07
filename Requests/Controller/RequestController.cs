using IB_projekat.Certificates.DTOS;
using IB_projekat.PaginatedResponseModel;
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
        public async Task<ActionResult<PaginationResponse<Request>>> GetByUserId(int userId, [FromQuery] int page = 1, [FromQuery] int pageSzie = 10)
        {
            var request = await _requestService.GetRequestsByCertificateSerialNumber(userId, page, pageSzie);
            var total = request.Count();
            return Ok(new PaginationResponse<Request>(request, page, pageSzie, total));
        }
        [HttpGet("all")]
        public async Task<ActionResult<PaginationResponse<Request>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSzie = 10)
        {
            var request = await _requestService.GetAll(page, pageSzie);
            var total = request.Count();
            return Ok(new PaginationResponse<Request>(request, page, pageSzie, total));
        }

    }
}
