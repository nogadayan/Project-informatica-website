using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project_informatica_website.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Project_informatica_website.Dbase;

namespace Project_informatica_website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // alle namen ophalen
            var Actor = GetActors();

            // stop de namen in de HTML
            return View(Actor);
        }
        [Route("Movies")]
        public IActionResult Movies()
        {
            var Movie = GetMovies();
            return View(Movie);
        }
        public List<string> GetNames()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<string> names = new List<string>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from movie", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        string Name = reader["Title"].ToString();

                        // voeg de naam toe aan de lijst met namen
                        names.Add(Name);
                    }
                }
            }

            // return de lijst met namen
            return names;
        }
        public List<Movie> GetMovies()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Movie> Movie = new List<Movie>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from movie", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        Movie m = new Movie
                        {
                            // selecteer de kolommen die je wil lezen."
                            IMDB = reader["IMDB"].ToString(),
                            Title = reader["Title"].ToString(),
                            Plot = reader["Plot"].ToString(),
                            File_Movie = reader["File_Movie"].ToString(),
                        };
                        // voeg movie toe aan de lijst
                        Movie.Add(m);
                    }
                }
            }

            // return de lijst met movies
            return Movie;
        }
        public List<Actor> GetActors()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de Actors in gaan opslaan
            List<Actor> Actor = new List<Actor>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from actor", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        Actor m = new Actor
                        {
                            // selecteer de kolommen die je wil lezen."
                            Actor_ID = reader["Actor_ID"].ToString(),
                            Name = reader["Name"].ToString(),
                            Birth_Date = reader["Birth Date"].ToString(),
                            File = reader["File"].ToString(),

                        };
                        // voeg actor toe aan de lijst
                        Actor.Add(m);
                    }
                }
            }
            // return de lijst met actors
            return Actor;
        }
        public List<movie_actors> Getmovie_actors()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de Actors in gaan opslaan
            List<movie_actors> movie_actors = new List<movie_actors>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from movie-actors", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        movie_actors m = new movie_actors
                        {
                            // selecteer de kolommen die je wil lezen."
                            Movie_Actors_ID = Convert.ToInt32(reader["Movie-Actors_ID"]),
                            IMDB = reader["IMDB"].ToString(),
                            Actor_ID = reader["Actor_ID"].ToString(),
                            Role = reader["Role"].ToString(),                      
                        };
                        // voeg actor toe aan de lijst
                        movie_actors.Add(m);
                    }
                }
            }
            // return de lijst met actors
            return movie_actors;
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
