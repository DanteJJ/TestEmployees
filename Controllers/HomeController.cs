using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestEmployees.Models;

namespace TestEmployees.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            db = new PruebasCursoEntities();
        }
        string cadena = "data source =admedic.chczlyjjs6gc.us-east-1.rds.amazonaws.com; initial catalog =PruebasCurso;User ID=imadmin;Password=impass_2021; integrated security = False";
        PruebasCursoEntities db;

        //Using Linq for obtain data
        public ActionResult Index(string Name)
        {
            List<EmployeeCLS> emp = new List<EmployeeCLS>();
            if (Name != null)
            {
                emp = (from em in db.Employeeds
                       select new EmployeeCLS()
                       {
                           ID = em.IdEmployeed,
                           Name = em.Name,
                           LastName = em.LastName,
                           BornDate = (DateTime)em.BornDate,
                           RFC = em.RFC,
                           StatusDes = em.Status,
                       }).Where(m=>m.Name.Contains(Name)).OrderBy(m => m.BornDate).ToList();
            }
            else
            {
                emp = (from em in db.Employeeds
                       select new EmployeeCLS()
                       {
                           ID = em.IdEmployeed,
                           Name = em.Name,
                           LastName = em.LastName,
                           BornDate = (DateTime)em.BornDate,
                           RFC = em.RFC,
                           StatusDes = em.Status,
                       }).OrderBy(m => m.BornDate).ToList();
            }
            ViewBag.Nombre = Name;
            return View(emp);
        }


        public ActionResult OpenNewEmployeed()
        {
            EmployeeCLS employeed = new EmployeeCLS();
            return PartialView(employeed);
        }

        //Here i'm gonna use Stored procedure to search if RFC exists 
        public sbyte CheckEmployeedExist(string RFC)
        {
            sbyte aux = 1;
            MySqlConnection conn = new MySqlConnection(cadena);
            MySqlCommand command = new MySqlCommand("Create_Read_Update_Delete_Employeed", conn);
            command.Parameters.AddWithValue("TypeQuery", 1);
            command.Parameters.AddWithValue("idEm", 0);
            command.Parameters.AddWithValue("nam", "");
            command.Parameters.AddWithValue("las", "");
            command.Parameters.AddWithValue("rf", RFC);
            command.Parameters.AddWithValue("bor", DateTime.Now);
            command.Parameters.AddWithValue("sta", "");
            command.CommandType = System.Data.CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                aux = (Int32.Parse(read[0].ToString()) != 0) ? aux : (sbyte)0;
            }
            conn.Close();
            return aux;
        }

        public ActionResult AddEmployeed(EmployeeCLS employeed)
        {
            Employeed employeedAdd = new Employeed() { 
                 Name = employeed.Name,
                 LastName = employeed.LastName,
                 RFC = employeed.RFC,
                 BornDate = employeed.BornDate,
                 Status = employeed.StatusDes
            };
            db.Employeeds.Add(employeedAdd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}