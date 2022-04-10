using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace teacherProject.Models
{
    public class Teacher
    {
        //the following datasets define teacher
        public int TeacherId { get; set; }
        public string TeacherFname { get; set; }
        public string TeacherLname { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime HireDate { get; set; }
        public int Salary { get; set; }
    }
}