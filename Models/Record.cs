using System;
using System.ComponentModel.DataAnnotations;


namespace HomeschoolHelperApi.Models

{
    public class Record
    {
        
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 0;
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}