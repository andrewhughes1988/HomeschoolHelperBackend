using System;
using System.ComponentModel.DataAnnotations;
using HomeschoolHelperApi.Models;

namespace HomeschoolHelperApi.DTOs.Records
{
    public class AddRecordDTO
    {
        
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 0;

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}