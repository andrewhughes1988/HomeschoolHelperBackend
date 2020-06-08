using System.Collections.Generic;
using HomeschoolHelperApi.Models;
using System.Threading.Tasks;
using HomeschoolHelperApi.DTOs.Subjects;

namespace HomeschoolHelperApi.Services.SubjectService
{
    public interface ISubjectService
    {
         Task<ServerResponse<List<GetSubjectDTO>>> GetAllSubjects();
         Task<ServerResponse<GetSubjectDTO>> GetSubjectById(int id);
         Task<ServerResponse<List<GetSubjectDTO>>> AddSubject(AddSubjectDTO newSubject);
         Task<ServerResponse<GetSubjectDTO>> UpdateSubject(UpdateSubjectDTO updatedSubject);
         Task<ServerResponse<List<GetSubjectDTO>>> DeleteSubject(int id);
    }
}