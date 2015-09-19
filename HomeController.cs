using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zadanie_insert.Models;
using System.Data.SqlClient;

namespace zadanie_insert.Controllers
{
    public class HomeController : Controller
    {
        // Metoda Index volana po odoslani formulara
        [HttpPost]
        public ActionResult Index(string firstName, string lastName)
        {
            User student = new User();
            student.firstName = firstName;
            student.lastName = lastName;

            using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog=TestDb; Integrated Security=SSPI;"))
            {
                con.Open();

                // Pre newId je nastavena predvolena hodnota na nulu
                int newId = 0;
                // Z tabulky dbo.UserTable sa nacita maximalna hodnota Id
                SqlCommand selectCmd = new SqlCommand("Select MAX(Id) from dbo.UserTable", con);          
                object obj = selectCmd.ExecuteScalar();
                // Zistuje sa ci metoda ExecuteScalar() nevratila nulovu hodnotu, ak nie tak sa do newId priradi maximalna hodnota Id z databazy navysena o 1
                if (obj != DBNull.Value)
                    newId = (int)obj + 1;

                student.id = newId;

                // Vlozenie noveho uzivatela do databazy
                SqlCommand insertCmd = new SqlCommand("Insert into dbo.UserTable Values(@NewID,@FirstName,@LastName)", con);
                insertCmd.Parameters.AddWithValue("@NewID", student.id);
                insertCmd.Parameters.AddWithValue("@FirstName", student.firstName);
                insertCmd.Parameters.AddWithValue("@LastName", student.lastName);

                // Zaznamenava sa pocet vlozenych zaznamov do tabulky
                int rowsAffected = insertCmd.ExecuteNonQuery();
                ViewBag.Rows = rowsAffected;

                con.Close();
            }
             
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
