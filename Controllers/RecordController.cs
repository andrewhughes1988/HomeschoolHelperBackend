using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HomeschoolHelperApi.Services.RecordService;
using System.Threading.Tasks;
using HomeschoolHelperApi.DTOs.Records;
using Microsoft.AspNetCore.Authorization;


namespace HomeschoolHelperApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService _recordService;

        public RecordController(IRecordService recordService)
        {
            this._recordService = recordService;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll() 
        {
            return Ok(await _recordService.GetAllRecords());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id) 
        {
            return Ok(await _recordService.GetRecordById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(AddRecordDTO newRecord) 
        {
            
            return Ok(await _recordService.AddRecord(newRecord));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRecord(UpdateRecordDTO updatedRecord) 
        {
            ServerResponse<GetRecordDTO> response = await _recordService.UpdateRecord(updatedRecord);
            
            if(response.Data == null) 
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            ServerResponse<List<GetRecordDTO>> response = await _recordService.DeleteRecord(id);
            
            if(response.Data == null) 
                return NotFound(response);

            return Ok(response);
        }
        
    }
}