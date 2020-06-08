using System;
using HomeschoolHelperApi.Models;

namespace HomeschoolHelperApi.DTOs.Subjects
{
    public class UpdateSubjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCore { get; set; }
    }
}