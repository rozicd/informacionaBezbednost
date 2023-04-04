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
            await _requestService.Create(requestDTO);
            return Ok();
        }

        [HttpPut("accept")]
        public async Task<IActionResult> AcceptRequest(RequestDTO requestDTO)
        {
            await _requestService.Accept(requestDTO);
            return Ok();
        }

        [HttpPut("decline")]
        public async Task<IActionResult> DeclineRequest(RequestDTO requestDTO)
        {
            await _requestService.Decline(requestDTO);
            return Ok();
        }
        [HttpGet("{userId}")]
        public async Task<IEnumerable<Request>> GetByUserId(int userId)
        {
            return await _requestService.GetByUserId(userId);
        }

    }
}
