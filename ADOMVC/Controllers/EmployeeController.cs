using ADOMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace ADOMVC.Controllers
{
    public class EmployeeController : Controller
    {
        SqlConnection connection = null;
        SqlCommand command = null;

        static string GetCOnnectionString()
        {
            return @"data source=ANAMIKA\SQLSERVER;initial catalog=EmpDb;integrated security=true";
        }
        static SqlConnection GetConnection()
        {
            return new SqlConnection(GetCOnnectionString());
        }
        public EmployeeController()
        {
        }
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            using (connection = GetConnection())
            {
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = "Select * from Employee";
                        command.Connection = connection;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee()
                                {
                                    Id = (int)reader["id"],
                                    Name = reader["name"].ToString(),
                                    Dob = (DateTime)reader["doj"],
                                    Salary = (int)reader["salary"]

                                };
                                employees.Add(employee);

                            }
                        }
                        else
                        {
                            ViewBag.msg = "There are no records";
                            return View();
                        }
                    }
                }
            }
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            using (connection = GetConnection())
            {
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = $"insert into Employee(name, doj,salary) values('{employee.Name}','{employee.Dob}', {employee.Salary})";
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        return RedirectToAction("Index");
                    }
                }
            }
        }
    }
}