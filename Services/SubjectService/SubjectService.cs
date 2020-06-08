using System.Linq;
using System.Collections.Generic;
using HomeschoolHelperApi.Models;
using System.Threading.Tasks;
using HomeschoolHelperApi.DTOs.Subjects;
using AutoMapper;
using System;
using HomeschoolHelperApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HomeschoolHelperApi.Services.SubjectService
{

    public class SubjectService : ISubjectService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public SubjectService(IMapper mapper, DataContext context, IHttpContextAccessor httpContext)
        {
            this._httpContext = httpContext;
            this._context = context;
            this._mapper = mapper;
        }


        public async Task<ServerResponse<List<GetSubjectDTO>>> AddSubject(AddSubjectDTO newSubject)
        {
            Subject subject = _mapper.Map<Subject>(newSubject);
            ServerResponse<List<GetSubjectDTO>> response = new ServerResponse<List<GetSubjectDTO>>();

            try 
            {

                subject.User = await _context.Users.FirstOrDefaultAsync( user => user.Id == GetUserId());

                if(subject.User == null)
                {
                    response.Success = false;
                    response.Message = "Unauthorized user.";
                }
                else 
                {
                    await _context.Subjects.AddAsync(subject);
                    await _context.SaveChangesAsync();

                    List<Subject> subjects = await _context.Subjects.Where
                            (subject => subject.User.Id == GetUserId()).ToListAsync();

                    response.Data = subjects.Select(subject => _mapper.Map<GetSubjectDTO>(subject)).ToList();
                    response.Message = "Subject has been saved successfully.";
                }
                
            }
            catch(Exception e) 
            {
                response.Success = false;
                response.Message = "An error occured while saving your subject data.";
            }


            return response;
           
        }


        public async Task<ServerResponse<List<GetSubjectDTO>>> GetAllSubjects()
        {

            ServerResponse<List<GetSubjectDTO>> response = new ServerResponse<List<GetSubjectDTO>>();

            try
            {

                List<Subject> subjects = await _context.Subjects.Where
                        (subject => subject.User.Id == GetUserId()).ToListAsync();

                if(subjects.Count == 0) 
                {
                    response.Success = false;
                    response.Message = "No subject data has been saved for the current user.";
                }

                else
                {
                    response.Data = subjects.Select(subject => _mapper.Map<GetSubjectDTO>(subject)).ToList();
                    response.Message = "Subjects were retrieved successfully.";
                }

                
            }
            catch(Exception e) 
            {
                response.Success = false;
                response.Message = "An error occurred while retrieving the subject data.";
            }

            return response;
        }


        public async Task<ServerResponse<GetSubjectDTO>> GetSubjectById(int id)
        {
            ServerResponse<GetSubjectDTO> response = new ServerResponse<GetSubjectDTO>();

            try
            {
                Subject subject = await _context.Subjects.FirstOrDefaultAsync
                        (subject => subject.Id == id && subject.User.Id == GetUserId());


                if(subject == null)
                {
                    response.Success = false;
                    response.Message = "Error occurred while fetching subject data for current user.";
                }
                else 
                {
                    response.Data = _mapper.Map<GetSubjectDTO>(subject);
                    response.Message = "Subject was retrieved successfully.";
                }
            }
            catch(Exception e) 
            {
                response.Success = false;
                response.Message = "An error occured while fetching the subject data.";
            }
           

            return response;

        }


        public async Task<ServerResponse<GetSubjectDTO>> UpdateSubject(UpdateSubjectDTO updatedSubject)
        {
            ServerResponse<GetSubjectDTO> response = new ServerResponse<GetSubjectDTO>();

            try
            {
                Subject subject = await _context.Subjects.FirstOrDefaultAsync
                        (subject => subject.Id == updatedSubject.Id && subject.User.Id == GetUserId());

                if(subject == null) 
                {
                    response.Success = false;
                    response.Message = "User is not authorized to update this data.";
                }

                else 
                {
                    subject.Name = updatedSubject.Name;
                    subject.IsCore = updatedSubject.IsCore;

                    _context.Subjects.Update(subject);
                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetSubjectDTO>(subject);
                    response.Message = "Subject was updated successfully.";
                }
                
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error occurred when fetching subject data.";
            }


            return response;

        }


        public async Task<ServerResponse<List<GetSubjectDTO>>> DeleteSubject(int id)
        {
            ServerResponse<List<GetSubjectDTO>> response = new ServerResponse<List<GetSubjectDTO>>();

            try
            {
                Subject subject = await _context.Subjects.FirstAsync
                        (subject => subject.Id == id && subject.User.Id == GetUserId());

                if(subject == null) 
                {
                    response.Success = false;
                    response.Message = "User is not authorized to update this data.";
                }
                
                else 
                {

                    List<Record> records = await _context.Records.Where
                            (record => record.SubjectId == subject.Id).ToListAsync();

                    _context.RemoveRange(records);
                    _context.Subjects.Remove(subject);

                    await _context.SaveChangesAsync();

                    response.Data = _context.Subjects.Select(subject => _mapper.Map<GetSubjectDTO>(subject)).ToList();
                    response.Message = "Subject and related records were deleted successfully.";
                }
                
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "An error occurred while attempt to delete the subject data.";
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