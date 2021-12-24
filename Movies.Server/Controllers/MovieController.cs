using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using Movies.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class MovieController : Controller
	{
		private readonly IMovieGrainClient _movieClient;
		private readonly ISearchGrainClient _searchClient;
		private readonly IGenreGrainClient _genreClient;
		private readonly ReferenceDataService _referenceDataService;

		public MovieController(
			ReferenceDataService referenceDataService,
			IMovieGrainClient movieClient,
			ISearchGrainClient searchClient,
			IGenreGrainClient genreClient
		)
		{
			_referenceDataService = referenceDataService;
			_movieClient = movieClient;
			_searchClient = searchClient;
			_genreClient = genreClient;
		}

		[HttpGet("getall")]
		public async Task<List<MovieModel>> GetAll()
		{
			var result = await _searchClient.GetAll().ConfigureAwait(false);
			return result;
		}

		[HttpGet("getallbygenres/{genreId}")]
		public async Task<List<MovieModel>> GetAllByGenre(long genreId)
		{
			var result = await _genreClient.GetMoviesByGenre(genreId).ConfigureAwait(false);
			return result;
		}

		[HttpGet("getmostrated")]
		public async Task<List<MovieModel>> GetMostRated()
		{
			var result = await _searchClient.GetMostRated().ConfigureAwait(false);
			return result;
		}

		[HttpGet("search/{query}")]
		public async Task<List<MovieModel>> Search(string query)
		{
			var results = await _searchClient.Get(query).ConfigureAwait(false);
			return results;
		}

		[HttpGet("{id}")]
		public async Task<MovieModel> Get(long id)
		{
			var result = await _movieClient.Get(id).ConfigureAwait(false);
			return result;
		}

		[HttpPost("{id}")]
		public async Task Set(
			[FromRoute] long id, 
			[FromForm] string name,
			[FromForm] string description,
			[FromForm] string img,
			[FromForm] string key,
			[FromForm] string length,
			[FromForm] decimal rate) 
			=> await _movieClient.Set(id, name, description, img, key, length, rate).ConfigureAwait(false);

		[HttpPost("Create")]
		public async Task<MovieModel> Create(
			[FromForm] string name, 
			[FromForm] string description,
			[FromForm] string img,
			[FromForm] string key,
			[FromForm] string length,
			[FromForm] decimal rate
		)
		{
			var movie = new MovieModel()
			{
				Name = name,
				Description = description,
				Img = img,
				Key = key,
				Length = length,
				Rate = rate
			};

			var id = await _referenceDataService.CreateMovieAsync(movie);

			var result = await _movieClient.Get(id).ConfigureAwait(false);
			return result;
		}

	}
}
