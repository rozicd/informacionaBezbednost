using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Service;
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

        [HttpGet]
        public async Task<IEnumerable<Certificate>> GetAll()
        {
            return await _certificateService.GetAll();
        }
        [HttpGet("validate/{serialNumber}")]
        public async Task<bool> ValidateCert(string serialNumber)
        {
            return await _certificateService.ValidateCert(serialNumber);
        }
        [HttpPost("validate")]
        public async Task<bool> ValidateCertFile([FromBody] byte[] certificateBytes)
        {
            X509Certificate2 certificate = new X509Certificate2(certificateBytes);

            return await _certificateService.ValidateCertFile(certificate);
        }
    }
}
