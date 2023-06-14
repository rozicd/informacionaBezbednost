using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Service;
using IB_projekat.PaginatedResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace IB_projekat.Certificates.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;
        private readonly ILogger _logger;

        public CertificateController(ILogger logger, ICertificateService certificateService)
        {
            _logger = logger;
            _certificateService = certificateService;
        }

        /*[HttpGet]
        public async Task<IEnumerable<Certificate>> GetAll()
        {
            return await _certificateService.GetAll();
        }*/

        [HttpGet("download/{serialNumber}")]
        [Authorize(Policy = "AuthorizedOnly")]
        public async Task<IActionResult> DownloadCertificate(string serialNumber)
        {
            _logger.Information("Certificate download requested - Serial Number: {SerialNumber}", serialNumber);

            Certificate cert = await _certificateService.GetCertificateBySerialNumber(serialNumber);
            if (cert == null)
            {
                _logger.Warning("Certificate not found - Serial Number: {SerialNumber}", serialNumber);
                return BadRequest("No such certificate exists");
            }

            // Find the certificate file
            string certFilePath = $"certs/{serialNumber}.crt";
            if (!System.IO.File.Exists(certFilePath))
            {
                _logger.Warning("Certificate file not found - Serial Number: {SerialNumber}", serialNumber);
                return BadRequest("Certificate not found");
            }

            // Return the certificate file as a response
            FileStream fileStream = new FileStream(certFilePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/x-x509-ca-cert")
            {
                FileDownloadName = $"{serialNumber}.crt"
            };
        }
        [Authorize(Policy = "AuthorizedOnly")]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<Certificate>>> GetAllCertificatesPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.Information("Get all certificates paginated - Page: {Page}, PageSize: {PageSize}", page, pageSize);

            var certificates = await _certificateService.GetAllCertificatesPaginated(page, pageSize);
            var total = certificates.Count();
            var response = new PaginationResponse<Certificate>(certificates, page, pageSize, total);

            _logger.Information("Returned paginated certificates - Total: {Total}", total);
            return Ok(response);
        }

        [Authorize(Policy = "AuthorizedOnly")]
        [HttpGet("validate/{serialNumber}")]
        public async Task<bool> ValidateCert(string serialNumber)
        {
            _logger.Information("Certificate validation requested - Serial Number: {SerialNumber}", serialNumber);

            return await _certificateService.ValidateCert(serialNumber);
        }

        [Authorize(Policy = "AuthorizedOnly")]
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateCertFile([FromBody] byte[] certificateBytes)
        {
            _logger.Information("Certificate file validation requested");

            X509Certificate2 certificate = new X509Certificate2(certificateBytes);

            return Ok(await _certificateService.ValidateCertFile(certificate));
        }

        [HttpDelete("revoke/{serialNumber}")]
        [Authorize(Policy = "AuthorizedOnly")]
        public async Task<bool> RevokeCertFile(string serialNumber)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            _logger.Information("Certificate revocation requested - Serial Number: {SerialNumber}, User Email: {UserEmail}", serialNumber, userEmail);

            return await _certificateService.RevokeCert(serialNumber, userEmail);
        }
    }
}
