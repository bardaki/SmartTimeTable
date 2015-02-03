using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class TTRequest
    {
        public List<Course> Courses { get; set; }
        public List<DayLimit> Days { get; set; }
        public List<HourLimit> HourLimits { get; set; }
        public int NumOfDays { get; set; }
        public int Semester { get; set; }
        public int SortBy { get; set; }
    }

    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectCode { get; set; }
    }

    public class DayLimit
    {
        public string Day { get; set; }
        public bool Checked { get; set; }
    }

    public class HourLimit
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string Day { get; set; }
        public bool Checked { get; set; }
    }
}