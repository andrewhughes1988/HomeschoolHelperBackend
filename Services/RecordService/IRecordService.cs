using System.Collections.Generic;
using System.Threading.Tasks;
using HomeschoolHelperApi.DTOs.Records;
using HomeschoolHelperApi.Models;

namespace HomeschoolHelperApi.Services.RecordService
{
    public interface IRecordService
    {
         Task<ServerResponse<List<GetRecordDTO>>> GetAllRecords();
         Task<ServerResponse<GetRecordDTO>> GetRecordById(int id);
         Task<ServerResponse<List<GetRecordDTO>>> AddRecord(AddRecordDTO newRecord);
         Task<ServerResponse<GetRecordDTO>> UpdateRecord(UpdateRecordDTO updatedRecord);
         Task<ServerResponse<List<GetRecordDTO>>> DeleteRecord(int id);
    }
}