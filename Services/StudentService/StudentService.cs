using System.Linq;
using System.Collections.Generic;
using HomeschoolHelperApi.Models;
using System.Threading.Tasks;
using AutoMapper;
using HomeschoolHelperApi.DTOs.Students;
using System;
using HomeschoolHelperApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HomeschoolHelperApi.Services.StudentService
{
    public class StudentService : IStudentService
    {
        public IMapper _mapper { get; }
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public StudentService(IMapper mapper, DataContext context, IHttpContextAccessor httpContext)
        {
            this._httpContext = httpContext;
            this._context = context;
            this._mapper = mapper;
        }


        public async Task<ServerResponse<List<GetStudentDTO>>> AddStudent(AddStudentDTO newStudent)
        {
            Student student = _mapper.Map<Student>(newStudent);
            ServerResponse<List<GetStudentDTO>> response = new ServerResponse<List<GetStudentDTO>>();

            try 
            {
                student.User = await _context.Users.FirstOrDefaultAsync(user => user.Id == GetUserId());

                if(student.User == null)
                {
                    response.Success = false;
                    response.Message = "User has not been authorized to save data.";
                }

                else
                {
                    await _context.Students.AddAsync(student);
                    await _context.SaveChangesAsync();

                    List<Student> students = await _context.Students.Where
                            (student => student.UserId == GetUserId()).ToListAsync();

                    response.Data = students.Select(student => _mapper.Map<GetStudentDTO>(student)).ToList();
                    response.Message = "Student was saved successfully.";
                }
                    
            }

            catch(Exception e) 
            {
                response.Success = false;
                response.Message = "An error occured while saving student data.";
            }


            return response;

        }


        public async Task<ServerResponse<List<GetStudentDTO>>> GetAllStudents()
        {
            ServerResponse<List<GetStudentDTO>> response = new ServerResponse<List<GetStudentDTO>>();

            try
            {

                List<Student> students = await _context.Students.Where
                    (student => student.User.Id == GetUserId()).ToListAsync();


                if(students.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No student data found for current user.";
                }

                else 
                {
                    response.Data = students.Select(student => _mapper.Map<GetStudentDTO>(student)).ToList();
                    response.Message = "Students were retrieved successfully.";
                }

            }

            catch(Exception e)
            {
                response.Success = false;
                response.Message = "An error occurred while retrieving the data.";
            }
            

            return response;

        }


        public async Task<ServerResponse<GetStudentDTO>> GetStudentById(int id)
        {

            ServerResponse<GetStudentDTO> response = new ServerResponse<GetStudentDTO>();

            try
            {

                Student student = await _context.Students.FirstOrDefaultAsync
                    (student => student.Id == id && student.UserId == GetUserId());

                if(student == null)
                {
                    response.Success = false;
                    response.Message = "Student data could not be retrieved for current user.";
                }

                else
                {
                    response.Data = _mapper.Map<GetStudentDTO>(student);
                    response.Message = "Student was retrieved successfully.";
                }

            }

            catch(Exception e)
            {
                response.Success = false;
                response.Message = "An error occurred while retrieving the data.";
            }
            

            return response;

        }


        public async Task<ServerResponse<GetStudentDTO>> UpdateStudent(UpdateStudentDTO updatedStudent)
        {

            ServerResponse<GetStudentDTO> response = new ServerResponse<GetStudentDTO>();


            try
            {

                Student student = await _context.Students.FirstAsync
                        (student => student.Id == updatedStudent.Id && student.UserId == GetUserId());

                if(student == null)
                {

                    response.Success = false;
                    response.Message = "Student data could not be updated by the current user.";

                }

                else 
                {

                    student.Name = updatedStudent.Name;

                    _context.Students.Update(student);
                    await _context.SaveChangesAsync();


                    response.Data = _mapper.Map<GetStudentDTO>(student);
                    response.Message = "Student was updated successfully.";

                }
                
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Student does not exist.";
            }


            return response;

        }


        public async Task<ServerResponse<List<GetStudentDTO>>> DeleteStudent(int id)
        {

            ServerResponse<List<GetStudentDTO>> response = new ServerResponse<List<GetStudentDTO>>();

            try
            {

                Student student = await _context.Students.FirstAsync
                        (student => student.Id == id && student.UserId == GetUserId());

                if(student == null)
                {

                    response.Success = false;
                    response.Message = "Student data could not be deleted by the current user.";

                }

                else 
                {

                    List<Record> records = await _context.Records.Where(record => record.StudentId == student.Id).ToListAsync();

                    _context.Records.RemoveRange(records);
                    _context.Students.Remove(student);

                    await _context.SaveChangesAsync();


                    response.Data = _context.Students.Select(student => _mapper.Map<GetStudentDTO>(student)).ToList();
                    response.Message = "Student and related records were deleted successfully.";

                }

                
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "An error occurred while trying to delete student data.";
            }


            return response;

        }


        private int GetUserId()
        {
            int userId = int.Parse(_httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            return userId;
            
        }
    }
}