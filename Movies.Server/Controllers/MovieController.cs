using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class MovieController : Controller
	{
		private readonly IMovieGrainClient _client;
		private readonly ISearchGrainClient _clientSearch;

		public MovieController(
			IMovieGrainClient client,
			ISearchGrainClient clientSearch
		)
		{
			_client = client;
			_clientSearch = clientSearch;
		}

		[HttpGet("getall")]
		public async Task<List<MovieModel>> Getall()
		{
			var result = await _clientSearch.GetAll().ConfigureAwait(false);
			return result;
		}

		[HttpGet("getmostrated")]
		public async Task<List<MovieModel>> GetMostRated()
		{
			var result = await _clientSearch.GetMostRated().ConfigureAwait(false);
			return result;
		}

		[HttpGet("search/{query}")]
		public async Task<List<MovieModel>> Search(string query)
		{
			var results = await _clientSearch.Get(query).ConfigureAwait(false);
			return results;
		}

		[HttpGet("{id}")]
		public async Task<MovieModel> Get(int id)
		{
			var result = await _client.Get(id).ConfigureAwait(false);
			return result;
		}

	}
}
