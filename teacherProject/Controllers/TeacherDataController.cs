using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using teacherProject.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
//using System.Web.Http.Cors;

namespace teacherProject.Controllers
{
    public class TeacherDataController : ApiController
    {
        // Accessing school sql database
        private readonly SchoolDbContext School = new SchoolDbContext();

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]

        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {

            // creating an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

     

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query
            cmd.CommandText = "SELECT * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key)";
            // sanatising search from injection attacks
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

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
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime) ResultSet["hiredate"];
                int Salary = Convert.ToInt32(ResultSet["salary"]);

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFName;
                NewTeacher.TeacherLname = TeacherLName;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;


                //add the teacher Name to the list
                Teachers.Add(NewTeacher);

            }
            //close connection between web server and mysql database
            Conn.Close();

            //Return the final list of teacher names
            return Teachers;

        }



        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            // creating an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
            
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
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime) ResultSet["hiredate"];
                int Salary = Convert.ToInt32(ResultSet["salary"]);

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
            }

            Conn.Close();

            return NewTeacher;

        }
        /// <summary>
        /// Deletes a teacher from a connected sql database provided the id of the teacher exists.
        /// </summary>
        /// necessary input parameter name = "Teacherid"(id of the teacher)
        /// <example>Post : /api/TeacherData/DeleteTeacher/6 </example>

        [HttpPost]
        public void DeleteTeacher(int Teacherid)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "DELETE from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", Teacherid);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        ///<summary> This query adds a new Teacher to the database </summary>
        /// <param name = "NewTeacher"> An object with fields that map to the columns of the teacher's table. non-deterministic </param>
        /// <emample>
        /// POST api/TeacherData/AddTeacher
        /// 
        /// FORM Data/ POST DATA/ Request body
        /// {
        ///     "TeacherFname":"Neo",
        ///     "TeacherLName":"Anderson",
        ///     "EmployeeNumber":"T001",
        ///     "HireDate":"2021-03-17",
        ///     "salary":"66.66"
        /// }
        ///
        /// </emample>
        /// 

        [HttpPost]

        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Debug.WriteLine(NewTeacher.TeacherFname);

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname,@TeacherLname,@EmployeeNumber,CURRENT_DATE(),@Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLName", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        /// <summary>
        /// Updates an Author on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="TeacherInfo">An object with fields that map to the columns of the author's table.</param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacher/8 
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
        //[EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Debug.WriteLine(TeacherInfo.TeacherFname);

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "update teachers set teacherfname =@TeacherFname, teacherlname=@TeacherLname, employeenumber=@EmployeeNumber, hiredate=CURRENT_DATE(), salary=@salary where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLName", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }


    }
}
