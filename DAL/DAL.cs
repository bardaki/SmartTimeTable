using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.DAL
{
    public class DAL  //static
    {
        //Get all departments
        public List<Departments> GetAllDepartments()
        {
            using (var ctx = new CoursesEntities())
            {
                return ctx.Departments.ToList();
            }
        }

        //Get all courses by specializtion
        public object GetCoursesBySpecialization(int specId)
        {
            using (var ctx = new CoursesEntities())
            {
                //ctx.Configuration.ProxyCreationEnabled = false;
                var result = (from c in ctx.Courses
                              from s in c.Specializtions
                              where s.Id == specId
                              select new { c.Id, c.Name, c.SubjectCode }).ToList();
                
                return result;
            }
        }
        
        //Get all specializations by department
        public List<Specializtions> GetSpecializationsByDepartment(int depId)
        {
            using (var ctx = new CoursesEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var spec = ctx.Specializtions.Where(n => n.DepId == depId).ToList();
                //spec.OrderBy(s => s.);
                return spec;
            }
        }

        //Get all courses that appears in current semester by user course choose
        public List<CoursesDetails> GetSemesterCourses(List<Course> courses, int semester)
        {
            if (courses == null)
                return null;
            using (var ctx = new CoursesEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var semesterCourses = new List<CoursesDetails>();
                TimeSpan t = new TimeSpan(10,0,0);
                foreach (var c in courses)
                {
                    semesterCourses.AddRange(ctx.CoursesDetails.
                        Where(s => s.SubjectCode == c.SubjectCode && s.Semester == semester).ToList()); 
                    //semesterCourses.AddRange(ctx.SummerSemester.
                    //    Where(s => s.SubjectDescription == c.Name).ToList()); 
                }
               
                semesterCourses.GroupBy(p => p.SubjectCode, p => p.SubjectDescription,
                         (key, g) => new { SubjectCode = key, Cuorses = g.ToList() });

                int Count = 0;
                List<string> Names = new List<string>();
                foreach(var c in semesterCourses)
                    if (!Names.Contains(c.SubjectDescription))
                    {
                        Names.Add(c.SubjectDescription);
                        Count++;
                    }
                if (Count != courses.Count())
                    return null;
                return semesterCourses;               
            }
        }

        //Search course by course code
        public List<Courses> SearchCourse(int courseCode)
        {
            using (var ctx = new CoursesEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var course = ctx.Courses.Where(c => c.SubjectCode == courseCode).ToList();
                
                if(course.Count > 0 )
                    return course;
                else
                    return null;
            }           
        }

        //Get link table
        public List<LinkTable> GetLink()
        {
            using (var ctx = new CoursesEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var linkTable = ctx.LinkTable.ToList();

                return linkTable;
            }
        }

        //Get specific course
        public List<CoursesDetails> GetCourses(int groupCode, int semester)
        {
            using (var ctx = new CoursesEntities())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                var courses = ctx.CoursesDetails.Where(c => c.GroupCode == groupCode && c.Semester == semester).ToList();

                return courses;
            }
        }
    }
}