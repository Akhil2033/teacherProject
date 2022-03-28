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
        private readonly SchoolDbContext School = new SchoolDbContext();

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]

        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {

            // creating an instance of a connection
            MySqlConnection mySqlConnection = School.AccessDatabase();
            MySqlConnection Conn = mySqlConnection;

            NewMethod(Conn, Conn);

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query
            cmd.CommandText = "SELECT * from teachers where lower(teacherfname) like lower('%" + SearchKey + "%') or lower(teacherlname) like lower('%" + SearchKey + "%')";

            //gather result set of query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //create an empty list of teachers
            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                //access coloumn information from db column as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                Decimal Salary = (Decimal)ResultSet["salary"];

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFName;
                NewTeacher.TeacherLname = TeacherLName;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = (float)Salary;


                //add the teacher Name to the list
                Teachers.Add(NewTeacher);

            }
            //close connection between web server and mysql database
            Conn.Close();

            //Return the final list of teacher names
            return Teachers;

        }

        private static void NewMethod(MySqlConnection Conn, MySqlConnection conn)
        {
            conn.Open();
        }

        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            // creating an instance of a connection
            MySqlConnection mySqlConnection = School.AccessDatabase();
            MySqlConnection Conn = mySqlConnection;

            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query
            cmd.CommandText = "SELECT * from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();


            //gather result set of query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();


            while (ResultSet.Read())
            {
                //access coloumn information from db column as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                Decimal Salary = (Decimal)ResultSet["salary"];

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFName;
                NewTeacher.TeacherLname = TeacherLName;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = (float)Salary;
            }

            return NewTeacher;

        }
    }
}
