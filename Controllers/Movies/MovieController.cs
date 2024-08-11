using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Business_Logic_Layer.MovieServices;
using Data.Models.Models.Response.Movie;

namespace MyShow.Controllers.Movies
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]  // Apply this to the entire controller
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<MovieController> _logger;

        public MovieController(IMovieService movieService, ILogger<MovieController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }
        /// <summary>
        /// Allow all users to view movie details by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("MovieById")]
        [AllowAnonymous]  // 
        public IActionResult GetMovie(int Id)
        {
            try
            {
                var movies = _movieService.GetMovieById(Id);
                if (movies == null)
                {
                    return NotFound();
                }
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting movie with ID {Id}", Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the movie.");
            }
        }

        /// <summary>
        /// GetAllMovies UserName wise
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpGet("GetAllMovies")]
        public async Task<IActionResult> GetAllMovies(string UserName)
        {
            try
            {
                var movies = await _movieService.GetAllMovies(UserName);
                return Ok(movies);
            }
            catch (Exception ex)
            {
                // Log exception (if logging is set up)
                _logger.LogError(ex, "Error while getting movie");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the movie.");
            }
        }

        /// <summary>
        /// InsertMovie for admin only 
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>

        [HttpPost("InsertMovie")]
        public IActionResult InsertMovie([FromBody] Movie movie)
        {
            try
            {
                if (movie == null)
                {
                    return BadRequest("Movie data is null.");
                }

                var newMovieId = _movieService.InsertMovie(movie);
                return CreatedAtAction(nameof(GetMovie), new { id = newMovieId }, "MovieId" + ": " + newMovieId + " " + movie.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while inserting movie.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while inserting the movie.");
            }
        }


        /// <summary>
        /// UpdateMovie  for admin only pass id and update movie data with movie id also you can pass
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        [HttpPut("UpdateMovie")]
        public IActionResult UpdateMovie(int Id, [FromBody] Movie movie)
        {
            try
            {
                if (movie == null || movie.Id != Id)
                {
                    return BadRequest("Invalid movie data.");
                }

                var existingMovie = _movieService.GetMovieById(Id);
                if (existingMovie == null)
                {
                    return NotFound();
                }

                _movieService.UpdateMovie(movie);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating movie with ID {Id}", Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the movie.");
            }
        }

        /// <summary>
        /// DeleteMovie for admin Active or InActive
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>

        [HttpDelete("DeleteMovie")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMovie(int id, string UserName)
        {
            try
            {
                var existingMovie = _movieService.GetMovieById( id);
                if (existingMovie == null)
                {
                    return NotFound();
                }

                _movieService.DeleteMovie(id, UserName);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting movie with ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the movie.");
            }
        }

    }

}
