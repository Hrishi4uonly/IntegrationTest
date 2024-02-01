using DemoProject.Models;
using DemoProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IStudentBGCService _studentBGCService;


        //public StudentController(IStudentService studentService, IStudentBGCService StudentBGCService)
        //{
        //    _studentService = studentService;
        //    _studentBGCService = StudentBGCService;
        //}
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
           
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var value = await _studentService.GetStudentsAsync();
           
            if (value is null)
            {
                return NotFound("The item not found");
            }
            return Ok(value);
        }

        [HttpGet("GetAsync/{Id}")]
        public async Task<IActionResult> GetStudentAsync(int Id)
        {
            var result = await _studentService.GetStudentAsync(Id);
            if (result is null)
            {
                return NotFound("The item not found");
            }
            if (Id<=0 )
            {
                return BadRequest("Bad Request");
            }
            return Ok(result);
        }

        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync(Student student)
        {
           //var bgc= _studentBGCService.GetBGCFromExternalApiAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is Not Valid.");
            }
            //Checking from external BGC Service 
            //if (!bgc.IsCompleted)
            //{
            //    return BadRequest(" BGC Pending");
            //}

            if (await _studentService.AddStudentAsync(student))
            {
                return Ok("Done");
            }
            return BadRequest("Something went wrong please try again.");
        }

        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> EditStudentAsync(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is Not Valid.");
            }

            if (await _studentService.EditStudentAsync(student))
            {
                return Ok("Done");
            }
            return BadRequest("Something went wrong please try again.");
        }

        [HttpDelete("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            if (await _studentService.DeleteStudentAsync(Id))
            {
                return Ok("Done");
            }
            return BadRequest("Something went wrong please try again.");
        }
    }
}
