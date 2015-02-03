using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class TimeTable
    {       
        public Day Sunday = new Day();
        public Day Monday = new Day();
        public Day Tuesday = new Day();
        public Day Wednesday = new Day();
        public Day Thursday = new Day();
        public Day Friday = new Day();

        public double StartHourAvg { get; set; }
        public double EndHourAvg { get; set; }
        public double WindowsAvg { get; set; }
    }
}