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
using Microsoft.AspNetCore.Http;
using System.Net;
using System.IO;
using System.Text.Json;

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
            // haal de User uit Cookie
            ViewData["user"] = HttpContext.Session.GetString("User");

            // alle namen ophalen
            var Actor = GetActors(null);

            // stop de namen in de HTML
            return View(Actor);
        }
        [Route("Kaartje/{id}")]
        public IActionResult Kaartje(string id)
        {
            // https://www.omdbapi.com/
            // Website gebruikt om infformatie over de movie te halen
            //
            // apiKey aangevraagd bij www.omdapi.com
            //
            string apiKey = "43e5c6f2";
            string url = "http://www.omdbapi.com/?i=" + id + "&apikey=" + apiKey;

            // Created movie for our model object
            MoviefromAPI m;

            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(url);

            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Get the stream containing content returned by the server.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();

                    // deserialize json response to movie object
                    m = JsonSerializer.Deserialize<MoviefromAPI>(responseFromServer);
                }
            }
            //
            // Haal de movie op die via id geselecteerd is
            //
            List<Movie> movies = GetMovies(id);
            ViewData["movies"] = movies;
            //
            // Haal nu de Actors_ID en Rol op van de Actors die bij de film horen
            //
            List<Movie_actors> movie_actors = GetMovie_Actors(id);
            ViewData["movie_actors"] = movie_actors;
            //
            // Nog afmaken Haal nu de Actors op die in de lijst Movie_Actors zitten
            //
            // Van elke movie_actors genereer de actors
            //Klaar!!
            //

            if (movie_actors != null)
            {
                List<Actor> actors1 = new List<Actor>();
                {
                    foreach (var movie_actor in movie_actors)
                    {
                        //List<Actor> tempactors = new List<Actor>();
                        List<Actor> tempactors = GetActors(movie_actor.Actor_ID);
                        if (tempactors != null)
                        {
                        foreach (var temp2actors in tempactors)
                            {
                                // nog te doen tijd uit de Birth_Date halen
                                actors1.Add(new Actor() { File = temp2actors.File, Name = temp2actors.Name, Birth_Date = temp2actors.Birth_Date });
                            }
                        }

                    }
                }
                ViewData["actors"] = actors1;
            }
            
            //
            // en weer terug
            //
            return View(m);
        }
        [Route("Login")]
        public IActionResult Login(string username, string password)
        {
            var Users = GetUsers();

            if (username != null)
            {
                HttpContext.Session.SetString("User", username); // username
                HttpContext.Session.SetString("password", password);
                return Redirect("/");
            }

            return View(Users);
        }
        [Route("Movies")]
        public IActionResult Movies()
        {
            var Movie = GetMovies(null);

            List<Actor> actors = GetActors(null);
            ViewData["actors"] = actors;

            return View(Movie);
        }
        public List<string> GetNames()
        {
            // stel in waar de database gevonden kan worden
            // string connectionString = "Server=172.16.160.21;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";
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
        public List<Movie> GetMovies(string imdb_tt)
        {
            // stel in waar de database gevonden kan worden
            // string connectionString = "Server=172.16.160.21;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Movie> Movie = new List<Movie>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                if (imdb_tt == null)
                {
                    MySqlCommand cmd = new MySqlCommand("select * from movie", conn);
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
                                Genre = reader["Genre"].ToString(),
                                Plot = reader["Plot"].ToString(),
                                File_Movie = reader["File_Movie"].ToString(),
                            };
                            // voeg movie toe aan de lijst
                            Movie.Add(m);
                        }
                    }
                }
                if (imdb_tt != null)
                {
                    MySqlCommand cmd = new MySqlCommand("select * from movie where IMDB = '" + imdb_tt +"'", conn);
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
                                Genre = reader["Genre"].ToString(),
                                Plot = reader["Plot"].ToString(),
                                File_Movie = reader["File_Movie"].ToString(),
                            };
                            // voeg movie toe aan de lijst
                            Movie.Add(m);
                        }
                    }
                }
                // resultaat van de query lezen

            }

            // return de lijst met movies
            return Movie;
        }
        public List<Actor> GetActors(string imdb_nm)
        {
            // stel in waar de database gevonden kan worden
            // string connectionString = "Server=172.16.160.21;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de Actors in gaan opslaan
            if (imdb_nm == null)
            { 
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
            if (imdb_nm != null)
            {
                List<Actor> Actor = new List<Actor>();

                // verbinding maken met de database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    // verbinding openen
                    conn.Open();

                    // SQL query die we willen uitvoeren
                    MySqlCommand cmd = new MySqlCommand("select * from actor where Actor_ID = '" + imdb_nm + "'", conn);

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
            return null;           
        }
        public List<Movie_actors> GetMovie_Actors(string imdb_ma)
        {
            // stel in waar de database gevonden kan worden
            // string connectionString = "Server=172.16.160.21;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Movie_actors> Movie_actors = new List<Movie_actors>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                if (imdb_ma == null)
                {
                    MySqlCommand cmd = new MySqlCommand("select * from movie_actors", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        // elke keer een regel (of eigenlijk: database rij) lezen
                        while (reader.Read())
                        {
                            Movie_actors m = new Movie_actors
                            {
                                // selecteer de kolommen die je wil lezen."
                                Movie_Actors_ID = Convert.ToInt32(reader["Movie_Actors_ID"]),
                                IMDB = reader["IMDB"].ToString(),
                                Actor_ID = reader["Actor_ID"].ToString(),
                                Role = reader["Role"].ToString(),
                            };
                            // voeg movie_actors toe aan de lijst
                            Movie_actors.Add(m);
                        }
                    }
                }
                if (imdb_ma != null)
                {
                    MySqlCommand cmd = new MySqlCommand("select * from movie_actors where IMDB = '" + imdb_ma + "'", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        // elke keer een regel (of eigenlijk: database rij) lezen
                        while (reader.Read())
                        {
                            Movie_actors m = new Movie_actors
                            {
                                // selecteer de kolommen die je wil lezen."
                                IMDB = reader["IMDB"].ToString(),
                                Actor_ID = reader["Actor_ID"].ToString(),
                                Role = reader["Role"].ToString(),
                            };
                            // voeg movie toe aan de lijst
                            Movie_actors.Add(m);
                        }
                    }
                }
                // resultaat van de query lezen

            }
            // return de lijst met movies
            return Movie_actors;
        }
        public List<Users> GetUsers()
        {
            // stel in waar de database gevonden kan worden
            // string connectionString = "Server=172.16.160.21;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Users> Users = new List<Users>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from users", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        Users m = new Users
                       {
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        };
                        // voeg de naam toe aan de lijst met namen
                        Users.Add(m);
                    }
                }
            }

            // return de lijst met namen
            return Users;
        }
        public List<string> GetUser(string Uname)
        {
            // stel in waar de database gevonden kan worden
            // string connectionString = "Server=172.16.160.21;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<string> Users = new List<string>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from users where Username = '" + Uname + "'", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"

                        string Password = reader["Password"].ToString();

                        // voeg de naam toe aan de lijst met namen
                        Users.Add(Password);
                    }
                }
            }

            // return de lijst met namen
            return Users;
        }
        [Route("Action_page")]
        public IActionResult Action_page(string firstname, string lastname, string country, string message)
        {
            return View();
        }
        [Route("Action_Login")]
        public IActionResult Action_Login(string uname, string psw)
        {
            // het wachtwoord zit in de eertse lijst positie !!
            string wachtwoord = GetUser(uname).First();
            if (psw == wachtwoord)
            {
                HttpContext.Session.SetString("User", uname);
                HttpContext.Session.SetString("password", psw);
                return Redirect("/");
            }

            return View();
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

        [HttpPost]
        public IActionResult Contact(Person person)
        {
            if (ModelState.IsValid)
                return Redirect("/succes");
            return View(person);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
