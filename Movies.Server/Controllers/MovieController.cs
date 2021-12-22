using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class MovieController : Controller
	{
		private readonly IMovieGrainClient _client;

		public MovieController(
			IMovieGrainClient client
		)
		{
			_client = client;
		}

		// GET api/moviedata/1234
		[HttpGet("{id}")]
		public async Task<MovieModel> Get(int id)
		{
			var result = await _client.Get(id).ConfigureAwait(false);
			return result;
		}

	}
}
