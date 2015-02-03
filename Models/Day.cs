using FinalProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Day
    {
        public string DayName { get; set; }
        public List<CoursesDetails> Courses = new List<CoursesDetails>();

        //Add course to list of courses
        public void Add(CoursesDetails c)
        {
            Courses.Add(c);
        }

        //Sort list of courses
        public void Sort()
        {
            Courses.Sort((a, b) => a.StartHour.Value.Hours
                .CompareTo(b.StartHour.Value.Hours));
        }
    }
}