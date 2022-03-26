using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using teacherProject.Models;
using MySql.Data.MySqlClient;

namespace teacherProject.Controllers
{
    public class TeacherDataController : ApiController
    {
        // accessing our sql database
        private SchoolDbContext School = new SchoolDbContext();

        [HttpGet]
        public IEnumerable<Teacher> ListTeachers()
        {

            // creating an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
           
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query
            cmd.CommandText = "SELECT * from teachers";

            //gather result set of query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //create an empty list of teachers
            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                //access coloumn information from db column as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLName = (string)ResultSet["teacherlname"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLName;
                NewTeacher.HireDate = HireDate;

                //add the teacher Name to the list
                Teachers.Add(NewTeacher); 


                //close connection between web server and mysql database
                Conn.Close();

                //Return the final list of teacher names
                return Teachers;

            }



        }
    }
}
