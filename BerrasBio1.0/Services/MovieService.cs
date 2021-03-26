using BerrasBio1._0.Data;
using BerrasBio1._0.Models;
using BerrasBio1._0.Models.JsonModels;
using Microsoft.EntityFrameworkCore;
using RestSharp;
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

                var response = client.Execute<MovieResponse>(new RestRequest($"/{movieId}?api_key={apiKey}"));
                var crewResponse = client.Execute<CrewResponse>(new RestRequest($"/{movieId}/credits?api_key={apiKey}"));
                var videoResponse = client.Execute<VideoResponse>(new RestRequest($"/{movieId}/videos?api_key={apiKey}"));

                Movie existingMovie = _db.Movies.Where(x => x.Name == response.Data.Title).FirstOrDefault();
                if (response.IsSuccessful && crewResponse.IsSuccessful && response.Data.BackdropPath != null && response.Data.PosterPath != null && response.Data.Overview != null && existingMovie == null && response.Data.Runtime != 0)
                {

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
                    Movie newMovie = new Movie()
                    {
                        Name = response.Data.Title,
                        RelaseDate = response.Data.ReleaseDate,
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



























