using AutoMapper;
using HomeschoolHelperApi.DTOs.Students;
using HomeschoolHelperApi.DTOs.Records;
using HomeschoolHelperApi.DTOs.Subjects;
using HomeschoolHelperApi.Models;
using HomeschoolHelperApi.DTOs.Users;

namespace HomeschoolHelperApi 
{

    public class AutoMapperProfile : Profile 
    {
        public AutoMapperProfile()
        {
            CreateMap<Record, GetRecordDTO>();
            CreateMap<AddRecordDTO, Record>();
            CreateMap<UpdateRecordDTO, Record>();

            CreateMap<Student, GetStudentDTO>();
            CreateMap<AddStudentDTO, Student>();
            CreateMap<UpdateStudentDTO, Student>();

            CreateMap<Subject, GetSubjectDTO>();
            CreateMap<AddSubjectDTO, Subject>();
            CreateMap<UpdateSubjectDTO, Subject>();

            CreateMap<UserLoginDTO, User>();
            CreateMap<User, UserRegisterDTO>();
            
        }
    }

}