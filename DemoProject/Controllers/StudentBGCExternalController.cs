using DemoProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace DemoProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentBGCExternalController : ControllerBase
    {
        private readonly IStudentBGCService _studentBGCService;

        public StudentBGCExternalController(IStudentBGCService StudentBGCService)
        {
            _studentBGCService = StudentBGCService;
        }

        [HttpGet("backgroundcheck")]
        public async Task<IActionResult> Get()
        {
            //StudentBGCApi client is an external service that does back ground verification
            return Ok(await _studentBGCService.GetBGCFromExternalApiAsync());
        }
    }
}


