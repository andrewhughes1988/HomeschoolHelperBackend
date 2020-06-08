using System;
using System.Linq;
using System.Collections.Generic;
using HomeschoolHelperApi.Models;
using Microsoft.AspNetCore.Mvc;
using HomeschoolHelperApi.Services.SubjectService;
using System.Threading.Tasks;
using HomeschoolHelperApi.DTOs.Subjects;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HomeschoolHelperApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SubjectController : ControllerBase
    {
        private ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            this._subjectService = subjectService;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _subjectService.GetAllSubjects());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id) 
        {
            return Ok(await _subjectService.GetSubjectById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(AddSubjectDTO newSubject) 
        {
            return Ok(await _subjectService.AddSubject(newSubject));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSubject(UpdateSubjectDTO updatedSubject) 
        {
            ServerResponse<GetSubjectDTO> response = await _subjectService.UpdateSubject(updatedSubject);
            
            if(response.Data == null) 
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            ServerResponse<List<GetSubjectDTO>> response = await _subjectService.DeleteSubject(id);
            
            if(response.Data == null) 
                return NotFound(response);

            return Ok(response);
        }
    }
}