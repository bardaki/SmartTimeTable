using FinalProject.DAL;
using SmartTimeTable.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Models
{
    //Enum for courseGroup colors
    enum Colors { Aqua, Aquamarine, Chartreuse, Coral, Crimson, DarkCyan, DarkSalmon, DarkSeaGreen,
            DarkTurquoise, DodgerBlue, Gold, GreenYellow, LightYellow, LightSteelBlue, MediumOrchid,
            Red, Salmon, SkyBlue, Yellow, YellowGreen }

    public class BL
    {
        private DAL.DAL dal = new DAL.DAL();

        //Find timeTables administration
        public List<TimeTable> GetTimeTable(List<CoursesDetails> semesterCourses, List<LinkTable> link, int NumOfDays, List<HourLimit> HourLimits, List<DayLimit> Days, int SortBy)
        {
            var Watch = Stopwatch.StartNew();
            List<List<CourseGroup>> CoursesByGroup = new List<List<CourseGroup>>();
            CoursesByGroup = ArrangeBySubjectCode(ArrangeByName(semesterCourses));
            AddDifferentIdPractices(CoursesByGroup, link);
            var TimeTbl = ArrangeCoursesByDay(CoursesByGroup);
            var TimeTablesConstraints = TTConstraints(TimeTbl, NumOfDays, HourLimits, Days);
            List<TimeTable> AllTimeTables = MakeTimeTable(TimeTablesConstraints);
            AllTimeTables = SortTT(AllTimeTables, SortBy);
            Watch.Stop();
            var Time = Watch.ElapsedMilliseconds;
            if (AllTimeTables.Count() == 0)
                return null;
            else
                return AllTimeTables;
        }

        //Arrange all courses by Name
        public List<List<CoursesDetails>> ArrangeByName(List<CoursesDetails> semesterCourses)
        {
            List<List<CoursesDetails>> AllCourses = new List<List<CoursesDetails>>();

            for (int i = 0; i < semesterCourses.Count(); i++)
            {
                var Contains = false;
                List<CoursesDetails> course = new List<CoursesDetails>();
                course.Add(semesterCourses[i]);
                var CourseName = semesterCourses[i].SubjectDescription;
                foreach (var l in AllCourses)
                {
                    if (l[0].SubjectDescription == CourseName)
                        Contains = true;
                }
                for (int j = i + 1; j < semesterCourses.Count() && !Contains; j++)
                {
                    var CurrentCourseName = semesterCourses[j].SubjectDescription;
                    if (CurrentCourseName != CourseName)
                    {
                        break;
                    }
                    else
                    {
                        course.Add(semesterCourses[j]);
                    }
                }
                if (!Contains)
                    AllCourses.Add(course);
            }

            return AllCourses;
        }
        
        //Arrange all courses by SubjectCode
        public List<List<CourseGroup>> ArrangeBySubjectCode(List<List<CoursesDetails>> allCourses)
        {
            List<List<CourseGroup>> CoursesByNameAndGroup = new List<List<CourseGroup>>();
            for (int i = 0; i < allCourses.Count(); i++)
            {
                List<CourseGroup> CourseGroup = new List<CourseGroup>();
                for (int j = 0; j < allCourses[i].Count(); j++)
                {
                    var Contains = false;
                    CourseGroup cg = new CourseGroup();
                    cg.coursesInGroup.Add(allCourses[i][j]);
                    var CourseCode = allCourses[i][j].GroupCode;
                    foreach (var l in CourseGroup)
                    {
                        if (l.coursesInGroup[0].GroupCode == CourseCode)
                            Contains = true;
                    }
                    for (int k = j + 1; k < allCourses[i].Count() && !Contains; k++)
                    {
                        var CurrentCourseCode = allCourses[i][k].GroupCode;
                        if (CurrentCourseCode != CourseCode)
                        {
                            break;
                        }
                        else
                        {
                            cg.coursesInGroup.Add(allCourses[i][k]);
                        }
                    }
                    if (!Contains)
                        CourseGroup.Add(cg);
                }
                CoursesByNameAndGroup.Add(CourseGroup);
            }
            CoursesByNameAndGroup.Sort((a, b) => a.Count().CompareTo(b.Count()));
            //Split courses with 2 practice
            foreach (var group in CoursesByNameAndGroup)
            {
                foreach (var cg in group.ToList())
                {
                    int counter = 0;
                    foreach (var c in cg.coursesInGroup)
                    {
                        if (c.OccupationDescription == "תרגול")
                        {
                            counter++;
                        }
                    }
                    if (counter > 1)
                    {
                        for (int i = 0; i < counter; i++)
                        {
                            var newGroup = new CourseGroup();
                            foreach (var c in cg.coursesInGroup.ToList())
                            {
                                if (c.OccupationDescription == "סופי-הרצאה")
                                {
                                    newGroup.coursesInGroup.Add(c);
                                }
                                else if (c.OccupationDescription == "תרגול" && !ContainsPractice(newGroup))
                                {
                                    newGroup.coursesInGroup.Add(c);
                                    cg.coursesInGroup.Remove(c);
                                }
                            }
                            group.Add(newGroup);
                        }
                        group.Remove(cg);
                    }
                }
            }

            //Set Color for each CourseGroup
            Random randomGen = new Random();
            Colors[] names = (Colors[])Enum.GetValues(typeof(Colors));
            List<Colors> SelectedColors = new List<Colors>();
            foreach (var Group in CoursesByNameAndGroup)
            {
                Colors randomColorName = names[randomGen.Next(names.Length)];
                while (SelectedColors.Contains(randomColorName))
                    randomColorName = names[randomGen.Next(names.Length)];
                foreach (var cg in Group)
                {
                    SelectedColors.Add(randomColorName);
                    foreach (var c in cg.coursesInGroup)
                        c.Color = randomColorName.ToString();
                }
            }
            return CoursesByNameAndGroup;
        }

        //Add practice with different id
        public List<List<CourseGroup>> AddDifferentIdPractices(List<List<CourseGroup>> groupCourses, List<LinkTable> link)
        {
            foreach (var l in groupCourses)
            {
                foreach (var c in l)
                {
                    if (c.coursesInGroup.Count() == 1)
                    {
                        foreach (var cl in link)
                        {
                            if (cl.PracticeCode == l[0].coursesInGroup[0].GroupCode && cl.Semester == l[0].coursesInGroup[0].Semester)
                            {
                                //var courses = dal.GetCourses((int)l[0].coursesInGroup[0].GroupCode, (int)cl.Semester);
                                int groupCode = (int)l[0].coursesInGroup[0].GroupCode;
                                int semester = (int)cl.Semester;
                                using (var ctx = new CoursesEntities())
                                {
                                    ctx.Configuration.LazyLoadingEnabled = false;
                                    var courses = ctx.CoursesDetails.Where(g => g.GroupCode == groupCode && g.Semester == semester && g.OccupationDescription == "סופי-הרצאה").ToList();
                                    foreach (var course in courses)
                                    {
                                        course.Color = c.coursesInGroup[0].Color;
                                        c.coursesInGroup.Add(course);
                                    }
                                }
                                
                                break;
                            }
                        }
                    }
                }
            }

            return groupCourses;
        }
        
        //Arrange courses by day
        public List<TimeTable> ArrangeCoursesByDay(List<List<CourseGroup>> coursesByGroup)
        {
            List<TimeTable> TimeTables = new List<TimeTable>();
            List<List<CourseGroup>> TtCourses = new List<List<CourseGroup>>();
            TtCourses = FindTT(coursesByGroup);
            //Set "week courses" to timeTables
            foreach (var tt in TtCourses)
            {
                TimeTable Tt = new TimeTable();
                foreach (var cg in tt)
                {
                    foreach (var c in cg.coursesInGroup)
                    {
                        switch (c.Day)
                        {
                            case ("א"):
                                Tt.Sunday.Add(c);
                                break;
                            case ("ב"):
                                Tt.Monday.Add(c);
                                break;
                            case ("ג"):
                                Tt.Tuesday.Add(c);
                                break;
                            case ("ד"):
                                Tt.Wednesday.Add(c);
                                break;
                            case ("ה"):
                                Tt.Thursday.Add(c);
                                break;
                            case ("ו"):
                                Tt.Friday.Add(c);
                                break;
                        }
                    }
                }
                Tt.Sunday.Sort();
                Tt.Monday.Sort();
                Tt.Tuesday.Sort();
                Tt.Wednesday.Sort();
                Tt.Thursday.Sort();
                Tt.Friday.Sort();

                TimeTables.Add(Tt);
            }
            return TimeTables;
        }
        
        //Check if selected course appropriate with previous selected courses
        public bool CheckCourse(CourseGroup cg, List<CourseGroup> stack)
        {
            foreach (var c in cg.coursesInGroup)
            {
                foreach (var group in stack)
                {
                    foreach (var course in group.coursesInGroup)
                    {
                        if (c.Day == course.Day && ((c.StartHour >= course.StartHour && c.StartHour <= course.EndHour) ||
                            (c.EndHour >= course.StartHour && c.EndHour <= course.EndHour)))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        
        //Main algorithm to find timeTables- Genetic Algorithm
        public List<List<CourseGroup>> FindTT(List<List<CourseGroup>> coursesByGroup)
        {
            List<List<CourseGroup>> MainList = new List<List<CourseGroup>>();
            List<List<CourseGroup>> SubList = new List<List<CourseGroup>>();

            if (coursesByGroup.Count() == 1)
            {
                foreach (var cg in coursesByGroup[0])
                {
                    List<CourseGroup> Stack = new List<CourseGroup>();
                    Stack.Add(cg);
                    MainList.Add(Stack);
                }
            }
            else
            {
                for (int i = 0; i < coursesByGroup.Count() - 1; i++)
                {
                    if (i == 0)
                    {
                        MainList = GetMatches(coursesByGroup[i], coursesByGroup[i + 1]);
                    }
                    else
                    {
                        var RelevantCourses = GetRelevantCourse(MainList);
                        SubList = GetMatches(RelevantCourses, coursesByGroup[i + 1]);
                        MainList = AddToMainList(MainList, SubList);
                    }
                }
            }
            return MainList;
        }
        
        //Find all match courses betwwen two courses lists
        public List<List<CourseGroup>> GetMatches(List<CourseGroup> first, List<CourseGroup> seconed)
        {
            List<List<CourseGroup>> Courses = new List<List<CourseGroup>>();
            foreach (var f in first)
            {
                foreach (var s in seconed)
                {
                    List<CourseGroup> Stack = new List<CourseGroup>();
                    Stack.Add(f);
                    if (CheckCourse(s, Stack))
                    {
                        Stack.Add(s);
                        Courses.Add(Stack);
                    }
                }
            }
            return Courses;
        }
        
        //Add sub list to main list
        public List<List<CourseGroup>> AddToMainList(List<List<CourseGroup>> mainList, List<List<CourseGroup>> subList)
        {
            List<List<CourseGroup>> NewMainList = new List<List<CourseGroup>>();
            foreach (var m in mainList)
            {
                foreach (var s in subList)
                {
                    if (ListsMatch(m, s))
                    {
                        List<CourseGroup> Cg = new List<CourseGroup>();
                        foreach (var n in m)
                        {
                            Cg.Add(n);
                        }
                        Cg.Add(s[s.Count() - 1]);
                        NewMainList.Add(Cg);
                    }
                }
            }
            return NewMainList;
        }
        
        //Check if there is a match between sub list and main list
        public bool ListsMatch(List<CourseGroup> m, List<CourseGroup> s)
        {
            if (m[m.Count() - 1] == s[0] && CheckCourse(s[1], m))
                return true;
            else
                return false;
        }
        
        //Return only relenavt courses that had match before
        public List<CourseGroup> GetRelevantCourse(List<List<CourseGroup>> mainList)
        {
            List<CourseGroup> RelevantCourses = new List<CourseGroup>();
            foreach (var main in mainList)
            {
                if (!RelevantCourses.Contains(main[main.Count() - 1]))
                    RelevantCourses.Add(main[main.Count() - 1]);
            }

            return RelevantCourses;
        }
        
        //Create full timeTable including empty hours from 8:00 to 23:00
        public List<TimeTable> MakeTimeTable(List<TimeTable> timeTables)
        {
            List<TimeTable> AllTimeTables = new List<TimeTable>();

            for (int i = 0; i < timeTables.Count(); i++)
            {
                //List<List<SummerSemester>> tTable = new List<List<SummerSemester>>();
                TimeTable tTable = new TimeTable();
                Day daya = new Day();
                Day dayb = new Day();
                Day dayc = new Day();
                Day dayd = new Day();
                Day daye = new Day();
                Day dayf = new Day();

                daya.Courses.Add(CreateDays("יום ראשון"));
                dayb.Courses.Add(CreateDays("יום שני"));
                dayc.Courses.Add(CreateDays("יום שלישי"));
                dayd.Courses.Add(CreateDays("יום רביעי"));
                daye.Courses.Add(CreateDays("יום חמישי"));
                dayf.Courses.Add(CreateDays("יום שישי"));

                var sTimea = new TimeSpan(8, 0, 0);
                var sTimeb = new TimeSpan(8, 0, 0);
                var sTimec = new TimeSpan(8, 0, 0);
                var sTimed = new TimeSpan(8, 0, 0);
                var sTimee = new TimeSpan(8, 0, 0);
                var sTimef = new TimeSpan(8, 0, 0);

                bool first = true;
                int sumStart = 0, sumEnd = 0;
                int countStart = 0, countEnd = 0;
                foreach (var c in timeTables[i].Sunday.Courses)
                {
                    //Average of start and end hours
                    if (timeTables[i].Sunday.Courses.Last() == c)
                    {
                        sumEnd += (int)c.EndHour.Value.Hours;
                        countEnd++;
                    }
                    if (first)
                    {
                        sumStart += (int)c.StartHour.Value.Hours;
                        countStart++;
                        first = false;
                    }

                    if (c.StartHour > sTimea)
                    {
                        CoursesDetails blankLess = CreateDays("");
                        blankLess.NumConLessons = (int)(((TimeSpan)(c.StartHour)).TotalHours - sTimea.TotalHours);
                        daya.Courses.Add(blankLess);
                        sTimea = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                    }
                    daya.Courses.Add(c);
                    sTimea = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                }
                first = true;
                foreach (var c in timeTables[i].Monday.Courses)
                {
                    if (timeTables[i].Monday.Courses.Last() == c)
                    {
                        sumEnd += (int)c.EndHour.Value.Hours;
                        countEnd++;
                    }
                    if (first)
                    {
                        sumStart += (int)c.StartHour.Value.Hours; 
                        countStart++;
                        first = false;
                    }
                    if (c.StartHour > sTimeb)
                    {
                        CoursesDetails blankLess = CreateDays("");
                        blankLess.NumConLessons = (int)(((TimeSpan)(c.StartHour)).TotalHours - sTimeb.TotalHours);
                        dayb.Courses.Add(blankLess);
                        sTimeb = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                    }
                    dayb.Courses.Add(c);
                    sTimeb = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                }
                first = true;
                foreach (var c in timeTables[i].Tuesday.Courses)
                {
                    if (timeTables[i].Tuesday.Courses.Last() == c)
                    {
                        sumEnd += (int)c.EndHour.Value.Hours;
                        countEnd++;
                    }
                    if (first)
                    {
                        sumStart += (int)c.StartHour.Value.Hours; 
                        countStart++;
                        first = false;
                    }
                    if (c.StartHour > sTimec)
                    {
                        CoursesDetails blankLess = CreateDays("");
                        blankLess.NumConLessons = (int)(((TimeSpan)(c.StartHour)).TotalHours - sTimec.TotalHours);
                        dayc.Courses.Add(blankLess);
                        sTimec = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                    }
                    dayc.Courses.Add(c);
                    sTimec = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                }
                first = true;
                foreach (var c in timeTables[i].Wednesday.Courses)
                {
                    if (timeTables[i].Wednesday.Courses.Last() == c)
                    {
                        sumEnd += (int)c.EndHour.Value.Hours;
                        countEnd++;
                    }
                    if (first)
                    {
                        sumStart += (int)c.StartHour.Value.Hours; 
                        countStart++;
                        first = false;
                    }
                    if (c.StartHour > sTimed)
                    {
                        CoursesDetails blankLess = CreateDays("");
                        blankLess.NumConLessons = (int)(((TimeSpan)(c.StartHour)).TotalHours - sTimed.TotalHours);
                        dayd.Courses.Add(blankLess);
                        sTimed = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                    }
                    dayd.Courses.Add(c);
                    sTimed = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                }
                first = true;
                foreach (var c in timeTables[i].Thursday.Courses)
                {
                    if (timeTables[i].Thursday.Courses.Last() == c)
                    {
                        sumEnd += (int)c.EndHour.Value.Hours;
                        countEnd++;
                    }
                    if (first)
                    {
                        sumStart += (int)c.StartHour.Value.Hours; 
                        countStart++;
                        first = false;
                    }
                    if (c.StartHour > sTimee)
                    {
                        CoursesDetails blankLess = CreateDays("");
                        blankLess.NumConLessons = (int)(((TimeSpan)(c.StartHour)).TotalHours - sTimee.TotalHours);
                        daye.Courses.Add(blankLess);
                        sTimee = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                    }
                    daye.Courses.Add(c);
                    sTimee = (TimeSpan)(c.EndHour + +new TimeSpan(0, 10, 0));
                }
                first = true;
                foreach (var c in timeTables[i].Friday.Courses)
                {
                    if (timeTables[i].Friday.Courses.Last() == c)
                    {
                        sumEnd += (int)c.EndHour.Value.Hours;
                        countEnd++;
                    }
                    if (first)
                    {
                        sumStart += (int)c.StartHour.Value.Hours; 
                        countStart++;
                        first = false;
                    }
                    if (c.StartHour > sTimef)
                    {
                        CoursesDetails blankLess = CreateDays("");
                        blankLess.NumConLessons = (int)(((TimeSpan)(c.StartHour)).TotalHours - sTimef.TotalHours);
                        dayf.Courses.Add(blankLess);
                        sTimef = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                    }
                    dayf.Courses.Add(c);
                    sTimef = (TimeSpan)(c.EndHour + new TimeSpan(0, 10, 0));
                }
                tTable.StartHourAvg = sumStart / countStart;
                tTable.EndHourAvg = sumEnd / countEnd;

                FillDay(daya, sTimea);
                FillDay(dayb, sTimeb);
                FillDay(dayc, sTimec);
                FillDay(dayd, sTimed);
                FillDay(daye, sTimee);
                FillDay(dayf, sTimef);

                tTable.Sunday = daya;
                tTable.Monday = dayb;
                tTable.Tuesday = dayc;
                tTable.Wednesday = dayd;
                tTable.Thursday = daye;
                tTable.Friday = dayf;

                AllTimeTables.Add(tTable);
            }
            return AllTimeTables;
        }
        
        //Create days
        public CoursesDetails CreateDays(string d)
        {
            CoursesDetails day = new CoursesDetails();
            day.SubjectDescription = d;
            day.Class = "";
            day.Lecturer = "";
            day.GroupCode = -1;

            return day;
        }
        
        //Fill day with empty hours
        public void FillDay(Day l, TimeSpan et)
        {
            var lastClass = new TimeSpan(23, 0, 0);
            if (et < lastClass)
            {
                CoursesDetails blankLess = CreateDays("");
                blankLess.NumConLessons = (int)(new TimeSpan(23, 0, 0).TotalHours - et.TotalHours);
                blankLess.GroupCode = -1;
                l.Add(blankLess);
            }
        }
        
        //Check constrains and delete timeTables
        public List<TimeTable> TTConstraints(List<TimeTable> timeTbl, int numOfDays, List<HourLimit> hourLimits, List<DayLimit> days)
        {
            List<TimeTable> TimeTables = new List<TimeTable>();
            TimeSpan SundayStartLimit = new TimeSpan(hourLimits[0].StartTime, 0, 0);
            TimeSpan SundayEndLimit = new TimeSpan(hourLimits[0].EndTime, 0, 0);
            TimeSpan MondayStartLimit = new TimeSpan(hourLimits[1].StartTime, 0, 0);
            TimeSpan MondayEndLimit = new TimeSpan(hourLimits[1].EndTime, 0, 0);
            TimeSpan TuesdayStartLimit = new TimeSpan(hourLimits[2].StartTime, 0, 0);
            TimeSpan TuesdayEndLimit = new TimeSpan(hourLimits[2].EndTime, 0, 0);
            TimeSpan WednesdayStartLimit = new TimeSpan(hourLimits[3].StartTime, 0, 0);
            TimeSpan WednesdayEndLimit = new TimeSpan(hourLimits[3].EndTime, 0, 0);
            TimeSpan ThursdayStartLimit = new TimeSpan(hourLimits[4].StartTime, 0, 0);
            TimeSpan ThursdayEndLimit = new TimeSpan(hourLimits[4].EndTime, 0, 0);
            TimeSpan FridayStartLimit = new TimeSpan(hourLimits[5].StartTime, 0, 0);
            TimeSpan FridayEndLimit = new TimeSpan(hourLimits[5].EndTime, 0, 0);

                    foreach (var tt in timeTbl)
                    {
                        var Check = true;
                                foreach (var c in tt.Sunday.Courses)
                                {
                                    if (c.StartHour < SundayStartLimit)
                                        Check = false;
                                    if (c.EndHour > SundayEndLimit)
                                        Check = false;
                                }
                                foreach (var c in tt.Monday.Courses)
                                {
                                    if (c.StartHour < MondayStartLimit)
                                        Check = false;
                                    if (c.EndHour > MondayEndLimit)
                                        Check = false;
                                }
                                foreach (var c in tt.Tuesday.Courses)
                                {
                                    if (c.StartHour < TuesdayStartLimit)
                                        Check = false;
                                    if (c.EndHour > TuesdayEndLimit)
                                        Check = false;
                                }
                                foreach (var c in tt.Wednesday.Courses)
                                {
                                    if (c.StartHour < WednesdayStartLimit)
                                        Check = false;
                                    if (c.EndHour > WednesdayEndLimit)
                                        Check = false;
                                }
                                foreach (var c in tt.Thursday.Courses)
                                {
                                    if (c.StartHour < ThursdayStartLimit)
                                        Check = false;
                                    if (c.EndHour > ThursdayEndLimit)
                                        Check = false;
                                }
                                foreach (var c in tt.Friday.Courses)
                                {
                                    if (c.StartHour < FridayStartLimit)
                                        Check = false;
                                    if (c.EndHour > FridayEndLimit)
                                        Check = false;
                                }
                                //Check days limit
                                if ((tt.Sunday.Courses.Count() > 0 && days[0].Checked) || (tt.Monday.Courses.Count() > 0 && days[1].Checked)
                                    || (tt.Tuesday.Courses.Count() > 0 && days[2].Checked) || (tt.Wednesday.Courses.Count() > 0 && days[3].Checked)
                                    || (tt.Thursday.Courses.Count() > 0 && days[4].Checked) || (tt.Friday.Courses.Count() > 0 && days[5].Checked))
                                    Check = false;

                                //Check Number of free days limit
                                int Counter = 0;
                                if (tt.Sunday.Courses.Count() == 0)
                                    Counter++;
                                if (tt.Monday.Courses.Count() == 0)
                                    Counter++;
                                if (tt.Tuesday.Courses.Count() == 0)
                                    Counter++;
                                if (tt.Wednesday.Courses.Count() == 0)
                                    Counter++;
                                if (tt.Thursday.Courses.Count() == 0)
                                    Counter++;
                                if (tt.Friday.Courses.Count() == 0)
                                    Counter++;

                                if (Counter < numOfDays)
                                    Check = false;

                        if (Check)
                            TimeTables.Add(tt);
                    }
            return TimeTables;
        }

        //Check if group contains practice
        public bool ContainsPractice(CourseGroup cg)
        {
            foreach (var c in cg.coursesInGroup)
            {
                if(c.OccupationDescription == "תרגול")
                    return true;
            }
            return false;
        }

        //Sort timetables as user request
        public List<TimeTable> SortTT(List<TimeTable> AllTimeTables, int sortBy)
        {
            switch (sortBy)
            {
                case 0:
                    AllTimeTables = AllTimeTables.OrderByDescending(t => t.StartHourAvg).ToList();
                    break;
                case 1:
                    AllTimeTables = AllTimeTables.OrderBy(t => t.StartHourAvg).ToList();
                    break;
                case 2:
                    AllTimeTables = AllTimeTables.OrderBy(t => t.EndHourAvg).ToList();
                    break;
                case 3:
                    AllTimeTables = AllTimeTables.OrderByDescending(t => t.EndHourAvg).ToList();
                    break;
            }
            return AllTimeTables;
        }

        //Get all departments
        public List<SelectListItem> GetAllDepartments()
        {
            var departments = dal.GetAllDepartments();
            return departments.Select(d => new SelectListItem() { Text = d.Name, Value = d.Id.ToString() }).ToList();
        }

        //Get all specializations by department
        public List<Specializtions> GetSpecializationsByDepartment(int depId){
            return dal.GetSpecializationsByDepartment(depId);
        }

        //Get all courses by specializtion
        public object GetCoursesBySpecialization(int specId){
            return dal.GetCoursesBySpecialization(specId);
        }

        //Get all courses that appears in current semester by user course choose
        public List<CoursesDetails> GetSemesterCourses(List<Course> courses, int semester)
        {
            return dal.GetSemesterCourses(courses, semester);
        }

        //Get link table
        public List<LinkTable> GetLink()
        {
            return dal.GetLink();
        }

        //Search course by course code
        public List<Courses> SearchCourse(int courseCode)
        {
            return dal.SearchCourse(courseCode);
        }

        //Get specific course
        public List<CoursesDetails> GetCourses(int groupCode, int semester)
        {
            return dal.GetCourses(groupCode, semester);
        }
    }
}