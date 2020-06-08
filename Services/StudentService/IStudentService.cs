using System.Collections.Generic;
using HomeschoolHelperApi.Models;
using System.Threading.Tasks;
using HomeschoolHelperApi.DTOs.Students;

namespace HomeschoolHelperApi.Services.StudentService
{
    public interface IStudentService
    {
         Task<ServerResponse<List<GetStudentDTO>>> GetAllStudents();
         Task<ServerResponse<GetStudentDTO>> GetStudentById(int id);
         Task<ServerResponse<List<GetStudentDTO>>> AddStudent(AddStudentDTO newStudent);
         Task<ServerResponse<GetStudentDTO>> UpdateStudent(UpdateStudentDTO updatedStudent);
         Task<ServerResponse<List<GetStudentDTO>>> DeleteStudent(int id);
    }
}