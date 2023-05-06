using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Service;
using IB_projekat.PaginatedResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace IB_projekat.Certificates.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;
        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        /*[HttpGet]
        public async Task<IEnumerable<Certificate>> GetAll()
        {
            return await _certificateService.GetAll();
        }*/

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<Certificate>>> GetAllCertificatesPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var certificates = await _certificateService.GetAllCertificatesPaginated(page, pageSize);
            var total = certificates.Count();
            var response = new PaginationResponse<Certificate>(certificates, page, pageSize, total);
            return Ok(response);
        }

        [HttpGet("validate/{serialNumber}")]
        public async Task<bool> ValidateCert(string serialNumber)
        {
            return await _certificateService.ValidateCert(serialNumber);
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateCertFile([FromBody] byte[] certificateBytes)
        {
            X509Certificate2 certificate = new X509Certificate2(certificateBytes);

             return Ok(await _certificateService.ValidateCertFile(certificate));
        }

        [HttpDelete("revoke/{serialNumber}")]
        public async Task<bool> RevokeCertFile(string serialNumber)
        {

            return await _certificateService.RevokeCert(serialNumber);
        }



    }
}
