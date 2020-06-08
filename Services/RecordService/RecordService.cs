using System.Collections.Generic;
using System.Linq;
using HomeschoolHelperApi.Models;
using System.Threading.Tasks;
using HomeschoolHelperApi.DTOs.Records;
using AutoMapper;
using System;
using HomeschoolHelperApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HomeschoolHelperApi.Services.RecordService
{
    public class RecordService : IRecordService
    {

        private IMapper _mapper { get; }
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public RecordService(IMapper mapper, DataContext context, IHttpContextAccessor httpContext)
        {
            this._httpContext = httpContext;
            this._context = context;
            this._mapper = mapper;

        }

        public async Task<ServerResponse<List<GetRecordDTO>>> AddRecord(AddRecordDTO newRecord)
        {
            
            ServerResponse<List<GetRecordDTO>> response = new ServerResponse<List<GetRecordDTO>>();

            try
            {

                Student student = await _context.Students.FirstOrDefaultAsync
                        (student => student.Id == newRecord.StudentId && student.UserId == GetUserId());

                Subject subject = await _context.Subjects.FirstOrDefaultAsync
                        (subject => subject.Id == newRecord.SubjectId && subject.UserId == GetUserId());

                User user = await _context.Users.FirstOrDefaultAsync(user => user.Id == GetUserId());

                if (student == null || subject == null || user == null)
                {
                    
                    response.Success = false;
                    response.Message = "There was an error saving record data for current user.";

                }

                else 
                {

                    Record record = _mapper.Map<Record>(newRecord);
                    record.Student = student;
                    record.Subject = subject;
                    record.User = user;

                    await _context.Records.AddAsync(record);
                    await _context.SaveChangesAsync();

                    List<Record> records = await _context.Records.ToListAsync();

                    response.Data = records.Select(record => _mapper.Map<GetRecordDTO>(record)).ToList();
                    response.Message = "Record was saved successfully.";    

                }
                
                
            }

            catch (Exception e)
            {
                response.Message = "An error occured, record could not be saved.";
            }



            return response;

        }


        public async Task<ServerResponse<List<GetRecordDTO>>> GetAllRecords()
        {

            ServerResponse<List<GetRecordDTO>> response = new ServerResponse<List<GetRecordDTO>>();
            
            try
            {
                List<Record> records = await _context.Records.Where(record => record.User.Id == GetUserId()).ToListAsync();


                if(records.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No record data is saved for the current user.";
                }

                else 
                {
                    response.Data = records.Select(record => _mapper.Map<GetRecordDTO>(record)).ToList();
                    response.Message = "Records were retrieved successfully.";
                }

            }

            catch(Exception e) 
            {
                response.Success = false;
                response.Message = "An error occured while fetching the record data.";
            }
            

            return response;

        }


        public async Task<ServerResponse<GetRecordDTO>> GetRecordById(int id)
        {

            ServerResponse<GetRecordDTO> response = new ServerResponse<GetRecordDTO>();

            try
            {

                Record record = await _context.Records.FirstOrDefaultAsync
                        (record => record.Id == id && record.UserId == GetUserId());

                if(record == null)
                {
                    response.Success = false;
                    response.Message = "Record data did not exist for current user.";
                }

                else
                {

                    response.Data = _mapper.Map<GetRecordDTO>(record);
                    response.Message = "Record retrieved successfully.";

                }

            }

            catch
            {

                response.Success = false;
                response.Message = "An error occurred while retrieving the record data.";

            }
            

            return response;

        }


        public async Task<ServerResponse<GetRecordDTO>> UpdateRecord(UpdateRecordDTO updatedRecord)
        {

            ServerResponse<GetRecordDTO> response = new ServerResponse<GetRecordDTO>();


            try
            {

                Record record = await _context.Records.FirstOrDefaultAsync
                        (record => record.Id == updatedRecord.Id && record.UserId == GetUserId());

                if(record == null)
                {
                    response.Success = false;
                    response.Message = "Unable to update record for current user.";
                }

                else 
                {

                    record.StudentId = updatedRecord.StudentId;
                    record.SubjectId = updatedRecord.SubjectId;
                    record.Hours = updatedRecord.Hours;
                    record.Minutes = updatedRecord.Minutes;
                    record.Date = updatedRecord.Date;
                    record.Notes = updatedRecord.Notes;

                    _context.Records.Update(record);
                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetRecordDTO>(record);
                    response.Message = "Record was updated successfully.";

                }
                
            }

            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Record does not exist.";
            }


            return response;

        }


        public async Task<ServerResponse<List<GetRecordDTO>>> DeleteRecord(int id)
        {
            ServerResponse<List<GetRecordDTO>> response = new ServerResponse<List<GetRecordDTO>>();

            try
            {
                
                Record record = await _context.Records.FirstAsync
                        (record => record.Id == id && record.UserId == GetUserId());

                if(record == null)
                {
                    response.Success = false;
                    response.Message = "Unable to delete record data for current user.";
                }

                else 
                {

                    _context.Records.Remove(record);
                    await _context.SaveChangesAsync();

                    response.Data = _context.Records.Select(record => _mapper.Map<GetRecordDTO>(record)).ToList();
                    response.Message = "Record was deleted successfully.";

                }
                
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Record does not exist.";
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