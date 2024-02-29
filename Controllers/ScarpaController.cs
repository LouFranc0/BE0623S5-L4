using Microsoft.AspNetCore.Mvc;
using S5L4_Scarpe.Models;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;


namespace S5L4_Scarpe.Controllers
{
    public class ScarpaController : Controller
    {
        private string connString = "Server=localhost,1433;Database=Scarpe; User Id=sa;Password=NotHunter2 Initial Catalog=Pescheria; Integrated Security=true; TrustServerCertificate=True";

        public IActionResult Index()
        {
            return View();

        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Add(string name, decimal price, string details, IFormFile coverImg)
        {
            var conn = new SqlConnection(connString);
            try
            {
                //validare dati

                conn.Open();

                //salviamo il file che ci è stato inviato 

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                string fileName = Path.GetFileName(coverImg.FileName);
                string fullFilePath = Path.Combine(path, fileName);
                FileStream stream = new FileStream(fullFilePath, FileMode.Create);
                coverImg.CopyTo(stream);

                //creare il comando

                var command = new SqlCommand(@"INSERT INTO Scarpe(NomeScarpa, PrezzoScarpa, DescrizioneScarpa, Immagine 1)", conn);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@description", details);
                //questo valore lo ricaviamo dopo aver salvato l'immagine nel disco
                command.Parameters.AddWithValue("@image", fileName);

                //eseguire il comando

                var nRows = command.ExecuteNonQuery();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View("error");
            }
            finally { conn.Close(); }
        }
    }
}