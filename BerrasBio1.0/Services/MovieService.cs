using BerrasBio1._0.Data;
using BerrasBio1._0.Models;
using BerrasBio1._0.Models.JsonModels;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace BerrasBio1._0.Services
{
    public class MovieService : IMovieService
    {
        private readonly CinemaContext _db;
        public MovieService(CinemaContext db)
        {
            _db = db;
        }

        //Function to automatically create showings. Not used in the project itself, but was used to run once to populate the database, 
        //from which we made an sql-script to use to seed the database if the database does'nt exist.
        public void CreateShowings()
        {
            Random random = new Random();
            //loop 100 times
            for (int i = 0; i < 100; i++)
            {
                //loop to create 5 showings per day, first one at 08, then one new every 3rd hour
                for(int f = 0; f < 15; f =f + 3)
                {
                    //add one day per outer loop
                    DateTime now = DateTime.Now.AddDays(i);
                    //Set the time for the showing
                    DateTime newDate = new DateTime(now.Year, now.Month, now.Day, 08 + f, 00, 00);
                    //Generate a random movie-id to pick what movie to show
                    var randomMovieId = random.Next(1, 112);
                    //Generate a random number 1-2 to determine what auditorium to have the showing in.
                    var randomAuditorium = random.Next(1, 3);

                    //Create the new showing
                    Showing showing = new Showing() { StartTime = newDate, MovieId = randomMovieId, AuditoriumId = randomAuditorium, Price = 5 };
                    int amountOfSeats;
                    //Set the amount of seat for the showing according to what auditorium (Would have been fetched from the db if we had more than 2 auditoriums)
                    if(randomAuditorium == 1)
                    {
                        amountOfSeats = 50;
                    } else
                    {
                        amountOfSeats = 100;
                    }
                    //Get the movie according to the random movieid set in the showing
                    Movie movie = _db.Movies.Where(m => m.Id == showing.MovieId).FirstOrDefault();
                    int duration = movie.Duration;
                    DateTime endTime = showing.StartTime.AddMinutes(duration);
                    showing.EndTime = endTime;
                    //Create the seats, assign the rows etc
                    List<Seat> seats = new List<Seat>();
                    string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
                    for (int x = 0; x < amountOfSeats / 10; x++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            seats.Add(new Seat { Booked = false, Row = letters[x].ToString(), Number = y });
                        }
                    }

                    //Set the price depending on the releasedate and showing-time.
                    if ((movie.RelaseDate - DateTime.Now).TotalDays < 30)
                    {
                        showing.Price += _db.Prices.Where(p => p.Name == "NewMovie").FirstOrDefault().Amount;
                    }
                    if (showing.StartTime.Hour > 10 && showing.StartTime.Hour < 19)
                    {
                        showing.Price += _db.Prices.Where(p => p.Name == "DayShow").FirstOrDefault().Amount;
                    }
                    if (showing.StartTime.Hour > 18 && showing.StartTime.Hour <= 23)
                    {
                        showing.Price += _db.Prices.Where(p => p.Name == "EveningShow").FirstOrDefault().Amount;
                    }
                    showing.Seats = seats;
                    _db.Showings.Add(showing);
                }   
            }
            _db.SaveChanges();
        }

        //Method to fetch movies and images from the movie db API
        public void FetchUpcomingMoviesFromApi(int amountOfPagesToFetch)
        {
            string apiKey = "abef0eabc96675adf06dad47584787b5";

            //fetch ids from upcoming movie-list from movie db api
            List<int> movieIds = new List<int>();
            for (int i = 1; i < amountOfPagesToFetch + 1; i++)
            {
                string upcomingMoviesUrl = $"https://api.themoviedb.org/3/movie/upcoming?api_key={apiKey}&language=en-US&page={i}";
                var upcomingClient = new RestClient(upcomingMoviesUrl);
                var upcomingResponse = upcomingClient.Execute<UpcomingMoviesResponse>(new RestRequest());
                foreach (var result in upcomingResponse.Data.Results)
                {
                    movieIds.Add(result.Id);
                }
            }

            //Fetch every single movie
            foreach (int movieId in movieIds)
            {
                string apiUrl = $"https://api.themoviedb.org/3/movie";
                //string CastUrl = $"https://api.themoviedb.org/3/movie/{movieId}/credits?api_key=abef0eabc96675adf06dad47584787b5";
                //string videoUrl = $"https://api.themoviedb.org/3/movie/{movieId}/videos?api_key=abef0eabc96675adf06dad47584787b5";
                var client = new RestClient(apiUrl);

                //Execute api-calls for movie, video and crew
                var response = client.Execute<MovieResponse>(new RestRequest($"/{movieId}?api_key={apiKey}"));
                var crewResponse = client.Execute<CrewResponse>(new RestRequest($"/{movieId}/credits?api_key={apiKey}"));
                var videoResponse = client.Execute<VideoResponse>(new RestRequest($"/{movieId}/videos?api_key={apiKey}"));

                //Filter the results to only save movies that have posters, backdrops, plot, and runtime provided and check so the movie isnt already in the db
                Movie existingMovie = _db.Movies.Where(x => x.Name == response.Data.Title).FirstOrDefault();
                if (response.IsSuccessful && crewResponse.IsSuccessful && response.Data.BackdropPath != null && response.Data.PosterPath != null && response.Data.Overview != null && existingMovie == null && response.Data.Runtime != 0)
                {
                    //Create writer, producer, director and actors.
                    Writer writer = new Writer();
                    Producer producer = new Producer();
                    Director director = new Director();
                    if (crewResponse.Data.Crew.Where(x => x.Job == "Author").FirstOrDefault() != null)
                    {
                        string writerName = crewResponse.Data.Crew.Where(x => x.Job == "Author").FirstOrDefault().Name;
                        writer = _db.Writers.Where(x => x.Name == writerName).FirstOrDefault();
                        if (writer == null && writerName != null)
                        {
                            writer = new Writer() { Name = writerName };
                        }
                    }
                    if (crewResponse.Data.Crew.Where(x => x.Job == "Producer").FirstOrDefault() != null)
                    {
                        string producerName = crewResponse.Data.Crew.Where(x => x.Job == "Producer").FirstOrDefault().Name;
                        producer = _db.Producers.Where(x => x.Name == producerName).FirstOrDefault();
                        if (producer == null && producerName != null)
                        {
                            producer = new Producer() { Name = producerName };
                        }
                    }
                    if (crewResponse.Data.Crew.Where(x => x.Job == "Director").FirstOrDefault() != null)
                    {
                        string directorName = crewResponse.Data.Crew.Where(x => x.Job == "Director").FirstOrDefault().Name;
                        director = _db.Directors.Where(x => x.Name == directorName).FirstOrDefault();
                        if (director == null && directorName != null)
                        {
                            director = new Director() { Name = directorName };
                        }
                    }

                    //Set the genres for the movie. If the genre dont exist in the database => create it.
                    List<Genre> genreList = new List<Genre>();
                    foreach (var genreInResponse in response.Data.Genres)
                    {
                        Genre genre = _db.Genres.Where(x => x.Name == genreInResponse.Name).FirstOrDefault();
                        if (genre == null)
                        {
                            genre = new Genre() { Name = genreInResponse.Name };
                        }
                        genreList.Add(genre);
                    }
                    List<Actor> actorList = new List<Actor>();
                    foreach (var actorInResponse in crewResponse.Data.Cast)
                    {
                        Actor actor = _db.Actors.Where(x => x.Name == actorInResponse.Name).FirstOrDefault();
                        if (actor == null)
                        {
                            actor = new Actor() { Name = actorInResponse.Name };
                        }
                        actorList.Add(actor);
                    }
                    //Set the trailer url, only accept videos that are trailers and is hosted on youtube.
                    string trailerUrl = "";
                    if (videoResponse.IsSuccessful)
                    {
                        foreach (var videoResult in videoResponse.Data.Results)
                        {
                            if (videoResult.Site == "YouTube" && videoResult.Type == "Trailer")
                            {
                                trailerUrl = $"https://youtu.be/{videoResult.Key}";
                                break;
                            }
                        }
                    }

                    //Create the movie
                    Movie newMovie = new Movie()
                    {
                        Name = response.Data.Title,
                        RelaseDate = Convert.ToDateTime(response.Data.ReleaseDate),
                        Plot = response.Data.Overview,
                        Duration = response.Data.Runtime,
                        ImageUrl = response.Data.PosterPath,
                        BackdropUrl = response.Data.BackdropPath,
                        Genres = genreList,
                        Actors = actorList
                    };
                    if (trailerUrl.Length > 10) { newMovie.TrailerUrl = trailerUrl; };
                    if (writer.Name != null) { newMovie.Writer = writer; };
                    if (producer.Name != null) { newMovie.Producer = producer; };
                    if (director.Name != null) { newMovie.Director = director; };

                    //Download and save the posterimage and the backdrop image.
                    using (WebClient webClient = new WebClient())
                    {
                        string path = Directory.GetCurrentDirectory();
                        webClient.DownloadFile($"https://image.tmdb.org/t/p/original{newMovie.ImageUrl}", $"{path}\\wwwroot\\assets\\img\\posters{newMovie.ImageUrl}");
                        webClient.DownloadFile($"https://image.tmdb.org/t/p/original{newMovie.BackdropUrl}", $"{path}\\wwwroot\\assets\\img\\backdrops{newMovie.BackdropUrl}");
                    }
                    _db.Movies.Add(newMovie);
                    _db.SaveChanges();
                }
            }

        }

        public List<Movie> GetAllMoviesFromDb()
        {
            List<Movie> allMovies = _db.Movies.ToList();
            return allMovies;
        }

        public List<Movie> GetMoviesByAmount(int amount)
        {
            List<Movie> movieList = _db.Movies.Take(amount).ToList();
            return movieList;
        }


        //Function to seed the database if it doesent exist.
        public void SeedFromSqlFile()
        {
            using (_db)
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                string path = Directory.GetCurrentDirectory();
                FileInfo file = new FileInfo($"{path}\\Data\\Seed.sql");
                string script = file.OpenText().ReadToEnd();
                command.CommandText = script;
                _db.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                   
                }
            }
        }
    }
}



























