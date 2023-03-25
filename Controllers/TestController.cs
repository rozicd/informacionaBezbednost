using Microsoft.AspNetCore.Mvc;

namespace InformacionaBezbednost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {

        [HttpGet("testovi")]
        public IActionResult GetKurcici()
        {
            List<String> testovi = new List<String>();
            testovi.Add("test1");
            testovi.Add("test2");
            testovi.Add("test3");
            testovi.Add("test4");
            return Ok(testovi);
        }
    }
}
