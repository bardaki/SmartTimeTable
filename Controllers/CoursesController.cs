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
        private DAL dal = new DAL();
        private BL bl = new BL();
        //Set the index page with departments data
        public ActionResult Index()
        {
            var departments = dal.GetAllDepartments();
            var items = departments.Select(d => new SelectListItem() { Text = d.Name, Value = d.Id.ToString() }).ToList();
            //ViewBag.Departments = items;
            var ReturnModel = new DepartmentsModel(items);

            return View(ReturnModel);
        }

        //Get years
        public ActionResult GetYears(string d)
        {
            var depId = Convert.ToInt32(d);
            var years = dal.GetSpecializationsByDepartment(depId);

            return Json(years);
        }

        //Get courses by specializtion
        public ActionResult GetCoursesBySpecialization(string d)
        {
            var specId = Convert.ToInt32(d);
            var courses = dal.GetCoursesBySpecialization(specId);

            return Json(courses);
        }

        //Get all timetables
        public ActionResult GetTimeTables(TTRequest Request)
        {
            var Courses = Request.Courses;
            var Semester = Request.Semester;
            var semesterCourses = dal.GetSemesterCourses(Courses, Semester);
            var link = dal.GetLink();
            if (semesterCourses == null)
                return null;

            var NumOfDays = Request.NumOfDays;
            var HourLimits = Request.HourLimits;
            var Days = Request.Days;
            var SortBy = Request.SortBy;
            List<TimeTable> TimeTables = bl.GetTimeTable(semesterCourses, link, NumOfDays, HourLimits, Days, SortBy);

            var jsonResult = Json(TimeTables, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        //Search course
        public ActionResult SearchCourse(int CourseCode)
        {
            var course = dal.SearchCourse(CourseCode);
            return Json(course);
        }

        //Get specific course
        public List<CoursesDetails> GetCourses(int groupCode, int semester)
        {
            return dal.GetCourses(groupCode, semester);
        }
    }
}
