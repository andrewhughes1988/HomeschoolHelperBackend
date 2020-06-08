using System;
using HomeschoolHelperApi.Models;
using System.ComponentModel.DataAnnotations;

namespace HomeschoolHelperApi.DTOs.Records
{
    public class UpdateRecordDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 0;

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}