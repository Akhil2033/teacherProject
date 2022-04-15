using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teacherProject.Models;
using System.Diagnostics;

namespace teacherProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        //GET : /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            Debug.WriteLine("SearchKey is :  " + SearchKey);
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }

        // Get /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
           
           
            return View(NewTeacher);
        }

        // Get: /Teacher/DeleteConfirm/{id}

        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            //displays info to /view/Teacher/show.cshtml
            return View(NewTeacher);
        }

        // Post: /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET: /Teacher/New
        public ActionResult New()
        {
            return View();
        }

        //POST: /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, int Salary)
        {
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }

        /// <summary>
        /// Routes to a dynamically generated "Teacher Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Teacher</param>
        /// <returns>A dynamic "Update Teacher" webpage which provides the current information of the Teacher and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Teacher/Update/5</example>

        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        public ActionResult Ajax_Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedAuthor = controller.FindTeacher(id);

            return View(SelectedAuthor);
        }
    }

    /// <summary>
    /// Receives a POST request containing information about an existing author in the system, with new values. Conveys this information to the API, and redirects to the "Teacher Show" page of our updated teacher.
    /// </summary>
    /// <param name="id">Id of the Teacher to update</param>
    /// <param name="TeacherFname">The updated first name of the author</param>
    /// <param name="TeacherLname">The updated last name of the author</param>
    /// <param name="EmployeeNumber"> The updated employee number</param>
    /// <param name="Hiredate">The updated hiredate of the teacher.</param>
    /// <param name="salary">The updated salary of the teacher.</param>
    /// <returns>A dynamic webpage which provides the current information of the teacher.</returns>
    /// <example>
    /// POST : /Teacher/Update/10
    /// FORM DATA / POST DATA / REQUEST BODY 
    /// {
    ///     "TeacherFname":"Neo",
    ///     "TeacherLName":"Anderson",
    ///     "EmployeeNumber":"T001",
    ///     "HireDate":"2021-03-17",
    ///     "salary":"66.66"
    /// }
    /// </example>
    [HttpPost]
    public ActionResult Update(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, int Salary)
    {
        Teacher TeacherInfo = new Teacher();
        TeacherInfo.TeacherFname = TeacherFname;
        TeacherInfo.TeacherLname = TeacherLname;
        TeacherInfo.EmployeeNumber = EmployeeNumber;
        TeacherInfo.HireDate = HireDate;
        TeacherInfo.Salary = Salary;

        TeacherDataController controller = new TeacherDataController();
        controller.UpdateTeacher(id, TeacherInfo);

        return RedirectToAction("Show/" + id);
    }
}
