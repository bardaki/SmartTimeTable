using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Models
{
    public class DepartmentsModel
    {
        public string Text = "Departments";
        public IEnumerable<SelectListItem> Departments { get; set; }

        public DepartmentsModel(IEnumerable<SelectListItem> Dep)
        {
            Departments = Dep;
        }
    }
}