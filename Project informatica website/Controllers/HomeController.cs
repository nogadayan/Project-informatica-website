﻿using Microsoft.AspNetCore.Mvc;
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
            var Movie = GetMovies();

            // stop de namen in de HTML
            return View(model: Movie);
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
                        };
                        // voeg movie toe aan de lijst
                        Movie.Add(m);
                    }
                }
            }

            // return de lijst met movies
            return Movie;
        }
        public List<Actors> GetActors()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110382;Uid=110382;Pwd=inf2021sql;";

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Actors> Actors = new List<Actors>();

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
                        Actors m = new Actors
                        {
                            // selecteer de kolommen die je wil lezen."
                            Actor_ID = reader["Actor_ID"].ToString(),
                            Name = reader["Name"].ToString(),
                            Birth_Date = reader["Birth Date"].ToString(),
                            Picture = (byte)reader["Picture"]

                        };
                        // voeg movie toe aan de lijst
                        Actors.Add(m);
                    }
                }
            }
            // return de lijst met movies
            return Actors;
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
