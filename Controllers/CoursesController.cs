using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.DAL;

namespace SmartTimeTable.Controllers
{
    public class CoursesController : Controller
    {
        private BL bl = new BL();

        //Set the index page with departments data
        public ActionResult Index()
        {
            var items = bl.GetAllDepartments();
            //var departments = dal.GetAllDepartments();
            //var items = departments.Select(d => new SelectListItem() { Text = d.Name, Value = d.Id.ToString() }).ToList();
            //ViewBag.Departments = items;
            var ReturnModel = new DepartmentsModel(items);

            return View(ReturnModel);
        }

        //Get years
        public ActionResult GetYears(string d)
        {
            var depId = Convert.ToInt32(d);
            var years = bl.GetSpecializationsByDepartment(depId);

            return Json(years);
        }

        //Get courses by specializtion
        public ActionResult GetCoursesBySpecialization(string d)
        {
            var specId = Convert.ToInt32(d);
            var courses = bl.GetCoursesBySpecialization(specId);

            return Json(courses);
        }

        //Get all timetables
        public ActionResult GetTimeTables(TTRequest request)
        {
            var Courses = request.Courses;
            var Semester = request.Semester;
            var semesterCourses = bl.GetSemesterCourses(Courses, Semester);
            var link = bl.GetLink();
            if (semesterCourses == null)
                return null;

            var NumOfDays = request.NumOfDays;
            var HourLimits = request.HourLimits;
            var Days = request.Days;
            var SortBy = request.SortBy;
            List<TimeTable> TimeTables = bl.GetTimeTable(semesterCourses, link, NumOfDays, HourLimits, Days, SortBy);

            var jsonResult = Json(TimeTables, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        //Search course
        public ActionResult SearchCourse(int courseCode)
        {
            var course = bl.SearchCourse(courseCode);
            return Json(course);
        }

        //Get specific course
        public List<CoursesDetails> GetCourses(int groupCode, int semester)
        {
            return bl.GetCourses(groupCode, semester);
        }
    }
}
