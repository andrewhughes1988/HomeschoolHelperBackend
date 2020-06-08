using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HomeschoolHelperApi.Models;
using HomeschoolHelperApi.Services.StudentService;
using System.Threading.Tasks;
using HomeschoolHelperApi.DTOs.Students;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HomeschoolHelperApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            this._studentService = studentService;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll() 
        {
            return Ok(await _studentService.GetAllStudents());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await _studentService.GetStudentById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(AddStudentDTO newStudent) 
        {
            return Ok(await _studentService.AddStudent(newStudent));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(UpdateStudentDTO updatedStudent) 
        {
           ServerResponse<GetStudentDTO> response = await _studentService.UpdateStudent(updatedStudent);
            
            if(response.Data == null) 
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            ServerResponse<List<GetStudentDTO>> response = await _studentService.DeleteStudent(id);
            
            if(response.Data == null) 
                return NotFound(response);

            return Ok(response);
        }
    }
}