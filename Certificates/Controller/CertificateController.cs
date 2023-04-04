using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
