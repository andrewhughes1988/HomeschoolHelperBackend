using System;
using HomeschoolHelperApi.Models;


namespace HomeschoolHelperApi.DTOs.Students
{
    public class UpdateStudentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}